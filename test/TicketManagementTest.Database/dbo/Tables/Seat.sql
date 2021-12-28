CREATE TABLE [dbo].[Seat] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [AreaId] INT NOT NULL,
    [Row]    INT NOT NULL,
    [Number] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Area_Seat] FOREIGN KEY ([AreaId]) REFERENCES [dbo].[Area] ([Id])
);

