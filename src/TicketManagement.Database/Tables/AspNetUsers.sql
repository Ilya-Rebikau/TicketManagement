CREATE TABLE [dbo].[AspNetUsers]
(
	[Id] nvarchar(450) NOT NULL,
	[UserName] nvarchar(256) NULL,
	[FirstName] nvarchar(256) NULL,
	[Surname] nvarchar(256) NULL,
	[Email] nvarchar(256) NULL,
	[Balance] decimal NULL,
	[PasswordHash] nvarchar(max) NULL,
	CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
)
