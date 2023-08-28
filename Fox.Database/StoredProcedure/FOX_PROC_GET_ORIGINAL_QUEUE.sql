            
-- AUTHOR:  <AUTHOR,MEHMOOD UL HASSAN>                              
-- CREATE DATE: <CREATE DATE,12/10/2017>                              
-- DESCRIPTION: <GET ORIGINAL QUEUE>                  
-- FOX_PROC_GET_ORIGINAL_QUEUE_10302022 1012714,1,null,null,1,50,'','','','','','',''                  
--EXEC FOX_PROC_GET_ORIGINAL_QUEUE 1012714,1,null,null,1,100,'','','','','53432809','',''                       
                
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ORIGINAL_QUEUE]                  
                     
(@PRACTICE_CODE   BIGINT,                   
 @CURRENT_PAGE    INT,                   
 @DATE_FROM       DATETIME,                   
 @DATE_TO         DATETIME,                   
 @INCLUDE_ARCHIVE BIT,                   
 @RECORD_PER_PAGE INT,                   
 @SEARCH_GO       VARCHAR(30),                     
 @STATUS_TEXT     VARCHAR(50),                  
 @SORCE_STRING    VARCHAR(100),                   
 @SORCE_TYPE      VARCHAR(50),                    
 @WORK_ID         VARCHAR(50),                    
 @SORT_BY         VARCHAR(50),                   
 @SORT_ORDER      VARCHAR(5)                  
)                  
--declare @PRACTICE_CODE   BIGINT = 1012714,                   
-- @CURRENT_PAGE    INT = 1,                   
-- @DATE_FROM       DATETIME = null,                   
-- @DATE_TO         DATETIME = null,                   
-- @INCLUDE_ARCHIVE BIT = 1,                   
-- @RECORD_PER_PAGE INT = 100000,                   
-- @SEARCH_GO       VARCHAR(30) = '',                     
-- @STATUS_TEXT     VARCHAR(50) = '',                  
-- @SORCE_STRING    VARCHAR(100) = '',                   
-- @SORCE_TYPE      VARCHAR(50) = 'Email',                    
-- @WORK_ID         VARCHAR(50) = '',                    
-- @SORT_BY         VARCHAR(50) = '',                   
-- @SORT_ORDER      VARCHAR(5) = '';                   
                
AS                  
BEGIN                  
   SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                    
         SET NOCOUNT ON;                  
                    
                    
   IF(@STATUS_TEXT  = 'completed' or @STATUS_TEXT  = '' or @STATUS_TEXT  = 'Trashed' )                          
   BEGIN                          
    SET @STATUS_TEXT = @STATUS_TEXT                          
   END                          
   ELSE                           
   BEGIN                          
     SET @STATUS_TEXT =  null                          
   END                      
         IF(@SORCE_STRING = '')                  
             BEGIN                  
                 SET @SORCE_STRING = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @SORCE_STRING = '%'+@SORCE_STRING+'%';                  
             END;                     
    IF(@WORK_ID = '' OR @WORK_ID = '0')                  
             BEGIN                  
                 SET @WORK_ID = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @WORK_ID = @WORK_ID;                        
             END;                               
             IF(@SORCE_TYPE = '')                  
             BEGIN                  
                 SET @SORCE_TYPE = NULL;                  
             END;                  
             ELSE                  
             BEGIN                               
                 SET @SORCE_TYPE = '%'+@SORCE_TYPE+'%';                  
             END;                       
                              
    IF CHARINDEX('(',@SEARCH_GO) > 0                  
    BEGIN                  
    SET @SEARCH_GO = REPLACE(@SEARCH_GO,'(', '')                       
    END                  
    IF CHARINDEX(')',@SEARCH_GO) > 0                  
    BEGIN                  
    SET @SEARCH_GO = REPLACE(@SEARCH_GO,')', '')                  
    END                  
      IF CHARINDEX(' ',@SEARCH_GO) > 0               
    BEGIN                  
    SET @SEARCH_GO = REPLACE(@SEARCH_GO,' ', '')                  
    END                  
      IF CHARINDEX('-',@SEARCH_GO) > 0                  
    BEGIN                  
    SET @SEARCH_GO = REPLACE(@SEARCH_GO,'-', '')                  
    END                  
         IF(@SORCE_TYPE = '')                  
             BEGIN                 
                 SET @SORCE_TYPE = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @SORCE_TYPE = '%'+@SORCE_TYPE+'%';                  
 END;                  
         DECLARE @ARCHIVEDATE DATE= GETDATE() - 30;           
         IF (@INCLUDE_ARCHIVE = 1)                   
         BEGIN               
          SET @ARCHIVEDATE = GETDATE() - 120                    
         END                
         -------------------------------------                      
         IF OBJECT_ID('TEMPDB..#COMPLETEDINITIAL') IS NOT NULL DROP TABLE #COMPLETEDINITIAL;                  
             SELECT WORK_ID,                  
                CASE                  
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                  
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                  
ELSE UNIQUE_ID                  
                END UNIQUE_ID,                  
                UNIQUE_ID UNIQUE_IDD                    
         INTO #COMPLETEDINITIAL                  
         FROM FOX_TBL_WORK_QUEUE WITH (NOLOCK)                  
         WHERE ISNULL(DELETED, 0) = 0                  
           AND UPPER(WORK_STATUS) = UPPER('COMPLETED') AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE);                  
         IF OBJECT_ID('TEMPDB..#COMPLETED') IS NOT NULL DROP TABLE #COMPLETED;                  
