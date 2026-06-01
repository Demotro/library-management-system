# Library Management System

Desktop application for managing books, readers, loans and reservations, built with C#, Windows Forms and SQLite.

The project demonstrates a complete desktop application with database storage, layered architecture, repository pattern, service layer, validation and unit-tested business logic.

## Features

- Add, edit and delete books
- Add, edit and delete readers
- Borrow and return books
- Reserve borrowed books
- Reservation queue for borrowed books
- Search and filter books
- Search readers
- Input validation for books and readers
- Limit of active loans per reader
- Limit of active reservations per reader
- Prevent deleting books with active loans or reservations
- Prevent deleting readers with active loans or reservations
- Local SQLite database storage
- Automatic database creation
- Unit-tested business logic

## Technologies

- C#
- .NET 8
- Windows Forms
- SQLite
- Microsoft.Data.Sqlite
- MSTest

## Architecture

The project is split into multiple parts:

- `Forms/` - Windows Forms user interface
- `Models/` - application models
- `Data/` - database connection and setup
- `Repositories/` - data access and CRUD operations
- `Services/` - main business logic
- `Knihovna.Tests/` - unit tests and fake repositories

The UI does not work directly with the database.  
Main application rules are handled in the service layer, while repositories are responsible for database operations.

## Database

The application uses a local SQLite database.

Main tables:

- `Knihy`
- `Ctenari`
- `Vypujcky`
- `Rezervace`

The database stores books, readers, active loans and reservations.  
It is created automatically when the application runs.

## Testing

The project uses MSTest for unit testing the main business logic.

The tests cover mainly:

- book validation
- reader validation
- borrowing books
- returning books
- reservations
- reservation queue logic
- active loan limits
- active reservation limits
- delete restrictions

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.