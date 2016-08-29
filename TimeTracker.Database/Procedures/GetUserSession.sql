CREATE PROCEDURE [dbo].[GetUserSession]
(
	@Id INT
)
AS
BEGIN
	SELECT [us].[Id], [us].[UserId], [us].[ClientId], [us].[Ticket], [us].[Expired], [us].[DateTime], [us].[Expiration]
	FROM [dbo].[UserSessions] [us]
	WHERE
	[us].[Id] = @Id
END
GO