SELECT UNIQUE_ID,                  
                COUNT(UNIQUE_ID) 'NO_OF_COMPLETED'                  
         INTO #COMPLETED                  
         FROM #COMPLETEDINITIAL                  
         GROUP BY UNIQUE_ID;                      
         ------------------------------------------------                           
         IF OBJECT_ID('TEMPDB..#PENDINGINITIAL') IS NOT NULL DROP TABLE #PENDINGINITIAL;                  
         SELECT WORK_ID,                  
                CASE                  
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                  
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                  
                    ELSE UNIQUE_ID                  
                END UNIQUE_ID,                   
                UNIQUE_ID UNIQUE_IDD                  
         INTO #PENDINGINITIAL                  
         FROM FOX_TBL_WORK_QUEUE  WITH (NOLOCK)                  
         WHERE ISNULL(DELETED, 0) = 0                  
               AND (UPPER(WORK_STATUS) <> UPPER('INDEXED') AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE));                  
         IF OBJECT_ID('TEMPDB..#PENDING') IS NOT NULL DROP TABLE #PENDING;                  
         SELECT UNIQUE_ID,                   
                COUNT(UNIQUE_ID) 'NO_OF_PENDING'                  
         INTO #PENDING                  
         FROM #PENDINGINITIAL                  
         GROUP BY UNIQUE_ID;                  
         ------------------------------------------                      
         IF OBJECT_ID('TEMPDB..#TOTAL_PAGES_INITIAL') IS NOT NULL DROP TABLE #TOTAL_PAGES_INITIAL;                  
         SELECT WORK_ID,                   
                TOTAL_PAGES,                  
                CASE                  
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                  
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                  
                    ELSE UNIQUE_ID                  
                END UNIQUE_ID,                   
                UNIQUE_ID UNIQUE_IDD                  
         INTO #TOTAL_PAGES_INITIAL            
         FROM FOX_TBL_WORK_QUEUE WITH (NOLOCK)                   
         WHERE ISNULL(DELETED, 0) = 0  AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE);                  
         IF OBJECT_ID('TEMPDB..#TOTAL_PAGES') IS NOT NULL DROP TABLE #TOTAL_PAGES;                  
         SELECT UNIQUE_ID,                  
   COUNT(UNIQUE_ID) NO_OF_SPLITS,                   
                SUM(TOTAL_PAGES) TOTAL_PAGES                  
         INTO #TOTAL_PAGES                  
         FROM #TOTAL_PAGES_INITIAL                  
         GROUP BY UNIQUE_ID;         
         ------------------------------------            
   IF OBJECT_ID('TEMPDB..#WORK_ORDERS') IS NOT NULL DROP TABLE #WORK_ORDERS;        
   SELECT * INTO #WORK_ORDERS FROM FOX_TBL_WORK_QUEUE         
   WHERE CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE);        
                       
   ------------------------------------            
         IF OBJECT_ID('TEMPDB..#TEMPRECORDS') IS NOT NULL DROP TABLE #TEMPRECORDS;                     
