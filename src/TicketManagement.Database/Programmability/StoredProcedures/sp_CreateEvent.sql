CREATE PROCEDURE [dbo].[sp_CreateEvent]
    @name NVARCHAR(120),
    @description NVARCHAR(MAX),
    @layoutId INT,
    @timeStart smalldatetime,
    @timeEnd smalldatetime,
    @imageUrl NVARCHAR(MAX),
	@Identity int OUT
AS
INSERT INTO Events(Name, Description, LayoutId, TimeStart, TimeEnd, ImageUrl) VALUES(@name, @description, @layoutId, @timeStart, @timeEnd, @imageUrl)
SET @Identity = SCOPE_IDENTITY()