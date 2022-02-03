CREATE TABLE [dbo].[EventSeats]
(
	[Id] int identity primary key,
	[EventAreaId] int NOT NULL,
	[Row] int NOT NULL,
	[Number] int NOT NULL,
	[State] int NOT NULL
)
