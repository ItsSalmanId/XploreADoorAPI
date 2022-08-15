IF (OBJECT_ID('FOX_PROC_GET_PS_CALL_LOG_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PS_CALL_LOG_LIST  
GO     
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
-- =============================================      
-- AUTHOR:  <DEVELOPER, YOUSAF>      
-- CREATE DATE: <CREATE DATE, 13/08/2018>      
-- DESCRIPTION: <GET PATIENT SURVEY CALL LOG LIST>      
    
-- [dbo].[FOX_PROC_GET_PS_CALL_LOG_LIST] 1012714, 101271453414910, 24631      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PS_CALL_LOG_LIST] --1011163, 101116354446587, 112233445      
 @PRACTICE_CODE BIGINT      
 ,@SURVEY_ID BIGINT      
 ,@PATIENT_ACCOUNT BIGINT      
AS      
BEGIN      
 SELECT CL.SURVEY_CALL_ID      
  ,CL.PRACTICE_CODE      
  ,CL.ACU_CALL_ID      
  ,CL.SURVEY_ID      
  ,CL.PATIENT_ACCOUNT    
  ,CL.FILE_NAME      
  ,CL.IS_RECEIVED      
  ,CASE       
   WHEN CL.CALL_OUT_COME IS NULL      
    THEN 'NO ANSWER'      
   WHEN CL.CALL_OUT_COME = 'ANSWER'      
    THEN 'ANSWERED'      
   ELSE CL.CALL_OUT_COME      
   END AS CALL_OUT_COME      
  ,CL.CALL_DURATION      
  ,CASE       
   WHEN CL.IS_TO_PATIENT = 1      
    THEN 'PATIENT'      
   WHEN CL.IS_TO_PATIENT = 0      
    THEN 'RESPONSIBLE PARTY'      
   WHEN CL.IS_TO_PATIENT IS NULL      
    THEN ''      
   END AS CALL_TO      
  ,(AU.LAST_NAME + ', ' + AU.FIRST_NAME) AS CALL_BY    
  ,CL.CALL_DURATION    
  ,CL.CREATED_BY    
  ,CL.MODIFIED_BY      
  ,CL.MODIFIED_DATE        
  ,CL.CREATED_DATE    
  ,PS.SERVICE_OR_PAYMENT_DESCRIPTION    
 FROM FOX_TBL_PATIENT_SURVEY_CALL_LOG CL      
 LEFT JOIN FOX_TBL_APPLICATION_USER AU ON CL.CREATED_BY = AU.USER_NAME    
 INNER JOIN FOX_TBL_PATIENT_SURVEY PS ON CL.SURVEY_ID = PS.SURVEY_ID      
 WHERE ISNULL(CL.DELETED, 0) = 0      
  AND CL.PRACTICE_CODE = @PRACTICE_CODE      
  AND CL.PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
END      
