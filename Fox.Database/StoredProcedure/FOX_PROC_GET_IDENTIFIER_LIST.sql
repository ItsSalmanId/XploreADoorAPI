IF (OBJECT_ID('FOX_PROC_GET_IDENTIFIER_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_IDENTIFIER_LIST 
GO 
----7   
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
  -- =============================================                                                    
-- AUTHOR:  <DEVELOPER, Abdur Rafay>                                                    
-- CREATE DATE: <CREATE DATE, 29/01/2019>                                                    
-- DESCRIPTION: <GET IDENTIFIER LIST>  
     
--exec [FOX_PROC_GET_IDENTIFIER_LIST] '1011163','','','',544100,1,0,'Type','ASC'                 
CREATE procedure [FOX_PROC_GET_IDENTIFIER_LIST]-- 1011163,'','al001',1, 2000, 'code' , 'desc'                                            
@practice_code   BIGINT,   
@search_string   VARCHAR(100),   
@name         VARCHAR(50),  
@code     VARCHAR(100),  
@type     BIGINT NULL,  
@current_page    INT,   
@record_per_page INT,   
@sort_by         VARCHAR(50),   
@sort_order      VARCHAR(5)  
AS  
     BEGIN  
         IF(@SEARCH_STRING = '')  
             BEGIN  
                 SET @SEARCH_STRING = NULL;  
             END;  
             ELSE  
             BEGIN  
                 SET @SEARCH_STRING = '%'+@SEARCH_STRING+'%';  
             END;  
        IF(@code = '')  
             BEGIN  
                 SET @code = NULL;  
             END;  
             ELSE  
             BEGIN  
                 SET @code = '%'+@code+'%';  
             END;  
       IF(@NAME = '')  
             BEGIN  
                 SET @NAME = NULL;  
             END;  
             ELSE  
             BEGIN  
                 SET @NAME = '%'+@NAME+'%';  
             END;  
  IF(@TYPE = '')  
             BEGIN  
                 SET @TYPE = NULL;  
             END;  
             ELSE  
             BEGIN  
                 SET @TYPE = @TYPE;  
             END;               
         --          
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;  
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;  
         DECLARE @TOATL_PAGESUDM FLOAT;  
  
         --          
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT l.IDENTIFIER_ID)  
         FROM FOX_TBL_IDENTIFIER l  
   INNER JOIN  FOX_TBL_IDENTIFIER_TYPE i   
   ON l.IDENTIFIER_TYPE_ID = I.IDENTIFIER_TYPE_ID   
         WHERE(@SEARCH_STRING IS NULL  
               OR l.NAME LIKE '%'+@SEARCH_STRING+'%'  
               OR l.CODE LIKE '%'+@SEARCH_STRING+'%'  
      OR i.NAME LIKE '%'+@SEARCH_STRING+'%'  
               OR CONVERT(VARCHAR(10), l.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'  
               OR CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%')              
                       
              AND (l.DELETED = 0)  
              AND l.PRACTICE_CODE = @PRACTICE_CODE  
              AND (@code IS NULL  
                   OR l.CODE LIKE '%'+@code+'%')  
              AND (@NAME IS NULL  
                   OR l.NAME  LIKE '%'+@NAME+'%')  
     AND (@TYPE IS NULL  
                   OR i.IDENTIFIER_TYPE_ID  = @TYPE)  
         --              
         IF(@RECORD_PER_PAGE = 0)  
             BEGIN  
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;  
             END;  
             ELSE  
             BEGIN  
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;  
             END;  
  
         --          
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;  
  SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);  
  
         --          
         SELECT *,   
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,   
                @TOTAL_RECORDS AS TOTAL_RECORDS  
         FROM  
         (  
             SELECT DISTINCT   
                    l.IDENTIFIER_ID,  
     l.IDENTIFIER_TYPE_ID,   
    l.CODE,  
     l.NAME,  
     i.NAME AS IDENTIFIER_TYPE_NAME,  
     l.CODE_NAME,  
     l.DESCRIPTION,   
                    l.CREATED_BY,                                   
                    l.CREATED_DATE,   
     convert(varchar,l.CREATED_DATE) AS Created_Date_Str,                                        
                    l.MODIFIED_BY,                                   
                    l.MODIFIED_DATE,  
     convert(varchar,l.MODIFIED_DATE) AS Modified_Date_Str,   
                    l.DELETED,  
     ROW_NUMBER() OVER (          
      ORDER BY l.CREATED_DATE DESC          
      ) AS ACTIVEROW,  
     l.END_DATE,  
     l.START_DATE,  
     isnull(l.IS_ACTIVE,1) AS IS_ACTIVE,  
     CASE WHEN l.IS_ACTIVE = 1  THEN 'NO' WHEN l.IS_ACTIVE = 0 THEN 'YES' else 'NO' END as Inactive                   
             FROM FOX_TBL_IDENTIFIER l  
    INNER JOIN  FOX_TBL_IDENTIFIER_TYPE i   
       ON l.IDENTIFIER_TYPE_ID = I.IDENTIFIER_TYPE_ID   
                  WHERE(@SEARCH_STRING IS NULL  
                    OR l.NAME LIKE '%'+@SEARCH_STRING+'%'  
     OR l.CODE LIKE '%'+@SEARCH_STRING+'%'  
     OR i.NAME LIKE '%'+@SEARCH_STRING+'%'  
     OR CONVERT(VARCHAR(10), l.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'  
     OR CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%')              
                       
              AND (l.DELETED = 0)  
              AND l.PRACTICE_CODE = @PRACTICE_CODE  
              AND (@code IS NULL  
                   OR l.CODE LIKE '%'+@code+'%')  
              AND (@NAME IS NULL  
                   OR l.NAME LIKE '%'+@NAME+'%')  
     AND (@TYPE IS NULL  
                   OR i.IDENTIFIER_TYPE_ID  = @TYPE)  
         ) AS FOX_TBL_LOCATION_CORPORATION  
         ORDER BY CASE  
                      WHEN @SORT_BY = 'Name'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN NAME  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'Name'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN NAME  
                  END DESC,  
       CASE  
                      WHEN @SORT_BY = 'Code'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN CODE  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'Code'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN CODE  
                  END DESC,  
      CASE  
                      WHEN @SORT_BY = 'Type'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN IDENTIFIER_TYPE_NAME  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'Type'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN IDENTIFIER_TYPE_NAME  
                  END DESC,  
                  CASE  
                      WHEN @SORT_BY = 'CreatedDate'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN CREATED_DATE  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'CreatedDate'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN CREATED_DATE  
                  END DESC,  
       CASE  
                      WHEN @SORT_BY = 'ModifiedDate'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN MODIFIED_DATE  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'ModifiedDate'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN MODIFIED_DATE  
                  END DESC  
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;  
     END; 