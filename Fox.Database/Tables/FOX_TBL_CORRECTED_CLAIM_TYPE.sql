USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_CORRECTED_CLAIM_TYPE]    Script Date: 7/21/2022 8:52:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_CORRECTED_CLAIM_TYPE](
	[CORRECTED_CLAIM_TYPE_ID] [int] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CORRECTED_CLAIMS_TYPE_DESC] [varchar](100) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_CORRECTED_CLAIM_TYPE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_C__CREAT__6DC2B862]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_CORRECTED_CLAIM_TYPE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_C__MODIF__6FAB00D4]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_C__DELET__709F250D]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_B836513EA0B547CCB04C16D89F2AD392]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK__FOX_TBL___6DCDDE5D000EA732] PRIMARY KEY CLUSTERED 
(
	[CORRECTED_CLAIM_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


