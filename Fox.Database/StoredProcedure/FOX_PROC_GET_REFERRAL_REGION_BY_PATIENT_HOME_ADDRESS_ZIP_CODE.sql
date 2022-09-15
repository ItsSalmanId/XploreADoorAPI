IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_HOME_ADDRESS_ZIP_CODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_HOME_ADDRESS_ZIP_CODE  
GO
  --EXEC [FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_HOME_ADDRESS_ZIP_CODE] 1011163,101116354812732          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_HOME_ADDRESS_ZIP_CODE] @PRACTICE_CODE   BIGINT,         
                                                                                @PATIENT_ACCOUNT BIGINT        
AS        
     BEGIN        
         SELECT TOP 5 ftrr.*        
         FROM dbo.FOX_TBL_REFERRAL_REGION ftrr        
              LEFT OUTER JOIN dbo.FOX_TBL_REFERRAL_REGION_COUNTY ftrrc ON ftrrc.REFERRAL_REGION_ID = ftrr.REFERRAL_REGION_ID        
                                                                          AND ftrrc.DELETED = 0        
                                                                          AND ftrrc.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT OUTER JOIN dbo.FOX_TBL_REGION_ZIPCODE_DATA ftrzd ON ftrzd.REGION_ZIPCODE_DATA_ID = ftrrc.REGION_ZIPCODE_DATA_ID        
                                                                       AND ftrzd.DELETED = 0        
                                                                       AND ftrzd.PRACTICE_CODE = @PRACTICE_CODE        
         WHERE ftrzd.ZIP_CODE IN        
         (        
             SELECT TOP 1 ftpa.ZIP        
             FROM dbo.FOX_TBL_PATIENT_ADDRESS ftpa        
             WHERE ftpa.DELETED = 0        
                   AND ftpa.ADDRESS_TYPE = 'Home Address'        
                   AND ftpa.PATIENT_ACCOUNT = @PATIENT_ACCOUNT        
             ORDER BY ftpa.PATIENT_ADDRESS_HISTORY_ID DESC        
         )        
               AND ftrr.DELETED = 0        
               --AND ftrr.IS_ACTIVE = 1         
               AND ISNULL(ftrr.IS_INACTIVE, 0) = 0     
               AND ftrr.PRACTICE_CODE = @PRACTICE_CODE        
         ORDER BY REFERRAL_REGION_ID DESC;        
     END; 