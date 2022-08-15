IF (OBJECT_ID('FOX_PROC_GET_TASK_TYPES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_TYPES  
GO   
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/27/2019>        
-- Description: <Description,,>        
-- =============================================           
CREATE PROCEDURE FOX_PROC_GET_TASK_TYPES    
  @PRACTICE_CODE BIGINT    
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
  SELECT * FROM FOX_TBL_TASK_TYPE         
  WHERE ISNULL(DELETED,0) = 0      
  AND PRACTICE_CODE = @PRACTICE_CODE      
END     
