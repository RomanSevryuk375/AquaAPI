# WORKLOG

## Project
- Repo: AquaAPI
- Branch: feature/telemety-dbcontext-configs
- Goal: учебная микросервисная система (Telemetry, Device, Control, Notification, ApiGateway)

## Last Updates (recent commits)
- d34d57a `chore(repo): add microservices scaffolding and build config`
- a185b60 `feat(telemetry): finalize repository wiring and project cleanup`
- 7bbec32 `chore(infrastructure): scaffold repository and DI extension stubs`
- e92cf36 `feat(infrastructure): implement base repository and unit of work`
- 67115c1 `feat(domain): add repository and unit-of-work contracts`

## Current Status
- TelemetryService: есть domain entities, EF configurations, base repository, unit of work, DI extension.
- Остальные сервисы и gateway: созданы как scaffolding (template-level).
- Root build config добавлен: `global.json`, `Directory.Build.props`.

## Known Risks / Notes
- В проекте используются `.slnx` файлы. Для стабильной работы нужно проверять совместимость SDK/CLI.
- `docs/` пока не закоммичена в текущей ветке.
- Нужно постепенно заменить template controllers/endpoint-ы на бизнес-флоу.

## Session Start Checklist
1. Проверить ветку: `git branch --show-current`
2. Проверить статус: `git status --short`
3. Проверить SDK: `dotnet --version`
4. Открыть `docs/NEXT_TASKS.md` и начать с P0
