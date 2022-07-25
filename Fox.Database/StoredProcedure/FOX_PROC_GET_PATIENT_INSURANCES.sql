IF (OBJECT_ID('FOX_PROC_GET_PATIENT_INSURANCES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_INSURANCES  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_INSURANCES] --101116354813760                
 @PATIENT_ACCOUNT BIGINT                  
AS                  
BEGIN                  
 SELECT PI.Patient_Insurance_Id                  
  ,PI.Patient_Account                  
  ,PI.Insurance_Id                  
  ,fi.INSURANCE_PAYERS_ID AS InsPayer_Id                  
  ,fi.INSURANCE_NAME AS InsPayer_Description                  
  ,fi.FOX_TBL_INSURANCE_ID                  
  ,PI.Pri_Sec_Oth_Type                  
  ,PI.Co_Payment                  
  ,PI.Deductions                  
  ,PI.Policy_Number                  
  ,PI.Group_Number                  
  ,PI.Effective_Date                  
  ,PI.Termination_Date                  
  ,PI.Relationship                
  --sattar change                
  ,isnull(PI.INACTIVE,0) INACTIVE                
  ,PI.Plan_Name                
  ,PI.SUPRESS_BILLING_UNTIL                
  ,PI.Subscriber                
  ,PI.FINANCIAL_CLASS_ID                
  ,fc.CODE as FINANCIAL_CLASS_CODE        
  ,PI.FOX_INSURANCE_STATUS        
  ,mtbcpi.Patient_Insurance_Id as MTBC_Patient_Insurance_Id               
  --sattar change                  
  ,PI.Created_By                  
  ,PI.Created_Date                  
  ,PI.Modified_By                  
  ,PI.Modified_Date                  
  ,fi.PHONE Insurance_Phone_Number1                  
  ,fi.[ADDRESS] Insurance_Address                  
  ,fi.ZIP Insurance_Zip                  
  ,fi.CITY Insurance_City                  
  ,fi.[STATE] Insurance_State                  
  ,PI.Deleted                  
  ,PI.IS_PRIVATE_PAY              
  ,isnull(mtbcpi.Eligibility_Status,'') Eligibility_Status          
 FROM FOX_TBL_PATIENT_INSURANCE PI                  
 JOIN FOX_TBL_INSURANCE fi ON fi.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID              
 left join PATIENT_INSURANCE mtbcpi on mtbcpi.Patient_Insurance_Id = PI.MTBC_Patient_Insurance_Id          
 left join FOX_TBL_FINANCIAL_CLASS fc on fc.FINANCIAL_CLASS_ID =   PI.FINANCIAL_CLASS_ID            
 --JOIN INSURANCES I ON PI.Insurance_Id = I.Insurance_Id                  
 --JOIN Insurance_Payers IP ON I.InsPayer_Id = IP.InsPayer_Id                  
 WHERE PI.Patient_Account = @PATIENT_ACCOUNT                
 --sattar change                 
     AND ISNULL(PI.Deleted,0) = 0                 
 and LOWER(PI.FOX_INSURANCE_STATUS) = 'c'                  
 --sattar change                  
---------------------------------------------                
 --SELECT PI.Patient_Insurance_Id                  
 -- ,PI.Patient_Account                  
 -- ,PI.Insurance_Id                  
 -- ,IP.InsPayer_Id                  
 -- ,fi.INSURANCE_NAME AS InsPayer_Description                  
 -- ,fi.FOX_TBL_INSURANCE_ID                  
 -- ,PI.Pri_Sec_Oth_Type                  
 -- ,PI.Co_Payment                  
 -- ,PI.Deductions                  
 -- ,PI.Policy_Number                  
 -- ,PI.Group_Number                  
 -- ,PI.Effective_Date                  
 -- ,PI.Termination_Date                  
 -- ,PI.Relationship                  
 -- ,PI.Created_By                  
 -- ,PI.Created_Date                  
 -- ,PI.Modified_By                  
 -- ,PI.Modified_Date                  
 -- ,I.Insurance_Phone_Number1                  
 -- ,I.Insurance_Address                  
 -- ,I.Insurance_Address                  
 -- ,I.Insurance_Zip                  
 -- ,I.Insurance_City                  
 -- ,I.Insurance_State                  
 -- ,PI.Deleted                  
 --FROM FOX_TBL_PATIENT_INSURANCE PI                  
 --JOIN FOX_TBL_INSURANCE fi ON fi.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID                  
 --JOIN INSURANCES I ON PI.Insurance_Id = I.Insurance_Id                  
 --JOIN Insurance_Payers IP ON I.InsPayer_Id = IP.InsPayer_Id                  
 --WHERE PI.Patient_Account = @PATIENT_ACCOUNT                  
END 

