CREATE PROCEDURE [dbo].[DeleteTimeRecordNote]
(
	@Id		INT
)
AS
BEGIN
	UPDATE [dbo].[TimeRecordNotes] SET [Deleted] = 1 WHERE [Id] = @Id
END
GO
