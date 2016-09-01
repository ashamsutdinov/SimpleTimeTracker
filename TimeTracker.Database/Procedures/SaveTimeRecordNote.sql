CREATE PROCEDURE [dbo].[SaveTimeRecordNote]
(
	@Id				INT,
	@DayRecordId	INT,
	@UserId			INT,
	@Text			NVARCHAR(MAX)
)
AS
BEGIN
	IF EXISTS (SELECT TOP 1 * FROM [dbo].[TimeRecordNotes] WHERE [Id] = @Id)
	BEGIN
		UPDATE [dbo].[TimeRecordNotes] SET
			[DayRecordId] = @DayRecordId,
			[UserId] = @UserId,
			[DateTime] = GETUTCDATE(),
			[Text] = @Text
		WHERE
			[Id] = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[TimeRecordNotes] ([DayRecordId], [UserId], [DateTime], [Text])
		VALUES (@DayRecordId, @UserId, GETUTCDATE(), @Text)
		SELECT @Id = @@IDENTITY
	END

	SELECT [tn].[Id], [tn].[DayRecordId], [tn].[UserId], [tn].[DateTime], [tn].[Text], [tn].[Deleted]
	FROM [dbo].[TimeRecordNotes] [tn]
	WHERE [tn].[Id] = @Id
END
GO
