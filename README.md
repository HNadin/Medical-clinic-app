# Medical_clinic

This project is an MVC (Model-View-Controller) application developed in C# that serves as an automated system for a medical clinic. The system allows managing various aspects related to medical services, appointments, doctors, and subscriptions for updates.

## Features
- Service Management: Admin users can create, edit, and delete medical services, along with associating doctors and nurses to each service.
- Appointment Booking: Users can book appointments for medical services by providing necessary details like name, phone number, selected service, and preferred doctor.
- Doctor Management: Admin users can manage doctors, including adding new doctors, editing existing ones, and deleting them.
- Nurses Management: Admin users can manage nurses, including adding new nurses, editing existing ones, and deleting them.
- User Appointments: Users can view their appointments and associated details.
- Subscription for Updates: Users can subscribe to receive updates about the clinic's services and appointments.
- Role-based Authorization: Different roles are defined, such as Admin and Patient, with restricted access to certain functionalities.

## Technologies Used

- C#: The primary programming language used for backend development.
- ASP.NET Core MVC: Framework for building web applications and APIs using the MVC pattern.
- Entity Framework Core: Object-relational mapping (ORM) framework used to interact with the database.
- Identity Framework: Provides user authentication and authorization functionalities.
- Microsoft SQL Server: Backend database management system used for data storage.

## Environment requirements

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server DBMS (or any other supported by Entity Framework Core)
- Development environment, for example, Visual Studio

## Installation and launch

1. Clone the repository to your local computer.
2. Create a database in your DBMS.
3. Change the database connection string in the `appsettings.json` file to match your settings.
4. Open a command prompt in the project root folder and run the following commands:

   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run