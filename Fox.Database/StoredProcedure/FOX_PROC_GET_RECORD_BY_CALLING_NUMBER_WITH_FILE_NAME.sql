IF (OBJECT_ID('FOX_PROC_GET_RECORD_BY_CALLING_NUMBER_WITH_FILE_NAME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECORD_BY_CALLING_NUMBER_WITH_FILE_NAME  
GO  
-- =============================================    
-- Author:  <Author,Abdur Rafey>    
-- Create date: <Create Date,06/04/2019>    
-- DESCRIPTION: <GET FOX PHD RECORDING PATH FROM SERVICE>    

--   [dbo].[FOX_PROC_GET_RECORD_BY_CALLING_NUMBER_WITH_FILE_NAME]'7878978945', 1011163

CREATE PROCEDURE [dbo].[FOX_PROC_GET_RECORD_BY_CALLING_NUMBER_WITH_FILE_NAME]
(@Phone_Number VARCHAR(10), 
@PRACTICE_CODE BIGINT)
AS
     BEGIN
         SET NOCOUNT ON;
         SELECT TOP 1 FOX_PHD_CALL_DETAILS_ID,CALL_RECORDING_PATH,CREATED_BY
         FROM FOX_TBL_PHD_CALL_DETAILS
         WHERE INCOMING_CALL_NO = @Phone_Number
			   AND PRACTICE_CODE = @PRACTICE_CODE
               AND CALL_RECORDING_PATH <> ''
               AND ISNULL(DELETED, 0) = 0
			   --AND CAST(CALL_DATE AS DATE) = CAST(GETDATE() AS DATE)
			   --AND CAST(CALL_DATE AS DATE) >= '07/08/2019' and CAST(CALL_DATE AS DATE) <= '07/14/2019'
			   --AND CONVERT(DATE, CREATED_DATE) BETWEEN CONVERT(DATE, GETDATE()) AND CONVERT(DATE, GETDATE())
         ORDER BY CREATED_DATE DESC;
     END;