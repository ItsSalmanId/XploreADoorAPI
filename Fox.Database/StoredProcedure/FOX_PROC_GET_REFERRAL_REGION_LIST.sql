IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_REGION_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_REGION_LIST  
GO                       
 --EXEC [FOX_PROC_GET_REFERRAL_REGION_LIST] 1011163,'', '', '', 1, 10, 'createdDate' , 'DESC'                                               
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REFERRAL_REGION_LIST]             
 @PRACTICE_CODE        BIGINT,                           
 @REFERRAL_REGION_CODE VARCHAR(50),                           
 @REFERRAL_REGION_NAME VARCHAR(100),                           
 @SEARCH_STRING        VARCHAR(100),                           
 @CURRENT_PAGE         INT,                           
 @RECORD_PER_PAGE      INT,                           
 @SORT_BY              VARCHAR(50),                           
 @SORT_ORDER           VARCHAR(5)                          
AS                          
     BEGIN                          
         IF(@REFERRAL_REGION_CODE = '')                          
             BEGIN                          
                 SET @REFERRAL_REGION_CODE = NULL;                          
             END;                          
             ELSE                          
             BEGIN                          
                 SET @REFERRAL_REGION_CODE = '%'+@REFERRAL_REGION_CODE;                        
             END;                          
         IF(@REFERRAL_REGION_NAME = '')                          
             BEGIN                          
                 SET @REFERRAL_REGION_NAME = NULL;                          
             END;                          
             ELSE                          
             BEGIN                          
                 SET @REFERRAL_REGION_NAME = '%'+@REFERRAL_REGION_NAME;                          
             END;                          
         IF(@SEARCH_STRING = '')                          
             BEGIN                          
                 SET @SEARCH_STRING = NULL;                          
             END;                          
             ELSE                          
             BEGIN                          
                 IF CHARINDEX('[', @SEARCH_STRING) > 0                          
                     BEGIN                          
                         SET @SEARCH_STRING = Replace(@SEARCH_STRING, '[', '');                          
                     END;                          
                 IF CHARINDEX(']', @SEARCH_STRING) > 0                          
                     BEGIN                          
                         SET @SEARCH_STRING = Replace(@SEARCH_STRING, ']', '');                          
                     END;                          
                 SET @SEARCH_STRING = '%'+@SEARCH_STRING+'%';                          
             END;              
            
    IF OBJECT_ID('TEMPDB..#REGION') IS NOT NULL DROP TABLE #REGION            
                
            
    SELECT R.REFERRAL_REGION_Id,                           
                    R.REFERRAL_REGION_CODE,                           
                    R.REFERRAL_REGION_NAME,                           
                    R.ACCOUNT_MANAGER_EMAIL,                           
                    R.ACCOUNT_MANAGER,                          
                    R.ACCOUNT_MANAGER_ID,                           
                    R.REGIONAL_DIRECTOR_ID,                           
                    R.ALTERNATE_REGION_ID,                          
     R.SENIOR_REGIONAL_DIRECTOR_ID,                    
                    R.STATE_CODE,                           
                    R.CREATED_BY,                           
                    R.CREATED_DATE,           
     CL.IS_FAX_COVER_LETTER,        
     CL.FILE_PATH,                        
                    CONVERT(VARCHAR, R.CREATED_DATE) AS Created_Date_Str,                           
     R.MODIFIED_BY,                           
                    R.MODIFIED_DATE,                           
                    CONVERT(VARCHAR, R.MODIFIED_DATE) AS Modified_Date_Str,                           
                    R.DELETED,       
                    R.IS_INACTIVE,                          
                    CASE                          
                        WHEN R.IS_ACTIVE = 1                          
                        THEN 'NO'                          
                 WHEN R.IS_ACTIVE = 0                          
                        THEN 'YES'                          
                        ELSE 'NO'                          
         END AS Inactive,                           
                                 
                    U.LAST_NAME+', '+U.FIRST_NAME AS REGIONAL_DIRECTOR,                
                    U1.LAST_NAME+', '+U1.FIRST_NAME AS ALT_REGIONAL_DIRECTOR_NAME,                       
                    U3.LAST_NAME+', '+U3.FIRST_NAME AS SENIOR_REGIONAL_DIRECTOR,                           
     U2.LAST_NAME+', '+U2.FIRST_NAME AS ACCOUNT_MANAGER_NAME,                
                    R.IN_ACTIVEDATE,              
      R.[START_DATE],                  
      R.END_DATE,            
      R.PRACTICE_CODE,              
     (SELECT SUBSTRING( (SELECT  Distinct ',' + COUNTY  FROM FOX_TBL_ZIP_STATE_COUNTY WHERE REFERRAL_REGION_ID = R.REFERRAL_REGION_Id FOR XML PATH('')), 2,10000) ) AS Seleced_Counties             
     INTO #REGION               
             FROM FOX_TBL_REFERRAL_REGION R                          
                  LEFT OUTER JOIN FOX_TBL_APPLICATION_USER U ON R.REGIONAL_DIRECTOR_ID = U.USER_ID                                        
                  --AND R.ALTERNATE_REGION_ID = U.USER_ID                                      
                  LEFT OUTER JOIN FOX_TBL_APPLICATION_USER U1 ON R.ALTERNATE_REGION_ID = U1.USER_ID                          
                  LEFT OUTER JOIN FOX_TBL_APPLICATION_USER U2 ON R.ACCOUNT_MANAGER_ID = U2.USER_ID                          
                  LEFT OUTER JOIN FOX_TBL_APPLICATION_USER U3 ON R.SENIOR_REGIONAL_DIRECTOR_ID = U3.USER_ID          
         LEFT OUTER JOIN FOX_TBL_REGION_COVER_SHEET CL ON R.REFERRAL_REGION_ID = CL.REFERRAL_REGION_ID                                           
                
                          
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                          
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                          
         DECLARE @TOATL_PAGESUDM FLOAT;                          
         SELECT @TOATL_PAGESUDM = COUNT(*)                          
         FROM #REGION R                                           
             WHERE(@SEARCH_STRING IS NULL                          
                   OR R.REFERRAL_REGION_CODE LIKE '%'+@SEARCH_STRING+'%'                          
                   OR R.REFERRAL_REGION_NAME LIKE '%'+@SEARCH_STRING+'%'                                                                             
                   OR R.ACCOUNT_MANAGER_NAME LIKE '%'+@SEARCH_STRING+'%'                    
                   OR R.REGIONAL_DIRECTOR LIKE '%'+@SEARCH_STRING+'%'               
       OR R.SENIOR_REGIONAL_DIRECTOR LIKE '%'+@SEARCH_STRING+'%'            
       OR R.Seleced_Counties LIKE '%'+@SEARCH_STRING+'%'            
                   )                          
                  AND (@REFERRAL_REGION_CODE IS NULL                          
                       OR R.REFERRAL_REGION_CODE LIKE '%'+@REFERRAL_REGION_CODE)                          
                  AND (@REFERRAL_REGION_NAME IS NULL                          
                       OR @REFERRAL_REGION_NAME LIKE '%'+@REFERRAL_REGION_NAME)                          
                  AND (R.DELETED = 0)                          
                  AND R.PRACTICE_CODE = @PRACTICE_CODE                                             
                     
               
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
             SELECT *,                          
                    ROW_NUMBER() OVER(ORDER BY R.CREATED_DATE DESC) AS ACTIVEROW                
             FROM #REGION  R            
             WHERE(@SEARCH_STRING IS NULL                          
                   OR R.REFERRAL_REGION_CODE LIKE '%'+@SEARCH_STRING+'%'                          
                   OR R.REFERRAL_REGION_NAME LIKE '%'+@SEARCH_STRING+'%'                                                                             
                   OR R.ACCOUNT_MANAGER_NAME LIKE '%'+@SEARCH_STRING+'%'                    
                   OR R.REGIONAL_DIRECTOR LIKE '%'+@SEARCH_STRING+'%'               
       OR R.SENIOR_REGIONAL_DIRECTOR LIKE '%'+@SEARCH_STRING+'%'            
       OR R.Seleced_Counties LIKE '%'+@SEARCH_STRING+'%'            
                   )                          
                  AND (@REFERRAL_REGION_CODE IS NULL                          
                    OR R.REFERRAL_REGION_CODE LIKE '%'+@REFERRAL_REGION_CODE)                          
                  AND (@REFERRAL_REGION_NAME IS NULL                          
                       OR @REFERRAL_REGION_NAME LIKE '%'+@REFERRAL_REGION_NAME)                          
                  AND (R.DELETED = 0)                          
                  AND R.PRACTICE_CODE = @PRACTICE_CODE                          
         ) AS FOX_TBL_REFERRAL_REGION                          
         ORDER BY CASE                          
                      WHEN @SORT_BY = 'Code'                          
                           AND @SORT_ORDER = 'ASC'                          
                      THEN REFERRAL_REGION_CODE                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'Code'                          
                     AND @SORT_ORDER = 'DESC'                          
                      THEN REFERRAL_REGION_CODE                          
                  END DESC,                          
                  CASE                          
                      WHEN @SORT_BY = 'RegionName'                          
                           AND @SORT_ORDER = 'ASC'                         
                      THEN REFERRAL_REGION_NAME                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'RegionName'                          
                           AND @SORT_ORDER = 'DESC'                          
                      THEN REFERRAL_REGION_NAME                          
                  END DESC,                          
                  CASE                          
                      WHEN @SORT_BY = 'ACC_MGR'                          
                           AND @SORT_ORDER = 'ASC'                          
                      THEN ACCOUNT_MANAGER                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'ACC_MGR'                          
                           AND @SORT_ORDER = 'DESC'                          
                      THEN ACCOUNT_MANAGER                          
                  END DESC,                          
                  CASE                       
                      WHEN @SORT_BY = 'ACC_MGR_EMAIL'                          
                      AND @SORT_ORDER = 'ASC'                          
                      THEN ACCOUNT_MANAGER_EMAIL                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'ACC_MGR_EMAIL'                          
                           AND @SORT_ORDER = 'DESC'                          
                      THEN ACCOUNT_MANAGER_EMAIL                          
                  END DESC,                
      CASE                          
                      WHEN @SORT_BY = 'County'                          
                      AND @SORT_ORDER = 'ASC'                          
                      THEN Seleced_Counties                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'County'                          
                           AND @SORT_ORDER = 'DESC'                          
                      THEN Seleced_Counties                         
                  END DESC,              
      CASE                          
                      WHEN @SORT_BY = 'Reg_Dir'                          
                      AND @SORT_ORDER = 'ASC'                          
                      THEN REGIONAL_DIRECTOR                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'Reg_Dir'                          
                           AND @SORT_ORDER = 'DESC'                          
                      THEN REGIONAL_DIRECTOR                          
         END DESC,              
      CASE                          
                      WHEN @SORT_BY = 'Sr_Reg_Dir'                          
                      AND @SORT_ORDER = 'ASC'                          
                      THEN SENIOR_REGIONAL_DIRECTOR                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'Sr_Reg_Dir'                          
         AND @SORT_ORDER = 'DESC'                          
                      THEN SENIOR_REGIONAL_DIRECTOR                          
                  END DESC,                        
                  CASE                          
                      WHEN @SORT_BY = 'createdDate'                          
                           AND @SORT_ORDER = 'ASC'                          
                      THEN CREATED_DATE                          
                  END ASC,                          
                  CASE                          
                      WHEN @SORT_BY = 'createdDate'                          
                           AND @SORT_ORDER = 'DESC'                          
                      THEN CREATED_DATE                          
                  END DESC                          
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                          
     END; 