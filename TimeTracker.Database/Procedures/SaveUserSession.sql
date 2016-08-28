CREATE PROCEDURE [dbo].[SaveUserSession]
(
	@Id			INT,
	@UserId		INT,
	@DateTime	DATETIME2,
	@Expiration	INT
)
AS
BEGIN
	IF EXISTS (SELECT TOP 1 * FROM [dbo].[UserSessions] WHERE [Id] = @Id)
	BEGIN
		UPDATE [dbo].[UserSessions] SET
			[DateTime] = @DateTime
			-- UserId and Expiration are immutable
		WHERE
			[Id] = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[UserSessions] ([UserId], [DateTime], [Expiration])
		VALUES (@UserId, @DateTime, @Expiration)
		SELECT @Id = @@IDENTITY
	END

	SELECT @Id
END
GO