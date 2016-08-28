CREATE PROCEDURE [dbo].[GetUserSettings]
(
	@UserId INT
)
AS
BEGIN
	SELECT [s].[Id], [s].[Description], [us].[Value]
	FROM [dbo].[UsersToSettings] [us]
	INNER JOIN [dbo].[UserSettings] [s] ON [us].[SettingId] = [s].[Id]
	WHERE [us].[UserId] = @UserId
END
GO
