USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_FREEZ_AGING_GROUP_BILL]    Script Date: 7/21/2022 8:59:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_FREEZ_AGING_GROUP_BILL](
	[PRACTICE_CODE] [bigint] NULL,
	[CLAIM_NO] [bigint] NOT NULL,
	[DOS] [date] NULL,
	[DOE] [date] NULL,
	[BILL_DATE] [date] NULL,
	[FINANCIAL_CLASS] [varchar](300) NOT NULL,
	[FC_CODE] [varchar](10) NOT NULL,
	[LOCATION_NAME] [varchar](200) NULL,
	[STATE] [varchar](2) NULL,
	[FACILITY] [varchar](100) NULL,
	[FACILITY_CODE] [varchar](10) NULL,
	[PROVIDER_NAME] [varchar](201) NULL,
	[AGING_DAYS] [int] NULL,
	[INSURANCE_NAME] [varchar](50) NULL,
	[REFERRAL_REGION_NAME] [varchar](30) NULL,
	[FACILITY_NAME] [varchar](50) NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[PATIENT_NAME] [varchar](101) NULL,
	[0-30] [money] NULL,
	[31-60] [money] NULL,
	[61-90] [money] NULL,
	[91-120] [money] NULL,
	[121-150] [money] NULL,
	[151-270] [money] NULL,
	[271-365] [money] NULL,
	[365+] [money] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


