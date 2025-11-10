# CNAB Microservices — DDD + CQRS + SOLID (MassTransit, PostgreSQL, Angular)

Project with 3 microservices:
- **api-cnab**: exposes endpoints (CNAB upload + store/balance queries); publishes MassTransit messages.
- **consumer-cnab**: consumes messages, parses and saves to PostgreSQL via EF Core.
- **client-cnab**: Angular (upload and store/balance listing).

## Run Everything
```bash
docker compose up --build
# API Swagger: http://localhost:8080/swagger
# Client:      http://localhost:4201
# RabbitMQ:    http://localhost:15672 (guest/guest)
```

## Structure
- `src/app`: Domain, Application, Presentation/Worker.
- `src/external`: Infrastructure.Persistence (EF Core), Infrastructure.Messaging (MassTransit).
- `test`: unit test projects (xUnit + FluentAssertions).

## Technologies
- .NET 8, EF Core + Npgsql, MassTransit + RabbitMQ, Angular 17.
