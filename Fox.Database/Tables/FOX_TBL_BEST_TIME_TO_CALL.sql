USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_BEST_TIME_TO_CALL]    Script Date: 7/21/2022 8:45:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_BEST_TIME_TO_CALL](
	[FOX_BEST_TIME_TO_CALL_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[RT_CODE] [varchar](10) NULL,
	[DESCRIPTION] [varchar](255) NULL,
	[CREATED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL DEFAULT ((0)),
	[END_DATE] [datetime] NULL,
	[START_DATE] [datetime] NULL,
	[IS_ACTIVE] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[FOX_BEST_TIME_TO_CALL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


