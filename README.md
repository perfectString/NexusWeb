
#  Nexus

 Nexus is a social web application that connects people through shared interests by allowing users to create, join, and manage real-life quests.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

##  Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [License](#license)
- [Contact](#contact)

---

##  About the Project

Nexus is an ASP.NET Core MVC web application that allows registered users to create, discover, and join quests based on shared interests.
The application was built for people looking to expand their connections based on their current interests or find new interests through participating in quest.

This project demonstrates layered architecture using Controllers, Services, ViewModels, Entity Framework Core, and ASP.NET Identity.

---

##  Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 8.0      | Web framework                    |
| Entity Framework Core | 8.0      | ORM / Database access            |
| SQL Server / SQLite   | -        | Database                         |
| Bootstrap             | 5.3      | Frontend styling                 |
| Razor Pages / Views   | -        | Server-side HTML rendering       |

---

##  Prerequisites

Install the following tools:

- .NET SDK 8.0  
  https://dotnet.microsoft.com/download

- Visual Studio 2022  
  https://visualstudio.microsoft.com/downloads/

- SQL Server Express

  https://www.microsoft.com/sql-server/sql-server-downloads

- SQL Server Management Studio (optional)  
  https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms

- Git  
  https://git-scm.com/downloads

---

##  Getting Started

### Clone repository

```bash
git clone https://github.com/perfectString/NexusWeb.git
cd Nexus
````

### Restore packages

```bash
dotnet restore
```

### Apply migrations

```bash
dotnet ef database update
```

### Run the application

```bash
dotnet run
```

The application runs at:

https://localhost:7197

or

http://localhost:5014

---

## Project Structure

```
Nexus
│
├── Data/
│ ├── Nexus.Data/ # Database access layer
│ │ ├── Configuration/ # Entity configurations
│ │ ├── Migrations/ # Entity Framework migrations
│ │ └── NexusDbContext.cs # Database context
│ │
│ ├── Nexus.Data.Models/ # Database entities
│ │ ├── Enums/ # Application enums
│ │ ├── Profile.cs
│ │ ├── Quest.cs
│ │ ├── QuestJoiner.cs
│ │ ├── ProfileInterests.cs
│ │ └── Interest.cs
│
├── Services/
│ └── Nexus.Data.Services.Core/ # Business logic layer
│ ├── QuestService.cs
│ ├── ProfileService.cs
│ └── Interfaces/
│
├── Web/
│ └── Nexus/ # Presentation layer (ASP.NET Core MVC)
│ ├── Controllers/
│ ├── Views/
│ ├── wwwroot/
│ └── Program.cs
│
├── Nexus.ViewModels/ # ViewModels for MVC
│
├── Nexus.Common/ # Shared constants and helpers
│ └── ValidationConstants.cs
│
└── Nexus.sln

---
```

##  Features

Users can:

- Register, login and logout
- Manage their profile and interests
- See other registered user's profiles
- Create quests
- Join and view existing quests
- Edit and delete quests (if they are the initiator)
- View joined quests

---

##  Database Setup

Uses Entity Framework Core Code-First approach.

Connection string located in:

```
appsettings.json
```


Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=NexusDb;Trusted_Connection=True;Encrypt=False"
}
```

Entity Framework documentation:
[https://learn.microsoft.com/ef/core/get-started](https://learn.microsoft.com/ef/core/get-started)

---

##  Configuration

Configuration file:

```
appsettings.json
```

Documentation:
[https://learn.microsoft.com/aspnet/core/fundamentals/configuration](https://learn.microsoft.com/aspnet/core/fundamentals/configuration)

---

##  License

This project uses the MIT License.

MIT License documentation:
[https://opensource.org/licenses/MIT](https://opensource.org/licenses/MIT)

---

##  Contact

GitHub profile:
[https://github.com/perfectString (https://github.com/perfectString)

Project repository:
[https://github.com/perfectString/NexusWeb](https://github.com/perfectString/NexusWeb)

