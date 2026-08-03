# Layered Products API — Architecture Overview

## Purpose and classification

This repository is a public technical sample, originally built as a coding exercise. It demonstrates a compact full-stack workflow; it is not client work, a deployed production system, or an implementation of microservices or event-driven architecture.

## Implemented topology

```text
React client
    │ HTTP + Bearer token
    ▼
Products.Api
    │
    ▼
Products.Application
    │ repository interface
    ▼
Products.Infrastructure
    │ EF Core
    ▼
SQLite
```

### `Products.Domain`

Defines the `Product` entity independently of HTTP and persistence.

### `Products.Application`

Contains DTOs, service and repository contracts, product validation, mapping, and token creation. `JwtOptions` and `DemoAuthOptions` describe configuration consumed by application/API services.

### `Products.Infrastructure`

Implements the EF Core `ProductsDbContext`, repository, dependency registration, and SQLite migrations. Decimal prices are stored as text to avoid converting monetary values through binary floating point.

### `Products.Api`

Owns controllers, authentication/authorization wiring, configurable CORS, Swagger, centralized exception responses, startup migration, and the database-aware health endpoint.

### Test projects

- `Products.UnitTests` checks application rules without a database.
- `Products.IntegrationTests` starts the real HTTP pipeline with unique temporary SQLite databases and test-only in-memory credentials.

### `frontend/products-web`

Provides login, health reporting, product creation, listing, and colour filtering. The client reads its API URL from `VITE_API_BASE_URL`, keeps the short-lived JWT in `sessionStorage`, checks expiration, and clears the session on `401` responses.

## Request flow

### Login

1. The client posts locally configured demo credentials to `POST /api/auth/login`.
2. The API compares them with configuration supplied outside source control.
3. `TokenService` creates a short-lived JWT with configured issuer, audience, and signing key.
4. The client keeps the token for the current browser tab/session only.

### Create product

1. The client sends an authenticated request to `POST /api/products`.
2. ASP.NET Core validates required fields, lengths, and the allowed price range.
3. `ProductService` repeats critical business validation, trims text, and creates the entity.
4. `ProductRepository` persists it through EF Core/SQLite.
5. The API returns `201 Created` with the product representation.

### Read and filter

1. An authenticated request reaches `GET /api/products`.
2. The application selects all products or delegates a case-insensitive colour filter.
3. Results are ordered by creation time and returned as JSON.

## Validation and errors

- Name: required, maximum 200 characters
- Description: maximum 1,000 characters
- Colour: required, maximum 100 characters
- Price: `0.01` through `999999999999.99`, maximum two decimal places
- Model-binding errors use ASP.NET Core validation Problem Details.
- Service validation and unexpected exceptions pass through the centralized exception handler.
- Internal exception details are not returned for unexpected errors.

## Persistence and migrations

SQLite keeps local setup small and reproducible. The API applies migrations during startup. Integration tests use a different temporary database per fixture and reset product data between tests.

This startup-migration approach is suitable for the sample; coordinated production migration would require a separate deployment strategy.

## Security boundary

The authentication flow demonstrates protected API calls, not production identity. It intentionally excludes:

- registration or account management;
- password hashing and a credential store;
- roles, permissions, refresh tokens, revocation, or rotation;
- cross-site request protections for cookie authentication;
- distributed key management.

Required credentials are supplied through User Secrets or environment variables and validated at startup. The frontend uses `sessionStorage`, but a production client would require a threat model and a deliberate session strategy.

## Health behavior

`GET /health` returns `200` only when the API can connect to SQLite. It returns `503` when the database is unavailable. This is a functional readiness signal for the sample, not a complete observability solution.

## Explicit non-capabilities

The current code does not implement message publishing, a broker, microservices, deployment automation, production monitoring, payments, multi-tenancy, or business outcome tracking. Any future architecture discussion must remain separate from claims about the implemented repository.
