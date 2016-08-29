CREATE PROCEDURE [dbo].[SaveUserSession]
(
	@Id			INT,
	@UserId		INT,
	@ClientId	NVARCHAR(256),
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
		INSERT INTO [dbo].[UserSessions] ([UserId], [ClientId], [DateTime], [Expiration])
		VALUES (@UserId, @ClientId, @DateTime, @Expiration)
		SELECT @Id = @@IDENTITY
	END

	SELECT [us].[Id], [us].[UserId], [us].[ClientId], [us].[Ticket], [us].[Expired], [us].[DateTime], [us].[Expiration]
	FROM [dbo].[UserSessions] [us]
	WHERE [us].[Id] = @Id
END
GO