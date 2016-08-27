CREATE TABLE [dbo].[UsersToRoles]
(
	[UserId] INT NOT NULL,
	[RoleId] NVARCHAR(32) NOT NULL,
	CONSTRAINT [FK.UsersToRoles.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]),
	CONSTRAINT [FK.UsersToRoles.RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[UserRoles]([Id]),
	CONSTRAINT [PK.UsersToRoles] PRIMARY KEY ([UserId], [RoleId])
)
GO
