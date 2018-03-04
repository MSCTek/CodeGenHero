CREATE TABLE [dbo].[Company] (
    [CompanyId]     UNIQUEIDENTIFIER CONSTRAINT [DF_Company_CompanyId] DEFAULT (newid()) NOT NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [CodeName]      NVARCHAR (50)    NOT NULL,
    [Address1]      NVARCHAR (100)   NULL,
    [Address2]      NVARCHAR (100)   NULL,
    [City]          NVARCHAR (100)   NULL,
    [State]         NVARCHAR (50)    NULL,
    [Zip]           NVARCHAR (50)    NULL,
    [WebsiteUrl]    NVARCHAR (100)   NULL,
    [CreatedDate]   DATETIME2 (7)    NOT NULL,
    [CreatedUserId] UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]   DATETIME2 (7)    NOT NULL,
    [UpdatedUserId] UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]     BIT              CONSTRAINT [DF_Company_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([CompanyId] ASC),
    CONSTRAINT [FK_Company_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_Company_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [UC_Company_CodeName] UNIQUE NONCLUSTERED ([CodeName] ASC)
);

