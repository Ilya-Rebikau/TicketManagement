CREATE PROCEDURE [dbo].[sp_CreateEvent]
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
INSERT INTO Events(Name, Description, LayoutId, TimeStart, TimeEnd, Image) VALUES(@name, @description, @layoutId, @timeStartDT, @timeEndDT, @image)