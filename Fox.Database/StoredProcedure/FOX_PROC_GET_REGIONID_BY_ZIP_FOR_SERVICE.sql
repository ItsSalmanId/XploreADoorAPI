IF (OBJECT_ID('FOX_PROC_GET_REGIONID_BY_ZIP_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REGIONID_BY_ZIP_FOR_SERVICE  
GO   
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get region ID by zip code>              
-- =============================================              
--EXEC FOX_PROC_GET_REGIONID_BY_ZIP_FOR_SERVICE  605102       
CREATE PROCEDURE FOX_PROC_GET_REGIONID_BY_ZIP_FOR_SERVICE      
@PRACTICE_CODE  BIGINT,      
 @ZIP_CODE varchar(50)      
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT REFERRAL_REGION_ID      
  FROM FOX_TBL_ZIP_STATE_COUNTY      
  where       
  PRACTICE_CODE = @PRACTICE_CODE      
  and SUBSTRING(ZIP_CODE,0,6) = SUBSTRING(@ZIP_CODE,0,6)      
  and isnull(Deleted,0)=0      
END  
