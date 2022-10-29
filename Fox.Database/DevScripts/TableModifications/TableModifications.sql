--------------------------------------------------------
----- Folder Name: DevScripts > TableModifications
----- File Name: TableModifications.sql	
----- Created By: Muhammad Arslan Tufail
----- Created Date: 08/25/2022
----- Modified By: Muhammad Taseer Iqbal
----- Modified Date: 10/29/2022
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
----- JIRA ID: FOX-6993
----- Task Name: QAR Perform an Audit - Issues - development
----- Assignee: Muhammad Taseer Iqbal
----  Non Replication Table
--------------------------------------------------------

	ALTER TABLE FOX_TBL_SURVEY_AUDIT_SCORES ALTER COLUMN CLIENT_EXPERIENCE_COMMENT VARCHAR(1000)
	GO
	ALTER TABLE FOX_TBL_SURVEY_AUDIT_SCORES ALTER COLUMN SYSTEM_PROCESS_COMMENT VARCHAR(1000)
	GO
	ALTER TABLE FOX_TBL_SURVEY_AUDIT_SCORES ALTER COLUMN WOW_FACTOR_COMMENT VARCHAR(1000)
	GO
--------------------------------------------------------