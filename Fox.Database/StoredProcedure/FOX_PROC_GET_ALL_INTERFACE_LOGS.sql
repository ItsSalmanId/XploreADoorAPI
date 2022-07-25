IF (OBJECT_ID('FOX_PROC_GET_ALL_INTERFACE_LOGS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ALL_INTERFACE_LOGS
GO
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ALL_INTERFACE_LOGS]     
 @PRACTICE_CODE        BIGINT,    
 @OPTION  VARCHAR(20),                          
 @SEARCH_STRING        VARCHAR(100),                 
 @CURRENT_PAGE         INT,                 
 @RECORD_PER_PAGE      INT,    
 @SORT_BY VARCHAR(20),    
 @SORT_ORDER VARCHAR(10),    
 @PATIENT_ACCOUNT BIGINT    
AS    
BEGIN    
    
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1    
    
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE    
 DECLARE @TOATL_PAGESUDM FLOAT    
 DECLARE @DOB DATETIME    
    
 IF ISDATE(@SEARCH_STRING) = 1    
 BEGIN    
  SET @DOB = CONVERT(DATETIME, @SEARCH_STRING)    
 END    
 ELSE    
 BEGIN    
  SET @DOB = NULL    
 END    
 SELECT @TOATL_PAGESUDM = COUNT(*)    
 FROM FOX_TBL_INTERFACE_LOG log  
 LEFT JOIN FOX_TBL_INTERFACE_SYNCH synch ON synch.FOX_INTERFACE_SYNCH_ID = log.FOX_INTERFACE_SYNCH_ID  
                                                       AND isnull(synch.DELETED, 0) = 0  
                                                       AND synch.PRACTICE_CODE = @PRACTICE_CODE   
 WHERE   ISNULL(log.DELETED, 0) = 0     
   AND log.PRACTICE_CODE = @PRACTICE_CODE    
   AND log.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
    
   AND    
   (    
       @OPTION = 'ALL'    
     AND    
      (    
     (log.ERROR LIKE '%'+@SEARCH_STRING+'%'   
    OR  
     synch.Work_ID LIKE '%'+@SEARCH_STRING+'%'     
  OR  
  synch.APPLICATION  LIKE '%'+@SEARCH_STRING+'%'     
  )  
     OR    
     (    
       @DOB IS NOT NULL    
       AND convert(date, log.CREATED_DATE) = CONVERT(date, @DOB)    
     )    
      )    
    OR    
    (    
      @OPTION = 'INCOMMING'    
      AND    
      (    
      (log.ERROR LIKE '%'+@SEARCH_STRING+'%'   
    OR  
     synch.Work_ID LIKE '%'+@SEARCH_STRING+'%'     
  OR  
  synch.APPLICATION  LIKE '%'+@SEARCH_STRING+'%'     
  )  
  AND log.IS_INCOMMING = 1                
     OR    
     (    
       @DOB IS NOT NULL    
       AND convert(date, log.CREATED_DATE) = CONVERT(date, @DOB)    
     )    
      )    
                )    
    OR    
    (    
      @OPTION = 'OUTGOING'    
      AND    
      (    
       
     (log.ERROR LIKE '%'+@SEARCH_STRING+'%'   
    OR  
     synch.Work_ID LIKE '%'+@SEARCH_STRING+'%'     
  OR  
  synch.APPLICATION  LIKE '%'+@SEARCH_STRING+'%'     
  )  
  AND log.IS_OUTGOING = 1                
     OR    
     (    
       @DOB IS NOT NULL    
       AND convert(date, log.CREATED_DATE) = CONVERT(date, @DOB)    
     )    
      )    
                )    
   )    
    
 IF (@RECORD_PER_PAGE = 0)    
  BEGIN    
   SET @RECORD_PER_PAGE = @TOATL_PAGESUDM    
  END    
 ELSE    
  BEGIN    
   SET @RECORD_PER_PAGE = @RECORD_PER_PAGE    
  END    
    
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM    
    
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)    
    
 SELECT *,                 
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                 
                @TOTAL_RECORDS AS TOTAL_RECORDS                
         FROM                
         (                
             SELECT --*  
   log.FOX_INTERFACE_LOG_ID AS FOX_INTERFACE_LOG_ID,  
   log.FOX_INTERFACE_SYNCH_ID AS FOX_INTERFACE_SYNCH_ID,   
   synch.Work_ID AS Work_ID,   
   log.PRACTICE_CODE AS PRACTICE_CODE,   
   log.PATIENT_ACCOUNT AS PATIENT_ACCOUNT,   
   '"' + log.ERROR + '"' AS ERROR,   
   '"' + log.ACK + '"' AS ACK,   
   log.IS_ERROR AS IS_ERROR,   
   '"' + log.LOG_MESSAGE + '"' AS LOG_MESSAGE,   
   log.IS_INCOMMING AS IS_INCOMMING,   
   log.IS_OUTGOING AS IS_OUTGOING,   
   log.CREATED_BY AS CREATED_BY,   
   log.CREATED_DATE AS CREATED_DATE,   
   convert(varchar,log.CREATED_DATE) AS Created_Date_Str,  
   log.MODIFIED_BY AS MODIFIED_BY,   
   log.MODIFIED_DATE AS MODIFIED_DATE,   
   log.DELETED AS DELETED,  
   synch.APPLICATION,  
   ROW_NUMBER() OVER ( ORDER BY log.CREATED_DATE DESC ) AS ACTIVEROW  
   FROM FOX_TBL_INTERFACE_LOG log  
            LEFT JOIN FOX_TBL_INTERFACE_SYNCH synch ON log.FOX_INTERFACE_SYNCH_ID = synch.FOX_INTERFACE_SYNCH_ID  
              AND isnull(synch.DELETED, 0) = 0  
                                                       AND synch.PRACTICE_CODE = @PRACTICE_CODE   
    WHERE   ISNULL(log.DELETED, 0) = 0     
     AND log.PRACTICE_CODE = @PRACTICE_CODE    
     AND log.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
    
     AND    
     (    
      @OPTION = 'ALL'    
      AND    
      (    
       
     (log.ERROR LIKE '%'+@SEARCH_STRING+'%'   
    OR  
     synch.Work_ID LIKE '%'+@SEARCH_STRING+'%'     
  OR  
  synch.APPLICATION  LIKE '%'+@SEARCH_STRING+'%'     
  )  
  OR    
     (    
       @DOB IS NOT NULL    
       AND convert(date, log.CREATED_DATE) = CONVERT(date, @DOB)    
     )    
      )    
    OR    
    (    
      @OPTION = 'INCOMMING'    
      AND    
      (    
       
     (log.ERROR LIKE '%'+@SEARCH_STRING+'%'   
    OR  
     synch.Work_ID LIKE '%'+@SEARCH_STRING+'%'     
  OR  
  synch.APPLICATION  LIKE '%'+@SEARCH_STRING+'%'     
  )  
  AND log.IS_INCOMMING = 1                
     OR    
     (    
       @DOB IS NOT NULL    
       AND convert(date, log.CREATED_DATE) = CONVERT(date, @DOB)    
     )    
      )    
                )    
    OR    
    (    
      @OPTION = 'OUTGOING'    
      AND    
      (    
       
     (log.ERROR LIKE '%'+@SEARCH_STRING+'%'   
    OR  
     synch.Work_ID LIKE '%'+@SEARCH_STRING+'%'     
  OR  
  synch.APPLICATION  LIKE '%'+@SEARCH_STRING+'%'     
  )  
  AND log.IS_OUTGOING = 1                
     OR    
     (    
       @DOB IS NOT NULL    
       AND convert(date, log.CREATED_DATE) = CONVERT(date, @DOB)    
     )    
      )    
                )    
     )            
         ) AS FOX_TBL_INTERFACE_LOG    
         
 ORDER BY    
  CASE WHEN @SORT_BY = 'CREATED_DATE' AND @SORT_ORDER = 'ASC' THEN CREATED_DATE END ASC,    
  CASE WHEN @SORT_BY = 'CREATED_DATE' AND @SORT_ORDER = 'DESC' THEN CREATED_DATE END DESC,    
  CASE WHEN @SORT_BY = 'MODIFIED_DATE' AND @SORT_ORDER = 'ASC' THEN MODIFIED_DATE END ASC,    
  CASE WHEN @SORT_BY = 'MODIFIED_DATE' AND @SORT_ORDER = 'DESC' THEN MODIFIED_DATE END    
 DESC OFFSET @START_FROM ROWS    
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY    
    
END 





