CREATE TABLE [dbo].[NotificationMethodType] (
    [NotificationMethodTypeId] INT            NOT NULL,
    [Name]                     NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_NotificationMethodType] PRIMARY KEY CLUSTERED ([NotificationMethodTypeId] ASC)
);

