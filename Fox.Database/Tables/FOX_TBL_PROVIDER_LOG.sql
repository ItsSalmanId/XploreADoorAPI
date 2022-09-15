USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_tbl_provider_log]    Script Date: 7/21/2022 9:28:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_tbl_provider_log](
	[Provider_Log_ID] [bigint] NOT NULL,
	[Practice_Code] [bigint] NOT NULL,
	[Provider_Code] [bigint] NOT NULL,
	[Termination_Date] [datetime] NULL,
	[Rejoining_Date] [datetime] NULL,
	[Created_By] [varchar](60) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [varchar](60) NOT NULL,
	[Modified_Date] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider_Log_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Fox_tbl_provider_log] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO

ALTER TABLE [dbo].[Fox_tbl_provider_log] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO

ALTER TABLE [dbo].[Fox_tbl_provider_log] ADD  DEFAULT ((0)) FOR [Deleted]
GO


