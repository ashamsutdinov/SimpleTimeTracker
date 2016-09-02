CREATE PROCEDURE [dbo].[GetTimeRecordNote]
(
	@Id	INT
)
AS
BEGIN
	SELECT [tn].[Id], [tn].[DayRecordId], [tn].[UserId], [tn].[DateTime], [tn].[Text], [tn].[Deleted]
	FROM [dbo].[TimeRecordNotes] [tn]	
	WHERE [tn].[Id] = @Id
END
GO