USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_INTERFACE_LOG]    Script Date: 7/21/2022 9:06:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_INTERFACE_LOG](
	[FOX_INTERFACE_LOG_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[ERROR] [varchar](1000) NULL,
	[ACK] [varchar](2500) NULL,
	[IS_ERROR] [bit] NULL,
	[LOG_MESSAGE] [varchar](5000) NULL,
	[IS_INCOMMING] [bit] NULL,
	[IS_OUTGOING] [bit] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_INTERFACE_LOG_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_I__CREAT__42F887B8]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_INTERFACE_LOG_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF__FOX_TBL_I__MODIF__44E0D02A]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_I__DELET__45D4F463]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_52B098506022470886F3A1A57E4DFF68]  DEFAULT (newsequentialid()),
	[FOX_INTERFACE_SYNCH_ID] [bigint] NULL,
	[ORIGINAL_TIME] [datetime] NULL,
 CONSTRAINT [PK__FOX_TBL___FCF14DEBE347A135] PRIMARY KEY CLUSTERED 
(
	[FOX_INTERFACE_LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

