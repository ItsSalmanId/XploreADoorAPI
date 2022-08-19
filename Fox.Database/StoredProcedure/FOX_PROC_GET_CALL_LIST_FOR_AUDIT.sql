IF (OBJECT_ID('FOX_PROC_GET_CALL_LIST_FOR_AUDIT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_CALL_LIST_FOR_AUDIT 
GO
-- =============================================                                
-- AUTHOR:  <DEVELOPER, USAMA BIN AHMED>                                
-- CREATE DATE: <CREATE DATE, 01/04/2020>                                
-- DESCRIPTION: <GET ALL CALL LIST FOR AUDIT>                  
          
-- EXEC [FOX_PROC_GET_CALL_LIST_FOR_AUDIT] 1012714,'Admin_5651352','survey',NULL,NULL,0                                   
                                     
                                
CREATE PROCEDURE [DBO].[FOX_PROC_GET_CALL_LIST_FOR_AUDIT]                                 
  @PRACTICE_CODE BIGINT                                
 ,@CALL_BY VARCHAR(100)                                
 ,@CALL_TYPE VARCHAR(100)                                
 ,@DATE_FROM       DATETIME                                   
 ,@DATE_TO         DATETIME                              
 ,@PHD_CALL_SCENARIO_ID  VARCHAR(100)                               
                                 
AS                                
BEGIN                                
                                
                                
  IF(@CALL_TYPE = 'all')                                
  BEGIN                                
 ( SELECT PSL.SURVEY_CALL_ID AS ID                                
    ,PSL.MODIFIED_BY AS CREATED_BY                                
    ,PSL.CREATED_DATE                                
    ,FILE_NAME as [FILE_NAME]                                
    ,'Patient Survey' AS Logs                              
  ,                              
    '' AS NAME                              
 , P.Chart_Id AS MRN                              
 ,P.First_Name                              
 ,P.Last_Name                              
 , sas.SCORING_CRITERIA AS SCORING_CRITERIA                     
  FROM FOX_TBL_PATIENT_SURVEY_CALL_LOG AS PSL                              
  LEFT JOIN Patient AS P                              
  ON PSL.PATIENT_ACCOUNT = P.Patient_Account           
  left join FOX_TBL_SURVEY_AUDIT_SCORES as sas                
on PSL.SURVEY_CALL_ID = sas.SURVEY_CALL_ID           
 WHERE PSL.MODIFIED_BY = @CALL_BY            
  AND ISNULL(sas.DELETED, 0) = 0         
     AND ISNULL(PSL.DELETED, 0) = 0                                
  AND ISNULL(IS_RECEIVED, 0) = 1                                
  AND PSL.PRACTICE_CODE = @PRACTICE_CODE                                
  AND (@DATE_FROM IS NULL                                  
              OR @DATE_TO IS NULL                                  
              OR CONVERT(DATE, PSL.CREATED_DATE)                                 
     BETWEEN CONVERT(DATE, @DATE_FROM)                                 
     AND CONVERT(DATE, @DATE_TO))                                 
 UNION                                
  SELECT FOX_PHD_CALL_DETAILS_ID AS ID,                                
  C.CREATED_BY,                                
  C.CALL_DATE,                                
  C.CALL_RECORDING_PATH as [FILE_NAME]                                
 ,'Patient Helpdesk' AS Logs                                
 ,R.NAME AS NAME                              
 ,P.Chart_Id AS MRN                             
 ,P.First_Name                              
 ,P.Last_Name                
 , null AS SCORING_CRITERIA                                
 FROM FOX_TBL_PHD_CALL_DETAILS C                                
 left join FOX_TBL_PHD_CALL_REASON R on C.CALL_REASON = R.PHD_CALL_REASON_ID                           
 left join FOX_TBL_PHD_CALL_SCENARIO AS SR                              
 ON C.CALL_SCENARIO = SR.PHD_CALL_SCENARIO_ID                              
 LEFT JOIN Patient AS P ON C.PATIENT_ACCOUNT = P.Patient_Account                               
 WHERE C.CREATED_BY = @CALL_BY                                
  AND ISNULL(C.DELETED, 0) = 0                                
  AND ISNULL(CALL_RECORDING_PATH , '') <> ''                           
  AND C.PRACTICE_CODE = @PRACTICE_CODE                              
  AND  (@PHD_CALL_SCENARIO_ID = 0 OR SR.PHD_CALL_SCENARIO_ID  = @PHD_CALL_SCENARIO_ID)                           
  AND SR.DELETED = 0                              
  AND (@DATE_FROM IS NULL                                  
              OR @DATE_TO IS NULL                                  
              OR CONVERT(DATE, C.CREATED_DATE)                                 
     BETWEEN CONVERT(DATE, @DATE_FROM)                                 
     AND CONVERT(DATE, @DATE_TO)) )                
  END                                
                                
  ELSE IF(@CALL_TYPE = 'survey')                                
  BEGIN                                
 SELECT PSL.SURVEY_CALL_ID AS ID                    
    ,PSL.PATIENT_ACCOUNT                    
    ,PSL.MODIFIED_BY AS CREATED_BY                                
    ,PSL.CREATED_DATE                                
   ,PSL.FILE_NAME as [FILE_NAME]                                
    ,'Patient Survey' AS LOGS,                                
    '' AS NAME                              
 , convert(varchar ,Ps.PATIENT_ACCOUNT_NUMBER ) AS MRN                             
 ,Ps.PATIENT_FIRST_NAME AS FIRST_NAME                              
 ,Ps.PATIENT_LAST_NAME  AS LAST_NAME          
 , sas.SCORING_CRITERIA AS SCORING_CRITERIA       
 , ps.SURVEY_FLAG         
  FROM FOX_TBL_PATIENT_SURVEY_CALL_LOG AS PSL                              
  left join FOX_TBL_PATIENT_SURVEY as ps                
on ps.SURVEY_ID = psl.SURVEY_ID     -- ps.PATIENT_ACCOUNT_NUMBER = psl.PATIENT_ACCOUNT  AND          
left join FOX_TBL_SURVEY_AUDIT_SCORES as sas                
on PSL.SURVEY_CALL_ID = sas.SURVEY_CALL_ID           
              
 WHERE PSL.MODIFIED_BY = @CALL_BY                                
     AND ISNULL(PSL.DELETED, 0) = 0          
  AND ISNULL(sas.DELETED, 0) = 0          
  AND ISNULL(PSL.IS_RECEIVED, 0) = 1                                
  AND PSL.PRACTICE_CODE = @PRACTICE_CODE                                
  AND (@DATE_FROM IS NULL                                  
              OR @DATE_TO IS NULL                                  
              OR CONVERT(DATE, PSL.CREATED_DATE)                        
     BETWEEN CONVERT(DATE, @DATE_FROM)                                 
     AND CONVERT(DATE, @DATE_TO))                                 
 ORDER BY PSL.CREATED_DATE DESC                                
 END                                
                                
 ELSE IF(@CALL_TYPE = 'phd')                                
  BEGIN                          
                                  
  SELECT                          
--  CASE WHEN SCR.PHD_CALL_ID = 0                          
-- THEN SCR.SURVEY_AUDIT_SCORES_ID                          
-- ELSE                          
-- C.FOX_PHD_CALL_DETAILS_ID                          
-- END AS FOX_PHD_CALL_DETAILS_ID                          
----  CASE WHEN                          
--  C.FOX_PHD_CALL_DETAILS_ID = 000000000                          
-- THEN                           
--CONVERT(nvarchar(MAX), C.FOX_PHD_CALL_DETAILS_ID) +  CONVERT( nvarchar (MAX) ,C.CREATED_DATE )                           
-- ELSE CONVERT (nvarchar(MAX),C.FOX_PHD_CALL_DETAILS_ID )                          
-- END AS STRING_ID,                          
 C.FOX_PHD_CALL_DETAILS_ID AS ID                          
 ,c.PATIENT_ACCOUNT                        
                           
                                   
   ,C.CREATED_BY                                
   ,C.CREATED_DATE                                
  ,C.CALL_RECORDING_PATH as [FILE_NAME]                           
                                 
        ,'Patient Helpdesk' AS LOGS,                          
  CASE                           
   WHEN                           
   C.FOX_PHD_CALL_DETAILS_ID = 000000000                          
   THEN  'Patient Helpdesk| Not Associated Call'                           
   ELSE                          
   'Patient Helpdesk'                          
    END AS LOGS                            
                            
                       
  ,R.NAME AS NAME                                
  , P.Chart_Id AS MRN                             
  ,P.First_Name  AS FIRST_NAME                              
  ,P.Last_Name AS  LAST_NAME                    
  ,SR.PHD_CALL_SCENARIO_ID                    
  ,sr.NAME AS CALL_SCANARIO          
  , null AS SCORING_CRITERIA              
 FROM FOX_TBL_PHD_CALL_DETAILS C                                
 left join FOX_TBL_PHD_CALL_REASON R on C.CALL_REASON = R.PHD_CALL_REASON_ID                           
 left join FOX_TBL_PHD_CALL_SCENARIO AS SR                              
 ON C.CALL_SCENARIO = SR.PHD_CALL_SCENARIO_ID                              
  LEFT JOIN Patient AS P                               
 ON C.PATIENT_ACCOUNT = P.Patient_Account                          
 --left join FOX_TBL_SURVEY_AUDIT_SCORES AS SCR                          
 --ON SCR.PHD_CALL_ID = c.FOX_PHD_CALL_DETAILS_ID                                 
                              
 WHERE C.CREATED_BY = @CALL_BY                                
     AND ISNULL(C.DELETED, 0) = 0                        
  --AND ISNULL(R.DELETED,0) = 0 AND ISNULL(SR.DELETED,0) = 0                                
  AND ISNULL(C.CALL_RECORDING_PATH , '') <> ''                                
  AND C.PRACTICE_CODE = @PRACTICE_CODE                        AND (@PHD_CALL_SCENARIO_ID = 0 OR SR.PHD_CALL_SCENARIO_ID  = @PHD_CALL_SCENARIO_ID)                                
  AND (@DATE_FROM IS NULL                                  
              OR @DATE_TO IS NULL                                  
              OR CONVERT(DATE, C.CREATED_DATE)                                 
     BETWEEN CONVERT(DATE, @DATE_FROM)                                 
     AND CONVERT(DATE, @DATE_TO))                                 
                                
 ORDER BY CREATED_DATE DESC                                
  END                                
END 