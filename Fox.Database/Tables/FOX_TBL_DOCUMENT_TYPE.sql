USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_DOCUMENT_TYPE]    Script Date: 7/21/2022 8:54:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_DOCUMENT_TYPE](
	[DOCUMENT_TYPE_ID] [int] NOT NULL,
	[RT_CODE] [varchar](10) NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_DOCUMENT_TYPE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_DOCUMENT_TYPE_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_DOCUMENT_TYPE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_DOCUMENT_TYPE_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_DOCUMENT_TYPE_DELETED]  DEFAULT ((0)),
	[IS_ONLINE_ORDER_LIST] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_D__IS_ON__4264EFBF]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_784CD4ADC4A1416F9BE740191267F35E]  DEFAULT (newsequentialid()),
	[END_DATE] [datetime] NULL,
	[IS_ACTIVE] [bit] NULL,
	[START_DATE] [datetime] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


