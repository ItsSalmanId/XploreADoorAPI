USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_TASK_LOG]    Script Date: 7/21/2022 9:36:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_TASK_LOG](
	[TASK_LOG_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[TASK_ID] [bigint] NULL,
	[ACTION] [varchar](100) NULL,
	[ACTION_DETAIL] [varchar](max) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_LOG_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_LOG_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_LOG_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_LOG_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_LOG_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_2D7A70BC94CB4398B4939E367F866A22]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_TASK_LOG] PRIMARY KEY CLUSTERED 
(
	[TASK_LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


