USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_ZIP_LOCALITY_DATA]    Script Date: 7/21/2022 9:41:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_ZIP_LOCALITY_DATA](
	[ZIP_Locality_Data_Id] [bigint] NOT NULL,
	[State] [varchar](2) NULL,
	[ZIP_Code] [varchar](10) NULL,
	[Carrier] [varchar](10) NULL,
	[Locality] [varchar](5) NULL,
	[Rural_Ind] [varchar](5) NULL,
	[Lab_CB_Locality] [varchar](5) NULL,
	[Rural_Ind_2] [varchar](5) NULL,
	[Plus_4_Flag] [bit] NULL,
	[Part_B_Drug_Indicator] [varchar](5) NULL,
	[Year_Quarter] [int] NULL,
	[Created_By] [varchar](50) NOT NULL CONSTRAINT [DF_FOX_TBL_ZIP_LOCALITY_DATA_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_Date] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_ZIP_LOCALITY_DATA_Created_Date]  DEFAULT (getdate()),
	[Modified_By] [varchar](50) NOT NULL CONSTRAINT [DF_FOX_TBL_ZIP_LOCALITY_DATA_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_Date] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_ZIP_LOCALITY_DATA_Modified_Date]  DEFAULT (getdate()),
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_ZIP_LOCALITY_DATA_Deleted]  DEFAULT ((0)),
 CONSTRAINT [PK_FOX_TBL_ZIP_LOCALITY_DATA] PRIMARY KEY CLUSTERED 
(
	[ZIP_Locality_Data_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


