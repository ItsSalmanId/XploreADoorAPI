Go
CREATE TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE](
	[FOX_OTP_ENABLE_ID] [bigint] NOT NULL,
	[USER_ID] [bigint] NOT NULL,
	[OTP_ENABLE_DATE] [datetime] NOT NULL,
	[CREATED_BY] [varchar](50) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](50) NOT NULL,
	[MODIFIED_DATE] [datetime] NOT NULL,
	[DELETED] [bit] NOT NULL,
 CONSTRAINT [PK_FOX_TBL_OTP_ENABLE_DATE] PRIMARY KEY CLUSTERED 
(
	[FOX_OTP_ENABLE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE] ADD  CONSTRAINT [DF_FOX_TBL_OTP_ENABLE_DATE_OTP_ENABLE_DATE]  DEFAULT (getdate()) FOR [OTP_ENABLE_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE] ADD  CONSTRAINT [DF_FOX_TBL_OTP_ENABLE_DATE_CREATED_BY]  DEFAULT ('FOX TEAM') FOR [CREATED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE] ADD  CONSTRAINT [DF_FOX_TBL_OTP_ENABLE_DATE_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE] ADD  CONSTRAINT [DF_FOX_TBL_OTP_ENABLE_DATE_MODIFIED_BY]  DEFAULT ('FOX TEAM') FOR [MODIFIED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE] ADD  CONSTRAINT [DF_FOX_TBL_OTP_ENABLE_DATE_MODIFIED_DATE]  DEFAULT (getdate()) FOR [MODIFIED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_OTP_ENABLE_DATE] ADD  CONSTRAINT [DF_FOX_TBL_OTP_ENABLE_DATE_DELETED]  DEFAULT ((0)) FOR [DELETED]
GO  
