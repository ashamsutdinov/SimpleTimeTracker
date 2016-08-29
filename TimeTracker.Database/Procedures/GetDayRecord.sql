CREATE PROCEDURE [dbo].[GetDayRecord]
(
	@Id	INT
)
AS
BEGIN
	SELECT [d].[Id], [d].[UserId], [d].[Date], [d].[TotalHours]
	FROM [dbo].[DayRecords] [d]
	WHERE [d].[Id] = @Id
END
GO
