/*
Meeting attendees should be able to receive notifications about upcoming meetings.
Attendees can opt to be notificed via either SMS or email
Meetings can be recurring and need to support different frequencies of recurrence (daily, weekly, multiple specific days of the week, etc.)
We want to display popular content more often and phase out unpopular content (track number of upvotes and downvotes for each content square).
Remove a table that is not needed anymore ("Xtraneous")
*/

/****** Object:  Table [dbo].[FrequencyType]    Script Date: 3/6/2018 12:11:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FrequencyType](
	[FrequencyTypeId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_FrequencyType] PRIMARY KEY CLUSTERED 
(
	[FrequencyTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingSchedule]    Script Date: 3/6/2018 12:11:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingSchedule](
	[MeetingScheduleId] [uniqueidentifier] NOT NULL,
	[MeetingId] [uniqueidentifier] NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[RecurrenceRuleId] [uniqueidentifier] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime2](7) NOT NULL,
	[UpdatedUserId] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MeetingSchedule] PRIMARY KEY CLUSTERED 
(
	[MeetingScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationMethodType]    Script Date: 3/6/2018 12:11:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationMethodType](
	[NotificationMethodTypeId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_NotificationMethodType] PRIMARY KEY CLUSTERED 
(
	[NotificationMethodTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationRule]    Script Date: 3/6/2018 12:11:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationRule](
	[NotificationRuleId] [uniqueidentifier] NOT NULL,
	[NotificationMethodTypeId] [int] NOT NULL,
	[MinutesBeforehand] [int] NOT NULL,
 CONSTRAINT [PK_NotificationRule] PRIMARY KEY CLUSTERED 
(
	[NotificationRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecurrenceRule]    Script Date: 3/6/2018 12:11:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecurrenceRule](
	[RecurrenceRuleId] [uniqueidentifier] NOT NULL,
	[FrequencyTypeId] [int] NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[Seconds] [int] NULL,
	[Minutes] [int] NULL,
	[Hour] [int] NULL,
	[WeekDayNum] [int] NULL,
	[OrdWeek] [int] NULL,
	[WeekDay] [nvarchar](50) NULL,
 CONSTRAINT [PK_RecurrenceRule] PRIMARY KEY CLUSTERED 
(
	[RecurrenceRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (8, N'Daily')
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (4, N'Hourly')
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (2, N'Minutely')
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (32, N'Monthly')
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (1, N'Secondly')
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (16, N'Weekly')
INSERT [dbo].[FrequencyType] ([FrequencyTypeId], [Name]) VALUES (64, N'Yearly')
INSERT [dbo].[MeetingSchedule] ([MeetingScheduleId], [MeetingId], [StartDate], [EndDate], [RecurrenceRuleId], [CreatedDate], [CreatedUserId], [UpdatedDate], [UpdatedUserId], [IsDeleted]) VALUES (N'422faa08-6f49-4a75-a0f0-56873a6d4bdc', N'a4e239cf-692a-4108-b9d3-c56a6e4eca2b', CAST(N'2018-02-26T08:30:00.0000000' AS DateTime2), CAST(N'2018-02-26T10:30:00.0000000' AS DateTime2), NULL, CAST(N'2018-02-18T10:38:01.0400000' AS DateTime2), N'b79ed0e3-ddb9-4920-8900-ffc55a73b4b5', CAST(N'2018-02-18T10:38:01.0400000' AS DateTime2), N'b79ed0e3-ddb9-4920-8900-ffc55a73b4b5', 0)
INSERT [dbo].[MeetingSchedule] ([MeetingScheduleId], [MeetingId], [StartDate], [EndDate], [RecurrenceRuleId], [CreatedDate], [CreatedUserId], [UpdatedDate], [UpdatedUserId], [IsDeleted]) VALUES (N'f0ad51bc-7083-4fec-bf0a-8961e116e3c1', N'47560033-421b-48c2-b8e6-88b878610835', CAST(N'2018-02-27T10:30:00.0000000' AS DateTime2), CAST(N'2018-02-27T12:00:00.0000000' AS DateTime2), NULL, CAST(N'2018-02-18T10:38:55.1333333' AS DateTime2), N'b79ed0e3-ddb9-4920-8900-ffc55a73b4b5', CAST(N'2018-02-18T10:38:55.1333333' AS DateTime2), N'b79ed0e3-ddb9-4920-8900-ffc55a73b4b5', 0)
INSERT [dbo].[MeetingSchedule] ([MeetingScheduleId], [MeetingId], [StartDate], [EndDate], [RecurrenceRuleId], [CreatedDate], [CreatedUserId], [UpdatedDate], [UpdatedUserId], [IsDeleted]) VALUES (N'c5720489-3c0c-4ef0-aee6-ea58f3dfcdb4', N'8e99f97e-4f68-4308-beeb-43af72232a44', CAST(N'2018-02-26T01:30:00.0000000' AS DateTime2), CAST(N'2018-02-26T03:30:00.0000000' AS DateTime2), NULL, CAST(N'2018-02-18T10:38:25.0100000' AS DateTime2), N'b79ed0e3-ddb9-4920-8900-ffc55a73b4b5', CAST(N'2018-02-18T10:38:25.0100000' AS DateTime2), N'b79ed0e3-ddb9-4920-8900-ffc55a73b4b5', 0)
INSERT [dbo].[NotificationMethodType] ([NotificationMethodTypeId], [Name]) VALUES (1, N'SMS')
INSERT [dbo].[NotificationMethodType] ([NotificationMethodTypeId], [Name]) VALUES (2, N'Email')
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_FrequencyType]    Script Date: 3/6/2018 12:11:11 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UC_FrequencyType] ON [dbo].[FrequencyType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MeetingSchedule] ADD  CONSTRAINT [DF_MeetingSchedule_MeetingScheduleId]  DEFAULT (newid()) FOR [MeetingScheduleId]
GO
ALTER TABLE [dbo].[MeetingSchedule] ADD  CONSTRAINT [DF_MeetingSchedule_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[NotificationRule] ADD  CONSTRAINT [DF_NotificationRule_NotificationRuleId]  DEFAULT (newid()) FOR [NotificationRuleId]
GO
ALTER TABLE [dbo].[RecurrenceRule] ADD  CONSTRAINT [DF_RecurrenceRule_RecurrenceRuleId]  DEFAULT (newid()) FOR [RecurrenceRuleId]
GO
ALTER TABLE [dbo].[MeetingSchedule]  WITH CHECK ADD  CONSTRAINT [FK_MeetingSchedule_Meeting] FOREIGN KEY([MeetingId])
REFERENCES [dbo].[Meeting] ([MeetingId])
GO
ALTER TABLE [dbo].[MeetingSchedule] CHECK CONSTRAINT [FK_MeetingSchedule_Meeting]
GO
ALTER TABLE [dbo].[MeetingSchedule]  WITH CHECK ADD  CONSTRAINT [FK_MeetingSchedule_RecurrenceRule] FOREIGN KEY([RecurrenceRuleId])
REFERENCES [dbo].[RecurrenceRule] ([RecurrenceRuleId])
GO
ALTER TABLE [dbo].[MeetingSchedule] CHECK CONSTRAINT [FK_MeetingSchedule_RecurrenceRule]
GO
ALTER TABLE [dbo].[MeetingSchedule]  WITH CHECK ADD  CONSTRAINT [FK_MeetingSchedule_User_Created] FOREIGN KEY([CreatedUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[MeetingSchedule] CHECK CONSTRAINT [FK_MeetingSchedule_User_Created]
GO
ALTER TABLE [dbo].[MeetingSchedule]  WITH CHECK ADD  CONSTRAINT [FK_MeetingSchedule_User_Updated] FOREIGN KEY([UpdatedUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[MeetingSchedule] CHECK CONSTRAINT [FK_MeetingSchedule_User_Updated]
GO
ALTER TABLE [dbo].[NotificationRule]  WITH CHECK ADD  CONSTRAINT [FK_NotificationRule_NotificationMethodType] FOREIGN KEY([NotificationMethodTypeId])
REFERENCES [dbo].[NotificationMethodType] ([NotificationMethodTypeId])
GO
ALTER TABLE [dbo].[NotificationRule] CHECK CONSTRAINT [FK_NotificationRule_NotificationMethodType]
GO
ALTER TABLE [dbo].[RecurrenceRule]  WITH CHECK ADD  CONSTRAINT [FK_RecurrenceRule_FrequencyType] FOREIGN KEY([FrequencyTypeId])
REFERENCES [dbo].[FrequencyType] ([FrequencyTypeId])
GO
ALTER TABLE [dbo].[RecurrenceRule] CHECK CONSTRAINT [FK_RecurrenceRule_FrequencyType]
GO




/*********************************
 * BingoContent
 *
 * NumberOfUpvotes, NumberOfDownvotes
 *********************************/



 /* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_Meeting
GO
ALTER TABLE dbo.Meeting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoContent
	DROP CONSTRAINT FK_BingoContent_User_CreatedUserId
GO
ALTER TABLE dbo.BingoContent
	DROP CONSTRAINT FK_BingoContent_User_UpdatedUserId
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_User
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_User_Created
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_User_Updated
GO
ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT DF_MeetingAttendee_MeetingAttendeeId
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT DF_MeetingAttendee_IsDeleted
GO
CREATE TABLE dbo.Tmp_MeetingAttendee
	(
	MeetingAttendeeId uniqueidentifier NOT NULL,
	MeetingId uniqueidentifier NOT NULL,
	UserId uniqueidentifier NOT NULL,
	NotificationRuleId uniqueidentifier NULL,
	CreatedDate datetime2(7) NOT NULL,
	CreatedUserId uniqueidentifier NOT NULL,
	UpdatedDate datetime2(7) NOT NULL,
	UpdatedUserId uniqueidentifier NOT NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_MeetingAttendee SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_MeetingAttendee ADD CONSTRAINT
	DF_MeetingAttendee_MeetingAttendeeId DEFAULT (newid()) FOR MeetingAttendeeId
GO
ALTER TABLE dbo.Tmp_MeetingAttendee ADD CONSTRAINT
	DF_MeetingAttendee_IsDeleted DEFAULT ((0)) FOR IsDeleted
GO
IF EXISTS(SELECT * FROM dbo.MeetingAttendee)
	 EXEC('INSERT INTO dbo.Tmp_MeetingAttendee (MeetingAttendeeId, MeetingId, UserId, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted)
		SELECT MeetingAttendeeId, MeetingId, UserId, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted FROM dbo.MeetingAttendee WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.MeetingAttendee
GO
EXECUTE sp_rename N'dbo.Tmp_MeetingAttendee', N'MeetingAttendee', 'OBJECT' 
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	PK_MeetingAttendee PRIMARY KEY CLUSTERED 
	(
	MeetingAttendeeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_Meeting FOREIGN KEY
	(
	MeetingId
	) REFERENCES dbo.Meeting
	(
	MeetingId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_User FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_User_Created FOREIGN KEY
	(
	CreatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_User_Updated FOREIGN KEY
	(
	UpdatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoContent
	DROP CONSTRAINT DF_BingoContent_BingoContentId
GO
ALTER TABLE dbo.BingoContent
	DROP CONSTRAINT DF_BingoContent_FreeSquareIndicator
GO
ALTER TABLE dbo.BingoContent
	DROP CONSTRAINT DF_BingoContent_IsDeleted
GO
CREATE TABLE dbo.Tmp_BingoContent
	(
	BingoContentId uniqueidentifier NOT NULL,
	[Content] nvarchar(250) NOT NULL,
	FreeSquareIndicator bit NOT NULL,
	NumberOfUpvotes int NOT NULL,
	NumberOfDownvotes int NOT NULL,
	CreatedDate datetime2(7) NOT NULL,
	CreatedUserId uniqueidentifier NOT NULL,
	UpdatedDate datetime2(7) NOT NULL,
	UpdatedUserId uniqueidentifier NOT NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_BingoContent SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_BingoContent ADD CONSTRAINT
	DF_BingoContent_BingoContentId DEFAULT (newid()) FOR BingoContentId
GO
ALTER TABLE dbo.Tmp_BingoContent ADD CONSTRAINT
	DF_BingoContent_FreeSquareIndicator DEFAULT ((0)) FOR FreeSquareIndicator
GO
ALTER TABLE dbo.Tmp_BingoContent ADD CONSTRAINT
	DF_BingoContent_NumberOfUpvotes DEFAULT 0 FOR NumberOfUpvotes
GO
ALTER TABLE dbo.Tmp_BingoContent ADD CONSTRAINT
	DF_BingoContent_NumberOfDownvotes DEFAULT 0 FOR NumberOfDownvotes
GO
ALTER TABLE dbo.Tmp_BingoContent ADD CONSTRAINT
	DF_BingoContent_IsDeleted DEFAULT ((0)) FOR IsDeleted
GO
IF EXISTS(SELECT * FROM dbo.BingoContent)
	 EXEC('INSERT INTO dbo.Tmp_BingoContent (BingoContentId, [Content], FreeSquareIndicator, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted)
		SELECT BingoContentId, [Content], FreeSquareIndicator, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted FROM dbo.BingoContent WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT FK_BingoInstanceContent_BingoContent
GO
DROP TABLE dbo.BingoContent
GO
EXECUTE sp_rename N'dbo.Tmp_BingoContent', N'BingoContent', 'OBJECT' 
GO
ALTER TABLE dbo.BingoContent ADD CONSTRAINT
	PK_BingoContent PRIMARY KEY CLUSTERED 
	(
	BingoContentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.BingoContent ADD CONSTRAINT
	FK_BingoContent_User_CreatedUserId FOREIGN KEY
	(
	CreatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoContent ADD CONSTRAINT
	FK_BingoContent_User_UpdatedUserId FOREIGN KEY
	(
	UpdatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_BingoContent FOREIGN KEY
	(
	BingoContentId
	) REFERENCES dbo.BingoContent
	(
	BingoContentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceContent SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/*********************************
* BingoInstanceContentStatusType
**********************************/

