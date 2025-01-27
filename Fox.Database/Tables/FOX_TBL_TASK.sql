USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_TASK]    Script Date: 7/21/2022 9:36:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_TASK](
	[TASK_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CASE_ID] [bigint] NULL,
	[TASK_TYPE_ID] [int] NULL,
	[CATEGORY_ID] [int] NULL,
	[PROVIDER_ID_old] [bigint] NULL,
	[LOC_ID] [bigint] NULL,
	[SEND_TO_ID] [bigint] NULL,
	[FINAL_ROUTE_ID] [bigint] NULL,
	[PRIORITY] [varchar](20) NULL,
	[DUE_DATE_TIME] [datetime] NULL,
	[IS_REQ_SIGNOFF] [bit] NULL,
	[IS_SENDING_ROUTE_DETAILS] [bit] NULL,
	[SEND_CONTEXT_ID] [bigint] NULL,
	[CONTEXT_INFO] [varchar](200) NULL,
	[DEVELIVERY_ID] [bigint] NULL,
	[DESTINATIONS] [varchar](200) NULL,
	[IS_TEMPLATE] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_IS_TEMPLATE]  DEFAULT ((0)),
	[IS_SEND_EMAIL_AUTO] [bit] NULL,
	[IS_COMPLETED] [bit] NULL,
	[IS_TEMPORARY_DELETED] [bit] NULL,
	[TEMPORARY_DELETED_BY] [varchar](70) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_DELETED]  DEFAULT ((0)),
	[PATIENT_ACCOUNT] [bigint] NULL,
	[GENERAL_NOTE_ID] [bigint] NULL,
	[IS_COMPLETED_INT] [int] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_IS_COMPLETED_INT]  DEFAULT ((0)),
	[ATTACHMENT_PATH] [varchar](500) NULL,
	[ATTACHMENT_TITLE] [varchar](500) NULL,
	[IS_FINAL_ROUTE_USER] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_T__IS_FI__3C806289]  DEFAULT ((1)),
	[IS_SEND_TO_USER] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_T__IS_SE__3D7486C2]  DEFAULT ((1)),
	[PROVIDER_ID] [bigint] NULL,
	[IS_FINALROUTE_MARK_COMPLETE] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_T__IS_FI__3E68AAFB]  DEFAULT ((0)),
	[IS_SENDTO_MARK_COMPLETE] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_T__IS_SE__3F5CCF34]  DEFAULT ((0)),
	[ATTACHED_FILE] [varchar](500) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_C0B0F20CCF4046CEA6B4C2C216BFA529]  DEFAULT (newsequentialid()),
	[FOX_GUID] [varchar](50) NULL,
	[COMPLETED_DATE] [datetime] NULL,
 CONSTRAINT [PK_FOX_TBL_TASK] PRIMARY KEY CLUSTERED 
(
	[TASK_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


