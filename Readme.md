## TodoApp C#.NET Core

A simple program to demonstrate a feature-rich To-do application.

#### Packages used
- EF Core
- Microsoft.Asp.Identity to handle User Management

## Requirements

- SQLServer LocalDB/Express/Developer
- .NET Core 6 (LTS)

### Apply migration first using [EF CORE CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet "EF CORE CLI"):

- `dotnet tool install --global dotnet-ef` to install the CLI
- configure database connection in appsettings.json
- `dotnet ef database update` to apply the tables in database.

### Run using `dotnet run`

