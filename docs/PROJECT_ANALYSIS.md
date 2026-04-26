# PROJECT_ANALYSIS

## Purpose

Документ фиксирует фактическое состояние `AquaAPI` по коду на 26 апреля 2026 года и обновляет выводы после прошлого ревью.

## Current State

Репозиторий содержит backend-контур из 5 сервисов и gateway:
- `ApiGateway` с YARP, JWT validation и агрегированным Swagger;
- `IdentityService` с регистрацией, логином, refresh/logout flow, профилем пользователя и Quartz jobs;
- `DeviceService` с CRUD по контроллерам, датчикам и реле, `X-Device-Token`, ping и HTTP telemetry ingress для МК;
- `TelemetryService` с хранением телеметрии, duplicate-check по `ExternalMessageId`, публикацией `TelemetryReceivedEvent` и job проверки отсутствия данных;
- `ControlService` с CRUD по аквариумам и правилам автоматизации, обработкой telemetry/sensor/relay events и schedule job;
- `NotificationService` с отдельной БД, API для уведомлений/напоминаний/журнала обслуживания и consumer-ами user/aquarium/alert events.

Главное изменение после прошлого ревью:
- исправлены runtime-дефекты `IdentityService` в регистрации и refresh response;
- `global.json` перенесен в корень репозитория и закрепляет .NET SDK `8.0.100` с `rollForward: latestFeature`;
- `docker-compose.yml` публикует `device-api` наружу на `localhost:5237`, поэтому появился direct lab mode для МК/эмулятора;
- `RabbitMqOptions__*` приведены в compose к фактическим options-классам сервисов;
- refresh endpoint переведен на DTO (`RefreshTokenRequestDto`);
- refresh token больше не хранится raw string: наружу выдается формат `<tokenId>.<secret>`, а в БД сохраняется `TokenHash`;
- `SensorNoDataAlertEvent` теперь получает `UserId` через `Aquarium.UserId`, а не через `HttpContext`;
- namespace aquarium events в `NotificationService` приведен к `Contracts.Events.AquariumEvents`;
- controller registration теперь возвращает onboarding DTO с `ControllerId` и `DeviceToken`;
- `CreatedAtRoute` в controller registration исправлен на `{ id = response.ControllerId }`;
- direct MCU lab mode и telemetry payload/response задокументированы в `README.md`;
- telemetry ingress получил `FluentValidation`-валидаторы для `MacAddress`, пустого `Items`, `SensorId == Guid.Empty`, `ExternalMessageId` и `RecordedAt`;
- найден новый дефект в telemetry validator: batch-size правило написано как `Count > 50` при сообщении о максимуме 50 элементов.

## Services

### ApiGateway

Реализовано:
- reverse proxy маршруты на `identity`, `telemetry`, `device`, `control`, `notification`;
- JWT Bearer validation;
- policy `Default` для `telemetry`, `device`, `control`, `notification`;
- агрегированный Swagger UI.

Ограничения:
- gateway остается фактической trust boundary;
- telemetry ingress через gateway защищен JWT policy, поэтому физический МК должен либо уметь отправлять JWT, либо ходить напрямую в `device-api`;
- direct lab mode теперь доступен через опубликованный `device-api` port `5237`.

### IdentityService

Реализовано:
- `POST /api/identity/v1/auth/register`;
- `POST /api/identity/v1/auth/login`;
- `POST /api/identity/v1/auth/refresh`;
- `POST /api/identity/v1/auth/logout`;
- `GET /api/identity/v1/profile`;
- `PUT /api/identity/v1/profile/me`;
- `POST /api/identity/v1/profile/password`;
- refresh token entity/repository;
- публикация `UserCreatedEvent` и `UserUpdatedEvent`;
- Quartz jobs для refresh-token cleanup и проверки истекших подписок.

Исправлено после прошлого ревью:
- регистрация больше не обращается к `existingUser!.SubscriptionId`, а берет `user.SubscriptionId`;
- refresh flow возвращает новый refresh token (`newRefreshToken`).

Ограничения:
- refresh token hashing внедрен, но reuse detection пока ограничена проверкой `IsUsed` без отдельной компрометации всей token family;
- `Secure = false` у auth cookie приемлем только для локального HTTP-стенда;
- downgrade просроченной подписки выполняется локально, без downstream event;
- прямой JWT validation в самом сервисе не настроен, несмотря на `UseAuthentication()`.

### DeviceService

Реализовано:
- CRUD/service-операции для `controllers`, `sensors`, `relays`;
- `POST /api/device/v1/controllers/{id}/ping`;
- публичный endpoint `POST /api/device/v1/sensors/telemetry`;
- batch ingress по `MacAddress + X-Device-Token + Items[]`;
- response telemetry ingress: `acceptedCount`, `skippedCount`, `validationErrors`;
- consumer команд смены состояния реле;
- job проверки offline-контроллеров с публикацией `ControllerNotOnlineEvent`.

Ограничения:
- telemetry batch валидирует пустой `Items`, но правило максимального размера сейчас инвертировано: `Must(x => x.Count > 50)` вместо ожидаемого ограничения до 50 элементов;
- telemetry item валидирует пустой/whitespace `ExternalMessageId`, но текст ошибки ошибочно говорит про `SensorId`;
- `Guid.Empty` для `SensorId` выделен отдельной validation error;
- `PingControllerAsync` не публикует событие "controller online", только обновляет `LastSeenAt`;
- прямой JWT validation в сервисе не настроен.

### TelemetryService

