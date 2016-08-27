CREATE PROCEDURE [dbo].[SaveTimeRecord]
(
	@Id			INT NOT NULL,
	@UserId		INT NOT NULL,
	@Date		DATE NOT NULL,
	@Hours		INT NOT NULL
)
AS
BEGIN
	DECLARE @DayRecordId INT
	SELECT @DayRecordId = [Id] FROM [dbo].[DayRecords] WHERE [Date] = @Date
	IF @DayRecordId IS NULL
	BEGIN
		INSERT INTO [dbo].[DayRecords] ([UserId], [Date]) VALUES (@UserId, @Date)
		SELECT @DayRecordId = @@IDENTITY
	END

	IF EXISTS (SELECT TOP 1 * FROM [dbo].[TimeRecords] WHERE [Id] = @Id)
	BEGIN		
		UPDATE [dbo].[TimeRecords] SET
			[DayRecordId] = @DayRecordId,
			[Hours] = @Hours 
		WHERE [Id] = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[TimeRecords] ([DayRecordId], [Hours]) VALUES (@DayRecordId, @Hours)
		SELECT @Id = @@IDENTITY
	END

	SELECT @Id
END
GO