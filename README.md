# Products Coding Test

A small but professional **.NET 8 + React** coding test submission that implements a secured Products Web API, real persistence with SQLite, automated tests, and a simple frontend that consumes the API.

## Project Overview

This solution was built to satisfy the requested coding test requirements while presenting the project in a clean, realistic, and reviewer-friendly way.

The implementation includes:

- anonymous health endpoint
- JWT-based authentication
- protected product endpoints
- SQLite persistence with Entity Framework Core
- unit tests
- integration tests
- React + Vite frontend
- architecture documentation

## Tech Stack

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- Swagger / OpenAPI

### Frontend
- React
- Vite

### Testing
- xUnit

## Solution Structure

```text
ProductsCodingTest.Net8/
├── Products.Api/
├── Products.Application/
├── Products.Domain/
├── Products.Infrastructure/
├── Products.UnitTests/
├── Products.IntegrationTests/
├── frontend/
│   └── products-web/
├── docs/
└── README.md
```

## Architecture Summary

The solution follows a layered structure to keep responsibilities separated clearly:

- **Products.Domain**: core `Product` entity
- **Products.Application**: DTOs, interfaces, services, and shared configuration such as `JwtOptions`
- **Products.Infrastructure**: EF Core, SQLite, repository implementation, migrations
- **Products.Api**: controllers, authentication setup, authorization, Swagger, CORS, startup
- **Products.UnitTests**: isolated service-level tests
- **Products.IntegrationTests**: end-to-end API tests
- **frontend/products-web**: React frontend that consumes the API

A more detailed architecture explanation is included here:

- `docs/architecture-overview.md`

## Authentication

The protected product endpoints use **JWT Bearer Authentication**.

### Demo credentials
- Username: `admin`
- Password: `Password123!`

## Available Endpoints

### Anonymous
- `GET /health`
- `POST /api/auth/login`

### Protected
- `GET /api/products`
- `GET /api/products?colour=Black`
- `POST /api/products`

## Functional Rules

### Product creation
- `Name` is required
- `Colour` is required
- `Price` must be greater than `0`

### Product filtering
- colour filtering is case-insensitive
- no matches return an empty array

## How to Run the Backend

From the repository root:

```bash
dotnet restore
dotnet build
dotnet run --project Products.Api/Products.Api.csproj
```

Once running, the API should be available through the local ASP.NET Core launch profile output.

Swagger should be available at:

```text
/swagger
```

## How to Run the Frontend

From the frontend folder:

```bash
cd frontend/products-web
npm install
npm run dev
```

The frontend runs locally through Vite and calls the backend API.

Make sure the backend is running before starting the frontend.

If PowerShell blocks `npm` scripts on Windows, run `npm.cmd install` and `npm.cmd run dev`, or use Command Prompt instead.

## How to Run Tests

### Unit tests
```bash
dotnet test Products.UnitTests
```

### Integration tests
```bash
dotnet test Products.IntegrationTests
```

### All tests
```bash
dotnet test
```

## Frontend Features

The frontend is intentionally simple and focused on demonstrating integration rather than heavy design.

Current functionality:
- login
- health status check
- products list
- colour filter
- create product form

## Persistence

The project uses **SQLite** with **Entity Framework Core**.

Migrations are included in the repository, and the project uses a local SQLite database for simple setup and real persistence.

## Testing Summary

The solution includes both unit and integration testing.

### Unit tests cover
- valid product creation
- invalid product creation
- get all products
- colour filtering
- case-insensitive filtering

### Integration tests cover
- health endpoint
- login endpoint
- unauthorized access to protected endpoints
- authorized product retrieval
- authorized product creation
- colour filtering through the real API

## Reviewer Notes

This project was intentionally designed to be:

- small in scope
- clear in structure
- realistic in implementation
- easy to run locally
- professional without unnecessary overengineering

The goal was to deliver a coding test submission that feels solid, modern, and easy to review.

## Future Improvements

Possible next improvements, if this were expanded beyond the coding test, could include:

- Docker support
- consistent global exception handling
- cleaner validation response formatting
- seed data improvements
- CI workflow for automated test execution
- richer frontend UX

## Documentation

Additional project documentation:

- `docs/architecture-overview.md`
