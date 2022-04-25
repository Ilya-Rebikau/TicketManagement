
# TicketManagement
## Ticket management application that provides different abilities for users.
### How to use:
1. To use this application you need to download this solution;
2. Create database from project TicketManagement.Database;
3. Change path to your SQL server in projects:
    - TicketManagement.Web in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.Web/appsettings.json "Database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*);
    - TicketManagement.UserAPI in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.UserAPI/appsettings.json "Database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*);
    - TicketManagement.EventManagerAPI in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.EventManagerAPI/appsettings.json "Database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*);
    - TicketManagement.VenueManagerAPI in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.VenueManagerAPI/appsettings.json "Database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*);
    - TicketManagement.PurchaseFlowAPI in file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.PurchaseFlowAPI/appsettings.json "Database config") (Server=.\\SQLEXPRESS change to Server=*path to your SQL server*);
4. To run all needed projects and start working with them were created 2 batch files:
    - [DebugStart.bat](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/DebugStart.bat "Run app in debug mode") to run application in debug mode.
    - [ReleaseStart.bat](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/ReleaseStart.bat "Run app in release mode") to run application in release mode.
5. If you want to enable redirects from ASP Web app to React app, then go to file [appsettings.json](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/TicketManagement.Web/appsettings.json "Config") and in section "FeatureManagement" set "Redirect" to "true", else set to "false".

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

# Second solution ThirdPartyEventEditor
## Giving possibility to work with third party events with json file as database.
### How to use
This solution don't have any special things to do before start, just download it and start, but in case you want change path to logs or json file with third party events, then do it in file [Web.config](https://github.com/EPAM-Gomel-NET-Lab/IlyaRebikau/blob/develop/src/ThirdPartyEventEditor/ThirdPartyEventEditor/Web.config "Config file") in section *appsetting*.  
To test this solution just run tests.
