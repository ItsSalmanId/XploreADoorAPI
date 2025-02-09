IF (OBJECT_ID('FOX_GET_SMART_REF_REGION') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_REF_REGION  
GO 
CREATE PROCEDURE [dbo].[FOX_GET_SMART_REF_REGION] --1011163, 'sdfgs'        
 (        
 @PRACTICE_CODE BIGINT        
 ,@SEARCHVALUE VARCHAR(MAX)        
 )        
AS        
BEGIN        
 IF (@SEARCHVALUE = '')        
 BEGIN        
  SET @SEARCHVALUE = NULL        
 END        
        
 SELECT TOP (100) REFERRAL_REGION_ID        
  ,REFERRAL_REGION_CODE        
  ,REFERRAL_REGION_NAME        
 FROM FOX_TBL_REFERRAL_REGION        
 WHERE (        
   REFERRAL_REGION_CODE LIKE '%' + @SEARCHVALUE + '%'        
   OR REFERRAL_REGION_NAME LIKE '%' + @SEARCHVALUE + '%'        
   )        
  AND ISNULL(deleted, 0) = 0        
  AND (    
 @PRACTICE_CODE  IS NULL     
 OR FOX_TBL_REFERRAL_REGION.PRACTICE_CODE = @PRACTICE_CODE    
 )        
END   
