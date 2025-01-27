USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_MATRIX_AND_FACILITY_FREEZ_LOG]    Script Date: 7/21/2022 9:07:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FOX_TBL_MATRIX_AND_FACILITY_FREEZ_LOG](
	[ID] [bigint] NOT NULL,
	[REPORT_NAME] [nvarchar](250) NULL,
	[FREEZE_DATE] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO


