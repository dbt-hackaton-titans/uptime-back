# Uptime Monitor Backend

This repository contains the backend for the Uptime Monitor project, built with ASP.NET Core (.NET 8) and designed to track, monitor, and report on the uptime status of systems and components. It provides RESTful APIs, real-time updates via SignalR, background monitoring services, email notifications, and a web-based health check endpoint.

## Features

- System and component uptime monitoring
- Real-time status updates with SignalR hubs
- Email notifications (SMTP configurable)
- Health check endpoints (`/healthz`)
- Swagger/OpenAPI documentation (development mode)
- SQLite database support (default, configurable)
- Background monitoring service
- Dockerized for easy deployment

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Docker (optional, for containerized deployment)
- SQLite (default) or alternative database (configurable)
- SMTP credentials for email notifications

### Configuration

Configuration files are located in `configuration/appsettings.json` and `configuration/serilog.json`. Sensitive data can be provided via environment variables or `secrets/`.

Example SMTP configuration:
```json
"Smtp": {
  "Host": "your-smtp-host",
  "Port": 587,
  "User": "your-smtp-user",
  "Pass": "your-smtp-pass",
  "From": "noreply@yourdomain.com"
}
```

### Running Locally

#### Via .NET CLI

```bash
dotnet build
dotnet run --project Titans.Uptime.Api
```

#### Via Docker

```bash
docker build -t uptime-back .
docker run -p 8080:8080 uptime-back
```

The API will be available at `http://localhost:8080`.

### API Documentation

When running in development mode, Swagger UI is available at `/swagger`.

### Health Checks

Health check endpoint: `GET /healthz`

### SignalR

Real-time updates are available via the `/monitoringHub` SignalR endpoint.

## Project Structure

- `Titans.Uptime.Api/` – Main API project (entry point)
- `Titans.Uptime.Application/` – Application logic and services
- `Titans.Uptime.Domain/` – Domain models and interfaces
- `Titans.Uptime.Persistence/` – Database context and migrations

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes
4. Push to the branch (`git push origin feature/my-feature`)
5. Open a Pull Request