SELECT   WORK_ID,                   
                B.UNIQUE_ID,                   
                B.PRACTICE_CODE,                   
                SORCE_TYPE,                   
                SORCE_NAME,                   
                CASE                  
                WHEN TP.NO_OF_SPLITS = NO_OF_COMPLETED                  
                THEN 'Completed'                  
                WHEN NO_OF_COMPLETED > 0                  
                THEN 'Partially Done'      
    WHEN (IS_TRASH_REFERRAL = 1 AND @STATUS_TEXT  != 'pending') THEN 'Trashed'       
                ELSE 'Pending'                  
                END WORK_STATUS,  
    CASE                    
    WHEN IS_TRASH_REFERRAL = 1 AND WORK_STATUS != 'Completed' THEN 'Index Pending'    
    WHEN WORK_STATUS = 'Completed' THEN 'Indexed'    
    ELSE WORK_STATUS          
    END AS Indexing_Status,  
                RECEIVE_DATE,                  
    convert(varchar,RECEIVE_DATE) AS Received_Date_Str,                      
                TP.TOTAL_PAGES,                      
                TP.NO_OF_SPLITS,                   
                FILE_PATH,                   
                ASSIGNED_TO,                   
                ASSIGNED_BY,                   
                ASSIGNED_DATE,                   
                COMPLETED_BY,                   
                COMPLETED_DATE,                   
                B.CREATED_BY,                   
                B.CREATED_DATE,                   
                B.MODIFIED_BY,                   
                B.MODIFIED_DATE,                           
                B.DELETED,                  
    B.OCR_STATUS_ID,                  
    OC.OCR_STATUS AS OCR_STATUS,                  
                IS_EMERGENCY_ORDER,                   
                CONVERT(BIT,                  
           CASE                  
                WHEN ISNULL(IsSigned, 0) = 1 OR B.CREATED_BY LIKE 'FOX TEAM' OR CHARINDEX('_', B.UNIQUE_ID) > 0 THEN 0                  
                ELSE 1                  
                END) AS IS_UNSIGNED,                  
                CONVERT(BIT,                  
                CASE                   
                WHEN  ISNULL(RS.FOR_STRATEGIC, 0) = 1 THEN 1                  
                WHEN  ISNULL(FC.CODE, '') = 'SA' THEN 1                   
                ELSE 0                         
                END) AS IS_STRATEGIC,                   
                ROW_NUMBER() OVER(ORDER BY B.CREATED_DATE ASC) AS ACTIVEROW,                  
                C.NO_OF_COMPLETED,                  
                P.NO_OF_PENDING                  
         INTO #TEMPRECORDS                  
         FROM #WORK_ORDERS B WITH (NOLOCK)                  
     LEFT JOIN FOX_TBL_REFERRAL_SENDER RS WITH (NOLOCK) ON RS.SENDER = B.SORCE_NAME                  
             AND ISNULL(RS.FOR_STRATEGIC, 0) = 1          
             AND ISNULL(RS.DELETED, 0) = 0                  
              LEFT JOIN #COMPLETED C ON C.UNIQUE_ID = B.WORK_ID                  
              LEFT JOIN #PENDING P ON P.UNIQUE_ID = B.WORK_ID                  
              LEFT JOIN #TOTAL_PAGES TP ON TP.UNIQUE_ID = B.WORK_ID                       
     LEFT JOIN FOX_TBL_OCR_STATUS OC WITH (NOLOCK) ON OC.OCR_STATUS_ID = B.OCR_STATUS_ID                  
             AND ISNULL(OC.DELETED, 0) = 0                           
     LEFT JOIN dbo.FOX_TBL_PATIENT AS ftp WITH (NOLOCK) ON B.Patient_Account = ftp.Patient_Account                    
     LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC WITH (NOLOCK) ON ftp.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                  
             AND B.Practice_Code = FC.PRACTICE_CODE                  
             AND ISNULL(FC.DELETED, 0) = 0                        
         WHERE(Replace(SORCE_NAME,'(','') LIKE '%'+@SEARCH_GO+'%'                  
               OR Replace(SORCE_TYPE,' ','') LIKE '%'+@SEARCH_GO+'%'                  
               OR Replace(Replace(Replace(Replace(WORK_STATUS,' ',''),'Guest_Created','Pending'),'Created','Pending'),'Index_Pending','Pending') LIKE '%'+@SEARCH_GO+'%'                    
               OR TP.TOTAL_PAGES LIKE '%'+@SEARCH_GO+'%'                  
      OR CONVERT(VARCHAR, RECEIVE_DATE, 101) LIKE '%'+@SEARCH_GO+'%'                  
               OR CONVERT(VARCHAR, RECEIVE_DATE, 100) LIKE '%'+@SEARCH_GO+'%'                  
               OR WORK_ID LIKE '%'+@SEARCH_GO+'%')                  
      AND ISNULL(B.DELETED, 0) = 0                    
      AND SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME)                  
               AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)                     
               AND ((@DATE_FROM IS NULL OR CAST(RECEIVE_DATE AS DATETIME) >= CAST(CONVERT(VARCHAR, @DATE_FROM+' 00:00:00') AS DATETIME))                  
               AND (@DATE_TO IS NULL OR CAST(RECEIVE_DATE AS DATETIME) <= CAST(CONVERT(VARCHAR, @DATE_TO+' 23:59:59') AS DATETIME)))                  
               AND (WORK_STATUS = @STATUS_TEXT OR (@STATUS_TEXT IS NULL AND LOWER(WORK_STATUS) <> 'completed') or @STATUS_TEXT = '')                  
      AND CHARINDEX('_', B.UNIQUE_ID) <= 0        
      AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE)                             
               AND B.PRACTICE_CODE = @PRACTICE_CODE                  
      AND (@WORK_ID IS NULL OR B.WORK_ID LIKE '%'+@WORK_ID+'%');           
                           
      DECLARE @TOATL_PAGESUDM FLOAT;                  
      SELECT @TOATL_PAGESUDM = COUNT(*) FROM #TEMPRECORDS;                    
      DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                      
         IF(@RECORD_PER_PAGE = 0)                  
             BEGIN                  
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                  
             END;                 
   SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                  
      SET @CURRENT_PAGE = @CURRENT_PAGE - 1;               
      DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                  
         SELECT *,                   
           @TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES,                  
                @TOTAL_RECORDS TOTAL_RECORDS                  
         FROM #TEMPRECORDS                  
         ORDER BY                  
         CASE                  
             WHEN @SORT_BY = 'SOURCETYPE'                  
                AND @SORT_ORDER = 'ASC'                  
   THEN SORCE_TYPE                  
         END ASC,                  
         CASE                  
             WHEN @SORT_BY = 'SOURCETYPE'                  
                  AND @SORT_ORDER = 'DESC'                  
             THEN SORCE_TYPE          
         END DESC,                  
         CASE                  
             WHEN @SORT_BY = 'SOURCENAME'                  
                  AND @SORT_ORDER = 'ASC'                  
             THEN SORCE_NAME                  
         END ASC,                  
         CASE                  
             WHEN @SORT_BY = 'SOURCENAME'                  
                  AND @SORT_ORDER = 'DESC'                  
           THEN SORCE_NAME                  
         END DESC,                  
         CASE                  
             WHEN @SORT_BY = 'DATETIMERECEIVED'                  
                  AND @SORT_ORDER = 'ASC'                  
             THEN RECEIVE_DATE                  
   END ASC,                  
         CASE                  
             WHEN @SORT_BY = 'DATETIMERECEIVED'                  
                  AND @SORT_ORDER = 'DESC'                  
             THEN RECEIVE_DATE                  
         END DESC,                  
         CASE                  
             WHEN @SORT_BY = 'STATUS'                  
                  AND @SORT_ORDER = 'ASC'                       
             THEN WORK_STATUS                  
         END ASC,                  
         CASE                  
             WHEN @SORT_BY = 'STATUS'                  
                  AND @SORT_ORDER = 'DESC'                  
             THEN WORK_STATUS                  
         END DESC,                  
         CASE       
             WHEN @SORT_BY = 'NOOFPAGES'                  
                  AND @SORT_ORDER = 'ASC'                  
             THEN TOTAL_PAGES                  
         END ASC,                  
         CASE                  
             WHEN @SORT_BY = 'NOOFPAGES'                  
                  AND @SORT_ORDER = 'DESC'                  
             THEN TOTAL_PAGES                  
         END DESC,                  
   CASE                  
             WHEN @SORT_BY = 'NOOFSPLITS'                  
                  AND @SORT_ORDER = 'ASC'                  
             THEN NO_OF_SPLITS                  
         END ASC,                  
         CASE                  
             WHEN @SORT_BY = 'NOOFSPLITS'                  
                  AND @SORT_ORDER = 'DESC'                  
             THEN NO_OF_SPLITS               
         END DESC,                  
         CASE                  
             WHEN @SORT_BY = ''                  
                  AND @SORT_ORDER = ''                  
             THEN CREATED_DATE                  
         END DESC                  
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                  
     END; 