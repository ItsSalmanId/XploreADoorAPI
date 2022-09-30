IF (OBJECT_ID('FOX_PROC_GET_TASK_TASK_SUB_TYPE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_TASK_SUB_TYPE  
GO   
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================           
CREATE PROCEDURE FOX_PROC_GET_TASK_TASK_SUB_TYPE    
 @TASK_ID BIGINT,    
 @PRACTICE_CODE BIGINT,     
 @TASK_SUB_TYPE_ID INT       
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from         
 SELECT TOP 1 * FROM FOX_TBL_TASK_TASK_SUB_TYPE    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
  AND ISNULL(DELETED,0) = 0    
  AND TASK_ID = @TASK_ID    
  AND TASK_SUB_TYPE_ID = @TASK_SUB_TYPE_ID    
      
END   
	