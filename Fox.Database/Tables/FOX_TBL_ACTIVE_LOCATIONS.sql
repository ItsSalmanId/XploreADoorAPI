USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_ACTIVE_LOCATIONS]    Script Date: 7/21/2022 4:32:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_ACTIVE_LOCATIONS](
	[LOC_ID] [bigint] NOT NULL,
	[REGION] [varchar](50) NULL,
	[CODE] [varchar](10) NULL,
	[NAME] [varchar](100) NULL,
	[Address] [varchar](500) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](2) NULL,
	[Zip] [varchar](9) NULL,
	[Phone] [varchar](10) NULL,
	[Fax] [varchar](10) NULL,
	[POS_Code] [varchar](10) NULL,
	[FOL] [varchar](100) NULL,
	[Capacity] [varchar](10) NULL,
	[Census] [varchar](10) NULL,
	[PT] [varchar](100) NULL,
	[OT] [varchar](100) NULL,
	[ST] [varchar](100) NULL,
	[EP] [varchar](100) NULL,
	[Lead] [varchar](100) NULL,
	[Parent] [varchar](100) NULL,
	[Description] [varchar](300) NULL,
	[Last_Update] [datetime] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ACTIVE_LOCATIONS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_A__CREAT__19B6ACCD]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_ACTIVE_LOCATIONS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_A__MODIF__1B9EF53F]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_A__DELET__1C931978]  DEFAULT ((0)),
	[Country] [varchar](500) NULL,
	[PRACTICE_CODE] [bigint] NULL CONSTRAINT [DF__FOX_TBL_A__PRACT__72BC77A9]  DEFAULT ((1012714)),
	[Work_Phone] [varchar](10) NULL,
	[Cell_Phone] [varchar](10) NULL,
	[Email_Address] [varchar](255) NULL,
	[IS_PRIVATE_HOME]  AS (case when [CODE] like '%hom%' then CONVERT([bit],(1),(0)) else CONVERT([bit],(0),(0)) end),
	[IS_ACTIVE] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_A__IS_AC__73B09BE2]  DEFAULT ((1)),
	[FACILITY_TYPE_ID] [bigint] NULL,
	[PT_PROVIDER_ID] [bigint] NULL,
	[OT_PROVIDER_ID] [bigint] NULL,
	[ST_PROVIDER_ID] [bigint] NULL,
	[EP_PROVIDER_ID] [bigint] NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[NPI] [varchar](10) NULL,
	[Facility_Code] [bigint] NULL,
	[LEAD_PROVIDER_ID] [bigint] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_B350D2B69A9B41ACA6A9284605E4994E]  DEFAULT (newsequentialid())
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


