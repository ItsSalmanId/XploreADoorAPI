USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PHD_CALL_SCENARIO]    Script Date: 7/21/2022 9:25:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PHD_CALL_SCENARIO](
	[PHD_CALL_SCENARIO_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PHD_CALL_SCENARIO_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PHD_CALL_SCENARIO_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PHD_CALL_SCENARIO_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PHD_CALL_SCENARIO_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PHD_CALL_SCENARIO_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_4698461609B04D4DB4C451EB4957342E]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_PHD_CALL_SCENARIO] PRIMARY KEY CLUSTERED 
(
	[PHD_CALL_SCENARIO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


