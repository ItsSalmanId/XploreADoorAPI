IF (OBJECT_ID('FOX_PROC_AUDTIT_SCORES_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_AUDTIT_SCORES_LIST
GO 
  -- =============================================              
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>              
-- CREATE DATE: <CREATE DATE, 12/04/2018>              
-- DESCRIPTION: <FOX PROC AUDTIT SCORES LIST>              
--  EXEC FOX_PROC_AUDTIT_SCORES_LIST  1011163,'','','','12/10/2020','12/12/2020',1,10,'Rep','DESC','PHD','544106'          
--  EXEC FOX_PROC_AUDTIT_SCORES_LIST_RAFAY  null,1011163,'','','','12/10/1990','12/12/2020',1,10,'Rep','DESC','PHD',0                        
CREATE PROCEDURE [dbo].[FOX_PROC_AUDTIT_SCORES_LIST]     
              @PATIENT_ACCOUNT   BIGINT,     
              @PRACTICE_CODE   BIGINT,    
                                                     @SEARCH_TEXT     VARCHAR(100),             
                                                     @AGENT_NAME      VARCHAR(100),             
                                                     @AUDITOR_NAME    VARCHAR(100),             
                                                     @DATE_FROM       DATETIME,             
                                                     @DATE_TO         DATETIME,             
                                                     @CURRENT_PAGE    INT,             
                                                     @RECORD_PER_PAGE INT,             
                                                     @SORT_BY         VARCHAR(50),             
                                                     @SORT_ORDER      VARCHAR(5),             
                                                     @CALL_TYPE       VARCHAR(20),          
              @PHD_CALL_SCENARIO_ID  VARCHAR(100)            
AS            
     BEGIN            
         IF(@SEARCH_TEXT = '')            
             BEGIN            
                 SET @SEARCH_TEXT = NULL;            
             END;            
             ELSE            
             BEGIN            
                 SET @SEARCH_TEXT = '%'+@SEARCH_TEXT+'%';            
             END;            
         IF(@AGENT_NAME = '')            
             BEGIN            
                 SET @AGENT_NAME = NULL;            
             END;            
             ELSE            
             BEGIN            
                 SET @AGENT_NAME = @AGENT_NAME;            
             END;            
         IF(@AUDITOR_NAME = '')            
             BEGIN            
                 SET @AUDITOR_NAME = NULL;            
             END;            
             ELSE            
             BEGIN            
                 SET @AUDITOR_NAME = @AUDITOR_NAME;            
             END;            
         IF(@CALL_TYPE = 'ALL')            
             BEGIN            
                 SET @CALL_TYPE = NULL;            
             END;            
             ELSE            
         IF(@CALL_TYPE = 'PHD')            
             BEGIN            
                 SET @CALL_TYPE = 'phd';            
             END;            
             ELSE            
         IF(@CALL_TYPE = 'SURVEY')            
             BEGIN            
                 SET @CALL_TYPE = 'survey';            
             END;            
        IF(@PATIENT_ACCOUNT = 0)            
             BEGIN            
                 SET @PATIENT_ACCOUNT = null;            
             END;    
         --                      
         --  IF OBJECT_ID('TEMPDB.DBO.#AUDDATA', 'U') IS NOT NULL DROP TABLE #AUDDATA                
         IF OBJECT_ID('TEMPDB.DBO.#AUDDATA1', 'U') IS NOT NULL            
             DROP TABLE #AUDDATA1;            
            
         --IF(@AUDITOR_NAME <> '')               
         --begin              
         -- SELECT AUDITOR_NAME               
         -- ,AGENT_NAME               
         -- ,CAST(AVG(CAST(TOTAL_POINTS AS decimal(5,1)))  AS decimal(5,1)) AS AUDITER_AVG               
         -- ,CAST(COUNT(SURVEY_AUDIT_SCORES_ID) AS bigint) AS AUDITER_EVALUATIONS              
         -- ,CAST(AVG(CAST(CLIENT_EXPERIENCE_TOTAL AS decimal(5,1))) AS decimal(5,1)) AS CLIENT_EXPERIENCE_AUDITER_TOTAL              
         -- ,CAST(AVG(CAST(SYSTEM_PROCESS_TOTAL AS decimal(5,1))) AS decimal(5,1)) AS SYSTEM_PROCESS_AUDITER_TOTAL              
   -- ,CAST(((SUM(CAST(BONUS_POINTS AS decimal(5,1))) - SUM(CAST(PERFORMANCE_KILLER AS decimal(5,1)))) /COUNT(SURVEY_AUDIT_SCORES_ID))  AS decimal(5,1)) AS AUDITER_WOW_FACTOR               
         --INTO #AUDDATA              
         --FROM FOX_TBL_SURVEY_AUDIT_SCORES              
         --WHERE AUDITOR_NAME = @AUDITOR_NAME              
         --GROUP BY AUDITOR_NAME, AGENT_NAME              
            
         SELECT AGENT_NAME,             
                CAST(AVG(CAST(TOTAL_POINTS AS DECIMAL(5, 1))) AS DECIMAL(5, 1)) AS TOTAL_AVG,             
                CAST(COUNT(SURVEY_AUDIT_SCORES_ID) AS BIGINT) AS AVG_EVALUATIONS,             
                CAST(AVG(CAST(CLIENT_EXPERIENCE_TOTAL AS DECIMAL(5, 1))) AS DECIMAL(5, 1)) AS CLIENT_EXPERIENCE_AVG_TOTAL,             
                CAST(AVG(CAST(SYSTEM_PROCESS_TOTAL AS DECIMAL(5, 1))) AS DECIMAL(5, 1)) AS SYSTEM_PROCESS_AVG_TOTAL,             
                CAST(((SUM(CAST(BONUS_POINTS AS DECIMAL(5, 1))) - SUM(CAST(PERFORMANCE_KILLER AS DECIMAL(5, 1)))) / COUNT(SURVEY_AUDIT_SCORES_ID)) AS DECIMAL(5, 1)) AS AVG_WOW_FACTOR            
         INTO #AUDDATA1            
             
         FROM FOX_TBL_SURVEY_AUDIT_SCORES AS S          
   LEFT JOIN FOX_TBL_APPLICATION_USER U        
   ON U.USER_NAME = S.CREATED_BY         
   LEFT JOIN FOX_TBL_PHD_CALL_DETAILS C           
    ON C.FOX_PHD_CALL_DETAILS_ID = S.PHD_CALL_ID               
    LEFT JOIN FOX_TBL_PHD_CALL_REASON R on R.PHD_CALL_REASON_ID =  C.CALL_REASON              
    LEFT JOIN FOX_TBL_PHD_CALL_SCENARIO AS SR              
     ON C.CALL_SCENARIO = SR.PHD_CALL_SCENARIO_ID AND ISNULL(SR.DELETED,0) = 0        
  WHERE U.DELETED = 0        
  --ISNULL(U.DELETED,0) = 0              
         GROUP BY AGENT_NAME;              
         --end              
            
         SELECT S.AGENT_NAME,             
                (U.FIRST_NAME+' '+U.LAST_NAME) AS NAME,             
                ROW_NUMBER() OVER(ORDER BY AGENT_NAME DESC) AS ACTIVEROW,             
                CAST(COUNT(SURVEY_AUDIT_SCORES_ID) AS BIGINT) AS EVALUATIONS,             
                CAST(AVG(CAST(CLIENT_EXPERIENCE_TOTAL AS DECIMAL(5, 1))) AS DECIMAL(5, 1)) AS CLIENT_EXPERIENCE_TOTAL,             
                CAST(AVG(CAST(SYSTEM_PROCESS_TOTAL AS DECIMAL(5, 1))) AS DECIMAL(5, 1)) AS SYSTEM_PROCESS_TOTAL,             
                CAST(AVG(CAST(TOTAL_POINTS AS DECIMAL(5, 1))) AS DECIMAL(5, 1)) AS TOTAL_POINTS,             
                CAST(((SUM(CAST(BONUS_POINTS AS DECIMAL(5, 1))) - SUM(CAST(PERFORMANCE_KILLER AS DECIMAL(5, 1)))) / COUNT(SURVEY_AUDIT_SCORES_ID)) AS DECIMAL(5, 1)) AS WOW_FACTOR            
         INTO #TEMPAGENTDATA            
         FROM FOX_TBL_SURVEY_AUDIT_SCORES S            
              LEFT JOIN fox_tbl_application_user U ON U.USER_NAME = S.AGENT_NAME            
                                                      AND ISNULL(U.DELETED, 0) = 0           
    LEFT JOIN FOX_TBL_PHD_CALL_DETAILS C           
    ON C.FOX_PHD_CALL_DETAILS_ID = S.PHD_CALL_ID               
    LEFT JOIN FOX_TBL_PHD_CALL_REASON R on R.PHD_CALL_REASON_ID =  C.CALL_REASON              
    LEFT JOIN FOX_TBL_PHD_CALL_SCENARIO AS SR              
     ON C.CALL_SCENARIO = SR.PHD_CALL_SCENARIO_ID AND ISNULL(SR.DELETED,0) = 0              
                          
         WHERE(@SEARCH_TEXT IS NULL              
               --OR S.AGENT_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
            
--OR AVG(CLIENT_EXPERIENCE_TOTAL) LIKE '%'+@SEARCH_TEXT+'%'            
              --OR ((SUM(BONUS_POINTS) - SUM(PERFORMANCE_KILLER)) /COUNT(SURVEY_AUDIT_SCORES_ID)) LIKE '%'+@SEARCH_TEXT+'%'            
              --OR AVG(SYSTEM_PROCESS_TOTAL) LIKE '%'+@SEARCH_TEXT+'%'            
              --OR AVG(TOTAL_POINTS) LIKE '%'+@SEARCH_TEXT+'%'            
              --OR COUNT(SURVEY_AUDIT_SCORES_ID) LIKE '%'+@SEARCH_TEXT+'%'        
                
              )            
              AND S.PRACTICE_CODE = @PRACTICE_CODE         
     AND  U.DELETED = 0          
     --AND ISNULL(U.DELETED,0) = 0             
              AND ISNULL(S.DELETED, 0) = 0            
              AND (@AGENT_NAME IS NULL            
                   OR S.AGENT_NAME = @AGENT_NAME)            
              AND (@AUDITOR_NAME IS NULL            
                   OR S.AUDITOR_NAME = @AUDITOR_NAME)            
              AND (@CALL_TYPE IS NULL            
                   OR S.CALL_TYPE = @CALL_TYPE)       
        AND (@PATIENT_ACCOUNT IS NULL            
                   OR S.PATIENT_ACCOUNT = @PATIENT_ACCOUNT)    
         AND (@DATE_FROM IS NULL            
                   OR @DATE_TO IS NULL            
                   OR CONVERT(DATE, S.CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))           
     AND  (@PHD_CALL_SCENARIO_ID = 0 OR SR.PHD_CALL_SCENARIO_ID  = @PHD_CALL_SCENARIO_ID)         
  AND ISNULL(U.DELETED, 0) = 0           
      --AND ISNULL(R.DELETED,0) = 0 AND ISNULL(SR.DELETED,0) = 0           
      --AND R.PRACTICE_CODE = @PRACTICE_CODE AND SR.PRACTICE_CODE = @PRACTICE_CODE          
               
         GROUP BY S.AGENT_NAME,             
                  (U.FIRST_NAME+' '+U.LAST_NAME);            
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;            
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;            
         DECLARE @TOATL_PAGESUDM FLOAT;            
         SELECT @TOATL_PAGESUDM = COUNT(*)            
         FROM #TEMPAGENTDATA;            
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
         SELECT *,             
                CAST(@TOATL_PAGESUDM AS INT) AS TOTAL_RECORD_PAGES,             
                CAST(@TOTAL_RECORDS AS INT) AS TOTAL_RECORDS            
         INTO #pagingation            
         FROM #TEMPAGENTDATA            
         ORDER BY CASE            
                      WHEN @SORT_BY = 'Rep'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN NAME            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'Rep'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN NAME            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'Overall'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN TOTAL_POINTS            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'Overall'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN TOTAL_POINTS            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'Client'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN CLIENT_EXPERIENCE_TOTAL            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'Client'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN CLIENT_EXPERIENCE_TOTAL       
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'System'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN SYSTEM_PROCESS_TOTAL            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'System'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN SYSTEM_PROCESS_TOTAL            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'Wow'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN WOW_FACTOR            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'Wow'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN WOW_FACTOR            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'Evaluation'            
                           AND @SORT_ORDER = 'ASC'            
    THEN EVALUATIONS            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'Evaluation'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN EVALUATIONS            
                  END DESC            
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;            
         IF(@AUDITOR_NAME <> '')            
             BEGIN            
                 IF OBJECT_ID('TEMPDB.DBO.#DATA_WITH_AUDITOR', 'U') IS NOT NULL            
                     DROP TABLE #DATA_WITH_AUDITOR;            
                 SELECT J.TOTAL_AVG,             
                        J.CLIENT_EXPERIENCE_AVG_TOTAL,             
                        J.SYSTEM_PROCESS_AVG_TOTAL,             
                        J.AVG_WOW_FACTOR,             
                        J.AGENT_NAME,             
                        J.AVG_EVALUATIONS,             
                        A.NAME,             
                        A.ACTIVEROW,             
                        EVALUATIONS,             
                        A.CLIENT_EXPERIENCE_TOTAL,             
                        A.SYSTEM_PROCESS_TOTAL,             
                        A.TOTAL_POINTS,             
              A.WOW_FACTOR,             
                        A.TOTAL_RECORD_PAGES,             
                        A.TOTAL_RECORDS            
                 INTO #DATA_WITH_AUDITOR            
                 FROM #AUDDATA1 J            
                      JOIN #pagingation A ON j.AGENT_NAME = A.AGENT_NAME;            
                 SELECT AGENT_NAME,             
                        NAME,             
                        ACTIVEROW,             
                        CAST(TOTAL_POINTS AS DECIMAL(5, 1)) AS TOTAL_POINTS,             
                        CAST(CLIENT_EXPERIENCE_TOTAL AS DECIMAL(5, 1)) AS CLIENT_EXPERIENCE_TOTAL,             
                        CAST(SYSTEM_PROCESS_TOTAL AS DECIMAL(5, 1)) AS SYSTEM_PROCESS_TOTAL,             
                        CAST(WOW_FACTOR AS DECIMAL(5, 1)) AS WOW_FACTOR,             
                        CAST(EVALUATIONS AS VARCHAR) AS EVALUATIONS,             
                        CAST(TOTAL_AVG AS DECIMAL(5, 1)) AS TOTAL_AVG,             
                        CAST(CLIENT_EXPERIENCE_AVG_TOTAL AS DECIMAL(5, 1)) AS CLIENT_EXPERIENCE_AVG_TOTAL,             
                        CAST(SYSTEM_PROCESS_AVG_TOTAL AS DECIMAL(5, 1)) AS SYSTEM_PROCESS_AVG_TOTAL,             
                        CAST(AVG_WOW_FACTOR AS DECIMAL(5, 1)) AS AVG_WOW_FACTOR,             
                        CAST(AVG_EVALUATIONS AS VARCHAR) AS AVG_EVALUATIONS            
                        ,            
            
                        --,convert(varchar,TOTAL_POINTS) +'     ' +'/'+'    '+  convert(varchar,TOTAL_AVG) AS TOTAL_POINTS               
                        --,convert(varchar,CLIENT_EXPERIENCE_TOTAL) +'     ' +'/'+'    '+  convert(varchar,CLIENT_EXPERIENCE_AVG_TOTAL) AS CLIENT_EXPERIENCE_TOTAL                
                        --,convert(varchar,SYSTEM_PROCESS_TOTAL)+'     ' +'/'+'    '+ convert(varchar,SYSTEM_PROCESS_AVG_TOTAL) AS SYSTEM_PROCESS_TOTAL                
                        --,convert(varchar,WOW_FACTOR)+'     ' +'/'+'    '+ convert(varchar,AVG_WOW_FACTOR) AS WOW_FACTOR                
                        --,convert(varchar,EVALUATIONS)+'     ' +'/'+'    '+ convert(varchar,AVG_EVALUATIONS) AS EVALUATIONS               
                        TOTAL_RECORD_PAGES,             
                        TOTAL_RECORDS            
                 FROM #DATA_WITH_AUDITOR;            
             END;            
             ELSE            
             BEGIN            
                 IF OBJECT_ID('TEMPDB.DBO.#DATA_WITHOUT_AUDITOR', 'U') IS NOT NULL            
                     DROP TABLE #DATA_WITHOUT_AUDITOR;            
                 SELECT A.AGENT_NAME,             
                        A.NAME,             
                        A.ACTIVEROW,             
                        A.EVALUATIONS,             
                        A.CLIENT_EXPERIENCE_TOTAL,             
                        A.SYSTEM_PROCESS_TOTAL,             
                A.TOTAL_POINTS,             
                        A.WOW_FACTOR,             
                        A.TOTAL_RECORD_PAGES,             
                        A.TOTAL_RECORDS            
                 INTO #DATA_WITHOUT_AUDITOR            
                 FROM #AUDDATA1 J            
                      JOIN #pagingation A ON j.AGENT_NAME = A.AGENT_NAME;            
                 SELECT AGENT_NAME,             
                        NAME,             
                        ACTIVEROW,             
                        TOTAL_POINTS,             
                        CLIENT_EXPERIENCE_TOTAL,           
                        SYSTEM_PROCESS_TOTAL,             
                        WOW_FACTOR,             
                        CAST(EVALUATIONS AS VARCHAR) AS EVALUATIONS            
                        ,            
                        --,convert(varchar,TOTAL_POINTS) AS TOTAL_POINTS            
                        --,convert(varchar,CLIENT_EXPERIENCE_TOTAL) AS CLIENT_EXPERIENCE_TOTAL            
                        --,convert(varchar,SYSTEM_PROCESS_TOTAL) AS SYSTEM_PROCESS_TOTAL            
                        --,convert(varchar,WOW_FACTOR) AS WOW_FACTOR            
                        --,convert(varchar,EVALUATIONS) AS EVALUATIONS                
                        TOTAL_RECORD_PAGES,             
                        TOTAL_RECORDS            
                 FROM #DATA_WITHOUT_AUDITOR;            
             END;            
     END;  
  
