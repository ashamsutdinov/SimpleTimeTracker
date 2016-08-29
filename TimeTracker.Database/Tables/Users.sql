CREATE TABLE [dbo].[Users]
(
	[Id]			INT NOT NULL IDENTITY PRIMARY KEY,
	[Login]			NVARCHAR(64) NOT NULL,
	-- sha1(password+salt)
	[PasswordHash]	NVARCHAR(64) NOT NULL,
	[PasswordSalt]	NVARCHAR(16) NOT NULL,
	[Name]			NVARCHAR(128) NOT NULL,
	[StateId]		NVARCHAR(32) NOT NULL,
	CONSTRAINT [Fk.Users.StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[UserStates] ([Id])
)
GO

CREATE INDEX [IDX.Users.Login] ON [dbo].[Users]([Login])
GO
