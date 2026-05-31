# Library Management System

Desktop application for managing a small library.  
The application allows users to manage books, readers, loans, returns and reservations.

## Features

- Add, edit and delete books
- Add, edit and delete readers
- Borrow available books
- Return borrowed books
- Reserve borrowed books
- Cancel book reservations
- Prevent deleting borrowed or reserved books
- Store data in a local SQLite database

## Technologies Used

- C#
- .NET 8
- Windows Forms
- SQLite
- Microsoft.Data.Sqlite
- Object-oriented programming
- Repository pattern
- Service layer

## Project Structure

- `Forms/` - Windows Forms user interface
- `Models/` - application models
- `Data/` - database connection and initialization
- `Repositories/` - database CRUD operations
- `Services/` - main application logic

## Database

The application uses a local SQLite database.  
The database is created automatically when the application starts.

Main tables:

- `Knihy`
- `Ctenari`
- `Vypujcky`
- `Rezervace`

## How It Works

Books and readers are stored in a SQLite database.  
Loans and reservations are also stored in the database.

Books can be borrowed only if they are available.  
Borrowed books can be reserved by readers.  
Available books cannot be reserved because they can be borrowed directly.

## How to Run

1. Open `Knihovna.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.

## Purpose

This project was created to practice C# desktop application development with Windows Forms.

It demonstrates:

- CRUD operations
- SQLite database usage
- Object-oriented programming
- Repository pattern
- Service layer
- Basic layered architecture
- Book loan and reservation logic

## Future Improvements

- Add unit tests
- Improve UI layout
- Add search and filtering
- Improve reservation queue visualization
- Add screenshots to README
- Add better error handling