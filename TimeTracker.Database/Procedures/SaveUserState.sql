CREATE PROCEDURE [dbo].[SaveUserState]
(
	@Id				NVARCHAR(32) NOT NULL,
	@Description	NVARCHAR(128) NOT NULL
)
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[UserStates] WHERE [Id] = @Id)
		UPDATE [dbo].[UserStates] SET [Description] = @Description WHERE [Id] = @Id		
	ELSE
		INSERT INTO [dbo].[UserStates] ([Id], [Description]) VALUES (@Id, @Description)
END
GO
