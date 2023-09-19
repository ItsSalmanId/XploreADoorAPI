IF (OBJECT_ID('FOX_PROC_UPDATE_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_TOKEN  
GO 
Create Procedure [dbo].[FOX_PROC_UPDATE_TOKEN]      
 @USERNAME BIGINT,        
 @TOKEN VARCHAR(100),        
 @USER_PROFILE VARCHAR(MAX)        
        
AS        
BEGIN         
 SET NOCOUNT ON;        
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;        
      
 UPDATE [dbo].[FOX_TBL_PROFILE_TOKENS]      
    SET [Profile] = @USER_PROFILE      
  WHERE (AuthToken = @TOKEN) AND (UserId = @USERNAME)      
      
 --INSERT INTO FOX_TBL_PROFILE_TOKENS( IssuedOn , ExpiresOn , AuthToken , UserId , Profile  )        
 --VALUES (@ISSUE_DATE , @EXPIRE_DATE, @TOKEN , @USERNAME, @USER_PROFILE )        
        
 SELECT IssuedOn ,  ExpiresOn ,  AuthToken , UserId , Profile        
 FROM FOX_TBL_PROFILE_TOKENS        
 WHERE AUTHTOKEN = @TOKEN        
        
END   
