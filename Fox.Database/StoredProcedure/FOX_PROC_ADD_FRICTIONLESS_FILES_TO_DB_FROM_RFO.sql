-- =============================================                                              
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                                              
-- CREATE DATE: <CREATE DATE, 22/09/2022>                                              
-- DESCRIPTION:  THIS PROCEDURE ISUSE TO ADD FRICTIONLESS FILE PATH                                         
-- =============================================         
CREATE PROCEDURE [dbo].[FOX_PROC_ADD_FRICTIONLESS_FILES_TO_DB_FROM_RFO]         
@FRICTIONLESS_ID BIGINT,            
@WORKID BIGINT,                   
@FILEPATH VARCHAR(500),                
@LOGOPATH VARCHAR(500),  
@PRACTICE_CODE BIGINT  
     
AS                                
BEGIN                                          
 BEGIN              
 INSERT INTO FOX_TBL_FRICTIONLESS_WORK_QUEUE_FILE_ALL               
 (FRICTIONLESS_REFERRAL_FILE_ID, UNIQUE_ID, FILE_PATH, FILE_PATH1, deleted, WORK_ID,PRACTICE_CODE)              
 VALUES (@FRICTIONLESS_ID,  CAST(@WORKID AS VARCHAR), @LOGOPATH, @FILEPATH, 0, @WORKID,@PRACTICE_CODE);                   
 END                  
END; 