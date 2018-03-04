CREATE TABLE [dbo].[BingoInstanceEventType] (
    [BingoInstanceEventTypeId] INT            NOT NULL,
    [Name]                     NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_BingoInstanceEventType] PRIMARY KEY CLUSTERED ([BingoInstanceEventTypeId] ASC)
);

