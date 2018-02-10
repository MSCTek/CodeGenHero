CREATE TABLE [dbo].[BingoContent] (
    [BingoContentId]      UNIQUEIDENTIFIER CONSTRAINT [DF_BingoContent_BingoContentId] DEFAULT (newid()) NOT NULL,
    [Content]             NVARCHAR (250)   NOT NULL,
    [FreeSquareIndicator] BIT              CONSTRAINT [DF_BingoContent_FreeSquareIndicator] DEFAULT ((0)) NOT NULL,
    [NumberOfUpvotes]     INT              CONSTRAINT [DF_BingoContent_NumberOfUpvotes] DEFAULT ((0)) NOT NULL,
    [NumberOfDownvotes]   INT              CONSTRAINT [DF_BingoContent_NumberOfDownvotes] DEFAULT ((0)) NOT NULL,
    [CreatedDate]         DATETIME2 (7)    NOT NULL,
    [CreatedUserId]       UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]         DATETIME2 (7)    NOT NULL,
    [UpdatedUserId]       UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]           BIT              CONSTRAINT [DF_BingoContent_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_BingoContent_User_CreatedUserId] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_BingoContent_User_UpdatedUserId] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

