# CNAB Microservices — DDD + CQRS + SOLID (MassTransit, PostgreSQL, Angular)

Project with 3 microservices:
- **api-cnab**: exposes endpoints (CNAB upload + store/balance queries); publishes MassTransit messages.
- **consumer-cnab**: consumes messages, parses and saves to PostgreSQL via EF Core.
- **client-cnab**: Angular (upload and store/balance listing).

## Prerequisites

Before running the project, make sure you have installed:

1. **Docker Desktop** (version 20.10 or higher)
   - Download: https://www.docker.com/products/docker-desktop
   - Make sure Docker Desktop is running

2. **Git** (to clone the repository)
   - Download: https://git-scm.com/downloads

## Step-by-Step Setup Guide

### 1. Clone the Repository
```bash
git clone https://github.com/danillofratta/test-bycoders-microservices-cnab.git
cd test-bycoders-microservices-cnab
```

### 2. Clean Previous Docker Data (if needed)
If you've run the project before, clean up to ensure fresh initialization:
```bash
docker compose down
docker system prune -f
docker volume prune -f
```

### 3. Run the Complete Stack
Execute the following command to build and start all services:
```bash
docker compose up --build
```

### 4. Wait for Services to Start
The startup process includes:
- ✅ **PostgreSQL**: Database initialization with tables creation
- ✅ **RabbitMQ**: Message broker setup
- ✅ **API (.NET)**: CNAB processing API with Swagger
- ✅ **Consumer (.NET)**: Background message processor
- ✅ **Client (Angular)**: Web interface

**Expected output:**
```
✔ Container postgres-container                           Healthy
✔ Container rabbitmq                                     Healthy  
✔ Container test-bycoders-microservices-cnab-api-1       Started
✔ Container test-bycoders-microservices-cnab-consumer-1  Started
✔ Container test-bycoders-microservices-cnab-client-1    Started
```

### 5. Verify Services Are Running
Check all containers are healthy:
```bash
docker compose ps
```

### 6. Access the Applications

Once all services are running, access:

- **📱 Web App**: http://localhost:4201
- **📋 API Documentation**: http://localhost:8080/swagger
- **🐰 RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **🗄️ PostgreSQL**: localhost:5432 (admin/root)

## Usage

### Upload CNAB Files
1. Go to http://localhost:4201/upload
2. Select a `.txt` or `.cnab` file
3. Click "📤 Enviar Arquivo"
4. Monitor the upload progress

### View Transactions
- Navigate to the transactions page to see processed CNAB data

### View Stores
- Check store balances and transaction summaries

## Troubleshooting

### If containers fail to start:
```bash
# Stop everything
docker compose down

# Clean up
docker system prune -f
docker volume prune -f

# Restart
docker compose up --build
```

### If database tables are missing:
```bash
# Remove volumes and restart
docker compose down -v
docker compose up --build
```

### Common Issues:
- **Port conflicts**: Make sure ports 4201, 8080, 5432, 5672, and 15672 are not in use
- **Memory issues**: Ensure Docker has at least 4GB RAM allocated
- **Network issues**: Check Docker Desktop network settings

## Architecture Overview

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Client        │    │   API           │    │   Consumer      │
│   (Angular)     │◄──►│   (.NET 8)      │◄──►│   (.NET 8)      │
│   Port: 4201    │    │   Port: 8080    │    │   Background    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │              ┌─────────────────┐              │
         └──────────────►│   RabbitMQ      │◄─────────────┘
                        │   Port: 5672    │
                        └─────────────────┘
                                 │
                        ┌─────────────────┐
                        │   PostgreSQL    │
                        │   Port: 5432    │
                        └─────────────────┘
```

## Tests + Coverage (≥ 80%)
```bash
dotnet test api-cnab/test/api-cnab-tests/api-cnab-tests.csproj
dotnet test consumer-cnab/test/consumer-cnab-tests/consumer-cnab-tests.csproj
```
The test projects are configured with **coverlet.collector** and `Threshold=80` (line coverage).

## Project Structure
```
├── api-cnab/                          # API Microservice (.NET 8)
│   ├── src/app/                        # Core application layers
│   │   ├── Cnab.Api.Domain/           # Domain entities and interfaces
│   │   ├── Cnab.Api.Application/      # Use cases and handlers (CQRS)
│   │   └── Cnab.Api.Presentation/     # Controllers and API endpoints
│   ├── src/external/                   # External infrastructure
│   │   ├── Cnab.Api.Infrastructure.Persistence/   # EF Core & repositories
│   │   └── Cnab.Api.Infrastructure.Messaging/     # MassTransit setup
│   └── test/                           # Unit tests with xUnit
├── consumer-cnab/                      # Consumer Microservice (.NET 8)
│   ├── src/app/                        # Core application layers  
│   │   ├── Cnab.Consumer.Domain/      # Domain entities and services
│   │   ├── Cnab.Consumer.Application/ # CNAB processing handlers
│   │   └── Cnab.Consumer.Worker/      # Background service host
│   ├── src/external/                   # External infrastructure
│   │   ├── Cnab.Consumer.Infrastructure.Persistence/  # EF Core setup
│   │   └── Cnab.Consumer.Infrastructure.Messaging/    # MassTransit consumers
│   └── test/                           # Unit tests with xUnit
├── client-cnab/                        # Frontend (Angular 17)
│   ├── src/app/                        # Angular components
│   │   ├── pages/dashboard/            # Main application pages
│   │   └── stores/                     # Store management components  
│   └── src/domain/                     # API services and DTOs
├── scripts/                            # Database initialization scripts
└── docker-compose.yml                 # Container orchestration
```

## Technologies Stack

### Backend (.NET 8)
- **Framework**: ASP.NET Core 8.0
- **Architecture**: Domain-Driven Design (DDD) + CQRS + Clean Architecture
- **Database**: PostgreSQL 15 with Entity Framework Core
- **Messaging**: RabbitMQ 4 with MassTransit
- **Testing**: xUnit + FluentAssertions + Coverlet (≥80% coverage)

### Frontend (Angular 17)
- **Framework**: Angular 17.3.0 (Standalone Components)
- **Styling**: Custom CSS with responsive design
- **HTTP Client**: Angular HttpClient with progress tracking
- **Routing**: Angular Router with lazy loading

### Infrastructure
- **Containerization**: Docker + Docker Compose
- **Database**: PostgreSQL 15 with automatic schema creation
- **Message Broker**: RabbitMQ 4 with management interface
- **Reverse Proxy**: Nginx (for Angular SPA)

### Development Tools
- **Package Management**: NuGet (.NET) + npm (Angular)
- **Code Quality**: EditorConfig, ESLint, Coverlet
- **API Documentation**: Swagger/OpenAPI 3.0
