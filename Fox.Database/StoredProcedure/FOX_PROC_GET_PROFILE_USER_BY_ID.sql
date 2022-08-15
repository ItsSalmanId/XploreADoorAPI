IF (OBJECT_ID('FOX_PROC_GET_PROFILE_USER_BY_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PROFILE_USER_BY_ID  
GO    
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================             
CREATE PROCEDURE FOX_PROC_GET_PROFILE_USER_BY_ID    
 @PRACTICE_CODE BIGINT,      
 @USER_ID BIGINT       
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SELECT * FROM FOX_TBL_APPLICATION_USER    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
 AND USER_ID = @USER_ID    
 AND ISNULL(DELETED,0) = 0    
END   
