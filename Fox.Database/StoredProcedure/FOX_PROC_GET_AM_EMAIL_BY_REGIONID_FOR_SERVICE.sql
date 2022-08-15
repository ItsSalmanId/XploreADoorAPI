IF (OBJECT_ID('FOX_PROC_GET_AM_EMAIL_BY_REGIONID_FOR_SERVICE ') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_AM_EMAIL_BY_REGIONID_FOR_SERVICE 
GO   
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get account email from region table by REGION_ID>              
-- =============================================              
--EXEC FOX_PROC_GET_AM_EMAIL_BY_REGIONID_FOR_SERVICE  605102       
CREATE PROCEDURE FOX_PROC_GET_AM_EMAIL_BY_REGIONID_FOR_SERVICE      
@PRACTICE_CODE  BIGINT,      
 @REFERRAL_REGION_ID varchar(50)      
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT ACCOUNT_MANAGER_EMAIL      
  FROM FOX_TBL_REFERRAL_REGION      
  where       
  PRACTICE_CODE = @PRACTICE_CODE      
  and ISNULL(IS_ACTIVE,0)=1      
  AND ACCOUNT_MANAGER_EMAIL IS NOT NULL      
  AND REFERRAL_REGION_ID = @REFERRAL_REGION_ID      
  and isnull(Deleted,0)=0      
END