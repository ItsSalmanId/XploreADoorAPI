IF (OBJECT_ID('FOX_PROC_INSERT_TASK_LOG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_TASK_LOG  
GO 
-- =============================================          
-- Author:  <Muhammad Imran>          
-- Create date: <09/26/2019>          
-- Description: <Description,,>          
-- =============================================               
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_TASK_LOG]        
 @ID BIGINT,        
 @PRACTICE_CODE BIGINT,            
 @TASK_ID BIGINT,            
 @TASK_LOG dbo.TASK_LOG_HISTORY READONLY,             
 @USER_NAME VARCHAR(70)            
            
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
 --DECLARE @ID BIGINT           
 DECLARE @ACTION VARCHAR(100)          
 DECLARE @ACTION_DETAIL VARCHAR(MAX)          
          
 DECLARE @TEMP TABLE (LOG_ID BIGINT, ACTION VARCHAR(100), ACTION_DETAIL VARCHAR(MAX))          
          
 DECLARE TASK_LOG_CURSOR CURSOR FOR          
 SELECT ACTION, ACTION_DETAIL           
 FROM @TASK_LOG          
          
          
 OPEN TASK_LOG_CURSOR;          
          
 FETCH NEXT FROM TASK_LOG_CURSOR INTO @ACTION, @ACTION_DETAIL          
          
WHILE @@FETCH_STATUS = 0          
  BEGIN          
 --EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'FOX_TASK_LOG_ID', @ID output           
     INSERT INTO @TEMP (LOG_ID, ACTION, ACTION_DETAIL)          
  VALUES (@ID, @ACTION, @ACTION_DETAIL)          
          
      --SELECT @empno = @empno + 1;          
          
      FETCH NEXT FROM TASK_LOG_CURSOR INTO @ACTION, @ACTION_DETAIL          
  END;          
          
CLOSE TASK_LOG_CURSOR;          
          
DEALLOCATE TASK_LOG_CURSOR;           
          
            
 INSERT FOX_TBL_TASK_LOG (TASK_LOG_ID, PRACTICE_CODE, TASK_ID, ACTION, ACTION_DETAIL, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED)          
 SELECT LOG_ID, @PRACTICE_CODE, @TASK_ID, ACTION, ACTION_DETAIL, @USER_NAME, GETDATE(), @USER_NAME, GETDATE(), 0 FROM @TEMP          
          
END 

