# api-cnab

- **DDD + CQRS**: Domain (puro), Application (Commands/Queries), Presentation (Minimal API).
- **MassTransit**: publisher para `cnab.lines`.
- **Read Model**: DbContext (`ReadDbContext`) apontando para o DB do consumer.

## Endpoints
- `POST /api/upload`: lê CNAB e publica 1 mensagem por linha.
- `GET /api/stores`: lista lojas com saldo.

## Testes
```bash
dotnet test test/api-cnab-tests/api-cnab-tests.csproj
```
- `Threshold=80` (coverlet).
