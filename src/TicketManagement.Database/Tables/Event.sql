CREATE TABLE [dbo].[Event]
(
	[Id] int primary key identity,
	[Name] nvarchar(120) NOT NULL,
	[Description] nvarchar(max) NOT NULL,
	[LayoutId] int NOT NULL, 
    [TimeStart] SMALLDATETIME NOT NULL, 
    [TimeEnd] SMALLDATETIME NOT NULL,
)
