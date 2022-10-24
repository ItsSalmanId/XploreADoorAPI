USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT_NOTESHISTORY]    Script Date: 7/21/2022 9:17:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY](
	[NOTE_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[NOTE_DESC] [varchar](100) NULL,
	[CREATED_BY] [varchar](70) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](70) NOT NULL,
	[MODIFIED_DATE] [datetime] NOT NULL,
	[DELETED] [bit] NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK__FOX_TBL___94BB9521E77F48E3] PRIMARY KEY CLUSTERED 
(
	[NOTE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY] ADD  CONSTRAINT [DF_FOX_TBL_PATIENT_NOTESHISTORY_CREATED_BY]  DEFAULT ('FOX TEAM') FOR [CREATED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY] ADD  CONSTRAINT [DF_FOX_TBL_PATIENT_NOTESHISTORY_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY] ADD  CONSTRAINT [DF_FOX_TBL_PATIENT_NOTESHISTORY_MODIFIED_BY]  DEFAULT ('FOX TEAM') FOR [MODIFIED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY] ADD  CONSTRAINT [DF_FOX_TBL_PATIENT_NOTESHISTORY_MODIFIED_DATE]  DEFAULT (getdate()) FOR [MODIFIED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY] ADD  CONSTRAINT [DF__FOX_TBL_P__DELET__39D90008]  DEFAULT ((0)) FOR [DELETED]
GO

ALTER TABLE [dbo].[FOX_TBL_PATIENT_NOTESHISTORY] ADD  CONSTRAINT [MSmerge_df_rowguid_C18A5AB592604E70A308B848BF778518]  DEFAULT (newsequentialid()) FOR [rowguid]
GO

