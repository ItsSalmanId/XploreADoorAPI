IF (OBJECT_ID('FOX_PROC_GET_RECONCILIATION_UPLOAD_LOG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECONCILIATION_UPLOAD_LOG  
GO 
CREATE PROCEDURE FOX_PROC_GET_RECONCILIATION_UPLOAD_LOG        
 @PRACTICE_CODE BIGINT         
AS        
BEGIN        
 SELECT TOP 1 * FROM  FOX_TBL_RECONCILIATION_UPLOAD_LOG  WHERE PRACTICE_CODE = @PRACTICE_CODE ORDER BY LAST_UPDATED_DATE DESC         
        
END