USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_APPLICATION_ROLE]    Script Date: 7/21/2022 8:41:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_APPLICATION_ROLE](
	[Id] [varchar](128) NOT NULL,
	[Name] [varchar](256) NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_dbo.FOX_TBL_APPLICATION_ROLE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FOX_TBL_APPLICATION_ROLE] ADD  CONSTRAINT [MSmerge_df_rowguid_16F22F1D6E714A32BB141C8144BBDA9A]  DEFAULT (newsequentialid()) FOR [rowguid]
GO


