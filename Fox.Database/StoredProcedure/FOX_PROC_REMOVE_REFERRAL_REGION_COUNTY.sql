IF (OBJECT_ID('FOX_PROC_REMOVE_REFERRAL_REGION_COUNTY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_REMOVE_REFERRAL_REGION_COUNTY  
GO   
--****************************************************************************11************************************************************************************  
-- =============================================            
-- Author:  <MUHAMMAD IMRAN>            
-- Create date: <Create Date,,>            
-- Description: <Description,,>            
-- =============================================           
-- FOX_PROC_REMOVE_REFERRAL_REGION_COUNTY 1011163, 'Sagadahoc County','ME', 548451                    
CREATE PROCEDURE [FOX_PROC_REMOVE_REFERRAL_REGION_COUNTY]             
 @PRACTICE_CODE BIGINT,            
 @COUNTY VARCHAR(250),            
 @STATE VARCHAR(50),            
 @REGION_ID BIGINT NULL            
AS            
BEGIN            
 -- SET NOCOUNT ON added to prevent extra result sets from            
 -- interfering with SELECT statements.            
 --SELECT ZIP_STATE_COUNTY_ID INTO #TEMP             
 --FROM FOX_TBL_ZIP_STATE_COUNTY            
 --WHERE REFERRAL_REGION_ID = @REGION_ID            
 --AND STATE = @STATE            
 --AND COUNTY = @COUNTY            
 --AND PRACTICE_CODE = @PRACTICE_CODE            
            
 --UPDATE FOX_TBL_REFERRAL_REGION_COUNTY            
 -- SET DELETED = 1            
 --WHERE REFERRAL_REGION_ID = @REGION_ID            
 --AND PRACTICE_CODE = @PRACTICE_CODE            
 --AND ZIP_STATE_COUNTY_ID IN (SELECT ZIP_STATE_COUNTY_ID FROM #TEMP)           
           
 --IF OBJECT_ID('tempdb..#TEMP') IS NOT NULL          
 --   DROP TABLE #TEMP       
      
        --sattar 05-01-2019  
 --SELECT ZIP_STATE_COUNTY_ID           
 --INTO #TEMP           
 --FROM FOX_TBL_ZIP_STATE_COUNTY          
 --WHERE REFERRAL_REGION_ID = @REGION_ID            
 --AND STATE = @STATE            
 --AND COUNTY = @COUNTY            
 --AND PRACTICE_CODE = @PRACTICE_CODE            
   
 --sattar 05-01-2019  
            
--DECLARE @REFERRAL_REGION_COUNTY_ID BIGINT           
--SET @REFERRAL_REGION_COUNTY_ID = ( SELECT TOP 1 REFERRAL_REGION_COUNTY_ID FROM FOX_TBL_REFERRAL_REGION_COUNTY  WHERE REFERRAL_REGION_ID = @REGION_ID and ZIP_STATE_COUNTY_ID In (SELECT ZIP_STATE_COUNTY_ID FROM #TEMP))          
          
--sattar 05-01-2019  
--UPDATE FOX_TBL_REFERRAL_REGION_COUNTY            
-- SET DELETED = 1          
--WHERE REFERRAL_REGION_COUNTY_ID IN ( SELECT REFERRAL_REGION_COUNTY_ID FROM FOX_TBL_REFERRAL_REGION_COUNTY  WHERE REFERRAL_REGION_ID = @REGION_ID and ZIP_STATE_COUNTY_ID In (SELECT ZIP_STATE_COUNTY_ID FROM #TEMP))          
--sattar 05-01-2019  
            
 UPDATE FOX_TBL_ZIP_STATE_COUNTY          
   SET REFERRAL_REGION_ID = NULL            
 WHERE REFERRAL_REGION_ID = @REGION_ID            
 AND STATE = @STATE            
 AND COUNTY = @COUNTY            
 AND PRACTICE_CODE = @PRACTICE_CODE            
            
            
END 