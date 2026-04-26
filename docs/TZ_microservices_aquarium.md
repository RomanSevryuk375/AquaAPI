# Учебное ТЗ: API для управления МК, аквариумами и террариумами

## Статус относительно текущего кода AquaAPI

- Этот файл описывает учебную/целевую архитектуру и сверяет ее с текущим кодом.
- По состоянию на 26 апреля 2026 года ingress телеметрии реализован через `DeviceService`, что соответствует правильной границе ответственности.
- Текущий endpoint: `POST /api/device/v1/sensors/telemetry`.
- `NotificationService` уже не является пустым шаблоном: есть API, consumers, jobs и отдельная БД.
- Старые blockers по register/refresh, refresh DTO/hash, aquarium namespace imports, `SensorNoDataAlertEvent.UserId` и публикации `device-api` исправлены в коде.
- `CreatedAtRoute` controller onboarding исправлен, пустой batch и `Guid.Empty` sensor id валидируются.
- Оставшиеся blockers для МК: исправить batch-size validator, подтвердить compose и пройти E2E smoke.

## 1. Цель проекта

Сделать учебную микросервисную систему для управления климатом, светом, фильтрацией и обслуживанием аквариумов/террариумов через МК.

Практическая цель:
- принять телеметрию от контроллера;
- сохранить измерения;
- проверить правила автоматизации;
- изменить состояние реле;
- уведомить пользователя при проблемах.

## 2. Минимальный состав сервисов

- `DeviceService` - МК, контроллеры, датчики, реле, ingress от железа.
- `TelemetryService` - история измерений и telemetry events.
- `ControlService` - аквариумы, правила, расписания, реакции.
- `NotificationService` - уведомления, напоминания, журнал обслуживания.
- `IdentityService` - пользователи, JWT, refresh tokens, подписки.
- `ApiGateway` - единая точка входа и JWT boundary.

## 3. Ключевой сценарий

1. МК отправляет telemetry batch в `DeviceService`.
2. `DeviceService` валидирует `MacAddress`, `X-Device-Token` и принадлежность датчиков.
3. `DeviceService` публикует `TelemetryReportedFromHardwareEvent`.
4. `TelemetryService` сохраняет данные и публикует `TelemetryReceivedEvent`.
5. `ControlService` проверяет правила и публикует relay-команду.
6. `DeviceService` меняет состояние реле.
7. `NotificationService` обрабатывает alert events и создает/отправляет уведомления.

## 4. Стек

- `.NET 8`
- `ASP.NET Core Web API`
- `EF Core`
- `PostgreSQL` отдельно на сервис
- `RabbitMQ`
- `MassTransit`
- `Quartz`
- `YARP`
- `Docker Compose`

## 5. Что обязательно в учебной части

- Один внешний ingress для железа через `DeviceService`.
- Одна асинхронная цепочка через RabbitMQ.
- Idempotency для обработки телеметрии.
- Сквозной маршрут через gateway.
- Отдельная БД на сервис.
- Документированный hardware/emulator smoke.

## 6. Что не входит в первый этап

- Kubernetes.
- High-load tuning.
- OTA firmware updates.
- Сложная RBAC-модель.
- Реальное железо как обязательная зависимость: первый smoke можно пройти эмулятором.

## 7. Критерии готовности

- `dotnet build` проходит воспроизводимо.
- `docker compose up --build` поднимает весь проект.
- МК access mode выбран и описан.
- Registration контроллера возвращает `ControllerId` и `DeviceToken`.
- Direct lab endpoint `http://localhost:5237` описан для МК/эмулятора.
- Сквозной сценарий "телеметрия -> решение -> команда -> уведомление" работает.
- README содержит API-примеры и smoke-сценарий.

## 8. Рекомендуемый telemetry ingress

Для AquaAPI прием телеметрии правильно строить через `DeviceService`, потому что этот сервис владеет контроллерами, токенами устройств, датчиками и реле.

Текущий вариант:

```http
POST http://localhost:5237/api/device/v1/sensors/telemetry
Header: X-Device-Token: <controller-device-token>
```

Текущий payload:

```json
{
  "macAddress": "AA:BB:CC:DD:EE:FF",
  "items": [
    {
      "sensorId": "a3c2f9c1-1111-2222-3333-444444444444",
      "value": 25.4,
      "externalMessageId": "esp32-1744621200-001:a3c2f9c1-1111-2222-3333-444444444444",
      "recordedAt": "2026-04-26T09:00:00Z"
    }
  ]
}
```

Что нужно довести:
- исправить правило максимального размера batch: сейчас код требует `items.Count > 50`, хотя ожидаемый лимит - до 50 элементов;
- исправить текст ошибки для пустого `externalMessageId`;
- описать response ошибки/частичного принятия;
- подтвердить direct `device-api` runtime-smoke-ом.

## 9. Что должен делать `DeviceService`

Ingress endpoint должен:
- проверить `MacAddress`;
- проверить `X-Device-Token`;
- проверить принадлежность сенсоров контроллеру;
- отклонять некорректные элементы диагностически;
- публиковать нормализованные telemetry events;
- по возможности обновлять `LastSeenAt` контроллера при валидном обмене.

## 10. Что должен делать `TelemetryService`

`TelemetryService` должен:
- проверить локальную проекцию сенсора;
- проверить `ExternalMessageId`;
- сохранить telemetry data;
- обработать duplicate delivery neutral success-ом;
- опубликовать `TelemetryReceivedEvent`;
- отслеживать no-data состояния.

Текущий gap:
- конкурентный duplicate-case требует явной обработки unique-index violation.

## 11. Что должен делать `ControlService`

`ControlService` должен:
- consume-ить `TelemetryReceivedEvent`;
- находить правила по сенсору;
- сравнивать значение с порогами;
- публиковать `ChangeRelayStateCommand`;
- публиковать alert events с корректным `UserId`.

Текущий статус:
- `UserId` в `SensorNoDataAlertEvent` исправлен через `Aquarium.UserId`.

## 12. Что должен делать `NotificationService`

`NotificationService` должен покрывать:
- reminders;
- maintenance log;
- user-facing notifications;
- sensor no data alerts;
- controller offline alerts;
- critical telemetry alerts.

Текущий статус:
- сервис структурно реализован;
- namespace imports по aquarium events исправлены;
- runtime alert smoke еще не подтвержден.

## 13. Практические next steps

1. Подтвердить `docker compose up --build`.
2. Пройти direct MCU lab mode smoke.
3. Исправить batch-size правило в `TelemetryBatchRequestValidator`.
4. Согласовать validation error messages telemetry ingress с README/docs.
5. Пройти E2E smoke с эмулятором.

## 14. Planned архитектурные расширения

### `planned`: CQRS + MediatR

Внедрять после первого стабильного E2E. Начинать с `ControlService`.

### `planned`: SignalR

Внедрять после стабильных telemetry/alert events. Цель: live dashboard без polling.

### `planned`: gRPC

Внедрять для внутренних synchronous query-сценариев, например проверки лимитов подписки.

### `planned`: gRPC streaming

Внедрять после стабильной HTTP-схемы обмена с МК. Цель: постоянный канал с эмулятором/контроллером.
