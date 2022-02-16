CREATE TABLE [dbo].[Tickets]
(
	[Id] INT identity NOT NULL PRIMARY KEY, 
    [EventSeatId] INT NOT NULL, 
    [UserId] nvarchar(450) NOT NULL
)
