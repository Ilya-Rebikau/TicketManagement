﻿CREATE TABLE [dbo].[Seats]
(
	[Id] int identity primary key,
	[AreaId] int NOT NULL,
	[Row] int NOT NULL,
	[Number] int NOT NULL,
)
