CREATE TRIGGER [TimeRecordTrigger]
ON [dbo].[TimeRecords]
AFTER DELETE, INSERT, UPDATE
AS
BEGIN
	DECLARE @TotalHours INT
	SELECT @TotalHours = (SELECT  SUM([Hours]) FROM [TimeRecords] [t] WHERE [t].[DayRecordId] = [d].[Id] AND [Deleted] = 0)
    FROM    [DayRecords] [d]
    WHERE   [d].[Id] IN (SELECT [DayRecordId] FROM deleted UNION SELECT [DayRecordId] FROM inserted)
	IF @TotalHours IS NULL
		SET @TotalHours = 0

	UPDATE  [d]
    SET     [TotalHours] = @TotalHours
    FROM    [DayRecords] [d]
    WHERE   [d].[Id] IN (SELECT [DayRecordId] FROM deleted UNION SELECT [DayRecordId] FROM inserted)
END
