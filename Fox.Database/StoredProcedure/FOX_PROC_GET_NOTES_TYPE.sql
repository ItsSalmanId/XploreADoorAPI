-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                              
-- CREATE DATE: <CREATE DATE, 05/01/2023>                                                                                              
-- DESCRIPTION: <GET LIST OF NOTES TYPE DETAILS>   
-- =============================================           
-- FOX_PROC_GET_NOTES_TYPE 'Notes', 1012714  
CREATE PROCEDURE [DBO].[FOX_PROC_GET_NOTES_TYPE]     
  @NOTES_NAME VARCHAR(MAX),                
  @PRACTICE_CODE BIGINT                                                   
AS                      
BEGIN                                          
  
Select NOTES_TYPE_ID From FOX_TBL_NOtes_Type WITH (NOLOCK) where  PRACTICE_CODE = @PRACTICE_CODE                  
    AND ISNULL(DELETED,0)= 0 AND NAME = @NOTES_NAME         
END 