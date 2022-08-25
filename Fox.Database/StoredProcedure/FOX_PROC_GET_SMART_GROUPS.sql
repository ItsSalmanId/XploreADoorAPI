IF (OBJECT_ID('FOX_PROC_GET_SMART_GROUPS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_GROUPS  
GO   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SMART_GROUPS] @PRACTICE_CODE BIGINT,   
                                             @SEARCH_TEXT   NVARCHAR(200)  
AS  
     BEGIN  
         SELECT *  
         FROM FOX_TBL_GROUP  
         WHERE GROUP_NAME LIKE @SEARCH_TEXT+'%'  
               AND PRACTICE_CODE = @PRACTICE_CODE  
               AND DELETED = 0  
               AND GROUP_ID IN  
         (  
             SELECT DISTINCT   
                    GROUP_ID  
             FROM FOX_TBL_USER_GROUP  
             WHERE DELETED = 0  
                   AND PRACTICE_CODE = @PRACTICE_CODE  
         );  
     END;  
  
  
  
  
-----------------------------------------------------------ALTER PROCEDURES----------------------------------------------------------------------  
  
  
