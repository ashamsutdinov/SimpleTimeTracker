CREATE TABLE [dbo].[DayRecords]
(
	[Id]			INT NOT NULL IDENTITY PRIMARY KEY,
	[UserId]		INT NOT NULL PRIMARY KEY,
	[Date]			DATE NOT NULL,
	[TotalHours]	INT NOT NULL DEFAULT 0,
	CONSTRAINT [FK.DayRecords.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
)
GO

CREATE INDEX [IDX.DayRecords.UserId] ON [dbo].[DayRecords] ([UserId])
GO

CREATE INDEX [IDX.DayRecods.TotalHours] ON [dbo].[DayRecords] ([TotalHours])
GO