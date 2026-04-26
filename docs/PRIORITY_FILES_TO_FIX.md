# PRIORITY_FILES_TO_FIX

## Tier 1: Immediate Blockers

### 1. Telemetry Ingress Validator Regression

Файлы:
- `src/Services/DeviceService/Device.Application/DTOs/Validators/TelemetryBatchRequestValidator.cs`
- `src/Services/DeviceService/Device.Application/DTOs/Validators/TelemetryItemRequestValidator.cs`
- `src/Services/DeviceService/Device.Application/Services/SensorService.cs`
- `README.md`
- `docs/ARCHITECTURE.md`

Что сделать:
- исправить batch-size правило: сейчас `Must(x => x.Count > 50)` конфликтует с сообщением `"Maximum batch size is 50 items."`;
- ожидаемое поведение для первого smoke: принимать `1..50` элементов и отклонять `> 50`;
- исправить текст ошибки для `ExternalMessageId`, сейчас он говорит `"SensorId must not be empty."`;
- сохранить диагностический response `acceptedCount/skippedCount/validationErrors`.

Почему это blocker:
- текущая валидация может отклонять нормальные telemetry batch-и от МК/эмулятора.

### 2. E2E Smoke and Runtime Chain Verification

Файлы:
- `docker-compose.yml`
- `src/Services/TelemetryService/Telemetry.Infrastructure/Messaging/TelemetryReportedFromHardwareConsumer.cs`
- `src/Services/ControlService/ControlService.Application/Services/TelemetryServiceFromEvent.cs`
- `src/Services/NotificationService/Notification.Infrastructure/Messaging/*`

Что сделать:
- поднять `docker compose up --build`;
- пройти сценарий register/login -> create controller/sensor/relay -> telemetry -> telemetry saved -> control reaction -> notification record;
- зафиксировать known gaps.

Почему это высокий приоритет:
- без runtime smoke готовность к МК пока подтверждена только inspection-ом, а не рабочим стендом.

## Tier 2: Reliability and Security

### 3. Controller Registration Response Contract

Файлы:
- `src/Services/DeviceService/DeviceService/Controllers/ControllersController.cs`
- `src/Services/DeviceService/Device.Application/DTOs/Controller/ControllerRegistredResponseDto.cs`
- `README.md`
- `docs/ARCHITECTURE.md`

Статус:
- исправлено: `CreatedAtRoute` использует `{ id = response.ControllerId }`;
- body содержит `ControllerId` и `DeviceToken`.

Оставить в наблюдении:
- проверить `Location` в E2E smoke.

### 4. Refresh Token Security Hardening Follow-Up

Файлы:
- `src/Services/IdentityService/IdentityService.Domain/Entities/RefreshTokenEntity.cs`
- `src/Services/IdentityService/IdentityService.Infrastructure/Repositories/RefreshTokenRepository.cs`
- `src/Services/IdentityService/IdentityService/Controllers/AuthController.cs`
- `src/Services/IdentityService/IdentityService.Application/DTOs/*`

Что сделать:
- описать новый формат refresh token-а `<tokenId>.<secret>`;
- определить reuse/family revocation policy;
- проверить длину и индексы `TokenHash`;
- решить, какие cookie settings нужны для production HTTPS.

Статус:
- runtime-баги регистрации и refresh response уже исправлены;
- raw storage и raw string request уже заменены на hash + DTO;
- production-grade session security еще не достигнут.

### 5. Telemetry Idempotency Under Concurrency

Файлы:
- `src/Services/TelemetryService/Telemetry.Application/Services/TelemetryDataService.cs`
- `src/Services/TelemetryService/Telemetry.Infrastructure/Repositories/TelemetryDataRepository.cs`
- `src/Services/TelemetryService/Telemetry.Infrastructure/Configurations/TelemetryDataConfiguration.cs`

Что сделать:
- оставить pre-check по `ExternalMessageId`;
- добавить явную обработку unique-index violation как neutral duplicate result;
- описать expected behavior при повторной доставке одного hardware message.

### 6. Alert Chain Runtime Verification

