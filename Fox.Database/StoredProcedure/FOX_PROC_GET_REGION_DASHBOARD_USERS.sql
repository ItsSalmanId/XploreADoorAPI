IF (OBJECT_ID('FOX_PROC_GET_REGION_DASHBOARD_USERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_USER  
GO  
-- Author:  <MUHAMMAD IMRAN >            
-- Create date: <Create Date,,>            
-- Description: <Description,,>            
-- =============================================            
            
create PROCEDURE FOX_PROC_GET_REGION_DASHBOARD_USERS-- 548519, 1011163            
 -- Add the parameters for the stored procedure here            
 @REFERRAL_REGION_ID BIGINT,             
 @PRACTICE_CODE BIGINT             
AS            
BEGIN            
 -- SET NOCOUNT ON added to prevent extra result sets from            
 -- interfering with SELECT statements.            
 SET NOCOUNT ON;            
            
    -- Insert statements for procedure here            
 SELECT *, U.FIRST_NAME, U.LAST_NAME,             
 (SELECT R.ROLE_NAME FROM FOX_TBL_ROLE R WHERE R.ROLE_ID = U.ROLE_ID) AS ROLE_NAME            
 FROM FOX_TBL_DASHBOARD_ACCESS DA            
 LEFT JOIN FOX_TBL_APPLICATION_USER U ON U.USER_NAME = DA.USER_NAME            
 --LEFT JOIN FOX_TBL_ROLE R ON U.ROLE_ID = U.ROLE_ID            
 WHERE DA.REFERRAL_REGION_ID = @REFERRAL_REGION_ID            
 AND U.PRACTICE_CODE = @PRACTICE_CODE            
 AND ISNULL(DA.DELETED,0) = 0          
 ORDER BY ROLE_NAME, U.LAST_NAME, U.FIRST_NAME        
END 