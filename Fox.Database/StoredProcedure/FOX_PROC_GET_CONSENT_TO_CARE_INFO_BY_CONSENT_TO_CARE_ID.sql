  -- =============================================                                                    
-- AUTHOR:  <Muhammad Salman>                                                    
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                                    
-- DESCRIPTION: <GET_CONSENT_TO_CARE>                                                                  
-- [dbo].[FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CONSENT_TO_CARE_ID] 1012714, '', '','','','','', 1012714                                   
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CONSENT_TO_CARE_ID--  534615, 1012714                                     
@CONSENT_TO_CARE_ID  BIGINT ,                          
@PRACTICE_CODE  BIGINT                                           
AS                                 
BEGIN                         
                      
Select p.Date_Of_Birth,TEMPLATE_HTML,                        
P.Last_Name                        
,P.First_Name                        
,P.Gender                      
,case                         
when isnull(au.last_name,'')='' then au.first_name                        
else au.last_name +', '+au.first_name                        
END as PROVIDER_FULL_NAME                      
,ipa.INSURANCE_PAYERS_ID,                        
case                         
when isnull(p.last_name,'')='' then p.first_name                                 
else p.first_name +', '+ ISNULL(LEFT(p.last_name  , 1), '')                    
END as PATIENT_FULL_NAME,          
case                         
when isnull(p.last_name,'')='' then p.first_name                        
else p.last_name +', '+ p.first_name                           
END as PATIENT_NAME,                       
dis.DESCRIPTION As DESCRIPTION,                   
case                         
when isnull(FTPC.last_name,'')='' then FTPC.first_name                        
else FTPC.last_name +', '+FTPC.first_name                              
END as CONTACTS_FULL_NAME    ,          
case                     
when isnull(FTORS.first_name,'')='' then FTORS.last_name                    
else FTORS.FIRST_NAME +' '+ ISNULL(LEFT(FTORS.LAST_NAME, 1), '')                        
END as OrderingRefNotes,           
STATUS_NAME,                 
cc.*                        
From FOX_TBL_CONSENT_TO_CARE cc  WITH(NOLOCK)                   
left outer join FOX_TBL_CONSENT_TO_CARE_STATUS cs WITH(NOLOCK) on  cs.CONSENT_TO_CARE_STATUS_ID = cc.STATUS_ID                         
left outer join PATIENT p WITH(NOLOCK) on  cc.Patient_Account = p.PATIENT_ACCOUNT                    
left outer join Fox_Tbl_Patient_Contacts FTPC WITH(NOLOCK) on  FTPC.Contact_ID = cc.SENT_TO_ID                        
left outer join FOX_TBL_PATIENT_INSURANCE pipa WITH(NOLOCK) on  cc.Patient_Account = pipa.PATIENT_ACCOUNT                         
left outer join FOX_TBL_INSURANCE ipa WITH(NOLOCK) on  ipa.FOX_TBL_INSURANCE_ID = pipa.FOX_TBL_INSURANCE_ID                         
left outer join FOX_TBL_APPLICATION_USER au WITH(NOLOCK) on  au.USER_NAME = cc.CREATED_BY                       
left outer join FOX_VW_CASE vwc WITH(NOLOCK) on  vwc.CASE_ID = cc.CASE_ID                     
left outer join FOX_TBL_DISCIPLINE dis WITH(NOLOCK) on  dis.DISCIPLINE_ID = vwc.DISCIPLINE_ID      
left outer join FOX_TBL_CASE FTC WITH(NOLOCK) on      FTC.CASE_ID = cc.CASE_ID     
left  outer join FOX_TBL_ORDERING_REF_SOURCE FTORS WITH(NOLOCK) on      FTORS.SOURCE_ID = FTC.ORDERING_REF_SOURCE_ID                    
where CONSENT_TO_CARE_ID = @CONSENT_TO_CARE_ID and cc.DELETED = 0  and cc.PRACTICE_CODE = @PRACTICE_CODE                            
                                              
END 