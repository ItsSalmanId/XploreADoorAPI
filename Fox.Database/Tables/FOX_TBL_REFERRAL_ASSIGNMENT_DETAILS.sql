USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS]    Script Date: 7/21/2022 9:30:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS](
	[FOX_REFRRAL_ASSIGNMENT_ID] [bigint] NOT NULL,
	[WORK_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NOT NULL,
	[ASSIGNED_BY] [varchar](70) NULL,
	[ASSIGNED_BY_DESIGNATION] [varchar](50) NULL,
	[ASSIGNED_TO] [varchar](70) NULL,
	[ASSIGNED_TO_DESIGNATION] [varchar](50) NULL,
	[ASSIGNED_TIME] [datetime] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_R__CREAT__7E833010]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_R__MODIF__006B7882]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_R__DELET__015F9CBB]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_E079C632EE884933B024AD6D6F379C2E]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK__FOX_TBL___5FA90656EABBBE45] PRIMARY KEY CLUSTERED 
(
	[FOX_REFRRAL_ASSIGNMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


