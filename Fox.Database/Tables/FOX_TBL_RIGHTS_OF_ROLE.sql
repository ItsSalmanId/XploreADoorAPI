USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_RIGHTS_OF_ROLE]    Script Date: 7/21/2022 9:33:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_RIGHTS_OF_ROLE](
	[RIGHTS_OF_ROLE_ID] [bigint] NULL,
	[RIGHT_ID] [bigint] NULL,
	[ROLE_ID] [bigint] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_RIGHTS_OF_ROLE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_R__CREAT__1937264C]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_RIGHTS_OF_ROLE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_R__MODIF__1B1F6EBE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_R__DELET__1C1392F7]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_418F152F9DD744669447D9A2203D38A2]  DEFAULT (newsequentialid())
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


