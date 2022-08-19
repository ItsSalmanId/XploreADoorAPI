IF (OBJECT_ID('FOX_PROC_GET_DISTINCT_MTBC_UNMAPPED_CATEGORY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DISTINCT_MTBC_UNMAPPED_CATEGORY 
GO   
------------------------------------------SP_HELPTEXT [FOX_PROC_GET_DISTINCT_MTBC_UNMAPPED_CATEGORY]------------------------  
---- =============================================                                                
---- Author:  <Muhammad Arslan Tufail>                                                
---- Create date: <6/11/2020>                                                
---- Description: <Get Unmapped Names>                                                
---- =============================================                                                           
    
-- EXEC [DBO].[FOX_PROC_GET_DISTINCT_MTBC_UNMAPPED_CATEGORY] 1012714           
CREATE PROCEDURE [dbo].[FOX_PROC_GET_DISTINCT_MTBC_UNMAPPED_CATEGORY]                 
@PRACTICE_CODE BIGINT                           
--DECLARE @PRACTICE_CODE BIGINT = '1011163'          
AS                                  
BEGIN                                
          
 SELECT DISTINCT MCA.CATEGORY_DESCRIPTION FROM         
 FOX_TBL_MTBC_CREDENTIALS_AUTOMATION AS MCA         
 INNER JOIN FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS AS ACA ON MCA.CATEGORY_DESCRIPTION NOT IN (SELECT DISTINCT [NAME] FROM FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS WHERE ISNULL(DELETED, 0) = 0)        
 WHERE MCA.DELETED = ACA.DELETED         
END; 