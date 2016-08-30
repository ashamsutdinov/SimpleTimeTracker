CREATE PROCEDURE [dbo].[GetTimeRecordNotes]
(
	@TimeRecordId	INT
)
AS
BEGIN
	SELECT [tn].[Id], [tn].[TimeRecordId], [tn].[UserId], [u].[Name] as [UserName], [tn].[DateTime], [tn].[Text]
	FROM [dbo].[TimeRecordNotes] [tn]
	INNER JOIN [dbo].[Users] [u] ON [tn].[UserId] = [u].[Id]
	WHERE [tn].[TimeRecordId] = @TimeRecordId AND [tn].[Deleted] = 0
	ORDER BY [tn].[Id] ASC
END
GO