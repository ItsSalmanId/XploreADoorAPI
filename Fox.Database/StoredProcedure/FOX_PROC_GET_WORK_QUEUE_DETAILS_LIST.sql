-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                              
-- CREATE DATE: <CREATE DATE, 05/01/2023>                                                                                              
-- DESCRIPTION: <GET LIST OF WORK QUEUE DETAILS> 
=============================================           
-- FOX_PROC_GET_WORK_QUEUE_DETAILS_LIST '816631', 1012714                                                                                                                                                                              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_DETAILS_LIST]                                                                                                           
(                                                                                                             
  @UNIQUE_ID VARCHAR(50),                    
  @PRACTICE_CODE BIGINT                                                                                                                                                     
)                                                                                                                
AS                                                                                                                
BEGIN                                                    
select count(*) From FOX_TBL_WORK_QUEUE WITH (NOLOCK) where UNIQUE_ID = @UNIQUE_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED ,0) = 0                          
END 