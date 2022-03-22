start "Build TicketManagement.Web" /D"%~dp0src\TicketManagement.Web" dotnet build
start "Build TicketManagement.UserAPI" /D"%~dp0src\TicketManagement.UserAPI" dotnet build
start "Build TicketManagement.EventManagerAPI" /D"%~dp0src\TicketManagement.EventManagerAPI" dotnet build
start "Build TicketManagement.VenueManagerAPI" /D"%~dp0src\TicketManagement.VenueManagerAPI" dotnet build
start "Build TicketManagement.PurchaseFlowAPI" /D"%~dp0src\TicketManagement.PurchaseFlowAPI" dotnet build
TIMEOUT 5
start "Run TicketManagement.Web" /D"%~dp0src\TicketManagement.Web" dotnet run --launch-profile "TicketManagement.Web.Release"
start "Run TicketManagement.UserAPI" /D"%~dp0src\TicketManagement.UserAPI" dotnet run --launch-profile "TicketManagement.UserAPI.Release"
start "Run TicketManagement.EventManagerAPI" /D"%~dp0src\TicketManagement.EventManagerAPI" dotnet run --launch-profile "TicketManagement.EventManagerAPI.Release"
start "Run TicketManagement.VenueManagerAPI" /D"%~dp0src\TicketManagement.VenueManagerAPI" dotnet run --launch-profile "TicketManagement.VenueManagerAPI.Release"
start "Run TicketManagement.PurchaseFlowAPI" /D"%~dp0src\TicketManagement.PurchaseFlowAPI" dotnet run --launch-profile "TicketManagement.PurchaseFlowAPI.Release"
TIMEOUT 5
explorer "https://localhost:5000"