CREATE TABLE [dbo].[Layout] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [VenueId]     INT            NOT NULL,
    [Description] NVARCHAR (120) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Venue_Layout] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venue] ([Id])
);

