# Library Management System

Desktop application for managing a small library.

The application allows users to manage books, readers, loans, returns and reservations.  
It is built as a C# Windows Forms project with a local SQLite database.

## Features

- Add, edit and delete books
- Add, edit and delete readers
- Borrow and return books
- Reserve borrowed books
- Cancel reservations
- FIFO reservation queue
- Limit of 5 active loans per reader
- Limit of 5 active reservations per reader
- Search and filter books
- Search readers
- Input validation for ISBN, e-mail, phone number and required fields
- Prevent deleting books or readers with active loans or reservations
- Local SQLite database storage

## Technologies Used

- C#
- .NET 8
- Windows Forms
- SQLite
- Microsoft.Data.Sqlite
- Object-oriented programming
- Repository pattern
- Service layer
- Unit tests

## Project Structure

- `Forms/` - Windows Forms user interface
- `Models/` - application models
- `Data/` - database connection and database setup
- `Repositories/` - database access and CRUD operations
- `Services/` - main application logic
- `Tests/` - unit tests and fake repositories

## Database

The application uses a local SQLite database.

Main tables:

- `Knihy`
- `Ctenari`
- `Vypujcky`
- `Rezervace`

The database is created automatically when the application runs.

## How It Works

Books and readers are stored in the SQLite database.

A book can be borrowed if it is available.  
If a book is already borrowed, another reader can reserve it.  
Reservations are handled in order, so the first reader in the queue has priority.

The main business logic is separated into a service layer, and database operations are handled through repositories.

## Tests

The project includes unit tests for the main application logic, including:

- book validation
- reader validation
- borrowing and returning books
- reservations
- reservation queue
- loan limits
- reservation limits
- delete restrictions

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.

## Purpose

This project was created to practice C# desktop application development.

It demonstrates:

- Windows Forms UI
- SQLite database usage
- layered architecture
- repository pattern
- service layer
- validation
- unit testing
- basic library management logic