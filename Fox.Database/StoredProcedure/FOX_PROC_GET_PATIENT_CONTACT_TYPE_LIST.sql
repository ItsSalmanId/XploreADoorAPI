IF (OBJECT_ID('FOX_PROC_GET_PATIENT_CONTACT_TYPE_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_CONTACT_TYPE_LIST  
GO   
----2   
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================                                                    
-- AUTHOR:  <DEVELOPER, Abdur Rafay>                                                    
-- CREATE DATE: <CREATE DATE, 26/01/2019>                                                    
-- DESCRIPTION: <GET PATIENT CONTACT TYPE LIST>       
--exec [FOX_PROC_GET_PATIENT_CONTACT_TYPE_LIST_TEST_RAFAY] '1011163','','','',1,0,'CreatedDate','DESC'                 
CREATE procedure [FOX_PROC_GET_PATIENT_CONTACT_TYPE_LIST]                                          
@practice_code   BIGINT,   
@search_string   VARCHAR(100),   
@name         VARCHAR(50),  
@code     VARCHAR(100),  
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
         --          
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;  
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;  
         DECLARE @TOATL_PAGESUDM FLOAT;  
  
         --          
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT l.Contact_Type_ID)  
         FROM FOX_TBL_PATIENT_CONTACT_TYPES l  
         WHERE(@SEARCH_STRING IS NULL  
               OR l.Type_Name LIKE '%'+@SEARCH_STRING+'%'  
               OR l.CODE LIKE '%'+@SEARCH_STRING+'%'  
               OR CONVERT(VARCHAR(10), l.Created_On, 101) LIKE '%'+@SEARCH_STRING+'%'  
               OR CONVERT(VARCHAR(10), l.Modified_On, 101) LIKE '%'+@SEARCH_STRING+'%')              
                       
              AND (l.DELETED = 0)  
              AND l.PRACTICE_CODE = @PRACTICE_CODE  
              AND (@code IS NULL  
                   OR l.CODE LIKE '%'+@code+'%')  
              AND (@NAME IS NULL  
                   OR l.Type_Name  LIKE '%'+@NAME+'%')  
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
                    l.Contact_Type_ID ,   
                    l.Type_Name,   
                    l.CODE,  
     l.Display_Order,   
                    l.CREATED_BY,                                   
                    l.Created_On,  
     convert(varchar,l.Created_On) AS Created_Date_Str,                                    
                    l.MODIFIED_BY,                                   
                    l.Modified_On,  
     convert(varchar,l.Modified_On) AS Modified_Date_Str,   
l.DELETED,  
     l.END_DATE,  
     l.START_DATE,  
     ROW_NUMBER() OVER (          
      ORDER BY l.Created_On DESC          
      ) AS ACTIVEROW,  
     ISNULL(l.IS_ACTIVE,1) AS IS_ACTIVE   ,  
        CASE WHEN l.IS_ACTIVE = 1  THEN 'NO' WHEN l.IS_ACTIVE = 0 THEN 'YES' else 'NO' END as Inactive  
     
                  FROM FOX_TBL_PATIENT_CONTACT_TYPES l  
                  WHERE(@SEARCH_STRING IS NULL  
                    OR l.Type_Name LIKE '%'+@SEARCH_STRING+'%'  
     OR l.CODE LIKE '%'+@SEARCH_STRING+'%'  
     OR CONVERT(VARCHAR(10), l.Created_On, 101) LIKE '%'+@SEARCH_STRING+'%'  
     OR CONVERT(VARCHAR(10), l.Modified_On, 101) LIKE '%'+@SEARCH_STRING+'%')              
                       
              AND (l.DELETED = 0)  
              AND l.PRACTICE_CODE = @PRACTICE_CODE  
 AND (@code IS NULL  
                   OR l.CODE LIKE '%'+@code+'%')  
              AND (@NAME IS NULL  
                   OR l.Type_Name LIKE '%'+@NAME+'%')  
         ) AS FOX_TBL_LOCATION_CORPORATION  
         ORDER BY CASE  
                      WHEN @SORT_BY = 'Name'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN Type_Name  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'Name'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN Type_Name  
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
                      WHEN @SORT_BY = 'CreatedDate'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN Created_On  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'CreatedDate'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN Created_On  
                  END DESC,  
       CASE  
                      WHEN @SORT_BY = 'ModifiedDate'  
                           AND @SORT_ORDER = 'ASC'  
                      THEN Modified_On  
                  END ASC,  
                  CASE  
                      WHEN @SORT_BY = 'ModifiedDate'  
                           AND @SORT_ORDER = 'DESC'  
                      THEN Modified_On  
                  END DESC  
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;  
     END; 