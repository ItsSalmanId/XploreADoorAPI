-- =============================================                        
-- Author:  <Abdul Sattar>                        
-- Create date: <08/01/2023>                        
-- Description: <Description,,>                        
-- =============================================                        
                        
CREATE PROCEDURE FOX_PROC_INSERT_SINGLE_WORK_ORDER_HISTORY                       
 -- Add the parameters for the stored procedure here                        
 (@MESSAGE VARCHAR(500),            
 @WORK_ID BIGINT,          
 @ID BIGINT,                  
 @UNIQUE_ID VARCHAR(50),                    
 @USER_NAME VARCHAR(70)   
 )                   
AS                         
BEGIN                    
      
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED  
INSERT INTO FOX_TBL_WORK_ORDER_HISTORY (LOG_ID, WORK_ID, UNIQUE_ID, LOG_MESSAGE, CREATED_BY, CREATED_ON)                     
       VALUES (@ID, @WORK_ID, @UNIQUE_ID, @MESSAGE, @USER_NAME, GETDATE())             
                    
END 