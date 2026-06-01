# Library Management System

Desktop application for managing books, readers, loans and reservations, built with C#, Windows Forms and SQLite.

The project includes local database storage, layered application structure, repository classes, input validation, reservation rules and unit-tested business logic.

## Highlights

- Layered structure with separated UI, business logic and database access
- SQLite database instead of file-based storage
- Repository pattern for database operations
- Service layer for main business rules
- Unit tests for core business logic
- Reservation queue with loan and reservation limits

## Features

- Add, edit and delete books
- Add, edit and delete readers
- Borrow and return books
- Reserve borrowed books
- Search and filter books
- Search readers
- Input validation for books and readers
- Limit of 5 active loans per reader
- Limit of 5 active reservations per reader
- Prevent deleting books with active loans or reservations
- Prevent deleting readers with active loans or reservations
- Automatic local SQLite database creation

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

- `Forms/` - Windows Forms user interface
- `Models/` - application models
- `Data/` - database connection and setup
- `Repositories/` - database access and CRUD operations
- `Services/` - main application logic
- `Knihovna.Tests/` - unit tests and fake repositories

## Database

The application uses a local SQLite database.

The database stores books, readers, loans and reservations.  
It is created automatically when the application runs.

## Application Logic

The project separates the user interface, business logic and database access.

Main rules such as borrowing, returning, reservations, validation, limits and delete restrictions are handled in the service layer.

Database operations are handled through repository classes.

## Testing

The project uses MSTest for unit testing the main business logic.

Tests cover validation, borrowing, returning, reservations, reservation queue logic, active loan limits, active reservation limits and delete restrictions.

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.