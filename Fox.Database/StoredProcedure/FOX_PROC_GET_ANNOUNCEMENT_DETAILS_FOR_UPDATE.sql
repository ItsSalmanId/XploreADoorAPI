-- AUTHOR:  <MUHAMMAD SALMAN>                      
-- CREATE DATE: <CREATE DATE, 09/24/2022>                      
-- DESCRIPTION: <GET DETAILS OF ANNOUNCEMENT>               
-- [FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_UPDATE] 1011163, '2022-10-01 00:00:00.000', '2022-10-01 00:00:00.000', ',54810141'              
CREATE PROCEDURE [DBO].[FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_UPDATE]                                   
(                                     
  @PRACTICE_CODE BIGINT,     
  @ANNOUNCEMENT_ID BIGINT                              
)                                        
AS                                        
BEGIN         
    
  SELECT *         
  FROM FOX_TBL_ANNOUNCEMENT AP   WITH (NOLOCK)              
  WHERE      
     AP.ANNOUNCEMENT_ID = @ANNOUNCEMENT_ID     
     AND AP.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(AP.DELETED, 0) = 0                
END    