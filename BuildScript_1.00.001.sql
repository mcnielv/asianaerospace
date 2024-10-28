DECLARE @dbname nvarchar(128)
SET @dbname = N'AacDB'

IF (NOT EXISTS (SELECT name 
FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = @dbname 
OR name = @dbname)))
BEGIN
	CREATE Database [AacDB] 
END
GO
USE [AacDB]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 04/21/2016 09:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TriggeredTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_TriggeredTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TriggeredTable] ADD  CONSTRAINT [DF_TriggeredTable_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[TriggeredTable] ADD  CONSTRAINT [DF_TriggeredTable_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](150) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](350) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[SSS] [nvarchar](50) NOT NULL,
	[TIN] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_RoleID]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Address]  DEFAULT ('') FOR [Address]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Phone]  DEFAULT ('') FOR [Phone]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
-- =============================================
-- Author:		Viray, McNiel N.
-- Create date: 21 April 2016
-- Description:	Trigger that will determine if someone deleted table 
-- =============================================
CREATE TRIGGER [dbo].[RoleDelete]
   ON  [dbo].[Role]
   AFTER DELETE
AS 
BEGIN
	DECLARE @user NVARCHAR(50)
		,@host NVARCHAR(50)
		,@roleID INT
		,@Desc NVARCHAR(300)
		
	SELECT @roleID = d.ID
	FROM deleted d
	UPDATE [Role]
	SET DateModified=GETDATE()
	WHERE ID=@roleID
	
	SELECT @user = SUSER_NAME()
		,@host = HOST_NAME()
		
	SET @Desc='Data deleted on [dbo].[Role]|user:'+@user+'|host:'+@host+'|RoleID:'+ CAST( @roleID as VARCHAR(10))
	INSERT INTO [dbo].[TriggeredTable](TableName,[Description])
	VALUES('[dbo].[Role]',@Desc)
		
	DELETE FROM [dbo].[TriggeredTable] WHERE [Description] IS NULL
END
GO
-- =============================================
-- Author:		Viray, McNiel N.
-- Create date: 21 April 2016
-- Description:	Trigger that will determine if someone created table 
-- =============================================
CREATE TRIGGER [dbo].[RoleInsert]
   ON  [dbo].[Role]
   FOR INSERT
AS 
BEGIN
	DECLARE @tablename NVARCHAR(100)
		,@roleID INT
		,@user NVARCHAR(50)
		,@host NVARCHAR(50)
	
	SELECT    @tablename = '[dbo].[' + OBJECT_NAME(object_id) +']'
	FROM    sys.dm_db_index_usage_stats
	WHERE    database_id = DB_ID('AacDB')
	AND        OBJECT_NAME(object_id) = 'Role'
	SELECT @tablename
	SELECT @roleID = IDENT_CURRENT('Role')
	
	SELECT @user = SUSER_NAME()
		,@host = HOST_NAME()
	
	IF @tablename='[dbo].[Role]'
	BEGIN
		INSERT INTO [dbo].[TriggeredTable](TableName,[Description])
		VALUES('[dbo].[Role]','Data added on [dbo].[Role], new itemid: '+CAST(@roleID AS NVARCHAR(50))+'|User:' + @user + '|Host:' + @host )
	END
END

GO
-- =============================================
-- Author:		Viray, McNiel N.
-- Create date: 21 April 2016
-- Description:	Trigger that will determine if someone modify table 
-- =============================================
CREATE TRIGGER [dbo].[RoleUpdate]
   ON  [dbo].[Role]
   FOR UPDATE
AS 
BEGIN
	DECLARE @user NVARCHAR(50)
		,@host NVARCHAR(50)
		,@roleID INT
		
	SELECT @roleID = d.ID
	FROM deleted d
	UPDATE [Role]
	SET DateModified=GETDATE()
	WHERE ID=@roleID
	
	SELECT @user = SUSER_NAME()
		,@host = HOST_NAME()
	INSERT INTO [dbo].[TriggeredTable](TableName,[Description])
	VALUES('[dbo].[Role]','Data updated on [dbo].[Role]|user:'+@user+'|host:'+@host+'|RoleID:'+ CAST( @roleID as VARCHAR(10)))
END

GO

