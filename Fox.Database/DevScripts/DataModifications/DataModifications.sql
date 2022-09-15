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

--------------------------------------------------------
