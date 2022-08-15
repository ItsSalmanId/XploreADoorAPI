IF (OBJECT_ID('FOX_PROC_GET_UNPAID_INSURANCE_CLAIMS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_UNPAID_INSURANCE_CLAIMS  
GO 
CREATE PROC [dbo].[FOX_PROC_GET_UNPAID_INSURANCE_CLAIMS]
(@PRACTICE_CODE         BIGINT, 
 @PATIENT_ACCOUNT       BIGINT, 
 @PATIENT_INSURANCE_IDS VARCHAR(MAX), 
 @EFFECTIVE_DATE        VARCHAR(15) NULL, 
 @TERMINATION_DATE      VARCHAR(15) NULL
)
AS        
    ----------------------------------------------        
    --DECLARE        
    --@PRACTICE_CODE        BIGINT=1011163,        
    --@PATIENT_ACCOUNT      BIGINT=101116354910961,        
    --@PATIENT_INSURANCE_ID BIGINT=54862937594084,      
    --@EFFECTIVE_DATE       VARCHAR(15) = '05/01/2018',       
    --@TERMINATION_DATE     VARCHAR(15) = NULL        
    ----------------------------------------------        
     BEGIN
         IF OBJECT_ID('tempdb..#TEMP_FOX_CLAIMS') IS NOT NULL
             DROP TABLE #TEMP_FOX_CLAIMS;
         IF OBJECT_ID('tempdb..#TEMP_PATIENT_INS_IDS') IS NOT NULL
             DROP TABLE #TEMP_PATIENT_INS_IDS;
         CREATE TABLE #TEMP_FOX_CLAIMS
         (Patient_Account      BIGINT, 
          Patient_Insurance_Id BIGINT, 
          Claim_Insurance_Id   BIGINT, 
          Insurance_Id         BIGINT, 
          DOS                  DATETIME NULL, 
          Claim_No             BIGINT,         
          --CASE_NO              VARCHAR(20) NULL,         
          AMOUNT_BILLED        VARCHAR(20) NULL, 
          AMOUNT_PAID          MONEY NULL, 
          AMOUNT_DUE           MONEY NULL, 
          Pri_Sec_Oth_Type     VARCHAR,        
          --PRI_STATUS           VARCHAR NULL,         
          --SEC_STATUS           VARCHAR NULL,         
          --OTH_STATUS           VARCHAR NULL,         
          --PAT_STATUS           VARCHAR NULL,         
          [STATUS]             VARCHAR NULL,
         );        
         -------------------------------------  
         CREATE TABLE #TEMP_PATIENT_INS_IDS
         (Patient_Insurance_Id BIGINT
         );        
         -------------------------------------     
         INSERT INTO #TEMP_PATIENT_INS_IDS
                SELECT r.splitdata
                FROM foxSplitString(@PATIENT_INSURANCE_IDS, ',') r;  
         -------------------------------------  
         INSERT INTO #TEMP_FOX_CLAIMS
                SELECT ftpi.Patient_Account, 
                       ftpi.Patient_Insurance_Id, 
                       ci.Claim_Insurance_Id, 
                       ci.Insurance_Id, 
                       c.DOS, 
                       c.Claim_No,          
                       --c.CASE_NO CASE_NO,        
                       c.Claim_Total AMOUNT_BILLED, 
                       c.Amt_Paid AMOUNT_PAID, 
                       c.Amt_Due AMOUNT_DUE, 
                       ftpi.Pri_Sec_Oth_Type,      
                       --c.PRI_STATUS, c.SEC_STATUS, c.OTH_STATUS, c.PAT_STATUS,      
                       CASE
                           WHEN ftpi.Pri_Sec_Oth_Type = 'P'
                           THEN c.PRI_STATUS
                           WHEN ftpi.Pri_Sec_Oth_Type = 'S'
                           THEN c.SEC_STATUS
                           WHEN ftpi.Pri_Sec_Oth_Type IN('T', 'Q')
                           THEN c.OTH_STATUS
                           WHEN ftpi.Pri_Sec_Oth_Type = 'PR'
                           THEN c.PAT_STATUS
                           ELSE ''
                       END AS [STATUS]
                FROM dbo.FOX_TBL_PATIENT_INSURANCE ftpi
                     INNER JOIN dbo.FOX_TBL_INSURANCE fti ON ftpi.FOX_TBL_INSURANCE_ID = fti.FOX_TBL_INSURANCE_ID
                     INNER JOIN dbo.Claims c ON C.Patient_Account = ftpi.Patient_Account
                     INNER JOIN dbo.Claim_Insurance ci ON ci.Claim_No = c.Claim_No
                                                          AND fti.Insurance_Id = ci.Insurance_Id
                                                          AND ci.Pri_Sec_Oth_Type = ftpi.Pri_Sec_Oth_Type
                WHERE ftpi.Patient_Insurance_Id IN
      (
                    SELECT Patient_Insurance_Id
                    FROM #TEMP_PATIENT_INS_IDS
                )
                      AND ftpi.Patient_Account = @PATIENT_ACCOUNT
                      AND c.DOS IS NOT NULL
                      AND (@EFFECTIVE_DATE IS NULL
                           OR CAST(c.DOS AS DATE) >= CONVERT(DATE, @EFFECTIVE_DATE, 101))
                      AND (@TERMINATION_DATE IS NULL
                           OR CAST(c.DOS AS DATE) <= CONVERT(DATE, @TERMINATION_DATE, 101))
                      AND isnull(c.PRI_STATUS, '') IN('B', 'R')
                     AND isnull(c.SEC_STATUS, '') NOT IN('N', 'B', 'R')
                AND isnull(c.OTH_STATUS, '') NOT IN('N', 'B', 'R')
         AND isnull(c.Amt_Due, 0) > 0;      
         ------------------------------      
         SELECT A.Patient_Account, 
                A.Patient_Insurance_Id, 
                A.Claim_Insurance_Id, 
                A.Insurance_Id, 
                A.DOS, 
                A.Claim_No,                    
                --,A.CASE_NO                   
                --,A.AMOUNT_BILLED             
                A.AMOUNT_PAID, 
                A.AMOUNT_DUE,
                CASE
                    WHEN A.Pri_Sec_Oth_Type = 'P'
                         AND isnull(A.CLAIM_STATUS, '') <> ''
                    THEN 'Pri - '+A.CLAIM_STATUS
                    WHEN A.Pri_Sec_Oth_Type = 'S'
                         AND isnull(A.CLAIM_STATUS, '') <> ''
                    THEN 'Sec - '+A.CLAIM_STATUS
                    WHEN A.Pri_Sec_Oth_Type IN('T', 'Q')
                         AND isnull(A.CLAIM_STATUS, '') <> ''
                    THEN 'Oth - '+A.CLAIM_STATUS
                    WHEN A.Pri_Sec_Oth_Type = 'PR'
                         AND isnull(A.CLAIM_STATUS, '') <> ''
                    THEN 'Patient - '+A.CLAIM_STATUS
                    ELSE ''
                END AS STATUS
         FROM
         (
             SELECT Patient_Account, 
                    Patient_Insurance_Id, 
                    Claim_Insurance_Id, 
                    Insurance_Id, 
                    DOS, 
                    Claim_No,                    
                    --,CASE_NO                   
                    --,AMOUNT_BILLED              
                    AMOUNT_PAID, 
                    AMOUNT_DUE,
                    CASE
                        WHEN [STATUS] = 'P'
                        THEN 'Paid'
                        WHEN [STATUS] = 'W'
                        THEN 'Waiting'
                        WHEN [STATUS] = 'N'
                        THEN 'New'
                        WHEN [STATUS] = 'B'
                        THEN 'Billed'
                        WHEN [STATUS] = 'R'
                        THEN 'Rebilled'
                        WHEN [STATUS] = 'D'
                        THEN 'Dormant'
                        WHEN [STATUS] = 'C'
                        THEN 'Collection'
                        ELSE ''
                    END AS CLAIM_STATUS, 
                    Pri_Sec_Oth_Type
             FROM #TEMP_FOX_CLAIMS
         ) A
         ORDER BY A.DOS DESC;
     END;
