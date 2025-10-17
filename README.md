# GameMatch Backend (.NET 8 + MySQL) — Sem BFF, Sem Front

## Projetos
- **GameMatch.Core**: entidades/enum
- **GameMatch.Infrastructure**: EF Core + MySQL (`AppDb`)
- **GameMatch.Services**: regras (Auth/Group/Match/Notification)
- **GameMatch.Api**: Web API (controllers + JWT + Swagger)

## Setup
1) MySQL
```sql
CREATE DATABASE gamematch CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
```
2) Configure `src/GameMatch.Api/appsettings.json` (ConnectionStrings + Jwt.Key).
3) Restaurar, migrar e rodar:
```bash
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initial -p src/GameMatch.Infrastructure -s src/GameMatch.Api
dotnet ef database update -p src/GameMatch.Infrastructure -s src/GameMatch.Api
dotnet run --project src/GameMatch.Api
```
Swagger: `/swagger`.

## Rotas base
- `POST /api/auth/register` / `POST /api/auth/login`
- `GET /api/groups` / `POST /api/groups`
- `POST /api/groups/{id}/positions`
- `GET /api/match/{groupId}`

> Observação: adapte a extração do `ownerId` (ex.: via JWT `sub`).
