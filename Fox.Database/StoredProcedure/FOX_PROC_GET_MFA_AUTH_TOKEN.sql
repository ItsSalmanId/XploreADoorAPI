-- Author:  <Author,Aftab Khan>        
-- Create date: <Create Date,12/10/2023>        
-- Description: <insert token in db for first login>    
--EXEC [dbo].[FOX_PROC_GET_MFA_AUTH_TOKEN] 53411372  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_MFA_AUTH_TOKEN]    
 @USER_ID BIGINT  
AS    
BEGIN       
 SELECT top 1 IssuedOn    
  ,ExpiresOn    
  ,AuthToken    
  ,UserId    
 FROM FOX_TBL_PROFILE_TOKENS with (nolock)    
 WHERE UserId = @USER_ID  ORDER BY IssuedOn Desc   
END    