CREATE PROCEDURE [dbo].[sp_UpdateEvent]
    @id INT,
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart SMALLDATETIME,
    @timeEnd SMALLDATETIME
AS
UPDATE Event set Name = @name, Description = @description, LayoutId = @layoutId, TimeStart = @timeStart, TimeEnd = @timeEnd where Id = @id