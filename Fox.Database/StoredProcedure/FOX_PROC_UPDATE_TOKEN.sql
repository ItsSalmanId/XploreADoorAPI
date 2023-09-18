IF (OBJECT_ID('FOX_PROC_UPDATE_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_TOKEN  
GO 
ALTER Procedure [dbo].[FOX_PROC_UPDATE_TOKEN]        
 @ISLOGOUT BIGINT,  
 @ISVALIDATE BIGINT,  
 @ISMFAVERIFIED BIGINT,    
 @USERNAME BIGINT,                
 @TOKEN VARCHAR(100),                
 @USER_PROFILE VARCHAR(MAX)                
                
AS                
BEGIN                 
 SET NOCOUNT ON;                
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;                
              
 UPDATE [dbo].[FOX_TBL_PROFILE_TOKENS]              
    SET [Profile] = @USER_PROFILE, isLogOut = @ISLOGOUT, isValidate =  @ISVALIDATE, isMFAVerified = @ISMFAVERIFIED             
  WHERE (AuthToken = @TOKEN) AND (UserId = @USERNAME)              
              
 --INSERT INTO FOX_TBL_PROFILE_TOKENS( IssuedOn , ExpiresOn , AuthToken , UserId , Profile  )                
 --VALUES (@ISSUE_DATE , @EXPIRE_DATE, @TOKEN , @USERNAME, @USER_PROFILE )                
                
 SELECT IssuedOn ,  ExpiresOn ,  AuthToken , UserId , Profile, isLogOut, isMFAVerified, isValidate        
 FROM FOX_TBL_PROFILE_TOKENS with (nolock)                 
 WHERE AUTHTOKEN = @TOKEN                
                
END 
