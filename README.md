# CNAB Microservices — DDD + CQRS + SOLID (MassTransit, PostgreSQL, Angular)

3 microservices:
- **api-cnab**: expõe endpoints (upload CNAB + consulta lojas/saldo); publica mensagens MassTransit.
- **consumer-cnab**: consome mensagens, parseia e grava no PostgreSQL via EF Core.
- **client-cnab**: Angular (upload e listagem de lojas/saldo).

## Subir tudo
```bash
docker compose up --build
# API Swagger: http://localhost:8080/swagger
# Client:      http://localhost:4201
# RabbitMQ:    http://localhost:15672 (guest/guest)
```


## Testes + Cobertura (≥ 80%)
```bash
dotnet test api-cnab/test/api-cnab-tests/api-cnab-tests.csproj
dotnet test consumer-cnab/test/consumer-cnab-tests/consumer-cnab-tests.csproj
```
Os projetos de teste estão com **coverlet.collector** e `Threshold=80` (line coverage).

## Estrutura
- `src/app`: Domain, Application, Presentation/Worker.
- `src/external`: Infrastructure.Persistence (EF Core), Infrastructure.Messaging (MassTransit).
- `test`: projetos de teste unitário (xUnit + FluentAssertions).

## Tecnologias
- .NET 8, EF Core + Npgsql, MassTransit + RabbitMQ, Angular 17.
