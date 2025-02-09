USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_EVALUATION_CRITERIA_CATEGORIES]    Script Date: 7/21/2022 9:51:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_EVALUATION_CRITERIA_CATEGORIES](
	[EVALUATION_CRITERIA_CATEGORIES_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CATEGORIES_NAME] [varchar](255) NULL,
	[EVALUATION_CRITERIA_ID] [bigint] NULL,
	[CATEGORIES_POINTS] [int] NULL,
	[CREATED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL DEFAULT ((0)),
	[CALL_TYPE] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[EVALUATION_CRITERIA_CATEGORIES_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


