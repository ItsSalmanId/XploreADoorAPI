IF (OBJECT_ID('FOX_PROC_GET_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT  
GO    
-- =============================================          
-- Author:  <Muhammad Imran>          
-- Create date: <09/27/2019>          
-- Description: <Description,,>          
-- =============================================                
CREATE PROCEDURE FOX_PROC_GET_PATIENT       
      
 @PATIENT_ACCOUNT BIGINT       
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
  SELECT TOP 1 * FROM PATIENT           
  WHERE ISNULL(DELETED,0) = 0        
  AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
    
END   
