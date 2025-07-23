use [NajmLogTrace]
GO
IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog' AND TYPE = 'U')
BEGIN
	CREATE TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog](
	[Id] [uniqueidentifier] NULL,
	[Message] [nvarchar](4000) NULL,
	[MachineName] [nvarchar](50) NULL,
	[IPAddress] [varchar](50) NULL,
	[Request] [nvarchar](2000) NULL,
	[Response] [nvarchar](2000) NULL,
	[StatusCode] [int] NULL,
	[Action] [nvarchar](500) NULL,
	[RequestURL] [nvarchar](1000) NULL,
	[HttpVerb] [char](6) NULL,
	[UserAgent] [varchar](256) NULL,
	[LogLevel] [tinyint] NULL,
	[LoggerType] [varchar](100) NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedDay] [tinyint] NULL,
	[CreatedMonth] [tinyint] NULL,
	[CreatedYear] [smallint] NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog_Id]  DEFAULT (newid()) FOR [Id]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog_CreatedDay]  DEFAULT (datepart(day,getdate())) FOR [CreatedDay]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog_CreatedMonth]  DEFAULT (datepart(month,getdate())) FOR [CreatedMonth]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIAuditLog_CreatedYear]  DEFAULT (datepart(year,getdate())) FOR [CreatedYear]
END
GO
IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog' AND TYPE = 'U')
BEGIN
	CREATE TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog](
	[Id] [uniqueidentifier] NULL,
	[Message] [nvarchar](4000) NULL,
	[Exception] [nvarchar](max) NULL,
	[MachineName] [nvarchar](50) NULL,
	[IPAddress] [varchar](50) NULL,
	[Request] [nvarchar](2000) NULL,
	[Response] [nvarchar](2000) NULL,
	[StatusCode] [int] NULL,
	[Action] [nvarchar](500) NULL,
	[RequestURL] [nvarchar](1000) NULL,
	[HttpVerb] [char](6) NULL,
	[UserAgent] [varchar](256) NULL,
	[LogLevel] [tinyint] NULL,
	[LoggerType] [varchar](100) NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedDay] [tinyint] NULL,
	[CreatedMonth] [tinyint] NULL,
	[CreatedYear] [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog_Id]  DEFAULT (newid()) FOR [Id]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog_CreatedDay]  DEFAULT (datepart(day,getdate())) FOR [CreatedDay]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog_CreatedMonth]  DEFAULT (datepart(month,getdate())) FOR [CreatedMonth]
ALTER TABLE [dbo].[NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog] ADD  CONSTRAINT [DF_NajmApplicationLogs_VehicleInsuranceInquiry_APIErrorLog_CreatedYear]  DEFAULT (datepart(year,getdate())) FOR [CreatedYear]

END
GO