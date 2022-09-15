IF (OBJECT_ID('FOX_PROC_INSERT_INTERFACE_DATA') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_INTERFACE_DATA  
GO 
-- =============================================              
-- Author:  <Muhammad Imran>              
-- Create date: <09/26/2019>              
-- Description: <Description,,>              
-- =============================================                  
CREATE PROCEDURE FOX_PROC_INSERT_INTERFACE_DATA    
 @ID BIGINT,       
 @PRACTICE_CODE BIGINT,               
 @CASE_ID BIGINT,              
 @Work_ID BIGINT,              
 @TASK_ID BIGINT,              
 @PATIENT_ACCOUNT BIGINT,              
 @USER_NAME VARCHAR(70),              
 @IS_SYNCED BIT,              
 @APPLICATION VARCHAR(50)          
       
AS                  
BEGIN                  
 -- SET NOCOUNT ON added to prevent extra result sets from                  
 -- interfering with SELECT statements.                  
 -- DECLARE @ID BIGINT              
 -- EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'FOX_INTERFACE_SYNCH_ID', @ID output      
       
 INSERT INTO FOX_TBL_INTERFACE_SYNCH(FOX_INTERFACE_SYNCH_ID, PRACTICE_CODE, PATIENT_ACCOUNT, CASE_ID, TASK_ID, IS_SYNCED, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED, Work_ID,APPLICATION)              
        VALUES(@ID, @PRACTICE_CODE, @PATIENT_ACCOUNT, @CASE_ID, @TASK_ID, 0, @USER_NAME, GETDATE(), @USER_NAME, GETDATE(), 0, @Work_ID,@APPLICATION)              
            
  Select * from FOX_TBL_INTERFACE_SYNCH where FOX_INTERFACE_SYNCH_ID=@ID            
END    
    
