USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_VISIT_TYPE]    Script Date: 7/21/2022 9:40:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_VISIT_TYPE](
	[VISIT_TYPE_ID] [bigint] NOT NULL,
	[DESCRIPTION] [varchar](50) NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_VISIT_TYPE_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_VISIT_TYPE_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NULL,
	[DELETED] [bit] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_505A041D0CC448E3910F40365AC0D711]  DEFAULT (newsequentialid()),
	[show_for_appointment] [bit] NULL,
	[RT_CODE] [varchar](15) NULL,
	[PRACTICE_CODE] [bigint] NULL,
 CONSTRAINT [PK_FOX_TBL_VISIT_TYPE] PRIMARY KEY CLUSTERED 
(
	[VISIT_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

