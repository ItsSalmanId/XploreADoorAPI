-- =============================================                          
-- AUTHOR:  MUHAMMAD SALMAN                          
-- CREATE DATE: 03-22-2023                          
-- DESCRIPTION: THIS SP IS TRIGGER TO GET DETAILS OF INSURANCE                          
-- =============================================                          
-- EXEC FOX_PROC_GET_INSURANCE_DETAILS_FOR_CONSENT_TO_CARE '00967', '1012714'                          
CREATE PROCEDURE FOX_PROC_GET_INSURANCE_DETAILS_FOR_CONSENT_TO_CARE ---   1012714534318092901,53413432                      
(                          
 @PATINET_ACCOUNT BIGINT,                               
 @CASE_ID BIGINT                                     
)                          
AS                          
BEGIN                          
                           
SELECT                     
TOP 1                  
i.INSURANCE_NAME ,                    
pi.Is_Authorization_Required,                    
CONVERT(varchar, pi.Co_Payment) AS Co_Payment,                    
pi.IS_COPAY_PER,                    
pi.GENERAL_COMMENTS,                 
pi.BENEFIT_COMMENTS,                    
CONVERT(varchar,pi.PT_ST_TOT_AMT_USED) AS PT_ST_TOT_AMT_USED,                    
CONVERT(varchar,pi.OT_TOT_AMT_USED) AS OT_TOT_AMT_USED,                   
CONVERT(varchar,pi.YEARLY_DED_AMT) AS YEARLY_DED_AMT,                  
CONVERT(varchar, pi.YEARLY_DED_AMT-pi.DED_MET) AS DED_REMAINING,                  
fta.EFFECTIVE_FROM,                  
fta.EFFECTIVE_TO,                  
convert(varchar,MULT_USED) MULT_USED                        
,convert(varchar,MULT_REMAINING) MULT_REMAINING,                  
convert(varchar,MULT_VALUE) MULT_VALUE,                  
AUTH_NUMBER ,                  
Pri_Sec_Oth_Type,         
CONVERT(varchar,pi.DED_MET) AS DED_MET,        
CONVERT(varchar,pi.MOP_AMT) AS MOP_AMT,        
CONVERT(varchar,pi.MYB_LIMIT_VISIT) AS MYB_LIMIT_VISI,        
CONVERT(varchar,pi.MYB_LIMIT_DOLLARS) AS MYB_LIMIT_DOLLARS,         
CONVERT(varchar, pi.MOP_AMT - pi.MOP_USED_OUTSIDE_RT) AS MOP_AMT_REMAINING                                       
                    
from FOX_TBL_PATIENT_INSURANCE pi WITH(NOLOCK)                                            
left join fox_tbl_insurance i WITH(NOLOCK) on i.FOX_TBL_INSURANCE_ID=pi.FOX_TBL_INSURANCE_ID                      
left join fox_tbl_auth fta WITH(NOLOCK) on fta.Parent_Patient_insurance_Id=pi.patient_insurance_id                                
where                     
pi.Patient_Account=@PATINET_ACCOUNT and isnull(pi.deleted,0)<>1 and pi.Pri_Sec_Oth_Type= 'P'                     
and isnull(pi.INACTIVE,0)<>1                             
and FOX_INSURANCE_STATUS='C'                     
AND  pi.INACTIVE = 0                                              
AND ((pi.Termination_Date IS NULL AND  CONVERT(DATE,GETDATE())  >= CONVERT(DATE,pi.Effective_Date) ) OR ((pi.Effective_Date IS NOT NULL AND pi.Termination_Date IS NOT NULL) AND CONVERT(DATE,GETDATE())                                                
BETWEEN CONVERT(DATE,pi.Effective_Date)  AND CONVERT(DATE,pi.Termination_Date)))                               
                  
UNION ALL                  
(                  
SELECT                     
TOP 1                  
i.INSURANCE_NAME ,                    
pi.Is_Authorization_Required,                    
CONVERT(varchar, pi.Co_Payment) AS Co_Payment,                    
pi.IS_COPAY_PER,                    
pi.GENERAL_COMMENTS,                  
pi.BENEFIT_COMMENTS,                  
CONVERT(varchar,pi.PT_ST_TOT_AMT_USED) AS PT_ST_TOT_AMT_USED,                    
CONVERT(varchar,pi.OT_TOT_AMT_USED) AS OT_TOT_AMT_USED,                   
CONVERT(varchar,pi.YEARLY_DED_AMT) AS YEARLY_DED_AMT,                  
CONVERT(varchar, pi.YEARLY_DED_AMT-pi.DED_MET) AS DED_REMAINING,                  
fta.EFFECTIVE_FROM,                  
fta.EFFECTIVE_TO,                  
convert(varchar,MULT_USED) MULT_USED                        
,convert(varchar,MULT_REMAINING) MULT_REMAINING,                  
convert(varchar,MULT_VALUE) MULT_VALUE,                  
AUTH_NUMBER   ,                  
Pri_Sec_Oth_Type,        
CONVERT(varchar,pi.DED_MET) AS DED_MET,        
CONVERT(varchar,pi.MOP_AMT) AS MOP_AMT,        
CONVERT(varchar,pi.MYB_LIMIT_VISIT) AS MYB_LIMIT_VISI,        
CONVERT(varchar,pi.MYB_LIMIT_DOLLARS) AS MYB_LIMIT_DOLLARS ,        
CONVERT(varchar, pi.MOP_AMT - pi.MOP_USED_OUTSIDE_RT) AS MOP_AMT_REMAINING                                          
                    
from FOX_TBL_PATIENT_INSURANCE pi WITH(NOLOCK)                                           
left join fox_tbl_insurance i WITH(NOLOCK) on i.FOX_TBL_INSURANCE_ID=pi.FOX_TBL_INSURANCE_ID                      
left join fox_tbl_auth fta WITH(NOLOCK) on fta.Parent_Patient_insurance_Id=pi.patient_insurance_id                                
where                     
pi.Patient_Account=@PATINET_ACCOUNT and isnull(pi.deleted,0)<>1 and pi.Pri_Sec_Oth_Type= 'S'                     
and isnull(pi.INACTIVE,0)<>1                             
and FOX_INSURANCE_STATUS='C'                     
AND  pi.INACTIVE = 0                                              
AND ((pi.Termination_Date IS NULL AND  CONVERT(DATE,GETDATE())  >= CONVERT(DATE,pi.Effective_Date) ) OR ((pi.Effective_Date IS NOT NULL AND pi.Termination_Date IS NOT NULL) AND CONVERT(DATE,GETDATE())                                                
BETWEEN CONVERT(DATE,pi.Effective_Date)  AND CONVERT(DATE,pi.Termination_Date)))                   
)                   
                             
END 