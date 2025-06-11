# AutoSelect

AutoSelect is a car service task management system developed using **ASP.NET Core Web API** with **Entity Framework Core** and **MySQL**. The system allows management of clients, cars, mechanics, tasks, parts, and reports.

> ✅ The frontend is developed separately using **React** and resides in a different Git branch or folder (`autoselect-frontend`).

---

## ✨ Features

- Manage car repair tasks and assignments
- Associate tasks with mechanics and cars
- Track availability of mechanics
- Log used parts and generate reports
- Modular backend with future support for Identity authentication

---

## ⚙️ Technologies Used

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core**
- **MySQL/MariaDB**
- **Swagger (OpenAPI)**
- (Planned) ASP.NET Core **Identity** for user roles and login

---

## 🧱 Database Schema

### `Mechanics`
| Field       | Type       | Description         |
|------------|------------|---------------------|
| Id         | int        | Primary Key         |
| Name       | longtext   | Mechanic's name     |
| IsAvailable| tinyint(1) | Availability flag   |

---

### `Clients`
| Field  | Type     | Description      |
|--------|----------|------------------|
| Id     | int      | Primary Key      |
| Name   | longtext | Client's name    |
| Phone  | longtext | Contact number   |

---

### `Cars`
| Field       | Type     | Description         |
|-------------|----------|---------------------|
| Id          | int      | Primary Key         |
| ClientId    | int      | Foreign key         |
| Brand       | longtext | Car make            |
| Model       | longtext | Car model           |
| LicensePlate| longtext | Registration number |

---

### `Tasks`
| Field       | Type        | Description            |
|-------------|-------------|------------------------|
| Id          | int         | Primary Key            |
| MechanicId  | int         | Foreign key to Mechanics |
| CarId       | int         | Foreign key to Cars    |
| Description | longtext    | Task details           |
| Status      | longtext    | e.g., Pending, Done    |
| CreatedAt   | datetime(6) | Timestamp              |

---

### `Parts`
| Field    | Type     | Description              |
|----------|----------|--------------------------|
| Id       | int      | Primary Key              |
| TaskId   | int      | Foreign key to Tasks     |
| Name     | longtext | Part name                |
| Quantity | int      | Quantity used            |

---

### `Reports`
| Field       | Type        | Description            |
|-------------|-------------|------------------------|
| Id          | int         | Primary Key            |
| TaskId      | int         | Foreign key to Tasks   |
| Description | longtext    | Report notes           |
| CompletedAt | datetime(6) | Completion timestamp   |

---

## 🧪 Sample Data Inserts

```sql
-- Mechanics
INSERT INTO Mechanics (Name, IsAvailable) VALUES
('Ivan Petrov', 1),
('Oleh Sydorenko', 0),
('Maria Kovalenko', 1);

-- Clients
INSERT INTO Clients (Name, Phone) VALUES
('Andrii Shevchenko', '+380671112233'),
('Olena Ivanova', '+380983334455');

-- Cars
INSERT INTO Cars (Id, ClientId, Brand, Model, LicensePlate) VALUES
(1, 1, 'Toyota', 'Corolla', 'AA1234BB'),
(2, 2, 'Volkswagen', 'Passat', 'BC5678CD');

-- Tasks
INSERT INTO Tasks (MechanicId, CarId, Description, Status, CreatedAt) VALUES
(1, 1, 'Oil change', 'In Progress', NOW()),
(3, 2, 'Suspension diagnostics', 'Pending', NOW());

-- Parts
INSERT INTO Parts (TaskId, Name, Quantity) VALUES
(1, 'Oil filter', 1),
(1, 'Engine oil 5W-30', 4),
(2, 'Front shock absorber', 2);

-- Reports
INSERT INTO Reports (TaskId, Description, CompletedAt) VALUES
(1, 'Oil change successfully completed', NOW()),
(2, 'Suspension issues detected', NULL);

```
---

## 📈 Planned Features

* ✅ RESTful API endpoints for each model
* 🔒 User login/registration with **ASP.NET Identity**
* 🌐 Localization support
* 📍 Interactive map for car drop-off/pickup locations
* 🔄 Frontend integration with React app

---

## 🛠️ Running the Backend

```bash
dotnet build
dotnet ef database update
dotnet run
```

> Swagger UI will be available at `https://localhost:5623/swagger`

---
```
## 📂 Structure

├── appsettings.Development.json
├── appsettings.json
├── AutoSelect.csproj
├── AutoSelect.sln
├── Controllers
│   ├── CarsController.cs
│   ├── ClientsController.cs
│   ├── MechanicsController.cs
│   ├── ReportsController.cs
│   └── TasksController.cs
├── Data
│   └── AppDbContext.cs
├── global.json
├── Models
│   ├── Car.cs
│   ├── Client.cs
│   ├── Mechanic.cs
│   ├── Part.cs
│   ├── Report.cs
│   └── Task.cs
├── Program.cs
└── Properties
    └── launchSettings.json
