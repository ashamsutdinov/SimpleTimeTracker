CREATE PROCEDURE [dbo].[GetUser]
(
	@Id INT
)
AS
BEGIN
	SELECT [u].[Id], [u].[Login], [u].[PasswordHash], [u].[PasswordSalt], [u].[Name], [u].[StateId], [s].[Description]	
	FROM [dbo].[Users] [u]
	INNER JOIN [dbo].[UserStates] [s] ON [u].[StateId] = [s].[Id]
	WHERE [u].[Id] = @Id

	EXEC [dbo].[GetUserRoles] @Id
	EXEC [dbo].[GetUserSettings] @Id
END
GO