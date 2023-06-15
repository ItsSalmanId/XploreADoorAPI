-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                              
-- CREATE DATE: <CREATE DATE, 05/01/2023>                                                                                              
-- DESCRIPTION: <GET LIST OF WORK QUEUE FILE ALL DETAILS>   
-- =============================================           
-- FOX_PROC_GET_WORK_QUEUE_FILE_ALL_DETAILS '816631'                                                                                      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_FILE_ALL_DETAILS]                                                                                                           
(                                                                                                             
  @UNIQUE_ID VARCHAR(50)                                                                                                                                                                    
)                                                                                                                
AS                                                                                                                
BEGIN                                                    
Select * From FOX_TBL_WORK_QUEUE_File_All  WITH (NOLOCK) where UNIQUE_ID = @UNIQUE_ID  AND ISNULL(DELETED ,0) = 0                          
END 