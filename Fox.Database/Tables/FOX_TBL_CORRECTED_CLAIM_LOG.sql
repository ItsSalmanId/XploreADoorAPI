USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_CORRECTED_CLAIM_LOG]    Script Date: 7/21/2022 8:52:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_CORRECTED_CLAIM_LOG](
	[CORRECTED_CLAIM_LOG_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CORRECTED_CLAIM_ID] [bigint] NULL,
	[ACTION] [varchar](50) NULL,
	[ACTION_DETAIL] [varchar](500) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_CORRECTED_CLAIM_LOG_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_C__CREAT__68FE0345]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_CORRECTED_CLAIM_LOG_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_C__MODIF__6AE64BB7]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_C__DELET__6BDA6FF0]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_A09E369E34F945E5A5FF0C9DB4E2D355]  DEFAULT (newsequentialid())
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


