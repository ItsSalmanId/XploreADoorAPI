-- =============================================        
-- Author:  <Author,AFTAB KHAN>        
-- Create date: <Create Date,09/10/2023>        
-- Description: <insert token in db for first login>  
--EXEC [dbo].[FOX_PROC_RESET_COUNT_FOR_MFA_USER] 'L2_53411372'          
CREATE PROCEDURE [dbo].[FOX_PROC_RESET_COUNT_FOR_MFA_USER]          
(          
    @USER_NAME VARCHAR(1000)          
)          
AS          
BEGIN          
 UPDATE FOX_TBL_MFA_INVALID_USER_ATTEMPTS SET FAIL_ATTEMPT_COUNT = 0, SENT_OTP_COUNT = 0, MODIFIED_DATE = GETDATE(), LAST_ATTEMPT_DATE_UTC = GETUTCDATE()     
  WHERE USER_NAME =         
   (SELECT MI.USER_NAME        
    FROM FOX_TBL_APPLICATION_USER AU with (nolock)          
    INNER JOIN FOX_TBL_MFA_INVALID_USER_ATTEMPTS MI ON AU.EMAIL = MI.USER_NAME          
    WHERE (AU.USER_NAME = @USER_NAME OR AU.EMAIL = @USER_NAME) and isnull(MI.USER_NAME,'')<>'')        
END                   
--SELECT * FROM FOX_TBL_MFA_INVALID_USER_ATTEMPTS WHERE USER_NAME = AU.EMAIL      LAST_ATTEMPT_DATE_UTC
