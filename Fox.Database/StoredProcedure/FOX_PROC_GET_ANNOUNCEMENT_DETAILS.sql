-- AUTHOR:  <MUHAMMAD SALMAN>                                                        
-- CREATE DATE: <CREATE DATE, 09/24/2022>                                                        
-- DESCRIPTION: <GET LIST OF ANNOUNCEMENT DETAILS>                              
-- [FOX_PROC_GET_ANNOUNCEMENT_DETAILS] 1011163, '10/07/2022', '10/07/2022' ,''                     
-- [FOX_PROC_GET_ANNOUNCEMENT_DETAILS] 1011163, '', '' ,''                         
-- [FOX_PROC_GET_ANNOUNCEMENT_DETAILS] 1011163, '', '' ,',54810154'                         
-- [FOX_PROC_GET_ANNOUNCEMENT_DETAILS] 1011163, '10/7/2022', '10/15/2022' ,'54810141,54810142,54810143,54810144,54810145,54810154,54810224,114,116,100,103,104,105,106,107,54410118,54410120,54810220,54810221'                                                            
CREATE PROCEDURE [DBO].[FOX_PROC_GET_ANNOUNCEMENT_DETAILS]                                                                     
(                                                                       
  @PRACTICE_CODE BIGINT,                                                 
  @ANNOUNCEMENT_DATE_FROM VARCHAR(50),                                                  
  @ANNOUNCEMENT_DATE_TO VARCHAR(50),                                                 
  @ROLE_ID VARCHAR(MAX)                                                  
)                                                                          
AS                                                                          
BEGIN               
              
  IF(@ROLE_ID = '')              
  BEGIN              
 SET @ROLE_ID = NULL              
  END            
  ELSE            
  BEGIN            
 SET @ROLE_ID = @ROLE_ID            
  END            
                 
  IF(@ANNOUNCEMENT_DATE_FROM = '')              
  BEGIN              
 SET @ANNOUNCEMENT_DATE_FROM = NULL              
  END            
  ELSE            
  BEGIN            
 SET @ANNOUNCEMENT_DATE_FROM = @ANNOUNCEMENT_DATE_FROM             
  END              
            
  IF(@ANNOUNCEMENT_DATE_TO = '')              
  BEGIN              
 SET @ANNOUNCEMENT_DATE_TO = NULL              
  END            
  ELSE            
  BEGIN            
 SET @ANNOUNCEMENT_DATE_TO = @ANNOUNCEMENT_DATE_TO            
  END      
                        
  IF OBJECT_ID('TEMPDB.DBO.#TEMP_ANNOUNCEMENT_DETAILS', 'U') IS NOT NULL DROP TABLE TEMP_ANNOUNCEMENT_DETAILS;                          
  SELECT AP.ANNOUNCEMENT_ID, AROLE.ANNOUNCEMENT_ROLE_ID, AP.ANNOUNCEMENT_DETAILS, AP.CREATED_DATE, AP.ANNOUNCEMENT_TITLE,              
  STUFF((SELECT ', ' + AROLE.ROLE_NAME                     
          FROM FOX_TBL_ANNOUNCEMENT_ROLE AS AROLE WITH (NOLOCK)                     
          WHERE AROLE.ANNOUNCEMENT_ID = AP.ANNOUNCEMENT_ID AND ISNULL(AROLE.DELETED, 0) = 0 AND AROLE.PRACTICE_CODE = @PRACTICE_CODE                    
          FOR XML PATH('')), 1, 1, '') [ROLE_NAME]  , AROLE.ROLE_ID,                         
  AP.ANNOUNCEMENT_DATE_FROM, AP.ANNOUNCEMENT_DATE_TO                    
  INTO #TEMP_ANNOUNCEMENT_DETAILS                           
  FROM FOX_TBL_ANNOUNCEMENT AP   WITH (NOLOCK)                    
  INNER JOIN FOX_TBL_ANNOUNCEMENT_ROLE AROLE WITH (NOLOCK) ON AROLE.ANNOUNCEMENT_ID = AP.ANNOUNCEMENT_ID                                                            
  WHERE                                  
  @ROLE_ID IS NULL OR AROLE.ROLE_ID IN (SELECT VAL FROM [DBO].F_SPLIT(@ROLE_ID , ','))                                                                                                                                                    
   AND AP.PRACTICE_CODE = @PRACTICE_CODE                                      
  AND ISNULL(AP.DELETED, 0) = 0  AND ISNULL(AROLE.DELETED, 0) = 0                
 ORDER BY AP.CREATED_DATE DESC                        
                  
 SELECT DISTINCT                  
 ANNOUNCEMENT_ID, [ROLE_NAME], ANNOUNCEMENT_DETAILS, ANNOUNCEMENT_TITLE, ANNOUNCEMENT_DATE_FROM, ANNOUNCEMENT_DATE_TO              
 FROM #TEMP_ANNOUNCEMENT_DETAILS  AS TEMP            
 WHERE                             
   (                                                                                         
   @ANNOUNCEMENT_DATE_FROM IS NULL OR (CONVERT(DATE, TEMP.ANNOUNCEMENT_DATE_FROM) >= CONVERT(DATE, @ANNOUNCEMENT_DATE_FROM))                                 
   )                                   
   AND(                                                                                         
   @ANNOUNCEMENT_DATE_TO IS NULL OR (CONVERT(DATE, TEMP.ANNOUNCEMENT_DATE_TO) <= CONVERT(DATE, @ANNOUNCEMENT_DATE_TO))                                
   )             
   AND ISNULL(ROLE_NAME, '')  <> ''      
   ORDER BY ANNOUNCEMENT_DATE_FROM ASC    
            
END 