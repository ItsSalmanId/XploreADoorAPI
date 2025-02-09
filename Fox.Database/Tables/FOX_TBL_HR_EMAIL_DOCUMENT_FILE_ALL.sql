USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_HR_EMAIL_DOCUMENT_FILE_ALL]    Script Date: 7/21/2022 9:53:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_HR_EMAIL_DOCUMENT_FILE_ALL](
	[HR_MTBC_EMAIL_DOCUMENT_FILE_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[HR_CONFIGURE_ID] [bigint] NULL,
	[DOCUMENT_PATH] [varchar](500) NULL,
	[CREATED_BY] [varchar](70) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](70) NOT NULL,
	[MODIFIED_DATE] [datetime] NOT NULL,
	[DELETED] [bit] NOT NULL,
	[ORIGINAL_FILE_NAME] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[HR_MTBC_EMAIL_DOCUMENT_FILE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


