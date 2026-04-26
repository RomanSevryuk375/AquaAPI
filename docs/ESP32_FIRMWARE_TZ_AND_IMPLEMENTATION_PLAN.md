# ESP32 Firmware TZ and Implementation Plan

## Document Purpose

Этот документ описывает техническое задание и план реализации прошивки для микроконтроллера ESP32, который подключается к backend `AquaAPI`.

Документ специально структурирован так, чтобы его было удобно использовать нейросетям и coding agents:
- требования разбиты на атомарные пункты;
- аппаратные модули перечислены явно;
- состояния, события, команды и контракты описаны отдельно;
- план реализации разделен на независимые firmware-модули;
- критерии приемки сформулированы проверяемо.

## Target Hardware

Обязательное железо:
- MCU: `ESP32`.
- Relay module: восьмиканальный модуль реле, `8 channels`.
- Display 1: `LCD1602`.
- RTC: `DS1307`.
- Display 2: четырехразрядный семисегментный индикатор `5641AS`.
- Indicators: светодиоды разных цветов для статуса.
- Navigation input: потенциометры `10 kOhm` или кнопки.

Потенциальные датчики:
- температура воды;
- температура поверхности;
- уровень воды;
- мутность воды;
- влажность;
- дополнительные датчики pH/kH/NO3, если будут добавлены позже.

## Backend Integration Summary

Текущий recommended ingress:

```http
POST http://localhost:5237/api/device/v1/sensors/telemetry
Header: X-Device-Token: <controller-device-token>
Content-Type: application/json
```

Payload:

```json
{
  "macAddress": "AA:BB:CC:DD:EE:FF",
  "items": [
    {
      "sensorId": "a3c2f9c1-1111-2222-3333-444444444444",
      "value": 25.4,
      "externalMessageId": "esp32-000001-temp-water",
      "recordedAt": "2026-04-26T09:00:00Z"
    }
  ]
}
```

Expected response:

```json
{
  "acceptedCount": 1,
  "validationErrors": [],
  "skippedCount": 0
}
```

Important backend assumptions:
- `macAddress` identifies the controller.
- `X-Device-Token` authenticates the controller.
- each telemetry item must reference a registered backend `sensorId`;
- `externalMessageId` must be unique and stable for idempotency;
- first lab mode should use direct `device-api` on port `5237`;
- gateway mode requires JWT and is not recommended for first MCU smoke.

Known backend blocker before stable MCU test:
- `TelemetryBatchRequestValidator` currently has an inverted batch-size rule: it requires `items.Count > 50`, while the expected contract is `1..50`.

## Firmware Goals

Primary goal:
- ESP32 must collect sensor values, control 8 relay channels, show local status on LCD1602/5641AS/LEDs, keep local time using DS1307, and send telemetry batches to `AquaAPI`.

Secondary goals:
- tolerate temporary Wi-Fi/backend failures;
- preserve relay safety state after reboot;
- provide local navigation/settings with buttons or potentiometers;
- keep firmware modular enough for later migration to MQTT/gRPC/streaming if backend evolves.

Non-goals for first firmware version:
- OTA updates;
- encrypted local storage;
- complex UI menus;
- full offline automation engine;
- Wi-Fi provisioning portal;
- gateway JWT flow.

## Firmware Architecture

Recommended language/framework:
- Arduino framework for ESP32 or ESP-IDF.

Recommended first implementation:
- Arduino framework if development speed is more important.
- ESP-IDF if long-term production quality is more important.

Core modules:
- `ConfigStore`: stores controller settings.
- `WiFiManager`: connects and reconnects Wi-Fi.
- `RtcClock`: reads and writes time using DS1307.
- `SensorManager`: polls physical sensors.
- `RelayManager`: controls 8 relay channels.
- `TelemetryQueue`: buffers telemetry before send.
- `ApiClient`: sends HTTP telemetry batches to AquaAPI.
- `DisplayManager`: controls LCD1602 and 5641AS.
- `LedIndicator`: controls status LEDs.
- `InputManager`: reads buttons or potentiometers.
- `UiController`: local menu/state screen logic.
- `Scheduler`: periodic task timing.
- `Diagnostics`: logs boot, network, API, sensor and relay status.

