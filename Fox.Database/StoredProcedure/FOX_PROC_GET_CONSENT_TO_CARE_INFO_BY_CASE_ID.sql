     -- =============================================                                          
-- AUTHOR:  <Muhammad Salman>                                          
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                          
-- DESCRIPTION: <GET_CONSENT_TO_CARE>                                                        
-- [dbo].[FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID] 1012714, 534678                        
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID                        
@CASE_ID  BIGINT,                                                
@PRACTICE_CODE  BIGINT                                      
AS                       
BEGIN                    
Select            
ctc.CONSENT_TO_CARE_ID,          
ctc.CASE_ID,          
ctc.PATIENT_ACCOUNT,             
sta.STATUS_NAME AS STATUS,       
case                     
when isnull(p.last_name,'')='' then p.first_name                     
else '('+ p.last_name +', '+ p.first_name + ')'                   
END as PATIENT_FULL_NAME,                         
case                     
when isnull(pc.last_name,'')='' then pc.first_name                    
else '('+ pc.last_name +', '+pc.first_name + ')'                          
END as CONTACTS_FULL_NAME    ,          
ctc.*         
From FOX_TBL_CONSENT_TO_CARE ctc WITH(NOLOCK)                    
left join FOX_TBL_CONSENT_TO_CARE_STATUS sta WITH(NOLOCK) on sta.CONSENT_TO_CARE_STATUS_ID = ctc.STATUS_ID        
left join PATIENT p WITH(NOLOCK) on ctc.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT      
left join Fox_Tbl_Patient_Contacts pc WITH(NOLOCK) on      ctc.SENT_TO_ID = pc.Contact_ID       
where ctc.CASE_ID = @CASE_ID         
and ctc.DELETED = 0  and ctc.PRACTICE_CODE = @PRACTICE_CODE              
and sta.DELETED = 0  and sta.PRACTICE_CODE = @PRACTICE_CODE                                     
              
END  
  
  