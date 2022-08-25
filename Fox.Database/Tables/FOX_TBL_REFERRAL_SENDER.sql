USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_REFERRAL_SENDER]    Script Date: 7/21/2022 9:58:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_REFERRAL_SENDER](
	[ID] [bigint] NOT NULL,
	[SENDER] [varchar](100) NOT NULL,
	[FOR_STRATEGIC] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_SENDER_FOR_STRATEGIC]  DEFAULT ((0)),
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_SENDER_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_SENDER_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_SENDER_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_SENDER_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_SENDER_DELETED]  DEFAULT ((0)),
 CONSTRAINT [PK_FOX_TBL_REFERRAL_SENDER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


