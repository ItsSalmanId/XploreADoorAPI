USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PHD_SERVICE_LOG]    Script Date: 7/21/2022 9:56:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PHD_SERVICE_LOG](
	[FOX_TBL_PHD_SERVICE_LOG_ID] [bigint] NOT NULL,
	[PHD_CALL_LOG_ID_COUNT] [int] NULL,
	[IS_SUCCESSFUL] [bigint] NULL,
	[CREATED_BY] [varchar](255) NOT NULL DEFAULT ('FOX PHD SERVICE'),
	[CREATED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](255) NOT NULL DEFAULT ('FOX PHD SERVICE'),
	[MODIFIED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[FOX_TBL_PHD_SERVICE_LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


