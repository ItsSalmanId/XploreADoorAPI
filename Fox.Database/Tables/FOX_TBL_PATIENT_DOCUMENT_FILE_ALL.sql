USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT_DOCUMENT_FILE_ALL]    Script Date: 7/21/2022 9:16:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT_DOCUMENT_FILE_ALL](
	[PATIENT_DOCUMENT_FILE_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NOT NULL,
	[PAT_DOCUMENT_ID] [bigint] NULL,
	[DOCUMENT_PATH] [varchar](500) NULL,
	[DOCUMENT_LOGO_PATH] [varchar](500) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_DOCUMENT_FILE_ALL_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_DOCUMENT_FILE_ALL_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_DOCUMENT_FILE_ALL_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_DOCUMENT_FILE_ALL_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_DOCUMENT_FILE_ALL_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_70E3F4B20FD24BD8B92B7A8750C9794A]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_PATIENT_DOCUMENT_FILE_ALL] PRIMARY KEY CLUSTERED 
(
	[PATIENT_DOCUMENT_FILE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

