# Library Management System

Desktop application for managing a small library, built with C#, Windows Forms and SQLite.

The application allows users to manage books, readers, loans, returns and reservations.

## Features

- Add, edit and delete books
- Add, edit and delete readers
- Borrow and return books
- Reserve borrowed books
- Reservation queue
- Search and filter records
- Input validation
- Local SQLite database storage
- Unit-tested business logic

## Technologies

- C#
- .NET 8
- Windows Forms
- SQLite
- Microsoft.Data.Sqlite
- Repository pattern
- Service layer
- MSTest

## Project Structure

- `Forms/` - user interface
- `Models/` - application models
- `Data/` - database setup
- `Repositories/` - database access
- `Services/` - business logic
- `Knihovna.Tests/` - unit tests

## Database

The application uses a local SQLite database.

The database stores books, readers, loans and reservations.

## Testing

The project uses MSTest for unit testing the main business logic.

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.