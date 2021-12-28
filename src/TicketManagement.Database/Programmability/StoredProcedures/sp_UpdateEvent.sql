CREATE PROCEDURE [dbo].[sp_UpdateEvent]
    @id INT,
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart NVARCHAR(30),
    @timeEnd NVARCHAR(30)
AS
DECLARE @timeStartDT smalldatetime
DECLARE @timeEndDT smalldatetime
SET @timeStartDT = CONVERT(smalldatetime, @timeStart, 20)
SET @timeEndDT = CONVERT(smalldatetime, @timeEnd, 20)
UPDATE Event set Name = @name, Description = @description, LayoutId = @layoutId, TimeStart = @timeStartDT, TimeEnd = @timeEndDT where Id = @id