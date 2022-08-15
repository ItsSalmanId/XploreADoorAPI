IF (OBJECT_ID('FOX_PROC_GET_INDEXERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXERS 
GO 
-- =============================================                  
-- Author:  <Author,Abdur Rafay>                  
-- Create date: <Create Date,06/22/2021>                  
-- DESCRIPTION: <GET INDEXERS>              
            
-- [FOX_PROC_GET_INDEXERS] 1011163 ,1,10,'hassan raza'          
            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INDEXERS](            
@PRACTICE_CODE BIGINT,      
@CURRENT_PAGE    INT,                                                                 
@RECORD_PER_PAGE INT,      
@SEARCH_TEXT     VARCHAR(100))              
AS              
     BEGIN              
      
SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                                                
DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE                         
DECLARE @TOATL_PAGESUDM FLOAT              
      
SELECT @TOATL_PAGESUDM = COUNT(*)      
  FROM FOX_TBL_ACTIVE_INDEXER IND       
 Left JOIN FOX_TBL_APPLICATION_USER USR on IND.INDEXER = USR.USER_NAME       
            AND USR.ROLE_ID = 101       
            AND USR.PRACTICE_CODE = @PRACTICE_CODE       
            AND ISNULL(USR.DELETED, 0) = 0      
   WHERE       
   ( USR.FIRST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR  USR.LAST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR ( USR.FIRST_NAME+' '+ USR.LAST_NAME) LIKE '%' + @SEARCH_TEXT+'%' )      
   AND IND.PRACTICE_CODE = @PRACTICE_CODE        
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
      
   SELECT USR.FIRST_NAME, USR.LAST_NAME, IND.* ,                                                                 
     @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                         
     @TOTAL_RECORDS TOTAL_RECORDS       
   FROM FOX_TBL_ACTIVE_INDEXER IND      
   Left JOIN FOX_TBL_APPLICATION_USER USR on IND.INDEXER = USR.USER_NAME       
            AND USR.ROLE_ID = 101       
            AND USR.PRACTICE_CODE = @PRACTICE_CODE       
            AND ISNULL(USR.DELETED, 0) = 0      
   WHERE       
   ( USR.FIRST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR  USR.LAST_NAME LIKE '%' + @SEARCH_TEXT+'%'      
   OR ( USR.FIRST_NAME+' '+ USR.LAST_NAME) LIKE '%' + @SEARCH_TEXT+'%' )      
   AND IND.PRACTICE_CODE = @PRACTICE_CODE        
   AND ISNULL(IND.DELETED, 0) = 0      
   ORDER BY USR.FIRST_NAME ASC      
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY          
   END      
     END;   
  
    
