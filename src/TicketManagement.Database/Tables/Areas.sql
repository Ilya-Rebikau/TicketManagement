﻿CREATE TABLE [dbo].[Areas]
(
	[Id] int identity primary key,
	[LayoutId] int NOT NULL,
	[Description] nvarchar(200) NOT NULL,
	[CoordX] int NOT NULL,
	[CoordY] int NOT NULL, 
    [BasePrice] FLOAT NOT NULL,
)
