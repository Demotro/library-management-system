# Library Management System

Desktop application for managing books, readers, loans and reservations, built with C#, Windows Forms and SQLite.

The project includes database storage, separated application logic, repository classes, validation and unit tests.

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

The database stores:

- books
- readers
- loans
- reservations

The database is created automatically when the application runs.

## Application Logic

The project separates the user interface, business logic and database access into different parts.

Main rules such as borrowing, returning, reservations, validation and delete restrictions are handled in the service layer.

Database operations are handled through repository classes.

## Testing

The project uses MSTest for unit testing the main business logic.

Tests cover:

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