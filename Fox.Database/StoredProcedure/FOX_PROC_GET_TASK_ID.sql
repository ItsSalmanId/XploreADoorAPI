IF (OBJECT_ID('FOX_PROC_GET_TASK_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_ID  
GO   
-- =============================================          
-- Author:  <Muhammad Imran>          
-- Create date: <09/26/2019>          
-- Description: <Description,,>          
-- =============================================                
CREATE PROCEDURE FOX_PROC_GET_TASK_ID          
 @PRACTICE_CODE BIGINT,        
 @NAME VARCHAR(100)        
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from            
 SET NOCOUNT ON;          
 SELECT TOP 1 * FROM FOX_TBL_TASK_TYPE           
 WHERE PRACTICE_CODE = @PRACTICE_CODE          
 AND LOWER(RT_CODE) = LOWER(@NAME)          
 AND ISNULL(DELETED,0) = 0          
END   
