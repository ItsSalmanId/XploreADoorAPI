USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_Tbl_Patient_Contact_Types]    Script Date: 7/21/2022 9:15:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_Tbl_Patient_Contact_Types](
	[Contact_Type_ID] [bigint] NOT NULL,
	[Type_Name] [varchar](50) NULL,
	[Practice_Code] [bigint] NULL,
	[Display_Order] [varchar](50) NULL,
	[Created_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Contact_Types_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_On] [datetime] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__Creat__1F2509CC]  DEFAULT (getdate()),
	[Modified_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Contact_Types_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_On] [datetime] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__Modif__210D523E]  DEFAULT (getdate()),
	[Deleted] [bit] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_974E048352334046B77765D3EDA16ACB]  DEFAULT (newsequentialid()),
	[CODE] [varchar](10) NULL,
	[END_DATE] [datetime] NULL,
	[IS_ACTIVE] [bit] NULL,
	[START_DATE] [datetime] NULL,
 CONSTRAINT [PK__Fox_Tbl___BEB702A79C126CFE] PRIMARY KEY CLUSTERED 
(
	[Contact_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


