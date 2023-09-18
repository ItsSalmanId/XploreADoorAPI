-- =============================================                
-- Author:  <AFTAB KHAN>                
-- Create date: <07/12/2023>                
-- Description: <FOX_PROC_SET_MFA_INVALID_LOGIN_ATTEMPTS>                
-- =============================================                
-- EXEC FOX_PROC_SET_MFA_OTP_ATTEMPTS 'aftabkhan@carecloud.com', "8/2/2023 11:52:21 AM"      
--select * from FOX_TBL_MFA_INVALID_USER_ATTEMPTS order by created_date desc  
CREATE PROCEDURE FOX_PROC_SET_MFA_OTP_ATTEMPTS                 
 -- Add the parameters for the stored procedure here                
 @USER_NAME NVARCHAR(100),        
 @LAST_ATTEMPTUTC_DATETIME DATETIME        
AS                
BEGIN                
 DECLARE @MAX_ID BIGINT                
 SET @MAX_ID = (SELECT ISNULL(MAX(INVALID_MFA_COUNT_ID), 500100) FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS)                
                  
 IF EXISTS(SELECT * FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS with (nolock) WHERE USER_NAME = @USER_NAME)                
 BEGIN                
  UPDATE FOX_TBL_MFA_INVALID_USER_ATTEMPTS                
   SET SENT_OTP_COUNT = ISNULL(SENT_OTP_COUNT, 0) + 1,                
       MODIFIED_DATE = GETDATE(),        
       LAST_ATTEMPT_DATE_UTC = @LAST_ATTEMPTUTC_DATETIME        
  WHERE USER_NAME = @USER_NAME                
                
  SELECT SENT_OTP_COUNT FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS  with (nolock)  WHERE USER_NAME = @USER_NAME                
 END                
 ELSE                
 BEGIN                
  INSERT INTO FOX_TBL_MFA_INVALID_USER_ATTEMPTS(INVALID_MFA_COUNT_ID, USER_NAME, FAIL_ATTEMPT_COUNT, CREATED_DATE, MODIFIED_DATE, LAST_ATTEMPT_DATE_UTC, SENT_OTP_COUNT)                
  VALUES (@MAX_ID + 1, @USER_NAME, 0, GETDATE(), GETDATE(), @LAST_ATTEMPTUTC_DATETIME, 1)                
                
  SELECT SENT_OTP_COUNT FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS with (nolock)  WHERE USER_NAME = @USER_NAME                
 END                
END    
    