IF (OBJECT_ID('FOX_PROC_GET_ROLE_BY_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ROLE_BY_ID  
GO    
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <Create Date,,>        
-- Description: <Description,,>        
-- =============================================        
 -- FOX_PROC_GET_USER_ROLE '1163TESTING', 1011163        
CREATE PROCEDURE FOX_PROC_GET_ROLE_BY_ID         
 -- Add the parameters for the stored procedure here        
 @ROLE_ID VARCHAR(70) NULL        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
        
           
 SELECT  R.*         
 FROM FOX_TBL_ROLE R         
 WHERE R.ROLE_ID = @ROLE_ID        
 AND ISNULL(DELETED,0) = 0        
         
END        
