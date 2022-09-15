IF (OBJECT_ID('FOX_PROC_GET_LOCATION_CHAR_CODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_LOCATION_CHAR_CODE  
GO  
-------------------------------------------------------------------------------------------------- FOX_PROC_GET_LOCATION_CHAR_CODE ---------------------------------------------------------------------------------------------    
---- =============================================                            
---- Author:  <Muhammad Arslan Tufail>                            
---- Create date: <04/09/2020>                            
---- Description: <Return Location Char Code>                            
---- =============================================              
-- EXEC [FOX_PROC_GET_LOCATION_CHAR_CODE] 'LOC-OTH-01'             
CREATE PROCEDURE [dbo].[FOX_PROC_GET_LOCATION_CHAR_CODE]              
@LOCATION_ID VARCHAR(20)      
AS              
BEGIN              
SELECT LOCATION_CHAR_CODE FROM lkup_location_ids      
WHERE LOCATION_ID = @LOCATION_ID      
END; 