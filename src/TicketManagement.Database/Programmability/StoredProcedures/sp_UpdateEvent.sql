CREATE PROCEDURE [dbo].[sp_UpdateEvent]
    @id INT,
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart NVARCHAR(30),
    @timeEnd NVARCHAR(30),
    @image NVARCHAR(MAX)
AS
DECLARE @timeStartDT smalldatetime
DECLARE @timeEndDT smalldatetime
SET @timeStartDT = CONVERT(smalldatetime, @timeStart, 20)
SET @timeEndDT = CONVERT(smalldatetime, @timeEnd, 20)
UPDATE Events set Name = @name, Description = @description, LayoutId = @layoutId, TimeStart = @timeStartDT, TimeEnd = @timeEndDT, Image = @image where Id = @id