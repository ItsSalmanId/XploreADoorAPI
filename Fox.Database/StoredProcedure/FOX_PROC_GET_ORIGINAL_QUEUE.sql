IF (OBJECT_ID('FOX_PROC_GET_ORIGINAL_QUEUE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORIGINAL_QUEUE  
GO 
-- AUTHOR:  <AUTHOR,MEHMOOD UL HASSAN>                                  
-- CREATE DATE: <CREATE DATE,12/10/2017>                                  
-- DESCRIPTION: <GET ORIGINAL QUEUE>                                         
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
WITH RECOMPILE                      
AS                      
     BEGIN                      
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
         SET NOCOUNT ON;                      

                      
   --      declare  @PRACTICE_CODE BIGINT = '1012714'                                  
   --       declare @CURRENT_PAGE INT = '1'                                        
   --      declare @DATE_FROM DATETIME= null                                        
   --      declare @DATE_TO DATETIME =null                                        
   --      declare @INCLUDE_ARCHIVE BIT = 0                                        
   --      declare @RECORD_PER_PAGE INT = 500                                        
   --       declare @SEARCH_GO VARCHAR(30) ='' 
		 -- declare @STATUS_TEXT     VARCHAR(50) = 'completed'                                       
   --      declare @SORCE_STRING VARCHAR(100)=''                                        
   --      declare @SORCE_TYPE VARCHAR(50)='email'
		 --declare @WORK_ID         VARCHAR(50) = ''                                        
   --      declare @SORT_BY VARCHAR(50)=''                                        
   --      declare @SORT_ORDER VARCHAR(5)  =''
		 
		 IF(@STATUS_TEXT  = 'completed' or @STATUS_TEXT  = '')      
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
                      
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                      
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                      
         DECLARE @TOATL_PAGESUDM FLOAT;                      
                      
         -------------------------------------                          
         IF OBJECT_ID('TEMPDB..#COMPLETEDINITIAL') IS NOT NULL                      
             DROP TABLE #COMPLETEDINITIAL;                      
             SELECT WORK_ID,                      
                CASE                      
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                      
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                      
                    ELSE UNIQUE_ID                      
                END UNIQUE_ID,                       
                UNIQUE_ID UNIQUE_IDD                        
         INTO #COMPLETEDINITIAL                      
         FROM FOX_TBL_WORK_QUEUE                      
         WHERE ISNULL(DELETED, 0) = 0  
               AND UPPER(WORK_STATUS) = UPPER('COMPLETED');                      
         IF OBJECT_ID('TEMPDB..#COMPLETED') IS NOT NULL                      
             DROP TABLE #COMPLETED;                      
         SELECT UNIQUE_ID,                       
                COUNT(UNIQUE_ID) 'NO_OF_COMPLETED'                      
         INTO #COMPLETED                      
         FROM #COMPLETEDINITIAL                      
         GROUP BY UNIQUE_ID;                      
                      
         ------------------------------------------------                          
         IF OBJECT_ID('TEMPDB..#INDEXEDINITIAL') IS NOT NULL                      
             DROP TABLE #INDEXEDINITIAL;                      
         SELECT WORK_ID,                      
             CASE                      
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                      
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                      
                    ELSE UNIQUE_ID                      
 END UNIQUE_ID,                       
                UNIQUE_ID UNIQUE_IDD                      
         INTO #INDEXEDINITIAL                      
         FROM FOX_TBL_WORK_QUEUE                      
         WHERE ISNULL(DELETED, 0) = 0                      
               AND UPPER(WORK_STATUS) LIKE '%'+UPPER('INDEXED')+'%';                      
         IF OBJECT_ID('TEMPDB..#INDEXED') IS NOT NULL                      
             DROP TABLE #INDEXED;                      
         SELECT UNIQUE_ID,                       
                COUNT(UNIQUE_ID) 'NO_OF_INDEXED'            
         INTO #INDEXED                      
         FROM #INDEXEDINITIAL                      
         GROUP BY UNIQUE_ID;                      
                      
         ----------------------------------------------------------                          
         --(UPPER(A.WORK_STATUS) NOT  LIKE '%'+UPPER('INDEXED')+'%' OR UPPER(A.WORK_ID)=UPPER('COMPLETED'))                            
         IF OBJECT_ID('TEMPDB..#PENDINGINITIAL') IS NOT NULL                      
             DROP TABLE #PENDINGINITIAL;                      
         SELECT WORK_ID,                      
                CASE                      
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                      
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                      
                    ELSE UNIQUE_ID                      
                END UNIQUE_ID,                       
                UNIQUE_ID UNIQUE_IDD                      
         INTO #PENDINGINITIAL                      
         FROM FOX_TBL_WORK_QUEUE                      
         WHERE ISNULL(DELETED, 0) = 0                      
               AND (UPPER(WORK_STATUS) NOT LIKE '%'+UPPER('INDEXED')+'%'                      
                    OR UPPER(WORK_ID) = UPPER('COMPLETED'));                      
         IF OBJECT_ID('TEMPDB..#PENDING') IS NOT NULL                      
             DROP TABLE #PENDING;                      
         SELECT UNIQUE_ID,                       
                COUNT(UNIQUE_ID) 'NO_OF_PENDING'               
         INTO #PENDING                      
         FROM #PENDINGINITIAL                      
         GROUP BY UNIQUE_ID;              
                      
         ------------------------------------------                          
         IF OBJECT_ID('TEMPDB..#TOTAL_PAGES_INITIAL') IS NOT NULL                      
             DROP TABLE #TOTAL_PAGES_INITIAL;                      
         SELECT WORK_ID,                       
                TOTAL_PAGES,                      
                CASE                      
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                      
                    THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                      
                    ELSE UNIQUE_ID                      
                END UNIQUE_ID,                       
                UNIQUE_ID UNIQUE_IDD                      
         INTO #TOTAL_PAGES_INITIAL                      
         FROM FOX_TBL_WORK_QUEUE                      
         WHERE ISNULL(DELETED, 0) = 0;                      
         IF OBJECT_ID('TEMPDB..#TOTAL_PAGES') IS NOT NULL                      
 DROP TABLE #TOTAL_PAGES;                      
         SELECT UNIQUE_ID,                       
                SUM(TOTAL_PAGES) TOTAL_PAGES                      
         INTO #TOTAL_PAGES                      
         FROM #TOTAL_PAGES_INITIAL                      
         GROUP BY UNIQUE_ID;                      
                      
         --  (SELECT SUM(TOTAL_PAGES) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0) TOTAL_PAGES,                                     
         ---------------                           
         IF OBJECT_ID('TEMPDB..#SPLITSINITIAL') IS NOT NULL                      
             DROP TABLE #SPLITSINITIAL;                      
         SELECT WORK_ID,                      
                CASE                      
                    WHEN CHARINDEX('_', UNIQUE_ID) > 0                      
  THEN SUBSTRING(UNIQUE_ID, 0, CHARINDEX('_', UNIQUE_ID))                      
                    ELSE UNIQUE_ID                      
                END UNIQUE_ID,                       
                UNIQUE_ID UNIQUE_IDD           
         INTO #SPLITSINITIAL              
         FROM FOX_TBL_WORK_QUEUE                      
         WHERE ISNULL(DELETED, 0) = 0;                      
         IF OBJECT_ID('TEMPDB..#SPLITS') IS NOT NULL                      
             DROP TABLE #SPLITS;                      
         SELECT UNIQUE_ID,                       
                COUNT(UNIQUE_ID) NO_OF_SPLITS                      
         INTO #SPLITS                      
         FROM #SPLITSINITIAL                      
         GROUP BY UNIQUE_ID;                      
                      
         --(SELECT COUNT(*) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0)                                  
         --NO_OF_SPLITS,                            
         ------------------------------------                          
         IF OBJECT_ID('TEMPDB..#TEMPRECORDS') IS NOT NULL          
             DROP TABLE #TEMPRECORDS; 
SELECT WORK_ID,                       
                B.UNIQUE_ID,                       
                B.PRACTICE_CODE,                       
                SORCE_TYPE,                       
                SORCE_NAME,                       
                WORK_STATUS,                        
                --(SELECT COUNT(*) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0                            
                --AND UPPER(A.WORK_STATUS)=UPPER('COMPLETED')                            
                --)                                  
                --'NO_OF_COMPLETED'                          
                C.NO_OF_COMPLETED,                   
                --(SELECT COUNT(*) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0                            
                --AND UPPER(A.WORK_STATUS) LIKE '%'+UPPER('INDEXED')+'%'                            
                --)                                  
                --'NO_OF_INDEXED'                          
                I.NO_OF_INDEXED,                        
                --(SELECT COUNT(*) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0                            
                --AND (UPPER(A.WORK_STATUS) NOT  LIKE '%'+UPPER('INDEXED')+'%' OR UPPER(A.WORK_ID)=UPPER('COMPLETED'))                     
                --)                         
                --'NO_OF_PENDING'                            
                P.NO_OF_PENDING,                       
                RECEIVE_DATE,                        
                --(SELECT SUM(TOTAL_PAGES) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0)                           
                --TOTAL_PAGES                          
                TP.TOTAL_PAGES,                        
                --(SELECT COUNT(*) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0)                                  
                --NO_OF_SPLITS                          
                S.NO_OF_SPLITS,                       
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
    --CONVERT(BIT,                      
    --CASE                       
    -- WHEN  ISNULL(B.IS_OCR, 0) = 1                      
    -- THEN 1                      
    -- ELSE 0                      
    -- END) AS IS_OCR,                    
 B.OCR_STATUS_ID,                  
  OC.OCR_STATUS AS OCR_STATUS,                  
                IS_EMERGENCY_ORDER,                       
                CONVERT(BIT,                      
                        CASE                      
                            WHEN ISNULL(IsSigned, 0) = 1                      
                                 OR B.CREATED_BY LIKE 'FOX TEAM'                      
                                 OR CHARINDEX('_', B.UNIQUE_ID) > 0                      
                            THEN 0                      
                            ELSE 1                      
                        END) AS IS_UNSIGNED,                      
    CONVERT(BIT,                
    CASE                       
    WHEN  ISNULL(RS.FOR_STRATEGIC, 0) = 1                      
    THEN 1                      
 WHEN  ISNULL(FC.CODE, '') = 'SA'                      
    THEN 1     
    ELSE 0     
    END) AS IS_STRATEGIC,                       
                ROW_NUMBER() OVER(ORDER BY B.CREATED_DATE ASC) AS ACTIVEROW                      
         INTO #TEMPRECORDS                      
         FROM FOX_TBL_WORK_QUEUE B                      
        LEFT JOIN FOX_TBL_REFERRAL_SENDER RS ON RS.SENDER = B.SORCE_NAME                      
                                         and ISNULL(RS.FOR_STRATEGIC, 0) = 1                      
              AND ISNULL(RS.DELETED, 0) = 0                      
         LEFT JOIN #COMPLETED C ON C.UNIQUE_ID = B.WORK_ID                      
              LEFT JOIN #PENDING P ON P.UNIQUE_ID = B.WORK_ID                      
              LEFT JOIN #INDEXED I ON I.UNIQUE_ID = B.WORK_ID                      
              LEFT JOIN #TOTAL_PAGES TP ON TP.UNIQUE_ID = B.WORK_ID                      
              LEFT JOIN #SPLITS S ON S.UNIQUE_ID = B.WORK_ID        
     LEFT JOIN FOX_TBL_OCR_STATUS OC ON OC.OCR_STATUS_ID = B.OCR_STATUS_ID                  
      AND ISNULL(OC.DELETED, 0) = 0       
   LEFT JOIN dbo.FOX_TBL_PATIENT AS ftp ON B.Patient_Account = ftp.Patient_Account                        
   LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC ON ftp.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                      
              AND B.Practice_Code = FC.PRACTICE_CODE                      
              AND ISNULL(FC.DELETED, 0) = 0    
         WHERE CHARINDEX('_', B.UNIQUE_ID) <= 0                        
               AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE)                        
               --AND ((@INCLUDE_ARCHIVE = 0 AND CONVERT(DATE, RECEIVE_DATE) >= CONVERT(DATE, @ARCHIVEDATE))                      
               --     OR (@INCLUDE_ARCHIVE = 1 ))                      
               AND B.PRACTICE_CODE = @PRACTICE_CODE                      
            AND ISNULL(B.DELETED, 0) = 0
			AND (@WORK_ID IS NULL OR B.WORK_ID LIKE '%'+@WORK_ID+'%')          
       AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE) 
	   AND (      
 WORK_STATUS = @STATUS_TEXT OR (@STATUS_TEXT IS NULL AND LOWER(WORK_STATUS) <> 'completed')  or @STATUS_TEXT = ''      
   )      
   ;                      
         SELECT @TOATL_PAGESUDM = COUNT(*)                      
         FROM #TEMPRECORDS                      
         WHERE(Replace(SORCE_NAME,'(','') LIKE '%'+@SEARCH_GO+'%'                      
               OR Replace(SORCE_TYPE,' ','') LIKE '%'+@SEARCH_GO+'%'                      
               OR Replace(Replace(Replace(Replace(WORK_STATUS,' ',''),'Guest_Created','Pending'),'Created','Pending'),'IndexPending','Pending') LIKE '%'+@SEARCH_GO+'%'
               OR TOTAL_PAGES LIKE '%'+@SEARCH_GO+'%'                   
      OR CONVERT(VARCHAR, RECEIVE_DATE, 101) LIKE '%'+@SEARCH_GO+'%'                    
               OR CONVERT(VARCHAR, RECEIVE_DATE, 100) LIKE '%'+@SEARCH_GO+'%'                      
               OR WORK_ID LIKE '%'+@SEARCH_GO+'%')                      
              AND ISNULL(DELETED, 0) = 0                      
              AND SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME)                      
              AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)                      
              AND ((@DATE_FROM IS NULL                      
                    OR CAST(RECEIVE_DATE AS DATETIME) >= CAST(CONVERT(VARCHAR, @DATE_FROM+' 00:00:00') AS DATETIME))                      
                   AND (@DATE_TO IS NULL                      
                        OR CAST(RECEIVE_DATE AS DATETIME) <= CAST(CONVERT(VARCHAR, @DATE_TO+' 23:59:59') AS DATETIME)))                        
              AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE)                        
              --AND ((@INCLUDE_ARCHIVE = 0 AND CONVERT(DATE, RECEIVE_DATE) >= CONVERT(DATE, @ARCHIVEDATE))                      
              --     OR (@INCLUDE_ARCHIVE = 1 ))                      
              AND CHARINDEX('_', UNIQUE_ID) <= 0                      
      AND PRACTICE_CODE = @PRACTICE_CODE
	  AND (@WORK_ID IS NULL OR WORK_ID  LIKE '%'+@WORK_ID+'%')
	  --AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)
	  AND (      
 WORK_STATUS = @STATUS_TEXT OR (@STATUS_TEXT IS NULL AND LOWER(WORK_STATUS) <> 'completed')  or @STATUS_TEXT = ''      
   )      
   ; 
                      
     --WHERE (SORCE_NAME LIKE  '%' + @SEARCH_GO+'%' OR SORCE_TYPE LIKE '%' + @SEARCH_GO+'%' OR WORK_STATUS LIKE '%' + @SEARCH_GO+'%' OR TOTAL_PAGES LIKE '%' + @SEARCH_GO+'%'                                  
         --OR CONVERT(VARCHAR,RECEIVE_DATE,101) LIKE '%'+@SEARCH_GO+'%' OR CONVERT(VARCHAR,RECEIVE_DATE,100) LIKE '%'+@SEARCH_GO+'%') AND ISNULL(DELETED,0) = 0                                         
         --AND SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME) AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)            
         --AND CONVERT(DATE,RECEIVE_DATE) > CONVERT(DATE,@ARCHIVEDATE) AND CHARINDEX('_' ,UNIQUE_ID ) <= 0  AND PRACTICE_CODE = @PRACTICE_CODE                          
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
           @TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES,                       
                @TOTAL_RECORDS TOTAL_RECORDS                      
         FROM                      
         (                      
             SELECT WORK_ID,                       
                    UNIQUE_ID,                       
                    PRACTICE_CODE,                       
     SORCE_TYPE,                       
                    SORCE_NAME,                      
                    CASE                      
                        WHEN NO_OF_SPLITS = NO_OF_COMPLETED                      
                        THEN 'Completed'                      
                        WHEN NO_OF_INDEXED > 0                      
                             OR NO_OF_COMPLETED > 0                      
          THEN 'Partially Done'                      
                        ELSE 'Pending'                      
                    END WORK_STATUS,          
     RECEIVE_DATE,                      
     convert(varchar,RECEIVE_DATE) AS Received_Date_Str,            
                    --(SELECT SUM(TOTAL_PAGES) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0)                                 
                    TOTAL_PAGES,                        
                    --(SELECT COUNT(*) FROM  FOX_TBL_WORK_QUEUE A   WHERE CONVERT(VARCHAR,A.UNIQUE_ID) LIKE   CONVERT(VARCHAR,B.WORK_ID) +'%'  AND ISNULL(A.DELETED,0) = 0)                                  
                    NO_OF_SPLITS,                       
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
        B.OCR_STATUS AS OCR_STATUS,                  
                    IS_EMERGENCY_ORDER,                       
                    IS_UNSIGNED,                      
                    IS_STRATEGIC,                      
                    ROW_NUMBER() OVER(ORDER BY B.CREATED_DATE ASC) AS ACTIVEROW                      
             FROM #TEMPRECORDS B                      
             WHERE(Replace(SORCE_NAME,'(','') LIKE '%'+@SEARCH_GO+'%'                      
                   OR Replace(SORCE_TYPE,' ','') LIKE '%'+@SEARCH_GO+'%'                      
                   OR Replace(Replace(Replace(Replace(WORK_STATUS,' ',''),'Guest_Created','Pending'),'Created','Pending'),'IndexPending','Pending')  LIKE '%'+@SEARCH_GO+'%'                      
                   OR TOTAL_PAGES LIKE '%'+@SEARCH_GO+'%'                      
                   OR CONVERT(VARCHAR, RECEIVE_DATE, 101) LIKE '%'+@SEARCH_GO+'%'                    
                   OR CONVERT(VARCHAR, RECEIVE_DATE, 100) LIKE '%'+@SEARCH_GO+'%'                      
  OR WORK_ID LIKE '%'+@SEARCH_GO+'%')                      
                  AND ISNULL(DELETED, 0) = 0                      
                  AND SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME)                      
                  AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)                      
                  AND ((@DATE_FROM IS NULL                      
                        OR CAST(RECEIVE_DATE AS DATETIME) >= CAST(CONVERT(VARCHAR, @DATE_FROM+' 00:00:00') AS DATETIME))                      
                       AND (@DATE_TO IS NULL                      
                            OR CAST(RECEIVE_DATE AS DATETIME) <= CAST(CONVERT(VARCHAR, @DATE_TO+' 23:59:59') AS DATETIME)))                        
        AND CONVERT(DATE, RECEIVE_DATE) > CONVERT(DATE, @ARCHIVEDATE)                        
                  --AND ((@INCLUDE_ARCHIVE = 0                      
                  --      AND CONVERT(DATE, RECEIVE_DATE) >= CONVERT(DATE, @ARCHIVEDATE))                      
                  --     OR (@INCLUDE_ARCHIVE = 1))                      
                  AND CHARINDEX('_', UNIQUE_ID) <= 0                      
                  AND PRACTICE_CODE = @PRACTICE_CODE
				  AND (@WORK_ID IS NULL OR B.WORK_ID LIKE '%'+@WORK_ID+'%')          
     --AND SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)   
	 AND (      
 WORK_STATUS = @STATUS_TEXT OR (@STATUS_TEXT IS NULL AND LOWER(WORK_STATUS) <> 'completed')  or @STATUS_TEXT = ''      
   )                      
         ) AS WORK_QUEUE                      
         ORDER BY                        
     --CASE WHEN @SORT_BY = 'INDEXEDBY' AND @SORT_ORDER = 'ASC' THEN INDEXED_BY  END ASC,                                        
         --CASE WHEN @SORT_BY = 'INDEXEDBY' AND @SORT_ORDER = 'DESC' THEN INDEXED_BY  END DESC,                                        
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
             THEN RECEIVE_DATE               END ASC,                      
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
