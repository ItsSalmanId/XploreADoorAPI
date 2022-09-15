USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT_ADDRESS]    Script Date: 7/21/2022 9:11:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT_ADDRESS](
	[PATIENT_ADDRESS_HISTORY_ID] [bigint] NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[ADDRESS] [varchar](500) NULL,
	[CITY] [varchar](50) NULL,
	[STATE] [varchar](2) NULL,
	[ZIP] [varchar](9) NULL,
	[FROM_DATE] [datetime] NULL,
	[END_DATE] [datetime] NULL,
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_P__DELET__0288CB1E]  DEFAULT ((0)),
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADDRESS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADDRESS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NULL,
	[CREATED_DATE] [datetime] NULL,
	[Home_Phone] [varchar](10) NULL,
	[COUNTRY] [varchar](50) NULL,
	[ADDRESS_TYPE] [varchar](50) NULL,
	[Same_As_POS] [bit] NULL,
	[PATIENT_POS_ID] [bigint] NULL,
	[POS_Phone] [varchar](10) NULL,
	[POS_Work_Phone] [varchar](10) NULL,
	[POS_Cell_Phone] [varchar](10) NULL,
	[POS_Fax] [varchar](10) NULL,
	[POS_Email_Address] [varchar](255) NULL,
	[POS_REGION] [varchar](100) NULL,
	[POS_County] [varchar](100) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_CBE9C6F2B8F4448F87905AEAA30FB26D]  DEFAULT (newsequentialid()),
	[WEBEHR_PATIENT_ADDRESS_ID] [bigint] NULL,
	[LATITUDE] [real] NULL,
	[LONGITUDE] [real] NULL,
	[WORK_ID] [bigint] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