## Configuration Model

Required firmware config:

```json
{
  "device": {
    "macAddress": "AA:BB:CC:DD:EE:FF",
    "controllerId": "backend-controller-guid",
    "deviceToken": "backend-device-token",
    "apiBaseUrl": "http://192.168.1.100:5237",
    "telemetryPath": "/api/device/v1/sensors/telemetry"
  },
  "wifi": {
    "ssid": "wifi-name",
    "password": "wifi-password"
  },
  "telemetry": {
    "sendIntervalMs": 10000,
    "maxBatchSize": 50,
    "maxQueueSize": 500
  },
  "relaySafeState": {
    "channel1": false,
    "channel2": false,
    "channel3": false,
    "channel4": false,
    "channel5": false,
    "channel6": false,
    "channel7": false,
    "channel8": false
  }
}
```

Sensor mapping config:

```json
[
  {
    "localKey": "water_temperature",
    "sensorId": "backend-sensor-guid",
    "pin": 32,
    "unit": "C",
    "pollIntervalMs": 5000,
    "enabled": true
  }
]
```

Relay mapping config:

```json
[
  {
    "channel": 1,
    "relayId": "backend-relay-guid",
    "pin": 16,
    "purpose": "filter",
    "activeLow": true,
    "safeState": false
  }
]
```

## Pin Planning

Create a dedicated pin map before coding.

Pin map requirements:
- avoid ESP32 boot-strapping pins for relay outputs where unsafe startup toggles are possible;
- keep I2C pins shared by LCD1602 I2C adapter and DS1307 if both use I2C;
- reserve ADC-capable pins for potentiometers and analog sensors;
- reserve enough GPIO for 5641AS digit and segment control if using direct multiplexing;
- prefer a driver chip for 5641AS if GPIO count becomes too high.

Recommended abstractions:
- never hardcode pins inside business logic;
- define all pins in `PinMap.h` or `pins_config.cpp`;
- each hardware module receives pins through constructor/config.

## Runtime State Machine

Firmware states:

```text
BOOT
LOAD_CONFIG
INIT_HARDWARE
CONNECT_WIFI
SYNC_TIME
ONLINE
DEGRADED_OFFLINE
SAFE_MODE
FACTORY_RESET
```

State definitions:
- `BOOT`: CPU started, no hardware assumption yet.
- `LOAD_CONFIG`: read config from flash/NVS.
- `INIT_HARDWARE`: initialize GPIO, relay safe states, displays, RTC, sensors.
- `CONNECT_WIFI`: attempt Wi-Fi connection.
- `SYNC_TIME`: prefer NTP if online, fallback to DS1307.
- `ONLINE`: telemetry can be sent to backend.
- `DEGRADED_OFFLINE`: sensors and local display work, telemetry is queued.
- `SAFE_MODE`: relays forced to safe states after critical error.
- `FACTORY_RESET`: clear config after explicit local action.

State transition rules:
- `BOOT -> LOAD_CONFIG` always.
- `LOAD_CONFIG -> INIT_HARDWARE` if config valid.
- `LOAD_CONFIG -> FACTORY_RESET` if config missing and reset requested.
- `INIT_HARDWARE -> CONNECT_WIFI` after hardware init.
- `CONNECT_WIFI -> SYNC_TIME` if Wi-Fi connected.
- `CONNECT_WIFI -> DEGRADED_OFFLINE` after retry timeout.
- `SYNC_TIME -> ONLINE` when time source is valid.
- `ONLINE -> DEGRADED_OFFLINE` on repeated API/Wi-Fi failures.
- `DEGRADED_OFFLINE -> ONLINE` after Wi-Fi and API health recover.
- any state -> `SAFE_MODE` on critical hardware failure.

