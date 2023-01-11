-- AUTHOR:  <MUHAMMAD SALMAN>                                                                    
-- CREATE DATE: <CREATE DATE, 12/15/2022>                                                                    
-- DESCRIPTION: <GET LIST OF PATIENT SURVEY QUESTIONS DETAILS>                                                              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_SURVEY_QUESTION]                                                                                 
(                                                                                   
  @PATIENT_ACCOUNT BIGINT,      
  @PRACTICE_CODE BIGINT                                                                                                               
)                                                                                      
AS                                                                                      
BEGIN             
    DECLARE @SURVEY_FORMAT_TYPEE VARCHAR(MAX)          
      SET @SURVEY_FORMAT_TYPEE  = (SELECT TOP 1 SURVEY_FORMAT_TYPE FROM FOX_TBL_PATIENT_SURVEY  WITH (NOLOCK) WHERE PATIENT_ACCOUNT_NUMBER = @PATIENT_ACCOUNT AND ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE)            
    IF(@SURVEY_FORMAT_TYPEE = 'New Format')            
      SELECT * FROM FOX_TBL_SURVEY_QUESTION  WITH (NOLOCK) where ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE             
END 