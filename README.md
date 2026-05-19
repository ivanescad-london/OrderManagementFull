# 📦 Order Management Full Stack

A full-stack order management system built with **ASP.NET Core (.NET 9)** for the backend and **Blazor Web App** for the frontend, containerised with Docker and deployed to **Microsoft Azure**.

🌐 **Live Demo**: https://ordersystem-ui.purpleriver-b71e199a.uksouth.azurecontainerapps.io

---

## 🚀 Features

- Full CRUD operations for Clients, Suppliers, Goods, and Orders
- Relational data model with proper entity relationships
- DTO pattern for clean API contracts
- Repository + Service layer architecture
- Entity Framework Core with SQL Server
- Automatic database creation and seeding
- OpenAPI + Swagger UI for backend testing
- Blazor Web App frontend with interactive Server rendering
- Generic service pattern for reusable frontend data access
- Dockerised with multi-container setup
- Deployed to Azure Container Apps with Azure SQL

---

## 🧱 Architecture

### Application layers
```
Blazor Pages → GenericService<T> → HttpClient → ASP.NET Core API → Repositories → EF Core → SQL Server
```

### Deployment architecture
```
Azure Container Apps Environment
├── ordersystem-ui    (Blazor frontend  — public)
└── ordersystem-api   (ASP.NET Core API — internal)
        ↓
Azure SQL Server (North Europe)
        ↓
Azure Container Registry (Docker images)
```

---

## 🛠 Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core (.NET 9), C# |
| Frontend | Blazor Web App (.NET 9), C# |
| Database | SQL Server / Azure SQL |
| ORM | Entity Framework Core |
| API Docs | OpenAPI / Swagger |
| Containerisation | Docker, Docker Compose |
| Cloud | Azure Container Apps, Azure Container Registry, Azure SQL |

---

## 📁 Project Structure

```
OrderManagementFull/
├── docker-compose.yml         ← local development (all 3 services)
├── OrderSystem/               ← Backend API
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   └── Dockerfile
└── OrderSystem.UI/            ← Blazor Frontend
    ├── Components/
    │   ├── Pages/
    │   │   ├── Goods/
    │   │   ├── Clients/
    │   │   ├── Suppliers/
    │   │   └── Orders/
    │   └── Layout/
    ├── Models/
    ├── Services/
    ├── Program.cs
    └── Dockerfile
```

---

## ▶️ Run Locally with Docker

### Prerequisites
- Docker Desktop
- Git

### Steps

```bash
# Clone the repository
git clone https://github.com/ivanescad-london/OrderManagementFull.git
cd OrderManagementFull

# Start all 3 containers
docker compose up --build
```

The app will be available at:
- **Blazor UI** → http://localhost:5000
- **Swagger UI** → http://localhost:7296/swagger

Docker starts the services in the correct order: **DB → API → UI**. The database is automatically created and seeded on first run.

---

## ▶️ Run Locally with Visual Studio

### Prerequisites
- Visual Studio 2022
- .NET 9 SDK
- SQL Server / LocalDB

### Steps

1. Clone the repository
2. Open `OrderManagementFull.sln`
3. Right-click Solution → **Set Startup Projects** → set both `OrderSystem` and `OrderSystem.UI` to **Start**
4. Press **F5**

---

## ☁️ Deploy to Azure

### Prerequisites
- Azure CLI installed (`winget install Microsoft.AzureCLI`)
- Docker Desktop
- Azure account

### Step 1: Login and set up resources

```bash
az login

az group create --name OrderManagementRG --location uksouth

az acr create --resource-group OrderManagementRG \
  --name ordermgmtregistry --sku Basic --admin-enabled true

az provider register --namespace Microsoft.App
az provider register --namespace Microsoft.OperationalInsights

az containerapp env create \
  --name OrderManagementEnv \
  --resource-group OrderManagementRG \
  --location uksouth
```

### Step 2: Create Azure SQL

```bash
az sql server create \
  --name ordermgmt-sqlserver \
  --resource-group OrderManagementRG \
  --location northeurope \
  --admin-user sqladmin \
  --admin-password <your-password>

az sql db create \
  --resource-group OrderManagementRG \
  --server ordermgmt-sqlserver \
  --name OrderSystemDb \
  --service-objective Basic

az sql server firewall-rule create \
  --resource-group OrderManagementRG \
  --server ordermgmt-sqlserver \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### Step 3: Build and push Docker images

```bash
az acr login --name ordermgmtregistry

docker build -t ordermgmtregistry.azurecr.io/ordersystem-api:latest ./OrderSystem
docker build -t ordermgmtregistry.azurecr.io/ordersystem-ui:latest ./OrderSystem.UI

docker push ordermgmtregistry.azurecr.io/ordersystem-api:latest
docker push ordermgmtregistry.azurecr.io/ordersystem-ui:latest
```

### Step 4: Deploy Container Apps

```bash
# Deploy API (internal)
az containerapp create \
  --name ordersystem-api \
  --resource-group OrderManagementRG \
  --environment OrderManagementEnv \
  --image ordermgmtregistry.azurecr.io/ordersystem-api:latest \
  --registry-server ordermgmtregistry.azurecr.io \
  --registry-username ordermgmtregistry \
  --registry-password <your-registry-password> \
  --target-port 8080 \
  --ingress internal \
  --env-vars 'ConnectionStrings__DefaultConnection=Server=ordermgmt-sqlserver.database.windows.net;Database=OrderSystemDb;User=sqladmin;Password=<your-password>;TrustServerCertificate=True;' \
             'ASPNETCORE_ENVIRONMENT=Production'

# Get API internal URL
az containerapp show \
  --name ordersystem-api \
  --resource-group OrderManagementRG \
  --query properties.configuration.ingress.fqdn \
  --output tsv

# Deploy UI (public)
az containerapp create \
  --name ordersystem-ui \
  --resource-group OrderManagementRG \
  --environment OrderManagementEnv \
  --image ordermgmtregistry.azurecr.io/ordersystem-ui:latest \
  --registry-server ordermgmtregistry.azurecr.io \
  --registry-username ordermgmtregistry \
  --registry-password <your-registry-password> \
  --target-port 8080 \
  --ingress external \
  --env-vars 'ASPNETCORE_ENVIRONMENT=Production' \
             'BackendUrl=http://<api-internal-url>'
```

---

## 📊 API Endpoints

All four controllers follow the same pattern:

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/{entity}/GetAll` | Get all records |
| GET | `/api/{entity}/Read/{id}` | Get by ID |
| POST | `/api/{entity}/Create` | Create new record |
| PUT | `/api/{entity}/Update/{id}` | Update existing record |
| DELETE | `/api/{entity}/Delete/{id}` | Delete record |

Entities: `Goods`, `Clients`, `Suppliers`, `Orders`

---

## 🧪 Seed Data

On first run the application automatically creates:

- 4 Clients
- 3 Suppliers
- 3 Goods
- 6 Orders

---

## 🎯 Purpose

This project demonstrates:

- Building RESTful APIs with ASP.NET Core
- Building interactive UIs with Blazor Web App (C# instead of JavaScript)
- Clean architecture across frontend and backend
- Generic programming patterns for reusable service layers
- Working with relational data using EF Core
- Containerising a multi-service application with Docker
- Deploying to Azure Container Apps with Azure SQL
- Connecting services via internal Docker and Azure networks

---

## 📌 Future Improvements

- Pagination and filtering
- JWT authentication
- Unit testing
- CI/CD pipeline (GitHub Actions)
- Custom domain name
- HTTPS for internal API communication

---

## 👤 Author

Ivan Simeonov
🔗 https://github.com/ivanescad-london