## Relay Control Requirements

Relay module:
- 8 channels.
- Each channel has configured GPIO pin.
- Each channel supports `activeLow`.
- Each channel has `safeState`.
- Startup must apply safe states before network operations.

Relay purposes:
- channel 1: filter;
- channel 2: filter reserve or second filter;
- channel 3: heater;
- channel 4: pump;
- channel 5: lamp;
- channel 6: light;
- channel 7: reserve;
- channel 8: reserve.

Relay commands for first firmware:
- local manual toggle from UI;
- apply configured safe state;
- future: poll backend for desired state or receive command channel.

Important gap:
- current backend event flow can publish relay commands internally, but first ESP32 HTTP plan only sends telemetry.
- firmware needs a command retrieval mechanism before backend-driven relay changes can reach physical relays.

Recommended command retrieval for first version:
- add a future backend endpoint like `GET /api/device/v1/controllers/{id}/commands/pending`;
- ESP32 polls commands every `2..5 seconds`;
- command includes `relayId`, `desiredState`, `mode`, `commandId`;
- ESP32 acknowledges command execution.

## Telemetry Requirements

Telemetry item fields:
- `sensorId`: backend GUID.
- `value`: numeric sensor value.
- `externalMessageId`: unique message id.
- `recordedAt`: ISO-8601 UTC timestamp.

`externalMessageId` format:

```text
<mac-without-separators>-<bootCounter>-<sequence>-<localSensorKey>
```

Example:

```text
AABBCCDDEEFF-0007-00000042-water_temperature
```

Telemetry batching:
- collect readings into queue;
- send every `sendIntervalMs`;
- max batch size: `50`;
- if offline, keep queue until `maxQueueSize`;
- if queue full, drop oldest non-critical readings first;
- never generate duplicate `externalMessageId` for a new reading;
- retry same queued reading with same `externalMessageId`.

Timestamp rules:
- use NTP when Wi-Fi is available;
- use DS1307 as fallback source;
- if time is invalid, mark device degraded and avoid sending telemetry until time is usable;
- all backend timestamps should be UTC.

## Display Requirements

LCD1602:
- default screen: current status.
- show Wi-Fi state.
- show API state.
- show current selected sensor value.
- show relay state page.
- show error page if API validation fails.

Example LCD screens:

```text
AquaAPI ONLINE
Twater 25.4 C
```

```text
WiFi OFFLINE
Queue 024/500
```

```text
Relay 1 FILTER
State ON
```

5641AS 4-digit indicator:
- default: show primary temperature multiplied or rounded as needed.
- alternate mode: show time `HHMM`.
- error mode: show short numeric/status code.

Suggested 5641AS modes:
- `TEMP`: `25.4` if decimal point wiring is available;
- `TIME`: `1345`;
- `ERR`: `E001`, `E002`, etc.;
- `RELAY`: `r001` style if letters are supported enough.

LED indicators:
- green: online/API OK;
- blue: Wi-Fi connected;
- yellow: degraded/offline queue active;
- red: critical error/safe mode;
- white or custom: relay activity pulse.

## Input Requirements

Supported navigation options:
- buttons;
- 10 kOhm potentiometers.

Buttons recommended mapping:
- `UP`;
- `DOWN`;
- `OK`;
- `BACK`;
- optional `MODE`.

Potentiometer recommended use:
- one potentiometer as menu selector;
- one potentiometer as value adjuster;
- button confirms selection.

Input processing:
- debounce buttons;
- smooth potentiometer ADC readings;
- convert ADC ranges to discrete menu indices;
- prevent accidental factory reset with long-press confirmation.

## Error Codes

Firmware should have stable short error codes.

Suggested codes:
- `E001`: config missing or invalid.
- `E002`: Wi-Fi connection failed.
- `E003`: RTC unavailable.
- `E004`: time invalid.
- `E005`: backend unavailable.
- `E006`: backend rejected telemetry validation.
- `E007`: sensor read failed.
- `E008`: relay hardware init failed.
- `E009`: telemetry queue overflow.
- `E010`: device token missing.

