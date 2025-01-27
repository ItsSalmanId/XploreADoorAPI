IF (OBJECT_ID('FOX_GET_INDEX_ALL_INFO_EMAIL') IS NOT NULL) DROP PROCEDURE FOX_GET_INDEX_ALL_INFO_EMAIL
GO
CREATE PROCEDURE [dbo].[FOX_GET_INDEX_ALL_INFO_EMAIL] (@WORK_ID BIGINT)    
AS    
BEGIN    
 SELECT wq.WORK_ID    
  ,wq.UNIQUE_ID    
  ,wq.DOCUMENT_TYPE
  ,FT.NAME AS DOCUMENT_NAME    
  ,wq.SORCE_NAME    
  ,ISNULL(S.LAST_NAME, '') AS REF_SOURCE_LAST_NAME    
  ,ISNULL(S.FIRST_NAME, '') AS REF_SOURCE_FIRST_NAME    
  ,ISNULL(R.REFERRAL_REGION_NAME, '') AS REFERRAL_REGION_NAME    
  ,ISNULL(wq.FACILITY_NAME, '') AS TREATMENT_LOCATION    
  ,ISNULL(P.FIRST_NAME, '') AS PATIENT_FIRST_NAME    
  ,ISNULL(P.LAST_NAME, '') AS PATIENT_LAST_NAME    
  ,ISNULL(p.GENDER, '') AS PATIENT_GENDER    
  ,P.DATE_OF_BIRTH AS PATIENT_DOB    
  ,ISNULL(P.CHART_ID, '') AS PATIENT_MRN    
  ,ISNULL(p.cell_phone, '') AS PATIENT_PHONE_NO    
  ,ISNULL(PA.ADDRESS, '') AS ADDRESS    
  ,ISNULL(PA.CITY, '') AS CITY    
  ,ISNULL(pa.STATE, '') AS STATE    
  ,ISNULL(PA.ZIP, '') AS ZIP    
  ,ISNULL(PA.COUNTRY, '') AS COUNTRY    
  ,WQ.DEPARTMENT_ID    
  ,ISNULL(IP.INSPAYER_DESCRIPTION, '') AS INS_NAME    
  ,ISNULL(WQ.REASON_FOR_VISIT, '') AS REASON_FOR_VISIT    
  ,ISNULL(WQ.ACCOUNT_NUMBER, '') AS ACCOUNT_NUMBER    
  ,ISNULL(WQ.UNIT_CASE_NO, '') AS UNIT_CASE_NO    
  ,ISNULL(wq.IS_EMERGENCY_ORDER, 0) AS IS_EMERGENCY_ORDER FROM FOX_TBL_WORK_QUEUE WQ    
	left JOIN	  
	fox_tbl_document_type FT ON FT.DOCUMENT_TYPE_ID=WQ.DOCUMENT_TYPE AND ISNULL(WQ.DELETED,0)=0  AND ISNULL(FT.DELETED,0)=0
   LEFT JOIN      
  FOX_TBL_ORDERING_REF_SOURCE S ON S.SOURCE_ID = WQ.SENDER_ID     AND ISNULL(s.DELETED,0)=0
  LEFT JOIN     
  FOX_TBL_REFERRAL_REGION R ON S.REFERRAL_REGION = R.REFERRAL_REGION_CODE and r.practice_code = wq.practice_code    AND ISNULL(r.DELETED,0)=0
  LEFT JOIN PATIENT P ON WQ.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT     AND ISNULL(P.DELETED,0)=0
  LEFT JOIN     
  FOX_TBL_PATIENT_ADDRESS PA ON p.Patient_Account = pa.PATIENT_ACCOUNT AND pa.ADDRESS_TYPE = 'Home Address'     
  LEFT JOIN PATIENT_INSURANCE PIN ON P.PATIENT_ACCOUNT  =  PIN.PATIENT_ACCOUNT AND PIN.PRI_SEC_OTH_TYPE = 'P'   AND ISNULL(PIN.DELETED,0)=0
  LEFT JOIN      
  INSURANCES I ON I.INSURANCE_ID = PIN.INSURANCE_ID  AND ISNULL(I.DELETED,0)=0
  LEFT JOIN       
  INSURANCE_PAYERS IP ON I.INSPAYER_ID = IP.INSPAYER_ID     AND ISNULL(IP.DELETED,0)=0
 WHERE  wq.WORK_ID = @WORK_ID    
END 


/********************************************************************************************************************************************************************************************************/
