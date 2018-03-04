CREATE TABLE [dbo].[NotificationRule] (
    [NotificationRuleId]       UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationRule_NotificationRuleId] DEFAULT (newid()) NOT NULL,
    [NotificationMethodTypeId] INT              NOT NULL,
    [MinutesBeforehand]        INT              NOT NULL,
    CONSTRAINT [PK_NotificationRule] PRIMARY KEY CLUSTERED ([NotificationRuleId] ASC),
    CONSTRAINT [FK_NotificationRule_NotificationMethodType] FOREIGN KEY ([NotificationMethodTypeId]) REFERENCES [dbo].[NotificationMethodType] ([NotificationMethodTypeId])
);

