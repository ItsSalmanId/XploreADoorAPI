-- =============================================      
-- Author:  <AFTAB KHAN>      
-- Create date: <07/12/2023>      
-- Description: <FOX_PROC_SET_MFA_VALID_LOGIN_ATTEMPTS>    
  
-- EXE FOX_PROC_SET_MFA_INVALID_LOGIN_ATTEMPTS 'aftabkhan@carecloud.com'   
  
CREATE PROCEDURE FOX_PROC_SET_MFA_VALID_LOGIN_ATTEMPTS  
@Email NVARCHAR(1000)  
AS   
BEGIN  
IF EXISTS(SELECT * FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS WHERE USER_NAME = @Email)      
 BEGIN      
  UPDATE FOX_TBL_MFA_INVALID_USER_ATTEMPTS      
   SET FAIL_ATTEMPT_COUNT = 0,      
   MODIFIED_DATE = GETDATE()      
  WHERE USER_NAME = @Email      
      
  SELECT * FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS with (nolock) WHERE USER_NAME = @Email      
 END   
 END
