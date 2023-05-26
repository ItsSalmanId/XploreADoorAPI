-- =============================================                        
-- Created By :  Muhammad Salman                        
-- Created date: 02/03/2023   
-- Description: This SP trigger to check expired token                       
-- =============================================   
ALTER PROCEDURE [dbo].[FOX_PROC_CHECK_EXPIRED_TOKEN]                   
@TOKEN VARCHAR(100)                        
AS                    
BEGIN                    
SET NOCOUNT ON;     
SELECT [TokenId]    
      ,[UserId]    
      ,[AuthToken]    
      ,[IssuedOn]    
      ,[ExpiresOn]    
      ,[Profile]    
      ,isnull([isLogOut],0) isLogOut    
   FROM  FOX_TBL_PROFILE_TOKENS with (nolock)            
WHERE AUTHTOKEN = @TOKEN     
End