CREATE PROCEDURE [dbo].[GetTimeRecord]
(
	@Id	INT
)
AS
BEGIN
	SELECT [t].[Id], [t].[DayRecordId], [t].[Hours], [t].[Caption], [t].[Deleted]
	FROM [dbo].[TimeRecords] [t]
	WHERE [t].[Id] = @Id
END
GO
