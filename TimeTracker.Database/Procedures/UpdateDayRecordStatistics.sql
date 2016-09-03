CREATE PROCEDURE [dbo].[UpdateDayRecordStatistics]
(
	@Id	INT
)
AS
BEGIN
	DECLARE @TotalHours INT
	SELECT @TotalHours = SUM([Hours]) FROM [dbo].[TimeRecords]
	WHERE [DayRecordId] = @Id AND [Deleted] = 0
	IF @TotalHours IS NULL
		SET @TotalHours = 0
	UPDATE [dbo].[DayRecords] SET [TotalHours] = @TotalHours WHERE [Id] = @Id
END
GO