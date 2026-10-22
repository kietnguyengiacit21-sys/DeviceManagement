# Device Management Software
ASP.NET Core MVC + Entity Framework Core + SQLite.

## Run
1. Install .NET 8 SDK.
2. Open this folder in VS Code.
3. Run `dotnet restore`, then `dotnet run`.
4. Open the localhost URL shown in terminal.

The SQLite database is created automatically on first run.

## Features
- CRUD for categories, devices and employees.
- Device name/code search and category/status filters.
- User search by name/email.
- Validation for required fields, email, phone, device code and duplicate category/device/user values.
- Category/user deletion protection when records are still referenced.

## Structure
`Models` contains entities and status; `Data` contains EF Core context; `Controllers` handle requests; `Views` contain Razor pages; `wwwroot` contains styling.
