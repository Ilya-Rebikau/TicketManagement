CREATE TABLE [dbo].[EventAreas]
(
	[Id] int identity primary key,
	[EventId] int NOT NULL,
	[Description] nvarchar(200) NOT NULL,
	[CoordX] int NOT NULL,
	[CoordY] int NOT NULL,
	[Price] float NOT NULL
)
