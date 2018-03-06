CREATE TABLE [dbo].[BingoInstanceContentStatusType] (
    [BingoInstanceContentStatusTypeId] INT            NOT NULL,
    [Name]                             NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_BingoInstanceContentStatusType] PRIMARY KEY CLUSTERED ([BingoInstanceContentStatusTypeId] ASC),
    CONSTRAINT [UC_BingoInstanceContentStatusType] UNIQUE NONCLUSTERED ([Name] ASC)
);

