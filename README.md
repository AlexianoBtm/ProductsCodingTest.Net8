# Layered Products API with .NET 8 and React

[![CI](https://github.com/AlexianoBtm/ProductsCodingTest.Net8/actions/workflows/ci.yml/badge.svg)](https://github.com/AlexianoBtm/ProductsCodingTest.Net8/actions/workflows/ci.yml)

> **Project classification:** public technical sample, originally built as a coding exercise. It is not client work, paid work, a production system, or evidence of commercial results.

This repository demonstrates a small authenticated product workflow across an ASP.NET Core API, EF Core/SQLite persistence, automated .NET tests, and a React/Vite client. The scope is intentionally narrow so the code and trade-offs can be reviewed quickly.

## What it demonstrates

- Layered .NET solution with Domain, Application, Infrastructure, and API projects
- JWT-protected product endpoints using local demo authentication
- EF Core migrations and SQLite persistence
- Product creation, listing, and case-insensitive colour filtering
- Consistent API validation and Problem Details error responses
- Database-aware health reporting
- Isolated unit and HTTP integration tests
- React client with session-expiry handling and configurable API URL
- CI checks for backend build/tests and frontend lint/build/audit

It does **not** demonstrate production identity, distributed systems, payments, deployment, multi-user authorization, or a production security posture.

## Solution structure

```text
ProductsCodingTest.Net8/
├── Products.Api/
├── Products.Application/
├── Products.Domain/
├── Products.Infrastructure/
├── Products.UnitTests/
├── Products.IntegrationTests/
├── frontend/products-web/
├── docs/
└── .github/workflows/ci.yml
```

See [Architecture overview](docs/architecture-overview.md) for responsibilities, request flow, and explicit limitations.

## API surface

| Access | Endpoint | Purpose |
|---|---|---|
| Public | `GET /health` | Confirms API and database availability |
| Public | `POST /api/auth/login` | Issues a short-lived JWT for the configured local demo user |
| Protected | `GET /api/products` | Lists products |
| Protected | `GET /api/products?colour=Black` | Filters products by colour |
| Protected | `POST /api/products` | Creates a product after validation |

## Prerequisites

- .NET 8 SDK
- Node.js 22 or a Vite-compatible Node.js 20 release
- npm

## Configure local-only credentials

No JWT signing key or demo password is stored in source control. Configure your own local values with .NET User Secrets:

```bash
dotnet user-secrets set --project Products.Api "Jwt:Key" "<random-value-of-at-least-32-bytes>"
dotnet user-secrets set --project Products.Api "DemoAuth:Password" "<your-local-demo-password>"
```

The default local demo username is `demo`. It is non-secret and can be overridden with `DemoAuth:Username`.

Environment variables are also supported through .NET's standard double-underscore mapping:

```text
Jwt__Key
DemoAuth__Username
DemoAuth__Password
ConnectionStrings__ProductsDb
Frontend__AllowedOrigins__0
```

The API fails at startup when required credentials are missing or too short. Do not reuse local demo values in another application.

## Run the backend

From the repository root:

```bash
dotnet restore
dotnet build --no-restore
dotnet run --project Products.Api
```

The API uses `Products.Api/products.db` by default and applies migrations on startup. Swagger is available in the Development environment.

## Run the frontend

```bash
cd frontend/products-web
cp .env.example .env
npm ci
npm run dev
```

`VITE_API_BASE_URL` defaults to `http://localhost:5193` and can be changed in the local `.env` file.

## Validate the repository

Backend:

```bash
dotnet restore
dotnet build --no-restore --configuration Release
dotnet test --no-build --configuration Release
dotnet list ProductsCodingTest.sln package --vulnerable --include-transitive
```

Frontend:

```bash
cd frontend/products-web
npm ci
npm run lint
npm run build
npm audit --audit-level=high
```

GitHub Actions runs these checks for pushes and pull requests targeting `main`.

## Security model and limitations

- Authentication is deliberately a local single-user demo configured outside source control.
- The demo password is compared directly; there is no user store, password hashing, refresh token, role model, or account lifecycle.
- JWTs are kept in browser `sessionStorage`, removed on logout, and rejected by the client after expiry.
- CORS origins and the frontend API URL are configurable.
- The historical JWT key and demo password in earlier commits were synthetic, exclusive to this sample, and are treated as permanently public and invalid.
- This code must be redesigned before any production or internet-facing deployment.

See [SECURITY.md](SECURITY.md) for reporting guidance.

## Data and content

The repository contains no client, patient, employee, or other real operational data. Products created during local use remain in an ignored SQLite database. All repository-specific code and graphics were created for this sample and may be published as portfolio evidence.

## Deliberate exclusions

- Production authentication and authorization
- Edit/delete endpoints and pagination
- Container or cloud deployment
- Message broker or event publishing
- Frontend automated tests
- Production observability and service-level objectives

These are scope boundaries, not claims of implemented capability.

## License

The source is publicly viewable for portfolio evaluation. No open-source reuse license is granted; see [LICENSE](LICENSE).
