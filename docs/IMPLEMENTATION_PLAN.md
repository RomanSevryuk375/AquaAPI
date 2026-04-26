# IMPLEMENTATION_PLAN

## Goal

Довести проект до воспроизводимого E2E сценария для первого подключения МК/эмулятора: auth, device onboarding, telemetry ingress, сохранение телеметрии, реакция control rules и alert/notification flow.

## Baseline (26.04.2026)

Уже реализовано:
- gateway routing + JWT validation;
- `IdentityService` с register/login/refresh/logout/profile;
- исправленные register и refresh runtime-баги;
- refresh request DTO и хранение refresh token через hash;
- `DeviceService` с controller/sensor/relay CRUD, ping и telemetry ingress;
- `device-api` опубликован наружу на `localhost:5237` для direct lab mode;
- `X-Device-Token` для ping/telemetry;
- controller registration возвращает `ControllerId` и `DeviceToken` в body;
- `CreatedAtRoute` в controller registration использует корректный route value `{ id = response.ControllerId }`;
- batch telemetry response с `acceptedCount`, `skippedCount`, `validationErrors`;
- telemetry ingress валидирует пустой `Items`;
- telemetry item валидирует `Guid.Empty` для `SensorId`;
- `TelemetryService` с сохранением telemetry, duplicate-check и sensor no-data job;
- `ControlService` с automation rules, relay commands и alert events;
- исправленный `UserId` в `SensorNoDataAlertEvent`;
- `NotificationService` с реальными API/controllers/consumers/jobs;
- отдельная PostgreSQL БД на сервис;
- RabbitMQ/MassTransit integration.

Не завершено:
- правило максимального размера telemetry batch реализовано с ошибкой: сейчас `Must(x => x.Count > 50)` конфликтует с текстом `"Maximum batch size is 50 items."`;
- `ExternalMessageId` валидируется через `NotEmpty`, но текст ошибки ошибочно говорит про `SensorId`;
- refresh flow требует reuse/family revocation policy;
- alert-to-notification E2E не подтвержден runtime smoke;
- `docker compose up --build` не подтвержден.

## Phase Plan

### Phase 1: Restore Build Reproducibility

Статус: `done`.

Задачи:
1. Корневой `global.json` зафиксирован как единственный SDK pin.
2. Старый `src/global.json` удален.
3. Рабочий режим сборки определен: последовательный build с `-m:1`.
4. Все API-проекты собраны.
5. `NotificationService` собран отдельно.

Выход фазы:
- команда имеет один воспроизводимый способ сборки и понимает, какой SDK используется.

Проверенная команда:

```powershell
$env:DOTNET_CLI_HOME="C:\Work Space\AquaAPI\.dotnet-home"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE="1"
dotnet build <API.csproj> -m:1 -v:minimal /p:MSBuildEnableWorkloadResolver=false
```

Примечание:
- параллельный restore/build без `-m:1` может падать без явных compile errors;
- это перенесено в tooling follow-up и не блокирует дальнейшие фазы.

### Phase 2: Prepare MCU Access Path

Статус: `done`.

Задачи:
1. Закрепить direct lab mode: `http://localhost:5237`.
2. Описать gateway mode как защищенный клиентский путь: JWT + `X-Device-Token`.
3. Зафиксировать URL, headers и пример payload.

Выход фазы:
- МК/эмулятор может физически достучаться до telemetry ingress.

### Phase 3: Fix Device Onboarding

Статус: `done`.

Задачи:
1. Исправить route value в `CreatedAtRoute`: выполнено, используется `{ id = response.ControllerId }`.
2. Обновить README/API examples при изменении контракта.

Выход фазы:
- оператор или клиент получает `ControllerId` и `DeviceToken` сразу после регистрации контроллера.

### Phase 4: Harden Telemetry Ingress

Статус: `partially done`.

Задачи:
1. Валидировать пустой batch: выполнено через `TelemetryBatchRequestValidator`.
2. Проверить, должен ли `SensorId == Guid.Empty` считаться validation error: выполнено через `TelemetryItemRequestValidator`.
3. Добавить понятные `validationErrors`: выполнено частично.
4. Описать оставшиеся правила валидации для МК/эмулятора.

Уже сделано:
- `ExternalMessageId` валидируется через `TelemetryItemRequestValidator`.
- Direct lab endpoint и payload описаны в `README.md`.
- `DeviceService` собирается после добавления `FluentValidation`.

Осталось:
- исправить batch-size правило на ожидаемое `Count <= 50`;
- исправить текст ошибки для `ExternalMessageId`;
- при необходимости добавить тесты/ручной smoke для случаев `items: []`, `sensorId: Guid.Empty`, batch > 50.

Выход фазы:
- первый hardware smoke становится диагностически понятным.

### Phase 5: Verify Event Chain

Статус: `pending`.

Задачи:
1. Поднять `docker compose up --build`.
2. Пройти сценарий:
   - register/login;
   - create controller/sensor/relay;
   - create aquarium/rule;
   - submit telemetry batch;
   - verify telemetry saved;
   - verify relay command/state reaction;
   - verify alert notification record.
3. Зафиксировать known gaps.

Выход фазы:
- есть подтвержденный E2E smoke, а не только code inspection.

### Phase 6: Security Hardening

Статус: `pending`.

Задачи:
1. Описать текущий формат `<tokenId>.<secret>`.
2. Добавить rotation/reuse family policy.
3. Настроить production cookie settings (`Secure = true` под HTTPS).
4. Subscription downgrade event.

Выход фазы:
- auth-контур становится ближе к production-grade.

### Phase 7: Planned Architecture Evolution

Статусы:
- `planned`: `CQRS + MediatR` для `ControlService`, затем `TelemetryService`;
- `planned`: `SignalR` для live dashboard;
- `planned`: `gRPC` для внутренних synchronous query-сценариев;
- `planned`: `gRPC streaming` для эмулятора/контроллера.

Порог входа:
- завершен первый воспроизводимый hardware/E2E smoke.

## Definition of Done

1. `dotnet build` проходит для всех API-проектов согласованным способом.
2. `docker compose up --build` поднимает gateway и все сервисы.
3. Direct MCU lab mode документирован.
4. Controller registration возвращает `ControllerId` и `DeviceToken`, а `Location` корректен.
5. Telemetry ingress принимает batch и возвращает понятную диагностику, включая корректный лимит batch size.
6. `TelemetryReportedFromHardwareEvent -> TelemetryReceivedEvent -> ControlService` подтвержден.
7. Relay command flow подтвержден.
8. Alert-to-notification flow подтвержден.
9. Оставшиеся roadmap-элементы явно помечены как planned, а не как реализованные.
