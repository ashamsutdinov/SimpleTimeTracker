CREATE FUNCTION [dbo].[GetUserSettings]
(
	@UserId INT
)
RETURNS @roles TABLE
(
	[Id] NVARCHAR(32),
	[Value] NVARCHAR(128)
)
AS
BEGIN
	INSERT @roles SELECT [SettingId], [Value] FROM [UsersToSettings] WHERE [UserId] = @UserId
	RETURN
END

