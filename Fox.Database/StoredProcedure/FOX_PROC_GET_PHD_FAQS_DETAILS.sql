-- AUTHOR:  <MUHAMMAD SALMAN>        
-- CREATE DATE: <CREATE DATE, 09/26/2022>        
-- DESCRIPTION: <GET DETAILS OF FAQS FOR SMART SEARCH>        
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PHD_FAQS_DETAILS]         
 (                                                                                     
 @PRACTICE_CODE BIGINT                                                                            
 ,@SEARCH_TEXT VARCHAR(30)                                                                                                                                                                                                                            
 )                                                                                  
AS                                                                                  
BEGIN                                                                                                                                                    
  SELECT * FROM FOX_TBL_PHD_FAQS_DETAILS  WITH (NOLOCK)    
  WHERE           
  PRACTICE_CODE = @PRACTICE_CODE       
  AND ISNULL(DELETED, 0) = 0     
  AND ISNULL(QUESTIONS, '') LIKE '%' + @SEARCH_TEXT + '%'                                                                             
END 
