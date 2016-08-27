/*
	We store user settings separate from user records because of the following reasons:
	1. users and settings are separate entities
	2. not all users will have settings (admins/managers don't have settings)
	3. in order to make settings entity easily extendable
*/
CREATE TABLE [dbo].[UserSettings]
(
	[Id]			NVARCHAR(32) NOT NULL PRIMARY KEY,
	[Description]	NVARCHAR(128) NOT NULL
)
GO