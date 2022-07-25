IF (OBJECT_ID('FOX_PROC_GET_PENDING_HIGH_BALANCE_DETAILED_REPORT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PENDING_HIGH_BALANCE_DETAILED_REPORT  
GO        
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PENDING_HIGH_BALANCE_DETAILED_REPORT]-- 1011163,'','al001',1, 2000, '' , ''                                                              
@practice_code   BIGINT,                   
@search_string   VARCHAR(100),                   
@current_page    INT,                   
@record_per_page INT,                   
@sort_by         VARCHAR(50),                   
@sort_order      VARCHAR(5)                  
  AS            
--DECLARE @practice_code   BIGINT = 1012714,                   
--@search_string   VARCHAR(100) = '',                   
--@current_page    INT = 1,                   
--@record_per_page INT = 20,                   
--@sort_by         VARCHAR(50) = 'WORK_ID',                   
--@sort_order      VARCHAR(5) = 'DESC'            
     BEGIN              
               
   IF OBJECT_ID('TEMPDB.DBO.#TEMPRECORD', 'U') IS NOT NULL                
         DROP TABLE #TEMPRECORD;                     
              
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                  
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                  
         DECLARE @TOATL_PAGESUDM FLOAT;                  
   DECLARE @NoOfDays INT              
              
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT WQ.WORK_ID)                  
         FROM FOX_TBL_WORK_QUEUE WQ                  
                
              LEFT JOIN Patient P ON(P.Patient_Account = WQ.PATIENT_ACCOUNT)                  
                                    AND ISNULL(WQ.DELETED, 0) = 0                  
                                    AND WQ.PRACTICE_CODE = @practice_code                  
              
              LEFT JOIN                  
         (                  
             SELECT Patient_Account,                   
                    SUM(amt_due) AS Patient_Balance                  
             FROM claims                  
             WHERE isnull(claims.deleted, 0) = 0                  
                   AND pat_status IN('N', 'R', 'B', 'C', 'D')                  
             GROUP BY Patient_Account              
         ) AS clms ON P.Patient_Account = clms.Patient_Account                  
                        
       INNER JOIN               
     (              
    SELECT DATEDIFF(DAY, CONVERT(VARCHAR, PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF, *              
       FROM PATIENT_STATEMENT_HISTORY              
    WHERE SUBMISSION_TYPE = 'PAT'              
     ) AS SH ON SH.Patient_Account = clms.Patient_Account            
  and CAST(SH.PROCESS_DATE AS DATE) = CAST((SELECT TOP 1 ISNULL(PROCESS_DATE,'') FROM PATIENT_STATEMENT_HISTORY  WHERE Patient_Account = clms.Patient_Account AND ISNULL(DELETED,0)= 0 ORDER BY PROCESS_DATE DESC) AS DATE)            
              
                 
     LEFT JOIN FOX_TBL_DOCUMENT_TYPE DOC ON(DOC.DOCUMENT_TYPE_ID = WQ.DOCUMENT_TYPE)                  
                                                    --AND ISNULL(DOC.IS_ACTIVE, 0) = 1                  
              LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS ON(ORS.SOURCE_ID = WQ.SENDER_ID)                  
                                                          --AND ISNULL(DOC.IS_ACTIVE, 0) = 1                  
                                                          AND ORS.PRACTICE_CODE = @practice_code                  
              LEFT JOIN FOX_TBL_PATIENT_POS POS ON(POS.Patient_Account = P.Patient_Account)                  
                                                  AND ISNULL(POS.Deleted, 0) = 0                  
                                                  AND (ISNULL(POS.Is_Default, 0) = 1)                  
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS LOC ON(LOC.LOC_ID = POS.Loc_ID)                  
                                                       AND ISNULL(LOC.Deleted, 0) = 0                  
                          AND ISNULL(LOC.IS_ACTIVE, 0) = 1                  
                                       AND LOC.PRACTICE_CODE = @practice_code                  
              LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS_PRIMARY ON INS_PRIMARY.Patient_Insurance_Id =                  
         (                  
             SELECT TOP 1 Patient_Insurance_Id                  
             FROM FOX_TBL_PATIENT_INSURANCE AS ftPI                  
    WHERE ftPI.Patient_Account = P.Patient_Account                  
                   AND ftPI.Pri_Sec_Oth_Type = 'P'                  
                   AND ISNULL(ftPI.INACTIVE, 0) = 0                  
                   AND ftPI.FOX_INSURANCE_STATUS = 'C'                  
                   AND ISNULL(ftPI.Deleted, 0) = 0                  
         )                  
              LEFT JOIN FOX_TBL_INSURANCE INS_PRI ON(INS_PRI.FOX_TBL_INSURANCE_ID = INS_PRIMARY.FOX_TBL_INSURANCE_ID)                  
                                                  AND INS_PRI.PRACTICE_CODE = @practice_code                  
                  
    LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS_SECONDARY ON INS_SECONDARY.Patient_Insurance_Id =                  
     (                  
      SELECT TOP 1 Patient_Insurance_Id                  
      FROM FOX_TBL_PATIENT_INSURANCE AS ftPI                  
      WHERE ftPI.Patient_Account = P.Patient_Account                  
         AND ftPI.Pri_Sec_Oth_Type = 'S'                  
         AND ISNULL(ftPI.INACTIVE, 0) = 0                  
         AND ftPI.FOX_INSURANCE_STATUS = 'C'                  
         AND ISNULL(ftPI.Deleted, 0) = 0                  
     )                  
              LEFT JOIN FOX_TBL_INSURANCE INS_SEC ON(INS_SEC.FOX_TBL_INSURANCE_ID = INS_SECONDARY.FOX_TBL_INSURANCE_ID)                  
                                                    AND INS_SEC.PRACTICE_CODE = @practice_code                  
         WHERE Patient_Balance >= 500   AND CALCULATED_DIFF > 45      
               AND WQ.WORK_STATUS = 'Completed'                 
               AND (@SEARCH_STRING IS NULL                  
                    OR P.Patient_Account LIKE '%'+@SEARCH_STRING+'%'                  
                    OR WQ.WORK_ID LIKE '%'+@SEARCH_STRING+'%'                  
                    OR (P.FIRST_NAME+' '+P.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'                  
                    OR p.Home_Phone LIKE '%'+@SEARCH_STRING+'%'                  
                    OR LOC.CODE LIKE '%'+@SEARCH_STRING+'%'                  
                    OR LOC.NAME LIKE '%'+@SEARCH_STRING+'%'                  
                    OR LOC.REGION LIKE '%'+@SEARCH_STRING+'%'                  
                    OR Patient_Balance LIKE '%'+@SEARCH_STRING+'%'                  
                    OR INS_PRI.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'                  
                    OR INS_SEC.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'                  
                    OR LOC.REGION LIKE '%'+@SEARCH_STRING+'%')                
         --IF(@RECORD_PER_PAGE = 0)                  
         --    BEGIN                  
         --        SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;                  
         --    END;                  
         --    ELSE                  
         --    BEGIN                  
         --        SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                  
         --    END;                  
         --DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                  
         --SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                  
                  
         --                       
                       
         SELECT *                
                --@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                   
               -- @TOTAL_RECORDS AS TOTAL_RECORDS                  
   INTO #TEMPRECORD                
      FROM                  
         (                  
             SELECT DISTINCT                   
                    WQ.WORK_ID,                   
                    P.Patient_Account,                   
                    P.Last_Name+', '+P.First_Name AS PATIENT_NAME,                   
                    P.Home_Phone AS PHONE,                   
                    '('+LOC.CODE+') '+LOC.NAME AS DEFAULT_POS,                   
                    LOC.REGION,                   
                    INS_PRI.INSURANCE_NAME AS PRIMARY_INSURANCE,                   
         INS_SEC.INSURANCE_NAME AS SECONDARY_INSURANCE,                   
                    DOC.NAME AS DOCUMENT_TYPE,                   
                    ORS.NAME+' | '+ORS.REFERRAL_REGION AS ORS,                   
                    WQ.SORCE_NAME AS WORK_ORDER_SOURCE,                   
                    WQ.FACILITY_NAME,                   
                    WQ.DEPARTMENT_ID AS DISCIPLINE_NO,                   
                    Patient_Balance,                 
     SH.PROCESS_DATE AS PROCESS_DATE,                
     SH.AMOUNT,                
     DATEDIFF(DAY, CONVERT(VARCHAR, SH.PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF,                
     SH.CLAIM_NO,            
                    ROW_NUMBER() OVER(ORDER BY WQ.WORK_ID DESC) AS ACTIVEROW                
   FROM FOX_TBL_WORK_QUEUE WQ                  
            LEFT JOIN Patient P ON(P.Patient_Account = WQ.PATIENT_ACCOUNT)                  
                                    AND ISNULL(WQ.DELETED, 0) = 0                  
                                    AND WQ.PRACTICE_CODE = @practice_code                  
              LEFT JOIN                  
         (                  
             SELECT Patient_Account,                   
                    SUM(amt_due) AS Patient_Balance                  
             FROM claims                  
             WHERE isnull(claims.deleted, 0) = 0                  
  AND pat_status IN('N', 'R', 'B', 'C', 'D')                  
             GROUP BY Patient_Account                  
         ) AS clms ON P.Patient_Account = clms.Patient_Account                  
              
       INNER JOIN               
     (              
    SELECT  DATEDIFF(DAY, CONVERT(VARCHAR, PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF, *                 
       FROM PATIENT_STATEMENT_HISTORY              
    WHERE SUBMISSION_TYPE = 'PAT'              
     ) AS SH ON SH.Patient_Account = clms.Patient_Account            
  and CAST(SH.PROCESS_DATE AS DATE) = CAST((SELECT TOP 1 ISNULL(PROCESS_DATE,'') FROM PATIENT_STATEMENT_HISTORY  WHERE Patient_Account = clms.Patient_Account AND ISNULL(DELETED,0)= 0 ORDER BY PROCESS_DATE DESC) AS DATE)            
              
              LEFT JOIN FOX_TBL_DOCUMENT_TYPE DOC ON(DOC.DOCUMENT_TYPE_ID = WQ.DOCUMENT_TYPE)                  
                                                    --AND ISNULL(DOC.IS_ACTIVE, 0) = 1                  
              LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS ON(ORS.SOURCE_ID = WQ.SENDER_ID)                  
                        -- AND ISNULL(DOC.IS_ACTIVE, 0) = 1                  
                                                          AND ORS.PRACTICE_CODE = @practice_code                  
              LEFT JOIN FOX_TBL_PATIENT_POS POS ON(POS.Patient_Account = P.Patient_Account)                  
                                                  AND ISNULL(POS.Deleted, 0) = 0                  
                                                  AND (ISNULL(POS.Is_Default, 0) = 1)                  
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS LOC ON(LOC.LOC_ID = POS.Loc_ID)                  
                                                       AND ISNULL(LOC.Deleted, 0) = 0                  
                                                       AND ISNULL(LOC.IS_ACTIVE, 0) = 1                  
                                                       AND LOC.PRACTICE_CODE = @practice_code                  
              LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS_PRIMARY ON INS_PRIMARY.Patient_Insurance_Id =                  
         (                  
             SELECT TOP 1 Patient_Insurance_Id        
             FROM FOX_TBL_PATIENT_INSURANCE AS ftPI                  
             WHERE ftPI.Patient_Account = P.Patient_Account                  
                   AND ftPI.Pri_Sec_Oth_Type = 'P'                  
                   AND ISNULL(ftPI.INACTIVE, 0) = 0                  
                   AND ftPI.FOX_INSURANCE_STATUS = 'C'                  
                   AND ISNULL(ftPI.Deleted, 0) = 0                  
         )                  
                  
              LEFT JOIN FOX_TBL_INSURANCE INS_PRI ON(INS_PRI.FOX_TBL_INSURANCE_ID = INS_PRIMARY.FOX_TBL_INSURANCE_ID)                  
                                                    AND INS_PRI.PRACTICE_CODE = @practice_code                  
                  
    LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS_SECONDARY ON INS_SECONDARY.Patient_Insurance_Id =                  
     (                  
      SELECT TOP 1 Patient_Insurance_Id                  
      FROM FOX_TBL_PATIENT_INSURANCE AS ftPI                  
      WHERE ftPI.Patient_Account = P.Patient_Account                  
         AND ftPI.Pri_Sec_Oth_Type = 'S'                  
         AND ISNULL(ftPI.INACTIVE, 0) = 0                  
         AND ftPI.FOX_INSURANCE_STATUS = 'C'                  
         AND ISNULL(ftPI.Deleted, 0) = 0                  
     )                  
              LEFT JOIN FOX_TBL_INSURANCE INS_SEC ON(INS_SEC.FOX_TBL_INSURANCE_ID = INS_SECONDARY.FOX_TBL_INSURANCE_ID)                  
                                                    AND INS_SEC.PRACTICE_CODE = @practice_code                  
             WHERE Patient_Balance >= 500   AND WQ.WORK_STATUS = 'Completed'       
                   AND (@SEARCH_STRING IS NULL                  
                        OR P.Patient_Account LIKE '%'+@SEARCH_STRING+'%'                  
                        OR WQ.WORK_ID LIKE '%'+@SEARCH_STRING+'%'                  
                        OR (P.FIRST_NAME+' '+P.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'                  
                        OR p.Home_Phone LIKE '%'+@SEARCH_STRING+'%'                  
                        OR LOC.CODE LIKE '%'+@SEARCH_STRING+'%'                  
                        OR LOC.NAME LIKE '%'+@SEARCH_STRING+'%'                  
                        OR LOC.REGION LIKE '%'+@SEARCH_STRING+'%'                  
                        OR Patient_Balance LIKE '%'+@SEARCH_STRING+'%'                  
                        OR INS_PRI.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'                  
                        OR INS_SEC.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'                  
                        OR LOC.REGION LIKE '%'+@SEARCH_STRING+'%')                
         ) AS PATIENT_HIGH_BALANCE_REPORT                  
         ORDER BY CASE                  
                      WHEN @SORT_BY = 'Work_Id'                  
                           AND @SORT_ORDER = 'ASC'                  
                     THEN WORK_ID                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Work_Id'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN WORK_ID                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Patient_account'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN Patient_Account                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Patient_account'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN Patient_Account                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Name'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN PATIENT_NAME                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Name'                  
                           AND @SORT_ORDER = 'DESC'                  
        THEN PATIENT_NAME                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Phone'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN PHONE                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Phone'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN PHONE                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Amount'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN Patient_Balance                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Amount'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN Patient_Balance                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'POS'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN DEFAULT_POS                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'POS'                  
                   AND @SORT_ORDER = 'DESC'                  
                      THEN DEFAULT_POS                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Region'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN REGION                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Region'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN REGION                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Primary'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN PRIMARY_INSURANCE                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Primary'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN PRIMARY_INSURANCE                  
                  END DESC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Secondary'                  
                           AND @SORT_ORDER = 'ASC'                  
                      THEN SECONDARY_INSURANCE                  
                  END ASC,                  
                  CASE                  
                      WHEN @SORT_BY = 'Secondary'                  
                           AND @SORT_ORDER = 'DESC'                  
                      THEN SECONDARY_INSURANCE                  
                  END DESC                   
         --OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;            
          
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
                   
    --select * from #TEMPRECORD           
    --ORDER BY PROCESS_DATE       
               
        
    SELECT                 
    work_id as WORK_ID,           
    Patient_Account,           
    PATIENT_NAME,           
    PROCESS_DATE,           
    PHONE,           
    DEFAULT_POS,            
    REGION,           
    PRIMARY_INSURANCE,           
    SECONDARY_INSURANCE,           
    DOCUMENT_TYPE,           
    ORS,           
    WORK_ORDER_SOURCE,           
    DISCIPLINE_NO,          
    FACILITY_NAME,          
    Patient_Balance as SUM_AMOUNT,          
    SUM(AMOUNT) as Patient_Balance,          
    CALCULATED_DIFF,       
    @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                   
    @TOTAL_RECORDS AS TOTAL_RECORDS          
    FROM #TEMPRECORD AS TRD           
    WHERE CALCULATED_DIFF > 45          
    GROUP BY work_id, Patient_Account, PATIENT_NAME, PHONE, DEFAULT_POS,  REGION, PRIMARY_INSURANCE, SECONDARY_INSURANCE, DOCUMENT_TYPE, ORS,          
    WORK_ORDER_SOURCE, DISCIPLINE_NO, FACILITY_NAME , Patient_Balance, CALCULATED_DIFF, PROCESS_DATE          
    HAVING SUM(AMOUNT) >= 500          
    ORDER BY WORK_ID              
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY           
  END; 