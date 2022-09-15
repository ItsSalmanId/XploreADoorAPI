IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_USER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_USER  
GO  
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================         
CREATE PROCEDURE FOX_PROC_GET_REFERRAL_USER    
 @SORCE_TYPE VARCHAR(15),    
 @EMAIL VARCHAR(255)    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 IF (UPPER(@SORCE_TYPE) = 'EMAIL' )    
 BEGIN    
   SELECT TOP 1 * FROM FOX_TBL_APPLICATION_USER       
   WHERE EMAIL = @EMAIL    
   AND ISNULL(DELETED,0) = 0    
   AND IS_ACTIVE = 1    
   AND IS_APPROVED = 1    
 END    
 ELSE IF (UPPER(@SORCE_TYPE) = 'FAX' )    
 BEGIN    
  SELECT TOP 1 * FROM FOX_TBL_APPLICATION_USER       
   WHERE ISNULL(DELETED,0) = 0    
   AND IS_ACTIVE = 1    
   AND IS_APPROVED = 1    
   AND (    
    FAX = @EMAIL    
    OR FAX_2 = @EMAIL    
    OR FAX_3 = @EMAIL    
   )    
 END    
END   
