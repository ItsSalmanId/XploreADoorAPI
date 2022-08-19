IF (OBJECT_ID('FOX_PROC_GET_INDEXED_QUEUE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXED_QUEUE 
GO
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INDEXED_QUEUE] --1011163,1,0,10,'','','','AssignTo','ASC','1163TESTING'            
(@PRACTICE_CODE   BIGINT,     
 @CURRENT_PAGE    INT,     
 @INCLUDE_ARCHIVE BIT,     
 @RECORD_PER_PAGE INT,     
 @SEARCH_TEXT     VARCHAR(30),     
 @SORCE_TYPE      VARCHAR(50),     
 @INDEXED_BY      VARCHAR(100),     
 @SORT_BY         VARCHAR(50),     
 @SORT_ORDER      VARCHAR(20),     
 @USER_NAME       VARCHAR(50)  = NULL    
)  
WITH RECOMPILE  
AS    
     BEGIN    
         SET NOCOUNT ON;    
         DECLARE @USERROLE VARCHAR(50);    
         SET @USERROLE =    
         (    
             SELECT R.ROLE_NAME    
             FROM FOX_TBL_APPLICATION_USER UR    
                  INNER JOIN FOX_TBL_ROLE R ON R.ROLE_ID = UR.ROLE_ID    
             WHERE UPPER(USER_NAME) = UPPER(@USER_NAME)    
         );    
         IF(@INDEXED_BY = '')    
             BEGIN    
                 SET @INDEXED_BY = NULL;    
             END;    
             ELSE    
             BEGIN    
                 SET @INDEXED_BY = '%'+@INDEXED_BY+'%';    
             END;    
         IF(@SORCE_TYPE = '')    
             BEGIN    
                 SET @SORCE_TYPE = NULL;    
             END;    
             ELSE    
             BEGIN    
                 SET @SORCE_TYPE = '%'+@SORCE_TYPE+'%';    
             END;    
         DECLARE @ArchiveDate DATE= GETDATE() - 30;    
         IF(@INCLUDE_ARCHIVE = 1)    
             BEGIN    
                 SET @ArchiveDate = GETDATE() - 120;    
             END;    
    
         --Page Count    
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;    
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;    
         DECLARE @TOATL_PAGESUDM FLOAT;    
         SELECT @TOATL_PAGESUDM = COUNT(*)    
         FROM FOX_TBL_WORK_QUEUE wq    
              JOIN FOX_TBL_APPLICATION_USER au ON wq.INDEXED_BY = au.USER_NAME    
         WHERE(SORCE_NAME LIKE '%'+@SEARCH_TEXT+'%'    
               OR SORCE_TYPE LIKE '%'+@SEARCH_TEXT+'%'    
               OR WORK_ID LIKE '%'+@SEARCH_TEXT+'%'    
               OR SORCE_TYPE LIKE '%'+@SEARCH_TEXT+'%'    
               OR WORK_ID LIKE '%'+@SEARCH_TEXT+'%'    
               OR au.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'    
               OR au.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'    
               OR ASSIGNED_BY LIKE '%'+@SEARCH_TEXT+'%'  
             
               OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.MODIFIED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.CREATED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
               OR CONVERT(VARCHAR, wq.CREATED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
               OR (CASE    
                       WHEN isnull(wq.DOCUMENT_TYPE, 0) = 1    
                       THEN 'POC'    
                       WHEN isnull(wq.DOCUMENT_TYPE, 0) = 2    
                       THEN 'Refferal Order'    
                       WHEN isnull(wq.DOCUMENT_TYPE, 0) = 3    
                       THEN 'Other'    
                       ELSE ''    
                   END) LIKE '%'+@SEARCH_TEXT+'%'    
               OR (CONVERT(VARCHAR, DATEDIFF(HOUR, ASSIGNED_DATE, INDEXED_DATE))+':'+CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, INDEXED_DATE) % 60)) LIKE '%'+@SEARCH_TEXT+'%'    
               OR (CONVERT(VARCHAR, DATEDIFF(HOUR, INDEXED_DATE, GETDATE()))+':'+CONVERT(VARCHAR, DATEDIFF(MINUTE, INDEXED_DATE, GETDATE()) % 60)) LIKE '%'+@SEARCH_TEXT+'%')    
              AND ISNULL(wq.DELETED, 0) = 0    
              AND (au.FIRST_NAME LIKE ISNULL(@INDEXED_BY, au.FIRST_NAME)    
                   OR au.LAST_NAME LIKE ISNULL(@INDEXED_BY, au.LAST_NAME))   
              AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)    
              AND CAST(wq.RECEIVE_DATE AS DATE) > @ArchiveDate    
     --AND (    
     --  (@INCLUDE_ARCHIVE = 0 AND CAST(wq.RECEIVE_DATE AS DATE) >= @ArchiveDate)    
     -- OR    
     --  (@INCLUDE_ARCHIVE = 1 )     
     --)    
              AND WORK_STATUS = 'Indexed'    
              AND WQ.PRACTICE_CODE = @PRACTICE_CODE    
              AND (@USERROLE = 'SUPERVISOR'    
                   OR @USERROLE = 'ADMINISTRATOR'    
                   OR wQ.ASSIGNED_TO = @USER_NAME);    
         IF(@RECORD_PER_PAGE = 0)    
             BEGIN    
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;    
             END;    
             ELSE    
             BEGIN    
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;    
             END;    
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;    
    
         --Get All Result with page count    
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);    
         SELECT *,     
                @TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES,     
                @TOTAL_RECORDS TOTAL_RECORDS    
         FROM    
         (    
             SELECT WORK_ID,     
                    UNIQUE_ID,     
                    wq.PRACTICE_CODE,     
                    SORCE_TYPE,     
                    SORCE_NAME,     
                    WORK_STATUS,     
                    RECEIVE_DATE,  
     convert(varchar,RECEIVE_DATE) AS Received_Date_Str,     
                    TOTAL_PAGES,     
                    NO_OF_SPLITS,     
                    FILE_PATH,    
                    CASE    
                        WHEN(AU_ASSIGNED_TO.LAST_NAME IS NULL    
                             OR AU_ASSIGNED_TO.LAST_NAME LIKE '')    
                            AND (AU_ASSIGNED_TO.FIRST_NAME IS NULL    
                                 OR AU_ASSIGNED_TO.FIRST_NAME LIKE '')    
                        THEN ''    
                        WHEN AU_ASSIGNED_TO.LAST_NAME IS NULL    
                             OR AU_ASSIGNED_TO.LAST_NAME LIKE ''    
                        THEN AU_ASSIGNED_TO.FIRST_NAME    
                        ELSE AU_ASSIGNED_TO.LAST_NAME+', '+AU_ASSIGNED_TO.FIRST_NAME    
                    END AS ASSIGNED_TO,     
                    ASSIGNED_BY,     
                    ASSIGNED_DATE,     
                    COMPLETED_BY,     
                    COMPLETED_DATE,     
                    wq.CREATED_BY,     
                    wq.CREATED_DATE,     
                    wq.MODIFIED_BY,     
                    wq.MODIFIED_DATE,     
                    wq.DELETED,     
             (    
                 SELECT COUNT(*)    
                 FROM FOX_TBL_WORK_TRANSFER    
                 WHERE WORK_ID = wq.WORK_ID    
             ) AS NO_OF_COMMENTS,    
                    CASE    
                        WHEN isnull(wq.DOCUMENT_TYPE, 0) = 1    
                        THEN 'POC'    
                        WHEN isnull(wq.DOCUMENT_TYPE, 0) = 2    
                        THEN 'Refferal Order'    
                        WHEN isnull(wq.DOCUMENT_TYPE, 0) = 3    
                        THEN 'Other'    
                        ELSE ''    
                    END AS DOCUMENT_TYPE,     
                    ISNULL((CONVERT(VARCHAR, DATEDIFF(minute, ASSIGNED_DATE, INDEXED_DATE) / 60)+':'+CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, INDEXED_DATE) % 60)), '') AS INDEX_COMPLETION_TIME,        
                    --Convert(varchar,DateDiff(HOUR, ASSIGNED_DATE, INDEXED_DATE))+':'+ Convert(varchar,DateDiff(MINUTE, ASSIGNED_DATE, INDEXED_DATE)%60) as INDEX_COMPLETION_TIME,            
                    (CONVERT(VARCHAR, DATEDIFF(minute, INDEXED_DATE, GETDATE()) / 60)+':'+CONVERT(VARCHAR, DATEDIFF(MINUTE, INDEXED_DATE, GETDATE()) % 60)) AS TIME_ELASPE_IN_QUEUE,        
                    --Convert(varchar,DateDiff(HOUR, INDEXED_DATE, GETDATE()))+':'+ Convert(varchar,DateDiff(MINUTE, INDEXED_DATE, GETDATE())%60) as TIME_ELASPE_IN_QUEUE,            
         ROW_NUMBER() OVER(ORDER BY wq.INDEXED_DATE DESC) AS ACTIVEROW,     
                    au.LAST_NAME+', '+au.FIRST_NAME INDEXED_BY,     
                    INDEXED_DATE,     
                    IS_EMERGENCY_ORDER IS_EMERGENCY,     
                    CONVERT(BIT,    
                            CASE    
                                WHEN ISNULL(IsSigned, 0) = 1    
                                     OR wq.CREATED_BY LIKE 'FOX TEAM'    
                                     OR CHARINDEX('_', wq.UNIQUE_ID) > 0    
                                THEN 0    
                                ELSE 1    
                            END) AS IS_UNSIGNED    
             FROM FOX_TBL_WORK_QUEUE wq    
                  JOIN FOX_TBL_APPLICATION_USER au ON wq.INDEXED_BY = au.USER_NAME    
                  LEFT JOIN FOX_TBL_APPLICATION_USER AU_ASSIGNED_TO ON AU_ASSIGNED_TO.USER_NAME = wq.ASSIGNED_TO    
             WHERE(SORCE_NAME LIKE '%'+@SEARCH_TEXT+'%'    
                   OR SORCE_TYPE LIKE '%'+@SEARCH_TEXT+'%'    
                   OR WORK_ID LIKE '%'+@SEARCH_TEXT+'%'    
                   OR au.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'    
                   OR au.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'    
                   OR ASSIGNED_BY LIKE '%'+@SEARCH_TEXT+'%'    
       OR AU_ASSIGNED_TO.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'   
          OR AU_ASSIGNED_TO.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'  
                   OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.MODIFIED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.CREATED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR CONVERT(VARCHAR, wq.CREATED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR (CASE    
                           WHEN isnull(wq.DOCUMENT_TYPE, 0) = 1    
                           THEN 'POC'    
                           WHEN isnull(wq.DOCUMENT_TYPE, 0) = 2    
                           THEN 'Refferal Order'    
                           WHEN isnull(wq.DOCUMENT_TYPE, 0) = 3    
                           THEN 'Other'    
                           ELSE ''    
                       END) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR (CONVERT(VARCHAR, DATEDIFF(HOUR, ASSIGNED_DATE, INDEXED_DATE))+':'+CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, INDEXED_DATE) % 60)) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR (CONVERT(VARCHAR, DATEDIFF(HOUR, INDEXED_DATE, GETDATE()))+':'+CONVERT(VARCHAR, DATEDIFF(MINUTE, INDEXED_DATE, GETDATE()) % 60)) LIKE '%'+@SEARCH_TEXT+'%'    
                   OR TOTAL_PAGES LIKE '%'+@SEARCH_TEXT+'%')    
                  AND ISNULL(wq.DELETED, 0) = 0    
                  AND (au.FIRST_NAME LIKE ISNULL(@INDEXED_BY, au.FIRST_NAME)    
                       OR au.LAST_NAME LIKE ISNULL(@INDEXED_BY, au.LAST_NAME))    
                  AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)    
                  AND CAST(WQ.RECEIVE_DATE AS DATE) > @ArchiveDate    
     -- AND (    
     --  (@INCLUDE_ARCHIVE = 0 AND CAST(wq.RECEIVE_DATE AS DATE) >= @ArchiveDate)    
     -- OR    
     --  (@INCLUDE_ARCHIVE = 1)     
     --)    
     AND WORK_STATUS = 'Indexed'    
                  AND WQ.PRACTICE_CODE = @PRACTICE_CODE    
                  AND (@USERROLE = 'SUPERVISOR'    
                       OR @USERROLE = 'ADMINISTRATOR'    
                       OR wQ.ASSIGNED_TO = @USER_NAME)    
         ) AS WORK_QUEUE    
         ORDER BY CASE    
                      WHEN @SORT_BY = 'id'    
                           AND @SORT_ORDER = 'ASC'    
       THEN UNIQUE_ID    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'id'    
 AND @SORT_ORDER = 'DESC'    
       THEN UNIQUE_ID    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'IndexedBy'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN INDEXED_BY    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'IndexedBy'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN INDEXED_BY    
                  END DESC,  
      CASE    
                      WHEN @SORT_BY = 'AssignTo'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN ASSIGNED_TO    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'AssignTo'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN ASSIGNED_TO    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'SourceType'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN SORCE_TYPE    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'SourceType'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN SORCE_TYPE    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'SourceName'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN SORCE_NAME    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'SourceName'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN SORCE_NAME    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'DateTimeReceived'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN RECEIVE_DATE    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'DateTimeReceived'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN RECEIVE_DATE    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'IndexCompletionTime'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN(DATEDIFF(MINUTE, ASSIGNED_DATE, INDEXED_DATE))    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'IndexCompletionTime'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN(DATEDIFF(MINUTE, ASSIGNED_DATE, INDEXED_DATE))    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'ElapsedQueue'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN(DATEDIFF(MINUTE, INDEXED_DATE, GETDATE()))    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'ElapsedQueue'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN(DATEDIFF(MINUTE, INDEXED_DATE, GETDATE()))    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = 'Pages'    
                           AND @SORT_ORDER = 'ASC'    
                      THEN TOTAL_PAGES    
                  END ASC,    
                  CASE    
                      WHEN @SORT_BY = 'Pages'    
                           AND @SORT_ORDER = 'DESC'    
                      THEN TOTAL_PAGES    
                  END DESC,    
                  CASE    
                      WHEN @SORT_BY = ''    
                           AND @SORT_ORDER = ''    
                      THEN INDEXED_DATE    
                  END DESC    
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;    
     END;  
  
/**************************************************************************************************************************************************************************************************/ 