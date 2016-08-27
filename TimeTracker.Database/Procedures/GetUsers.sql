CREATE PROCEDURE [dbo].[GetUsers]
(
	@PageNumber	INT,
	@PageSize	INT,
	@Total		INT OUTPUT
)
AS
BEGIN
	SELECT @Total = COUNT(*) FROM [dbo].[Users]

	SELECT * FROM [dbo].[Users]
	ORDER BY [Id] ASC
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
