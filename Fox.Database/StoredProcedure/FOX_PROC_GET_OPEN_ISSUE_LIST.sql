IF (OBJECT_ID('FOX_PROC_GET_OPEN_ISSUE_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_OPEN_ISSUE_LIST  
GO 
-- =============================================          
-- AUTHOR:  <DEVELOPER, YOUSAF>          
-- CREATE DATE: <CREATE DATE, 10/16/2018>          
-- DESCRIPTION: <GET OPEN ISSUE; GET OPEN ISSUE LIST WITH NOTES>         
-- FOX_PROC_GET_OPEN_ISSUE_LIST 1011163, 5481020, 544105        
CREATE PROCEDURE FOX_PROC_GET_OPEN_ISSUE_LIST  
(@PRACTICE_CODE  BIGINT,   
 @CASE_ID        BIGINT,   
 @CASE_STATUS_ID BIGINT  
)  
AS  
     BEGIN  
  
IF @CASE_ID = 0  
 BEGIN  
  SET @CASE_ID = NULL  
 END  
  
         SELECT TT.TASK_TYPE_ID,   
                TT.NAME AS TaskTypeName,   
                N.NOTES  
         FROM FOX_TBL_TASK_TYPE TT  
              LEFT JOIN FOX_TBL_TASK T ON TT.TASK_TYPE_ID = T.TASK_TYPE_ID  
                                          AND T.CASE_ID = @CASE_ID  
                                          AND ISNULL(T.DELETED, 0) = 0  
              LEFT JOIN FOX_TBL_NOTES N ON T.TASK_ID = N.TASK_ID  
                                           AND ISNULL(N.DELETED, 0) = 0  
             AND ISNULL(N.IS_ACTIVE, 0) = 1  
         WHERE ISNULL(TT.DELETED, 0) = 0  
               AND TT.PRACTICE_CODE = @PRACTICE_CODE  
               AND TT.CASE_STATUS_ID = @CASE_STATUS_ID  
  ORDER BY TT.TASK_TYPE_ID  
     END;  
  
