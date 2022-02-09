
# TicketManagement
## Ticket management application that provides different abilities for users (without graphical interface).
To create this application were studied: ADO .NET, principles of OOP, SOLID, unit and integration testing, code convention, SQL, etc.  
During the creation of the application were created: data access layer with models, repositories (using ADO .NET) and connection to MS SQL; business logic layer with services (services using IRepository interface to get access to data in database, also add validations to create and update operations); test database with test data; unit tests with validation testing; integration tests with correct input data testing and CRUD operations testing.  
To use this application you need to download this solution, change path to your SQL server in project TicketManagement.DataAccess in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.DataAccess/appsettings.json "Database config") (Data Source=.\\SQLEXPRESS change to Data Source=*path to your SQL server*), create database from project TicketManagement.Database.  
Since this application without graphical interface you can test it thanks to unit and integration tests.  
To start integration tests you need to create test database from project TicketManagement.Database and change path to it in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.DataAccess/appsettings.json "Test database config") (Data Source=.\\SQLEXPRESS change to Data Source=*path to your SQL server*) in TicketManagement.IntegrationTests project.  
