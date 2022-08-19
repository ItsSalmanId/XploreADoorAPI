IF (OBJECT_ID('FOX_PROC_INSERT_TASK_TASK_SUB_TYPE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_TASK_TASK_SUB_TYPE  
GO   
-- =============================================              
-- Author:  <Muhammad Imran>              
-- Create date: <09/26/2019>              
-- Description: <Description,,>              
-- =============================================               
CREATE PROCEDURE FOX_PROC_INSERT_TASK_TASK_SUB_TYPE        
 @ID BIGINT,      
 @TASK_ID BIGINT,        
 @TASK_SUB_TYPE_ID INT,        
 @PRACTICE_CODE BIGINT,        
 @USER_NAME VARCHAR(70)        
           
AS              
BEGIN              
         
 IF NOT EXISTS (SELECT * FROM FOX_TBL_TASK_TASK_SUB_TYPE WHERE TASK_ID = @TASK_ID AND TASK_SUB_TYPE_ID = @TASK_SUB_TYPE_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0) = 0)        
 BEGIN        
  --DECLARE @ID BIGINT        
  --EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'FOX_TBL_TASK_TASK_SUB_TYPE_ID', @ID output          
          
  INSERT INTO FOX_TBL_TASK_TASK_SUB_TYPE(TASK_TASK_SUB_TYPE_ID, PRACTICE_CODE, TASK_ID, TASK_SUB_TYPE_ID, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED)        
         VALUES (@ID, @PRACTICE_CODE, @TASK_ID, @TASK_SUB_TYPE_ID, @USER_NAME, GETDATE(), @USER_NAME, GETDATE(), 0)        
 END        
        
END   
  
