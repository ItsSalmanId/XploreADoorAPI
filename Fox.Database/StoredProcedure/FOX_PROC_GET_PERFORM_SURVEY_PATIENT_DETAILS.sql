-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                      
-- CREATE DATE: <CREATE DATE, 12/15/2022>                                                                                      
-- DESCRIPTION: <GET LIST OF PATIENT DETAILS>                                                                                
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PERFORM_SURVEY_PATIENT_DETAILS]                                                                                                   
(                                                                                                     
  @PATIENT_ACCOUNT BIGINT,            
  @PRACTICE_CODE BIGINT,      
  @SURVEY_ID BIGINT                                                                                                                                          
)                                                                                                        
AS                                                                                                        
BEGIN                                            
 DECLARE @FROMDATE DATETIME                             
 DECLARE @TODATE DATETIME                                
 SET @FROMDATE = GETDATE() - 21                                
 SET @TODATE = GETDATE()                   
       
Select PATIENT_ACCOUNT From FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL                            
JOIN FOX_TBL_Patient_SURVEY PS  WITH (NOLOCK) on PS.PATIENT_ACCOUNT_NUMBER = SL.PATIENT_ACCOUNT AND PS.SURVEY_ID = SL.SURVEY_ID                   
where                         
SL.CREATED_DATE BETWEEN @FROMDATE  AND @TODATE                         
AND PS.PATIENT_ACCOUNT_NUMBER =  @PATIENT_ACCOUNT      
AND PS.SURVEY_ID =  @SURVEY_ID                                       
AND PS.IS_SURVEYED = 1                                         
AND ISNULL(PS.DELETED, 0) = 0             
AND SL.PRACTICE_CODE = @PRACTICE_CODE            
AND PS.PRACTICE_CODE = @PRACTICE_CODE                                
END 