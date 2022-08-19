IF (OBJECT_ID('FOX_PROC_GET_CITY_STATE_COUNTY_REGION_BY_CITY_NEW') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_CITY_STATE_COUNTY_REGION_BY_CITY_NEW  
GO   
-- 02  
-- =============================================  
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>  
-- CREATE DATE: <CREATE DATE, 05/08/2019>  
-- DESCRIPTION: <FOX PROC GET CITY STATE COUNTY REGION BY CITY NEW>  
  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_CITY_STATE_COUNTY_REGION_BY_CITY_NEW] --'Bentonville',1011163        
@CITY VARCHAR(20),     
@PRACTICE_CODE BIGINT         
AS        
     BEGIN        
                    SELECT TOP 1 LEFT(ZCS.ZIP_Code, 5) AS ZIP_CODE,         
                        ZCS.place_name AS CITY_NAME,         
                        ZCS.State AS STATE_CODE,      
                        ZSC.COUNTY AS COUNTY,      
                        ZSC.REFERRAL_REGION_ID AS REFERRAL_REGION_ID,      
                        RR.REFERRAL_REGION_NAME AS REGION        
                 FROM FOX_TBL_ZIP_STATE_COUNTY ZCS      
     LEFT JOIN FOX_TBL_ZIP_STATE_COUNTY ZSC ON ZSC.ZIP_CODE = ZCS.ZIP_Code      
                                           AND ISNULL(ZSC.DELETED, 0) = 0    
                                           AND ZSC.PRACTICE_CODE = @PRACTICE_CODE      
     LEFT JOIN fox_tbl_referral_region RR ON RR.REFERRAL_REGION_ID = ZSC.REFERRAL_REGION_ID      
                                           AND ISNULL(RR.DELETED, 0) = 0      
                                           AND RR.IS_ACTIVE = 1      
                                           AND ISNULL(RR.IS_INACTIVE, 0) = 0    
                                           AND RR.PRACTICE_CODE = @PRACTICE_CODE      
                 WHERE ISNULL(ZCS.Deleted, 0) = 0        
                       AND ZCS.place_name LIKE '%' +@CITY + '%';        
     END;   
