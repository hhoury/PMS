# PMS Solution

## Overview

This repository contains the **PMS** solution, a modular and maintainable web API built with **.NET 8** and **C# 12**. The project is designed using **Clean Architecture** principles, ensuring separation of concerns, testability, and scalability. It implements the **Repository Pattern** combined with the **Unit of Work Pattern** for robust data access and transaction management.

---

## Architecture

### Clean Architecture

The solution is organized into distinct layers:

- **Domain**: Contains core business entities and enumerations.
- **Application**: Holds business logic, DTOs, models, and application exceptions.
- **Persistence**: Implements data access, repositories, configurations, and the database context.
- **API**: Exposes endpoints, configures middleware, and handles dependency injection.

This structure enforces clear boundaries and dependency rules, making the codebase easy to maintain and extend.

### Patterns Used

- **Repository Pattern**: Abstracts data access logic, providing a clean interface for querying and persisting domain entities.
- **Unit of Work Pattern**: Coordinates the work of multiple repositories by managing transactions, ensuring data consistency.

---

## Features

- JWT-based authentication and authorization
- Role-based access control with ASP.NET Core Identity
- Swagger/OpenAPI documentation
- Centralized exception handling
- Serilog-based request logging
- CORS support

---
