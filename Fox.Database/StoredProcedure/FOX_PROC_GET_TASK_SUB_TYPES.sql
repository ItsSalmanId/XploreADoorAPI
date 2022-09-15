IF (OBJECT_ID('FOX_PROC_GET_TASK_SUB_TYPES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_SUB_TYPES  
GO 
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/27/2019>        
-- Description: <Description,,>        
-- =============================================          
CREATE PROCEDURE FOX_PROC_GET_TASK_SUB_TYPES      
 @PRACTICE_CODE BIGINT,    
 @TASK_TYPE_ID INT    
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
  SELECT * FROM FOX_TBL_TASK_SUB_TYPE         
  WHERE ISNULL(DELETED,0) = 0      
  AND PRACTICE_CODE = @PRACTICE_CODE    
  AND TASK_TYPE_ID = @TASK_TYPE_ID       
      
END     
