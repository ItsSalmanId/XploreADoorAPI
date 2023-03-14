-- =============================================              
-- Author:  Muhammad Salman             
-- Create date: 12/22/2022              
-- Description: This SP trigger to get send Sms And Email Unsubscription details for survey automation service              
-- =============================================              
-- FOX_PROC_GET_SMS_AND_EMAIL_UNSUBSCRIPTION_DETAILS 816631 ,1011163 , 'SMS'             
CREATE PROCEDURE FOX_PROC_GET_SMS_AND_EMAIL_UNSUBSCRIPTION_DETAILS               
 -- Add the parameters for the stored procedure here              
 @PATIENT_ACCOUNT BIGINT,            
 @PRACTICE_CODE BIGINT,   
 @SURVEY_ID BIGINT,            
 @SMS_OR_EMAIL VARCHAR(10)            
AS              
BEGIN              
         
 IF @SMS_OR_EMAIL = 'SMS'         
 BEGIN      
 Select PATIENT_ACCOUNT From  FOX_TBL_AUTOMATED_SURVEY_UNSUBSCRIPTION where PATIENT_ACCOUNT = @PATIENT_ACCOUNT AND SURVEY_ID = @SURVEY_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(SMS_UNSUBSCRIBE, 0) = 1 AND ISNULL(DELETED, 0) = 0          
 END      
 ELSE       
 BEGIN      
 Select PATIENT_ACCOUNT From  FOX_TBL_AUTOMATED_SURVEY_UNSUBSCRIPTION where PATIENT_ACCOUNT = @PATIENT_ACCOUNT AND SURVEY_ID = @SURVEY_ID AND PRACTICE_CODE = @PRACTICE_CODE AND  ISNULL(EMAIL_UNSUBSCRIBE, 0) = 1  AND ISNULL(DELETED, 0) = 0           
 END      
END