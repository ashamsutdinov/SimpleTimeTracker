CREATE PROCEDURE [dbo].[GetUserSettingValue]
(
	@UserId		INT,
	@SettingId	NVARCHAR(32)
)
AS
BEGIN
	SELECT [us].[Value]
	FROM [dbo].[UsersToSettings] [us]
	INNER JOIN [dbo].[UserSettings] [s] ON [us].[SettingId] = [s].[Id]
	WHERE [us].[UserId] = @UserId AND [us].[SettingId] = @SettingId
END
GO