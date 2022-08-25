IF (OBJECT_ID('FOX_PROC_GET_PRACTICE_ORGANIZATION_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PRACTICE_ORGANIZATION_LIST  
GO
----3	 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================  
-- AUTHOR:  <DEVELOPER, YOUSAF>  
-- CREATE DATE: <CREATE DATE, 07/18/2018>  
-- DESCRIPTION: <GET PRACTICE ORGANIZATION LIST>  

CREATE PROCEDURE [dbo].[FOX_PROC_GET_PRACTICE_ORGANIZATION_LIST] --1011163,'', '', 1, 0, 'CREATED_DATE' , 'DESC'  
@PRACTICE_CODE   BIGINT, 
@NAME            VARCHAR(100), 
@SEARCH_STRING   VARCHAR(100), 
@CURRENT_PAGE    INT, 
@RECORD_PER_PAGE INT, 
@SORT_BY         VARCHAR(50), 
@SORT_ORDER      VARCHAR(5)
AS
     BEGIN
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;
         DECLARE @TOATL_PAGESUDM FLOAT;
         SELECT @TOATL_PAGESUDM = COUNT(*)
         FROM FOX_TBL_PRACTICE_ORGANIZATION
         WHERE ISNULL(DELETED, 0) = 0
               AND PRACTICE_CODE = @PRACTICE_CODE
               AND (CODE LIKE '%'+@SEARCH_STRING+'%'
                    OR NAME LIKE '%'+@SEARCH_STRING+'%'
                    OR DESCRIPTION LIKE '%'+@SEARCH_STRING+'%'
                    OR ZIP LIKE '%'+@SEARCH_STRING+'%'
                    OR CITY LIKE '%'+@SEARCH_STRING+'%'
                    OR STATE LIKE '%'+@SEARCH_STRING+'%'
                    OR ADDRESS LIKE '%'+@SEARCH_STRING+'%');
         IF(@RECORD_PER_PAGE = 0)
             BEGIN
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;
             END;
             ELSE
             BEGIN
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;
             END;
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);
         SELECT *, 
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES, 
                @TOTAL_RECORDS AS TOTAL_RECORDS
         FROM
         (
             SELECT PRACTICE_ORGANIZATION_ID, 
                    PRACTICE_CODE, 
                    CODE, 
                    ISNULL(NAME, '') AS NAME, 
                    ISNULL(DESCRIPTION, '') AS DESCRIPTION, 
                    ISNULL(ZIP, '') AS ZIP, 
                    ISNULL(CITY, '') AS CITY, 
                    ISNULL(STATE, '') AS STATE, 
                    ISNULL(ADDRESS, '') AS ADDRESS, 
                    CREATED_BY, 
                    CREATED_DATE, 
                    MODIFIED_BY, 
                    MODIFIED_DATE, 
                    DELETED, 
                    ISNULL(IS_ACTIVE, 1) AS IS_ACTIVE,
                    CASE
                        WHEN IS_ACTIVE = 1
                        THEN 'NO'
                        WHEN IS_ACTIVE = 0
                        THEN 'YES'
                        ELSE 'NO'
                    END AS Inactive, 
                    ROW_NUMBER() OVER(ORDER BY CREATED_DATE DESC) AS ACTIVEROW
             FROM FOX_TBL_PRACTICE_ORGANIZATION
             WHERE ISNULL(DELETED, 0) = 0
                   AND PRACTICE_CODE = @PRACTICE_CODE
                   AND (CODE LIKE '%'+@SEARCH_STRING+'%'
                        OR NAME LIKE '%'+@SEARCH_STRING+'%'
                        OR DESCRIPTION LIKE '%'+@SEARCH_STRING+'%'
                        OR ZIP LIKE '%'+@SEARCH_STRING+'%'
                        OR CITY LIKE '%'+@SEARCH_STRING+'%'
                        OR STATE LIKE '%'+@SEARCH_STRING+'%'
                        OR ADDRESS LIKE '%'+@SEARCH_STRING+'%')
         ) AS PRACTICE_ORGANIZATION_LIST
         ORDER BY CASE
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
                      WHEN @SORT_BY = 'NAME'
                           AND @SORT_ORDER = 'ASC'
                      THEN NAME
                  END ASC,
                  CASE
                      WHEN @SORT_BY = 'NAME'
                           AND @SORT_ORDER = 'DESC'
                      THEN NAME
                  END DESC,
                  CASE
                      WHEN @SORT_BY = 'ZIP'
                           AND @SORT_ORDER = 'ASC'
                      THEN ZIP
                  END ASC,
                  CASE
                      WHEN @SORT_BY = 'ZIP'
                           AND @SORT_ORDER = 'DESC'
                      THEN ZIP
                  END DESC,
                  CASE
                      WHEN @SORT_BY = 'CITY'
                           AND @SORT_ORDER = 'ASC'
                      THEN CITY
                  END ASC,
                  CASE
                      WHEN @SORT_BY = 'CITY'
                           AND @SORT_ORDER = 'DESC'
                      THEN CITY
                  END DESC,
                  CASE
                      WHEN @SORT_BY = 'STATE'
                           AND @SORT_ORDER = 'ASC'
                      THEN STATE
                  END ASC,
                  CASE
                      WHEN @SORT_BY = 'STATE'
                           AND @SORT_ORDER = 'DESC'
                      THEN STATE
                  END DESC,
                  CASE
                      WHEN @SORT_BY = 'ADDRESS'
                           AND @SORT_ORDER = 'ASC'
                      THEN ADDRESS
                  END ASC,
                  CASE
                      WHEN @SORT_BY = 'ADDRESS'
                           AND @SORT_ORDER = 'DESC'
                      THEN ADDRESS
                  END DESC,
                  CASE
                      WHEN @SORT_BY = 'CREATED_DATE'
                           AND @SORT_ORDER = 'ASC'
                      THEN CREATED_DATE
                  END ASC,
                  CASE
                      WHEN @SORT_BY = 'CREATED_DATE'
                           AND @SORT_ORDER = 'DESC'
                      THEN CREATED_DATE
                  END DESC
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;
     END;

