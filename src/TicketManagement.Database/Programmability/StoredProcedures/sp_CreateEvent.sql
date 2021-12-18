CREATE PROCEDURE [dbo].[sp_CreateEvent]
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart SMALLDATETIME,
    @timeEnd SMALLDATETIME
AS
INSERT INTO Event(Name, Description, LayoutId, TimeStart, TimeEnd) VALUES(@name, @description, @layoutId, @timeStart, @timeEnd)