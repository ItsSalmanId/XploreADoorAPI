IF (OBJECT_ID('FOX_GET_FOX_PROVIDER') IS NOT NULL ) DROP PROCEDURE FOX_GET_FOX_PROVIDER  
GO 
-- =============================================                                                              
-- AUTHOR:  <DEVELOPER, JOHAR>                                                              
-- CREATE DATE: <CREATE DATE, 11/05/2018>                                                              
-- DESCRIPTION: <GET FOX PROVIDER LIST>                                                              
----   [DBO].[FOX_GET_FOX_PROVIDER] 1011163,'',1,0,0,'CREATED_DATE','desc','','','','','','','',''            
--[DBO].[[FOX_GET_FOX_PROVIDER]] 1011163,'',1,0,0,'CREATED_DATE','desc','','','@mtbc','','','','',''                                                      
-- select * from FOX_TBL_PROVIDER where TREATMENT_LOC_ID is NULL                              
CREATE PROCEDURE [dbo].[FOX_GET_FOX_PROVIDER] @PRACTICE_CODE     BIGINT,           
                                             @SEARCH_STRING     VARCHAR(100),           
                                             @CURRENT_PAGE      INT,           
                                             @RECORD_PER_PAGE   INT,           
                                             @FOX_PROVIDER_ID   BIGINT,           
                                             @SORT_BY           VARCHAR(50),           
                                             @SORT_ORDER        VARCHAR(5),           
                                             @FIRST_NAME        VARCHAR(25),           
                                             @LAST_NAME         VARCHAR(25),          
            @EMAIL             VARCHAR(25),           
                                             @RT_CODE           VARCHAR(50),           
                                             @NPI               VARCHAR(11),           
                                             @DISCIPLINE        VARCHAR(10),           
                                             @TREATING_REGION   VARCHAR(16),           
                                             @TREATING_LOCATION VARCHAR(25)          
          
--declare @PRACTICE_CODE           BIGINT = 1011163                            
--declare @SEARCH_STRING           VARCHAR(100)  = 'ha'                               
--declare @CURRENT_PAGE            INT            = 1                                   
--declare @RECORD_PER_PAGE         INT  = 10                            
--declare @FOX_PROVIDER_ID         BIGINT  = 0                             
--declare @SORT_BY                 VARCHAR(50)   = 'CREATED_DATE'                                            
--declare @SORT_ORDER              VARCHAR(5)  = 'asc'                            
          
