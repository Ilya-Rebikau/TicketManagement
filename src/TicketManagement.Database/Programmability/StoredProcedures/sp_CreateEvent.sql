CREATE PROCEDURE [dbo].[sp_CreateEvent]
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart NVARCHAR(MAX),
    @timeEnd NVARCHAR(MAX),
    @imageUrl NVARCHAR(MAX)
AS
DECLARE @timeStartDT smalldatetime
DECLARE @timeEndDT smalldatetime
SET @timeStartDT = CONVERT(smalldatetime, @timeStart, 20)
SET @timeEndDT = CONVERT(smalldatetime, @timeEnd, 20)
INSERT INTO Events(Name, Description, LayoutId, TimeStart, TimeEnd, ImageUrl) VALUES(@name, @description, @layoutId, @timeStartDT, @timeEndDT, @imageUrl)