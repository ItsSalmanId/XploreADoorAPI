USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_REGION_ZIPCODE_DATA]    Script Date: 7/21/2022 9:32:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_REGION_ZIPCODE_DATA](
	[REGION_ZIPCODE_DATA_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[STATE] [varchar](2) NULL,
	[REGION] [varchar](50) NULL,
	[INSURANCE_CODE] [varchar](50) NULL,
	[RR_MCR] [varchar](50) NULL,
	[ZIP_CODE] [varchar](9) NULL,
	[TOWN_CITY] [varchar](100) NULL,
	[COUNTY_CITY] [varchar](100) NULL,
	[REGIONAL_DIRECTOR] [varchar](255) NULL,
	[SENIOR_REGIONAL_DIRECTOR] [varchar](255) NULL,
	[ACCOUNT_MANAGER] [varchar](255) NULL,
	[SPECIAL_NOTES_FOR_ASSIGNMENTS] [varchar](3000) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REGION_ZIPCODE_DATA_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_REGION_ZIPCODE_DATA_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REGION_ZIPCODE_DATA_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_REGION_ZIPCODE_DATA_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_REGION_ZIPCODE_DATA_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_93D5607BA38C4472823EC794397E51BC]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_REGION_ZIPCODE_DATA] PRIMARY KEY CLUSTERED 
(
	[REGION_ZIPCODE_DATA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


