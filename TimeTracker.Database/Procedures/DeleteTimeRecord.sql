CREATE PROCEDURE [dbo].[DeleteTimeRecord]
(
	@Id	INT
)
AS
BEGIN
	DECLARE @DayRecordId INT
	SELECT @DayRecordId = [DayRecordId] FROM [dbo].[TimeRecords] WHERE [Id] = @Id
	IF @DayRecordId IS NOT NULL
	BEGIN
		UPDATE [dbo].[TimeRecords] SET [Deleted] = 1 WHERE [Id] = @Id
		EXEC [dbo].[UpdateDayRecordStatistics] @DayRecordId
	END
END
GO
