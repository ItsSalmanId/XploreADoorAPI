IF (OBJECT_ID('FOX_PROC_INSERT_TASK') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_TASK  
GO    
-- =============================================                    
-- Author:  <Muhammad Imran>                    
-- Create date: <09/26/2019>                    
-- Description: <Description,,>                    
-- =============================================                         
CREATE PROCEDURE FOX_PROC_INSERT_TASK        
 @ID BIGINT,        
 @PRACTICE_CODE BIGINT,                  
 @PATIENT_ACCOUNT VARCHAR(100),                
 @IS_COMPLETED_INT INT,                
 @USER_NAME VARCHAR(70),                
 @IS_TEMPLATE BIT,                
 @TASK_TYPE_ID INT,                
 @SEND_TO_ID BIGINT,                
 @FINAL_ROUTE_ID BIGINT,                
 @PRIORITY VARCHAR(20),                
 @DUE_DATE_TIME DATETIME,                
 @CATEGORY_ID INT,                
 @IS_REQ_SIGNOFF BIT,                
 @IS_SENDING_ROUTE_DETAILS BIT,                
 @SEND_CONTEXT_ID BIGINT,                
 @CONTEXT_INFO VARCHAR(200),                
 @DEVELIVERY_ID BIGINT,                
 @DESTINATIONS VARCHAR(200),                
 @LOC_ID BIGINT,                
 @PROVIDER_ID BIGINT,                
 @IS_SEND_EMAIL_AUTO BIT,                
 @DELETED BIT,                
 @IS_SEND_TO_USER BIT,                
 @IS_FINAL_ROUTE_USER BIT,                
 @IS_FINALROUTE_MARK_COMPLETE BIT,                
 @IS_SENDTO_MARK_COMPLETE BIT                
                
AS                    
BEGIN                    
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                
 -- SET NOCOUNT ON added to prevent extra result sets from                    
 -- interfering with SELECT statements.                    
 --DECLARE @ID BIGINT                
 --EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'FOX_TASK_ID', @ID output                
-- select * from FOX_TBL_TASK where TASK_ID = @ID                
 INSERT INTO FOX_TBL_TASK (                
         TASK_ID,                
         PRACTICE_CODE,                  
         PATIENT_ACCOUNT,                
         IS_COMPLETED_INT,                         
         IS_TEMPLATE,                
         TASK_TYPE_ID,                
         SEND_TO_ID,                
         FINAL_ROUTE_ID,                
         PRIORITY,                
         DUE_DATE_TIME,                
         CATEGORY_ID,                
         IS_REQ_SIGNOFF,                
         IS_SENDING_ROUTE_DETAILS,                
         SEND_CONTEXT_ID,                
         CONTEXT_INFO,                
         DEVELIVERY_ID ,                
         DESTINATIONS ,                
         LOC_ID ,                
         PROVIDER_ID ,                
         IS_SEND_EMAIL_AUTO ,                
          DELETED ,                
         IS_SEND_TO_USER ,                
         IS_FINAL_ROUTE_USER ,                
         IS_FINALROUTE_MARK_COMPLETE ,                
         IS_SENDTO_MARK_COMPLETE,                
         CREATED_BY,                
         CREATED_DATE,                
         MODIFIED_BY,                
         MODIFIED_DATE)                
       VALUES (                
         @ID,                
         @PRACTICE_CODE,                  
         @PATIENT_ACCOUNT,                
         @IS_COMPLETED_INT,                         
         @IS_TEMPLATE,                
         @TASK_TYPE_ID,                
         @SEND_TO_ID,                
         @FINAL_ROUTE_ID,                
         @PRIORITY,                
         @DUE_DATE_TIME,                
         @CATEGORY_ID,                
         @IS_REQ_SIGNOFF,                
         @IS_SENDING_ROUTE_DETAILS,                
         @SEND_CONTEXT_ID,                
         @CONTEXT_INFO,                
         @DEVELIVERY_ID ,                
         @DESTINATIONS ,                
         @LOC_ID ,       
         @PROVIDER_ID ,                
         @IS_SEND_EMAIL_AUTO ,                
         @DELETED ,                
         @IS_SEND_TO_USER ,                
         @IS_FINAL_ROUTE_USER ,                
         @IS_FINALROUTE_MARK_COMPLETE ,                
         @IS_SENDTO_MARK_COMPLETE,                
         @USER_NAME,                
         GETDATE(),                
   @USER_NAME,                
         GETDATE()                
                        
       )                
                
  SELECT * FROM FOX_TBL_TASK WHERE TASK_ID = @ID                
END           
  
