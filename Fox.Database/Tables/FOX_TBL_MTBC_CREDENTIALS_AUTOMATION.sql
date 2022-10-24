USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_MTBC_CREDENTIALS_AUTOMATION]    Script Date: 7/21/2022 9:53:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_MTBC_CREDENTIALS_AUTOMATION](
	[MTBC_CREDENTIALS_AUTOMATION_ID] [bigint] NOT NULL,
	[ASSOCIATION_ID] [varchar](255) NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[FIRST_NAME] [varchar](255) NULL,
	[LAST_NAME] [varchar](255) NULL,
	[WORK_EMAIL] [varchar](255) NULL,
	[PERSONAL_MOBILE] [varchar](255) NULL,
	[CERTIFICATION_DESCRIPTION] [varchar](255) NULL,
	[CATEGORY_DESCRIPTION] [varchar](255) NULL,
	[EFFECTIVE_DATE] [datetime] NULL,
	[EXPIRATION_DATE] [datetime] NULL,
	[HR_FILE_NAME] [varchar](255) NULL,
	[CREATED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](255) NOT NULL DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL DEFAULT ((0)),
	[ISSUING_STATE] [varchar](100) NULL,
	[EMPLOYEE_NAME_DESCRIPTION] [varchar](100) NULL,
	[UNIVERSITY_DESCRIPTION] [varchar](100) NULL,
	[MENTOR] [varchar](100) NULL,
	[DATA_TYPE] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[MTBC_CREDENTIALS_AUTOMATION_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

