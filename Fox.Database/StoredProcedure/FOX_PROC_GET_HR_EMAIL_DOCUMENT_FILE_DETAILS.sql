IF (OBJECT_ID('FOX_PROC_GET_HR_EMAIL_DOCUMENT_FILE_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_HR_EMAIL_DOCUMENT_FILE_DETAILS 
GO  
----------------------------------------SP_HELPTEXT FOX_PROC_GET_HR_EMAIL_DOCUMENT_FILE_DETAILS------------------------    
---- =============================================                                                    
---- Author:  <Muhammad Arslan Tufail>                                                    
---- Create date: <6/11/2020>                                                    
---- Description: <GET MTBC DOCUMENT FILE Names>                                                    
---- =============================================                                                                     
-- EXEC [DBO].[FOX_PROC_GET_HR_EMAIL_DOCUMENT_FILE_DETAILS] 1012714  
                
CREATE PROCEDURE [dbo].[FOX_PROC_GET_HR_EMAIL_DOCUMENT_FILE_DETAILS]                     
@PRACTICE_CODE BIGINT                               
AS                                      
BEGIN                                    
   SELECT ACA.HR_CONFIGURE_ID, ACA.PRACTICE_CODE, ACA.NAME, ACA.IS_ENABLED, ACA.DELETED, DFA.DOCUMENT_PATH, reverse(left(reverse( DFA.DOCUMENT_PATH),charindex('.',reverse( DFA.DOCUMENT_PATH))-1)) AS FILE_EXTENSION, DFA.ORIGINAL_FILE_NAME FROM             

   FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS AS ACA            
   INNER JOIN FOX_TBL_HR_EMAIL_DOCUMENT_FILE_ALL AS DFA ON ACA.HR_CONFIGURE_ID = DFA.HR_CONFIGURE_ID            
   WHERE ACA.PRACTICE_CODE = @PRACTICE_CODE AND            
   ISNULL(ACA.DELETED, 0) = 0 AND ISNULL(DFA.DELETED, 0) = 0             
END; 