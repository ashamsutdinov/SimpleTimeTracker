CREATE TYPE [dbo].[KeyValueCollection] AS TABLE
(
	[Id] NVARCHAR(32) NOT NULL,
	[Value] NVARCHAR(128) NOT NULL
)
GO
