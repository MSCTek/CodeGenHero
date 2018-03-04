CREATE TABLE [dbo].[User] (
    [UserId]        UNIQUEIDENTIFIER CONSTRAINT [DF_User_UserId] DEFAULT (newid()) NOT NULL,
    [CompanyId]     UNIQUEIDENTIFIER NOT NULL,
    [Email]         NVARCHAR (250)   NOT NULL,
    [FirstName]     NVARCHAR (50)    NOT NULL,
    [LastName]      NVARCHAR (50)    NOT NULL,
    [CreatedDate]   DATETIME2 (7)    NOT NULL,
    [CreatedUserId] UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]   DATETIME2 (7)    NOT NULL,
    [UpdatedUserId] UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]     BIT              CONSTRAINT [DF_User_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

