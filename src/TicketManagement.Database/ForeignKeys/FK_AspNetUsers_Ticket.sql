ALTER TABLE dbo.Ticket
ADD CONSTRAINT FK_AspNetUsers_Ticket FOREIGN KEY (UserId)     
    REFERENCES dbo.AspNetUsers (Id)