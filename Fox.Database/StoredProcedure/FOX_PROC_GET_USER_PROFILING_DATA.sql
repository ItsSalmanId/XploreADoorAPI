IF (OBJECT_ID('FOX_PROC_GET_USER_PROFILING_DATA') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USER_PROFILING_DATA  
GO     
-- =============================================        
-- Author:  <Author,Mehmood ul Hassan>        
-- Create date: <Create Date,12/10/2017>        
-- Description: <get user data to create profile>        
CREATE PROCEDURE [dbo].[FOX_PROC_GET_USER_PROFILING_DATA] --'1163testing'        
 @UserName VARCHAR(100)        
AS        
BEGIN        
 SET NOCOUNT ON;        
        
 --declare @UserName varchar(100)='1163testing'        
 SELECT USER_ID USERID        
  ,USER_NAME USERNAME        
  ,P.PRACTICE_CODE PRACTICECODE        
  ,P.PRAC_NAME PRACTICENAME        
  ,P.PRAC_ADDRESS PRACTICEADDRESS        
  ,P.PRAC_ADDRESS_LINE2 PRACTICEADDRESSLINE2        
  ,P.PRAC_CITY PRACCITY        
  ,P.PRAC_STATE PRACSTATE        
  ,P.PRAC_ZIP PRACZIP        
  ,P.PRAC_PHONE PRACPHONE        
  ,P.EMAIL_ADDRESS PRACEMAILADDRESS        
  ,P.PRACTICE_ALIASES PRACTICEALIAS        
  ,PU.FIRST_NAME FIRSTNAME        
  ,PU.LAST_NAME LASTNAME        
  ,PU.EMAIL USEREMAILADDRESS        
  ,IS_ADMIN        
  ,PU.ROLE_ID AS RoleId        
  ,MANAGER_ID        
  ,'FoxDocumentDirectory' AS PracticeDocumentDirectory        
  ,EXTENSION        
  ,IS_ACTIVE_EXTENSION        
  ,PRACTICE_ORGANIZATION_ID        
  ,USER_TYPE AS UserType        
  ,SIGNATURE_PATH        
  ,R.ROLE_NAME      
  ,SENDER_TYPE    
  ,EMAIL    
 FROM FOX_TBL_APPLICATION_USER AS PU        
 INNER JOIN PRACTICES AS P ON PU.PRACTICE_CODE = P.PRACTICE_CODE        
 INNER JOIN PRACTICES_PROFILE_OTHER_INFO AS oi ON p.Practice_Code = oi.PRACTICE_CODE        
 LEFT JOIN FOX_TBL_ROLE R ON R.ROLE_ID = PU.ROLE_ID        
  AND R.Practice_Code = P.PRACTICE_CODE        
  AND ISNULL(R.DELETED, 0) = 0        
 WHERE (        
   USER_NAME = @UserName        
   OR EMAIL = @UserName        
   )        
  AND pu.deleted = 0        
  AND pu.is_active = 1        
END 

