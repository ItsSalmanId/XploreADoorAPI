IF (OBJECT_ID('FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE  
GO 
--  [dbo].[FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE] 1,10,'','','','External_User_Ord_Ref_Source','1163testing' , 0                               
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE]                                                             
(                                        
  @CURRENT_PAGE INT                                                  
 ,@RECORD_PER_PAGE INT                                                  
 ,@SEARCH_TEXT VARCHAR(30)                                                  
 ,@SORT_BY VARCHAR(50)                                                  
 ,@SORT_ORDER VARCHAR(5)                      
 ,@USER_TYPE VARCHAR(100)                      
 ,@USER_NAME VARCHAR(100)                      
 ,@IsSigned BIT               
 ,@UserEmail VARCHAR(500)                                      
 )                                        
AS                                        
BEGIN                
 --declare @CURRENT_PAGE INT   = 1                                              
 --declare @RECORD_PER_PAGE INT = 10                                            
 --declare @SEARCH_TEXT VARCHAR(30)= '08/02/2021'                                                  
 --declare @SORT_BY VARCHAR(50)      = ''                                            
 --declare @SORT_ORDER VARCHAR(5)= ''                      
 --declare @USER_TYPE VARCHAR(100) = 'External_User_Ord_Ref_Source'                      
 --declare @USER_NAME VARCHAR(100)   =  '1163TESTING'                  
 --declare @IsSigned BIT    = 0              
 --declare @UserEmail VARCHAR(500) = 'abdurrafay@mtbc.com'                       
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                    
                                     
                               
IF (@RECORD_PER_PAGE = 0)                                        
 BEGIN                                        
  SELECT @RECORD_PER_PAGE = COUNT(*)                                        
  FROM FOX_TBL_WORK_QUEUE                                        
 END                                        
 ELSE                                        
 BEGIN                           
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                        
 END                                                                                         
                                        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                        
                                        
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                                        
 DECLARE @TOATL_PAGESUDM FLOAT                                        
                 
     IF(@IsSigned = 1)                      
    BEGIN                                       
 SELECT @TOATL_PAGESUDM = COUNT(*)                              
 FROM FOX_TBL_WORK_QUEUE w                         
 LEFT JOIN Patient p ON w.patient_account = p.Patient_Account                       
  LEFT JOIN FOX_TBL_APPLICATION_USER a ON w.CREATED_BY = a.USER_NAME                      
  Where isnull(w.IsSigned,0)=0 and isnull(w.IS_DENIED,0) = 0 and w.REFERRAL_EMAIL_SENT_TO = @UserEmail                 
     AND                  
  (                  
  p.First_Name  LIKE '%' + @SEARCH_TEXT + '%'                    
             OR p.Last_Name LIKE '%' + @SEARCH_TEXT + '%'                  
                             
      OR w.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                  
              OR w.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                  
      OR CONVERT(VARCHAR(30), w.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                   
      OR w.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                   
       OR '' LIKE '%' + @SEARCH_TEXT + '%'                      
  )                     
   END               
                                                    
 ELSE                            
 BEGIN                      
 SELECT @TOATL_PAGESUDM = COUNT(*)                     
  FROM FOX_TBL_WORK_QUEUE w                         
 LEFT JOIN Patient p ON w.patient_account = p.Patient_Account                       
  LEFT JOIN FOX_TBL_APPLICATION_USER a ON w.CREATED_BY = a.USER_NAME                      
  Where w.CREATED_BY = @USER_NAME               
    AND          
  (                  
    p.First_Name  LIKE '%' + @SEARCH_TEXT + '%'                    
               OR p.Last_Name LIKE '%' + @SEARCH_TEXT + '%'                   
      OR w.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                  
              OR w.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                  
      OR CONVERT(VARCHAR(30), w.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                   
      OR w.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                   
      OR '' LIKE '%' + @SEARCH_TEXT + '%'                        
  )               
  END                                         
                                        
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                                        
                                        
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                                        
   IF(@IsSigned = 1)                             
 BEGIN                                       
 SELECT *                                        
  ,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                                        
  ,@TOTAL_RECORDS TOTAL_RECORDS                                        
 FROM                                                                              
    (                    
   SELECT p.First_Name, p.Last_Name, w.UNIQUE_ID, w.SORCE_NAME, w.IsSigned, w.REFERRAL_EMAIL_SENT_TO,                    
   w.RECEIVE_DATE, a.USER_TYPE, a.USER_NAME, w.CREATED_DATE                      
  FROM FOX_TBL_WORK_QUEUE w                         
 LEFT JOIN Patient p ON w.patient_account = p.Patient_Account                       
  LEFT JOIN FOX_TBL_APPLICATION_USER a ON w.CREATED_BY = a.USER_NAME                      
  Where isnull(w.IsSigned,0)=0 and isnull(w.IS_DENIED,0) = 0 and w.REFERRAL_EMAIL_SENT_TO = @UserEmail                 
     AND                  
  (                  
  p.First_Name  LIKE '%' + @SEARCH_TEXT + '%'                    
             OR p.Last_Name LIKE '%' + @SEARCH_TEXT + '%'                      
      OR w.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                  
              OR w.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                  
      OR CONVERT(VARCHAR(30), w.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                   
      OR w.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                   
       OR '' LIKE '%' + @SEARCH_TEXT + '%'                      
  )  )               
  AS WORK_QUEUE                                        
 ORDER BY CREATED_DATE DESC OFFSET @START_FROM ROWS               
  END                                                                            
   ELSE                                            
 BEGIN                       
 SELECT *,                                                                               
     @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                                       
     @TOTAL_RECORDS TOTAL_RECORDS                                                                              
    FROM                                                                              
    (                    
  SELECT                       
p.First_Name, p.Last_Name, w.IsSigned, w.REFERRAL_EMAIL_SENT_TO, w.UNIQUE_ID, w.SORCE_NAME, w.RECEIVE_DATE,a.USER_TYPE,a.USER_NAME, w.CREATED_DATE                      
  FROM FOX_TBL_WORK_QUEUE w                         
 LEFT JOIN Patient p ON w.patient_account = p.Patient_Account                       
  LEFT JOIN FOX_TBL_APPLICATION_USER a ON w.CREATED_BY = a.USER_NAME                      
  Where w.CREATED_BY = @USER_NAME               
    AND                  
  (                  
    p.First_Name  LIKE '%' + @SEARCH_TEXT + '%'                    
            OR p.Last_Name LIKE '%' + @SEARCH_TEXT + '%'                     
      OR w.REFERRAL_EMAIL_SENT_TO LIKE '%' + @SEARCH_TEXT + '%'                  
              OR w.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                  
      OR CONVERT(VARCHAR(30), w.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                   
      OR w.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                   
      OR '' LIKE '%' + @SEARCH_TEXT + '%'                        
  )                 
  ) AS WORK_QUEUE                    
ORDER BY CREATED_DATE DESC               
                 
     OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                                                                              
  print   @START_FROM              
    END                                 
END 