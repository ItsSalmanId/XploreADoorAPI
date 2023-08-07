-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                                  
-- CREATE DATE: <CREATE DATE, 05/01/2023>                                                                                                  
-- DESCRIPTION: <GET LIST OF WORK QUEUE FILE ALL DETAILS>                                                                                            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_FILE_ALL_LIST]                                                                                                               
(                                                                                                                 
  @WORK_ID BIGINT                                                                                                                                                                        
)                                                                                                                    
AS                                                                                                                    
BEGIN         
DECLARE @Count VARCHAR (MAX);      
set @Count =   CAST((select Count(*) From FOX_TBL_WORK_QUEUE_File_All WITH (NOLOCK) where WORK_ID = @WORK_ID  AND ISNULL(DELETED ,0) = 0) AS VARCHAR (MAX))    
select  @Count                                                              
END 