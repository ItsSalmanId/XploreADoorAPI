-- =============================================          
-- Author:  Muhammad Salman          
-- Create date: 12/03/2022          
-- Description: This SP trigger to get patient details for survey automation service                
-- FOX_PROC_GET_PATIENT_SURVEY_DETAILS_SERVICE 1011163 
-- =============================================    
CREATE PROCEDURE FOX_PROC_GET_PATIENT_SURVEY_DETAILS_SERVICE           
 -- Add the parameters for the stored procedure here          
 @PRACTICE_CODE BIGINT          
AS          
BEGIN       
  
---DECLARE @PRACTICE_CODE BIGINT = 1011163    
    
    ---- if FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG is empty     
    DECLARE @FILE_EXIST VARCHAR(MAX) =  (SELECT TOP 1 FILE_NAME FROM FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG WITH (NOLOCK) WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 ORDER BY CREATED_DATE DESC)    
    DECLARE @CREATED_DATE DATETIME = (SELECT TOP 1 CREATED_DATE FROM FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG WITH (NOLOCK) WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 ORDER BY CREATED_DATE DESC)                                  
    DECLARE @FROMDATE DATETIME =  (dateadd(day,30,convert(datetime,@CREATED_DATE,101)))     
    DECLARE @DATE_CHECk VARCHAR(MAX) =     
 (SELECT TOP 1 FILE_NAME FROM FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG WITH (NOLOCK)     
         WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0     
                          AND    
          GETDATE() BETWEEN CREATED_DATE AND @FROMDATE    
           ORDER BY CREATED_DATE DESC)    
    
        DECLARE @SURVEY_FILE_NAME VARCHAR(MAX)              
           SET @SURVEY_FILE_NAME = (SELECT TOP 1 FILE_NAME FROM FOX_TBL_PATIENT_SURVEY WITH (NOLOCK) WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 ORDER BY CREATED_DATE DESC)          
      DECLARE @EXISTING_SURVEY_FILE_NAME VARCHAR(MAX)            
        SET @EXISTING_SURVEY_FILE_NAME = (SELECT TOP 1 FILE_NAME FROM FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG WITH (NOLOCK)     
         WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0  ORDER BY CREATED_DATE DESC)      
    
  if(@SURVEY_FILE_NAME = @EXISTING_SURVEY_FILE_NAME)  
  BEGIN  
         IF((@FILE_EXIST <> ''))  
         BEGIN  
             IF((@DATE_CHECk <> ''))  
             BEGIN  
        SELECT           
                          PS.PATIENT_ACCOUNT_NUMBER,          
                          PS.SURVEY_ID,          
                          PS.PATIENT_FIRST_NAME,          
                          PS.PATIENT_LAST_NAME,          
                          P.Email_Address,          
                          P.Home_Phone,          
                          PS.SURVEY_FORMAT_TYPE,          
                          PS.REGION,          
                          PS.PROVIDER,          
                          PS.PT_OT_SLP,          
                           PS.FILE_NAME             
                          FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)          
                          INNER JOIN PATIENT AS P WITH (NOLOCK) ON PS.PATIENT_ACCOUNT_NUMBER = + '00' + P.CHART_ID AND P.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(P.DELETED, 0) = 0     
                          WHERE PS.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(PS.DELETED, 0) = 0 AND PS.FILE_NAME = @SURVEY_FILE_NAME   
              END  
         END  
         ELSE  
         BEGIN  
           SELECT           
                          PS.PATIENT_ACCOUNT_NUMBER,          
                          PS.SURVEY_ID,          
                          PS.PATIENT_FIRST_NAME,          
                          PS.PATIENT_LAST_NAME,          
                          P.Email_Address,          
                          P.Home_Phone,          
                          PS.SURVEY_FORMAT_TYPE,          
                          PS.REGION,          
                          PS.PROVIDER,          
                          PS.PT_OT_SLP,          
                           PS.FILE_NAME             
                          FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)          
                          INNER JOIN PATIENT AS P WITH (NOLOCK) ON PS.PATIENT_ACCOUNT_NUMBER = + '00' + P.CHART_ID AND P.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(P.DELETED, 0) = 0     
                          WHERE PS.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(PS.DELETED, 0) = 0 AND PS.FILE_NAME = @SURVEY_FILE_NAME   
         END  
        END  
  ELSE  
  BEGIN  
       SELECT           
                          PS.PATIENT_ACCOUNT_NUMBER,          
                          PS.SURVEY_ID,          
                          PS.PATIENT_FIRST_NAME,          
                          PS.PATIENT_LAST_NAME,          
                          P.Email_Address,          
                          P.Home_Phone,          
                          PS.SURVEY_FORMAT_TYPE,          
                          PS.REGION,          
                          PS.PROVIDER,          
                          PS.PT_OT_SLP,          
                           PS.FILE_NAME             
                          FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)          
                          INNER JOIN PATIENT AS P WITH (NOLOCK) ON PS.PATIENT_ACCOUNT_NUMBER = + '00' + P.CHART_ID AND P.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(P.DELETED, 0) = 0     
                          WHERE PS.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(PS.DELETED, 0) = 0 AND PS.FILE_NAME = @SURVEY_FILE_NAME   
  END  
  
END 