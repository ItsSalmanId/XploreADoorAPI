IF (OBJECT_ID('FOX_PROC_GENERATE_INSERT_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GENERATE_INSERT_TOKEN
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,Mehmood ul Hassan>
-- Create date: <Create Date,12/10/2017>
-- Description:	<insert token in db for first login>
ALTER PROCEDURE [dbo].[FOX_PROC_GENERATE_INSERT_TOKEN] @USERNAME BIGINT                
 ,@TOKEN VARCHAR(100)                
 ,@USER_PROFILE VARCHAR(MAX)    
 ,@ISMFAVERIFIED BIGINT    
 ,@ISLOGOUT BIGINT      
 ,@ISVALIDATE BIGINT  
AS                
BEGIN                
 SET NOCOUNT ON;                
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;                
                
 DECLARE @ISSUE_DATE DATETIME = GETDATE();                
 DECLARE @EXPIRE_DATE DATETIME = DATEADD(ss, 600, GETDATE());                
              
 DECLARE @TAB TABLE (MAXID BIGINT)                  
 DECLARE @MAXID BIGINT                  
                  
 INSERT INTO @TAB EXEC Web_GetMaxColumnID 'FOX_TOKEN_ID'                  
 SET @MAXID = (SELECT TOP 1 MAXID FROM @TAB)              
                
 INSERT INTO FOX_TBL_PROFILE_TOKENS (                
 TokenId              
  ,IssuedOn                
  ,ExpiresOn                
  ,AuthToken                
  ,UserId                
  ,PROFILE     
  ,IsMFAVerified    
  ,isLogOut  
  ,isValidate  
    
  )                
 VALUES (                
  @MAXID              
  ,@ISSUE_DATE                
  ,@EXPIRE_DATE                
  ,@TOKEN                
  ,@USERNAME                
  ,@USER_PROFILE     
  ,@ISMFAVERIFIED    
  ,@ISLOGOUT  
  ,@ISVALIDATE  
    
  )                
                
 SELECT IssuedOn                
  ,ExpiresOn                
  ,AuthToken                
  ,UserId                
  ,PROFILE     
  ,IsMFAVerified    
  ,isLogOut  
  ,isValidate  
 FROM FOX_TBL_PROFILE_TOKENS                
 WHERE AUTHTOKEN = @TOKEN                
END 





