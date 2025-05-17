# Projects descriptions 

EventBlazorApp: Blazor app consuming EventManagerApi.
EventManagerApi: Web API for managing events and registrations, featuring JWT authentication and role-based authorization.
EventManagerApi.Tests: Test project for EventManagerApi

## Setup Instructions

1. **Clone the repository**

2. **Configure the database**
   - Update your connection string in the `EventManagerApi.Data.EventDbContext` as needed.
   - Run EF Core migrations to create the database with seeded data.

4. **Run the API**
  - Start the EventManagerApi project (has swagger ui support).

## Architecture Decisions

- **Clean separation of concerns**:  
  - `EventService` encapsulates business logic and caching.
  - `EventController` handles HTTP requests and responses.
  - `IEventDbContext` abstracts data access for easier testing and flexibility.

- **Authentication & Authorization**:  
  - JWT Bearer authentication is used for stateless, secure API access.
  - Role-based authorization restricts event creation, update, and deletion to "Admin" users.

- **Caching**:  
  - In-memory caching is implemented for event queries to improve performance for repeated requests with the same filters.

- **Validation**:  
  - Data annotations are used for model validation (e.g., string length, date in the future).

## Assumptions

- Users and roles are managed via ASP.NET Core Identity.
- The "Admin" role is required for event management (create, update, delete).
- The API is not intended for public anonymous access; all endpoints require authentication.

## Improvements Given More Time

- Add server route to get every registrations for a given user.

- Replace in-memory cache with a distributed cache (e.g., Redis) for multi-instance deployments.

- Expand test coverage, especially for edge cases and authorization scenarios.

- Improve error responses and validation feedback for clients.

- Splitting EventManagerApi as to have a microservice dedicated to registrations with a messaging bus.