Файлы:
- `src/Services/ControlService/ControlService.Application/Services/TelemetryServiceFromEvent.cs`
- `src/Services/ControlService/ControlService.Application/Services/SensorServiceFromEvent.cs`
- `src/Services/NotificationService/Notification.Infrastructure/Messaging/*`
- `src/Services/NotificationService/Notification.Application/Services/*AlertSender.cs`

Что сделать:
- подтвердить `CriticalTelemetryThresholdAlertEvent -> notification`;
- подтвердить `SensorNoDataEvent -> SensorNoDataAlertEvent -> notification`;
- проверить, не создается ли избыточный дубль alert-ов при нескольких affected rules.

Статус:
- `UserId` в sensor-no-data ветке уже исправлен через `existingAquarium.UserId`;
- runtime-smoke еще не пройден.

### 7. Subscription Downgrade Event Contract

Файлы:
- `src/Services/IdentityService/IdentityService.Application/Services/SubscriptionExpiredChecker.cs`
- `src/Contracts/Contracts/Events/*`

Что сделать:
- определить event о downgrade/изменении подписки;
- решить, какие сервисы должны реагировать;
- зафиксировать продуктовый эффект downgrade.

## Tier 3: Engineering Hygiene

### 8. Build/SDK Tooling Follow-Up

Файлы:
- `global.json`
- `src/Directory.Build.props`
- сервисные `.csproj`/`.slnx`
- `README.md`
- `docs/IMPLEMENTATION_PLAN.md`

Что сделать:
- сохранить корневой `global.json` с валидным SDK feature band;
- использовать подтвержденную команду `dotnet build <API.csproj> -m:1 -v:minimal /p:MSBuildEnableWorkloadResolver=false`;
- отдельно расследовать параллельный restore/build без `-m:1`, который может падать без явных compile errors;
- подтвердить `docker compose up --build`.

Статус:
- `dotnet build` подтвержден для всех 5 API-проектов через `-m:1`;
- `Device.API.csproj` после добавления telemetry validators собирается успешно;
- остаются предупреждения `NU1900` по vulnerability data с NuGet;
- `IdentityService` имеет один nullable warning по `SubscriptionEntity.Name`.

### 9. Repository Cleanliness

Файлы/артефакты:
- `.gitignore`
- `.vs`
- `.idea`
- `.dotnet`
- `.dotnet-home`
- `build.log`
- `infra-build*.log`
- `restore-diag.log`
- `src/**/*.Backup.tmp`

Что сделать:
- исключить IDE/build/temp артефакты из отслеживания;
- оставить только воспроизводимые исходники, миграции и документацию;
- не коммитить локальные SDK/home директории.

### 10. Naming and Contract Cleanup

Файлы и артефакты:
- `AquariumCreatedEvend`
- `AquarimUdatedEvent`
- `AquriumId`
- `Device.Infrastrucrute`
- `ControllerRegistredResponseDto`

Что сделать:
- решить, что считается legacy-contract и что можно переименовать сейчас;
- если переименовывать события, делать это синхронно во всех producers/consumers;
- не добавлять новые сущности с опечатками.

## Done Since Previous Review

- Исправлен register bug в `AuthService`.
- Исправлен refresh response: возвращается новый refresh token.
- Добавлен `RefreshTokenRequestDto`.
- Refresh token storage переведен на `TokenHash`.
- Корневой `global.json` исправлен на валидный SDK feature band.
- `device-api` опубликован в compose на `5237`.
- Direct MCU lab mode и telemetry payload/response описаны в `README.md`.
- `ExternalMessageId` валидируется на null/empty/whitespace.
- `NotificationService` imports переведены на `Contracts.Events.AquariumEvents`.
- `SensorNoDataAlertEvent.UserId` больше не берется из `HttpContext`-dependent `IUserContext`.
- `ControllersController` теперь возвращает onboarding DTO с `DeviceToken` в body.
- `CreatedAtRoute` в `ControllersController` исправлен на `{ id = response.ControllerId }`.
- Добавлены `FluentValidation` validators для telemetry batch/items.
- Пустой `Items` и `Guid.Empty` sensor id теперь валидируются.

## Not A Priority Now

- Перенос ingress в `TelemetryService`: текущий boundary через `DeviceService` правильнее.
- CQRS/MediatR/SignalR/gRPC: это roadmap после стабилизации first hardware smoke.
- Расширение внешних notification providers: сначала нужен E2E alert chain.