/****** Object:  Table [dbo].[BingoInstanceContentStatusType]    Script Date: 3/6/2018 12:40:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BingoInstanceContentStatusType](
	[BingoInstanceContentStatusTypeId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_BingoInstanceContentStatusType] PRIMARY KEY CLUSTERED 
(
	[BingoInstanceContentStatusTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[BingoInstanceContentStatusType] ([BingoInstanceContentStatusTypeId], [Name]) VALUES (4, N'Disputed')
GO
INSERT [dbo].[BingoInstanceContentStatusType] ([BingoInstanceContentStatusTypeId], [Name]) VALUES (2, N'Tapped')
GO
INSERT [dbo].[BingoInstanceContentStatusType] ([BingoInstanceContentStatusTypeId], [Name]) VALUES (1, N'Untapped')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_BingoInstanceContentStatusType]    Script Date: 3/6/2018 12:40:48 AM ******/
ALTER TABLE [dbo].[BingoInstanceContentStatusType] ADD  CONSTRAINT [UC_BingoInstanceContentStatusType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*********************************
 * BingoInstanceContent
 * 
 * UserId, BingoInstanceContentStatusTypeId
 *********************************/

 BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceContentStatusType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT FK_BingoInstanceContent_BingoInstance
GO
ALTER TABLE dbo.BingoInstance SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT FK_BingoInstanceContent_BingoContent
GO
ALTER TABLE dbo.BingoContent SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT FK_BingoInstanceContent_User_Created
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT FK_BingoInstanceContent_User_Updated
GO
ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT DF_BingoInstanceContent_BingoInstanceContentId
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT DF_BingoInstanceContent_FreeSquareIndicator
GO
ALTER TABLE dbo.BingoInstanceContent
	DROP CONSTRAINT DF_BingoInstanceContent_IsDeleted
GO
CREATE TABLE dbo.Tmp_BingoInstanceContent
	(
	BingoInstanceContentId uniqueidentifier NOT NULL,
	BingoContentId uniqueidentifier NOT NULL,
	BingoInstanceId uniqueidentifier NOT NULL,
	UserId uniqueidentifier NOT NULL,
	Col int NOT NULL,
	Row int NOT NULL,
	FreeSquareIndicator bit NOT NULL,
	BingoInstanceContentStatusTypeId int NOT NULL,
	CreatedDate datetime2(7) NOT NULL,
	CreatedUserId uniqueidentifier NOT NULL,
	UpdatedDate datetime2(7) NOT NULL,
	UpdatedUserId uniqueidentifier NOT NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_BingoInstanceContent SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_BingoInstanceContent ADD CONSTRAINT
	DF_BingoInstanceContent_BingoInstanceContentId DEFAULT (newid()) FOR BingoInstanceContentId
GO
ALTER TABLE dbo.Tmp_BingoInstanceContent ADD CONSTRAINT
	DF_BingoInstanceContent_FreeSquareIndicator DEFAULT ((1)) FOR FreeSquareIndicator
GO
ALTER TABLE dbo.Tmp_BingoInstanceContent ADD CONSTRAINT
	DF_BingoInstanceContent_BingoInstanceContentStatusTypeId DEFAULT 1 FOR BingoInstanceContentStatusTypeId
GO
ALTER TABLE dbo.Tmp_BingoInstanceContent ADD CONSTRAINT
	DF_BingoInstanceContent_IsDeleted DEFAULT ((0)) FOR IsDeleted
GO
IF EXISTS(SELECT * FROM dbo.BingoInstanceContent)
	 EXEC('INSERT INTO dbo.Tmp_BingoInstanceContent (BingoInstanceContentId, BingoContentId, BingoInstanceId, Col, Row, FreeSquareIndicator, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted)
		SELECT BingoInstanceContentId, BingoContentId, BingoInstanceId, Col, Row, FreeSquareIndicator, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted FROM dbo.BingoInstanceContent WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.BingoInstanceEvent
	DROP CONSTRAINT FK_BingoInstanceEvent_BingoInstanceContent
GO
DROP TABLE dbo.BingoInstanceContent
GO
EXECUTE sp_rename N'dbo.Tmp_BingoInstanceContent', N'BingoInstanceContent', 'OBJECT' 
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	PK_BingoInstanceContent PRIMARY KEY CLUSTERED 
	(
	BingoInstanceContentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_User_Created FOREIGN KEY
	(
	CreatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_User_Updated FOREIGN KEY
	(
	UpdatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_BingoContent FOREIGN KEY
	(
	BingoContentId
	) REFERENCES dbo.BingoContent
	(
	BingoContentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_BingoInstance FOREIGN KEY
	(
	BingoInstanceId
	) REFERENCES dbo.BingoInstance
	(
	BingoInstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_User FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceContent ADD CONSTRAINT
	FK_BingoInstanceContent_BingoInstanceContentStatusType FOREIGN KEY
	(
	BingoInstanceContentStatusTypeId
	) REFERENCES dbo.BingoInstanceContentStatusType
	(
	BingoInstanceContentStatusTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BingoInstanceEvent ADD CONSTRAINT
	FK_BingoInstanceEvent_BingoInstanceContent FOREIGN KEY
	(
	BingoInstanceContentId
	) REFERENCES dbo.BingoInstanceContent
	(
	BingoInstanceContentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BingoInstanceEvent SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



/*********************************
 * MeetingAttendee.NotificationRuleId
 *********************************/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_User
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_User_Created
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_User_Updated
GO
ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT FK_MeetingAttendee_Meeting
GO
ALTER TABLE dbo.Meeting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT DF_MeetingAttendee_MeetingAttendeeId
GO
ALTER TABLE dbo.MeetingAttendee
	DROP CONSTRAINT DF_MeetingAttendee_IsDeleted
GO
CREATE TABLE dbo.Tmp_MeetingAttendee
	(
	MeetingAttendeeId uniqueidentifier NOT NULL,
	MeetingId uniqueidentifier NOT NULL,
	UserId uniqueidentifier NOT NULL,
	NotificationRuleId uniqueidentifier NULL,
	CreatedDate datetime2(7) NOT NULL,
	CreatedUserId uniqueidentifier NOT NULL,
	UpdatedDate datetime2(7) NOT NULL,
	UpdatedUserId uniqueidentifier NOT NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_MeetingAttendee SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_MeetingAttendee ADD CONSTRAINT
	DF_MeetingAttendee_MeetingAttendeeId DEFAULT (newid()) FOR MeetingAttendeeId
GO
ALTER TABLE dbo.Tmp_MeetingAttendee ADD CONSTRAINT
	DF_MeetingAttendee_IsDeleted DEFAULT ((0)) FOR IsDeleted
GO
IF EXISTS(SELECT * FROM dbo.MeetingAttendee)
	 EXEC('INSERT INTO dbo.Tmp_MeetingAttendee (MeetingAttendeeId, MeetingId, UserId, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted)
		SELECT MeetingAttendeeId, MeetingId, UserId, CreatedDate, CreatedUserId, UpdatedDate, UpdatedUserId, IsDeleted FROM dbo.MeetingAttendee WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.MeetingAttendee
GO
EXECUTE sp_rename N'dbo.Tmp_MeetingAttendee', N'MeetingAttendee', 'OBJECT' 
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	PK_MeetingAttendee PRIMARY KEY CLUSTERED 
	(
	MeetingAttendeeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_Meeting FOREIGN KEY
	(
	MeetingId
	) REFERENCES dbo.Meeting
	(
	MeetingId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_User FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_User_Created FOREIGN KEY
	(
	CreatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MeetingAttendee ADD CONSTRAINT
	FK_MeetingAttendee_User_Updated FOREIGN KEY
	(
	UpdatedUserId
	) REFERENCES dbo.[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT


ALTER TABLE [dbo].[MeetingAttendee]  WITH CHECK ADD  CONSTRAINT [FK_MeetingAttendee_NotificationRule] FOREIGN KEY([NotificationRuleId])
REFERENCES [dbo].[NotificationRule] ([NotificationRuleId])
GO

ALTER TABLE [dbo].[MeetingAttendee] CHECK CONSTRAINT [FK_MeetingAttendee_NotificationRule]
GO


