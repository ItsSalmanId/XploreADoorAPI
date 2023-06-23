-- AUTHOR:  <Aftab khan>                                                                                            
-- CREATE DATE: <CREATE DATE, 05/11/2023>                                                                                            
-- DESCRIPTION: <GET_PENDING_HIGH_BALANCE_DETAILED_REPORT>  
-- =============================================           
--  [FOX_PROC_GET_PENDING_HIGH_BALANCE_DETAILED_REPORT_NEW_LOGIC]-- 1011163,'','al001',1, 2000, '' , '' 

CREATE PROCEDURE [dbo].[FOX_PROC_GET_PENDING_HIGH_BALANCE_DETAILED_REPORT_NEW_LOGIC]    
@practice_code   BIGINT,    
@search_string   VARCHAR(100),    
@current_page    INT,    
@record_per_page INT,    
@sort_by         VARCHAR(50),    
@sort_order      VARCHAR(5)                        
  AS                  
--DECLARE @practice_code BIGINT = 1012714,    
--@search_string VARCHAR(100) = '',    
--@current_page INT = 1,    
--@record_per_page INT = 10,    
--@sort_by VARCHAR(50) = 'WORK_ID',    
--@sort_order VARCHAR(5) = 'DESC'                  
BEGIN    
IF OBJECT_ID('TEMPDB.DBO.#CLAIMS_PATIENT_ACCOUNT', 'U') IS NOT NULL DROP TABLE #CLAIMS_PATIENT_ACCOUNT    
SELECT C.Patient_Account,    
SUM(amt_due) AS Patient_Balance    
INTO #CLAIMS_PATIENT_ACCOUNT    
FROM claims C  WITH (NOLOCK) 
JOIN Patient P WITH (NOLOCK)  ON P.Patient_Account = C.Patient_Account AND P.Practice_Code=@practice_code    
left join fox_tbl_patient ftp WITH (NOLOCK) on ftp.patient_account =  P.patient_account AND P.Practice_Code=@practice_code    
left join FOX_TBL_FINANCIAL_CLASS fc WITH (NOLOCK) on fc.FINANCIAL_CLASS_ID = ftp.FINANCIAL_CLASS_ID AND P.Practice_Code=@practice_code    
WHERE isnull(C.deleted, 0) = 0                        
AND P.PRACTICE_CODE = @practice_code    
AND ISNULL(P.DELETED,0) = 0    
AND ISNULL(C.PTL_STATUS,0) = 0    
AND ISNULL(C.PAT_STATUS,'') IN ('N','R','B','D')    
AND C.DX_Code1  <> 'Retainer'    
and fc.NAME <> 'SA- Special Account'     
--and p.Patient_Account = 1012714600132172    
GROUP BY C.Patient_Account      
having SUM(amt_due) >= 500    
    
--select * from #CLAIMS_PATIENT_ACCOUNT    
    
IF OBJECT_ID('TEMPDB.DBO.#TEMPRECORD', 'U') IS NOT NULL DROP TABLE #TEMPRECORD    
SELECT DISTINCT    
P.Patient_Account,    
min(cls.PROCESS_DATE) AS PROCESS_DATE,    
c.claim_no,     
C.Amt_Due    
INTO #TEMPRECORD    
FROM Patient P  WITH (NOLOCK)   
left join fox_tbl_patient ftp WITH (NOLOCK) on ftp.patient_account =  P.patient_account AND P.Practice_Code=@practice_code    
left join FOX_TBL_FINANCIAL_CLASS fc WITH (NOLOCK) on fc.FINANCIAL_CLASS_ID = ftp.FINANCIAL_CLASS_ID AND P.Practice_Code=@practice_code      
LEFT JOIN #CLAIMS_PATIENT_ACCOUNT AS clms WITH (NOLOCK)  ON P.Patient_Account = clms.Patient_Account    
join claims c on c.patient_account = clms.patient_account    
      AND ISNULL(C.PTL_STATUS,0) = 0    
      AND ISNULL(C.PAT_STATUS,'') IN ('N','R','B','D')    
   AND C.DX_Code1  <> 'RETAINER'    
   AND ISNULL(c.DELETED,0) = 0    
join claims_submitted cls WITH (NOLOCK) on cls.claim_no = c.claim_no     
      and isnull(CLS.deleted,0)=0     
      --and YEAR(PROCESS_DATE)>=2022             
   AND cls.SUBMISSION_TYPE = 'PAT'     
   and ISNULL(cls.PAT_STATUS,'') IN ('N','R')    
