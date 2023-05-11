----------------------------------------------------------------------------------------------      
--Get patients whose       
--financial class should not be Special Account and on the basis of      
--claim first submission PROCESS_DATE > 45 days      
--and submission type should be 'PAT'       
--and patient status in ('N','R')      
--and claims with      
--PTL Status should be false      
--Patient status in ('N','R','B','D')      
--DX Code should not be 'Retainer'      
--SUM(amt_due) >= 500      
--Excluding all patients who has payment credit      
----------------------------------------------------------------------------------------------      
CREATE PROCEDURE FOX_PROC_GET_PATIENT_PENDING_BALANCE_NEW_LOGIC                          
@practice_code BIGINT NULL,      
@PATIENT_ACCOUNT BIGINT NULL                        
--DECLARE @practice_code   BIGINT = 1012714            
AS      
BEGIN      
IF OBJECT_ID('TEMPDB.DBO.#CLAIMS_PATIENT_ACCOUNT', 'U') IS NOT NULL DROP TABLE #CLAIMS_PATIENT_ACCOUNT      
SELECT C.Patient_Account,      
SUM(amt_due) AS Patient_Balance      
INTO #CLAIMS_PATIENT_ACCOUNT      
FROM claims C  with (nolock)    
JOIN Patient P with (nolock) ON P.Patient_Account = C.Patient_Account AND P.Practice_Code=@practice_code      
left join fox_tbl_patient ftp with (nolock) on ftp.patient_account =  P.patient_account AND P.Practice_Code=@practice_code      
left join FOX_TBL_FINANCIAL_CLASS fc with (nolock) on fc.FINANCIAL_CLASS_ID = ftp.FINANCIAL_CLASS_ID AND P.Practice_Code=@practice_code      
WHERE isnull(C.deleted, 0) = 0                          
AND P.PRACTICE_CODE = @practice_code      
AND ISNULL(P.DELETED,0) = 0      
AND ISNULL(C.PTL_STATUS,0) = 0      
AND ISNULL(C.PAT_STATUS,'') IN ('N','R','B','D')      
AND C.DX_Code1  <> 'Retainer'      
and fc.NAME <> 'SA- Special Account'       
and p.Patient_Account = @PATIENT_ACCOUNT      
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
FROM Patient P with (nolock)    
left join fox_tbl_patient ftp with (nolock) on ftp.patient_account =  P.patient_account AND P.Practice_Code=@practice_code      
left join FOX_TBL_FINANCIAL_CLASS fc with (nolock) on fc.FINANCIAL_CLASS_ID = ftp.FINANCIAL_CLASS_ID AND P.Practice_Code=@practice_code        
LEFT JOIN #CLAIMS_PATIENT_ACCOUNT AS clms with (nolock) ON P.Patient_Account = clms.Patient_Account      
join claims c with (nolock) on c.patient_account = clms.patient_account      
      AND ISNULL(C.PTL_STATUS,0) = 0      
      AND ISNULL(C.PAT_STATUS,'') IN ('N','R','B','D')      
   AND C.DX_Code1  <> 'RETAINER'      
   AND ISNULL(c.DELETED,0) = 0      
join claims_submitted cls with (nolock) on cls.claim_no = c.claim_no       
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
from #TEMPRECORD with (nolock)     
where PROCESS_DATE < GETDATE()- 45      
      
      
IF OBJECT_ID('TEMPDB.DBO.#TEMPRECORD1', 'U') IS NOT NULL DROP TABLE #TEMPRECORD1      
SELECT distinct                                   
trd.Patient_Account,trd.claim_no,SC.PROCESS_DATE,      
trd.Amt_Due,      
DATEDIFF(DAY, CONVERT(VARCHAR, SC.PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF      
into #TEMPRECORD1        
FROM #TEMPRECORD TRD  with (nolock)              
join #SubmissionClaims SC on sc.Claim_No = trd.Claim_No      
       
      
IF OBJECT_ID('TEMPDB.DBO.#fiveHindredAcct', 'U') IS NOT NULL DROP TABLE #fiveHindredAcct      
select fd.Patient_Account,SUM(fd.Amt_Due) Patient_Balance      
into #fiveHindredAcct      
from #TEMPRECORD1 fd with (nolock)     
join #SubmissionClaims  SC with (nolock) on SC.Claim_No = fd.Claim_No      
group by fd.Patient_Account      
having  SUM(fd.Amt_Due) >= 500       
      
      
      
IF OBJECT_ID('TEMPDB.DBO.#PatientNBC', 'U') IS NOT NULL DROP TABLE #PatientNBC      
select SUM(Amount_over_paid) Amount_over_paid,C.Patient_Account      
into #PatientNBC      
from MIS_TBL_Claim_negative_balance nbc  with (nolock)    
join Claims C with (nolock) on C.Claim_No = nbc.Claim_no and ISNULL(C.Deleted,0) = 0 and c.Amt_Due < 0      
where ISNULL(nbc.deleted,0) = 0       
and Payment_Source = 'P'      
and practice_code =  1012714      
group by C.Patient_Account       
      
IF OBJECT_ID('TEMPDB.DBO.#HighBalancePatients', 'U') IS NOT NULL DROP TABLE #HighBalancePatients      
SELECT distinct      
trd.Patient_Account,      
fa.Patient_Balance,      
min(PROCESS_DATE) as PROCESS_DATE      
--DATEDIFF(DAY, CONVERT(VARCHAR, PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF      
into #HighBalancePatients      
FROM #TEMPRECORD1 TRD with (nolock)     
join #fiveHindredAcct fa with (nolock) on fa.Patient_Account = trd.Patient_Account      
left join #PatientNBC nbc with (nolock) on nbc.Patient_Account = trd.Patient_Account      
where isnull(nbc.Amount_over_paid,0) <> 0      
group by trd.Patient_Account,fa.Patient_Balance      
--where trd.Claim_No = 6003723040      
--order by 5 desc      
      
select distinct      
Patient_Account,      
Patient_Balance,      
PROCESS_DATE,      
DATEDIFF(DAY, CONVERT(VARCHAR, PROCESS_DATE, 23), CONVERT(VARCHAR, GETDATE(), 23)) AS CALCULATED_DIFF      
from #HighBalancePatients      
END