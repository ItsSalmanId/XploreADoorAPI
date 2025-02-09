USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_DISCIPLINE]    Script Date: 7/21/2022 8:53:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_DISCIPLINE](
	[DISCIPLINE_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_DISCIPLINE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_DISCIPLINE_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_DISCIPLINE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_DISCIPLINE_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_DISCIPLINE_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_7E341EA5EBC346AD8D932277FAFD165F]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_DISCIPLINE] PRIMARY KEY CLUSTERED 
(
	[DISCIPLINE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


