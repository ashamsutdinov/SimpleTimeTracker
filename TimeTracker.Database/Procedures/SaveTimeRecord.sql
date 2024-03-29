﻿CREATE PROCEDURE [dbo].[SaveTimeRecord]
(
	@Id			INT,
	@UserId		INT,
	@Date		DATE,
	@Caption	NVARCHAR(256),
	@Hours		INT
)
AS
BEGIN
	DECLARE @DayRecordId INT
	SELECT @DayRecordId = [Id] FROM [dbo].[DayRecords] WHERE [Date] = @Date AND [UserId] = @UserId
	IF @DayRecordId IS NULL
	BEGIN
		INSERT INTO [dbo].[DayRecords] ([UserId], [Date]) VALUES (@UserId, @Date)
		SELECT @DayRecordId = @@IDENTITY
	END

	IF EXISTS (SELECT TOP 1 * FROM [dbo].[TimeRecords] WHERE [Id] = @Id)
	BEGIN
		DECLARE @OldDayRecordId INT
		SELECT @OldDayRecordId = [DayRecordId] FROM [dbo].[TimeRecords] WHERE [Id] = @Id
		UPDATE [dbo].[TimeRecords] SET
			[DayRecordId] = @DayRecordId,
			[Caption] = @Caption,
			[Hours] = @Hours 
		WHERE [Id] = @Id
		EXEC [dbo].[UpdateDayRecordStatistics] @OldDayRecordId
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[TimeRecords] ([DayRecordId], [Hours], [Caption]) VALUES (@DayRecordId, @Hours, @Caption)
		SELECT @Id = @@IDENTITY
	END	

	SELECT [t].[Id], [t].[DayRecordId], [t].[Hours], [t].[Caption], [t].[Deleted]
	FROM [dbo].[TimeRecords] [t]
	WHERE [t].[Id] = @Id

	EXEC [dbo].[UpdateDayRecordStatistics] @DayRecordId
END
GO