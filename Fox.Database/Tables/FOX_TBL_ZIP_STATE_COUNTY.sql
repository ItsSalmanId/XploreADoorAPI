USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_ZIP_STATE_COUNTY]    Script Date: 7/21/2022 10:02:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_ZIP_STATE_COUNTY](
	[ZIP_STATE_COUNTY_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[ZIP_CODE] [varchar](5) NULL,
	[TYPE] [varchar](10) NULL,
	[DECOMMISSIONED] [varchar](1) NULL,
	[PLACE_NAME] [varchar](50) NULL,
	[ACCEPTABLE_CITIES] [varchar](500) NULL,
	[UNACCEPTABLE_CITIES] [varchar](2500) NULL,
	[STATE] [varchar](2) NULL,
	[COUNTY] [varchar](50) NULL,
	[TIMEZONE] [varchar](20) NULL,
	[AREA_CODES] [varchar](20) NULL,
	[WORLD_REGION] [varchar](20) NULL,
	[COUNTRY] [varchar](100) NULL,
	[APPROXIMATE_LATITUDE] [varchar](20) NULL,
	[APPROXIMATE_LONGITUDE] [varchar](20) NULL,
	[POLYGON_OFFSET_LATITUDE] [varchar](20) NULL,
	[POLYGON_OFFSET_LONGITUDE] [varchar](20) NULL,
	[INTERNAL_POINT_LATITUDE] [varchar](20) NULL,
	[INTERNAL_POINT_LONGITUDE] [varchar](20) NULL,
	[LATITUDE_MIN] [varchar](20) NULL,
	[LATITUDE_MAX] [varchar](20) NULL,
	[LONGITUDE_MIN] [varchar](20) NULL,
	[LONGITUDE_MAX] [varchar](20) NULL,
	[AREA_LAND] [varchar](20) NULL,
	[AREA_WATER] [varchar](20) NULL,
	[HOUSING_COUNT] [varchar](20) NULL,
	[POPULATION_COUNT] [varchar](20) NULL,
	[IRS_ESTIMATED_POPULATION_2015] [varchar](20) NULL,
	[WHITE] [varchar](20) NULL,
	[BLACK_OR_AFRICAN_AMERICAN] [varchar](20) NULL,
	[AMERICAN_INDIAN_OR_ALASKAN_NATIVE] [varchar](20) NULL,
	[ASIAN] [varchar](20) NULL,
	[NATIVE_HAWAIIAN_AND_OTHER_PACIFIC_ISLANDER] [varchar](20) NULL,
	[OTHER_RACE] [varchar](20) NULL,
	[TWO_OR_MORE_RACES] [varchar](20) NULL,
	[TOTAL_MALE_POPULATION] [varchar](20) NULL,
	[TOTAL_FEMALE_POPULATION] [varchar](20) NULL,
	[POP_UNDER_10] [varchar](20) NULL,
	[POP_10_TO_19] [varchar](20) NULL,
	[POP_20_TO_29] [varchar](20) NULL,
	[POP_30_TO_39] [varchar](20) NULL,
	[POP_40_TO_49] [varchar](20) NULL,
	[POP_50_TO_59] [varchar](20) NULL,
	[POP_60_TO_69] [varchar](20) NULL,
	[POP_70_TO_79] [varchar](20) NULL,
	[POP_80_PLUS] [varchar](20) NULL,
	[REFERRAL_REGION_ID] [bigint] NULL,
	[IS_MAP] [bit] NULL,
	[CREATED_BY] [varchar](70) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](70) NOT NULL,
	[MODIFIED_DATE] [datetime] NOT NULL,
	[DELETED] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ZIP_STATE_COUNTY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

