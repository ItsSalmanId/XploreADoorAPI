USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT_UPDATE_HISTORY]    Script Date: 7/21/2022 9:22:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT_UPDATE_HISTORY](
	[patient_update_history_id] [bigint] NOT NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[field_name] [varchar](100) NULL,
	[previous_value] [varchar](500) NULL,
	[new_value] [varchar](500) NULL,
	[ip_address] [varchar](20) NULL,
	[updated_by] [varchar](70) NULL,
	[updated_time] [datetime] NULL,
	[isActive] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_P__isAct__491B4398]  DEFAULT ((1)),
	[isDeleted] [bit] NOT NULL CONSTRAINT [DF__FOX_TBL_P__isDel__4A0F67D1]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_F798FF808E684ABC86F771BD28833AB1]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK__FOX_TBL___89180DADA013F9D9] PRIMARY KEY CLUSTERED 
(
	[patient_update_history_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


