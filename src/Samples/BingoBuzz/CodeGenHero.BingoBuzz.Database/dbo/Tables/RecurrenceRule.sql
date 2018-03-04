CREATE TABLE [dbo].[RecurrenceRule] (
    [RecurrenceRuleId] UNIQUEIDENTIFIER CONSTRAINT [DF_RecurrenceRule_RecurrenceRuleId] DEFAULT (newid()) NOT NULL,
    [FrequencyTypeId]  INT              NOT NULL,
    [EndDate]          DATETIME2 (7)    NULL,
    [Seconds]          INT              NULL,
    [Minutes]          INT              NULL,
    [Hour]             INT              NULL,
    [WeekDayNum]       INT              NULL,
    [OrdWeek]          INT              NULL,
    [WeekDay]          NVARCHAR (50)    NULL,
    CONSTRAINT [PK_RecurrenceRule] PRIMARY KEY CLUSTERED ([RecurrenceRuleId] ASC),
    CONSTRAINT [FK_RecurrenceRule_FrequencyType] FOREIGN KEY ([FrequencyTypeId]) REFERENCES [dbo].[FrequencyType] ([FrequencyTypeId])
);

