USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_Talkrehab_Disabled_modules]    Script Date: 7/21/2022 9:36:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_Talkrehab_Disabled_modules](
	[TALKREHAB_MODULE_ID] [bigint] NULL,
	[MODULE_NAME] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](100) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](100) NULL,
	[Deleted] [bit] NULL,
	[IS_SHOW] [bit] NOT NULL DEFAULT ((0))
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


