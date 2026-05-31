# Library Management System

Desktop application for managing a small library, built with C#, Windows Forms and SQLite.

## Features

- Manage books and readers
- Borrow and return books
- Reserve borrowed books
- FIFO reservation queue
- Search and filter records
- Input validation
- Prevent deleting records with active loans or reservations
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
- `Models/` - data models
- `Data/` - database setup
- `Repositories/` - database access
- `Services/` - business logic
- `Knihovna.Tests/` - unit tests

## Database

The application uses a local SQLite database with these main tables:

- `Knihy`
- `Ctenari`
- `Vypujcky`
- `Rezervace`

The database is created automatically when the application runs.

## Testing

The project includes unit tests for validation, borrowing, returning, reservations, reservation queue logic and delete restrictions.

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.