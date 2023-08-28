IF (OBJECT_ID('FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS  
GO  
        
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------          
---------------------------------------------------------------------------------------------------------------------        
-- =============================================        
-- Author:  <Author,Mehmood ul Hassan>        
-- Create date: <Create Date,12/10/2017>        
-- DESCRIPTION: <GET ORiginal QUEUE>        
 --EXEC FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS 1012714,53432809        
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS] --1011163,53434417        
 (        
 @PRACTICE_CODE BIGINT        
 ,@WORK_ID BIGINT        
 )        
AS        
BEGIN        
 SET NOCOUNT ON;        
        
 SELECT WORK_ID        
  ,UNIQUE_ID,       
CASE                  
    WHEN IS_TRASH_REFERRAL = 1 AND WORK_STATUS != 'Completed' THEN 'Index Pending'  
    WHEN WORK_STATUS = 'Completed' THEN 'Indexed'  
    ELSE WORK_STATUS        
    END AS WORK_STATUS,  
  TOTAL_PAGES NO_OF_PAGES        
  ,AT.LAST_NAME + ', ' + AT.FIRST_NAME AS ASSIGNED_TO        
  ,wq.IS_EMERGENCY_ORDER        
  ,CB.LAST_NAME + ', ' + CB.FIRST_NAME AS COMPLETED_BY        
  ,FILE_PATH        
 FROM FOX_TBL_WORK_QUEUE WQ        
 LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME        
 LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME        
 WHERE (        
   UNIQUE_ID LIKE CONVERT(VARCHAR, @WORK_ID) + '_%'        
   OR WORK_ID = @WORK_ID        
   )        
  AND WQ.PRACTICE_CODE = @PRACTICE_CODE        
END 