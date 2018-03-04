CREATE TABLE [dbo].[BingoInstance] (
    [BingoInstanceId]            UNIQUEIDENTIFIER CONSTRAINT [DF_BingoInstance_BingoInstanceId] DEFAULT (newid()) NOT NULL,
    [MeetingId]                  UNIQUEIDENTIFIER NULL,
    [NumberOfColumns]            INT              NOT NULL,
    [NumberOfRows]               INT              NOT NULL,
    [IncludeFreeSquareIndicator] BIT              CONSTRAINT [DF_BingoInstance_IncludeFreeSquareIndicator] DEFAULT ((1)) NOT NULL,
    [CreatedDate]                DATETIME2 (7)    NOT NULL,
    [CreatedUserId]              UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]                DATETIME2 (7)    NOT NULL,
    [UpdatedUserId]              UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]                  BIT              CONSTRAINT [DF_BingoInstance_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BingoInstance] PRIMARY KEY CLUSTERED ([BingoInstanceId] ASC),
    CONSTRAINT [FK_BingoInstance_Meeting] FOREIGN KEY ([MeetingId]) REFERENCES [dbo].[Meeting] ([MeetingId]),
    CONSTRAINT [FK_BingoInstance_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_BingoInstance_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

