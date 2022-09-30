IF (OBJECT_ID('FOX_PROC_GET_INSURANCE_PAYER_NAME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INSURANCE_PAYER_NAME 
GO     
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INSURANCE_PAYER_NAME] --101116354412324, 500212  
 (  
 @PATIENT_ACCOUNT BIGINT  
 ,@INSURANCE_ID BIGINT  
 )  
AS  
BEGIN  
 SELECT PI.Patient_Insurance_Id  
  ,PI.Patient_Account  
  ,PI.Insurance_Id  
  ,IP.InsPayer_Id  
  ,IP.InsPayer_Description  
  ,PI.Pri_Sec_Oth_Type  
  ,PI.Co_Payment  
  ,PI.Deductions  
  ,PI.Policy_Number  
  ,PI.Group_Number  
  ,PI.Effective_Date  
  ,PI.Termination_Date  
  ,PI.Relationship  
  ,PI.Created_By  
  ,PI.Created_Date  
  ,PI.Modified_By  
  ,PI.Modified_Date  
  ,I.Insurance_Phone_Number1  
  ,I.Insurance_Address  
  ,I.Insurance_Address  
  ,I.Insurance_Zip  
  ,I.Insurance_City  
  ,I.Insurance_State  
  ,PI.Deleted  
 FROM FOX_TBL_PATIENT_INSURANCE PI  
 JOIN FOX_TBL_INSURANCE FI ON FI.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID  
 JOIN INSURANCES I ON I.Insurance_Id = FI.Insurance_Id  
 JOIN Insurance_Payers IP ON I.InsPayer_Id = IP.InsPayer_Id  
 WHERE PI.Patient_Account = @PATIENT_ACCOUNT  
  AND I.Insurance_Id = @INSURANCE_ID  
END  
