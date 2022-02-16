CREATE TABLE [dbo].[AspNetUserClaims]
(
	[Id] int NOT NULL IDENTITY,
	[UserId] nvarchar(450) NOT NULL,
	[ClaimType] nvarchar(max) NULL,
	[ClaimValue] nvarchar(max) NULL,
	CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id])
)
