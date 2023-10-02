 -- =============================================                  
-- Author:  <Muhammad Salman>                  
-- Create date: <09/26/2>                  
-- Description: <Description,,>       
-- [dbo].[FOX_PROC_UPDATE_TASK_LOG] 1012714, 15785185, '','','','',''            
-- =============================================    
CREATE PROCEDURE FOX_PROC_UPDATE_TASK_LOG                          
 @PRACTICE_CODE BIGINT,                          
 @TASK_ID BIGINT,                                
 @USER_NAME VARCHAR(70),                
 @ACTION_DETAIL VARCHAR(MAX),                
 @ACTION VARCHAR(MAX)                                
AS                            
BEGIN                  
  DECLARE @TAB_TASK_LOG TABLE (MAX_TASK_LOG_ID BIGINT)                  
DECLARE @MAXID BIGINT                  
INSERT INTO @TAB_TASK_LOG EXEC Web_GetMaxColumnID 'FOX_TASK_LOG_ID'                  
SET @MAXID = (SELECT TOP 1 MAX_TASK_LOG_ID FROM @TAB_TASK_LOG)                  
INSERT FOX_TBL_TASK_LOG (TASK_LOG_ID, PRACTICE_CODE, TASK_ID, ACTION, ACTION_DETAIL, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED)                
VALUES (@MAXID, @PRACTICE_CODE, @TASK_ID, @ACTION, @ACTION_DETAIL, @USER_NAME, GETDATE(), @USER_NAME, GETDATE(), 0)          
                                 
END 