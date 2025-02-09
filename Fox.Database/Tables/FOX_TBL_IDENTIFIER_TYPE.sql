USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_IDENTIFIER_TYPE]    Script Date: 7/21/2022 9:02:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_IDENTIFIER_TYPE](
	[IDENTIFIER_TYPE_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_TYPE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_TYPE_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_TYPE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_TYPE_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_IDENTIFIER_TYPE_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_8BCC93127E1F4D3EA44E4950AF963944]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_IDENTIFIER_TYPE] PRIMARY KEY CLUSTERED 
(
	[IDENTIFIER_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


