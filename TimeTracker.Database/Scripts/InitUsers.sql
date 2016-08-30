DECLARE @adminRoles AS KeyCollection
INSERT INTO @adminRoles ([Id]) VALUES (N'user')
INSERT INTO @adminRoles ([Id]) VALUES (N'manager')
INSERT INTO @adminRoles ([Id]) VALUES (N'administrator')

DECLARE @adminSettings AS KeyValueCollection

-- pass: 123456
EXEC [dbo].[SaveUser] 0, N'admin', N'Gc9zQji6q183pegVZfyqZltTYjGRCvir573eoq8KBvo=', N'fIA2eJam', N'Administrator', N'active', @adminRoles, @adminSettings