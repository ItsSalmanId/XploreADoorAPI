--------------------------------------------------------
----- Folder Name: DevScripts > DataModifications
----- File Name: DataModifications.sql	
----- Created By: Muhammad Arslan Tufail
----- Created Date: 08/25/2022
----- Modified By: Muhammad Taseer Iqbal
----- Modified Date: 09/15/2022
--------------------------------------------------------

--------------------------------------------------------
----- JIRA ID: FOX-6593
----- Task Name: Item P504 | Quality Assurance Scoring Enhancements - development
----- Assignee: Muhammad Taseer Iqbal
--------------------------------------------------------

UPDATE FOX_TBL_EVALUATION_CRITERIA_CATEGORIES 
SET DELETED = 1 
WHERE EVALUATION_CRITERIA_CATEGORIES_ID IN (
600216
,600217
,600218
,600219
,600220
,600221
,600222
,600223
,600224
,600225
,600226
,600227
,600228
,600229)
GO

INSERT INTO [dbo].[FOX_TBL_EVALUATION_CRITERIA_CATEGORIES]
           ([EVALUATION_CRITERIA_CATEGORIES_ID]
           ,[PRACTICE_CODE]
           ,[CATEGORIES_NAME]
           ,[EVALUATION_CRITERIA_ID]
           ,[CATEGORIES_POINTS]
           ,[CREATED_BY]
           ,[CREATED_DATE]
           ,[MODIFIED_BY]
           ,[MODIFIED_DATE]
           ,[DELETED]
           ,[CALL_TYPE])
     VALUES
           (600248,1011163,'Appropriate Greeting'                                                    ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600249,1011163,'Was the agent able to match the tone of patient'                         ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600250,1011163,'Was the agent able to demonstrate compassion and empathy'                ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600251,1011163,'Proper grammar and pronunciation used'                                   ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600252,1011163,'Did the agent verify the patients identity'                              ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600253,1011163,'Was the agent able to answer the patients questions'                     ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600254,1011163,'Did the agent demonstrate strong product knowledge'                      ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600255,1011163,'Was the agent able to use available resources to communicate information',600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600256,1011163,'Appropriate follow_up documentation completed and communicated'          ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600257,1011163,'Appropriate closing'                                                     ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   ---------------------------------------------------------for live pratice--------------------------------------------------------------------------
           (600258,1012714,'Appropriate Greeting'                                                    ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600259,1012714,'Was the agent able to match the tone of patient'                         ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600260,1012714,'Was the agent able to demonstrate compassion and empathy'                ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600261,1012714,'Proper grammar and pronunciation used'                                   ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600262,1012714,'Did the agent verify the patients identity'                              ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600263,1012714,'Was the agent able to answer the patients questions'                     ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600264,1012714,'Did the agent demonstrate strong product knowledge'                      ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600265,1012714,'Was the agent able to use available resources to communicate information',600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600266,1012714,'Appropriate follow_up documentation completed and communicated'          ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600267,1012714,'Appropriate closing'                                                     ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD');
GO

UPDATE FOX_TBL_EVALUATION_CRITERIA SET PERCENTAGE = 40 WHERE CRITERIA_NAME = 'Client Experience' AND CALL_TYPE = 'PHD'
UPDATE FOX_TBL_EVALUATION_CRITERIA SET PERCENTAGE = 60 WHERE CRITERIA_NAME = 'System Product and Process' AND CALL_TYPE = 'PHD'
GO

UPDATE FOX_TBL_SURVEY_AUDIT_SCORES SET SCORING_CRITERIA = 'old' WHERE CALL_TYPE = 'phd'
GO

--------------------------------------------------------Consent To Care---------------------------------
DECLARE @MAX_DOCUMENT_TYPE_ID BIGINT = (select MAX(DOCUMENT_TYPE_ID)+1 from FOX_TBL_DOCUMENT_TYPE)
INSERT INTO FOX_TBL_DOCUMENT_TYPE (DOCUMENT_TYPE_ID,RT_CODE,NAME,DESCRIPTION,IS_ONLINE_ORDER_LIST, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED)
Values (@MAX_DOCUMENT_TYPE_ID, 'CONT', 'PCORS', 'Patient Consent Form', 0, 'FOX TEAM',GETDATE(), 'FOX TEAM',GETDATE(), 0)




---------------FOX_TBL_CONSENT_TO_CARE_STATUS Insert Query-------------------

DECLARE @TAB_STATUS_ID TABLE (MAX_STATUS_ID BIGINT)
DECLARE @MAX_STATUS_ID BIGINT

INSERT INTO @TAB_STATUS_ID EXEC Web_GetMaxColumnID 'CONSENT_TO_CARE_STATUS_ID'
SET @MAX_STATUS_ID = (SELECT TOP 1 MAX_STATUS_ID FROM @TAB_STATUS_ID)

DECLARE @TempTable TABLE (STATUS_NAME NVARCHAR(255), PRACTICE_CODE INT)

INSERT INTO @TempTable (STATUS_NAME, PRACTICE_CODE)
VALUES
    ('Sent', 1012714),
    ('Signed', 1012714),
    ('No Response', 1012714),
    ('Expired', 1012714),
    ('Unsent', 1012714),
    ('Sent', 1011163),
    ('Signed', 1011163),
    ('No Response', 1011163),
    ('Expired', 1011163),
    ('Unsent', 1011163)


INSERT INTO FOX_TBL_CONSENT_TO_CARE_STATUS (CONSENT_TO_CARE_STATUS_ID, STATUS_NAME, PRACTICE_CODE)
SELECT
    @MAX_STATUS_ID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
    STATUS_NAME,
    PRACTICE_CODE
FROM @TempTable

GO 

insert INTO MAINTENANCE_COUNTER (Col_Name, Col_Counter) values ('CONSENT_TO_CARE_STATUS_ID', 100)


insert INTO MAINTENANCE_COUNTER (Col_Name, Col_Counter) values ('CONSENT_TO_CARE_ID', 100)


insert INTO MAINTENANCE_COUNTER (Col_Name, Col_Counter) values ('DOCUMENTS_ID', 100)
