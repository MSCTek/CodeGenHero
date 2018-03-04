CREATE TABLE [dbo].[Meeting] (
    [MeetingId]     UNIQUEIDENTIFIER CONSTRAINT [DF_Meeting_MeetingId] DEFAULT (newid()) NOT NULL,
    [CompanyId]     UNIQUEIDENTIFIER NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [CreatedDate]   DATETIME2 (7)    NOT NULL,
    [CreatedUserId] UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate]   DATETIME2 (7)    NOT NULL,
    [UpdatedUserId] UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]     BIT              CONSTRAINT [DF_Meeting_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED ([MeetingId] ASC),
    CONSTRAINT [FK_Meeting_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]),
    CONSTRAINT [FK_Meeting_User_Created] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_Meeting_User_Updated] FOREIGN KEY ([UpdatedUserId]) REFERENCES [dbo].[User] ([UserId])
);

