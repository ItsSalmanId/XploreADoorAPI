 -- =============================================                                        
-- AUTHOR:  <Muhammad Salman>                                        
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                        
-- DESCRIPTION: <GET_CONSENT_TO_CARE>                                                      
-- [dbo].[FOX_PROC_GET_CONSENT_TO_CARE_DOCUMENTS_INFO] 531555,1012714,       
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_DOCUMENTS_INFO                       
@CONSENT_TO_CARE_ID  BIGINT,                                              
@PRACTICE_CODE  BIGINT                                    
AS                     
BEGIN                  
Select         
CONSENT_TO_CARE_ID,    
LOGO_PATH,      
IMAGE_PATH        
From FOX_TBL_CONSENT_TO_CARE_DOCUMENTS ctc WITH(NOLOCK)                 
where CONSENT_TO_CARE_ID = @CONSENT_TO_CARE_ID       
and ctc.DELETED = 0  and ctc.PRACTICE_CODE = @PRACTICE_CODE                                             
END