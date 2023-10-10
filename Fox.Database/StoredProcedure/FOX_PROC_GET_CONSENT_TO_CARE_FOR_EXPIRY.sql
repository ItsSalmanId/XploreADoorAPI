-- =============================================            
-- Author:  <Abdul Sattar>            
-- Create date: <08/22/2023>            
-- Description: <Description,Get all consent to care records which needs to expire,>     
---FOX_PROC_GET_CONSENT_TO_CARE_FOR_EXPIRY 1012714,525125          
-- =============================================       
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_FOR_EXPIRY     
(@PRACTICE_CODE BIGINT,    
@SENT_STATUS_ID BIGINT    
)      
AS      
BEGIN      
SELECT * FROM     
FOX_TBL_CONSENT_TO_CARE WITH(NOLOCK)     
WHERE [EXPIRY_DATE_UTC] < GETUTCDATE() and isnull(DELETED,0)=0    
and STATUS_ID = @SENT_STATUS_ID AND PRACTICE_CODE = @PRACTICE_CODE    
END 