Each error should be:
- logged to serial;
- visible on LCD1602;
- optionally visible on 5641AS;
- mapped to LED behavior.

## Safety Requirements

Startup safety:
- initialize relay GPIO as output immediately;
- set all relays to configured safe states before Wi-Fi;
- do not toggle relays during display/RTC init.

Offline safety:
- continue local sensor reading;
- keep relays in last safe/local state unless critical sensor condition requires safe state;
- queue telemetry;
- show offline status.

Critical safety:
- if heater sensor is unavailable, turn heater relay off;
- if water level sensor indicates unsafe low level, turn pump/heater off;
- if firmware detects invalid config, enter safe mode;
- if relay control pin init fails, enter safe mode.

## Implementation Plan

### Phase 0: Firmware Project Skeleton

Tasks:
1. Choose Arduino framework or ESP-IDF.
2. Create project structure.
3. Add modules:
   - `PinMap`;
   - `ConfigStore`;
   - `RelayManager`;
   - `RtcClock`;
   - `DisplayManager`;
   - `InputManager`;
   - `SensorManager`;
   - `TelemetryQueue`;
   - `ApiClient`;
   - `Diagnostics`.
4. Add serial logging.
5. Add compile-time feature flags.

Acceptance:
- firmware compiles;
- serial boot log prints firmware version;
- relays initialize to safe state.

### Phase 1: Hardware Bring-Up

Tasks:
1. Test relay channel 1..8 manually.
2. Test LCD1602 output.
3. Test DS1307 read/write.
4. Test 5641AS display mode.
5. Test LEDs.
6. Test buttons or potentiometers.

Acceptance:
- each hardware module has a standalone diagnostic screen/log.
- no relay toggles unexpectedly during boot.

### Phase 2: Config and UI

Tasks:
1. Store Wi-Fi credentials.
2. Store `apiBaseUrl`.
3. Store `macAddress`.
4. Store `deviceToken`.
5. Store sensor mapping.
6. Store relay mapping.
7. Implement LCD status screens.
8. Implement basic navigation.

Acceptance:
- config survives reboot;
- user can view Wi-Fi/API/sensor/relay status locally.

### Phase 3: Wi-Fi and Time

Tasks:
1. Connect to Wi-Fi.
2. Reconnect after disconnect.
3. Sync time from NTP.
4. Write synced time into DS1307.
5. Use DS1307 fallback when offline.

Acceptance:
- valid UTC timestamp is available before telemetry send.
- device enters `DEGRADED_OFFLINE` if Wi-Fi is unavailable.

### Phase 4: Sensor Polling

Tasks:
1. Implement sensor abstraction.
2. Poll sensors by configured interval.
3. Validate sensor readings.
4. Push readings into telemetry queue.
5. Show primary reading on LCD1602 and 5641AS.

Acceptance:
- readings are stable in serial logs;
- invalid readings produce `E007`;
- telemetry queue receives one item per configured sensor interval.

### Phase 5: Telemetry HTTP Integration

Tasks:
1. Implement `ApiClient`.
2. Add `X-Device-Token` header.
3. Build JSON payload.
4. Send up to 50 items per request.
5. Parse `acceptedCount`, `skippedCount`, `validationErrors`.
6. On success, remove accepted items from queue.
7. On network failure, keep queue.
8. On validation failure, show `E006` and log backend message.

Acceptance:
- ESP32 can send telemetry to direct `device-api`.
- backend returns `202 Accepted`.
- duplicate retry uses same `externalMessageId`.

### Phase 6: Relay Local Control

Tasks:
1. Implement relay state model.
2. Implement manual local toggle from UI.
3. Show relay state on LCD1602.
4. Persist last selected relay mode if needed.
5. Add safety overrides.

