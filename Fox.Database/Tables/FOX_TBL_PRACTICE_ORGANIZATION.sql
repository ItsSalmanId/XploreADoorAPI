USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PRACTICE_ORGANIZATION]    Script Date: 7/21/2022 9:26:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PRACTICE_ORGANIZATION](
	[PRACTICE_ORGANIZATION_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CODE] [varchar](10) NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[ZIP] [varchar](9) NULL,
	[CITY] [varchar](50) NULL,
	[STATE] [varchar](2) NULL,
	[ADDRESS] [varchar](500) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PRACTICE_ORGANIZATION_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PRACTICE_ORGANIZATION_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PRACTICE_ORGANIZATION_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PRACTICE_ORGANIZATION_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PRACTICE_ORGANIZATION_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_627BF9FC90C24070B2235FE5452A45B7]  DEFAULT (newsequentialid()),
	[END_DATE] [datetime] NULL,
	[START_DATE] [datetime] NULL,
	[IS_ACTIVE] [bit] NULL,
 CONSTRAINT [PK_FOX_TBL_PRACTICE_ORGANIZATION] PRIMARY KEY CLUSTERED 
(
	[PRACTICE_ORGANIZATION_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

