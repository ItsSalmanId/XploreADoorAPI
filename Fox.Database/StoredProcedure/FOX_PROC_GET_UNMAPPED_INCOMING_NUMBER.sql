IF (OBJECT_ID('FOX_PROC_GET_UNMAPPED_INCOMING_NUMBER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_UNMAPPED_INCOMING_NUMBER  
GO   
-- =============================================        
-- Author:  Muhammad Arslan Tufail        
-- Create date: 07/02/2022        
-- Description: This procedure is trigger to get unmapped call recording incoming number        
-- =============================================        
-- FOX_PROC_GET_UNMAPPED_INCOMING_NUMBER '05/23/2022'        
CREATE PROCEDURE FOX_PROC_GET_UNMAPPED_INCOMING_NUMBER         
 -- Add the parameters for the stored procedure here        
 @DATE_FROM DATETIME        
AS        
BEGIN        
 SELECT INCOMING_CALL_NO FROM FOX_TBL_PHD_CALL_DETAILS          
 WHERE CONVERT(DATE, CREATED_DATE, 101) = CONVERT(DATE, @DATE_FROM, 101) AND ISNULL(CALL_RECORDING_PATH,'') = '' AND ISNULL(DELETED,0) = 0       
 AND INCOMING_CALL_NO NOT IN (SELECT INCOMING_CALL_NO FROM FOX_TBL_CRAWLER_AVAYA_RECORDING_NOT_FOUND_LOGS       
 WHERE CAST(RECORDING_FOR_DATE AS DATE) = CAST(@DATE_FROM AS DATE) AND ISNULL(DELETED,0) = 0)       
 AND INCOMING_CALL_NO NOT IN (SELECT CALL_NO FROM FOX_TBL_PHD_CALL_UNMAPPED       
 WHERE CAST(CALL_DATE AS DATE) = CAST(@DATE_FROM AS DATE) AND ISNULL(DELETED,0) = 0)         
 GROUP BY INCOMING_CALL_NO HAVING COUNT(INCOMING_CALL_NO) > 1        
END 