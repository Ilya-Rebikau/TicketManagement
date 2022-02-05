CREATE PROCEDURE [dbo].[sp_UpdateEvent]
    @id INT,
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart smalldatetime,
    @timeEnd smalldatetime,
    @imageUrl NVARCHAR(MAX)
AS
UPDATE Events set Name = @name, Description = @description, LayoutId = @layoutId, TimeStart = @timeStart, TimeEnd = @timeEnd, ImageUrl = @imageUrl where Id = @id