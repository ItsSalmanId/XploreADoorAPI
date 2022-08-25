IF (OBJECT_ID('FOX_PROC_GET_PATIENT_PAT_DOCUMENTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_PAT_DOCUMENTS  
GO
----CREATED BY: ABDULSATTAR----            
----------11/21/2018----------                 
--FOX_PROC_GET_PATIENT_PAT_DOCUMENTS '101116354813457',NULL,'1011163', 1, 0,'', 'ASC'       
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_PAT_DOCUMENTS]             
(@PATIENT_ACCOUNT AS  VARCHAR(100),       
 @DOCUMENT_TYPE_ID VARCHAR(10) = null,       
 @PRACTICE_CODE    BIGINT,       
 @CURRENT_PAGE     INT,       
 @RECORD_PER_PAGE  INT,       
 @SORT_BY          VARCHAR(50),       
 @SORT_ORDER       VARCHAR(5)      
--DECLARE @PATIENT_ACCOUNT AS VARCHAR(100)= '101116354813450', @DOCUMENT_TYPE_ID VARCHAR(10)= NULL, @PRACTICE_CODE BIGINT= 1011163, @CURRENT_PAGE INT= 1, @RECORD_PER_PAGE INT= 10, @SORT_BY VARCHAR(50)= 'CREATED_DATE', @SORT_ORDER VARCHAR(5)= 'DESC';    
)      
AS      
BEGIN    
    IF(@RECORD_PER_PAGE = 0)    
        BEGIN    
            SELECT @RECORD_PER_PAGE = COUNT(*)    
            FROM FOX_TBL_WORK_QUEUE;    
        END;    
        ELSE    
        BEGIN    
            SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;    
        END;    
    
    --    
    SET @CURRENT_PAGE = @CURRENT_PAGE - 1;    
    DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;    
    DECLARE @TOATL_PAGESUDM FLOAT;    
    IF OBJECT_ID('tempdb.dbo.#PAT_DOC_TEMP', 'U') IS NOT NULL    
        DROP TABLE #PAT_DOC_TEMP;    
    
         CREATE TABLE #PAT_DOC_TEMP (
			PATIENT_ACCOUNT BIGINT,
			PAT_DOCUMENT_ID BIGINT NULL,
			PARENT_DOCUMENT_ID BIGINT null,
			CASE_ID bigint null,
			CASE_NO varchar(100) null,
			RT_CASE_NO  varchar(100) null,
			CASE_STATUS_ID int null,  
			CASE_STATUS  varchar(100) null,  
			WORK_ID   bigint null,
			DOCUMENT_TYPE bigint null,  
			SHOW_ON_PATIENT_PORTAL bit null,    
			COMMENTS varchar(1000) null,
			CREATED_BY varchar(70) null,     
			CREATED_DATE datetime null,     
			MODIFIED_BY varchar(70) null,     
			MODIFIED_DATE datetime null,     
			[START_DATE] datetime null,     
			[END_DATE] datetime null,
			START_DATE_str  varchar(70) null,         
			END_DATE_str  varchar(70) null
		)
         --    
	insert into #PAT_DOC_TEMP
    SELECT p.PATIENT_ACCOUNT,     
             --pat.PAT_DOCUMENT_ID,
				--pat.PARENT_DOCUMENT_ID,   
				NULL,
				NULL,
		   --c.CASE_ID,   
                --c.CASE_NO,   
                --c.RT_CASE_NO,   
                --c.CASE_STATUS_ID,  
                --cs.NAME AS CASE_STATUS,  
				NULL,
				NULL,NULL,
				NULL,NULL,    
           wq.WORK_ID,     
           wq.DOCUMENT_TYPE,     
          --pat.SHOW_ON_PATIENT_PORTAL,   
                --pat.COMMENTS,   
				NULL,NULL,  
           wq.CREATED_BY,     
           wq.CREATED_DATE,     
           wq.MODIFIED_BY,     
           wq.MODIFIED_DATE,     
           --pat.[START_DATE],     
           --pat.[END_DATE],     
           --CONVERT(VARCHAR, pat.[START_DATE], 101) AS START_DATE_str,     
           --CONVERT(VARCHAR, pat.[END_DATE], 101) AS END_DATE_str    
		     NULL,NULL,NULL,NULL
    --INTO #PAT_DOC_TEMP    
    FROM Patient AS p    
         INNER JOIN FOX_TBL_WORK_QUEUE AS wq ON wq.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
                                                AND ISNULL(wq.DELETED, 0) = 0    
                                                AND wq.PRACTICE_CODE = @PRACTICE_CODE        
         --LEFT JOIN FOX_TBL_PATIENT_PAT_DOCUMENT AS pat ON pat.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
         --                                                 AND pat.WORK_ID = wq.WORK_ID    
         --                           AND ISNULL(pat.DELETED, 0) = 0    
         --                                                 AND pat.PRACTICE_CODE = @PRACTICE_CODE    
         --LEFT JOIN FOX_TBL_CASE AS c ON c.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
         --                               AND c.CASE_ID = pat.CASE_ID    
         --LEFT JOIN FOX_TBL_CASE_STATUS AS cs ON cs.CASE_STATUS_ID = c.CASE_STATUS_ID    
         --                                       AND ISNULL(cs.DELETED, 0) = 0    
         --                                       AND cs.PRACTICE_CODE = @PRACTICE_CODE  
    WHERE p.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
          AND ISNULL(p.DELETED, 0) = 0    
          AND p.PRACTICE_CODE = @PRACTICE_CODE    
          AND (@DOCUMENT_TYPE_ID IS NULL    
               OR wq.DOCUMENT_TYPE = @DOCUMENT_TYPE_ID);
    --UNION ALL   
	insert into #PAT_DOC_TEMP 
    SELECT p.PATIENT_ACCOUNT,     
           pad.PAT_DOCUMENT_ID,
		   pad.PARENT_DOCUMENT_ID,  
		   c.CASE_ID,        
           c.CASE_NO,        
           c.RT_CASE_NO,        
         --NULL AS RT_CASE_NO,     
           c.CASE_STATUS_ID,     
           cs.NAME AS CASE_STATUS,     
           pad.WORK_ID,     
           pad.DOCUMENT_TYPE,     
           pad.SHOW_ON_PATIENT_PORTAL,     
           pad.COMMENTS,     
           pad.CREATED_BY,     
           pad.CREATED_DATE,     
           pad.MODIFIED_BY,     
           pad.MODIFIED_DATE,     
           pad.[START_DATE],     
           pad.[END_DATE],     
           CONVERT(VARCHAR, pad.[START_DATE], 101) AS START_DATE_str,     
           CONVERT(VARCHAR, pad.[END_DATE], 101) AS END_DATE_str    
    FROM Patient AS p    
         INNER JOIN FOX_TBL_PATIENT_PAT_DOCUMENT AS pad ON pad.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
                                                           --AND pad.WORK_ID IS NULL    
                                                           AND ISNULL(pad.DELETED, 0) = 0    
                                                           AND pad.PRACTICE_CODE = @PRACTICE_CODE    
         LEFT JOIN FOX_TBL_CASE AS c ON c.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
                                        AND c.CASE_ID = pad.CASE_ID    
         LEFT JOIN FOX_TBL_CASE_STATUS AS cs ON cs.CASE_STATUS_ID = c.CASE_STATUS_ID    
                                                AND ISNULL(cs.DELETED, 0) = 0    
                                                AND cs.PRACTICE_CODE = @PRACTICE_CODE    
    WHERE p.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
          AND ISNULL(p.DELETED, 0) = 0    
          AND p.PRACTICE_CODE = @PRACTICE_CODE    
          AND (@DOCUMENT_TYPE_ID IS NULL    
               OR pad.DOCUMENT_TYPE = @DOCUMENT_TYPE_ID);      
    ------------------------------------------------------- -------------------------------------------------------                 
    
    SELECT @TOATL_PAGESUDM = COUNT(*)    
    FROM #PAT_DOC_TEMP;    
    
    --          
    DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;    
    SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);    
    
    --          
    SELECT *,     
           @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,     
           @TOTAL_RECORDS AS TOTAL_RECORDS    
    FROM    
    (    
        SELECT *,     
               ROW_NUMBER() OVER(ORDER BY CREATED_DATE ASC) AS ACTIVEROW    
        FROM #PAT_DOC_TEMP    
    ) AS PatientDocument    
    ORDER BY CASE    
                 WHEN @SORT_BY = 'CASENO'    
                      AND @SORT_ORDER = 'ASC'    
                 THEN PatientDocument.CASE_NO    
             END ASC,    
             CASE    
                 WHEN @SORT_BY = 'CASENO'    
                      AND @SORT_ORDER = 'DESC'    
                 THEN PatientDocument.CASE_NO    
             END DESC,    
             CASE    
                 WHEN @SORT_BY = 'CASESTATUS'    
