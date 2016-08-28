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

	SELECT @Id
END
GO
