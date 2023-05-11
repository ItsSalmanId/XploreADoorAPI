-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                            
-- CREATE DATE: <CREATE DATE, 03/07/2022>                                                                                            
-- DESCRIPTION: <GET LIST OF WORK QUEUE DETAILS>  
-- =============================================           
-- FOX_PROC_GET_WORK_QUEUE_DETAILS '816631', 1012714                                                                                       
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_DETAILS]                                                                                                         
(                                                                                                           
  @UNIQUE_ID VARCHAR(50),                  
  @PRACTICE_CODE BIGINT                                                                                                                                                   
)                                                                                                              
AS                                                                                                              
BEGIN                                                  
Select * From FOX_TBL_WORK_QUEUE WITH (NOLOCK) where UNIQUE_ID = @UNIQUE_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED ,0) = 0                        
END 