USE [MIS_DB]
GO

CREATE TABLE [dbo].[FOX_TBL_SURVEY_QUESTION](
	[SURVEY_QUESTIONS_ID] [bigint] NOT NULL,
	[SURVEY_QUESTIONS] [varchar](max) NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](70) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](70) NULL,
	[DELETED] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[SURVEY_QUESTIONS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FOX_TBL_SURVEY_QUESTION] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_SURVEY_QUESTION] ADD  DEFAULT ('FOX_TEAM') FOR [CREATED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_SURVEY_QUESTION] ADD  DEFAULT (getdate()) FOR [MODIFIED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_SURVEY_QUESTION] ADD  DEFAULT ('FOX_TEAM') FOR [MODIFIED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_SURVEY_QUESTION] ADD  DEFAULT ((0)) FOR [DELETED]
GO