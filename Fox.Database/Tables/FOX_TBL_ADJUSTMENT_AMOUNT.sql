USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_ADJUSTMENT_AMOUNT]    Script Date: 7/21/2022 8:39:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_ADJUSTMENT_AMOUNT](
	[ADJUSTMENT_AMOUNT_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NOT NULL,
	[ADJUSTMENT_AMOUNT] [varchar](100) NULL,
	[EXPRESSION] [varchar](20) NULL,
	[RANGE_FROM] [money] NULL,
	[RANGE_TO] [money] NULL,
	[DESCRIPTION] [varchar](1000) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_AMOUNT_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_AMOUNT_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_AMOUNT_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_AMOUNT_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_ADJUSTMENT_AMOUNT_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_FA0B025826FE4FC9ABD351BCF7BBF197]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_ADJUSTMENT_AMOUNT] PRIMARY KEY CLUSTERED 
(
	[ADJUSTMENT_AMOUNT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