WHERE Patient_Balance >= 500     
and fc.NAME <> 'SA- Special Account'    
--and p.Patient_Account = 1012714600132172    
group by P.Patient_Account,c.claim_no,C.Amt_Due    
     
    
    
IF OBJECT_ID('TEMPDB.DBO.#SubmissionClaims', 'U') IS NOT NULL DROP TABLE #SubmissionClaims    
select *    
into #SubmissionClaims    
from #TEMPRECORD    
where PROCESS_DATE < GETDATE()- 45    
    
    
IF OBJECT_ID('TEMPDB.DBO.#TEMPRECORD1', 'U') IS NOT NULL DROP TABLE #TEMPRECORD1    
SELECT distinct                                 
trd.Patient_Account,trd.claim_no,SC.PROCESS_DATE,    
trd.Amt_Due,    
DATEDIFF(DAY, CONVERT(VARCHAR, SC.PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF    
into #TEMPRECORD1      
FROM #TEMPRECORD TRD  WITH (NOLOCK)             
join #SubmissionClaims SC WITH (NOLOCK) on sc.Claim_No = trd.Claim_No    
     
    
IF OBJECT_ID('TEMPDB.DBO.#fiveHindredAcct', 'U') IS NOT NULL DROP TABLE #fiveHindredAcct    
select fd.Patient_Account,SUM(fd.Amt_Due) Patient_Balance    
into #fiveHindredAcct    
from #TEMPRECORD1 fd  WITH (NOLOCK)   
join #SubmissionClaims  SC WITH (NOLOCK) on SC.Claim_No = fd.Claim_No    
group by fd.Patient_Account    
having  SUM(fd.Amt_Due) >= 500     
    
    
    
IF OBJECT_ID('TEMPDB.DBO.#PatientNBC', 'U') IS NOT NULL DROP TABLE #PatientNBC    
select SUM(Amount_over_paid) Amount_over_paid,C.Patient_Account    
into #PatientNBC    
from MIS_TBL_Claim_negative_balance nbc WITH (NOLOCK)    
join Claims C WITH (NOLOCK) on C.Claim_No = nbc.Claim_no and ISNULL(C.Deleted,0) = 0 and c.Amt_Due < 0    
where ISNULL(nbc.deleted,0) = 0     
and Payment_Source = 'P'    
and practice_code =  1012714    
group by C.Patient_Account     
    
IF OBJECT_ID('TEMPDB.DBO.#HighBalancePatients', 'U') IS NOT NULL DROP TABLE #HighBalancePatients    
SELECT distinct    
trd.Patient_Account,    
fa.Patient_Balance,    
min(PROCESS_DATE) as PROCESS_DATE    
,DATEDIFF(DAY, CONVERT(VARCHAR, min(PROCESS_DATE), 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF    
into #HighBalancePatients    
FROM #TEMPRECORD1 TRD  WITH (NOLOCK)   
join #fiveHindredAcct fa WITH (NOLOCK)  on fa.Patient_Account = trd.Patient_Account    
left join #PatientNBC nbc WITH (NOLOCK) on nbc.Patient_Account = trd.Patient_Account    
where isnull(nbc.Amount_over_paid,0) <> 0    
group by trd.Patient_Account,fa.Patient_Balance    
--where trd.Claim_No = 6003723040    
--order by 5 desc    
    
    
 IF OBJECT_ID('TEMPDB.DBO.#HBRDATA', 'U') IS NOT NULL DROP TABLE #HBRDATA;    
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
     PROCESS_DATE,                     
     CALCULATED_DIFF,                 
                    ROW_NUMBER() OVER(ORDER BY WQ.WORK_ID DESC) AS ACTIVEROW    
     INTO #HBRDATA                      
    FROM FOX_TBL_WORK_QUEUE WQ  WITH (NOLOCK)                       
    LEFT JOIN Patient P WITH (NOLOCK) ON(P.Patient_Account = WQ.PATIENT_ACCOUNT)    
              AND WQ.PRACTICE_CODE = @practice_code       
              --AND YEAR(WQ.CREATED_DATE)>=2022       
              AND ISNULL(WQ.DELETED, 0) = 0      
    INNER JOIN #HighBalancePatients AS hbp WITH (NOLOCK)  ON hbp.Patient_Account = p.Patient_Account    
    LEFT JOIN FOX_TBL_DOCUMENT_TYPE DOC WITH (NOLOCK) ON(DOC.DOCUMENT_TYPE_ID = WQ.DOCUMENT_TYPE)                       
    LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS WITH (NOLOCK)  ON(ORS.SOURCE_ID = WQ.SENDER_ID)        
              AND ORS.PRACTICE_CODE = @practice_code                        
    LEFT JOIN FOX_TBL_PATIENT_POS POS WITH (NOLOCK)  ON(POS.Patient_Account = P.Patient_Account)                        
              AND ISNULL(POS.Deleted, 0) = 0                        
              AND (ISNULL(POS.Is_Default, 0) = 1)                        
    LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS LOC WITH (NOLOCK) ON(LOC.LOC_ID = POS.Loc_ID)                        
              AND ISNULL(LOC.Deleted, 0) = 0                        
              AND ISNULL(LOC.IS_ACTIVE, 0) = 1                        
              AND LOC.PRACTICE_CODE = @practice_code                        
    LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS_PRIMARY WITH (NOLOCK) ON INS_PRIMARY.Patient_Insurance_Id =                        
                (SELECT TOP 1 Patient_Insurance_Id FROM FOX_TBL_PATIENT_INSURANCE AS ftPI WITH (NOLOCK)  WHERE ftPI.Patient_Account = P.Patient_Account                        
                          AND ftPI.Pri_Sec_Oth_Type = 'P'                        
                          AND ISNULL(ftPI.INACTIVE, 0) = 0                        
                          AND ftPI.FOX_INSURANCE_STATUS = 'C'                        
                          AND ISNULL(ftPI.Deleted, 0) = 0)      
    LEFT JOIN FOX_TBL_INSURANCE INS_PRI WITH (NOLOCK)  ON(INS_PRI.FOX_TBL_INSURANCE_ID = INS_PRIMARY.FOX_TBL_INSURANCE_ID)                        
              AND INS_PRI.PRACTICE_CODE = @practice_code     
    LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS_SECONDARY WITH (NOLOCK) ON INS_SECONDARY.Patient_Insurance_Id =                   
          (SELECT TOP 1 Patient_Insurance_Id FROM FOX_TBL_PATIENT_INSURANCE AS ftPI WHERE ftPI.Patient_Account = P.Patient_Account    
             AND ftPI.Pri_Sec_Oth_Type = 'S'                        
             AND ISNULL(ftPI.INACTIVE, 0) = 0                        
             AND ftPI.FOX_INSURANCE_STATUS = 'C'                        
             AND ISNULL(ftPI.Deleted, 0) = 0                        
          )    
    LEFT JOIN FOX_TBL_INSURANCE INS_SEC WITH (NOLOCK) ON(INS_SEC.FOX_TBL_INSURANCE_ID = INS_SECONDARY.FOX_TBL_INSURANCE_ID)                        
              AND INS_SEC.PRACTICE_CODE = @practice_code                        
             WHERE WQ.WORK_STATUS = 'Completed'             
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
    
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                        
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                        
         DECLARE @TOATL_PAGESUDM FLOAT;                 
                    
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT WORK_ID) FROM #HBRDATA                     
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
    
    IF OBJECT_ID('TEMPDB.DBO.#HBRDATAREPORT', 'U') IS NOT NULL DROP TABLE #HBRDATAREPORT;    
         SELECT                      
      *,    
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,    
               @TOTAL_RECORDS AS TOTAL_RECORDS    
      INTO #HBRDATAREPORT    
      from #HBRDATA    
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
                  CASE                                              WHEN @SORT_BY = 'Patient_account'                        
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
    Patient_Balance,                
    CALCULATED_DIFF,             
    @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,    
    @TOTAL_RECORDS AS TOTAL_RECORDS                
    FROM #HBRDATAREPORT AS TRD                 
    --WHERE CALCULATED_DIFF > 45                
    GROUP BY work_id, Patient_Account, PATIENT_NAME, PHONE, DEFAULT_POS,  REGION, PRIMARY_INSURANCE,       
 SECONDARY_INSURANCE, DOCUMENT_TYPE, ORS,             
    WORK_ORDER_SOURCE, DISCIPLINE_NO, FACILITY_NAME , Patient_Balance, CALCULATED_DIFF, PROCESS_DATE                
    --HAVING SUM(AMOUNT) >= 500                
    ORDER BY WORK_ID                    
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                 
  END; 