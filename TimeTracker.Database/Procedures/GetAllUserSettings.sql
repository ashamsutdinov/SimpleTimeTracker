CREATE PROCEDURE [dbo].[GetAllUserSettings]
AS
BEGIN
	SELECT [s].[Id], [s].[Description]
	FROM [dbo].[UserSettings] [s]
END
GO