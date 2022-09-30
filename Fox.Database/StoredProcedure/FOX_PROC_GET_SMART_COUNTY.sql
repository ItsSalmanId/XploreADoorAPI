IF (OBJECT_ID('FOX_PROC_GET_SMART_COUNTY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_COUNTY  
GO 
-- 05  
-- =============================================  
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>  
-- CREATE DATE: <CREATE DATE, 05/08/2019>  
-- DESCRIPTION: <FOX PROC GET SMART COUNTY>  
-- [dbo].[FOX_PROC_GET_SMART_COUNTY] 'Adams',1011163   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SMART_COUNTY] --'Bentonville',1011163        
@COUNTY VARCHAR(20),     
@PRACTICE_CODE BIGINT         
AS        
     BEGIN        
                 SELECT distinct COUNTY         
                 FROM   FOX_TBL_ZIP_STATE_COUNTY      
                 WHERE ISNULL(Deleted, 0) = 0   
        AND PRACTICE_CODE = @PRACTICE_CODE   
                       AND COUNTY LIKE '%' + @COUNTY + '%';        
     END; 