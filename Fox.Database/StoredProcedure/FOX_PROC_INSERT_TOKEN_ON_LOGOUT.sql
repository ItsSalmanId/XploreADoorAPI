IF (OBJECT_ID('FOX_PROC_INSERT_TOKEN_ON_LOGOUT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_TOKEN_ON_LOGOUT  
GO   
-------------------------------------------------------------- [FOX_PROC_INSERT_TOKEN_ON_LOGOUT] -------------------------------------------------------------------        
-- =============================================    
-- Author:  Muhammad Arslan    
-- Create date: 2/1/2021    
-- Description: This Procedure is use to Insert Profile Token    
-- =============================================    
CREATE PROCEDURE FOX_PROC_INSERT_TOKEN_ON_LOGOUT     
 -- Add the parameters for the stored procedure here    
 @TokenSecurityID BIGINT,    
 @AUTHTOKEN VARCHAR(4000),     
 @ISSUEDON DATETIME,    
 @USER_NAME VARCHAR(50)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 INSERT INTO FOX_TBL_PROFILE_TOKENS_SECURITY(TokenSecurityID, isLogOut, AuthToken, IssuedOn, ExpiresOn, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED)    
 VALUES(@TokenSecurityID, 1, @AUTHTOKEN, @ISSUEDON, GETDATE(), @USER_NAME, GETDATE(), @USER_NAME, GETDATE(), 0);    
END 