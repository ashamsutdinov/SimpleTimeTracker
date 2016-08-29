CREATE PROCEDURE [dbo].[GetAllUserRoles]
AS
BEGIN
	SELECT [r].[Id], [r].[Description]
	FROM [dbo].[UserRoles] [r]	
END
GO