INSERT INTO [Role]([Name],[Description])
VALUES('Administrator','Overall access to the system.')
GO
INSERT INTO [Role]([Name],[Description])
VALUES('Pilot','Role that have limited access to the application.')
GO
INSERT INTO [Role]([Name],[Description])
VALUES('Checker','Approves pilots data entry.')
GO
INSERT INTO [Role]([Name],[Description])
VALUES('Encoder','Data encoder, able to insert data to designated table.')
GO
INSERT INTO [Role]([Name],[Description])
VALUES('Scheduler','Create aircraft schedule.')
GO
INSERT INTO [Role]([Name],[Description])
VALUES('Aircraft Crew','Aircraft crew, no access to app.')
GO
--INSERT INTO [Role]([Name],[Description])
--VALUES('Co-Pilot','Role that have limited access to the application.')
--GO
CREATE TABLE [dbo].[Destination](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Destination] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Destination] ADD  CONSTRAINT [DF_Destination_Description]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[Destination] ADD  CONSTRAINT [DF_Destination_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Destination] ADD  CONSTRAINT [DF_Destination_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
CREATE TABLE [dbo].[AircraftType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_AircraftType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AircraftType] ADD  CONSTRAINT [DF_AircraftType_Description]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[AircraftType] ADD  CONSTRAINT [DF_AircraftType_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[AircraftType] ADD  CONSTRAINT [DF_AircraftType_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
CREATE TABLE [dbo].[Flight](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftTypeID] [int] NOT NULL,
	[PilotID] [int] NOT NULL,
	[CoPilotID] [int] NOT NULL,
	[Crew] [nvarchar](350) NOT NULL,
	[FlightNumber] [nvarchar](50) NOT NULL,
	[AircraftRegistration] [nvarchar](150) NOT NULL,
	[Route] [nvarchar](150) NOT NULL,
	[Particulars] [nvarchar](50) NOT NULL,
	[PreparedByID] [int] NULL,
	[ReviewedByID] [int] NULL,
	[CheckedByID] [int] NULL,
	[NotedByID] [int] NULL,
	[Comments] [nvarchar](max) NULL,
	[Passengers] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Flight] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Flight] ADD  CONSTRAINT [DF_Flight_Crew]  DEFAULT ('') FOR [Crew]
GO
ALTER TABLE [dbo].[Flight] ADD  CONSTRAINT [DF_Flight_FlightNumber]  DEFAULT ('') FOR [FlightNumber]
GO
ALTER TABLE [dbo].[Flight] ADD  CONSTRAINT [DF_Flight_Route]  DEFAULT ('') FOR [Route]
GO
ALTER TABLE [dbo].[Flight] ADD  CONSTRAINT [DF_Flight_Particulars]  DEFAULT ('') FOR [Particulars]
GO
ALTER TABLE [dbo].[Flight] ADD  CONSTRAINT [DF_Flight_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Flight] ADD  CONSTRAINT [DF_Flight_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
CREATE TABLE [dbo].[FlightDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FlightID] [int] NOT NULL,
	[FromID] [int] NOT NULL,
	[ToID] [int] NOT NULL,
	[OffBlk] [int] NOT NULL,
	[OnBlk] [int] NOT NULL,
	[BLKTime] [int] NOT NULL,
	[OffGRD] [int] NOT NULL,
	[OnGRD] [int] NOT NULL,
	[FLTTime] [int] NOT NULL,
	[WaitingTimeFrom] [int] NOT NULL,
	[WaitingTimeTo] [int] NOT NULL,
	[NumberOfCycle] [int] NOT NULL,
	[RON] [int] NULL,
	[PreFLT] [int] NOT NULL,
	[PostFLT] [int] NOT NULL,
	[FOB] [int] NOT NULL,
	[BO] [int] NOT NULL,
	[REM] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_FlightDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FlightDetail]  WITH CHECK ADD  CONSTRAINT [FK_FlightDetail_FlightID] FOREIGN KEY([FlightID])
REFERENCES [dbo].[Flight] ([ID])
GO
ALTER TABLE [dbo].[FlightDetail] CHECK CONSTRAINT [FK_FlightDetail_FlightID]
GO
CREATE TABLE [dbo].[Registration](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Registration_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_AircraftID] FOREIGN KEY([AircraftID])
REFERENCES [dbo].[AircraftType] ([ID])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_AircraftID]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [DF_Registration_Description]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [DF_Registration_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
CREATE TABLE [dbo].[AircraftTypeRegistration](
	[AircraftTypeID] [int] NOT NULL,
	[RegistrationID] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AircraftTypeRegistration]  WITH CHECK ADD  CONSTRAINT [FK_AircraftTypeRegistration_AircraftID] FOREIGN KEY([AircraftTypeID])
REFERENCES [dbo].[AircraftType] ([ID])
GO
ALTER TABLE [dbo].[AircraftTypeRegistration] CHECK CONSTRAINT [FK_AircraftTypeRegistration_AircraftID]
GO
ALTER TABLE [dbo].[AircraftTypeRegistration]  WITH CHECK ADD  CONSTRAINT [FK_AircraftTypeRegistration_RegistrationID] FOREIGN KEY([RegistrationID])
REFERENCES [dbo].[Registration] ([ID])
GO
ALTER TABLE [dbo].[AircraftTypeRegistration] CHECK CONSTRAINT [FK_AircraftTypeRegistration_RegistrationID]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [DF_Registration_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
CREATE TABLE [dbo].[Schedule](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AircraftID] [int] NOT NULL,
	[RegistrationID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[RouteStartID] [int] NOT NULL,
	[RouteDestinationID] [int] NOT NULL,
	[RouteEndID] [int] NOT NULL,
	[PilotID] [int] NOT NULL,
	[AssistantPilotID] [int] NOT NULL,
	[Notes] [nvarchar](max) NOT NULL,
	[Passengers] [nvarchar](max) NOT NULL,
	[FlightInfo] [nvarchar](max) NOT NULL,
	[TechnicalStops] [nvarchar](max) NOT NULL,
	[ETC] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Schedule] ADD  CONSTRAINT [DF_Schedule_Notes]  DEFAULT ('') FOR [Notes]
GO
ALTER TABLE [dbo].[Schedule] ADD  CONSTRAINT [DF_Schedule_Passengers]  DEFAULT ('') FOR [Passengers]
GO
ALTER TABLE [dbo].[Schedule] ADD  CONSTRAINT [DF_Schedule_FlightInfo]  DEFAULT ('') FOR [FlightInfo]
GO
ALTER TABLE [dbo].[Schedule] ADD  CONSTRAINT [DF_Schedule_TechnicalStops]  DEFAULT ('') FOR [TechnicalStops]
GO
ALTER TABLE [dbo].[Schedule] ADD  CONSTRAINT [DF_Schedule_ETC]  DEFAULT ('') FOR [ETC]
GO
CREATE TABLE [dbo].[ScheduleCrew](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleID] [int] NOT NULL,
	[CrewID] [int] NOT NULL,
 CONSTRAINT [PK_ScheduleCrew] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ScheduleCrew]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleCrew_ScheduleID] FOREIGN KEY([ScheduleID])
