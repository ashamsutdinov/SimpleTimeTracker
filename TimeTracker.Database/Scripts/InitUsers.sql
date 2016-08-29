DECLARE @adminRoles AS KeyCollection
INSERT INTO @adminRoles ([Id]) VALUES (N'user')
INSERT INTO @adminRoles ([Id]) VALUES (N'manager')
INSERT INTO @adminRoles ([Id]) VALUES (N'administrator')

DECLARE @adminSettings AS KeyValueCollection

-- pass: 123456
EXEC [dbo].[SaveUser] 0, N'admin', N'c632f2af8a456e5c438672ed02aaa4da6b6685bf', N'VstLW', N'Administrator', N'active', @adminRoles, @adminSettings