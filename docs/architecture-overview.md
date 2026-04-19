# Products Coding Test — Architecture Overview

## Purpose

This repository contains a coding test submission for a **Products Web API** built with **.NET 8** and a simple **React + Vite** frontend.

The goal of the solution is not only to satisfy the requested features, but also to present a small, clean, and professional implementation that feels realistic for a production-minded environment.

The project follows a **small but enterprise-minded** approach:

- small in scope
- clear in structure
- realistic in technical choices
- easy to review and run
- intentionally free of unnecessary overengineering

---

## Requested Requirements

The original coding test requested a .NET 6+ Products Web API with:

- an anonymous health check endpoint
- secured endpoints to:
  - create a product
  - list all products as JSON
  - get products by colour
- unit tests
- integration tests
- a simple architecture diagram showing how the service could fit into an event-driven or distributed architecture
- a React or Angular frontend that consumes the API
- a public GitHub repository

This solution fulfills those requirements using **.NET 8**, which remains within the requested `.NET 6+` range while reflecting a more current stack choice.

---

## Technology Stack

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- Swagger / OpenAPI

### Testing
- xUnit
- Unit tests for application logic
- Integration tests for real HTTP/API behavior

### Frontend
- React
- Vite

---

## Solution Structure

The repository is organized into separate projects with clear responsibilities:

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

This structure follows the layered architecture and keeps responsibilities separated clearly.

---

## Layer Responsibilities

### Products.Domain
Contains the core domain model.

**Current responsibility**
- Defines the `Product` entity

**Why it exists**
- Keeps the business concept of a product independent from HTTP, persistence, or UI concerns

---

### Products.Application
Contains application logic, contracts, DTOs, and configuration abstractions used by the business layer.

**Current responsibilities**
- Product DTOs
- Auth DTOs
- Service interfaces
- Repository interfaces
- Application services
- JWT options configuration class

**Important architectural note**
`JwtOptions` is intentionally placed in **Products.Application.Configuration**, not in the API project. This keeps configuration models used by the token service in the application layer and preserves cleaner dependency direction.

---

### Products.Infrastructure
Contains persistence and external implementation details.

**Current responsibilities**
- `ProductsDbContext`
- EF Core configuration
- SQLite persistence
- Repository implementation
- EF Core migrations
- Infrastructure dependency injection wiring

**Why it exists**
- Keeps database concerns separate from application logic and HTTP concerns

---

### Products.Api
Contains the HTTP layer and application startup configuration.

**Current responsibilities**
- Controllers
- Authentication wiring
- Authorization setup
- Swagger configuration
- CORS configuration
- Dependency injection registration
- Request pipeline setup

**Why it exists**
- Exposes the application as a usable API and keeps transport concerns out of the core layers

---

### Products.UnitTests
Contains isolated tests for application logic.

**Current responsibilities**
- Tests for `ProductService`
- Fake repository used to verify business rules without a real database

---

### Products.IntegrationTests
Contains end-to-end tests for API behavior.

**Current responsibilities**
- Health endpoint tests
- Login endpoint tests
- Protected products endpoint tests
- Integration host setup with test database migration handling

---

### frontend/products-web
Contains the React frontend that consumes the API.

**Current responsibilities**
- Health status display
- Login flow
- Products list
- Product creation form
- Colour filtering

---

## Domain Model

The main entity in the system is `Product`.

### Product fields
- `Id`
- `Name`
- `Description`
- `Colour`
- `Price`
- `CreatedAtUtc`

This model is intentionally simple. It is realistic enough to feel professional, but small enough to keep the solution focused on the actual coding test requirements.

---

## API Design

### Anonymous endpoints

#### `GET /health`
Used to verify that the service is running.

**Expected response**
```json
{
  "status": "OK"
}
```

#### `POST /api/auth/login`
Used to obtain a JWT for testing protected endpoints.

**Demo credentials**
- Username: `admin`
- Password: `Password123!`

**Response**
- JWT token
- Expiration timestamp

---

### Protected endpoints

#### `GET /api/products`
Returns all products as JSON.

#### `GET /api/products?colour=Black`
Returns products filtered by colour.

**Behavior**
- colour filtering is case-insensitive
- returns an empty array when there are no matches

#### `POST /api/products`
Creates a new product.

**Validation rules**
- `Name` is required
- `Colour` is required
- `Price` must be greater than `0`

These API behaviors align with the final architecture direction of the project.

