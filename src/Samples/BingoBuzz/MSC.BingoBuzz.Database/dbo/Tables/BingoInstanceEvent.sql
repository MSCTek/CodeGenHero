CREATE TABLE [dbo].[BingoInstanceEvent] (
    [BingoInstanceEventId]     UNIQUEIDENTIFIER CONSTRAINT [DF_BingoInstanceEvent_BingoInstanceEventId] DEFAULT (newid()) NOT NULL,
    [BingoInstanceEventTypeId] INT              NOT NULL,
    [BingoInstanceId]          UNIQUEIDENTIFIER NULL,
    [BingoInstanceContentId]   UNIQUEIDENTIFIER NULL,
    [CreatedDate]              DATETIME2 (7)    NOT NULL,
    [CreatedUserId]            UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]              DATETIME2 (7)    NOT NULL,
    [UpdatedUserId]            UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]                BIT              CONSTRAINT [DF_BingoInstanceEvent_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BingoInstanceEvent] PRIMARY KEY CLUSTERED ([BingoInstanceEventId] ASC),
    CONSTRAINT [FK_BingoInstanceEvent_BingoInstance] FOREIGN KEY ([BingoInstanceId]) REFERENCES [dbo].[BingoInstance] ([BingoInstanceId]),
    CONSTRAINT [FK_BingoInstanceEvent_BingoInstanceContent] FOREIGN KEY ([BingoInstanceContentId]) REFERENCES [dbo].[BingoInstanceContent] ([BingoInstanceContentId]),
    CONSTRAINT [FK_BingoInstanceEvent_BingoInstanceEventType] FOREIGN KEY ([BingoInstanceEventTypeId]) REFERENCES [dbo].[BingoInstanceEventType] ([BingoInstanceEventTypeId]),
    CONSTRAINT [FK_BingoInstanceEvent_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_BingoInstanceEvent_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

