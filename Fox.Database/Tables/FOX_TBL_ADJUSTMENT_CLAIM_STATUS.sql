USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_ADJUSTMENT_CLAIM_STATUS]    Script Date: 7/21/2022 8:40:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_ADJUSTMENT_CLAIM_STATUS](
	[STATUS_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NOT NULL,
	[STATUS_NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](1000) NULL,
	[STATUS_CATEGORY] [varchar](100) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_CLAIM_STATUS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSMENT_CLAIM_STATUS_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_CLAIM_STATUS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSMENT_CLAIM_STATUS_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSMENT_CLAIM_STATUS_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_B5236B3E1A024418A42EC9F848F4E040]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_ADJUSMENT_CLAIM_STATUS] PRIMARY KEY CLUSTERED 
(
	[STATUS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


