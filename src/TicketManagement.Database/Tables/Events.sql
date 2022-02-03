CREATE TABLE [dbo].[Events]
(
	[Id] int primary key identity,
	[Name] nvarchar(120) NOT NULL,
	[Description] nvarchar(max) NOT NULL,
	[LayoutId] int NOT NULL, 
    [TimeStart] SMALLDATETIME NOT NULL, 
    [TimeEnd] SMALLDATETIME NOT NULL, 
    [Image] NVARCHAR(MAX) NOT NULL,
)
