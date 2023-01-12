-- =============================================              
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>              
-- CREATE DATE: <CREATE DATE, 26/04/2018>              
-- DESCRIPTION: <GET FEEDBACK CALLER_ LIST> 
-- MODIFIED BY: <MUHAMMAD TASEER IQBAL>   
-- MODIFIED DATE: <CREATE DATE, 01/12/2023>  
-- =============================================    
-- EXEC FOX_PROC_GET_FEEDBACK_CALLER_LIST 1011163,54410118                          
CREATE PROCEDURE [DBO].[FOX_PROC_GET_FEEDBACK_CALLER_LIST]               
  @PRACTICE_CODE BIGINT,              
  @ROLE_ID BIGINT                         
AS              
BEGIN                                  
 SELECT USER_NAME, (FIRST_NAME + ' '+ LAST_NAME) AS NAME, EMAIL            
 FROM FOX_TBL_APPLICATION_USER  with (nolock)            
 WHERE              
      ROLE_ID = @ROLE_ID               
      AND ISNULL(DELETED, 0) = 0              
   AND PRACTICE_CODE = @PRACTICE_CODE          
    AND IS_ACTIVE = 1      
   UNION                           
   SELECT  DISTINCT phd.CREATED_BY AS USER_NAME, (AU.FIRST_NAME + ' '+ AU.LAST_NAME) AS NAME , EMAIL            
   FROM FOX_TBL_PHD_CALL_DETAILS phd with (nolock)               
   left JOIN FOX_TBL_APPLICATION_USER  as AU on  phd.CREATED_BY = AU.USER_NAME AND AU.PRACTICE_CODE = @PRACTICE_CODE               
   WHERE              
      ISNULL(phd.DELETED, 0) = 0              
   AND phd.PRACTICE_CODE = @PRACTICE_CODE          
    AND ISNULL(AU.DELETED,0)= 0         
  AND AU.IS_ACTIVE = 1                
 ORDER BY NAME ASC              
END 