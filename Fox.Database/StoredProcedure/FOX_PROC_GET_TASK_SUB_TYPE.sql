IF (OBJECT_ID('FOX_PROC_GET_TASK_SUB_TYPE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_SUB_TYPE  
GO   
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================         
CREATE PROCEDURE FOX_PROC_GET_TASK_SUB_TYPE    
 @PRACTICE_CODE BIGINT,     
 @TASK_TYPE_ID BIGINT,    
 @NAME VARCHAR(200)    
     
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from            
 SELECT TOP 1 * FROM FOX_TBL_TASK_SUB_TYPE    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
  AND ISNULL(DELETED,0) = 0    
  AND TASK_TYPE_ID = @TASK_TYPE_ID    
  AND LOWER(NAME) = LOWER(@NAME)    
END   
