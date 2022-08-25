IF (OBJECT_ID('FOX_PROC_GET_MSP_TASK_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_MSP_TASK_LIST  
GO   
-- =============================================    
-- Author:  <Muhammad Imran>    
-- Create date: <09/26/2019>    
-- Description: <Description,,>    
-- =============================================        
CREATE PROCEDURE FOX_PROC_GET_MSP_TASK_LIST    
 @TASK_TYPE_ID INT,    
 @PATIENT_ACCOUNT BIGINT,    
 @PRACTICE_CODE BIGINT     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
 SELECT * FROM FOX_TBL_TASK     
 WHERE TASK_TYPE_ID = @TASK_TYPE_ID    
 AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
 AND PRACTICE_CODE = @PRACTICE_CODE     
 AND ISNULL(DELETED,0) = 0    
 ORDER BY CREATED_DATE DESC    
END    
