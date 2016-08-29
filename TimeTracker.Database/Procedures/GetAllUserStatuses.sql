CREATE PROCEDURE [dbo].[GetAllUserStatuses]
AS
BEGIN
	SELECT [s].[Id], [s].[Description]
	FROM [dbo].[UserStates] [s]	
END
GO