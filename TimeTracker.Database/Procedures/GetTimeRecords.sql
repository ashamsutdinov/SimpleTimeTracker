CREATE PROCEDURE [dbo].[GetTimeRecords]
(
	@UserId INT = NULL,
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@PageNumber INT,
	@PageSize INT,
	@Total INT OUTPUT
)
AS
BEGIN
	--page consist of day records, not time records.
	--each day record consist of any number of time records
	SELECT @Total = COUNT(*) FROM [dbo].[DayRecords] WHERE
		(@UserId IS NULL OR [UserId] = @UserId) AND
		(@FromDate IS NULL OR [Date] >= @FromDate) AND
		(@ToDate IS NULL OR [Date] <= @ToDate) AND 
		([TotalHours] > 0)
		
	CREATE TABLE #result
	(
		[DayRecordId]	INT,		
		[UserId]		INT,
		[Name]			NVARCHAR(128),
		[Date]			DATE,
		[TotalHours]	INT,
		[TimeRecordId]	INT
	)
	
	
	
	SELECT * FROM #result
	DROP TABLE #result	
END
GO