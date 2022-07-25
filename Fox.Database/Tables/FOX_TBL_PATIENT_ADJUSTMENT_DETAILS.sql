USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT_ADJUSTMENT_DETAILS]    Script Date: 7/21/2022 9:11:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT_ADJUSTMENT_DETAILS](
	[ADJUSTMENT_DETAIL_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NOT NULL,
	[REQUESTED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_REQUESTED_DATE]  DEFAULT (getdate()),
	[REQUESTED_BY] [varchar](70) NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[DOS_FROM] [datetime] NULL,
	[DOS_TO] [datetime] NULL,
	[DISCIPLINE_ID] [int] NULL,
	[ADJUSTMENT_AMOUNT] [money] NULL,
	[FLAG_17B9W] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17B9W]  DEFAULT ((0)),
	[FLAG_17BAN] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17BAN]  DEFAULT ((0)),
	[FLAG_17BDW] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17BDW]  DEFAULT ((0)),
	[FLAG_17BEA] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17BEA]  DEFAULT ((0)),
	[FLAG_17BER] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17BER]  DEFAULT ((0)),
	[FLAG_17CHP] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17CHP]  DEFAULT ((0)),
	[FLAG_17CO] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17CO]  DEFAULT ((0)),
	[FLAG_17FUA] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17FUA]  DEFAULT ((0)),
	[FLAG_17FUO] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17FUO]  DEFAULT ((0)),
	[FLAG_17HHE] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17HHE]  DEFAULT ((0)),
	[FLAG_17INC] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_INC]  DEFAULT ((0)),
	[FLAG_17INS] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_INS]  DEFAULT ((0)),
	[FLAG_17LTC] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17LTC]  DEFAULT ((0)),
	[FLAG_17MCR] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17MCR]  DEFAULT ((0)),
	[FLAG_17MDW] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17MDW]  DEFAULT ((0)),
	[FLAG_17MED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17MED]  DEFAULT ((0)),
	[FLAG_17NOA] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17NOA]  DEFAULT ((0)),
	[FLAG_17PTF] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17PTF]  DEFAULT ((0)),
	[FLAG_17SBW] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17SBW]  DEFAULT ((0)),
	[FLAG_17SCW] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17SCW]  DEFAULT ((0)),
	[FLAG_17WCW] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17WCW]  DEFAULT ((0)),
	[FLAG_17PEC] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17PEC]  DEFAULT ((0)),
	[FLAG_17PED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_17PED]  DEFAULT ((0)),
	[FLAG_OTHER] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_FLAG_OTHER]  DEFAULT ((0)),
	[OTHER_DESCRIPTION] [varchar](100) NULL,
	[REASON] [varchar](3000) NULL,
	[ADJUSTMENT_STATUS_ID] [int] NULL,
	[APPROVED_BY] [varchar](70) NULL,
	[APPROVED_DATE] [datetime] NULL,
	[APPROVED_SIGN_PATH] [varchar](500) NULL,
	[ASSIGNED_TO] [varchar](70) NULL,
	[CLOSED_BY] [varchar](70) NULL,
	[CLOSED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_DD51265DEC594360B84A3CDB0422098E]  DEFAULT (newsequentialid()),
	[CLAIM_NO] [bigint] NULL,
 CONSTRAINT [PK_FOX_TBL_PATIENT_ADJUSTMENT_DETAILS] PRIMARY KEY CLUSTERED 
(
	[ADJUSTMENT_DETAIL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


