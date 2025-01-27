USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_COMMUNICATION_CALL_STATUS]    Script Date: 7/21/2022 8:50:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_COMMUNICATION_CALL_STATUS](
	[FOX_CALL_STATUS_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CALL_STATUS_NAME] [varchar](50) NOT NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_COMMUNICATION_CALL_STATUS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_COMMUNICATION_CALL_STATUS_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_COMMUNICATION_CALL_STATUS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_COMMUNICATION_CALL_STATUS_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_COMMUNICATION_CALL_STATUS_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_525B8DB6681F40C58ED54A0F83885D98]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_COMMUNICATION_CALL_STATUS] PRIMARY KEY CLUSTERED 
(
	[FOX_CALL_STATUS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


