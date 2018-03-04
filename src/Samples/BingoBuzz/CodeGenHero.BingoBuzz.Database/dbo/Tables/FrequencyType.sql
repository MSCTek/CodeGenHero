CREATE TABLE [dbo].[FrequencyType] (
    [FrequencyTypeId] INT            NOT NULL,
    [Name]            NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_FrequencyType] PRIMARY KEY CLUSTERED ([FrequencyTypeId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_FrequencyType]
    ON [dbo].[FrequencyType]([Name] ASC);

