   -- =============================================                                      
-- AUTHOR:  <Muhammad Salman>                                      
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                      
-- DESCRIPTION: <GET_CONSENT_TO_CARE>                                                    
-- [dbo].[FOX_PROC_SAVE_EXTERNAL_USER_INFO] 1012714, '', '','','','','', 1012714                     
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_DETAILS_BY_CASE_ID                             
@CASE_ID  BIGINT,                                            
@PRACTICE_CODE  BIGINT                            
AS                   
BEGIN             
Declare @EXPIRED_STATUS_ID BIGINT =(select top 1 CONSENT_TO_CARE_STATUS_ID From   FOX_TBL_CONSENT_TO_CARE_STATUS WITH(NOLOCK) where STATUS_NAME = 'Expired' and ISNULL(DELETED, 0) = 0  and PRACTICE_CODE = @PRACTICE_CODE)      
Declare @SEND_STATUS_ID BIGINT =(select top 10 CONSENT_TO_CARE_STATUS_ID From   FOX_TBL_CONSENT_TO_CARE_STATUS WITH(NOLOCK) where STATUS_NAME = 'Sent' and ISNULL(DELETED, 0) = 0  and PRACTICE_CODE = @PRACTICE_CODE)        
Update FOX_TBL_CONSENT_TO_CARE SET STATUS_ID = @EXPIRED_STATUS_ID     
where CASE_ID = @CASE_ID and     
ISNULL(DELETED, 0) = 0  and     
PRACTICE_CODE = @PRACTICE_CODE  AND    
STATUS_ID = @SEND_STATUS_ID    
    
IF @@ROWCOUNT > 0    
    SELECT 'True' AS Result    
ELSE    
    SELECT 'False' AS Result    
                                   
END 