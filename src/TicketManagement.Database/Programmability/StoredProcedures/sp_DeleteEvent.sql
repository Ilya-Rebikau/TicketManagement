CREATE PROCEDURE [dbo].[sp_DeleteEvent]
    @id INT
AS
Delete from Event where Id=@id