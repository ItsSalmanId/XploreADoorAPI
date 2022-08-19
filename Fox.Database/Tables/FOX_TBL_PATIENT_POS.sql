USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_Tbl_Patient_POS]    Script Date: 7/21/2022 9:19:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_Tbl_Patient_POS](
	[Patient_POS_ID] [bigint] NOT NULL,
	[Patient_Account] [bigint] NOT NULL,
	[Loc_ID] [bigint] NOT NULL,
	[Effective_From] [datetime] NULL,
	[Effective_To] [datetime] NULL,
	[Is_Void] [bit] NULL,
	[Is_Default] [bit] NULL,
	[Created_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_POS_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_Date] [datetime] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__Creat__3BC1487A]  DEFAULT (getdate()),
	[Modified_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_POS_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_Date] [datetime] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__Modif__3DA990EC]  DEFAULT (getdate()),
	[Deleted] [bit] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_D4886609F9174316B8AC1F11C2CB3CD1]  DEFAULT (newsequentialid()),
	[FOX_GUID] [varchar](50) NULL,
 CONSTRAINT [PK__Fox_Tbl___5CB96490A4E3E940] PRIMARY KEY CLUSTERED 
(
	[Patient_POS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


