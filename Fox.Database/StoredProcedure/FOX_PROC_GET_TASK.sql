IF (OBJECT_ID('FOX_PROC_GET_TASK') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK  
GO   
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/27/2019>        
-- Description: <Description,,>        
-- =============================================        
    
CREATE PROCEDURE FOX_PROC_GET_TASK    
 @PRACTICE_CODE BIGINT,    
 @TASK_ID BIGINT    
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
   
  SELECT TOP 1 * FROM FOX_TBL_TASK         
  WHERE ISNULL(DELETED,0) = 0      
  AND PRACTICE_CODE = @PRACTICE_CODE    
  AND TASK_ID = @TASK_ID    
END     
