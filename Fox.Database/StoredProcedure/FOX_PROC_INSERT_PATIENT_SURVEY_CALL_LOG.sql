IF (OBJECT_ID('FOX_PROC_INSERT_PATIENT_SURVEY_CALL_LOG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_PATIENT_SURVEY_CALL_LOG  
GO 
			  -- =============================================              
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>              
-- CREATE DATE: <CREATE DATE, 05/011/2020>              
-- DESCRIPTION: <INSERT PATIENT SURVEY CALL LOG>              
              
 --EXEC FOX_PROC_INSERT_PATIENT_SURVEY_CALL_LOG 548395,         
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
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_PATIENT_SURVEY_CALL_LOG]              
@SURVEY_CALL_ID BIGINT,       
@PRACTICE_CODE BIGINT,       
@ACU_CALL_ID BIGINT,       
@SURVEY_ID BIGINT,       
@PATIENT_ACCOUNT BIGINT,       
@FILE_NAME VARCHAR(500),      
@IS_RECEIVED BIT,      
@CALL_OUT_COME VARCHAR(50),      
@CALL_DURATION VARCHAR(20),      
@IS_TO_PATIENT BIT,      
@CREATED_BY VARCHAR(70),      
@CREATED_DATE DATETIME,      
@MODIFIED_BY VARCHAR(70),      
@MODIFIED_DATE DATETIME,      
@DELETED BIT      
      
AS              
BEGIN              
            
INSERT INTO FOX_TBL_PATIENT_SURVEY_CALL_LOG              
(              
SURVEY_CALL_ID,      
PRACTICE_CODE,      
ACU_CALL_ID,      
SURVEY_ID,      
PATIENT_ACCOUNT,      
FILE_NAME,      
IS_RECEIVED,      
CALL_OUT_COME,      
CALL_DURATION,      
IS_TO_PATIENT,      
CREATED_BY,      
CREATED_DATE,      
MODIFIED_BY,      
MODIFIED_DATE,      
DELETED)              
VALUES              
(      
@SURVEY_CALL_ID,      
@PRACTICE_CODE,      
@ACU_CALL_ID,      
@SURVEY_ID,      
@PATIENT_ACCOUNT,      
@FILE_NAME,      
@IS_RECEIVED,      
@CALL_OUT_COME,      
@CALL_DURATION,      
@IS_TO_PATIENT,      
@CREATED_BY,      
GETDATE(),      
@MODIFIED_BY,      
GETDATE(),      
@DELETED);              
END 


