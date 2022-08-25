IF (OBJECT_ID('FOX_PROC_GET_USER_ROLE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USER_ROLE  
GO     
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <Create Date,,>        
-- Description: <Description,,>        
-- =============================================        
 -- FOX_PROC_GET_USER_ROLE '1163TESTING', 1011163        
CREATE PROCEDURE FOX_PROC_GET_USER_ROLE         
 -- Add the parameters for the stored procedure here        
 @USER_NAME VARCHAR(70) NULL,         
 @PRACTICE_CODE BIGINT NULL        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
        
    -- Insert statements for procedure here        
 SELECT  R.*         
 FROM FOX_TBL_APPLICATION_USER AU        
 INNER JOIN FOX_TBL_ROLE R ON R.ROLE_ID = AU.ROLE_ID         
 WHERE AU.PRACTICE_CODE = @PRACTICE_CODE        
 AND AU.USER_NAME = @USER_NAME        
 AND ISNULL(AU.DELETED,0) = 0        
END        
