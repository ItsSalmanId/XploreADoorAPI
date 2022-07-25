USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_AVAYA_CALL_RECORDINGS_LOGS]    Script Date: 7/21/2022 9:46:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_AVAYA_CALL_RECORDINGS_LOGS](
	[AVAYA_CALL_RECORDINGS_LOGS_ID] [bigint] NOT NULL,
	[RECORDING_NAME] [varchar](100) NULL,
	[CURRENT_FOLDER] [varchar](100) NULL,
	[CALL_DATE] [datetime] NULL,
	[CALL_START_TIME] [datetime] NULL,
	[CALL_END_TIME] [datetime] NULL,
	[CALL_NO] [varchar](10) NULL,
	[CALL_BY] [varchar](70) NULL,
	[CALL_DIRECTION] [varchar](20) NULL,
	[OFFICE_NAME] [varchar](20) NULL,
	[EXTENSION] [varchar](5) NULL,
	[CORE_QUEUE] [varchar](100) NULL,
	[CREATED_DATE_ON_SERVER] [datetime] NULL DEFAULT (getdate()),
	[SERVICE_READ_DATE] [datetime] NULL DEFAULT (getdate()),
	[DS_READING_TIME] [varchar](100) NULL,
	[SUCCESS_OR_ERROR] [varchar](100) NULL,
	[INITIAL_SIZE] [varchar](100) NULL,
	[FINAL_SIZE] [varchar](100) NULL,
	[CREATED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[AVAYA_CALL_RECORDINGS_LOGS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


