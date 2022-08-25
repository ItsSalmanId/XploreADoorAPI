IF (OBJECT_ID('FOX_PROC_GET_EMPLOYERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_EMPLOYERS 
GO    
CREATE PROCEDURE [dbo].[FOX_PROC_GET_EMPLOYERS] --1011163,'todd'  
 (  
 @PRACTICE_CODE BIGINT  
 ,@SEARCHVALUE VARCHAR(MAX)  
 )  
AS  
BEGIN  
 IF (@SEARCHVALUE = '')  
 BEGIN  
  SET @SEARCHVALUE = NULL  
 END  
  
 SELECT TOP (100) Employer_Code  
  ,Employer_Name  
  ,Employer_Address  
  ,Employer_ZIP  
  ,Employer_City  
  ,Employer_State  
  ,Employer_Contact_Phone  
  ,FAX_NUMBER  
  ,PLAN_NAME  
  ,IS_REFERRAL_REQUIRED  
  ,REFERRAL_PROVIDER_ID  
  ,CASE   
   WHEN isnull(s.FIRST_NAME, '') <> ''  
    THEN isnull(s.FIRST_NAME, '') + ' ' + isnull(s.LAST_NAME, '')  
   ELSE ''  
   END AS REFERRAL_PROVIDER_NAME  
 FROM Employers e  
 LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE s ON s.SOURCE_ID = e.REFERRAL_PROVIDER_ID  
 WHERE (Employer_Name LIKE '%' + @SEARCHVALUE + '%')  
  AND ISNULL(e.Deleted, 0) = 0  
END  
