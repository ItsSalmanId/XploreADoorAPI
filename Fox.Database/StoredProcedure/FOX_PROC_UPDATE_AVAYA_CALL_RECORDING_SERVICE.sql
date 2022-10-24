IF (OBJECT_ID('FOX_PROC_UPDATE_AVAYA_CALL_RECORDING_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_AVAYA_CALL_RECORDING_SERVICE  
GO   
-- =============================================      
-- Author:  Muhammad Arslan Tufail      
-- Create date: 06/23/2022      
-- Description: This SP trigger to Update Call Recording      
-- =============================================      
-- [dbo].[FOX_PROC_UPDATE_AVAYA_CALL_RECORDING_SERVICE] '','','',''      
CREATE PROCEDURE FOX_PROC_UPDATE_AVAYA_CALL_RECORDING_SERVICE       
@CALL_RECORDING_NAME VARCHAR(500),      
@INCOMING_CALL_NO VARCHAR(30),      
@DATE_FROM DATETIME,      
@DATE_TO DATETIME      
      
AS      
BEGIN      
   UPDATE FOX_TBL_PHD_CALL_DETAILS      
   SET CALL_RECORDING_PATH = @CALL_RECORDING_NAME      
   WHERE INCOMING_CALL_NO = @INCOMING_CALL_NO       
   AND CONVERT(DATE, CREATED_DATE, 101) BETWEEN CONVERT(DATE, @DATE_FROM, 101) AND CONVERT(DATE, @DATE_TO, 101)       
END   