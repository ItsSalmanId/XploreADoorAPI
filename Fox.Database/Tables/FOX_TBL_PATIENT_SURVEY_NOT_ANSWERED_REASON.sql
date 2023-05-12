CREATE TABLE [dbo].[FOX_TBL_PATIENT_SURVEY_NOT_ANSWERED_REASON](
	[NOT_ANSWERD_REASON_ID] [bigint] NOT NULL PRIMARY KEY,
	[SURVEY_ID] [bigint] NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[NOT_ANSWERED_REASON] [varchar](70) Null,
	[CREATED_DATE] [datetime] default GetDATE() NULL,
	[CREATED_BY] [varchar](70) default 'FOXTEAM' NULL,
	[MODIFIED_DATE] [datetime] NULL default GetDATE(),
	[MODIFIED_BY] [varchar](70) default  'FOXTEAM' NULL ,
	[DELETED] [bit] NULL default 0
	)