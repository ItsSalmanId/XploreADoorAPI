IF (OBJECT_ID('FOX_PROC_GET_PROFILE_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PROFILE_TOKEN  
GO   
-------------------------------------------------------------- [FOX_PROC_GET_PROFILE_TOKEN] -------------------------------------------------------------------         
-- =============================================      
-- Author:  Muhammad Arslan      
-- Create date: 2/1/2021      
-- Description: This Procedure is used to Match Token against the User Name      
-- =============================================      
-- EXEC FOX_PROC_GET_PROFILE_TOKEN 'aE1tF8XB99h5cDNfj8qC58PV209Av7yHoUQZRB9PA6CZGpCQTTCXCsVmiE5f7xiI4qSlndn9c0y6c9T6', '1163TESTING'    
CREATE PROCEDURE FOX_PROC_GET_PROFILE_TOKEN       
 -- Add the parameters for the stored procedure here      
 @AUTHTOKEN VARCHAR(4000),       
 @USERID BIGINT      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
    -- Insert statements for procedure here      
 SELECT TOP 1 * FROM FOX_TBL_PROFILE_TOKENS      
 WHERE AuthToken = SUBSTRING(@AUTHTOKEN, 0, 101) AND UserId = @USERID        
END 