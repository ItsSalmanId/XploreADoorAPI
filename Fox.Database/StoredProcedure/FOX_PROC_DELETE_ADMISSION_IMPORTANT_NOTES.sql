-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                              
-- CREATE DATE: <CREATE DATE, 05/01/2023>                                                                                              
-- DESCRIPTION: <GET LIST OF NOTES DETAILS>   
-- =============================================           
-- FOX_PROC_DELETE_ADMISSION_IMPORTANT_NOTES 1012714,5265454  
CREATE PROCEDURE FOX_PROC_DELETE_ADMISSION_IMPORTANT_NOTES        
 @PRACTICE_CODE BIGINT,                               
 @Work_ID BIGINT                        
AS                      
BEGIN             
UPDATE FOX_TBL_NOTES set DELETED = 1 , MODIFIED_DATE = GETDATE() where Work_ID =  @Work_ID and PRACTICE_CODE = @PRACTICE_CODE                       
END 