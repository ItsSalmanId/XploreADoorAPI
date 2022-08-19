USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT]    Script Date: 7/21/2022 9:11:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT](
	[FOX_TBL_PATIENT_ID] [bigint] NOT NULL,
	[Patient_Account] [bigint] NULL,
	[Title] [varchar](10) NULL,
	[Best_Time_of_Call_Home] [varchar](50) NULL,
	[Best_Time_of_Call_Work] [varchar](50) NULL,
	[Best_Time_of_Call_Cell] [varchar](50) NULL,
	[Fax_Number] [varchar](10) NULL,
	[PCP] [bigint] NULL,
	[Employment_Status] [varchar](50) NULL,
	[Patient_Status] [varchar](50) NULL,
	[Student_Status] [varchar](50) NULL,
	[Expired] [bit] NULL,
	[CHK_ABN] [bit] NULL,
	[CHK_HOME_HEALTH_EPISODE] [bit] NULL,
	[Created_By] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_Date] [datetime] NULL,
	[Modified_By] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_Date] [datetime] NULL,
	[Deleted] [bit] NULL CONSTRAINT [DF_FOX_TBL_PATIENT_Deleted]  DEFAULT ((0)),
	[PRACTICE_ORGANIZATION_ID] [bigint] NULL,
	[POA_EMERGENCY_CONTACT] [varchar](100) NULL,
	[FINANCIAL_CLASS_ID] [int] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_09FB46A9C6C2470BB3D3997489F0A246]  DEFAULT (newsequentialid()),
	[Best_Time_Call_Cell_Id] [bigint] NULL,
	[Best_Time_Call_Work_Id] [bigint] NULL,
	[Best_Time_Call_Home_Id] [bigint] NULL,
	[IS_OPENED_BY] [varchar](50) NULL,
	[INACTIVE_DATE] [datetime] NULL,
	[IS_SYNCH] [bit] NULL,
 CONSTRAINT [PK_FOX_TBL_PATIENT] PRIMARY KEY CLUSTERED 
(
	[FOX_TBL_PATIENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


