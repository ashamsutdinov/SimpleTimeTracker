CREATE PROCEDURE [dbo].[SaveUser]
(
	@Id				INT NOT NULL,
	@Login			NVARCHAR(64) NOT NULL,
	@PasswordHash	NVARCHAR(64) NOT NULL,
	@PasswordSalt	NVARCHAR(16) NOT NULL,
	@Name			NVARCHAR(128) NOT NULL,
	@StateId		NVARCHAR(16) NOT NULL,
	@Roles			KeyCollection NOT NULL READONLY,
	@Settings		KeyValueCollection NOT NULL READONLY
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
