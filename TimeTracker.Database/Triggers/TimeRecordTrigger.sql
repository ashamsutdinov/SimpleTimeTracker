CREATE TRIGGER [TimeRecordTrigger]
ON [dbo].[TimeRecords]
AFTER DELETE, INSERT, UPDATE
AS
BEGIN

	UPDATE  [d]
    SET     [TotalHours] = (SELECT  SUM([Hours]) FROM [TimeRecords] [t] WHERE [t].[DayRecordId] = [d].[Id] AND [Deleted] = 0)
    FROM    [DayRecords] [d]
    WHERE   [d].[Id] IN (SELECT [DayRecordId] FROM deleted UNION SELECT [DayRecordId] FROM inserted)
END
