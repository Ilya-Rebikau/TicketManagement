CREATE TABLE [dbo].[Tickets]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [EventSeatId] INT NOT NULL, 
    [UserId] nvarchar(450) NOT NULL
)
