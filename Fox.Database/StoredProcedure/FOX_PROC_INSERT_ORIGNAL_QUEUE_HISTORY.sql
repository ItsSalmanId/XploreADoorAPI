IF (OBJECT_ID('FOX_PROC_INSERT_ORIGNAL_QUEUE_HISTORY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_ORIGNAL_QUEUE_HISTORY  
GO   
-- =============================================            
-- Author:  <Muhammad Imran>            
-- Create date: <09/30/2019>            
-- Description: <Description,,>            
-- =============================================            
-- =============================================                    
-- Author:  <Muhammad Imran>                    
-- Create date: <09/30/2019>                    
-- Description: <Description,,>                    
-- =============================================                    
                    
CREATE PROCEDURE FOX_PROC_INSERT_ORIGNAL_QUEUE_HISTORY                   
 -- Add the parameters for the stored procedure here                    
 @ORIGNAL_QUEUE_HISTORY ORIGNAL_QUEUE_HISTORY READONLY,        
 @WORK_ID BIGINT,      
 @ID BIGINT,              
 @UNIQUE_ID VARCHAR(50),                
 @USER_NAME VARCHAR(70)                
AS                     
BEGIN                
  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED          
 --DECLARE @ID BIGINT                
 DECLARE @MSG VARCHAR(500)                
                
 DECLARE @TEMP TABLE(LOG_ID BIGINT, MSG VARCHAR(500))                
                
                
                
                 
DECLARE ORIGNAL_QUEUE_HISTORY CURSOR FOR                
  SELECT MSG                
  FROM   @ORIGNAL_QUEUE_HISTORY                
                
OPEN ORIGNAL_QUEUE_HISTORY;                
                
FETCH NEXT FROM ORIGNAL_QUEUE_HISTORY INTO @MSG                
                
WHILE @@FETCH_STATUS = 0                
  BEGIN                
 --EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'LOG_ID', @ID output                 
     INSERT INTO @TEMP (LOG_ID, MSG)                
  VALUES (@ID, @MSG)                
                
      --SELECT @empno = @empno + 1;                
                
      FETCH NEXT FROM ORIGNAL_QUEUE_HISTORY INTO @MSG                
  END;                
                
CLOSE ORIGNAL_QUEUE_HISTORY;                
                
DEALLOCATE ORIGNAL_QUEUE_HISTORY;                
                
                
INSERT INTO FOX_TBL_WORK_ORDER_HISTORY(LOG_ID, WORK_ID, UNIQUE_ID, LOG_MESSAGE, CREATED_BY, CREATED_ON)                 
       SELECT LOG_ID, @WORK_ID, @UNIQUE_ID, MSG, @USER_NAME, GETDATE() FROM @TEMP                
                
END 