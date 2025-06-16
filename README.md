📦 backend/README.md – ASP.NET Core Web API

# AutoSelect – Backend

This is the backend for the AutoSelect system, built using **ASP.NET Core Web API** and **Entity Framework Core**.  
It manages mechanics, clients, cars, tasks, parts, and reports related to car service operations.

## 🛠️ Technologies

- ASP.NET Core 8
- Entity Framework Core (Code First)
- MariaDB / MySQL
- RESTful API
- Swagger (OpenAPI)
- Planned: ASP.NET Core Identity (authentication & authorization)

## 📁 Project Structure
```
AutoSelect.Backend/
├── Controllers/
├── Models/
├── DTOs/
├── Data/
├── Migrations/
├── Program.cs
└── appsettings.json
```

## 📦 Database

The database includes the following tables:

- `Mechanics`
- `Clients`
- `Cars`
- `Tasks`
- `Parts`
- `Reports`

> You can find the database schema and sample data in [`docs/database.sql`](../docs/database.sql)

## ▶️ Running the API

1. Configure your `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;port=3306;database=AutoSelect;user=root;password=yourpassword"
   }

    Run database migrations:

dotnet ef database update

Run the project:

    dotnet run

The API will be available at https://localhost:5001 (or http://localhost:5000).
🔍 Swagger

Once the project is running, access the Swagger UI at:

https://localhost:5001/swagger
