CREATE TABLE [dbo].[TimeRecordNotes]
(
	[Id]			INT NOT NULL IDENTITY PRIMARY KEY,
	[TimeRecordId]	INT NOT NULL,
	[UserId]		INT NOT NULL,
	[DateTime]		DATETIME2 NOT NULL,
	[Text]			NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [FK.TimeRecordNotes.TimeRecordId] FOREIGN KEY ([TimeRecordId]) REFERENCES [dbo].[TimeRecords] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK.TimeRecordNotes.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
)
GO

CREATE INDEX [IDX.TimeRecordNotes.TimeRecordId] ON [dbo].[TimeRecordNotes] ([TimeRecordId])
GO
