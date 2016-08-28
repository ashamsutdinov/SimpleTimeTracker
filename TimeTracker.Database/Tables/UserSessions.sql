CREATE TABLE [dbo].[UserSessions]
(
	[Id]			INT NOT NULL IDENTITY PRIMARY KEY,
	[UserId]		INT NOT NULL,
	[Ticket]		NVARCHAR(256) NOT NULL,
	[Expired]		BIT NOT NULL DEFAULT 0,
	-- datetime user last accessed the session
	[DateTime]		DATETIME2 NOT NULL,
	-- session expiration, in seconds
	[Expiration]	INT NOT NULL,
	CONSTRAINT [FK.UserSessions.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
)
GO

CREATE INDEX [IDX.UserSessions.UserId] ON [dbo].[UserSessions] ([UserId])
GO

CREATE INDEX [IDX.UserSessions.Ticket] ON [dbo].[UserSessions] ([Ticket])
GO

CREATE INDEX [IDX.UserSessions.Expired] ON [dbo].[UserSessions] ([Expired])
GO

CREATE INDEX [IDX.UserSessions.DateTime] ON [dbo].[UserSessions] ([DateTime])
GO
