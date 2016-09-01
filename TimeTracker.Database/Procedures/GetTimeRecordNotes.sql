CREATE PROCEDURE [dbo].[GetTimeRecordNotes]
(
	@DayRecordId	INT
)
AS
BEGIN
	SELECT [tn].[Id], [tn].[DayRecordId], [tn].[UserId], [u].[Name] as [UserName], [tn].[DateTime], [tn].[Text]
	FROM [dbo].[TimeRecordNotes] [tn]
	INNER JOIN [dbo].[Users] [u] ON [tn].[UserId] = [u].[Id]
	WHERE [tn].[DayRecordId] = @DayRecordId AND [tn].[Deleted] = 0
	ORDER BY [tn].[DateTime] ASC
END
GO