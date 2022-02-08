ALTER TABLE dbo.EventSeats
ADD CONSTRAINT FK_Area_EventSeat FOREIGN KEY ([EventAreaId])     
    REFERENCES dbo.EventAreas (Id)