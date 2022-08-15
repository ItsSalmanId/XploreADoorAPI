USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_Tbl_Broadcast]    Script Date: 7/21/2022 8:45:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_Tbl_Broadcast](
	[Broadcast_Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Broadcast_Date] [date] NULL,
	[Broadcast_Type] [varchar](50) NULL,
	[Title] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Show_To_Clinician] [bit] NULL,
	[Show_To_Regional_Director] [bit] NULL,
	[Show_To_Account_Manager] [bit] NULL,
	[deleted] [bit] NULL CONSTRAINT [DELETED]  DEFAULT ((0)),
	[created_date] [datetime] NULL,
	[modified_date] [datetime] NULL,
	[modified_by] [varchar](50) NULL,
	[created_by] [varchar](50) NULL,
	[Attributed_Description] [varchar](max) NULL,
	[Header_Color] [varchar](50) NULL,
	[Title_Color] [varchar](50) NULL,
	[Description_Color] [varchar](50) NULL,
	[Background_Color] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Broadcast_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


