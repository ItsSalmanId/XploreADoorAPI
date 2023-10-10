
-- =============================================            
-- Author:  <Abdul Sattar>            
-- Create date: <08/22/2023>            
-- Description: <Description,Get consent status id by status name,>     
---FOX_PROC_GET_CONSENT_STATUS_ID_BY_NAME 1012714,'sent'         
-- =============================================    
            
CREATE PROCEDURE FOX_PROC_GET_CONSENT_STATUS_ID_BY_NAME            
 @PRACTICE_CODE BIGINT,          
 @NAME VARCHAR(100)          
AS            
BEGIN            
 SET NOCOUNT ON;            
 SELECT TOP 1 * FROM FOX_TBL_CONSENT_TO_CARE_STATUS WITH(NOLOCK)            
 WHERE PRACTICE_CODE = @PRACTICE_CODE            
 AND LOWER(STATUS_NAME) = LOWER(@NAME)            
 AND ISNULL(DELETED,0) = 0            
END 