IF (OBJECT_ID('FOX_PROC_GET_SUPERVISOR') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SUPERVISOR  
GO 
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================          
CREATE PROCEDURE FOX_PROC_GET_SUPERVISOR    
 @PRACTICE_CODE BIGINT,      
 @EMAIL VARCHAR(255)    
 AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SELECT * FROM FOX_TBL_APPLICATION_USER    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
 AND LOWER(EMAIL) LIKE LOWER(@EMAIL)    
 AND ISNULL(DELETED,0) = 0    
END   
