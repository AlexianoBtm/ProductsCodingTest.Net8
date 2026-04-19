# Products API

A small full-stack product management sample built with .NET 8 and React.

The solution includes a secure Products API, a simple React frontend, automated tests, and supporting documentation. It is designed to be straightforward to run locally while keeping a clean layered architecture and realistic implementation choices.

## Overview

This repository contains:

- A .NET 8 Web API for product management
- JWT-based authentication
- SQLite persistence with automatic startup migrations
- Unit and integration tests
- A React + Vite frontend that consumes the API
- Architecture documentation for the overall solution shape

## Features

- Public health endpoint
- Login endpoint that returns a JWT
- Protected product endpoints
- Create product
- List products
- Filter products by colour
- Frontend integration for login, health check, listing, filtering, and product creation

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
- ASP.NET Core integration testing

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

The backend follows a layered structure:

- **Products.Domain**: core domain model
- **Products.Application**: DTOs, interfaces, services, and application logic
- **Products.Infrastructure**: EF Core persistence and repository implementations
- **Products.Api**: HTTP endpoints, authentication, Swagger, and app startup
- **Products.UnitTests**: unit tests for application logic
- **Products.IntegrationTests**: end-to-end API behavior tests
- **frontend/products-web**: React client application

## API Endpoints

### Public
- `GET /health`
- `POST /api/auth/login`

### Protected
- `GET /api/products`
- `GET /api/products?colour=Black`
- `POST /api/products`

## Demo Credentials

Use the following credentials to obtain a JWT:

```text
Username: admin
Password: Password123!
```

## Running the Backend

From the repository root:

```bash
dotnet restore
dotnet build
dotnet run --project Products.Api
```

Notes:

- The API uses SQLite locally.
- Database migrations are applied automatically on startup.
- Swagger is available once the API is running.

## Running the Frontend

Open a second terminal:

```bash
cd frontend/products-web
npm install
npm run dev
```

Default local frontend URL:

```text
http://localhost:5173
```

## Running Tests

Run all tests:

```bash
dotnet test
```

Or run them by project:

```bash
dotnet test Products.UnitTests
dotnet test Products.IntegrationTests
```

## Authentication in Swagger

1. Run the API
2. Open Swagger
3. Call `POST /api/auth/login`
4. Copy the returned token
5. Click **Authorize**
6. Enter:

```text
Bearer <your-token>
```

## Example Product Request

```json
{
  "name": "Gaming Mouse",
  "description": "Wireless mouse",
  "colour": "Black",
  "price": 49.99
}
```

## Local Notes

If PowerShell blocks `npm`, use:

```bash
npm.cmd install
npm.cmd run dev
```

## Documentation

Additional project documentation is available in the `docs` folder.

## Possible Future Improvements

- Docker support for API and frontend
- Seed data for local development
- Centralized exception handling
- Improved validation response formatting
- UI polish and better state handling
