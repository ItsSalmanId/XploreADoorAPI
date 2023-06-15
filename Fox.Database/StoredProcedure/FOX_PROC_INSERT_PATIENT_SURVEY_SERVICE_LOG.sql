-- =============================================            
-- Author:  Muhammad Arslan Tufail            
-- Create date: 12/03/2022            
-- Description: This SP trigger to INSERT Log of patient for survey automation service            
-- =============================================            
-- FOX_PROC_INSERT_PATIENT_SURVEY_SERVICE_LOG 1011163            
CREATE PROCEDURE FOX_PROC_INSERT_PATIENT_SURVEY_SERVICE_LOG                      
 @SURVEY_AUTOMATION_LOG_ID BIGINT,            
 @PATIENT_ACCOUNT BIGINT,            
 @SURVEY_ID BIGINT,            
 @FILE_NAME VARCHAR(MAX),            
 @PRACTICE_CODE BIGINT,      
 @IS_SMS bit,      
 @IS_EMAIL bit,  
 @DELETED bit     
AS            
BEGIN            
    DECLARE @EXISTING_SURVEY_AUTOMATION_LOG_ID BIGINT      
 SET @EXISTING_SURVEY_AUTOMATION_LOG_ID = ( Select TOP 1 SURVEY_AUTOMATION_LOG_ID From FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG  where PATIENT_ACCOUNT = @PATIENT_ACCOUNT AND SURVEY_ID = @SURVEY_ID AND PRACTICE_CODE = @PRACTICE_CODE AND DELETED = 0)      
 IF(@EXISTING_SURVEY_AUTOMATION_LOG_ID <> '')      
   UPDATE FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG      
      SET PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
     ,SURVEY_ID = @SURVEY_ID      
     ,[FILE_NAME]  = @FILE_NAME        
     ,PRACTICE_CODE = @PRACTICE_CODE      
     ,IS_SMS = @IS_SMS      
     ,IS_EMAIL = @IS_EMAIL  
  ,DELETED = @DELETED     
       where SURVEY_AUTOMATION_LOG_ID = @EXISTING_SURVEY_AUTOMATION_LOG_ID         
   ELSE       
       INSERT INTO FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG            
      (            
      SURVEY_AUTOMATION_LOG_ID            
      ,PATIENT_ACCOUNT            
      ,SURVEY_ID            
      ,[FILE_NAME]           
      ,PRACTICE_CODE      
      ,IS_SMS      
      ,IS_EMAIL     
   ,DELETED           
      )            
      VALUES            
      (            
      @SURVEY_AUTOMATION_LOG_ID,            
      @PATIENT_ACCOUNT,            
      @SURVEY_ID,            
      @FILE_NAME,            
      @PRACTICE_CODE,      
      @IS_SMS,      
      @IS_EMAIL,  
   @DELETED          
      )        
END 