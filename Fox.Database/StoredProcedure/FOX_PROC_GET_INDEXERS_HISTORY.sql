IF (OBJECT_ID('FOX_PROC_GET_INDEXERS_HISTORY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXERS_HISTORY 
GO    
  -- =============================================                  
-- Author:  <Author,Abdur Rafay>                  
-- Create date: <Create Date,06/22/2021>                  
-- DESCRIPTION: <GET INDEXERS>              
            
-- [FOX_PROC_GET_INDEXERS_HISTORY] 1011163 ,1,10,'08/16/2021','8/16/2021 12:19:07 PM'          
            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INDEXERS_HISTORY](            
@PRACTICE_CODE BIGINT,      
@CURRENT_PAGE    INT,                                                                 
@RECORD_PER_PAGE INT,      
@SEARCH_TEXT     VARCHAR(100),      
@DATE   DATETIME)              
AS              
     BEGIN              
      
SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                                                
DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE                         
DECLARE @TOATL_PAGESUDM FLOAT              
      
SELECT @TOATL_PAGESUDM = COUNT(*)      
  FROM FOX_TBL_ACTIVE_INDEXER_HISTORY IND       
 Left JOIN FOX_TBL_APPLICATION_USER USR on IND.INDEXER = USR.USER_NAME       
            AND USR.PRACTICE_CODE = @PRACTICE_CODE       
            AND ISNULL(USR.DELETED, 0) = 0      
   WHERE       
   ( USR.FIRST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR  USR.LAST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR ( USR.FIRST_NAME+' '+ USR.LAST_NAME) LIKE '%' + @SEARCH_TEXT+'%'       
   OR IND.WORK_ID LIKE '%' + @SEARCH_TEXT+'%'      
   OR CONVERT(VARCHAR, IND.CREATED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'                             
   OR CONVERT(VARCHAR, IND.CREATED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'      
   )      
   AND IND.PRACTICE_CODE = @PRACTICE_CODE      
   AND IND.WORK_ID <> 0    
   AND (@DATE IS NULL  OR CAST(IND.CREATED_DATE AS DATE) BETWEEN CAST(@DATE AS DATE) AND CAST(@DATE AS DATE))      
      
   AND ISNULL(IND.DELETED, 0) = 0      
      
IF(@RECORD_PER_PAGE = 0)                                                                
     BEGIN                                                       
      SET @RECORD_PER_PAGE = @TOATL_PAGESUDM                                                                
     END                                                                
     ELSE                                                                
     BEGIN                                
      SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                                                
     END                                                                
                                                                
    --                                                               
    IF(@RECORD_PER_PAGE <> 0)                                    BEGIN                                                              
    DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM                                                                
    SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)        
      
   SELECT USR.FIRST_NAME, USR.LAST_NAME, USR.FIRST_NAME+' '+USR.LAST_NAME AS FULL_NAME, IND.* , CONVERT(VARCHAR, IND.CREATED_DATE) AS CREATED_DATE_STR,                                                                
     @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                         
     @TOTAL_RECORDS TOTAL_RECORDS       
  FROM FOX_TBL_ACTIVE_INDEXER_HISTORY IND       
 Left JOIN FOX_TBL_APPLICATION_USER USR on IND.INDEXER = USR.USER_NAME       
            AND USR.PRACTICE_CODE = @PRACTICE_CODE       
            AND ISNULL(USR.DELETED, 0) = 0      
   WHERE       
   ( USR.FIRST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR  USR.LAST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR ( USR.FIRST_NAME+' '+ USR.LAST_NAME) LIKE '%' + @SEARCH_TEXT+'%'       
   OR IND.WORK_ID LIKE '%' + @SEARCH_TEXT+'%'      
   OR CONVERT(VARCHAR, IND.CREATED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'                             
   OR CONVERT(VARCHAR, IND.CREATED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'      
   )      
   AND IND.PRACTICE_CODE = @PRACTICE_CODE       
   AND IND.WORK_ID <> 0    
   AND (@DATE IS NULL  OR CAST(IND.CREATED_DATE AS DATE) BETWEEN CAST(@DATE AS DATE) AND CAST(@DATE AS DATE))      
   AND ISNULL(IND.DELETED, 0) = 0      
   ORDER BY FULL_NAME ASC      
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY          
   END      
     END;   
  
