-- =============================================      
-- AUTHOR:  <DEVELOPER, YOUSAF>      
-- CREATE DATE: <CREATE DATE, 06/30/2018>      
-- DESCRIPTION: <GET PATIENT SURVEY HISTORY LIST>      
ALTER PROCEDURE [DBO].[FOX_PROC_GET_PATIENT_SURVEY_HISTORY_LIST]-- 1011163, 101116354870990, 816631      
 @PRACTICE_CODE BIGINT      
 ,@SURVEY_ID BIGINT      
 ,@PATIENT_ACCOUNT BIGINT      
AS      
BEGIN      
 SELECT SH.SURVEY_HISTORY_ID      
  ,SH.SURVEY_ID      
  ,SH.PATIENT_ACCOUNT      
  ,SH.IS_CONTACT_HQ      
  ,SH.IS_RESPONSED_BY_HQ      
  ,SH.IS_REFERABLE      
  ,SH.IS_IMPROVED_SETISFACTION      
  ,SH.IS_QUESTION_ANSWERED      
  ,SH.FEEDBACK      
  ,SH.SURVEY_FLAG      
  ,SH.SURVEY_STATUS_BASE      
  ,SH.SURVEY_STATUS_CHILD          
  ,SH.SURVEY_DATE      
  ,IS_SMS    
  ,IS_EMAIL,  
   CASE                                                                       
       WHEN SL.IS_EMAIL = 1                                                                                             
       THEN 'EMAIL'     
        WHEN SL.IS_SMS = 1                                                                                             
       THEN 'SMS'     
       Else    
      (AU.LAST_NAME + ', ' + AU.FIRST_NAME)                                                                                                                                              
      END AS SURVEY_BY          
 FROM FOX_TBL_PATIENT_SURVEY_HISTORY SH      
 LEFT JOIN FOX_TBL_APPLICATION_USER AU  WITH (NOLOCK) ON SH.SURVEY_BY = AU.USER_NAME      
 LEFT JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON SH.SURVEY_ID = SL.SURVEY_ID AND ISNULL(SL.DELETED, 0) = 0     
 WHERE ISNULL(Sh.DELETED, 0) = 0      
  AND sh.PRACTICE_CODE = @PRACTICE_CODE      
  AND sh.PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
END 