Acceptance:
- each relay can be toggled locally.
- safe mode forces configured safe states.

### Phase 7: Backend Command Flow

Tasks:
1. Define backend command polling endpoint.
2. Implement command poll in firmware.
3. Apply relay command.
4. Acknowledge command result.
5. Add idempotency by `commandId`.

Acceptance:
- backend can change physical relay state.
- repeated command does not double-apply unsafe operation.

### Phase 8: Long-Run Smoke

Tasks:
1. Run firmware for 8 hours.
2. Disconnect Wi-Fi for 10 minutes.
3. Restore Wi-Fi and verify queue flush.
4. Reboot ESP32 and verify safe relay states.
5. Verify DS1307 fallback time.
6. Verify backend telemetry history.

Acceptance:
- no crash/reboot loop;
- no unexpected relay toggle;
- telemetry gap and recovery are explainable in logs.

## Suggested Firmware File Structure

```text
firmware/
  platformio.ini
  src/
    main.cpp
    PinMap.h
    AppState.h
    ConfigStore.h
    ConfigStore.cpp
    WiFiManagerEx.h
    WiFiManagerEx.cpp
    RtcClock.h
    RtcClock.cpp
    RelayManager.h
    RelayManager.cpp
    SensorManager.h
    SensorManager.cpp
    TelemetryQueue.h
    TelemetryQueue.cpp
    ApiClient.h
    ApiClient.cpp
    DisplayManager.h
    DisplayManager.cpp
    LedIndicator.h
    LedIndicator.cpp
    InputManager.h
    InputManager.cpp
    UiController.h
    UiController.cpp
    Diagnostics.h
    Diagnostics.cpp
  test/
    test_telemetry_queue.cpp
    test_external_message_id.cpp
```

## AI Agent Implementation Rules

When generating firmware code, follow these rules:
- keep hardware access isolated behind modules;
- no business logic directly inside `loop()`;
- `loop()` should call scheduler/task methods only;
- all pins must come from `PinMap`;
- all backend URLs and tokens must come from config;
- do not hardcode backend `sensorId` values in source code;
- generate stable `externalMessageId`;
- use non-blocking timing with `millis()` where possible;
- avoid long `delay()` calls except short display timing;
- relay safe state must be applied before Wi-Fi connection;
- serial logs must include state transitions and API responses;
- telemetry queue must survive temporary network errors;
- every API validation error should be visible in logs and UI.

## Backend Tasks Needed For Full MCU Support

Required before stable relay control from backend:
- fix telemetry batch-size validator to accept `1..50`;
- add or document relay command retrieval endpoint for ESP32;
- add command acknowledgement endpoint;
- provide a device-facing config endpoint or documented manual provisioning process;
- run `docker compose up --build`;
- run E2E smoke with ESP32 or emulator.

Optional later:
- MQTT command channel;
- SignalR/gRPC streaming channel;
- OTA firmware update service;
- signed firmware/config;
- encrypted secrets at rest.

## First MCU Smoke Checklist

Backend checklist:
- `device-api` reachable from ESP32 network as `http://<host-ip>:5237`;
- controller exists in backend;
- `DeviceToken` copied into firmware config;
- sensors exist in backend;
- backend `sensorId` values copied into firmware config;
- telemetry batch-size validator fixed;
- RabbitMQ and service databases running.

Firmware checklist:
- ESP32 connects to Wi-Fi;
- DS1307 returns valid time or NTP sync works;
- relays start in safe state;
- LCD1602 shows `ONLINE` or `OFFLINE`;
- 5641AS shows primary value/time/error;
- LEDs reflect state;
- telemetry queue receives readings;
- HTTP request includes `X-Device-Token`;
- backend returns `202 Accepted`.

Pass criteria:
- at least one telemetry item is saved in `TelemetryService`;
- backend response has `acceptedCount > 0`;
- firmware logs `ONLINE`;
- LCD1602 shows no critical error;
- no relay toggles unexpectedly.
