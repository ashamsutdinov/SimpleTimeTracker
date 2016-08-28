CREATE PROCEDURE [dbo].[GetUserRoles]
(
	@UserId INT
)
AS
BEGIN
	SELECT [r].[Id], [r].[Description]
	FROM [dbo].[UserRoles] [r]
	INNER JOIN [dbo].[UsersToRoles] [ur] ON [ur].[RoleId] = [r].[Id]
	WHERE [ur].[UserId] = @UserId
END
GO
