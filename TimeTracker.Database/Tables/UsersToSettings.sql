CREATE TABLE [dbo].[UsersToSettings]
(
	[UserId]	INT NOT NULL,
	[SettingId] NVARCHAR(32) NOT NULL,
	[Value]		NVARCHAR(128) NOT NULL
	CONSTRAINT [FK.UsersToSettings.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK.UsersToSettings.SettingId] FOREIGN KEY ([SettingId]) REFERENCES [dbo].[UserSettings] ([Id]),
	CONSTRAINT [PK.UsersToSettings] PRIMARY KEY ([UserId], [SettingId])
)
GO
