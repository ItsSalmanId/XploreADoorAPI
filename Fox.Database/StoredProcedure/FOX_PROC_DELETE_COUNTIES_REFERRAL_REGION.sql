IF (OBJECT_ID('FOX_PROC_DELETE_COUNTIES_REFERRAL_REGION') IS NOT NULL ) DROP PROCEDURE FOX_PROC_DELETE_COUNTIES_REFERRAL_REGION
GO
   -- EXEC  FOX_PROC_DELETE_COUNTIES_REFERRAL_REGION 1011163, 548410, 54423697
CREATE PROCEDURE FOX_PROC_DELETE_COUNTIES_REFERRAL_REGION         
 @PRACTICE_CODE      BIGINT,                           
 @REFERRAL_REGION_ID BIGINT,          
 @ZIP_STATE_COUNTY_ID BIGINT           
                                         
AS                          
                        
BEGIN            
  DECLARE @COUNTY VARCHAR(50)          
  SET @COUNTY = ( SELECT COUNTY FROM FOX_TBL_ZIP_STATE_COUNTY WHERE ZIP_STATE_COUNTY_ID = @ZIP_STATE_COUNTY_ID)           
  DECLARE @STATE VARCHAR(50)          
  SET @STATE = ( SELECT STATE FROM FOX_TBL_ZIP_STATE_COUNTY WHERE ZIP_STATE_COUNTY_ID = @ZIP_STATE_COUNTY_ID)           
  --select @COUNTY          
  --select @STATE           
            
   SELECT * FROM FOX_TBL_ZIP_STATE_COUNTY          
   WHERE PRACTICE_CODE = @PRACTICE_CODE          
  AND REFERRAL_REGION_ID = @REFERRAL_REGION_ID          
  AND COUNTY = @COUNTY               
  AND STATE= @STATE           
                     
  UPDATE FOX_TBL_ZIP_STATE_COUNTY           
  SET      REFERRAL_REGION_ID = @REFERRAL_REGION_ID          
  WHERE PRACTICE_CODE = @PRACTICE_CODE          
  AND REFERRAL_REGION_ID = @REFERRAL_REGION_ID          
  AND COUNTY = @COUNTY               
  AND STATE= @STATE                  
END; 