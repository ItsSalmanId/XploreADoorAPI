USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_ROLE]    Script Date: 7/21/2022 9:33:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_ROLE](
	[ROLE_ID] [bigint] NOT NULL,
	[ROLE_NAME] [varchar](50) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ROLE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_R__CREAT__1DFBDB69]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ROLE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_R__MODIF__1FE423DB]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_R__DELET__20D84814]  DEFAULT ((0)),
	[PRACTICE_CODE] [bigint] NULL CONSTRAINT [DF__FOX_TBL_R__PRACT__21CC6C4D]  DEFAULT ((1012714)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_347776AABE104360ADE1054ACA0F4E50]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK__FOX_TBL___5AC4D222A19B4331] PRIMARY KEY CLUSTERED 
(
	[ROLE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