AS          
     BEGIN          
         IF(@FOX_PROVIDER_ID = 0)          
             BEGIN          
                 SET @FOX_PROVIDER_ID = NULL;          
             END;          
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;          
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;          
         DECLARE @TOATL_PAGESUDM FLOAT;          
         IF(@FIRST_NAME = '')          
             BEGIN          
                 SET @FIRST_NAME = NULL;          
             END;          
         IF(@LAST_NAME = '')          
             BEGIN          
                 SET @LAST_NAME = NULL;          
             END;          
  IF(@EMAIL = '')          
             BEGIN          
                 SET @EMAIL = NULL;          
             END;          
         IF(@RT_CODE = '')          
             BEGIN          
                 SET @RT_CODE = NULL;          
             END;          
         IF(@NPI = '')          
             BEGIN          
                 SET @NPI = NULL;          
             END;          
         IF(@DISCIPLINE = '')          
             BEGIN          
                 SET @DISCIPLINE = NULL;          
             END;          
         IF(@TREATING_REGION = '')          
             BEGIN          
                 SET @TREATING_REGION = NULL;          
             END;          
         IF(@TREATING_LOCATION = '')          
             BEGIN          
                 SET @TREATING_LOCATION = NULL;          
             END;          
          
         --                                      
         SELECT @TOATL_PAGESUDM = COUNT(*)          
         FROM FOX_TBL_PROVIDER P          
              LEFT JOIN FOX_TBL_REFERRAL_REGION R ON R.REFERRAL_REGION_ID = P.REFERRAL_REGION_ID          
                                                     AND ISNULL(R.DELETED, 0) = 0          
                                                     AND R.PRACTICE_CODE = @PRACTICE_CODE      
     LEFT JOIN FOX_TBL_APPLICATION_USER AS AU ON R.REGIONAL_DIRECTOR_ID = AU.USER_ID      
              AND ISNULL(AU.DELETED, 0) = 0      
              AND AU.PRACTICE_CODE = @PRACTICE_CODE      
     LEFT JOIN FOX_TBL_APPLICATION_USER AS U3 ON R.SENIOR_REGIONAL_DIRECTOR_ID = U3.USER_ID       
              AND ISNULL(U3.DELETED, 0) = 0      
              AND U3.PRACTICE_CODE = @PRACTICE_CODE                         
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS L ON L.LOC_ID = P.TREATMENT_LOC_ID          
                                                      AND ISNULL(L.DELETED, 0) = 0          
                                                      AND L.PRACTICE_CODE = @PRACTICE_CODE          
              LEFT JOIN FOX_TBL_DISCIPLINE D ON D.DISCIPLINE_ID = P.DISCIPLINE_ID          
                                                AND ISNULL(D.DELETED, 0) = 0          
                                                AND D.PRACTICE_CODE = @PRACTICE_CODE          
              LEFT JOIN FOX_TBL_VISIT_QOUTA_PER_WEEK W ON W.VISIT_QOUTA_WEEK_ID = P.VISIT_QOUTA_WEEK_ID          
                                                          AND ISNULL(W.DELETED, 0) = 0          
                                                          AND W.PRACTICE_CODE = @PRACTICE_CODE          
              LEFT JOIN FOX_TBL_APPLICATION_USER u ON u.USER_NAME = P.CREATED_BY          
              LEFT JOIN FOX_TBL_APPLICATION_USER us ON us.USER_NAME = P.MODIFIED_BY          
      
         WHERE P.PRACTICE_CODE = @PRACTICE_CODE          
               AND ISNULL(P.DELETED, 0) = 0          
               AND (@FIRST_NAME IS NULL          
                    OR (P.FIRST_NAME LIKE '%'+@FIRST_NAME+'%'))          
               AND (@LAST_NAME IS NULL          
                    OR (P.LAST_NAME LIKE '%'+@LAST_NAME+'%'))          
     AND (@EMAIL IS NULL          
                    OR (P.EMAIL LIKE '%'+@EMAIL+'%'))          
               AND (@RT_CODE IS NULL          
                    OR (P.FOX_PROVIDER_CODE LIKE '%'+@RT_CODE+'%'))          
               AND (@NPI IS NULL          
                    OR (P.INDIVIDUAL_NPI LIKE '%'+@NPI+'%'))          
               AND (@DISCIPLINE IS NULL          
                    OR (P.DISCIPLINE_ID LIKE '%'+@DISCIPLINE+'%'))          
               AND (@TREATING_REGION IS NULL          
                    OR (R.REFERRAL_REGION_NAME LIKE '%'+@TREATING_REGION+'%'))          
               AND (@TREATING_LOCATION IS NULL          
                    OR (L.NAME LIKE '%'+@TREATING_LOCATION+'%'))          
               AND (P.PROVIDER_CODE LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.FIRST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.LAST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
      OR P.EMAIL LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.INDIVIDUAL_NPI LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR R.REFERRAL_REGION_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.STATE LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR L.NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.CLR LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR L.CODE LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR D.NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.PRIMARY_POS_DISTANCE LIKE '%%'+@SEARCH_STRING+'%%'     
                    OR W.VISIT_QOUTA_WEEK LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.ACTIVE_CASES LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.PTO_HRS LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.CREATED_BY LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.MODIFIED_BY LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR P.PTO_HRS LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR u.LAST_NAME+', '+u.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'          
                    OR us.LAST_NAME+', '+us.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'          
                    OR u.FIRST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR u.LAST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR us.FIRST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR us.LAST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
     OR R.ACCOUNT_MANAGER LIKE '%%' + @SEARCH_STRING + '%%'      
     OR AU.FIRST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
     OR AU.LAST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
     OR U3.FIRST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
     OR U3.LAST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
     OR u.EMAIL LIKE '%%'+@SEARCH_STRING+'%%'          
     OR us.EMAIL LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR p.FOX_PROVIDER_CODE LIKE '%%'+@SEARCH_STRING+'%%'          
                    OR CONVERT(VARCHAR(10), P.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'          
             OR CONVERT(VARCHAR(10), P.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'          
     OR p.STATUS LIKE '%%'+@SEARCH_STRING+'%%')          
               AND ISNULL(P.DELETED, 0) = 0;          
          
         -----                              
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
          
         --Get Total Result with Page No                                      
         SELECT *,           
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,           
                @TOTAL_RECORDS AS TOTAL_RECORDS          
         FROM          
         (          
             SELECT ROW_NUMBER() OVER(ORDER BY P.FOX_PROVIDER_ID DESC) AS ROW,           
                    P.FOX_PROVIDER_ID,           
                    P.IS_INACTIVE,           
      CASE WHEN P.IS_INACTIVE = 0  THEN 'NO' WHEN P.IS_INACTIVE = 1 THEN 'YES' else 'YES' END as Inactive,          
                    P.PROVIDER_CODE,           
                    P.FOX_PROVIDER_CODE,           
                    P.FIRST_NAME,           
                    P.LAST_NAME,          
     LOWER (p.EMAIL) AS EMAIL,           
                    P.INDIVIDUAL_NPI,           
                    P.ADDRESS,           
                    P.STATE,           
                    P.SSN,           
                    P.REFERRAL_REGION_ID,           
                    R.REFERRAL_REGION_NAME AS REFERRAL_REGION_NAME,           
                    P.TREATMENT_LOC_ID,           
                    L.NAME AS TREATMENT_LOCATION,           
                    P.DISCIPLINE_ID,           
                    D.NAME AS DISPLINE_NAME,           
                    P.TAXONOMY_CODE,           
P.PRIMARY_POS_DISTANCE,           
                    P.VISIT_QOUTA_WEEK_ID,           
                    w.VISIT_QOUTA_WEEK,           
                    P.ACTIVE_CASES,           
                    P.PTO_HRS,           
                    P.CREATED_BY,           
                    P.STATUS,        
     R.ACCOUNT_MANAGER,      
     AU.LAST_NAME + ', ' + AU.FIRST_NAME AS REGIONAL_DIRECTOR ,      
     U3.LAST_NAME + ', ' + U3.FIRST_NAME AS SENIOR_REGIONAL_DIRECTOR,      
                    P.CLR,          
                    CASE          
                        WHEN u.USER_NAME IS NULL          
                        THEN P.CREATED_BY          
                        ELSE u.LAST_NAME+', '+u.FIRST_NAME          
                    END AS CREATED_BYNAME,           
             CASE          
                        WHEN us.USER_NAME IS NULL          
                        THEN P.MODIFIED_BY          
                        ELSE us.LAST_NAME+', '+us.FIRST_NAME          
                    END AS MODIFIED_BYNAME,           
          
                    P.CREATED_DATE,           
                    CONVERT(VARCHAR, P.CREATED_DATE, 101) AS CREATED_DATE_STRING,           
                    P.MODIFIED_DATE,           
                    CONVERT(VARCHAR, P.MODIFIED_DATE, 101) AS MODIFIED_DATE_STRING          
             FROM FOX_TBL_PROVIDER P          
                  LEFT JOIN FOX_TBL_REFERRAL_REGION R ON R.REFERRAL_REGION_ID = P.REFERRAL_REGION_ID          
                                                         AND ISNULL(R.DELETED, 0) = 0          
                                                         AND R.PRACTICE_CODE = @PRACTICE_CODE        
         LEFT JOIN FOX_TBL_APPLICATION_USER AS AU ON R.REGIONAL_DIRECTOR_ID = AU.USER_ID      
              AND ISNULL(AU.DELETED, 0) = 0      
              AND AU.PRACTICE_CODE = @PRACTICE_CODE      
         LEFT JOIN FOX_TBL_APPLICATION_USER AS U3 ON R.SENIOR_REGIONAL_DIRECTOR_ID = U3.USER_ID       
              AND ISNULL(U3.DELETED, 0) = 0      
              AND U3.PRACTICE_CODE = @PRACTICE_CODE                       
                  LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS L ON L.LOC_ID = P.TREATMENT_LOC_ID          
                                                          AND ISNULL(L.DELETED, 0) = 0          
                                                          AND L.PRACTICE_CODE = @PRACTICE_CODE          
                  LEFT JOIN FOX_TBL_DISCIPLINE D ON D.DISCIPLINE_ID = P.DISCIPLINE_ID          
                                                    AND ISNULL(D.DELETED, 0) = 0          
                                                    AND D.PRACTICE_CODE = @PRACTICE_CODE          
                  LEFT JOIN FOX_TBL_VISIT_QOUTA_PER_WEEK W ON W.VISIT_QOUTA_WEEK_ID = P.VISIT_QOUTA_WEEK_ID          
                                                              AND ISNULL(W.DELETED, 0) = 0          
                                                              AND W.PRACTICE_CODE = @PRACTICE_CODE          
                  LEFT JOIN FOX_TBL_APPLICATION_USER u ON u.USER_NAME = P.CREATED_BY          
                  LEFT JOIN FOX_TBL_APPLICATION_USER us ON us.USER_NAME = P.MODIFIED_BY       
            
     --LEFT JOIN FOX_TBL_APPLICATION_USER AS AU ON R.REGIONAL_DIRECTOR_ID = AU.USER_ID      
     --   LEFT JOIN FOX_TBL_APPLICATION_USER AS U3 ON R.SENIOR_REGIONAL_DIRECTOR_ID = U3.USER_ID       
         
             WHERE P.PRACTICE_CODE = @PRACTICE_CODE          
                   AND ISNULL(P.DELETED, 0) = 0          
                   AND (@FIRST_NAME IS NULL          
                        OR (P.FIRST_NAME LIKE '%'+@FIRST_NAME+'%'))          
             AND (@LAST_NAME IS NULL          
                        OR (P.LAST_NAME LIKE '%'+@LAST_NAME+'%'))          
       AND (@EMAIL IS NULL          
                        OR (P.EMAIL LIKE '%'+@EMAIL+'%'))  
                   AND (@RT_CODE IS NULL          
                        OR (P.FOX_PROVIDER_CODE LIKE '%'+@RT_CODE+'%'))          
                   AND (@NPI IS NULL          
                        OR (P.INDIVIDUAL_NPI LIKE '%'+@NPI+'%'))          
                   AND (@DISCIPLINE IS NULL          
                        OR (P.DISCIPLINE_ID LIKE '%'+@DISCIPLINE+'%'))          
                   AND (@TREATING_REGION IS NULL          
                        OR (R.REFERRAL_REGION_NAME LIKE '%'+@TREATING_REGION+'%'))          
                   AND (@TREATING_LOCATION IS NULL          
                        OR (L.NAME LIKE '%'+@TREATING_LOCATION+'%'))          
                   AND (P.PROVIDER_CODE LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.FIRST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.LAST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
       OR P.EMAIL LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.INDIVIDUAL_NPI LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR R.REFERRAL_REGION_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.STATE LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR L.NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR L.CODE LIKE '%%'+@SEARCH_STRING+'%%'          
      OR P.CLR LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR D.NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.PRIMARY_POS_DISTANCE LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR W.VISIT_QOUTA_WEEK LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.ACTIVE_CASES LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.PTO_HRS LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.CREATED_BY LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.MODIFIED_BY LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR P.PTO_HRS LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR u.LAST_NAME+', '+u.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'          
                        OR us.LAST_NAME+', '+us.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'          
                        OR u.FIRST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR u.LAST_NAME LIKE '%%'+@SEARCH_STRING+'%%'      
      OR R.ACCOUNT_MANAGER LIKE '%%' + @SEARCH_STRING + '%%'      
      OR AU.FIRST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
      OR AU.LAST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
      OR U3.FIRST_NAME LIKE '%%' + @SEARCH_STRING + '%%'      
      OR U3.LAST_NAME LIKE '%%' + @SEARCH_STRING + '%%'          
      OR u.EMAIL LIKE '%%'+@SEARCH_STRING+'%%'          
      OR us.EMAIL LIKE '%'+@SEARCH_STRING+'%'          
                        OR us.FIRST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR us.LAST_NAME LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR p.FOX_PROVIDER_CODE LIKE '%%'+@SEARCH_STRING+'%%'          
                        OR CONVERT(VARCHAR(10), P.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'          
                        OR CONVERT(VARCHAR(10), P.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'          
      OR p.STATUS LIKE '%%'+@SEARCH_STRING+'%%')          
                   AND ISNULL(P.DELETED, 0) = 0          
         ) AS PROVIDERS          
         ORDER BY CASE          
                      WHEN @SORT_BY = 'Code'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN FOX_PROVIDER_CODE          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'Code'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN FOX_PROVIDER_CODE          
                  END DESC,          
       CASE          
                      WHEN @SORT_BY = 'FIRST_NAME'          
             AND @SORT_ORDER = 'ASC'          
                      THEN FIRST_NAME          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'FIRST_NAME'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN FIRST_NAME          
                  END DESC,          
       CASE          
                      WHEN @SORT_BY = 'CLR'          
                           AND @SORT_ORDER = 'ASC'       
                      THEN CLR          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'CLR'          
                      AND @SORT_ORDER = 'DESC'          
                      THEN CLR          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'LAST_NAME'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN LAST_NAME          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'LAST_NAME'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN LAST_NAME          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'INDIVIDUAL_NPI'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN INDIVIDUAL_NPI          
                  END ASC,          
                 CASE          
                      WHEN @SORT_BY = 'INDIVIDUAL_NPI'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN INDIVIDUAL_NPI          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'REFERRAL_REGION_NAME'          
  AND @SORT_ORDER = 'ASC'          
                      THEN REFERRAL_REGION_NAME          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'REFERRAL_REGION_NAME'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN REFERRAL_REGION_NAME          
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
                      WHEN @SORT_BY = 'SENIOR_REGIONAL_DIRECTOR'          
                     AND @SORT_ORDER = 'ASC'          
                      THEN SENIOR_REGIONAL_DIRECTOR          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'SENIOR_REGIONAL_DIRECTOR'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN SENIOR_REGIONAL_DIRECTOR          
                  END DESC,         
      CASE          
                      WHEN @SORT_BY = 'REGIONAL_DIRECTOR'          
                     AND @SORT_ORDER = 'ASC'          
                      THEN REGIONAL_DIRECTOR          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'REGIONAL_DIRECTOR'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN REGIONAL_DIRECTOR          
                  END DESC,      
      CASE          
                      WHEN @SORT_BY = 'ACCOUNT_MANAGER'          
              AND @SORT_ORDER = 'ASC'          
                      THEN ACCOUNT_MANAGER          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'ACCOUNT_MANAGER'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN ACCOUNT_MANAGER          
                  END DESC,                   
                  CASE          
                      WHEN @SORT_BY = 'TREATMENT_LOCATION'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN TREATMENT_LOCATION          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'TREATMENT_LOCATION'       
                           AND @SORT_ORDER = 'DESC'          
                      THEN TREATMENT_LOCATION          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'DISPLINE_NAME'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN DISPLINE_NAME          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'DISPLINE_NAME'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN DISPLINE_NAME          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'PRIMARY_POS_DISTANCE'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN PRIMARY_POS_DISTANCE          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'PRIMARY_POS_DISTANCE'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN PRIMARY_POS_DISTANCE          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'VISIT_QOUTA_WEEK'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN VISIT_QOUTA_WEEK          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'VISIT_QOUTA_WEEK'          
           AND @SORT_ORDER = 'ASC'          
                      THEN VISIT_QOUTA_WEEK          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'ACTIVE_CASES'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN ACTIVE_CASES          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'ACTIVE_CASES'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN ACTIVE_CASES          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'PTO_HRS'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN PTO_HRS          
                  END DESC,          
                  CASE          
               WHEN @SORT_BY = 'PTO_HRS'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN PTO_HRS          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'CREATED_DATE'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN CREATED_DATE          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'CREATED_DATE'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN CREATED_DATE          
                  END ASC,          
                  CASE          
                     WHEN @SORT_BY = 'MODIFIED_BY'          
                           AND @SORT_ORDER = 'DESC'          
          THEN MODIFIED_BYNAME          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'MODIFIED_BY'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN MODIFIED_BYNAME          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'CREATED_BYNAME'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN MODIFIED_DATE          
                  END DESC,          
    CASE          
                   WHEN @SORT_BY = 'CREATED_BYNAME'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN MODIFIED_DATE          
                  END ASC,          
      CASE          
                      WHEN @SORT_BY = 'Status'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN STATUS          
                  END DESC,          
                  CASE          
                      WHEN @SORT_BY = 'Status'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN STATUS          
                  END ASC          
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;          
     END;