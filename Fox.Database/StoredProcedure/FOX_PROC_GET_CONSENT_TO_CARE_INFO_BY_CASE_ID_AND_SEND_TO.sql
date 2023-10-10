 -- =============================================                                
-- AUTHOR:  <Muhammad Salman>                                
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                
-- DESCRIPTION: <GET_CONSENT_TO_CARE>                                              
-- [dbo].[FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID_AND_SEND_TO] 124598, 1012714, "patinet", 545454        
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID_AND_SEND_TO                       
@CASE_ID  BIGINT,                                      
@PRACTICE_CODE  BIGINT,        
@SEND_TO varchar(50),      
@SENT_TO_ID BIGINT                         
AS             
BEGIN          
Select * From FOX_TBL_CONSENT_TO_CARE WITH(NOLOCK)  where CASE_ID = @CASE_ID and DELETED = 0  and PRACTICE_CODE = @PRACTICE_CODE     
 AND SEND_TO = @SEND_TO      
and SENT_TO_ID = @SENT_TO_ID                             
END 