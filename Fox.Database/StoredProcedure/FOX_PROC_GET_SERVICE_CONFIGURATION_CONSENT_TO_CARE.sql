   -- =============================================                  
-- Author:  <Muhammad Salman>                  
-- Create date: <09/26/2>                  
-- Description: <Description,,>       
-- [dbo].[FOX_PROC_GET_SERVICE_CONFIGURATION_CONSENT_TO_CARE]           
-- =============================================          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SERVICE_CONFIGURATION_CONSENT_TO_CARE]        
AS        
     BEGIN        
         SELECT FOXREHAB_CONFIG_ID,         
                PRACTICE_CODE,         
                EMERGENCY_EMAIL,         
                ENVIRONMENT_EMAIL,         
                FAX_ID,         
                FAX_USERNAME,         
                FAX_PASSWORD,         
                DELETED,         
                CREATED_BY,         
                CREATED_DATE,         
                MODIFIED_BY,         
                MODIFIED_DATE,         
                EMAIL_PASSWORD,         
                EMAIL_USERNAME,         
                DIR_PATH_DB+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\ConsentToCareImages' AS IMAGES_PATH_DB,         
                DIR_PATH_DB+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\ConsentToCareDocuments\' AS ORIGINAL_FILES_PATH_DB,         
                DIR_PATH_SERVER+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\ConsentToCareImages' AS IMAGES_PATH_SERVER        
                ,--IMAGES_PATH           
                DIR_PATH_SERVER+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\ConsentToCareDocuments\' AS ORIGINAL_FILES_PATH_SERVER   
    ,DIR_PATH_SERVER+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\ConsentToCareDocuments\' AS DOCUMENTS_PATH_SERVER            
                ,--PDF_PATH           
                ENVIRONMENT_NAME,         
                LOAD_FAXES,         
                LOAD_EMAILS,        
                (SELECT Office_Id FROM Maintenance) AS Office_Id        
         FROM FOX_TBL_FAX_SERVICE_CONFIG  WITH(NOLOCK)       
         WHERE DELETED = 0 and isnull(is_direct_mail,0) <> 1;        
     END; 