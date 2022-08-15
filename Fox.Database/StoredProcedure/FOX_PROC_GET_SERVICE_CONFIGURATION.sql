IF (OBJECT_ID('FOX_PROC_GET_SERVICE_CONFIGURATION') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SERVICE_CONFIGURATION  
GO    
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SERVICE_CONFIGURATION]  
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
                DIR_PATH_DB+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\Images' AS IMAGES_PATH_DB,   
                DIR_PATH_DB+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\OriginalFiles\' AS ORIGINAL_FILES_PATH_DB,   
                DIR_PATH_SERVER+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\Images' AS IMAGES_PATH_SERVER  
                ,--IMAGES_PATH     
                DIR_PATH_SERVER+CAST(PRACTICE_CODE AS VARCHAR(20))+'\'+CONVERT(VARCHAR(10), GETDATE(), 110)+'\OriginalFiles\' AS ORIGINAL_FILES_PATH_SERVER  
                ,--PDF_PATH     
                ENVIRONMENT_NAME,   
                LOAD_FAXES,   
                LOAD_EMAILS,  
                (SELECT Office_Id FROM Maintenance) AS Office_Id  
         FROM FOX_TBL_FAX_SERVICE_CONFIG  
         WHERE DELETED = 0;  
     END; 