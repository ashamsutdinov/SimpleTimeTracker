CREATE PROCEDURE [dbo].[GetTimeRecords]
(
	@UserId		INT = NULL,
	@FromDate	DATE = NULL,
	@ToDate		DATE = NULL,
	@PageNumber	INT,
	@PageSize	INT,
	@Total		INT OUTPUT
)
AS
BEGIN	
		
	CREATE TABLE #result
	(
		[Id]	INT,		
		[UserId]		INT,
		[UserName]		NVARCHAR(128),
		[Date]			DATE,
		[TotalHours]	INT,
		[TimeRecordId]	INT,
		[Caption]		NVARCHAR(256),
		[Hours]			INT
	)

	DECLARE @DayRecordId INT
	DECLARE @LoopUserId INT
	DECLARE @Name NVARCHAR(128)
	DECLARE @Date DATE
	DECLARE @TotalHours INT

	DECLARE DayRecordsLoop CURSOR FOR 
		SELECT [d].[Id], [d].[UserId], [u].[Name], [d].[Date], [d].[TotalHours]
		FROM [dbo].[DayRecords] [d]
		INNER JOIN [dbo].[Users] [u] ON [d].[UserId] = [u].[Id]
		WHERE
		(@UserId IS NULL OR [d].[UserId] = @UserId) AND
		(@FromDate IS NULL OR [d].[Date] >= @FromDate) AND
		(@ToDate IS NULL OR [d].[Date] <= @ToDate) AND 
		([d].[TotalHours] > 0)
		ORDER BY [d].[Date] ASC
		OFFSET (@PageNumber - 1) * @PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY

	OPEN DayRecordsLoop
	FETCH DayRecordsLoop INTO @DayRecordId, @LoopUserId, @Name, @Date, @TotalHours

	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO #result ([Id], [UserId], [UserName], [Date], [TotalHours], [TimeRecordId], [Caption], [Hours])
		SELECT @DayRecordId, @LoopUserId, @Name, @Date, @TotalHours, [t].[Id], [t].[Caption], [t].[Hours]
		FROM [dbo].[TimeRecords] [t]
		WHERE [t].[DayRecordId] = @DayRecordId AND [t].[Deleted] = 0		
		ORDER BY [t].[Id] ASC		

		FETCH DayRecordsLoop INTO @DayRecordId, @LoopUserId, @Name, @Date, @TotalHours
	END

	CLOSE DayRecordsLoop
	DEALLOCATE DayRecordsLoop	
	
	SELECT [Id], [UserId], [UserName], [Date], [TotalHours], [TimeRecordId], [Caption], [Hours]	FROM #result

	SELECT @Total = COUNT(*) FROM [dbo].[DayRecords] WHERE
		(@UserId IS NULL OR [UserId] = @UserId) AND
		(@FromDate IS NULL OR [Date] >= @FromDate) AND
		(@ToDate IS NULL OR [Date] <= @ToDate) AND 
		([TotalHours] > 0)
END
GO