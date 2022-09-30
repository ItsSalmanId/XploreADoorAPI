USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_APPOINTMENT_MILEAGE]    Script Date: 7/21/2022 8:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_APPOINTMENT_MILEAGE](
	[FOX_TBL_APPOINTMENT_MILEAGE_ID] [bigint] NOT NULL,
	[APPOINTMENT_ID_FROM] [bigint] NULL,
	[APPOINTMENT_ID_TO] [bigint] NULL,
	[Time] [varchar](50) NULL,
	[DISTANCE] [varchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](50) NOT NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[DELETED] [bit] NULL,
 CONSTRAINT [PK_FOX_TBL_APPOINTMENT_MILEAGE] PRIMARY KEY CLUSTERED 
(
	[FOX_TBL_APPOINTMENT_MILEAGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


