USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_Tbl_Patient_Alias]    Script Date: 7/21/2022 9:55:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_Tbl_Patient_Alias](
	[PATIENT_ALIAS_ID] [bigint] NOT NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[ALIAS_TRACKING_NUMBER] [varchar](100) NULL,
	[RT_ALIAS_TRACKING] [varchar](100) NULL,
	[FIRST_NAME] [varchar](50) NULL,
	[LAST_NAME] [varchar](50) NULL,
	[MIDDLE_INITIALS] [varchar](5) NULL,
	[Created_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Alias_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_Date] [datetime] NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Alias_Created_Date]  DEFAULT (getdate()),
	[Modified_By] [varchar](70) NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Alias_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_Date] [datetime] NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Alias_Modified_Date]  DEFAULT (getdate()),
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_Fox_Tbl_Patient_Alias_Deleted]  DEFAULT ((0)),
	[FOX_GUID] [varchar](50) NULL,
 CONSTRAINT [PK_Fox_Tbl_Patient_Alias] PRIMARY KEY CLUSTERED 
(
	[PATIENT_ALIAS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


