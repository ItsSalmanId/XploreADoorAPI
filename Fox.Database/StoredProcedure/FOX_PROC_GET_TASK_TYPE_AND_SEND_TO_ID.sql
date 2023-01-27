-- =============================================                                      
-- Author:  <Abdur Rafay>                                      
-- Create date: <12/27/2021>                                      
-- Description: <GET TASK TYPE AND SEND_TO_ID>                                      
-- =============================================          
-- EXEC   FOX_PROC_GET_TASK_TYPE_AND_SEND_TO_ID 1012714        
        
CREATE PROCEDURE FOX_PROC_GET_TASK_TYPE_AND_SEND_TO_ID                     
 @PRACTICE_CODE BIGINT        
 AS         
 BEGIN        
        
 DECLARE @GROUP_ID BIGINT,        
  @TASK_TYPE_ID BIGINT         
        
 SELECT @GROUP_ID = GROUP_ID         
 FROM FOX_TBL_GROUP        
 WHERE PRACTICE_CODE = @PRACTICE_CODE       
 AND GROUP_NAME = '03TL'        
 AND ISNULL(DELETED, 0) = 0      
        
        
SELECT @TASK_TYPE_ID = TASK_TYPE_ID        
FROM FOX_TBL_TASK_TYPE         
WHERE PRACTICE_CODE = @PRACTICE_CODE        
AND LOWER(NAME) = LOWER('Billing Review Required')        
        
 SELECT @TASK_TYPE_ID AS TASK_TYPE_ID, @GROUP_ID AS SEND_TO_ID        
        
 END     
    