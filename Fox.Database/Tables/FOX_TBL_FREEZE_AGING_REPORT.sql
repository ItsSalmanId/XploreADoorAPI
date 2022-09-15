USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_FREEZE_AGING_REPORT]    Script Date: 7/21/2022 9:00:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_FREEZE_AGING_REPORT](
	[PRACTICE_CODE] [bigint] NULL,
	[CLAIM_NO] [bigint] NOT NULL,
	[FINANCIAL_CLASS] [varchar](10) NULL,
	[DESCRIPTION] [varchar](300) NULL,
	[PAYER] [varchar](10) NULL,
	[PATIENT] [varchar](100) NULL,
	[PATIENT_ACCOUNT] [varchar](30) NULL,
	[DATE_OF_SERVICE] [varchar](30) NULL,
	[BILL_DATE] [smalldatetime] NULL,
	[Location_CODE] [bigint] NULL,
	[Attending_Physician] [bigint] NULL,
	[Billing_Physician] [bigint] NULL,
	[CODE] [varchar](10) NULL,
	[BILLED_CHARGES] [money] NULL,
	[EXPECTED_CHARGES] [money] NULL,
	[PAID_TO_CHARGES] [money] NOT NULL,
	[ADJUSTED_TO_CHARGES] [money] NULL,
	[LESS_THIRTY] [money] NULL,
	[THIRTY_TO_SIXTY] [money] NULL,
	[SIXTY_TO_NINTY] [money] NULL,
	[NINTY_TO_ONEEIGHTY] [money] NULL,
	[ONEEIGHTY_TO_TWOSEVENTY] [money] NULL,
	[TWOSEVENTY_TO_THREESIXTYFIVE] [money] NULL,
	[THREESIXTYFIVE_PLUS] [money] NULL,
	[BALANCE] [money] NULL,
	[BILLED_UNITS] [int] NOT NULL,
	[FOXUNITS] [bigint] NOT NULL,
	[TOTAL_PATIENTS] [varchar](1) NOT NULL,
	[NEW_PATIENTS] [varchar](1) NOT NULL,
	[RETURNING_PATIENTS] [varchar](1) NOT NULL,
	[AVG_CHARGE_PAT] [varchar](1) NOT NULL,
	[AVG_PAID_PAT] [varchar](1) NOT NULL,
	[RENDERING_PROVIDER] [varchar](10) NULL,
	[BILLING_PROVIDER] [varchar](10) NULL,
	[REGION_CODE] [varchar](30) NULL,
	[REGION_NAME] [varchar](50) NULL,
	[LOCATION] [bigint] NULL,
	[LOCATION_NAME] [varchar](200) NULL,
	[POSTINGDATE] [smalldatetime] NULL,
	[BC] [varchar](1) NOT NULL,
	[LASTBILLED] [smalldatetime] NULL,
	[LASTPAID] [varchar](1) NOT NULL,
	[DAYS] [int] NULL,
	[BUCKET] [varchar](7) NULL,
	[DOE] [smalldatetime] NULL,
	[ACTUAL_AMOUNT] [varchar](1) NOT NULL,
	[STATE] [varchar](2) NULL,
	[ADJUSTMENT_REASON] [nvarchar](508) NULL,
	[FACILITY_CODE] [varchar](10) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