AND @SORT_ORDER = 'ASC'    
              THEN PatientDocument.CASE_STATUS    
             END ASC,    
             CASE    
                 WHEN @SORT_BY = 'CASESTATUS'    
                      AND @SORT_ORDER = 'DESC'    
                 THEN PatientDocument.CASE_STATUS    
             END DESC,    
             CASE    
                 WHEN @SORT_BY = 'ORDERID'    
                      AND @SORT_ORDER = 'ASC'    
                 THEN PatientDocument.WORK_ID    
             END ASC,    
             CASE    
                 WHEN @SORT_BY = 'ORDERID'    
                      AND @SORT_ORDER = 'DESC'    
                 THEN PatientDocument.WORK_ID    
             END DESC,    
             CASE    
                 WHEN @SORT_BY = 'COMMENTS'    
                      AND @SORT_ORDER = 'ASC'    
                 THEN PatientDocument.COMMENTS    
             END ASC,    
             CASE    
                 WHEN @SORT_BY = 'COMMENTS'    
                      AND @SORT_ORDER = 'DESC'    
                 THEN PatientDocument.COMMENTS    
             END DESC,    
             CASE    
                 WHEN @SORT_BY = ''    
      AND @SORT_ORDER = ''    
             THEN PatientDocument.CREATED_DATE    
             END DESC    
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;    
END

