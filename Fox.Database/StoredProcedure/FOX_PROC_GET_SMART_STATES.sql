IF (OBJECT_ID('FOX_PROC_GET_SMART_STATES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_STATES  
GO 
---------------------------------------------------------------------------------------------------------------------------   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SMART_STATES] @SEARCH_TEXT VARCHAR(50)  
AS  
     BEGIN  
         SELECT *  
         FROM STATES  
         WHERE STATE_CODE LIKE @SEARCH_TEXT+'%'  
               OR STATE_NAME LIKE '%'+@SEARCH_TEXT+'%';  
     END;  
  
  
  
---------------------------------------------------------------------------------------------------------------------------  
  