REFERENCES [dbo].[Schedule] ([ID])
GO
ALTER TABLE [dbo].[ScheduleCrew] CHECK CONSTRAINT [FK_ScheduleCrew_ScheduleID]
GO
ALTER TABLE [dbo].[AircraftTypeRegistration]
DROP CONSTRAINT FK_AircraftTypeRegistration_AircraftID
GO
ALTER TABLE [dbo].[AircraftTypeRegistration]
DROP FK_AircraftTypeRegistration_RegistrationID
GO
ALTER TABLE [User]
ADD [Active] BIT NOT NULL DEFAULT(1)
GO
ALTER TABLE [Schedule]
ADD [WaitingStart] DATETIME NULL
GO
ALTER TABLE [Schedule]
ADD [WaitingEnd] DATETIME NULL
GO
--create admin
INSERT INTO [User](RoleID,[Username],[Password],Firstname,Lastname,Middlename,[Address],Phone,Email,SSS,TIN) 
VALUES(1,'banbanboi','eygiG6UZ7/sZjmmJnv0FUw==','Mikhail','Ivanov','Putin','123 Makati City','09179999999','mikhailivanov@email.com','123467372','12344322')
GO
--Create Scheduler
INSERT INTO [User](RoleID,[Username],[Password],Firstname,Lastname,Middlename,[Address],Phone,Email,SSS,TIN) 
VALUES(5,'banbanboi2','eygiG6UZ7/sZjmmJnv0FUw==','Yuri','Kirov','Stalin','123 Makati City','09179999999','mikhailivanov@email.com','123467372','12344322')
GO
ALTER TABLE [Role]
ADD [Active] BIT NOT NULL CONSTRAINT [DF_Role_Active] DEFAULT(1)
GO 
UPDATE [Role]
SET Active = 0
WHERE ID IN (3,4)
GO
CREATE TABLE [dbo].[Logs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](550) NOT NULL,
	[ModifiedBy] [nvarchar](150) NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DF_Logs_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO