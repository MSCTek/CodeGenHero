CREATE TABLE [dbo].[MeetingAttendee] (
    [MeetingAttendeeId]  UNIQUEIDENTIFIER CONSTRAINT [DF_MeetingAttendee_MeetingAttendeeId] DEFAULT (newid()) NOT NULL,
    [MeetingId]          UNIQUEIDENTIFIER NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [NotificationRuleId] UNIQUEIDENTIFIER NULL,
    [CreatedDate]        DATETIME2 (7)    NOT NULL,
    [CreatedUserId]      UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]        DATETIME2 (7)    NOT NULL,
    [UpdatedUserId]      UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]          BIT              CONSTRAINT [DF_MeetingAttendee_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MeetingAttendee] PRIMARY KEY CLUSTERED ([MeetingAttendeeId] ASC),
    CONSTRAINT [FK_MeetingAttendee_Meeting] FOREIGN KEY ([MeetingId]) REFERENCES [dbo].[Meeting] ([MeetingId]),
    CONSTRAINT [FK_MeetingAttendee_NotificationRule] FOREIGN KEY ([NotificationRuleId]) REFERENCES [dbo].[NotificationRule] ([NotificationRuleId]),
    CONSTRAINT [FK_MeetingAttendee_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_MeetingAttendee_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_MeetingAttendee_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

