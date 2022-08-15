IF (OBJECT_ID('FOX_PROC_GET_INTERFACE_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INTERFACE_PATIENT  
GO   
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/27/2019>        
-- Description: <Description,,>        
-- =============================================        
CREATE PROCEDURE FOX_PROC_GET_INTERFACE_PATIENT     
 @PRACTICE_CODE BIGINT,     
 @PATIENT_ACCOUNT BIGINT     
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
  SELECT TOP 1 * FROM FOX_TBL_INTERFACE_SYNCH         
  WHERE PRACTICE_CODE = @PRACTICE_CODE    
  AND ISNULL(DELETED,0) = 0      
  AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
  AND IS_SYNCED = 1    
      
END   
