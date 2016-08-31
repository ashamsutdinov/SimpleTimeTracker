CREATE PROCEDURE [dbo].[GetUserDayRecordByDate]
(
	@UserId	INT,
	@Date	DATE
)
AS
BEGIN
	SELECT [d].[Id], [d].[UserId], [d].[Date], [d].[TotalHours]
	FROM [dbo].[DayRecords] [d]
	WHERE [d].[UserId] = @UserId AND [d].[Date] = @Date
END
GO
