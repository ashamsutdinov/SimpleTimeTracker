CREATE TABLE [dbo].[TimeRecordNotes]
(
	[Id]			INT NOT NULL IDENTITY PRIMARY KEY,
	[DayRecordId]	INT NOT NULL,
	[UserId]		INT NOT NULL,
	[DateTime]		DATETIME2 NOT NULL,
	[Text]			NVARCHAR(MAX) NOT NULL,
	[Deleted]		BIT NOT NULL DEFAULT 0
	CONSTRAINT [FK.TimeRecordNotes.DayRecordId] FOREIGN KEY ([DayRecordId]) REFERENCES [dbo].[DayRecords] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK.TimeRecordNotes.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
)
GO

CREATE INDEX [IDX.TimeRecordNotes.TimeRecordId] ON [dbo].[TimeRecordNotes] ([DayRecordId])
GO
