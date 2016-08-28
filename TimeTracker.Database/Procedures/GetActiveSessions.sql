CREATE PROCEDURE [dbo].[GetActiveSessions]
(
	@UserId INT
)
AS
BEGIN
	SELECT [us].[Id], [us].[Ticket] 
	FROM [dbo].[UserSessions] [us]
	WHERE
	[us].[UserId] = @UserId AND
	[us].[Expired] = 0 AND
	DATEADD(SECOND, [us].[Expiration], [us].[DateTime]) > GETUTCDATE()
	ORDER BY [us].[Id] ASC
END
GO
