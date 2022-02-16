
# TicketManagement
## Ticket management application that provides different abilities for users.
### How to use:
1. To use this application you need to download this solution;
2. Create database from project TicketManagement.Database;
3. Change path to your SQL server in project TicketManagement.Web in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.Web/appsettings.json "Database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*);
4. Graphical interface was created, use TicketManagement.Web project to run it.

### Credentials:
There are 4 roles in this application, you can use them.
1. Admin account - login: admin@mail.ru, password: Qwer!1
2. Event manager account - login: eventmanager@mail.ru, password: Qwer!1
3. Venue manager account - login: venuemanager@mail.ru, password: Qwer!1
4. User account - login: user@mail.ru, password: Qwer!1  

There is no need to change path to your sql server in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.DataAccess/appsettings.json "Old database config") because this file is using for old repositories (with ADO.NET).
## You can test it thanks to unit and integration tests.
### How to use integration tests for BLL:
1. Create test database from project TicketManagement.Database;
2. Change path to database in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/test/TicketManagement.IntegrationTests/appsettings.json "Test database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*) in TicketManagement.IntegrationTests project;  
3. Run tests.
