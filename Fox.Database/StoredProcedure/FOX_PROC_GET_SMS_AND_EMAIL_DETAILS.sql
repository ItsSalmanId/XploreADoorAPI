-- Author:  Muhammad Salman       
-- Create date: 12/22/2022        
-- Description: This SP trigger to get send Sms And Email details for survey automation service        
-- =============================================        
-- FOX_PROC_GET_SMS_AND_EMAIL_DETAILS 101116354816632 ,1011163 ,101116354870914       
CREATE PROCEDURE FOX_PROC_GET_SMS_AND_EMAIL_DETAILS         
 -- Add the parameters for the stored procedure here        
 @PATIENT_ACCOUNT BIGINT,      
 @PRACTICE_CODE BIGINT,      
 @SURVEY_ID BIGINT      
AS        
BEGIN        
        
  Select PS.IS_SURVEYED,* From FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL      
  INNER JOIN FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID AND PS.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(PS.DELETED, 0) = 0 AND PS.SURVEY_ID = @SURVEY_ID      
   where SL.PATIENT_ACCOUNT = @PATIENT_ACCOUNT AND SL.PRACTICE_CODE = @PRACTICE_CODE AND SL.SURVEY_ID = @SURVEY_ID  AND ISNULL(SL.IS_SMS, 1) = 1  
          
END 