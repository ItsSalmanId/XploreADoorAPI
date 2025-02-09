IF (OBJECT_ID('FOX_TBL_GET_PREVIOUS_EMAILS') IS NOT NULL ) DROP PROCEDURE FOX_TBL_GET_PREVIOUS_EMAILS  
GO 
-- FOX_TBL_GET_PREVIOUS_EMAILS 99921243, 1012714  
CREATE procedure FOX_TBL_GET_PREVIOUS_EMAILS   
@WORK_ID BIGINT,  
@PRACTICE_CODE BIGINT  
  
AS   
  
BEGIN  
 SELECT   
 CREATED_DATE,  
 [TO] AS SEND_TO,  
 CREATED_BY,  
 CASE  
  WHEN WORK_STATUS LIKE  'CREATED'  
  THEN 'On RFO Creation'  
  ELSE 'Re-sent'  
 END AS [DESCRIPTION]  
  FROM FOX_TBL_EMAIL_FAX_LOG WHERE WORK_ID = @WORK_ID  
 AND STATUS like 'Success'  
 AND PRACTICE_CODE = @PRACTICE_CODE  
 AND ATTACHMENT_PATH IS NOT NULL  
 --AND WORK_STATUS NOT LIKE 'Completed'  
 order by Created_Date DESC  
END