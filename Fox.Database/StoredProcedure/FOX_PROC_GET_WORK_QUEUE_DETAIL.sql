-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                                
-- CREATE DATE: <CREATE DATE, 03/07/2022>                                                                                                
-- DESCRIPTION: <GET LIST OF WORK QUEUE DETAILS>                                                                                          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_DETAIL]                                                                                                             
(                                                                                                               
  @WORK_ID VARCHAR(50),                      
  @PRACTICE_CODE BIGINT                                                                                                                                                       
)                                                                                                                  
AS                                                                                                                  
BEGIN                                                      
SELECT * FROM FOX_TBL_WORK_QUEUE WITH (NOLOCK) WHERE WORK_ID = @WORK_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED ,0) = 0                            
END 