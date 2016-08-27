CREATE FUNCTION [dbo].[GetUserSettingValue]
(
	@UserId INT,
	@SettingId NVARCHAR(32)
)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @value NVARCHAR(128)
	SELECT @value = [Value] FROM [dbo].[UsersToSettings] WHERE [UserId] = @UserId AND [SettingId] = @SettingId
	RETURN @value
END
GO
