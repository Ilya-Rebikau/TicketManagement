ALTER TABLE dbo.EventAreas
ADD CONSTRAINT FK_Event_EventArea FOREIGN KEY ([EventId])     
    REFERENCES dbo.[Events] (Id)