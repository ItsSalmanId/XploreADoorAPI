-- AUTHOR:  <MUHAMMAD SALMAN>                                                                    
-- CREATE DATE: <CREATE DATE, 12/15/2022>                                                                    
-- DESCRIPTION: <GET LIST OF PATIENT DETAILS>                                                              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SURVEY_PATIENT_DETAILS]                                                                                 
(                                                                                   
  @PATIENT_ACCOUNT BIGINT,      
   @PRACTICE_CODE BIGINT,    
   @SURVEY_ID BIGINT                                                                                                                  
)                                                                                      
AS                                                                                      
BEGIN           
      
DECLARE @SURVEY_FILE_NAME VARCHAR(MAX)        
        
 SET @SURVEY_FILE_NAME = (SELECT TOP 1 FILE_NAME FROM FOX_TBL_PATIENT_SURVEY WITH (NOLOCK) WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 ORDER BY CREATED_DATE DESC)        
                        
SELECT PROVIDER, REGION,PT_OT_SLP,PS.SURVEY_ID,CAST(PS.PATIENT_ACCOUNT_NUMBER AS varchar) AS PATIENT_ACCOUNT FROM FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)      
 INNER JOIN PATIENT AS P WITH (NOLOCK) ON PS.PATIENT_ACCOUNT_NUMBER = + '00' + P.CHART_ID AND P.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(P.DELETED, 0) = 0        
WHERE     
PATIENT_ACCOUNT_NUMBER = @PATIENT_ACCOUNT     
AND PS.SURVEY_ID = @SURVEY_ID     
AND ISNULL(PS.DELETED, 0) = 0        
AND PS.FILE_NAME = @SURVEY_FILE_NAME        
                        
END