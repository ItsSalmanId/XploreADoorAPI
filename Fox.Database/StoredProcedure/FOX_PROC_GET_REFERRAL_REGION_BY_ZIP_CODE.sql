IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_REGION_BY_ZIP_CODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_REGION_BY_ZIP_CODE  
GO    
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REFERRAL_REGION_BY_ZIP_CODE] @PRACTICE_CODE BIGINT,   
                                                            @ZIP_CODE      VARCHAR(10)  
AS  
     BEGIN  
         SELECT TOP 1 ftrr.*  
         FROM dbo.FOX_TBL_REFERRAL_REGION ftrr  
              LEFT OUTER JOIN dbo.FOX_TBL_REFERRAL_REGION_COUNTY ftrrc ON ftrrc.REFERRAL_REGION_ID = ftrr.REFERRAL_REGION_ID  
                                                                          AND ftrrc.DELETED = 0  
                                                                          AND ftrrc.PRACTICE_CODE = @PRACTICE_CODE  
              LEFT OUTER JOIN dbo.FOX_TBL_REGION_ZIPCODE_DATA ftrzd ON ftrzd.REGION_ZIPCODE_DATA_ID = ftrrc.REGION_ZIPCODE_DATA_ID  
                                                                       AND ftrzd.DELETED = 0  
                                                                       AND ftrzd.PRACTICE_CODE = @PRACTICE_CODE  
         WHERE ftrzd.ZIP_CODE = @ZIP_CODE  
               AND ftrr.DELETED = 0  
               AND ftrr.IS_ACTIVE = 1  
               AND ftrr.PRACTICE_CODE = @PRACTICE_CODE  
         ORDER BY REFERRAL_REGION_ID DESC;  
     END;  
  
---------------------------------------------------------------------------------------------------------------------------  
