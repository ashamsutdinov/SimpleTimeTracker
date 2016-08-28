CREATE PROCEDURE [dbo].[SaveUserRole]
(
	@Id				NVARCHAR(32),
	@Description	NVARCHAR(128)
)
AS
BEGIN
	IF EXISTS (SELECT TOP 1 * FROM [dbo].[UserRoles] WHERE [Id] = @Id)
		UPDATE [dbo].[UserRoles] SET [Description] = @Description WHERE [Id] = @Id
	ELSE
		INSERT INTO [dbo].[UserRoles] ([Id], [Description]) VALUES (@Id, @Description)		
END
GO
