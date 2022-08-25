IF (OBJECT_ID('FOX_PROC_GET_ORDERING_REFERRAL_SOURCE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORDERING_REFERRAL_SOURCE  
GO  
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================              
CREATE PROCEDURE FOX_PROC_GET_ORDERING_REFERRAL_SOURCE      
 @SOURCE_ID BIGINT         
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
  SELECT TOP 1 * FROM FOX_TBL_ORDERING_REF_SOURCE  WITH(NOLOCK)        
  WHERE SOURCE_ID = @SOURCE_ID        
  AND ISNULL(DELETED,0) = 0        
END   
