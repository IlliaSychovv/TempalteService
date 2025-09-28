# Templater - HTML Template Engine & PDF Generator

A powerful web application for creating, managing, and generating PDF documents from HTML templates with dynamic data substitution.


## Architecture

The project is built following Clean Architecture principles and is divided into layers:

**Domain** — domain entities, business rules, and domain exceptions.  
**Application** — business logic, services, interfaces, DTOs, validation, and use cases.  
**Infrastructure** — repository implementations, database access (Entity Framework Core), migrations, and external services.  
**Web** — ASP.NET Core Web API, controllers, middleware, dependency injection, and Swagger documentation.

## Main Technologies and Tools

**.NET 8** — modern development platform  
**Entity Framework Core** — ORM for database access with PostgreSQL  
**RazorLight** — template rendering engine for Razor syntax  
**iText7** — professional PDF generation library  
**Mapster** — DTO ↔ Entity mapping  
**FluentValidation** — DTO validation  
**SequentialGuid** — GUID generation for entities  
**Swagger** — auto-generated API documentation  
**xUnit, Moq, Shouldly** — unit testing framework and mocking  
**Docker** — containerization and deployment  
**PostgreSQL** — primary database  
**React 19** — modern UI library with TypeScript  
**Vite** — fast build tool and dev server  

## Features

**Template CRUD operations** — create, read, update, delete HTML templates  
**PDF generation** — convert HTML templates to PDF with dynamic data substitution  
**Razor template rendering** — support for Razor syntax with `@Model["key"]` placeholders  
**Advanced validation** — HTML structure and placeholder syntax validation  
**Pagination support** — efficient template listing with pagination  
**Global exception handling** — centralized error handling and logging  
**Swagger documentation** — interactive API documentation  
**Unit test coverage** — comprehensive testing with mocks  
**Docker containerization** — easy deployment and development setup  
**PostgreSQL database** — reliable data storage with Entity Framework Core

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started) (optional)

### Quick Start with Docker

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Templater
   ```

2. **Start services with Docker**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - API: http://localhost:5135
   - Swagger: http://localhost:5135/swagger

4. **Run the frontend**
   ```bash
   cd C:\Users\Admin\RiderProjects\Templater\my-templates-app
   npm install
   npm run dev
   ```

### Manual Setup

1. **Setup Database**
   ```bash
   # Create PostgreSQL database
   createdb templater
   
   # Run migrations
   cd C:\Users\Admin\RiderProjects\Templater
   dotnet ef migrations add (MigrationName) -p Templater.Infrastructure -s Templater.Web
   dotnet ef database update -p Templater.Infrastructure -s Templater.Web
   ```

2. **Run Backend**
   ```bash
   dotnet run
   ```

3. **Run Frontend**
   ```bash
   cd C:\Users\Admin\RiderProjects\Templater\my-templates-app
   npm install
   npm run dev
   ```

## API Documentation

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/v1/templater` | Get paginated templates list |
| `GET` | `/api/v1/templater/{id}` | Get template by ID |
| `POST` | `/api/v1/templater` | Create new template |
| `PATCH` | `/api/v1/templater/{id}` | Update template |
| `DELETE` | `/api/v1/templater/{id}` | Delete template |
| `POST` | `/api/v1/templater/{id}/generate` | Generate PDF from template |

### Usage Examples
**Request/Response Examples**

**Create Template**
```http
POST /api/v1/templater
Content-Type: application/json

{
  "name": "Certificate",
  "htmlContent": "<div style='font-family:Times New Roman, serif; text-align:center; padding:50px; border:5px solid gold; background:#f9f9f9;'>
  <h1 style='color:#8b4513; font-size:36px; margin:20px 0;'>CERTIFICATE</h1>
  <p style='font-size:18px; margin:30px 0;'>This is to certify that</p>
  <h2 style='color:#2c3e50; font-size:28px; margin:20px 0;'>@Model["StudentName"]</h2>
  <p style='font-size:16px; margin:20px 0;'>has successfully completed the course</p>
  <h3 style='color:#34495e; font-size:24px; margin:20px 0;'>@Model["CourseName"]</h3>
  <p style='font-size:14px; margin-top:40px;'>Date of issue: @Model["IssueDate"]</p>
</div>"
}
```

**Generate PDF**
```http
POST /api/v1/templater/{id}/generate
Content-Type: application/json

{
  "data": {
    "StudentName": "John Doe",
    "CourseName": "Microservices",
    "IssueDate": "2025-09-28"
  }
}
```

## Development

### Project Structure
```
Templater/
├── Templater.Domain/          # Domain entities
├── Templater.Application/     # Business logic, services, DTOs
├── Templater.Infrastructure/  # Data access, repositories
├── Templater.Web/            # Web API controllers
├── Templater.Tests/          # Unit tests
└── my-templates-app/         # React frontend
```

### Running Tests
```bash
dotnet test
```

### Database Migrations
```bash
# Add migration
dotnet ef migrations add MigrationName --project Templater.Infrastructure

# Update database
dotnet ef database update --project Templater.Infrastructure
```

## Docker Setup

### Services
- **PostgreSQL**: Database server on port 5432
- **API**: .NET application on port 5135

### Commands
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs api
docker-compose logs postgres

# Stop services
docker-compose down

# Rebuild and restart
docker-compose up -d --build
```
