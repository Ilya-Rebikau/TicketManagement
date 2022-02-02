ALTER TABLE dbo.Ticket
ADD CONSTRAINT FK_EventSeat_Ticket FOREIGN KEY (EventSeatId)     
    REFERENCES dbo.EventSeat (Id)