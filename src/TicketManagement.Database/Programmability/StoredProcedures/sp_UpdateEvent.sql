CREATE PROCEDURE [dbo].[sp_UpdateEvent]
    @id INT,
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT
AS
UPDATE Event set Name = @name, Description = @description, LayoutId = @layoutId where Id = @id