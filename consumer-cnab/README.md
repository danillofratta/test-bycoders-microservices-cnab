# consumer-cnab

- **Worker** com MassTransit consumer (`cnab.lines`) → CQRS Command → persiste no PostgreSQL.
- **EF Core** com migrations (`InitialCreate`) aplicadas no startup (`Database.Migrate()`).

## Testes
```bash
dotnet test test/consumer-cnab-tests/consumer-cnab-tests.csproj
```
- `Threshold=80` (coverlet).
