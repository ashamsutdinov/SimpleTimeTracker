CREATE FUNCTION [dbo].[GetUserRoles]
(
	@UserId INT
)
RETURNS @roles TABLE
(
	[Id] NVARCHAR(32)
)
AS
BEGIN
	INSERT @roles SELECT [RoleId] FROM [UsersToRoles] WHERE [UserId] = @UserId
	RETURN
END
