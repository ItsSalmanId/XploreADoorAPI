USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_FREEZ_AGING_BILL]    Script Date: 7/21/2022 8:57:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_FREEZ_AGING_BILL](
	[CLAIM_NO] [bigint] NOT NULL,
	[BILL_DATE] [date] NULL,
	[DOS] [date] NULL,
	[DOE] [date] NULL,
	[AGING_DAYS] [int] NULL,
	[AGING_AMOUNT] [money] NULL,
	[FINANCIAL_CLASS] [varchar](300) NOT NULL,
	[FC_CODE] [varchar](10) NOT NULL,
	[LOCATION_NAME] [varchar](200) NULL,
	[STATE] [varchar](2) NULL,
	[FACILITY] [varchar](100) NULL,
	[FACILITY_CODE] [varchar](10) NULL,
	[PROVIDER_NAME] [varchar](201) NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[Insurance_Name] [varchar](50) NULL,
	[REFERRAL_REGION_NAME] [varchar](30) NULL,
	[FACILITY_NAME] [varchar](50) NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[PATIENT_NAME] [varchar](101) NULL,
	[PRI_STATUS] [varchar](1) NULL,
	[SEC_STATUS] [varchar](1) NULL,
	[OTH_STATUS] [varchar](1) NULL,
	[PAT_STATUS] [varchar](1) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


