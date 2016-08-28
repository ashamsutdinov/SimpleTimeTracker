CREATE PROCEDURE [dbo].[AbandonSession]
(
	@Id	INT
)
AS
BEGIN
	UPDATE [dbo].[UserSessions] SET [Expired] = 1 WHERE [Id] = @Id
END
GO