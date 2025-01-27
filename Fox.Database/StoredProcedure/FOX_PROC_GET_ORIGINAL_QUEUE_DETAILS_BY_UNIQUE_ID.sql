IF (OBJECT_ID('FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS_BY_UNIQUE_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS_BY_UNIQUE_ID  
GO 
-- =============================================  
-- Author:  <Author, Syed Ali Hassan and Yousaf>  
-- Create date: <Create Date, 2/22/2018>  
-- DESCRIPTION: <GET ORiginal QUEUE DETAILS BY UNIQUE_ID>  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS_BY_UNIQUE_ID] --1011163,5447838  
 @PRACTICE_CODE BIGINT  
 ,@UNIQUE_ID VARCHAR(100)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT WORK_ID  
  ,UNIQUE_ID  
  ,WORK_STATUS  
  ,TOTAL_PAGES NO_OF_PAGES  
  ,AT.LAST_NAME + ', ' + AT.FIRST_NAME AS ASSIGNED_TO  
  ,CB.LAST_NAME + ', ' + CB.FIRST_NAME AS COMPLETED_BY  
  ,FILE_PATH  
  ,wq.IS_EMERGENCY_ORDER  
 FROM FOX_TBL_WORK_QUEUE WQ  
 LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME  
 LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME  
 WHERE UNIQUE_ID LIKE @UNIQUE_ID  
  AND WQ.PRACTICE_CODE = @PRACTICE_CODE  
END  
