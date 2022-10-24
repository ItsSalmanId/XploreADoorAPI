USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_CALLS_LOG]    Script Date: 7/21/2022 8:48:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_CALLS_LOG](
	[FOX_CALLS_LOG_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CASE_ID] [bigint] NOT NULL,
	[FOX_CALL_TYPE_ID] [bigint] NOT NULL,
	[DISCHARGE_DATE] [datetime] NULL,
	[PATIENT_STATUS] [varchar](255) NULL,
	[GROUP_IDENTIFIER_ID] [int] NOT NULL,
	[RESULT_OF_CALL] [varchar](255) NULL,
	[CALL_DATE] [datetime] NULL,
	[CASE_NO] [varchar](50) NULL,
	[ADMISSION_DATE] [datetime] NULL,
	[CASE_STATUS] [varchar](255) NULL,
	[PROVIDER_ID] [bigint] NULL,
	[REGION_ID] [bigint] NULL,
	[LOCATION_ID] [bigint] NULL,
	[FOX_CALL_STATUS_ID] [bigint] NULL,
	[FOX_CALL_RESULT_ID] [bigint] NULL,
	[FOX_CARE_STATUS_ID] [bigint] NULL,
	[IS_WORK_CALL] [bit] NULL,
	[IS_CELL_CALL] [bit] NULL,
	[IS_HOME_CALL] [bit] NULL,
	[COMMENTS] [varchar](255) NULL,
	[REMARKABLE_REPORT_COMMENTS] [varchar](255) NULL,
	[COMPLETED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_CALLS_LOG_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_CALLS_LOG_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_CALLS_LOG_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_CALLS_LOG_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_CALLS_LOG_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_5A3FA68C0BD443349A974A5CFB73EAE3]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_CALLS_LOG] PRIMARY KEY CLUSTERED 
(
	[FOX_CALLS_LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

