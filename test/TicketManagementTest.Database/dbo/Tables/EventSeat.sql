CREATE TABLE [dbo].[EventSeat] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [EventAreaId] INT NOT NULL,
    [Row]         INT NOT NULL,
    [Number]      INT NOT NULL,
    [State]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Area_EventSeat] FOREIGN KEY ([EventAreaId]) REFERENCES [dbo].[EventArea] ([Id])
);

