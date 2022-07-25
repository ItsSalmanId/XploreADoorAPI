IF (OBJECT_ID('FOX_PROC_GET_TASK_SUB_TYPE_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_SUB_TYPE_LIST  
GO 
-- =============================================  
-- AUTHOR:  <DEVELOPER, YOUSAF>  
-- CREATE DATE: <CREATE DATE, 05/29/2018>  
-- DESCRIPTION: <GET TASK SUB TYPE LIST>  
  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_TASK_SUB_TYPE_LIST] --1011163, 54425693  
 (  
 @PRACTICE_CODE BIGINT  
 ,@TASK_ID BIGINT  
 )  
AS  
BEGIN  
 SELECT TST.*  
 FROM FOX_TBL_TASK_SUB_TYPE AS TST  
 INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS TTST ON TTST.TASK_SUB_TYPE_ID = TST.TASK_SUB_TYPE_ID  
  AND TTST.TASK_ID = @TASK_ID  
  AND ISNULL(TTST.DELETED, 0) = 0  
  AND TTST.PRACTICE_CODE = @PRACTICE_CODE  
 WHERE ISNULL(TST.DELETED, 0) = 0  
  AND TST.PRACTICE_CODE = @PRACTICE_CODE  
END  
  
