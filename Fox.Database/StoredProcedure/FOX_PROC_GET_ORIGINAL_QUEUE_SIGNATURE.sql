 --------------------------------------------------------
----- Created By: Muhammad Taseer Iqbal
----- Created Date: 01/07/2023
----- Modified By: Muhammad Taseer Iqbal
----- Modified Date: 10/29/2022  
      
--  [DBO].[FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE] 1,10,'','','','EXTERNAL_USER_ORD_REF_SOURCE','1163TESTING' , 0                                 
CREATE PROCEDURE [DBO].[FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE]                                                               
(                                          
  @CURRENT_PAGE INT                                                    
 ,@RECORD_PER_PAGE INT                                                    
 ,@SEARCH_TEXT VARCHAR(30)                                                    
 ,@SORT_BY VARCHAR(50)                                                    
 ,@SORT_ORDER VARCHAR(5)                        
 ,@USER_TYPE VARCHAR(100)                        
 ,@USER_NAME VARCHAR(100)                        
 ,@ISSIGNED BIT                 
 ,@USEREMAIL VARCHAR(500)                                        
 )                                          
AS                                          
BEGIN                  
 --DECLARE @CURRENT_PAGE INT   = 1                                                
 --DECLARE @RECORD_PER_PAGE INT = 10                                              
 --DECLARE @SEARCH_TEXT VARCHAR(30)= '08/02/2021'                                                    
 --DECLARE @SORT_BY VARCHAR(50)      = ''                                              
 --DECLARE @SORT_ORDER VARCHAR(5)= ''                        
 --DECLARE @USER_TYPE VARCHAR(100) = 'EXTERNAL_USER_ORD_REF_SOURCE'                        
 --DECLARE @USER_NAME VARCHAR(100)   =  '1163TESTING'                    
 --DECLARE @ISSIGNED BIT    = 0                
 --DECLARE @USEREMAIL VARCHAR(500) = 'ABDURRAFAY@MTBC.COM'                         
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                      
                                       
                                 
IF (@RECORD_PER_PAGE = 0)                                          
 BEGIN                                          
  SELECT @RECORD_PER_PAGE = COUNT(*)                                          
  FROM FOX_TBL_WORK_QUEUE  WITH (NOLOCK)                                        
 END                                          
 ELSE                                          
 BEGIN                             
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                          
 END                                                                                           
                                          
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                          
                                          
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                                          
 DECLARE @TOATL_PAGESUDM FLOAT                                          
                   
     IF(@ISSIGNED = 1)                        
    BEGIN                                         
 SELECT @TOATL_PAGESUDM = COUNT(*)                                
 FROM FOX_TBL_WORK_QUEUE W WITH (NOLOCK)                
 LEFT JOIN PATIENT P WITH (NOLOCK) ON W.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT                         
  LEFT JOIN  FOX_TBL_APPLICATION_USER A WITH (NOLOCK) ON W.CREATED_BY = A.USER_NAME                        
  WHERE ISNULL(W.ISSIGNED,0)=0 AND ISNULL(W.IS_DENIED,0) = 0 AND W.REFERRAL_EMAIL_SENT_TO = @USEREMAIL                   
     AND                    
  (                    
  P.FIRST_NAME  LIKE '%' + @SEARCH_TEXT + '%'                      
             OR P.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'                    
                               
      OR W.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                    
              OR W.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                    
      OR CONVERT(VARCHAR(30), W.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                     
      OR W.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                     
       OR '' LIKE '%' + @SEARCH_TEXT + '%'                        
  )                       
   END                 
                                   
 ELSE                                              
 BEGIN                        
 SELECT @TOATL_PAGESUDM = COUNT(*)                       
  FROM FOX_TBL_WORK_QUEUE W         WITH (NOLOCK)                    
 LEFT JOIN PATIENT P WITH (NOLOCK) ON W.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT                         
  LEFT JOIN  FOX_TBL_APPLICATION_USER A WITH (NOLOCK) ON W.CREATED_BY = A.USER_NAME                        
  WHERE W.CREATED_BY = @USER_NAME                 
    AND            
  (                    
    P.FIRST_NAME  LIKE '%' + @SEARCH_TEXT + '%'                      
               OR P.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'                     
      OR W.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                    
              OR W.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                    
      OR CONVERT(VARCHAR(30), W.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                     
      OR W.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                     
      OR '' LIKE '%' + @SEARCH_TEXT + '%'                          
  )                 
  END                                           
                                          
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                                          
                                          
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                                          
   IF(@ISSIGNED = 1)                               
 BEGIN                                         
 SELECT *                                          
  ,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                                          
  ,@TOTAL_RECORDS TOTAL_RECORDS                                          
 FROM                                                                                
    (                      
   SELECT P.FIRST_NAME, P.LAST_NAME, W.UNIQUE_ID, W.SORCE_NAME, W.ISSIGNED, W.REFERRAL_EMAIL_SENT_TO,                      
   W.RECEIVE_DATE, A.USER_TYPE, A.USER_NAME, W.CREATED_DATE                        
  FROM FOX_TBL_WORK_QUEUE W     WITH (NOLOCK)                        
 LEFT JOIN PATIENT P WITH (NOLOCK) ON W.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT                         
  LEFT JOIN FOX_TBL_APPLICATION_USER A WITH (NOLOCK) ON W.CREATED_BY = A.USER_NAME                        
  WHERE ISNULL(W.ISSIGNED,0)=0 AND ISNULL(W.IS_DENIED,0) = 0 AND W.REFERRAL_EMAIL_SENT_TO = @USEREMAIL                   
     AND                    
  (                    
  P.FIRST_NAME  LIKE '%' + @SEARCH_TEXT + '%'                      
             OR P.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'                        
      OR W.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                    
              OR W.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                    
      OR CONVERT(VARCHAR(30), W.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                     
      OR W.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                     
       OR '' LIKE '%' + @SEARCH_TEXT + '%'                        
  )  )                 
  AS WORK_QUEUE                                          
 --ORDER BY CREATED_DATE DESC OFFSET @START_FROM ROWS              
  ORDER BY CREATED_DATE DESC OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY PRINT @START_FROM        
  END                                                                              
   ELSE                                              
 BEGIN                         
 SELECT *,                                                                                 
     @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                                         
     @TOTAL_RECORDS TOTAL_RECORDS                                                                                
    FROM                                                                                
    (                      
  SELECT                         
P.FIRST_NAME, P.LAST_NAME, W.ISSIGNED, W.REFERRAL_EMAIL_SENT_TO, W.UNIQUE_ID, W.SORCE_NAME, W.RECEIVE_DATE,A.USER_TYPE,A.USER_NAME, W.CREATED_DATE                        
  FROM FOX_TBL_WORK_QUEUE W WITH (NOLOCK)                          
 LEFT JOIN PATIENT P WITH (NOLOCK) ON W.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT                         
  LEFT JOIN FOX_TBL_APPLICATION_USER A WITH (NOLOCK) ON W.CREATED_BY = A.USER_NAME                        
  WHERE W.CREATED_BY = @USER_NAME                 
    AND                    
  (                    
    P.FIRST_NAME  LIKE '%' + @SEARCH_TEXT + '%'                      
               OR P.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'                       
      OR W.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                    
              OR W.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                    
      OR CONVERT(VARCHAR(30), W.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                     
      OR W.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                     
      OR '' LIKE '%' + @SEARCH_TEXT + '%'                          
  )                   
  ) AS WORK_QUEUE                      
ORDER BY CREATED_DATE DESC                 
                   
     OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                                                                                
  PRINT   @START_FROM                
    END                                   
END 