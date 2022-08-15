IF (OBJECT_ID('FOX_PROC_GET_DISTINCT_MTBC_CREDENTIALS_CATEGORY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DISTINCT_MTBC_CREDENTIALS_CATEGORY 
GO   
------------------------------------------SP_HELPTEXT [FOX_PROC_GET_DISTINCT_MTBC_CREDENTIALS_CATEGORY]------------------------       
---- =============================================                                              
---- Author:  <Muhammad Arslan Tufail>                                              
---- Create date: <6/11/2020>                                              
---- Description: <Get Distinct Names>                                              
---- =============================================                                                      
-- EXEC [DBO].[FOX_PROC_GET_DISTINCT_MTBC_CREDENTIALS_CATEGORY] 1011163  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_DISTINCT_MTBC_CREDENTIALS_CATEGORY]               
@PRACTICE_CODE BIGINT                         
--DECLARE @PRACTICE_CODE BIGINT = '1011163'        
AS                                
BEGIN                              
 DECLARE @match_count INT;        
 DECLARE @unmatch_count INT;        
         
 SELECT          
  @match_count = COUNT(DISTINCT CATEGORY_DESCRIPTION)        
 from FOX_TBL_MTBC_CREDENTIALS_AUTOMATION WHERE PRACTICE_CODE = @PRACTICE_CODE  AND DELETED = 0 AND CATEGORY_DESCRIPTION IN        
 (SELECT DISTINCT [NAME] FROM FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS WHERE DELETED = 0)         
         
 SELECT          
  @unmatch_count = COUNT(DISTINCT CATEGORY_DESCRIPTION)        
 from FOX_TBL_MTBC_CREDENTIALS_AUTOMATION WHERE PRACTICE_CODE = @PRACTICE_CODE AND DELETED = 0 AND CATEGORY_DESCRIPTION NOT IN        
 (SELECT DISTINCT [NAME] FROM FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS WHERE DELETED = 0)         
        
 SELECT @match_count AS [MatchCategoriesCount],@unmatch_count AS [UnmatchCategoriesCount];        
END; 