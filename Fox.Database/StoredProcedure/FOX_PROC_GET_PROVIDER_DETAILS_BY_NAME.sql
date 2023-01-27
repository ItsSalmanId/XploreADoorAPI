-- =============================================                                                  
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                                                  
-- CREATE DATE: <CREATE DATE, 12/24/2022>                                                  
-- DESCRIPTION:  THIS PROCEDURE IS USE TO GET PROVIDER DETAILS                                           
-- =============================================       
CREATE PROCEDURE FOX_PROC_GET_PROVIDER_DETAILS_BY_NAME(        
@FIRST_NAME VARCHAR(25),        
@LAST_NAME VARCHAR(25),        
@STATE VARCHAR(2),        
@PRACTICE_CODE BIGINT        
)        
AS        
BEGIN      
  IF (@FIRST_NAME = '')                                                                                    
 BEGIN                                                                                    
  SET @FIRST_NAME = NULL                                                                                    
 END         
SELECT TAXONOMY.DESCRIPTION , PROVIDER.INDIVIDUAL_NPI,PROVIDER.FIRST_NAME, PROVIDER.LAST_NAME,PROVIDER.ADDRESS,PROVIDER.CITY,PROVIDER.STATE,PROVIDER.ZIP,PROVIDER.REGION_NAME,PROVIDER.REGION_CODE,      
PROVIDER.FAX,PROVIDER.PHONE      
FROM FOX_TBL_PROVIDER AS PROVIDER  WITH(NOLOCK)      
INNER JOIN TAXONOMY_CODES AS TAXONOMY  WITH(NOLOCK)      
ON PROVIDER.TAXONOMY_CODE = TAXONOMY.TAXONOMY_CODES        
WHERE         
(PROVIDER.FIRST_NAME = @FIRST_NAME  AND PROVIDER.STATE = @STATE)    
OR (PROVIDER.LAST_NAME = @LAST_NAME AND PROVIDER.STATE = @STATE) AND PROVIDER.PRACTICE_CODE = @PRACTICE_CODE  AND ISNULL(PROVIDER.DELETED,0)=0      
END