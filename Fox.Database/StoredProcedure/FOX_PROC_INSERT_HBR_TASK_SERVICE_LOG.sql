-- =============================================                      
-- Author:  <Author,Abdur Rafey>                      
-- Create date: <Create Date,21/12/2021>                      
-- DESCRIPTION: <INSERT FOX HBR TASK SERVICE LOG>                      
                  
--   [dbo].[FOX_PROC_INSERT_HBR_TASK_SERVICE_LOG] 1011163, 1010,1010                
                  
CREATE PROCEDURE [FOX_PROC_INSERT_HBR_TASK_SERVICE_LOG]                  
(@PRACTICE_CODE BIGINT       
,@PATIENT_ACCOUNT BIGINT      
,@TASK_ID  BIGINT)                  
AS                  
BEGIN                  
select concat((SELECT office_id FROM MAINTENANCE),100)  
DECLARE @LOG_ID BIGINT                                      
SET @LOG_ID = (SELECT ISNULL(MAX(ADD_HBR_TASK_LOGS_ID),(select concat((SELECT office_id FROM MAINTENANCE),100))) FROM FOX_TBL_ADD_HBR_TASK_LOGS)         
        
INSERT INTO FOX_TBL_ADD_HBR_TASK_LOGS(        
ADD_HBR_TASK_LOGS_ID       
,PRACTICE_CODE      
,PATIENT_ACCOUNT        
,TASK_ID        
,CREATED_BY        
,CREATED_DATE        
,MODIFIED_BY        
,MODIFIED_DATE        
,DELETED        
)        
VALUES        
(        
@LOG_ID + 1        
,@PRACTICE_CODE      
,@PATIENT_ACCOUNT       
,@TASK_ID      
,'FOX HBR TASK SERVICE'        
,GETDATE()        
,'FOX HBR TASK SERVICE'        
,GETDATE()        
,0        
);        
        
END;    
    