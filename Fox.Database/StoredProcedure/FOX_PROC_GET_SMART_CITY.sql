IF (OBJECT_ID('FOX_PROC_GET_SMART_CITY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_CITY  
GO  
-- 04  
-- =============================================  
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>  
-- CREATE DATE: <CREATE DATE, 05/08/2019>  
-- DESCRIPTION: <FOX PROC GET SMART CITY>  
  
-- [dbo].[FOX_PROC_GET_SMART_CITY] 'Ben',1011163   
  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SMART_CITY] --'Bentonville',1011163        
@CITY VARCHAR(20),     
@PRACTICE_CODE BIGINT         
AS        
     BEGIN        
                 SELECT distinct place_name AS CITY_NAME         
                 FROM   FOX_TBL_ZIP_STATE_COUNTY      
                 WHERE ISNULL(Deleted, 0) = 0   
        AND PRACTICE_CODE = @PRACTICE_CODE   
                       AND place_name LIKE '%' +@CITY + '%';        
     END;   
