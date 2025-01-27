USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_IDENTIFIER]    Script Date: 7/21/2022 9:02:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_IDENTIFIER](
	[IDENTIFIER_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[IDENTIFIER_TYPE_ID] [int] NULL,
	[CODE] [varchar](10) NULL,
	[NAME] [varchar](100) NULL,
	[CODE_NAME]  AS ((('['+[CODE])+'] ')+[NAME]),
	[DESCRIPTION] [varchar](300) NULL,
	[ADDRESS] [varchar](500) NULL,
	[CITY] [varchar](50) NULL,
	[STATE] [varchar](2) NULL,
	[ZIP] [varchar](9) NULL,
	[MEDICARE_PROVIDER_NUMBER] [varchar](20) NULL,
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_DELETED]  DEFAULT ((0)),
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_MODIFIED_DATE]  DEFAULT (getdate()),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_49FB50EEA3BA44F980CF4B8B71AE40E8]  DEFAULT (newsequentialid()),
	[END_DATE] [datetime] NULL,
	[IS_ACTIVE] [bit] NULL,
	[START_DATE] [datetime] NULL,
 CONSTRAINT [PK_FOX_TBL_IDENTIFIER] PRIMARY KEY CLUSTERED 
(
	[IDENTIFIER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


