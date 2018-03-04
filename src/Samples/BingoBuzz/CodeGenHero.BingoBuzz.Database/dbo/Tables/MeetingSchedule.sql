CREATE TABLE [dbo].[MeetingSchedule] (
    [MeetingScheduleId] UNIQUEIDENTIFIER CONSTRAINT [DF_MeetingSchedule_MeetingScheduleId] DEFAULT (newid()) NOT NULL,
    [MeetingId]         UNIQUEIDENTIFIER NULL,
    [StartDate]         DATETIME2 (7)    NULL,
    [EndDate]           DATETIME2 (7)    NULL,
    [RecurrenceRuleId]  UNIQUEIDENTIFIER NULL,
    [CreatedDate]       DATETIME2 (7)    NOT NULL,
    [CreatedUserId]     UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]       DATETIME2 (7)    NOT NULL,
    [UpdatedUserId]     UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF_MeetingSchedule_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MeetingSchedule] PRIMARY KEY CLUSTERED ([MeetingScheduleId] ASC),
    CONSTRAINT [FK_MeetingSchedule_Meeting] FOREIGN KEY ([MeetingId]) REFERENCES [dbo].[Meeting] ([MeetingId]),
    CONSTRAINT [FK_MeetingSchedule_RecurrenceRule] FOREIGN KEY ([RecurrenceRuleId]) REFERENCES [dbo].[RecurrenceRule] ([RecurrenceRuleId]),
    CONSTRAINT [FK_MeetingSchedule_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_MeetingSchedule_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

