# GK Customer Portal API

This project is a .NET-based Customer Portal API developed as part of a Senior .NET Technical Assessment.  
It demonstrates a clean, layered architecture with secure data handling, authentication, and full CRUD support.

---

## Architecture

The solution follows a layered architecture with clear separation of concerns:

- **Controllers** – Handle HTTP requests and responses
- **Services** – Contain business logic and validation
- **Data Layer** – Handles database interactions via Entity Framework Core
- **Authentication Layer** – Implements Basic Authentication using a custom handler
- **Encryption Layer** – Handles AES-256 encryption for sensitive data

---

## Key Features

- Full CRUD operations for Customer entity
- Pagination and optional filtering (First Name)
- Secure Basic Authentication
- PII Data Encryption (AES-256)
- Input validation using Data Annotations
- Global error handling and proper HTTP status codes
- Swagger API documentation
- Unit testing using xUnit and Moq
- Containerized using Docker and Docker Compose

---

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- xUnit + Moq (Unit Testing)
- Swagger / OpenAPI
- Docker & Docker Compose
- Microsoft Identity (for authentication)

---

## Requirements Mapping

### ✔ Customer Entity
- Id, FirstName, LastName, Email, Age implemented via `CustomerModel`

### ✔ Data Access Layer
- Implemented using `AppDbContext` and Entity Framework Core
- SQLite used as the database

### ✔ Service Layer
- `CustomerService` handles:
  - Business logic
  - Validation
  - CRUD operations

### ✔ API Endpoints
- `GET /api/customers` – Get all (with paging + filter)
- `GET /api/customers/{id}` – Get by Id
- `POST /api/customers` – Create
- `PUT /api/customers/{id}` – Update
- `DELETE /api/customers/{id}` – Delete

### ✔ Validation & Error Handling
- Data annotations + service-level validation
- Proper HTTP responses:
  - 200 OK
  - 201 Created
  - 204 No Content
  - 400 Bad Request
  - 404 Not Found

### ✔ PII Encryption
- AES-256 encryption applied via EF Core Value Converters
- Fields encrypted at rest:
  - FirstName
  - LastName
  - Email

### ✔ Authentication

- Custom Basic Authentication handler
- Credentials validated against ASP.NET Identity store
- API secured using `[Authorize]`

**Login Credentials:**

- Username: `admin@gk.com`  
- Password: `Password123!`


### ✔ Unit Testing
- Implemented using **xUnit** and **Moq**
- Tests cover:
  - Controller logic
  - Service interaction
  - Success and failure scenarios

---

## How to Run Locally (Docker)

### Prerequisites
- Docker Desktop installed and running

### Steps

1. Clone the repository
2. Navigate to the root directory
3. Run:

```bash
docker-compose up -d --build