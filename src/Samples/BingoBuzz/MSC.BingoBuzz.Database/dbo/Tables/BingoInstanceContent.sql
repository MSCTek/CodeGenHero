CREATE TABLE [dbo].[BingoInstanceContent] (
    [BingoInstanceContentId] UNIQUEIDENTIFIER CONSTRAINT [DF_BingoInstanceContent_BingoInstanceContentId] DEFAULT (newid()) NOT NULL,
    [BingoInstanceId]        UNIQUEIDENTIFIER NOT NULL,
    [Col]                    INT              NOT NULL,
    [Row]                    INT              NOT NULL,
    [FreeSquareIndicator]    BIT              CONSTRAINT [DF_BingoInstanceContent_FreeSquareIndicator] DEFAULT ((1)) NOT NULL,
    [CreatedDate]            DATETIME2 (7)    NOT NULL,
    [CreatedUserId]          UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]            DATETIME2 (7)    NOT NULL,
    [UpdatedUserId]          UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]              BIT              CONSTRAINT [DF_BingoInstanceContent_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BingoInstanceContent] PRIMARY KEY CLUSTERED ([BingoInstanceContentId] ASC),
    CONSTRAINT [FK_BingoInstanceContent_BingoInstance] FOREIGN KEY ([BingoInstanceId]) REFERENCES [dbo].[BingoInstance] ([BingoInstanceId]),
    CONSTRAINT [FK_BingoInstanceContent_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_BingoInstanceContent_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

