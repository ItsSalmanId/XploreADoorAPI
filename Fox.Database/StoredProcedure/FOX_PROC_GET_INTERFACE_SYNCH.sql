IF (OBJECT_ID('FOX_PROC_GET_INTERFACE_SYNCH') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INTERFACE_SYNCH  
GO    
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================          
CREATE PROCEDURE FOX_PROC_GET_INTERFACE_SYNCH      
 @PRACTICE_CODE BIGINT,      
 @PATIENT_ACCOUNT BIGINT,      
 @Work_ID BIGINT      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
    
 SET NOCOUNT ON;      
      
      
 SELECT  * FROM FOX_TBL_INTERFACE_SYNCH       
 WHERE PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
 AND PRACTICE_CODE = @PRACTICE_CODE      
 AND Work_ID = @Work_ID       
 AND ISNULL(DELETED,0) = 0      
 AND IS_SYNCED = 0    
     
END    
