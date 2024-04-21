# MillesHotel Solution
## Overview
The MillesHotel solution is a .NET 8.0 application that simulates a hotel management system. It includes two projects: MillesHotel and MillesHotelLibrary.
## Projects
## MillesHotel
This is the main project of the solution. It is a console application that provides a user interface for interacting with the hotel management system. It includes various menus for managing bookings, customers, rooms, invoices, and admin tasks.
The project references the MillesHotelLibrary project and several NuGet packages including Autofac, Bogus, Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools, Microsoft.Extensions.Configuration, and Microsoft.Extensions.Configuration.Json.
## MillesHotelLibrary
This project is a class library that contains the core logic and data models for the hotel management system. It includes services for managing bookings, customers, rooms, invoices, and admin tasks. It also includes interfaces and data models that represent the entities in the system.
The project references several NuGet packages including Autofac, Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools, Microsoft.Extensions.Configuration, and Microsoft.Extensions.Configuration.Json.
## Data Models
The data models in the MillesHotelLibrary project represent the entities in the hotel management system. These include Booking, Customer, Room, Invoice, Admin, and Country.
## Services
The services in the MillesHotelLibrary project provide the core logic for managing the entities in the hotel management system. These include BookingService, CustomerService, RoomService, InvoiceService, and AdminService.
## Menus
The menus in the MillesHotel project provide a user interface for interacting with the hotel management system. These include BookingMenu, CustomerMenu, RoomMenu, InvoiceMenu, and AdminMenu.
## Database
The solution uses Entity Framework Core with a SQL Server database. The HotelDbContext class in the MillesHotelLibrary project is the DbContext for the application.
## Getting Started
To run the application, you will need .NET 8.0 SDK installed. Open the solution in Visual Studio, set MillesHotel as the startup project, and run the application.
## Contributing
Contributions are welcome. Please open an issue to discuss your ideas before making changes.
