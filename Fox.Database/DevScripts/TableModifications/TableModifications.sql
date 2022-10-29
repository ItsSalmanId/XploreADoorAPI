--------------------------------------------------------
----- Folder Name: DevScripts > TableModifications
----- File Name: TableModifications.sql	
----- Created By: Muhammad Arslan Tufail
----- Created Date: 08/25/2022
----- Modified By: Muhammad Taseer Iqbal
----- Modified Date: 09/15/2022
--------------------------------------------------------

--------------------------------------------------------
----- JIRA ID: FOX-6593
----- Task Name: Item P504 | Quality Assurance Scoring Enhancements - development
----- Assignee: Muhammad Taseer Iqbal
----  Non Replication Table
--------------------------------------------------------

ALTER TABLE FOX_TBL_SURVEY_AUDIT_SCORES
ADD  APPROPRIATE_GREETING BIGINT
,APPROPRIATE_GREETING_TOTAL BIGINT
,TONE_OF_PATIENT BIGINT
,TONE_OF_PATIENT_TOTAL BIGINT
,COMPASSION_AND_EMPATHY BIGINT
,COMPASSION_AND_EMPATHY_TOTAL BIGINT
,GRAMMAR_AND_PRONUNCIATION BIGINT
,GRAMMAR_AND_PRONUNCIATION_TOTAL BIGINT
,PATIENT_IDENTITY BIGINT
,PATIENT_IDENTITY_TOTAL BIGINT
,ANSWER_PATIENT_QUESTIONS BIGINT
,ANSWER_PATIENT_QUESTIONS_TOTAL BIGINT
,STRONG_PRODUCT_KNOWLEDGE BIGINT
,STRONG_PRODUCT_KNOWLEDGE_TOTAL BIGINT
,COMMUNICATE_INFORMATION BIGINT
,COMMUNICATE_INFORMATION_TOTAL BIGINT
,DOCUMENTATION_COMPLETED_COMMUNICATED BIGINT
,DOCUMENTATION_COMPLETED_COMMUNICATED_TOTAL BIGINT
,APPROPRIATE_CLOSING BIGINT
,APPROPRIATE_CLOSING_TOTAL BIGINT;
GO

--------------------------------------------------------
----- JIRA ID: FOX-6872
----- Task Name: Item P522 | Alert Window Upon Login
----- Assignee: Muhammad Salman
----  Non Replication Table
--------------------------------------------------------
CREATE TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_ROLE](
	[ANNOUNCEMENT_ROLE_ID] [bigint] NOT NULL,
	[ROLE_ID] [bigint] NULL,
	[ROLE_NAME] [varchar](50) NULL,
	[ANNOUNCEMENT_ID] [bigint] NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](70) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](70) NULL,
	[DELETED] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ANNOUNCEMENT_ROLE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_ROLE] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_ROLE] ADD  DEFAULT ('FOX_TEAM') FOR [CREATED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_ROLE] ADD  DEFAULT (getdate()) FOR [MODIFIED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_ROLE] ADD  DEFAULT ('FOX_TEAM') FOR [MODIFIED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_ROLE] ADD  DEFAULT ('0') FOR [DELETED]
GO

--------------------------------------------------------
----- JIRA ID: FOX-6872
----- Task Name: Item P522 | Alert Window Upon Login
----- Assignee: Muhammad Salman
----  Non Replication Table
--------------------------------------------------------
CREATE TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_HISTORY](
	[ANNOUNCEMENT_HISTORY_ID] [bigint] NOT NULL,
	[ANNOUNCEMENT_ID] [bigint] NULL,
	[USER_ID] [bigint] NULL,
	[USER_NAME] [varchar](50) NULL,
	[SHOW_COUNT] [int] NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](70) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](70) NULL,
	[DELETED] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ANNOUNCEMENT_HISTORY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_HISTORY] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_HISTORY] ADD  DEFAULT ('FOX_TEAM') FOR [CREATED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_HISTORY] ADD  DEFAULT (getdate()) FOR [MODIFIED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_HISTORY] ADD  DEFAULT ('FOX_TEAM') FOR [MODIFIED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT_HISTORY] ADD  DEFAULT ('0') FOR [DELETED]
GO
--------------------------------------------------------
----- JIRA ID: FOX-6872
----- Task Name: Item P522 | Alert Window Upon Login
----- Assignee: Muhammad Salman
----  Non Replication Table
--------------------------------------------------------
CREATE TABLE [dbo].[FOX_TBL_ANNOUNCEMENT](
	[ANNOUNCEMENT_ID] [bigint] NOT NULL,
	[ANNOUNCEMENT_DATE_FROM] [datetime] NULL,
	[ANNOUNCEMENT_DATE_TO] [datetime] NULL,
	[ANNOUNCEMENT_TITLE] [varchar](100) NULL,
	[ANNOUNCEMENT_DETAILS] [varchar](max) NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](70) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](70) NULL,
	[DELETED] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ANNOUNCEMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT] ADD  DEFAULT ('FOX_TEAM') FOR [CREATED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT] ADD  DEFAULT (getdate()) FOR [MODIFIED_DATE]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT] ADD  DEFAULT ('FOX_TEAM') FOR [MODIFIED_BY]
GO

ALTER TABLE [dbo].[FOX_TBL_ANNOUNCEMENT] ADD  DEFAULT ('0') FOR [DELETED]
GO
--------------------------------------------------------
----- JIRA ID: FOX-6595
----- Task Name: Item P512 | Knowledge Base/ FAQs in Patient Helpdesk
----- Assignee: Muhammad Salman
----  Non Replication Table
--------------------------------------------------------
CREATE TABLE [dbo].[FOX_TBL_PHD_FAQS_DETAILS](
	[FAQS_ID] [bigint] NULL,
	[QUESTIONS] [varchar](250) NOT NULL,
	[ANSWERS] [nvarchar](max) NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[CREATED_BY] [varchar](70) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](70) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[DELETED] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

