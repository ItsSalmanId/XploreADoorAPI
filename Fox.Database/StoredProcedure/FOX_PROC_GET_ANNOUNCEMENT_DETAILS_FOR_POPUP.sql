-- AUTHOR:  <MUHAMMAD SALMAN>                                
-- CREATE DATE: <CREATE DATE, 09/24/2022>                                
-- DESCRIPTION: <GET LIST OF ANNOUNCEMENT DETAILS>                         
-- [FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_POPUP] 1011163,'103'      
                    
CREATE PROCEDURE [DBO].[FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_POPUP]                                             
(                                               
  @PRACTICE_CODE BIGINT,                                    
  @ROLE_ID BIGINT                        
)                                                  
AS                                                  
BEGIN                   
              
  SELECT AP.ANNOUNCEMENT_ID, AROLE.ROLE_ID,ltrim(left(AP.ANNOUNCEMENT_DETAILS, 600)) As ANNOUNCEMENT_DETAILS ,     
  AP.CREATED_DATE, AP.ANNOUNCEMENT_TITLE, AROLE.ROLE_NAME, AROLE.ROLE_ID, AP.ANNOUNCEMENT_DATE_TO, AP.ANNOUNCEMENT_DATE_FROM                    
  FROM FOX_TBL_ANNOUNCEMENT AP   WITH (NOLOCK)                        
  INNER JOIN FOX_TBL_ANNOUNCEMENT_ROLE AROLE WITH (NOLOCK) ON AROLE.ANNOUNCEMENT_ID = AP.ANNOUNCEMENT_ID                                    
  WHERE                      
  AROLE.ROLE_ID = @ROLE_ID   
  AND                           
   CONVERT(DATE, AP.ANNOUNCEMENT_DATE_FROM) = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))                
   AND CONVERT(DATE, AP.ANNOUNCEMENT_DATE_TO) = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))                                 
  AND AP.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(AP.DELETED, 0) = 0                 
  AND ISNULL(AROLE.DELETED, 0) = 0                               
  ORDER BY AP.CREATED_DATE DESC                             
END 

------------------------------------------------------------------------------------------------------------------------------------------------------------------------