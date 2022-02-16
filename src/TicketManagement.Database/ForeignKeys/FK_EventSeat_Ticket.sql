ALTER TABLE dbo.Tickets
ADD CONSTRAINT FK_EventSeat_Ticket FOREIGN KEY (EventSeatId)     
    REFERENCES dbo.EventSeats (Id)