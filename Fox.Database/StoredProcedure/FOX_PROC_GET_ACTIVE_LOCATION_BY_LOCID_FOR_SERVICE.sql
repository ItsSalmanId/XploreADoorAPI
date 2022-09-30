IF (OBJECT_ID('FOX_PROC_GET_ACTIVE_LOCATION_BY_LOCID_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ACTIVE_LOCATION_BY_LOCID_FOR_SERVICE
GO  
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get active location by LOC_ID and practice code>              
-- =============================================              
--EXEC FOX_PROC_GET_ACTIVE_LOCATION_BY_LOCID_FOR_SERVICE  605102  
CREATE PROCEDURE FOX_PROC_GET_ACTIVE_LOCATION_BY_LOCID_FOR_SERVICE      
 @PRACTICE_CODE  BIGINT       
 ,@LOC_ID BIGINT       
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT [NAME],ZIP      
  FROM FOX_TBL_ACTIVE_LOCATIONS      
  where       
  PRACTICE_CODE = @PRACTICE_CODE      
  AND LOC_ID = @LOC_ID      
  and isnull(Deleted,0)=0      
END  
