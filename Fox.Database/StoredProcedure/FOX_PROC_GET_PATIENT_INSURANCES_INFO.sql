IF (OBJECT_ID('FOX_PROC_GET_PATIENT_INSURANCES_INFO') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_INSURANCES_INFO  
GO 
CREATE Procedure [dbo].[FOX_PROC_GET_PATIENT_INSURANCES_INFO]  --101116354412413                    
  -- ADD THE PARAMETERS FOR THE STORED PROCEDURE HERE                        
 @PATIENTACCOUNT BIGINT                 
                      
AS                        
BEGIN                        
 -- SET NOCOUNT ON ADDED TO PREVENT EXTRA RESULT SETS FROM                        
 -- INTERFERING WITH SELECT STATEMENTS.                        
SET NOCOUNT ON;            
          
--DECLARE @PATIENTACCOUNT BIGINT       = 1012629500548          
DECLARE @PROVIDER  TABLE          
(          
 PROVID_LNAME VARCHAR(100),                
 PROVID_FNAME VARCHAR(100),          
 PROVID_STATE_LICENSE VARCHAR(12),          
 SSN VARCHAR(9),          
 NPI VARCHAR(15),          
 PRACTICE_CODE BIGINT,          
 Group_NPI VARCHAR(20)          
)          
          
INSERT INTO @PROVIDER           
SELECT TOP 1 PS.PROVID_LNAME,PS.PROVID_FNAME,PS.PROVID_STATE_LICENSE,PS.SSN,ISNULL(PP1.Individual_NPI,PS.NPI)NPI,PS.PRACTICE_CODE,PP1.Group_NPI           
FROM PATIENT P           
JOIN PROVIDERS PS WITH(NOLOCK)  ON PS.PRACTICE_CODE = P.PRACTICE_CODE            
JOIN PROVIDER_PAYERS PP1  WITH(NOLOCK) ON PS.PROVIDER_CODE = PP1.PROVIDER_CODE AND ISNULL(PP1.DELETED,0) = 0          
WHERE P.PATIENT_ACCOUNT = @PATIENTACCOUNT AND ISNULL(PS.DELETED,0) = 0 AND ISNULL(PS.IS_ACTIVE,0) = 1           
          
--SELECT @@ROWCOUNT          
          
IF @@ROWCOUNT = 0          
BEGIN          
INSERT INTO @PROVIDER           
SELECT TOP 1 PS.PROVID_LNAME,PS.PROVID_FNAME,PS.PROVID_STATE_LICENSE,PS.SSN,PS.NPI,PS.PRACTICE_CODE,Null as Group_NPI          
FROM PATIENT P           
JOIN PROVIDERS PS WITH(NOLOCK)  ON PS.PRACTICE_CODE = P.PRACTICE_CODE          
WHERE P.PATIENT_ACCOUNT = @PATIENTACCOUNT AND ISNULL(PS.NPI,'') != '' AND ISNULL(PS.DELETED,0) = 0 AND ISNULL(PS.IS_ACTIVE,0) = 1           
END          
          
--SELECT * FROM @PROVIDER          
           
           
-- INSERT STATEMENTS FOR PROCEDURE HERE                        
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                      
SELECT DISTINCT CASE ISNULL(PI.PRI_SEC_OTH_TYPE,'') WHEN 'P' THEN 1 WHEN 'S' THEN 2 WHEN 'T' THEN 3 WHEN 'Q' THEN 4 WHEN 'O' THEN 5 END AS INS_TYPE,   
PI.PATIENT_INSURANCE_ID,PI.Parent_Patient_insurance_Id,PI.MTBC_Patient_Insurance_Id,PI.PATIENT_INSURANCE_ID,PI.RELATIONSHIP,PI.GROUP_NUMBER , PI.POLICY_NUMBER,P.ZIP,P.STATE,P.ADDRESS,  
P.LAST_NAME,P.CITY,P.FIRST_NAME,P.SSN,P.DATE_OF_BIRTH, P.GENDER,ISNULL(G.GUARANT_LNAME,'') AS GURANTOR_LNAME,P.PATIENT_ACCOUNT,IP.INSPAYER_DESCRIPTION,FOX_INS.INSURANCE_NAME as Fox_Insurance_Name,      
ISNULL(G.GUARANT_FNAME,'') AS GURANTOR_FNAME,ISNULL(G.GUARANT_DOB,'01/01/1900') AS GUARANTOR_DOB,ISNULL(G.GUARANT_SSN,'') AS GUARANTOR_SSN, PI.PRI_SEC_OTH_TYPE,                      
ISNULL(G.GUARANT_GENDER,'') AS GUARANTOR_GENDER,PRC.PRAC_NAME AS PRACTICE_NAME,ISNULL(PRO.Group_NPI,PRC.NPI) AS PRACTICE_NPI,INS.INSURANCE_ID,PI.FOX_TBL_INSURANCE_ID,IP.INSPAYER_ID,                      
PRC.PRAC_TAX_ID AS PRACTICE_TAX_ID, PRO.PROVID_LNAME AS PROVIDER_LNAME,                
ISNULL(G.GUARANT_ADDRESS,'') AS GUARANT_ADDRESS, ISNULL(G.GUARANT_CITY,'') AS GUARANT_CITY,ISNULL(G.GUARANT_STATE,'') AS GUARANT_STATE, ISNULL(G.GUARANT_ZIP,'') AS GUARANT_ZIP,                      
PRO.PROVID_FNAME AS PROVIDER_FNAME,PRO.PROVID_STATE_LICENSE AS PROVID_STATE_LICENSE,PRO.SSN AS PROVIDER_SSN,ISNULL(E1.IS_REALTIME,'N') AS REALTIMEELIG,                      
CONVERT(DATETIME,'1900-01-01') AS PROVIDER_DOB,PRO.NPI AS PROVIDER_NPI,                
'' AS PROVIDER_NUMBER,                      
E1.INSPAYER_ELIGIBILITY_ID,E1.PAYER_NAME,E1.PAYER_SOURCE,PRC.PRACTICE_CODE,PRC.PRAC_TYPE                      
FROM PATIENT P WITH(NOLOCK)                    
JOIN PRACTICES PRC  WITH(NOLOCK) ON P.PRACTICE_CODE = PRC.PRACTICE_CODE                    
JOIN FOX_TBL_PATIENT_INSURANCE PI WITH(NOLOCK) ON P.PATIENT_ACCOUNT = PI.PATIENT_ACCOUNT AND PI.FOX_INSURANCE_STATUS = 'C'          
--JOIN INSURANCES INS  WITH(NOLOCK) ON PI.INSURANCE_ID = INS.INSURANCE_ID          
JOIN FOX_TBL_INSURANCE FOX_INS  WITH(NOLOCK) ON FOX_INS.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID           
JOIN INSURANCES INS  WITH(NOLOCK) ON INS.INSURANCE_ID = FOX_INS.INSURANCE_ID          
JOIN INSURANCE_PAYERS IP  WITH(NOLOCK) ON INS.INSPAYER_ID = IP.INSPAYER_ID                    
LEFT OUTER JOIN @PROVIDER PRO ON PRO.PRACTICE_CODE = P.PRACTICE_CODE           
LEFT OUTER JOIN GUARANTORS G  WITH(NOLOCK) ON PI.SUBSCRIBER = G.GUARANTOR_CODE AND ISNULL(G.DELETED,0) <> 1                      
LEFT OUTER JOIN ELIGIBILITY_PAYER_INFORMATION E1 WITH(NOLOCK) ON E1.INSPAYER_ELIGIBILITY_ID = IP.INSPAYER_ELIGIBILITY_ID             
AND E1.WEB_ACCESS<>'N' AND ISNULL(E1.DELETED,0)<> 1                      
WHERE P.PATIENT_ACCOUNT = @PATIENTACCOUNT                 
AND ISNULL(PI.PRI_SEC_OTH_TYPE,'') != ''            
AND ISNULL(PI.DELETED,0) = 0                    
AND ISNULL(INS.DELETED,0) = 0                       
AND ISNULL(IP.DELETED,0) = 0          
ORDER BY 1               
           
END