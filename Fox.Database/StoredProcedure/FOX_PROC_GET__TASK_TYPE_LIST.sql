IF (OBJECT_ID('FOX_PROC_GET__TASK_TYPE_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET__TASK_TYPE_LIST
GO
  -- =============================================                                                            
-- AUTHOR:  <DEVELOPER, Abdur Rafay>                                                            
-- CREATE DATE: <CREATE DATE, 25/01/2019>                                                            
-- DESCRIPTION: <GET TASK TYPE LIST>                    
--exec [FOX_PROC_GET__TASK_TYPE_LIST] '1011163','','','',1,0,'CreatedDate','DESC'     
CREATE procedure [dbo].[FOX_PROC_GET__TASK_TYPE_LIST]-- 1011163,'','al001',1, 2000, 'code' , 'desc' 
@practice_code   BIGINT,           
@search_string   VARCHAR(100),           
@name         VARCHAR(50),          
@description     VARCHAR(100),          
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
        IF(@DESCRIPTION = '')          
             BEGIN          
                 SET @DESCRIPTION = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @DESCRIPTION = '%'+@DESCRIPTION+'%';          
             END;          
       IF(@NAME = '')          
             BEGIN          
                 SET @NAME = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @NAME = '%'+@NAME+'%';          
             END;                       
         --                  
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;          
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;          
         DECLARE @TOATL_PAGESUDM FLOAT;          
          
         --                  
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT l.TASK_TYPE_ID)          
         FROM FOX_TBL_TASK_TYPE l          
         WHERE(@SEARCH_STRING IS NULL          
               OR l.NAME LIKE '%'+@SEARCH_STRING+'%'          
               OR l.RT_CODE LIKE '%'+@SEARCH_STRING+'%'          
               OR CONVERT(VARCHAR(10), l.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'          
               OR CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%')                      
                               
              AND (l.DELETED = 0)          
              AND l.PRACTICE_CODE = @PRACTICE_CODE          
              AND (@DESCRIPTION IS NULL          
                   OR l.RT_CODE LIKE '%'+@DESCRIPTION+'%')          
              AND (@NAME IS NULL          
                   OR l.NAME LIKE '%'+@NAME+'%')          
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
                    l.TASK_TYPE_ID ,           
                    l.NAME,           
                    l.Description,          
     l.RT_CODE,          
     l.CATEGORY_CODE,          
     l.CASE_STATUS_ID,           
                    l.CREATED_BY,                 
                    l.CREATED_DATE,          
     convert(varchar,l.CREATED_DATE) AS Created_Date_Str,                                           
                    l.MODIFIED_BY,                                           
                    l.MODIFIED_DATE,           
     convert(varchar,l.MODIFIED_DATE) AS Modified_Date_Str,          
    l.DELETED,        
     l.SHOW_IN_TASKS,          
     ROW_NUMBER() OVER (                  
      ORDER BY l.CREATED_DATE DESC                  
      ) AS ACTIVEROW,          
     ISNULL(l.IS_ACTIVE,1) AS IS_ACTIVE ,          
     CASE WHEN l.IS_ACTIVE = 1  THEN 'NO' WHEN l.IS_ACTIVE = 0 THEN 'YES' else 'NO' END as Inactive                            
                                            
             FROM FOX_TBL_TASK_TYPE l          
                  WHERE(@SEARCH_STRING IS NULL          
                   OR l.NAME LIKE '%'+@SEARCH_STRING+'%'          
               OR l.RT_CODE LIKE '%'+@SEARCH_STRING+'%'          
               OR CONVERT(VARCHAR(10), l.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'          
               OR CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%')                      
                               
              AND (l.DELETED = 0)          
              AND l.PRACTICE_CODE = @PRACTICE_CODE          
        AND (@DESCRIPTION IS NULL          
                   OR l.RT_CODE LIKE '%'+@DESCRIPTION+'%')          
              AND (@NAME IS NULL          
                   OR l.NAME LIKE '%'+@NAME+'%')          
         ) AS FOX_TBL_GROUP_IDENTIFIER          
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
                      THEN RT_CODE          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'Code'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN RT_CODE          
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