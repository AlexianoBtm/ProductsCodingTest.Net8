# Products API quickstart

> **Project classification:** public technical sample, originally built as a coding exercise. This document describes the API that exists in this repository. It is not client work, production documentation, or evidence of commercial results.

This guide takes a developer from local setup to an authenticated API call, product creation, listing, filtering, and common error handling.

## API at a glance

| Method | Path | Authentication | Purpose |
|---|---|---:|---|
| `GET` | `/health` | No | Check API and SQLite availability |
| `POST` | `/api/auth/login` | No | Exchange the configured local demo credentials for a short-lived JWT |
| `GET` | `/api/products` | Bearer JWT | List all products |
| `GET` | `/api/products?colour={value}` | Bearer JWT | Filter products by colour, case-insensitively |
| `POST` | `/api/products` | Bearer JWT | Validate and create a product |

The examples below use the HTTP development profile at `http://localhost:5193`.

## 1. Prerequisites

- .NET 8 SDK
- `curl` or another HTTP client
- A local clone of this repository

## 2. Configure local-only credentials

The API will not start until a JWT signing key and demo password are supplied outside source control.

From the repository root:

```bash
dotnet user-secrets set --project Products.Api "Jwt:Key" "<random-value-of-at-least-32-bytes>"
dotnet user-secrets set --project Products.Api "DemoAuth:Password" "<local-demo-password-of-at-least-12-characters>"
```

The default local username is `demo`. The default JWT issuer, audience, and expiry are configured in `Products.Api/appsettings.json`.

Do not reuse these local demo values in another application or commit them to source control.

## 3. Start the API

```bash
dotnet restore
dotnet run --project Products.Api --launch-profile http
```

The API applies its SQLite migrations at startup. With the HTTP profile, the base URL is:

```text
http://localhost:5193
```

Check readiness:

```bash
curl -i http://localhost:5193/health
```

Successful response:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

```json
{
  "status": "Healthy",
  "database": "Available"
}
```

If SQLite cannot be reached, the endpoint returns `503 Service Unavailable` with:

```json
{
  "status": "Unhealthy",
  "database": "Unavailable"
}
```

## 4. Authenticate

Send the configured local username and password to the login endpoint:

```bash
curl -i \
  -X POST http://localhost:5193/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "demo",
    "password": "<your-local-demo-password>"
  }'
```

Successful response:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

```json
{
  "token": "<jwt>",
  "expiresAtUtc": "2026-08-05T20:00:00Z"
}
```

The timestamp above is illustrative. Copy the returned `token` value for the protected requests below.

Bash:

```bash
TOKEN="<paste-token-here>"
```

PowerShell:

```powershell
$TOKEN = "<paste-token-here>"
```

Invalid credentials return `401 Unauthorized`:

```json
{
  "error": "Invalid username or password."
}
```

## 5. Create a product

Bash:

```bash
curl -i \
  -X POST http://localhost:5193/api/products \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop Stand",
    "description": "Adjustable aluminum stand",
    "colour": "Silver",
    "price": 34.99
  }'
```

PowerShell:

```powershell
$body = @{
  name = "Laptop Stand"
  description = "Adjustable aluminum stand"
  colour = "Silver"
  price = 34.99
} | ConvertTo-Json

Invoke-RestMethod \
  -Method Post \
  -Uri "http://localhost:5193/api/products" \
  -Headers @{ Authorization = "Bearer $TOKEN" } \
  -ContentType "application/json" \
  -Body $body
```

Successful creation returns `201 Created` and the saved representation:

```json
{
  "id": "0f8fad5b-d9cb-469f-a165-70867728950e",
  "name": "Laptop Stand",
  "description": "Adjustable aluminum stand",
  "colour": "Silver",
  "price": 34.99,
  "createdAtUtc": "2026-08-05T19:00:00Z"
}
```

The identifier and timestamp above are illustrative and will differ for each created product.

### Product request fields

| Field | Type | Rules |
|---|---|---|
| `name` | string | Required; non-whitespace; maximum 200 characters |
| `description` | string | Optional; maximum 1,000 characters; stored as an empty string when omitted or null |
| `colour` | string | Required; non-whitespace; maximum 100 characters |
| `price` | decimal | From `0.01` through `999999999999.99`; maximum two decimal places |

String values are trimmed before persistence.

## 6. List products

```bash
curl -i \
  http://localhost:5193/api/products \
  -H "Authorization: Bearer $TOKEN"
```

The response is a JSON array ordered by creation time according to the repository implementation:

```json
[
  {
    "id": "0f8fad5b-d9cb-469f-a165-70867728950e",
    "name": "Laptop Stand",
    "description": "Adjustable aluminum stand",
    "colour": "Silver",
    "price": 34.99,
    "createdAtUtc": "2026-08-05T19:00:00Z"
  }
]
```

An empty database returns:

```json
[]
```

## 7. Filter by colour

The `colour` query parameter is trimmed and matched case-insensitively.

```bash
curl -i \
  "http://localhost:5193/api/products?colour=silver" \
  -H "Authorization: Bearer $TOKEN"
```

A missing, empty, or whitespace-only `colour` value behaves like the unfiltered list endpoint.

## 8. Authentication failures

`GET /api/products` and `POST /api/products` require:

```http
Authorization: Bearer <jwt>
```

A missing, invalid, or expired token returns `401 Unauthorized`. The client should obtain a new token by logging in again; this sample does not implement refresh tokens.

## 9. Validation and error responses

The API uses two validation paths:

1. ASP.NET Core model validation for request shape, required fields, maximum lengths, and the configured numeric range.
2. Application validation for trimmed strings, positive price, maximum price, and decimal precision.

Model-validation failures return `400 Bad Request` with `application/problem+json` and an `errors` object keyed by field.

Application-validation failures also return `400 Bad Request` with `application/problem+json`. For example, a price with more than two decimal places returns fields equivalent to:

```json
{
  "title": "Validation failed",
  "status": 400,
  "detail": "Product price cannot contain more than two decimal places."
}
```

ASP.NET Core may add metadata such as `type` or `traceId` to a Problem Details response.

Unexpected exceptions return `500 Internal Server Error` with a generic detail; internal exception messages are not returned to the caller.

## 10. OpenAPI and Swagger

Swagger is enabled only when `ASPNETCORE_ENVIRONMENT` is `Development`.

- Swagger UI: `http://localhost:5193/swagger`
- OpenAPI document: `http://localhost:5193/swagger/v1/swagger.json`

The Swagger configuration includes a Bearer security definition. Use the JWT returned by `/api/auth/login` in the Swagger UI authorization dialog.

## 11. Verify the implemented behavior

Run the repository checks from the root:

```bash
dotnet restore
dotnet build ProductsCodingTest.sln --no-restore --configuration Release
dotnet test ProductsCodingTest.sln --no-build --configuration Release
dotnet list ProductsCodingTest.sln package --vulnerable --include-transitive
```

The integration tests cover:

- valid and invalid login;
- protected endpoints rejecting unauthenticated requests;
- authenticated list and create operations;
- validation failures returning Problem Details;
- case-insensitive colour filtering;
- database-aware health behavior.

## 12. Scope and security limitations

This is a deliberately small local sample. It does not implement:

- registration, account management, password hashing, or a user store;
- roles, permissions, refresh tokens, revocation, or rotation;
- production secret management or distributed signing keys;
- pagination, update, or delete endpoints;
- rate limiting, production monitoring, or service-level objectives;
- a production deployment or internet-facing security posture.

Review `README.md`, `docs/architecture-overview.md`, and `SECURITY.md` before reusing any design ideas. The sample must be redesigned before production use.
