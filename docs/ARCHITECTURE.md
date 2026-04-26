# ARCHITECTURE

## Topology

- `ApiGateway`: единая точка входа, JWT validation, reverse proxy, агрегированный Swagger UI.
- `IdentityService`: регистрация, логин, refresh/logout, профиль пользователя, выпуск JWT, refresh tokens, Quartz jobs, user events.
- `DeviceService`: контроллеры, датчики, реле, heartbeat, HTTP ingress телеметрии от железа, обработка relay-команд.
- `TelemetryService`: хранение телеметрии, duplicate-check по `ExternalMessageId`, публикация `TelemetryReceivedEvent`, контроль задержки данных датчиков.
- `ControlService`: аквариумы, правила автоматизации, schedule processing, реакция на telemetry/device events.
- `NotificationService`: уведомления, напоминания, maintenance log, sync пользовательских и aquarium-данных из событий.

## Current Runtime (As Is)

### Gateway + Auth

- `ApiGateway` проксирует `identity`, `telemetry`, `device`, `control`, `notification`.
- JWT проверяется только на gateway.
- Маршруты `/api/telemetry/*`, `/api/device/*`, `/api/control/*`, `/api/notification/*` защищены policy `Default`.
- `/api/identity/*` доступен для auth flow; profile/logout требуют авторизацию на controller level.

Практический вывод для МК:
- gateway path требует JWT и `X-Device-Token`;
- direct lab path уже опубликован через `device-api` на `localhost:5237`;
- первый hardware smoke проще вести напрямую в `device-api`, а gateway path оставить для user-facing clients.

### Identity Flow

Endpoint-ы:
- `POST /api/identity/v1/auth/register`
- `POST /api/identity/v1/auth/login`
- `POST /api/identity/v1/auth/refresh`
- `POST /api/identity/v1/auth/logout`
- `GET /api/identity/v1/profile`
- `PUT /api/identity/v1/profile/me`
- `POST /api/identity/v1/profile/password`

Фактический runtime:
- после регистрации публикуется `UserCreatedEvent`;
- после изменения профиля публикуется `UserUpdatedEvent`;
- access token генерируется через `JwtProvider`;
- refresh token хранится в таблице `refresh_tokens`;
- cleanup и subscription-check выполняются Quartz jobs.

Текущий статус:
- register и refresh response regression исправлены;
- refresh endpoint принимает DTO `RefreshTokenRequestDto`;
- refresh token выдается как `<tokenId>.<secret>`, а в БД хранится `TokenHash`;
- access token дополнительно кладется в `jwt` cookie с `HttpOnly`, но `Secure = false` подходит только для локального HTTP;
- subscription downgrade не публикует downstream event.

### Device + Hardware Ingress Flow

Canonical hardware ingress находится в `DeviceService`.

Endpoint:
- `POST /api/device/v1/sensors/telemetry`
- Header: `X-Device-Token: <controller-device-token>`

Payload:
```json
{
  "macAddress": "AA:BB:CC:DD:EE:FF",
  "items": [
    {
      "sensorId": "00000000-0000-0000-0000-000000000000",
      "value": 25.4,
      "externalMessageId": "controller-001-42",
      "recordedAt": "2026-04-26T09:00:00Z"
    }
  ]
}
```

Flow:
1. `SensorService.ProcessTelemetryBatchAsync` находит controller по `MacAddress`.
2. `X-Device-Token` сверяется с `DeviceTokenHash`.
3. Каждый item сверяется с датчиками контроллера.
4. Для валидных items публикуется `TelemetryReportedFromHardwareEvent`.
5. Response: `202 Accepted` с `acceptedCount`, `skippedCount`, `validationErrors`.

Ограничения:
- пустой `Items` валидируется через `TelemetryBatchRequestValidator`;
- пустой/whitespace `ExternalMessageId` валидируется, но текст ошибки сейчас ошибочно говорит про `SensorId`;
- `Guid.Empty` для `SensorId` выделен отдельной validation error;
- правило максимального размера batch сейчас инвертировано: код требует `Count > 50`, хотя сообщение говорит про максимум 50 элементов;
- registration action возвращает `DeviceToken` в body, а `CreatedAtRoute` уже передает `{ id = response.ControllerId }`.

### Telemetry Flow

- Публичный API `TelemetryService` read-only:
  - `GET /api/telemetry/v1/data`
  - `GET /api/telemetry/v1/data/{id}`
- Ingress идет через RabbitMQ event `TelemetryReportedFromHardwareEvent`.
- Идемпотентность: pre-check по `ExternalMessageId` + unique index.
- После сохранения публикуется `TelemetryReceivedEvent`.
- `CheckSensorStateJob` публикует `SensorNoDataEvent`.

Ограничение:
- конкурентный duplicate-case требует явной обработки unique-index violation.

### Control Flow

- `ControlService` consume-ит telemetry, sensor и relay events.
- API покрывает `aquariums` и `automation-rules`.
- Публикует relay commands и alert events.
- `CriticalTelemetryThresholdAlertEvent` получает `UserId` через `aquarium.UserId`.
- `SensorNoDataAlertEvent` также получает `UserId` через `existingAquarium.UserId`.

