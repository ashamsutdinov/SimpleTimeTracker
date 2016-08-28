CREATE PROCEDURE [dbo].[SaveUser]
(
	@Id				INT,
	@Login			NVARCHAR(64),
	@PasswordHash	NVARCHAR(64),
	@PasswordSalt	NVARCHAR(16),
	@Name			NVARCHAR(128),
	@StateId		NVARCHAR(16),
	@Roles			KeyCollection READONLY,
	@Settings		KeyValueCollection READONLY
)
AS
BEGIN
	IF EXISTS (SELECT TOP 1 * FROM [dbo].[Users] WHERE [Id] = @Id)
	BEGIN
		UPDATE [dbo].[Users] SET
			[Login] = @Login,
			[PasswordHash] = @PasswordHash,
			[PasswordSalt] = @PasswordSalt,
			[Name] = @Name,
			[StateId] = @StateId
		WHERE
			[Id] = @Id

		DELETE FROM [dbo].[UsersToRoles] WHERE [UserId] = @Id
		DELETE FROM [dbo].[UsersToSettings] WHERE [UserId] = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Users] ([Login], [PasswordHash], [PasswordSalt], [Name], [StateId])
		VALUES (@Login, @PasswordHash, @PasswordSalt, @Name, @StateId)

		SELECT @Id = @@IDENTITY
	END

	INSERT INTO [dbo].[UsersToRoles] SELECT @Id, [Id] FROM @Roles
	INSERT INTO [dbo].[UsersToSettings] SELECT @Id, [Id], [Value] FROM @Settings

	SELECT @Id
END
GO
