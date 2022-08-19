USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_Tbl_Patient_Contacts]    Script Date: 7/21/2022 9:16:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_Tbl_Patient_Contacts](
	[Contact_ID] [bigint] NOT NULL,
	[Patient_Account] [bigint] NULL,
	[Contact_Type_Id] [bigint] NULL,
	[First_Name] [varchar](50) NULL,
	[MI] [varchar](1) NULL,
	[Last_Name] [varchar](50) NULL,
	[Address] [varchar](500) NULL,
	[Zip] [varchar](9) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](2) NULL,
	[Country] [varchar](50) NULL,
	[Email_Address] [varchar](255) NULL,
	[Fax_Number] [varchar](10) NULL,
	[Home_Phone] [varchar](10) NULL,
	[Call_Home] [bit] NULL,
	[Best_Time_Call_Home] [varchar](50) NULL,
	[Preferred_Contact] [int] NULL,
	[Work_Phone] [varchar](10) NULL,
	[Call_Work] [bit] NULL,
	[Best_Time_Call_Work] [varchar](50) NULL,
	[Cell_Phone] [varchar](10) NULL,
	[Call_Cell] [bit] NULL,
	[Best_Time_Call_Cell] [varchar](50) NULL,
	[Preferred_Delivery_Method] [varchar](50) NULL,
	[Start_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Marketing_Referral_Source] [bit] NULL,
	[Flag_Financially_Responsible_Party] [bit] NULL,
	[Flag_Preferred_Contact] [bit] NULL,
	[Flag_Emergency_Contact] [bit] NULL,
	[Flag_Power_Of_Attorney] [bit] NULL,
	[Flag_Power_Of_Attorney_Financial] [bit] NULL,
	[Flag_Power_Of_Attorney_Medical] [bit] NULL,
	[Flag_Lives_In_Household_SLC] [bit] NULL,
	[Flag_Service_Location] [bit] NULL,
	[Created_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Contacts_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_On] [datetime] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__Creat__22F59AB0]  DEFAULT (getdate()),
	[Modified_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Contacts_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_On] [datetime] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__Modif__24DDE322]  DEFAULT (getdate()),
	[Deleted] [bit] NULL,
	[Room] [varchar](50) NULL,
	[EXT_WORK_PHONE] [int] NULL,
	[STATEMENT_ADDRESS_MARKED] [bit] NOT NULL CONSTRAINT [DF__Fox_Tbl_P__STATE__25D2075B]  DEFAULT ((0)),
	[Is_Same_Patient_Address] [bit] NULL CONSTRAINT [DF__Fox_Tbl_P__Is_Sa__26C62B94]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_2069FDEF6E3E4DF48BF2BCE7618D2C46]  DEFAULT (newsequentialid()),
	[Best_Time_Call_Cell_Id] [bigint] NULL,
	[Best_Time_Call_Work_Id] [bigint] NULL,
	[Best_Time_Call_Home_Id] [bigint] NULL,
	[FOX_GUID] [varchar](50) NULL,
 CONSTRAINT [PK__Fox_Tbl___82ACC1CD1BBBD84C] PRIMARY KEY CLUSTERED 
(
	[Contact_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


