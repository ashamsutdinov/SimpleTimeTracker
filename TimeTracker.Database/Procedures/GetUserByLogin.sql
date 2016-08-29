CREATE PROCEDURE [dbo].[GetUserByLogin]
(
	@Login	NVARCHAR(64)
)
AS
BEGIN
	DECLARE @Id INT
	SELECT @Id = [Id] FROM [dbo].[Users] WHERE [Login] = @Login
	
	EXEC [dbo].[GetUser] @Id
END
GO