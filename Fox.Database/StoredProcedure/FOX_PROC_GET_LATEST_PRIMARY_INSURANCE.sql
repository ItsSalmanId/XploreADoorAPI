IF (OBJECT_ID('FOX_PROC_GET_LATEST_PRIMARY_INSURANCE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_LATEST_PRIMARY_INSURANCE  
GO   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_LATEST_PRIMARY_INSURANCE] --101116354813370    
 @PATIENT_ACCOUNT BIGINT  
AS  
BEGIN  
 SELECT TOP 1 PI.Patient_Insurance_Id  
  ,PI.Patient_Account  
  ,PI.Insurance_Id  
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
  ,PI.Created_By  
  ,PI.Created_Date  
  ,PI.Modified_By  
  ,PI.Modified_Date  
  ,PI.Deleted  
 FROM FOX_TBL_PATIENT_INSURANCE AS PI  
 LEFT JOIN FOX_TBL_INSURANCE fi ON fi.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID   
 AND ISNULL(PI.DELETED, 0) = 0    
 WHERE PI.Patient_Account = @PATIENT_ACCOUNT  
 AND PI.Pri_Sec_Oth_Type ='P'  
 AND (PI.FOX_INSURANCE_STATUS='C' OR PI.FOX_INSURANCE_STATUS='')  
 AND ISNULL(PI.DELETED, 0) = 0    
 ORDER BY PI.Modified_Date DESC  
END  
  
