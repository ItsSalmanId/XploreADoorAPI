IF (OBJECT_ID('FOX_PROC_INSERT_PATIENT_SURVEY_HISTORY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_PATIENT_SURVEY_HISTORY  
GO 
-- =============================================                        
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>                        
-- CREATE DATE: <CREATE DATE, 05/09/2020>                        
-- DESCRIPTION: <INSERT PATIENT SURVEY HISTORY>                        
                        
 --EXEC FOX_PROC_INSERT_PATIENT_SURVEY_HISTORY 548395,                   
 --    1011163,                  
 --    0000,                  
 --    000,                  
 --    1,                  
 --    0,                  
 --    1,                  
 --    0,                  
 --    'Test',                  
 --    'Green  ',                  
 --    'Completed',                  
 --    'Recommended' ,                  
 --    'Rafay',                
 -- '10/22/2019 3:17:09 AM',                
 --    'RAFAY',                
 --  NULL,                
 --  0,                
 --  1                               
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_PATIENT_SURVEY_HISTORY] --222,1011163,111,0000,1,1,1,1,'test','red','complete survey','completed' ,'test','01/15/2020','test','01/15/2020', 1,1,1,1                      
@SURVEY_HISTORY_ID BIGINT,                  
@PRACTICE_CODE BIGINT,                  
@SURVEY_ID BIGINT,                
@PATIENT_ACCOUNT BIGINT,                
@IS_CONTACT_HQ BIT,                
@IS_RESPONSED_BY_HQ BIT,                
@IS_REFERABLE BIT,                
@IS_IMPROVED_SETISFACTION BIT,                
@FEEDBACK VARCHAR(300),                
@SURVEY_FLAG VARCHAR(50),                
@SURVEY_STATUS_BASE VARCHAR(50),                
@SURVEY_STATUS_CHILD VARCHAR(50),                
@SURVEY_BY VARCHAR(70),                
@SURVEY_DATE DATETIME,                
@CREATED_BY  VARCHAR(70),                
@CREATED_DATE DATETIME,                
@DELETED BIT,                
@IS_QUESTION_ANSWERED BIT,          
@IS_EXCEPTIONAL BIT,       
@IS_PROTECTIVE_EQUIPMENT BIT               
                
AS                        
BEGIN                        
                      
INSERT INTO FOX_TBL_PATIENT_SURVEY_HISTORY                        
(                        
SURVEY_HISTORY_ID,                
PRACTICE_CODE,                
SURVEY_ID,                
PATIENT_ACCOUNT,                
IS_CONTACT_HQ,                
IS_RESPONSED_BY_HQ,                
IS_REFERABLE,                
IS_IMPROVED_SETISFACTION,                
FEEDBACK,                
SURVEY_FLAG,                
SURVEY_STATUS_BASE,                
SURVEY_STATUS_CHILD,                
SURVEY_BY,                
SURVEY_DATE,                
CREATED_BY,                
CREATED_DATE,                
DELETED,                
IS_QUESTION_ANSWERED,          
IS_EXCEPTIONAL,      
IS_PROTECTIVE_EQUIPMENT)                        
VALUES                        
  (@SURVEY_HISTORY_ID,                        
   @PRACTICE_CODE,                   
   @SURVEY_ID,                  
   @PATIENT_ACCOUNT,                        
   @IS_CONTACT_HQ,                        
   @IS_RESPONSED_BY_HQ,                
   @IS_REFERABLE,                
   @IS_IMPROVED_SETISFACTION,                
   @FEEDBACK,                
   @SURVEY_FLAG,                
   @SURVEY_STATUS_BASE,                
   @SURVEY_STATUS_CHILD,                
   @SURVEY_BY,                
   CAST(@SURVEY_DATE AS DATETIME),                
   @CREATED_BY,                
   GETDATE(),                
   @DELETED,                
   @IS_QUESTION_ANSWERED,          
   @IS_EXCEPTIONAL,      
   @IS_PROTECTIVE_EQUIPMENT);                        
END  
  
