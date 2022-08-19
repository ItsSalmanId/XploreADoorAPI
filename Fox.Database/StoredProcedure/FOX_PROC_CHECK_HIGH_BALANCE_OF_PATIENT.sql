IF (OBJECT_ID('FOX_PROC_CHECK_HIGH_BALANCE_OF_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHECK_HIGH_BALANCE_OF_PATIENT  
GO   
-- =============================================                                                
-- Author:  <Abdur Rafay>                                                
-- Create date: <12/14/2021>                                                
-- Description: <CHECK HIGH BALANCE OF PATIENT>                                                
-- =============================================                                                
-- EXEC   FOX_PROC_CHECK_HIGH_BALANCE_OF_PATIENT 1012714                  
CREATE PROCEDURE FOX_PROC_CHECK_HIGH_BALANCE_OF_PATIENT                  
 @PRACTICE_CODE BIGINT                     
AS                    
BEGIN                    
                    
SELECT DISTINCT clm.patient_account as Patient_Account  
FROM claims clm                    
join PATIENT PAT ON CLM.patient_account = PAT.PATIENT_ACCOUNT AND ISNULL(pat.deleted, 0) = 0    
JOIN CLAIM_PAYMENTS CP ON CP.CLAIM_NO = CLM.CLAIM_NO AND ISNULL(CP.DELETED,0) = 0                  
WHERE isnull(clm.deleted, 0) = 0      
and pat.practice_code = @PRACTICE_CODE                    
AND clm.pat_status is not null and clm.pat_status IN('N','R','B','C','D')     
GROUP BY clm.patient_account                           
Having SUM(amt_due) > 4000   
AND DATEDIFF(DAY,MAX(CP.DATE_ENTRY),GETDATE()) > 45                   
                  
END