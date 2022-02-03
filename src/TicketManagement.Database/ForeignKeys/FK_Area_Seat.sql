ALTER TABLE dbo.Seats
ADD CONSTRAINT FK_Area_Seat FOREIGN KEY (AreaId)     
    REFERENCES dbo.Areas (Id)