Реализовано:
- `GET /api/telemetry/v1/data`;
- `GET /api/telemetry/v1/data/{id}`;
- consumer `TelemetryReportedFromHardwareConsumer`;
- duplicate-check через `ExternalMessageId`;
- unique index на `ExternalMessageId`;
- публикация `TelemetryReceivedEvent`;
- sync локального каталога сенсоров через sensor events;
- `CheckSensorStateJob`, публикующий `SensorNoDataEvent`.

Ограничения:
- публичного `POST /api/telemetry/v1/data` нет, canonical ingress идет через `DeviceService`;
- конкурентный duplicate-case явно не обработан на уровне db-exception/neutral contract;
- прямой JWT validation в сервисе не настроен.

### ControlService

Реализовано:
- `GET/POST/PUT/DELETE /api/control/v1/aquariums`;
- `GET/POST/PUT/DELETE /api/control/v1/automation-rules`;
- consumer-ы telemetry, sensor и relay events;
- публикация `CriticalTelemetryThresholdAlertEvent` и `SensorNoDataAlertEvent`;
- публикация aquarium events;
- `ScheduleProcessJob`.

Исправлено после прошлого ревью:
- `SensorNoDataAlertEvent` теперь заполняет `UserId` через `existingAquarium.UserId`.

Ограничения:
- публичных API для расписаний и vacation mode пока нет;
- `SensorNoDataAlertEvent` публикуется внутри цикла по affected rules, поэтому один sensor-no-data может породить несколько alert events;
- прямой JWT validation в сервисе не настроен.

### NotificationService

Реализовано:
- API для notifications/reminders/maintenance logs;
- отдельная PostgreSQL БД и EF migrations;
- consumer-ы `UserCreatedEvent`, `UserUpdatedEvent`, aquarium events и alert events;
- providers для Telegram/email;
- Quartz jobs `ReminderCheckerJob` и `UnpublishedNoticeProcessorJob`.

Исправлено после прошлого ревью:
- imports aquarium events приведены к `Contracts.Events.AquariumEvents`.

Ограничения:
- сборка сервиса подтверждена через последовательный `dotnet build ... -m:1`;
- обработка уведомлений зависит от тестовых placeholder-настроек Telegram/email;
- прямой JWT validation в сервисе не настроен.

## Cross-Cutting Findings

### Runtime Topology

`docker-compose.yml` поднимает:
- `api-gateway`, `identity-api`, `telemetry-api`, `device-api`, `control-api`, `notification-api`;
- `rabbitmq`;
- `identity-db`, `telemetry-db`, `device-db`, `control-db`, `notification-db`.

Наружу опубликованы gateway (`5055`), `device-api` (`5237`), RabbitMQ и базы. Это закрывает базовый direct HTTP path для МК/эмулятора без JWT, если используется `X-Device-Token`.

### Messaging

Event-driven цепочка архитектурно покрывает:
- user creation/update;
- aquarium lifecycle;
- sensor lifecycle;
- hardware telemetry ingestion;
- telemetry received;
- relay command flow;
- alert dispatch into notification context.

Главные оставшиеся риски:
- нет подтвержденного E2E runtime-smoke;
- нет события о subscription downgrade;
- idempotency telemetry не закрывает конкурентный дубль явно;
- alert-to-notification требует runtime-проверки `NotificationService` в E2E smoke.

### Build/Tooling

Факты на 26 апреля 2026:
- валидный `global.json` теперь лежит в корне и выбирает .NET SDK `8.0.420` через roll-forward от `8.0.100`;
- старый `src/global.json` удален;
- запуск `dotnet` без writable `DOTNET_CLI_HOME` может падать на first-time setup из-за прав;
- воспроизводимая локальная сборка подтверждена для всех 5 API-проектов через последовательный build с `-m:1`;
- `IdentityService` собирается с одним nullable warning в `SubscriptionEntity.Name`;
- параллельный restore/build без `-m:1` может падать без явных compile errors и остается tooling follow-up;
- `docker compose up --build` не подтвержден.
- targeted build `Device.API.csproj` после добавления validators подтвержден: сборка успешна, остались предупреждения `NU1900` по vulnerability data с NuGet.

### Repository Hygiene

В репозитории присутствуют IDE/build артефакты и временные файлы:
- `.vs`, `.idea`;
- `build.log`, `infra-build*.log`, `restore-diag.log`;
- `*.Backup.tmp`;
- локальные `.dotnet*` директории.

## Main Risks

1. Telemetry batch-size validation сейчас ошибочно требует `Count > 50` при заявленном максимуме 50 элементов.
2. Direct MCU lab mode задокументирован, но не подтвержден E2E smoke-ом.
3. Refresh token hashing внедрен, но reuse/family revocation policy еще не production-grade.
4. Auth cookie создается с `Secure = false`, что допустимо только локально.
5. Notification/alert chain не подтверждена фактическим E2E smoke.
6. `docker compose up --build` еще не подтвержден.
7. Репозиторий загрязнен временными и IDE/build артефактами.

## Summary

Проект стал ближе к первому hardware smoke: исправлен SDK pinning, опубликован direct `device-api`, telemetry ingress существует, device token возвращается в response body, refresh token flow стал безопаснее, alert `UserId` в sensor-no-data ветке исправлен.

Текущий статус: `почти готов к первому HTTP-smoke с эмулятором/МК по direct device-api`, но `еще не готов как полностью воспроизводимый стенд`.

Первый следующий фокус:
- исправить batch-size правило в `TelemetryBatchRequestValidator`;
- пройти `docker compose up --build` и E2E smoke.
