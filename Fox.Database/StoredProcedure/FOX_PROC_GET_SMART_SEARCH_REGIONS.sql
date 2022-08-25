IF (OBJECT_ID('FOX_PROC_GET_SMART_SEARCH_REGIONS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_SEARCH_REGIONS  
GO 
-- =============================================          
-- AUTHOR:  <DEVELOPER, ABDUL SATTAR>          
-- CREATE DATE: <CREATE DATE, 08/31/2019>          
-- DESCRIPTION: <Get smart search regions>          
          
-- [dbo].[FOX_PROC_GET_SMART_SEARCH_REGIONS] 'Adams',1011163           
          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SMART_SEARCH_REGIONS] --1011163                
--@REGION_STRING VARCHAR(50),             
@PRACTICE_CODE BIGINT                 
AS                
     BEGIN                
                 SELECT distinct REFERRAL_REGION_CODE,        
     REFERRAL_REGION_NAME,        
     REFERRAL_REGION_ID,        
     REFERRAL_REGION_CODE +' - '+ REFERRAL_REGION_NAME AS REGION_CODE_NAME        
                 FROM   FOX_TBL_REFERRAL_REGION              
                 WHERE ISNULL(Deleted, 0) = 0        
     AND ISNULL(IS_ACTIVE, 0) = 1           
        AND PRACTICE_CODE = @PRACTICE_CODE           
        --               AND (        
        --REFERRAL_REGION_CODE LIKE '%' + @REGION_STRING + '%'        
        --OR REFERRAL_REGION_NAME LIKE  @REGION_STRING + '%'        
        --);        
     END;      
