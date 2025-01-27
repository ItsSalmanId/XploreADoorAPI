USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_GENERAL_NOTE]    Script Date: 7/21/2022 9:01:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_GENERAL_NOTE](
	[GENERAL_NOTE_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[PARENT_GENERAL_NOTE_ID] [bigint] NULL,
	[CASE_ID] [bigint] NULL,
	[TASK_ID] [bigint] NULL,
	[NOTE_DESCRIPTION] [varchar](max) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_GENERAL_NOTE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_GENERAL_NOTE_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_GENERAL_NOTE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_GENERAL_NOTE_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_GENERAL_NOTE_DELETED]  DEFAULT ((0)),
	[IS_PATIENT_ALERT] [bit] NULL CONSTRAINT [DF__FOX_TBL_G__IS_PA__2512ED4C]  DEFAULT ((0)),
	[IS_ACK_HL7] [bit] NULL,
	[ALERT_TYPE_ID] [bigint] NULL,
	[PATIENT_ALERT_EFFECTIVE_TO] [datetime] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_99405F3E06864409ABD2B83B9F8F2EE3]  DEFAULT (newsequentialid()),
	[FOX_GUID] [varchar](50) NULL,
 CONSTRAINT [PK_FOX_TBL_GENERAL_NOTE] PRIMARY KEY CLUSTERED 
(
	[GENERAL_NOTE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


