﻿CREATE TABLE [dbo].[Venues]
(
	[Id] int identity primary key,
	[Description] nvarchar(120) NOT NULL,
	[Address] nvarchar(200) NOT NULL,
	[Phone] nvarchar(30), 
    [Name] NVARCHAR(50) UNIQUE NOT NULL,
)
