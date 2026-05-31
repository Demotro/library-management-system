# Library Management System

Desktop application for managing a small library, built with C#, Windows Forms and SQLite.

The project focuses on basic library operations, clean separation of logic and data access, and unit-tested business rules.

## Features

- Manage books and readers
- Borrow and return books
- Reserve already borrowed books
- FIFO reservation queue
- Search and filter books and readers
- Input validation for ISBN, e-mail, phone number and required fields
- Limit of 5 active loans per reader
- Limit of 5 active reservations per reader
- Prevent deleting records with active loans or reservations
- Local SQLite database storage

## Technologies

- C#
- .NET 8
- Windows Forms
- SQLite
- Microsoft.Data.Sqlite
- MSTest

## Architecture

The application is split into several parts:

- `Forms/` - Windows Forms user interface
- `Models/` - book, reader, loan and reservation models
- `Data/` - SQLite database connection and setup
- `Repositories/` - database access and CRUD operations
- `Services/` - main business logic
- `Knihovna.Tests/` - unit tests and fake repositories

The UI communicates with the service layer, while database operations are handled through repositories.  
This keeps the main business rules separated from the Windows Forms code.

## Database

The application uses a local SQLite database that is created automatically when the application runs.

Main database tables:

- `Knihy`
- `Ctenari`
- `Vypujcky`
- `Rezervace`

## Business Logic

Books can be borrowed only when they are available.

If a book is already borrowed, readers can create reservations for it.  
Reservations are processed in order, so the first reader in the queue has priority when the book becomes available again.

The application also prevents invalid operations, such as deleting books or readers that are still connected to active loans or reservations.

## Testing

The project includes unit tests for the main service logic, including:

- book and reader validation
- borrowing and returning books
- reservation rules
- reservation queue behavior
- loan and reservation limits
- delete restrictions

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.