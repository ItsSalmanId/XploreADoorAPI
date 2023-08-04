-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                                
-- CREATE DATE: <CREATE DATE, 05/01/2023>                                                                                                
-- DESCRIPTION: <GET First OF WORK QUEUE FILE ALL DETAILS>                                                                                          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_FILE_ALL_RECORD]                                                                                                             
(                                                                                                               
  @WORK_ID BIGINT                                                                                                                                                                       
)                                                                                                                  
AS                                                                                                                  
BEGIN                                                      
select top 1 * From FOX_TBL_WORK_QUEUE WITH (NOLOCK) where WORK_ID = @WORK_ID  AND ISNULL(DELETED ,0) = 0                        
END 
