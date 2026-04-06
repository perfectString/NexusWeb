
#  Nexus

Nexus is a social web application that connects people through shared quests and interests by allowing users to create, discover, and join real-life quests, gain xp and participate in leaderboards.


![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

##  Table of Contents

- [About the Project](#about-the-project)
- [How to Use Video](#how-to-use-video)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Test Profile](#test-profie)
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

Nexus introduces a competitive layer to increase user engagement by allowing users to track their activity, earn experience points, progress through levels, and compare their progress with others. Active participation and competition create a more dynamic and motivating experience.

This project demonstrates the practical application of layered architecture and separation of concerns by organizing the system into Controllers, Services, and Data layers.
The service layer is divided into core and management services, supported by helper components for handling business logic such as level progression and reward calculation.
It utilizes ViewModels to decouple the UI from data models, Entity Framework Core for database interaction, ASP.NET Identity for authentication and user management, as well as data seeding.
The application also includes, unit testing of the services, pagination, and custom exception handling & custom exception views for improved reliability.

---
## How To Use Video


▶️ https://youtube.com/your-link

🎥🎥🎥🎥🎥🎥🎥🎥🎥
---
##  Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 8.0      | Web framework                    |
| Entity Framework Core | 8.0      | ORM / Database access            |
| SQL Server / SQLite   | -        | Database                         |
| Bootstrap             | 5.3      | Frontend styling                 |
| Razor Pages / Views   | -        | Server-side HTML rendering       |
| EFC.InMemory          | 8.0      | In-memory database for testing   |
| NUnit                 | 3.14.0   | Unit testing framework           |
| Coverlet.collector    | 6.0.0    | Code coverage collection         |
| Moq                   | 4.20.72  | Mock dependencies in unit tests  |

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

##  Test Profie
login for admin profile for testing purposes:

email: 
```bash
admin@nexus.com
````
password:
```bash
nexus1
````

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

Start the application and open your browser and you should see the home page: 
<img width="1863" height="924" alt="image" src="https://github.com/user-attachments/assets/69fe37d1-1fa3-49c2-a575-a3691590a8c1" />

Register your profile:
<img width="1872" height="918" alt="image" src="https://github.com/user-attachments/assets/c956aae5-26a3-40af-8252-8c3b048b6757" />


When you are ready you will be taken to your profile and you will be able to customize it:
<img width="1873" height="929" alt="image" src="https://github.com/user-attachments/assets/6809aaf3-a069-484d-b00a-5e54cc8aadce" />

You are going to be taken to your profile where you can see your current level and other user information:
<img width="1875" height="924" alt="image" src="https://github.com/user-attachments/assets/d289b95c-284d-4d35-bc4f-70f4910f04e7" />



When you are ready you can take a look through the existing users in the database:
<img width="1862" height="924" alt="image" src="https://github.com/user-attachments/assets/273acd18-05fe-4f7b-b3e3-2cc3712a02b2" />


Feel free to explore, create, join quests!
<img width="1866" height="928" alt="image" src="https://github.com/user-attachments/assets/de41f019-78f7-47e9-82d4-b5eeca6dad61" />

Also remember to pay close attention to the leaderboards and your level!

<img width="1868" height="931" alt="image" src="https://github.com/user-attachments/assets/2133570d-7f38-413d-b938-a37bafd2a59e" />



## Project Structure

```
Nexus
│
├── Data/
│ ├── Nexus.Data/                        # Database access layer
│ │ ├── Configuration/                   # Entity configurations
│ │ ├── Migrations/                      # EF Core migrations
│ │ ├── Seeding/                         # Data seeding logic
│ │ └── NexusDbContext.cs                # Database context
│ │
│ ├── Nexus.Data.Models/                 # Database entities
│ │                                      
│ │ ├── Profile.cs
│ │ ├── Quest.cs
│ │ ├── QuestJoiner.cs
│ │ ├── ProfileInterest.cs
│ │ ├── QuestInterest.cs
│ │ └── Interest.cs
│
├── Services/
│ ├── Nexus.Data.Services.Core/          # Core business logic
│ │ ├── Interfaces/
│ │ ├── QuestService.cs
│ │ ├── QuestManagementService.cs
│ │ ├── ProfileService.cs
│ │ ├── ProfileManagementService.cs
│ │ ├── LeaderboardService.cs            # Leaderboard & ranking logic
│ │ ├── Helpers/
│ │      ├── LevelHelper.cs                   # XP & level calculation
│ │      ├── RewardHelper.cs                  # Rewards logic
│ │      └── FindAdminHelper.cs               # Admin helper logic
│ │
├── Tests/                                 #Unit testing of the services
│ 
│ 
│
├── Web/
│ └── Nexus/                             # Presentation layer (ASP.NET Core MVC)
│ ├── Controllers/                       # Controllers of the application
│ ├── Views/                             # Views of the application
│ ├── wwwroot/
│ ├── Identity/
│ ├── Areas/
│ │     ├── Admin
│ │           ├── Controllers/                       # Admin controllers of the application
│ │           ├── Views/                             # Admin views of the application            
│ └── Program.cs
│
│ ├── Nexus.ViewModels/                   # ViewModels
│ ├── Nexus.Web.Infrastructure/           # Utilities and extensions
├── Nexus.Common/                         # Shared constants and messages
│ ├── ValidationConstants.cs
│ ├── GlobalConstants.cs
│ ├── Enums/                             # Application enums
│ ├── Exceptions/                        # Custom exceptions
│ ├── OutPutMessages.cs
│ └── ValidationConstants.cs
│
└── Nexus.sln
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
- Participate in leaderboards
- Earn xp and level
- View other users' level 

Admins can:
Admins have access to all standard user functionalities, as well as additional management features, including:
- Manage user profiles
- Modify level and xp for each user 
- Delete user profiles
- Manage quests
- Modify difficulty and state of quests
- Delete quests

future implementation: slug
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
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=NexusDbApril;Trusted_Connection=True;Encrypt=False"
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
[https://github.com/perfectString](https://github.com/perfectString)

Project repository:
[https://github.com/perfectString/NexusWeb](https://github.com/perfectString/NexusWeb)

