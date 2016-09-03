DECLARE @defaultSettings KeyValueCollection
INSERT INTO @defaultSettings ([Id], [Value]) VALUES (N'PreferredWorkingHoursPerDay', N'8')

-- all password: 123456

DECLARE @adminRoles KeyCollection
INSERT INTO @adminRoles ([Id]) VALUES (N'user')
INSERT INTO @adminRoles ([Id]) VALUES (N'manager')
INSERT INTO @adminRoles ([Id]) VALUES (N'administrator')
EXEC [dbo].[SaveUser] 0, N'admin', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'System Administrator', N'active', @adminRoles, @defaultSettings

DECLARE @managerRoles KeyCollection
INSERT INTO @managerRoles ([Id]) VALUES (N'user')
INSERT INTO @managerRoles ([Id]) VALUES (N'manager')
EXEC [dbo].[SaveUser] 0, N'manager', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'User Manager', N'active', @managerRoles, @defaultSettings

DECLARE @userRoles KeyCollection
INSERT INTO @userRoles ([Id]) VALUES (N'user')
EXEC [dbo].[SaveUser] 0, N'user0', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #0', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user1', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #1', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user2', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #2', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user3', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #3', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user4', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #4', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user5', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #5', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user6', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #6', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user7', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #7', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user8', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #8', N'active', @userRoles, @defaultSettings
EXEC [dbo].[SaveUser] 0, N'user9', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Test User #9', N'active', @userRoles, @defaultSettings