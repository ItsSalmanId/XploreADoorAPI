IF (OBJECT_ID('FOX_PROC_GET_RECORD_BY_CALLING_NUMBER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECORD_BY_CALLING_NUMBER  
GO 
-- =============================================      
-- Author:  <Author,Syed Asim Shah>      
-- Create date: <Create Date,05/17/2019>      
-- DESCRIPTION: <UPDATE FOX PHD RECORDING PATH FROM SERVICE>      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_RECORD_BY_CALLING_NUMBER](@Phone_Number VARCHAR(10), @PRACTICE_CODE BIGINT)  
AS  
     BEGIN  
         SET NOCOUNT ON;  
         SELECT TOP 1 FOX_PHD_CALL_DETAILS_ID  
         FROM FOX_TBL_PHD_CALL_DETAILS  
         WHERE INCOMING_CALL_NO = @Phone_Number and PRACTICE_CODE = @PRACTICE_CODE  
               AND ISNULL(CALL_RECORDING_PATH, '') = ''  
               AND DELETED = 0  
      AND CAST(CALL_DATE AS DATE) = CAST(GETDATE() AS DATE)  
      -- CONVERT(DATE, GETDATE()) AND CONVERT(DATE, GETDATE())  
      -- >= '07/08/2019' and CAST(CALL_DATE AS DATE) <= '07/14/2019'  
        
         --ORDER BY CALL_DATE;  
     END;  
  
