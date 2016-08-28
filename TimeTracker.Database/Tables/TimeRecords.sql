CREATE TABLE [dbo].[TimeRecords]
(
	[Id]			INT NOT NULL IDENTITY PRIMARY KEY,
	[DayRecordId]	INT NOT NULL,
	[Hours]			INT NOT NULL,
	[Caption]		NVARCHAR(256) NOT NULL,
	[Deleted]		BIT NOT NULL DEFAULT 0
	CONSTRAINT [FK.TimeRecords.DayRecordId] FOREIGN KEY ([DayRecordId]) REFERENCES [dbo].[DayRecords] ([Id])
)
GO

CREATE INDEX [IDX.TimeRecords.DayRecordId] ON [dbo].[TimeRecords] ([DayRecordId])
GO

CREATE INDEX [IDX.TimeRecords.Deleted] ON [dbo].[TimeRecords] ([Deleted])
GO