Ограничения:
- публичных API для schedule/vacation mode пока нет;
- sensor no-data alert может публиковаться по каждому affected rule.

### Notification Flow

- API:
  - `/api/notification/v1/notifications`
  - `/api/notification/v1/reminders`
  - `/api/notification/v1/maintenance-logs`
- Consumers:
  - `UserCreatedEvent`
  - `UserUpdatedEvent`
  - aquarium lifecycle events
  - `CriticalTelemetryThresholdAlertEvent`
  - `SensorNoDataAlertEvent`
  - `ControllerNotOnlineEvent`
- Providers:
  - Telegram
  - Email

Текущий статус:
- namespace mismatch по aquarium events в исходниках уже исправлен;
- build подтвержден через последовательный `dotnet build ... -m:1`;
- runtime smoke для alert-to-notification еще не выполнен;
- внешние provider-настройки остаются тестовыми.

## Cross-Cutting

### Databases

Отдельная PostgreSQL БД на сервис:
- `identity_db`
- `telemetry_db`
- `device_db`
- `control_db`
- `notification_db`

### Messaging

RabbitMQ + MassTransit используются для:
- user sync;
- aquarium sync;
- sensor sync;
- telemetry ingestion;
- telemetry received;
- relay command flow;
- alerts.

### Security Boundary

- Реальная авторизация централизована на gateway.
- Downstream-сервисы вызывают `UseAuthentication()`, но локальный JWT scheme не регистрируют.
- Такой режим допустим только при явной gateway-only trust boundary.
- Для direct MCU access к `device-api` нужно отдельно учитывать, что endpoint обходит gateway JWT layer и опирается на `X-Device-Token`.

### Build Boundary

- Корневой `global.json` валиден и выбирает .NET 8 SDK через `8.0.100` + `rollForward: latestFeature`.
- В текущей среде `dotnet --info` выбирает SDK `8.0.420`.
- `dotnet build` подтвержден для всех 5 API-проектов через последовательный режим `-m:1`.
- Параллельный restore/build без `-m:1` может падать без явных compile errors и не считается рабочим режимом сборки.

## Planned Architecture Extensions

### `planned`: CQRS + MediatR

Цель:
- разнести write/read use cases по handler-ам;
- добавить pipeline behaviors для валидации, логирования и idempotency.

Первая зона:
- `ControlService`, затем `TelemetryService`.

### `planned`: SignalR

Цель:
- live dashboard без polling;
- push telemetry/alert/offline events в UI.

Рекомендуемая точка:
- `ApiGateway` или отдельный realtime edge-сервис.

### `planned`: gRPC

Цель:
- быстрые synchronous internal query-сценарии.

Use case-ы:
- проверка лимитов подписки;
- синхронная проверка конфигурации контроллера.

### `planned`: gRPC streaming

Цель:
- долгоживущий канал с эмулятором/контроллером;
- push команд, конфигурации и потенциально OTA.

## Architecture Gaps

1. Telemetry ingress validation содержит дефект batch-size правила: `Count > 50` вместо ожидаемого `Count <= 50`.
2. Direct MCU lab mode задокументирован, но E2E smoke еще не пройден.
3. Telemetry idempotency не закрывает конкурентный дубль явно.
4. Alert-to-notification E2E не подтвержден.
5. `docker compose up --build` не подтвержден.
6. Refresh token flow требует reuse/family revocation policy.

## Current End-to-End Scenario

1. Клиент получает JWT через `IdentityService`.
2. Клиент работает через `ApiGateway`.
3. Создаются controller/sensor/relay сущности в `DeviceService`.
4. Создаются aquarium/rule сущности в `ControlService`.
5. МК отправляет batch телеметрию в `POST /api/device/v1/sensors/telemetry`.
6. `DeviceService` публикует `TelemetryReportedFromHardwareEvent`.
7. `TelemetryService` сохраняет данные и публикует `TelemetryReceivedEvent`.
8. `ControlService` вычисляет правило и отправляет relay-команду.
9. `DeviceService` применяет команду.
10. `NotificationService` должен обработать alert-событие и создать/отправить уведомление.

## MCU Test Readiness

Оценка на 26 апреля 2026: `почти готово к первому HTTP smoke с эмулятором/МК по direct device-api`, но `не готово как полностью воспроизводимый стенд`.

Готово:
- есть HTTP telemetry endpoint;
- есть `X-Device-Token`;
- есть batch payload;
- `device-api` опубликован на `localhost:5237`;
- registration response возвращает `DeviceToken` в body;
- `CreatedAtRoute` onboarding endpoint-а исправлен;
- пустой `Items` и `Guid.Empty` sensor id валидируются;
- есть RabbitMQ flow в сторону telemetry/control;
- исправлена часть старых event/user ownership дефектов.

Не готово:
- gateway route требует JWT;
- compose/E2E smoke не подтверждены;
- batch-size validation сейчас ошибочно отклоняет нормальные пачки до 50 элементов;
- E2E alert flow не проверен.
