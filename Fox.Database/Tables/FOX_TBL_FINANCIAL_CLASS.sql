USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_FINANCIAL_CLASS]    Script Date: 7/21/2022 8:56:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_FINANCIAL_CLASS](
	[FINANCIAL_CLASS_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CODE] [varchar](10) NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_FINANCIAL_CLASS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_FINANCIAL_CLASS_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_FINANCIAL_CLASS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_FINANCIAL_CLASS_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_FINANCIAL_CLASS_DELETED]  DEFAULT ((0)),
	[SHOW_FOR_INSURANCE] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_F__SHOW___1F5A13F6]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_E250A835533742A9A45D3637177566BA]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_FINANCIAL_CLASS] PRIMARY KEY CLUSTERED 
(
	[FINANCIAL_CLASS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


