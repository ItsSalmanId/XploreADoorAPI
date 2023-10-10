
 -- =============================================                                      
-- AUTHOR:  <Muhammad Salman>                                      
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                      
-- DESCRIPTION: <GE PROVIDER CODE>                                                    
-- [dbo].[FOX_PROC_GET_USER_ID_BY_PROVIDER_CODE] 1012714, 6053290          
CREATE procedure FOX_PROC_GET_USER_ID_BY_PROVIDER_CODE --1012714, 6053290    
@PRACTICE_CODE BIGINT,    
@TREATING_PROVIDER_ID BIGINT    
AS     
BEGIN    
SELECT cast(FTAU.[USER_ID] as varchar(50)) as [USER_ID] FROM PROVIDERS P WITH(NOLOCK)   
INNER JOIN FOX_TBL_PROVIDER FTP WITH(NOLOCK) ON FTP.provider_code = P.provider_code    
INNER JOIN FOX_TBL_APPLICATION_USER FTAU WITH(NOLOCK) ON FTAU.EMAIL = P.EMAIL_ADDRESS    
WHERE FOX_PROVIDER_ID = @TREATING_PROVIDER_ID AND ISNULL(FTP.DELETED,0)=0 AND FTP.PRACTICE_CODE = @PRACTICE_CODE    
AND ISNULL(FTAU.DELETED,0)=0 AND ISNULL(FTAU.IS_ACTIVE,0)=1    
END    
    
