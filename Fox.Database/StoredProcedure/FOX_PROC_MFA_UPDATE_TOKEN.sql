-- =============================================        
-- Author:  <Author,AFTAB KHAN>        
-- Create date: <Create Date,09/10/2023>        
-- Description: <insert token in db for first login>        
CREATE PROCEDURE [dbo].[FOX_PROC_MFA_UPDATE_TOKEN]  
    @OLD_TOKEN VARCHAR(1000),  
 @NEW_TOKEN VARCHAR(1000),  
   @USERID BIGINT      
AS    
BEGIN    
 SET NOCOUNT ON;    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;    
    
 UPDATE [dbo].[FOX_TBL_PROFILE_TOKENS]    
 SET AuthToken = @NEW_TOKEN   
 WHERE AuthToken = @OLD_TOKEN    
  AND UserId = @USERID    
    
 --INSERT INTO FOX_TBL_PROFILE_TOKENS( IssuedOn , ExpiresOn , AuthToken , UserId , Profile  )        
 --VALUES (@ISSUE_DATE , @EXPIRE_DATE, @TOKEN , @USERNAME, @USER_PROFILE )        
 SELECT IssuedOn    
  ,ExpiresOn    
  ,AuthToken    
  ,UserId    
  ,PROFILE    
 FROM FOX_TBL_PROFILE_TOKENS    
 WHERE AUTHTOKEN = @NEW_TOKEN    
END    
    