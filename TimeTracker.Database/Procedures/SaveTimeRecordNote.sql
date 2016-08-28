CREATE PROCEDURE [dbo].[SaveTimeRecordNote]
(
	@Id				INT,
	@TimeRecordId	INT,
	@UserId			INT,
	@Text			NVARCHAR(MAX)
)
AS
BEGIN
	IF EXISTS (SELECT TOP 1 * FROM [dbo].[TimeRecordNotes] WHERE [Id] = @Id)
	BEGIN
		UPDATE [dbo].[TimeRecordNotes] SET
			[TimeRecordId] = @TimeRecordId,
			[UserId] = @UserId,
			[DateTime] = GETUTCDATE(),
			[Text] = @Text
		WHERE
			[Id] = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[TimeRecordNotes] ([TimeRecordId], [UserId], [DateTime], [Text])
		VALUES (@TimeRecordId, @UserId, GETUTCDATE(), @Text)
		SELECT @Id = @@IDENTITY
	END

	SELECT [tn].[Id], [tn].[TimeRecordId], [tn].[UserId], [tn].[DateTime], [tn].[Text], [tn].[Deleted]
	FROM [dbo].[TimeRecordNotes] [tn]
	WHERE [tn].[Id] = @Id
END
GO
