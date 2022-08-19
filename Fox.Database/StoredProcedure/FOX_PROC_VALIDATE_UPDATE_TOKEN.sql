IF (OBJECT_ID('FOX_PROC_VALIDATE_UPDATE_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_VALIDATE_UPDATE_TOKEN  
GO   
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure [dbo].[FOX_PROC_VALIDATE_UPDATE_TOKEN]  -- '39e91dd7-d012-4622-952e-ffeb8c943cba'  
@TOKEN_VALUE varchar (50)  
AS                
BEGIN                
 --SET NOCOUNT ON;  
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;  
 DECLARE @TOKEN_VALID VARCHAR = 1;  
 UPDATE PT   
 --SET EXPIRESON = DATEADD(SS,(ISNULL(AUTO_LOCK_TIMESPAN, 10)*60),GETDATE())  
 SET EXPIRESON = DATEADD(SS,(480*60),GETDATE())  
 FROM FOX_TBL_PROFILE_TOKENS PT   
 INNER JOIN FOX_TBL_APPLICATION_USER PU ON PT.USERID = PU.USER_ID  
 WHERE PT.AUTHTOKEN = @TOKEN_VALUE AND EXPIRESON >= GETDATE()  
  
 IF(@@ROWCOUNT = 0)  
 BEGIN  
  UPDATE FOX_TBL_PROFILE_TOKENS SET EXPIRESON =GETDATE() WHERE AUTHTOKEN = @TOKEN_VALUE  
  SET @TOKEN_VALID = 0;  
 END  
 SELECT @TOKEN_VALID AS TOKEN_VALID  
  
END  
  
  
  
