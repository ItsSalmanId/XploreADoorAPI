IF (OBJECT_ID('GET_OPEN_ISSUE_LIST') IS NOT NULL ) DROP PROCEDURE GET_OPEN_ISSUE_LIST  
GO 
CREATE PROCEDURE [dbo].[GET_OPEN_ISSUE_LIST] --1011163, 548352, 544105        
 @PRACTICE_CODE BIGINT    
 ,@CASE_ID BIGINT    
 ,@CASE_STATUS_ID BIGINT    
 --DECLARE @PRACTICE_CODE BIGINT = 1011163        
 -- ,@CASE_ID BIGINT = 544100        
 -- ,@CASE_STATUS_ID INT = 544105        
AS    
BEGIN    
 SELECT isnull(TEMP.TASK_ID, 0) TASK_ID    
  ,TT.TASK_TYPE_ID    
  ,TT.NAME AS TASK_TYPE    
  ,TST.TASK_SUB_TYPE_ID    
  ,TST.NAME AS TASK_SUB_TYPE   
  ,TST.RT_CODE AS RT_CODE  
  ,CASE     
   WHEN TEMP.TASK_SUB_TYPE_ID IS NULL    
    THEN CONVERT(BIT, 0)    
   ELSE CONVERT(BIT, 1)    
   END AS IS_CHECKED    
 FROM FOX_TBL_TASK_TYPE AS TT    
 INNER JOIN FOX_TBL_TASK_SUB_TYPE AS TST ON TST.TASK_TYPE_ID = TT.TASK_TYPE_ID    
  AND ISNULL(TST.DELETED, 0) = 0    
  AND TST.PRACTICE_CODE = @PRACTICE_CODE    
 LEFT JOIN (    
  SELECT c.CASE_ID    
   ,t.TASK_ID    
   ,TTST.TASK_SUB_TYPE_ID    
  FROM FOX_TBL_CASE AS c    
  INNER JOIN FOX_TBL_TASK AS T ON T.CASE_ID = c.CASE_ID    
   AND ISNULL(T.DELETED, 0) = 0    
   AND T.PRACTICE_CODE = @PRACTICE_CODE    
  INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS TTST ON TTST.TASK_ID = T.TASK_ID    
   AND ISNULL(TTST.DELETED, 0) = 0    
   AND TTST.PRACTICE_CODE = @PRACTICE_CODE    
  WHERE c.CASE_ID = @CASE_ID    
   AND ISNULL(c.DELETED, 0) = 0    
   AND c.PRACTICE_CODE = @PRACTICE_CODE    
  ) AS TEMP ON TEMP.TASK_SUB_TYPE_ID = TST.TASK_SUB_TYPE_ID    
 WHERE TT.CASE_STATUS_ID = @CASE_STATUS_ID    
  AND ISNULL(TT.DELETED, 0) = 0    
  AND TT.PRACTICE_CODE = @PRACTICE_CODE    
END 