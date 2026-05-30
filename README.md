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
- Prevent deleting books that are currently borrowed or reserved
- Save and load data using XML files

## Technologies Used

- C#
- .NET 8
- Windows Forms
- XML serialization
- Object-oriented programming

## Project Structure

- `Form1.cs` - main application form and user interface logic
- `Databaze.cs` - main data handling and application logic
- `Kniha.cs` - book model
- `Ctenar.cs` - reader model
- `KnihaDialog.cs` - dialog for adding and editing books
- `CtenarDialog.cs` - dialog for adding and editing readers

## How It Works

The application stores books and readers in memory while it is running.  
When the application is closed, the data is saved into XML files.  
When the application starts again, the saved data is loaded back into the application.

Books can be borrowed only if they are available.  
Borrowed books can be reserved by readers.  
Available books cannot be reserved because they can be borrowed directly.

## How to Run

1. Open the solution file `Knihovna.sln` in Visual Studio.
2. Build the solution.
3. Run the application.

## Purpose of the Project

This project was created to practice desktop application development in C# using Windows Forms.  
It demonstrates basic CRUD operations, object-oriented programming, working with collections and saving/loading data using XML serialization.