CREATE PROCEDURE [dbo].[GetAllUserStates]
AS
BEGIN
	SELECT [s].[Id], [s].[Description]
	FROM [dbo].[UserStates] [s]	
END
GO