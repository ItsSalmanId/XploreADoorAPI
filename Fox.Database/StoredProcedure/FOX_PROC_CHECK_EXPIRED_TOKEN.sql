IF (OBJECT_ID('FOX_PROC_CHECK_EXPIRED_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHECK_EXPIRED_TOKEN
GO  
-------------------------------------------------------------- [FOX_PROC_CHECK_EXPIRED_TOKEN] -------------------------------------------------------------------          
-- =============================================                  
-- Author:  <Muhammad, Nouman>                  
-- Create date: <05/1/2020>                  
-- Description: <SELECT THE EXPIRED TOKEN>            
-- EXEC FOX_PROC_CHECK_EXPIRED_TOKEN 'aE1tF8XB99h5cDNfj8qC58PV209Av7yHoUQZRB9PA6CZGpCQTTCXCsVmiE5f7xiI4qSlndn9c0y6c9T6'         
CREATE PROCEDURE [dbo].[FOX_PROC_CHECK_EXPIRED_TOKEN]             
 @TOKEN VARCHAR(MAX)                  
AS              
BEGIN              
 SET NOCOUNT ON;              
              
              
 SELECT top 1 * FROM FOX_TBL_PROFILE_TOKENS_SECURITY            
 WHERE AUTHTOKEN = SUBSTRING(@TOKEN, 0, 100)   AND  isLogOut = 1  AND ExpiresOn <= GETDATE()        
          
END 