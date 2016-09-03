CREATE PROCEDURE [dbo].[DeleteTimeRecord]
(
	@Id		INT
)
AS
BEGIN
	UPDATE [dbo].[TimeRecords] SET [Deleted] = 1 WHERE [Id] = @Id
END
GO
