# SignalRChatApp

A real-time chat application built with **ASP.NET Core 10** using SignalR for WebSocket-based messaging, Entity Framework Core for data persistence, and ASP.NET Core Identity for authentication.

## Features

- **User Authentication** — Register, login, and logout with ASP.NET Core Identity (email-based).
- **1:1 Private Chat** — Send and receive real-time messages between users via SignalR.
- **Group Chat** — Create groups, add members, and send group messages in real time.
- **Online Tracking** — Tracks user connections via `UserConnection` entities.
- **Unread Message Tracking** — Infrastructure for unread message counts.

## Tech Stack

| Technology | Purpose |
|---|---|
| .NET 10 / ASP.NET Core MVC | Web framework |
| SignalR | Real-time WebSocket communication |
| Entity Framework Core 10 | ORM & data access |
| SQL Server (LocalDB) | Database |
| ASP.NET Core Identity | Authentication & user management |
| Razor Views | Server-side rendering |
| jQuery + SignalR JS Client | Client-side interactivity |

## Project Structure

```
SignalRChatApp/
├── Contracts/           # Interfaces & DTOs
│   ├── IAuthService.cs
│   ├── IChatService.cs
│   ├── IGroupService.cs
│   ├── AuthResult.cs
│   ├── ChatMessageDto.cs
│   ├── GroupDto.cs
│   ├── UnreadCountDto.cs
│   └── UserDto.cs
├── Controllers/         # MVC Controllers
│   ├── AuthController.cs
│   ├── ChatController.cs
│   ├── GroupsController.cs
│   └── HomeController.cs
├── Data/                # EF Core DbContext & configurations
│   ├── AppDbContext.cs
│   └── Configurations/
├── Hubs/                # SignalR Hub
│   └── ChatHub.cs
├── Migrations/          # EF Core migrations
├── Models/              # Domain entities
│   ├── AppUser.cs
│   ├── ChatMessage.cs
│   ├── Group.cs
│   ├── UserConnection.cs
│   ├── UserGroup.cs
│   └── ErrorViewModel.cs
├── Services/            # Business logic implementations
│   ├── AuthService.cs
│   ├── ChatService.cs
│   └── GroupService.cs
├── ViewModel/           # View models
│   └── AuthViewModel.cs
├── Views/               # Razor views
│   ├── Auth/            # Login, Register
│   ├── Chat/            # Main chat interface
│   ├── Groups/          # Group management
│   ├── Home/            # Landing & privacy pages
│   └── Shared/          # Layout, error, validation partials
├── wwwroot/             # Static assets
│   ├── css/             # Stylesheets
│   ├── js/              # SignalR client scripts
│   └── lib/             # jQuery & validation libraries
├── Program.cs           # Application entry point & DI setup
├── appsettings.json     # Configuration (connection string, etc.)
└── SignalRChatApp.csproj
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server LocalDB (or any SQL Server instance)

### Setup

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd SignalRChatApp
   ```

2. **Update the connection string** in `appsettings.json` if needed:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SignalRChatApp;Integrated Security=True;..."
   }
   ```

3. **Apply database migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

5. Navigate to `https://localhost:5001` (or the URL shown in the console).

## Usage

1. **Register** a new account via `/Auth/Register`.
2. **Login** at `/Auth/Login`.
3. **Chat one-on-one** — Select a user from the sidebar to start a private conversation.
4. **Create a group** at `/Groups/Create`.
5. **Add members** to your group via `/Groups/AddMember/:id`.
6. **Send group messages** — Join a group conversation from the chat interface.

## SignalR Hub Endpoints

| Method | Description |
|---|---|
| `SendMessageToUser(userId, content)` | Send a private message |
| `SendMessageToGroup(groupId, content)` | Send a group message |
| `JoinGroup(groupId)` | Join a SignalR group for real-time updates |
| `LeaveGroup(groupId)` | Leave a SignalR group |

Client events: `ReceiveMessage`, `ReceiveGroupMessage`.

## Database Schema

| Table | Description |
|---|---|
| `AspNetUsers` | User accounts (Identity) |
| `ChatMessages` | 1:1 and group messages |
| `Groups` | Chat groups |
| `UserGroups` | Many-to-many user-group membership |
| `UserConnections` | Active SignalR connection tracking |
