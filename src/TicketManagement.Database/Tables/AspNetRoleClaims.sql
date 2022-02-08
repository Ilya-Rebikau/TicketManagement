CREATE TABLE [dbo].[AspNetRoleClaims]
(
	[Id] int NOT NULL IDENTITY,
	[RoleId] nvarchar(450) NOT NULL,
	[ClaimType] nvarchar(max) NULL,
	[ClaimValue] nvarchar(max) NULL,
	CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id])
)
