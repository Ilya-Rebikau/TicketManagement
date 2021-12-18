CREATE PROCEDURE [dbo].[sp_CreateEvent]
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT
AS
INSERT INTO Event(Name, Description, LayoutId) VALUES(@name, @description, @layoutId)
