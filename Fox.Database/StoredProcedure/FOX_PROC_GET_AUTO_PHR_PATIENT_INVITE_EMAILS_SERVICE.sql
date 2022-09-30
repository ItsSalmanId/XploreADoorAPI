IF (OBJECT_ID('FOX_PROC_GET_AUTO_PHR_PATIENT_INVITE_EMAILS_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_AUTO_PHR_PATIENT_INVITE_EMAILS_SERVICE  
GO    
-- =============================================                    
-- AUTHOR:  MUHAMMAD ARSLAN TUFAIL                    
-- CREATE DATE: 03/11/2022                    
-- DESCRIPTION: THIS PROCEDURE IS USED TO SEND AUTO EMAILS INVITES                    
-- =============================================                    
-- FOX_PROC_GET_AUTO_PHR_PATIENT_INVITE_EMAILS_SERVICE '1011163'                    
CREATE PROCEDURE FOX_PROC_GET_AUTO_PHR_PATIENT_INVITE_EMAILS_SERVICE                    
@PRACTICE_CODE BIGINT                    
AS                    
BEGIN                    
                    
 IF OBJECT_ID('TEMPDB.DBO.#TEMPPATIENTS', 'U') IS NOT NULL DROP TABLE #TEMPPATIENTS;                           
 IF OBJECT_ID('TEMPDB.DBO.#SELECTEDPAT', 'U') IS NOT NULL DROP TABLE #SELECTEDPAT;                        
                    
 SET NOCOUNT ON;                    
                    
 --SELECT  P.PATIENT_ACCOUNT, P.FIRST_NAME, P.LAST_NAME, P.EMAIL_ADDRESS                    
 SELECT  P.PATIENT_ACCOUNT                    
 INTO #TEMPPATIENTS                  
 FROM PATIENT P                           
 INNER JOIN CLAIMS C ON C.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT AND  ISNULL(C.DELETED, 0) = 0                               
 WHERE ISNULL(P.DELETED, 0) = 0 AND P.EMAIL_ADDRESS <> '' AND P.PRACTICE_CODE = @PRACTICE_CODE                 
 AND ISNULL(C.PAT_STATUS,'') IN ('B','D') AND ISNULL(P.PTL_STATUS,0) = 0                      
 GROUP BY P.PATIENT_ACCOUNT                      
 --GROUP BY P.PATIENT_ACCOUNT, P.FIRST_NAME, P.LAST_NAME, P.EMAIL_ADDRESS                                               
 HAVING SUM(C.AMT_DUE) > 100                    
                    
 SELECT PP.PATIENT_ACCOUNT, PP.FIRST_NAME, PP.LAST_NAME, PP.EMAIL_ADDRESS, PP.CELL_PHONE, PP.PRACTICE_CODE                    
 INTO #SELECTEDPAT                    
 FROM #TEMPPATIENTS AS TEMPAT                    
 INNER JOIN PATIENT AS PP ON PP.PATIENT_ACCOUNT = TEMPAT.PATIENT_ACCOUNT                    
                    
 SELECT TOP 100 * FROM #SELECTEDPAT AS SELPAT                    
 WHERE SELPAT.PATIENT_ACCOUNT NOT IN (SELECT PATIENT_ACCOUNT FROM WS_FOX_TBL_PHR_USERS AS PU)                    
                    
END   
  
  