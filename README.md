
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

### Clone repository using any kind of terminal

```bash
git clone https://github.com/perfectString/NexusWeb.git
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
### Another way of running my app:

Im going to provide detailed instructions for running my app with the Visual Studio terminal.
1. Open visual studio and click on clone a repo.
<img width="410" height="169" alt="image" src="https://github.com/user-attachments/assets/f82c1b4f-2789-4015-a077-9bd4a6c61ab4" />

and put this repository location: 
```
https://github.com/perfectString/NexusWeb.git
```
2. Open the solution and find the Web folder. Inside right click on Nexus and open a new terminal 
<img width="406" height="473" alt="image" src="https://github.com/user-attachments/assets/58a8053b-c931-4e12-86a2-05351acb9cb3" />
<img width="503" height="311" alt="image" src="https://github.com/user-attachments/assets/3e35b42c-92c3-4495-8224-77825556d7c3" />

developer powershell will open and the next step will be to restore the packages. 

3.Restore packages using:

```
dotnet restore
```

4. After the nugget packages are restored you can update the database using
```
dotnet ef database update
```
or in the package manager console

```
Update-Database
```

### Using my app: 

After you restored the packages its time to run the app. 
Apply any corrections to the connection string if you need to (you can find more info about this if you scroll down on my readme file).

The application runs at:

https://localhost:7197

or

http://localhost:5014

---

Start the application and open your browser and you should see the home page. 
<img width="1857" height="932" alt="image" src="https://github.com/user-attachments/assets/f3e0ad68-a843-4f8d-9eef-03d4010f5aa6" />

When you are ready you will be taken to your profile and you will be able to customize it:
<img width="1872" height="935" alt="image" src="https://github.com/user-attachments/assets/288f0ab1-e117-47f0-acc7-37fb19c1715d" />

When you are ready you can take a look through the existing users in the database:
<img width="1873" height="931" alt="image" src="https://github.com/user-attachments/assets/1ab56435-fdae-4f7e-8723-782ddcf9db62" />

Feel free to explore, create, join quests!
<img width="1848" height="927" alt="image" src="https://github.com/user-attachments/assets/e5b4e83f-a73a-491e-b7bd-6cc70eafeb71" />

In the future an xp sistem will be implemented where you will be able to gain exp through completing quests.
Also you will be able to explore quests based on your interests!

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

