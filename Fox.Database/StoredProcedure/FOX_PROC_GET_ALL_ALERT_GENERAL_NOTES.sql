IF (OBJECT_ID('FOX_PROC_GET_ALL_ALERT_GENERAL_NOTES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ALL_ALERT_GENERAL_NOTES
GO
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ALL_ALERT_GENERAL_NOTES] @PRACTICE_CODE   BIGINT,         
                                                              @PATIENT_ACCOUNT BIGINT        
AS        
     BEGIN        
         SELECT fta.*,         
                ftat.DESCRIPTION AS ALERT_TYPE_NAME,     
    CASE                       
    WHEN ISNULL(ftau.LAST_NAME, '') = '' THEN ISNULL(ftau.FIRST_NAME, '')                      
    ELSE ISNULL(ftau.LAST_NAME, '') + ', ' + ISNULL(ftau.FIRST_NAME, '')                      
      END AS CREATED_BY_FULL_NAME            
               -- ftau.LAST_NAME+', '+ftau.FIRST_NAME AS CREATED_BY_FULL_NAME        
         FROM dbo.FOX_TBL_ALERT fta        
              LEFT OUTER JOIN dbo.FOX_TBL_ALERT_TYPE ftat ON ftat.ALERT_TYPE_ID = fta.ALERT_TYPE_ID        
              LEFT OUTER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON fta.CREATED_BY = ftau.USER_NAME        
         WHERE FTA.DELETED = 0        
               AND fta.PATIENT_ACCOUNT = @PATIENT_ACCOUNT        
               AND fta.PRACTICE_CODE = @PRACTICE_CODE        
               AND (      
    fta.EFFECTIVE_TILL IS NULL      
     OR       
    CONVERT(DATE, fta.EFFECTIVE_TILL) >= CONVERT(DATE, GETDATE())        
               )        
         ORDER BY 1 DESC;        
     END; 