---

## Authentication Flow

The project uses **JWT Bearer Authentication** for protected endpoints.

### Current flow
1. The user logs in through `POST /api/auth/login`
2. The API validates the demo credentials
3. A JWT is generated through the token service
4. The frontend or Swagger sends the token as a Bearer token
5. Protected endpoints require that token

### Why JWT was chosen
JWT provides a more realistic secured API flow than a simple API key approach and better reflects how protected endpoints are commonly handled in professional systems.

---

## Persistence Flow

The project uses **SQLite** through **Entity Framework Core**.

### Current persistence path
1. Request reaches the API controller
2. Controller calls the application service
3. Application service applies validation and mapping
4. Repository persists or queries data through EF Core
5. Data is stored in SQLite

### Why SQLite was chosen
SQLite provides real persistence without creating unnecessary setup friction for the reviewer. It makes the project more realistic than an in-memory list while keeping local execution simple.

---

## Frontend Flow

The React frontend is intentionally simple and focused on proving integration rather than visual complexity.

### Current frontend features
- login
- health status check
- products list
- colour filter
- create product form

### Integration details
- the frontend calls the backend API directly
- JWT is stored temporarily for authenticated calls
- CORS was configured in the API so the frontend can call the backend from local development

This matches the intended direction of keeping the frontend simple, functional, and professional rather than overdesigned.

---

## Testing Strategy

### Unit Tests
The unit tests focus on application logic in `ProductService`.

**Covered behavior**
- creating a valid product
- rejecting empty name
- rejecting empty colour
- rejecting invalid price
- returning all products
- filtering by colour
- case-insensitive colour filtering

---

### Integration Tests
The integration tests verify real API behavior end to end.

**Covered behavior**
- `GET /health` returns `200 OK`
- `POST /api/auth/login` returns a valid token for correct credentials
- protected product endpoints return `401 Unauthorized` without a token
- product creation succeeds with a valid token
- product listing succeeds with a valid token
- colour filtering returns matching products only

---

## High-Level Request Flow

### Create Product
1. User logs in
2. User receives JWT
3. Frontend sends authenticated request to `POST /api/products`
4. API validates input
5. Application service creates the domain entity
6. Repository stores it in SQLite
7. API returns `201 Created`

### List Products
1. User is authenticated
2. Frontend calls `GET /api/products`
3. API retrieves data through the application and infrastructure layers
4. API returns a JSON array of products

### Filter by Colour
1. Frontend sends `GET /api/products?colour=...`
2. API applies case-insensitive filtering
3. API returns matching results

---

## Event-Driven Architecture Positioning

This coding test does **not** implement real event publishing or a real message broker.

However, the service is intentionally documented as if it could later fit into a broader distributed or event-driven architecture.

A simple conceptual future flow would be:

```text
React Frontend
    │
    ▼
Products API
    │
    ├── SQLite Database
    │
    └── ProductCreated event
            │
            ├── Orders Service
            └── Payments Service
```

### Interpretation
- the frontend interacts with the Products API
- the Products API persists product data
- after product creation, the service could later publish a `ProductCreated` event
- other services could consume that event depending on business needs

This event-driven architecture is documented only for the coding test. It is not implemented physically in the current project.

---

## Key Design Decisions

### Why .NET 8
The requirement allows `.NET 6+`, and `.NET 8` presents a modern and current stack choice while remaining fully compliant.

### Why layered architecture
It keeps the project easy to review and makes responsibilities obvious:
- domain model stays clean
- application logic stays testable
- infrastructure stays replaceable
- API stays focused on HTTP concerns

### Why JWT
It gives the protected endpoints a realistic authentication flow.

### Why SQLite
It provides real persistence with minimal reviewer setup friction.

### Why React + Vite
It gives a modern frontend with fast local startup and enough capability to demonstrate full-stack integration.

---

## Current Completion State

At this stage, the core solution includes:

- completed backend architecture
- working persistence
- anonymous health and login endpoints
- protected product endpoints
- working React frontend
- passing unit tests
- passing integration tests

The remaining work for final submission is focused on:
- repository cleanup
- final README polish
- final docs/packaging review
- final reviewer-style validation

---

## Final Notes

This solution is intentionally designed to be:

- small in scope
- clear in structure
- realistic in implementation
- easy for a reviewer to run
- professional without unnecessary complexity

The project is meant to demonstrate sound engineering judgment, not just feature completion.
