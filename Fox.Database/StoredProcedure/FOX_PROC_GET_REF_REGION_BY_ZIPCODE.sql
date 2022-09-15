IF (OBJECT_ID('FOX_PROC_GET_REF_REGION_BY_ZIPCODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REF_REGION_BY_ZIPCODE  
GO    
--8   
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------[FOX_PROC_GET_REF_REGION_BY_ZIPCODE] 1011163,'12345'    
CREATE PROCEDURE [FOX_PROC_GET_REF_REGION_BY_ZIPCODE] --1011163,'07543'     
@PRACTICE_CODE BIGINT,   
@ZIPCODE       VARCHAR(50)  
AS  
     BEGIN  
         SELECT TOP 1 RR.REFERRAL_REGION_ID,   
                      RR.REFERRAL_REGION_NAME,   
                      RR.REFERRAL_REGION_CODE  
         FROM fox_tbl_referral_region AS RR  
              JOIN FOX_TBL_ZIP_STATE_COUNTY AS zsc ON zsc.REFERRAL_REGION_ID = RR.REFERRAL_REGION_ID  
                                                      AND ISNULL(zsc.DELETED, 0) = 0  
                                                      AND zsc.PRACTICE_CODE = @PRACTICE_CODE  
                 
         WHERE zsc.ZIP_CODE = @ZIPCODE  
               AND zsc.IS_MAP = 1  
      AND ISNULL(RR.IS_ACTIVE, 0) = 1  
               AND ISNULL(RR.DELETED, 0) = 0  
               AND RR.PRACTICE_CODE = @PRACTICE_CODE;  
     END;  
