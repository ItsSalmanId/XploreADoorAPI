-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                              
-- CREATE DATE: <CREATE DATE, 05/17/2023>                                                                                              
-- DESCRIPTION: <GET LIST OF ADMISSION NOTES DETAILS> 
---=============================================           
-- FOX_PROC_GET_ADMISSION_NOTES_DETAILS '816631', 1012714                                                                                                                                                                              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ADMISSION_NOTES_DETAILS]                                                                                                           
(                                                                                                             
  @WORK_ID BIGINT,                    
  @PRACTICE_CODE BIGINT                                                                                                                                                     
)                                                                                                                
AS                                                                                                                
BEGIN                                                    
select * From FOX_TBL_NOTES WITH (NOLOCK) where Work_ID = 99919126  AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED ,0) = 0                          
END 