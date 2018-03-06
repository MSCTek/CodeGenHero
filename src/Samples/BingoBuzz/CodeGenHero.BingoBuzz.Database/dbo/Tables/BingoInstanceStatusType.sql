CREATE TABLE [dbo].[BingoInstanceStatusType] (
    [BingoInstanceStatusTypeId] INT            NOT NULL,
    [Name]                      NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_BingoInstanceStatusType] PRIMARY KEY CLUSTERED ([BingoInstanceStatusTypeId] ASC),
    CONSTRAINT [UC_BingoInstanceStatusType] UNIQUE NONCLUSTERED ([Name] ASC)
);



