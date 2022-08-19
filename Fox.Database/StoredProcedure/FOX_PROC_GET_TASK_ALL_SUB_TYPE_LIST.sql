IF (OBJECT_ID('FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST  
GO   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST] --1011163, 5481619, 544100      
@PRACTICE_CODE BIGINT,   
@TASK_ID       BIGINT = NULL,   
@TASK_TYPE_ID  INT      
--DECLARE @PRACTICE_CODE BIGINT= 1011163,       
--  @TASK_ID BIGINT= 5481619,       
--  @TASK_TYPE_ID INT= 544100;      
AS  
     BEGIN      
         --      
         IF OBJECT_ID('tempdb..#TEMP_FOX_TBL_TASK') IS NOT NULL  
             DROP TABLE #TEMP_FOX_TBL_TASK;  
         IF OBJECT_ID('tempdb..#TEMP_FOX_TBL_TASK_TYPE') IS NOT NULL  
             DROP TABLE #TEMP_FOX_TBL_TASK_TYPE;      
         --      
         CREATE TABLE #TEMP_FOX_TBL_TASK  
         (TASK_ID               BIGINT,   
          TASK_SUB_TYPE_ID      INT,   
          TASK_TASK_SUB_TYPE_ID BIGINT  
         );      
         --      
         IF ISNULL(@TASK_ID, 0) <> 0  
             BEGIN  
                 INSERT INTO #TEMP_FOX_TBL_TASK  
                        SELECT DISTINCT   
                               t.TASK_ID AS TASK_ID,   
                               ttst.TASK_SUB_TYPE_ID,   
                               ttst.TASK_TASK_SUB_TYPE_ID  
                        FROM FOX_TBL_TASK AS t  
                             INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS ttst ON ttst.TASK_ID = t.TASK_ID  
                                                                              AND ttst.DELETED <> 1  
                                                                              AND ttst.PRACTICE_CODE = @PRACTICE_CODE  
                        WHERE t.DELETED <> 1  
                              AND t.PRACTICE_CODE = @PRACTICE_CODE  
                              AND t.TASK_ID = @TASK_ID;  
             END;      
         --      
         SELECT DISTINCT   
                tst.NAME AS TASK_SUB_TYPE,   
                tst.TASK_TYPE_ID AS TASK_TYPE_ID,   
                tt.NAME AS TASK_TYPE,   
                tst.TASK_SUB_TYPE_ID AS TASK_SUB_TYPE_ID  
         INTO #TEMP_FOX_TBL_TASK_TYPE  
         FROM FOX_TBL_TASK_TYPE AS tt  
              INNER JOIN FOX_TBL_TASK_SUB_TYPE AS tst ON tst.TASK_TYPE_ID = tt.TASK_TYPE_ID  
                                                         AND tst.DELETED <> 1  
                                                         AND tst.PRACTICE_CODE = @PRACTICE_CODE  
         WHERE tt.DELETED <> 1  
               AND tt.PRACTICE_CODE = @PRACTICE_CODE  
               AND tt.TASK_TYPE_ID = @TASK_TYPE_ID;      
         --      
         SELECT b.TASK_SUB_TYPE,   
                a.TASK_ID,   
                b.TASK_TYPE_ID,   
                b.TASK_TYPE,   
                b.TASK_SUB_TYPE_ID,  
                CASE  
                    WHEN a.TASK_TASK_SUB_TYPE_ID IS NULL  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS IS_CHECKED  
         FROM #TEMP_FOX_TBL_TASK_TYPE AS b  
              LEFT JOIN #TEMP_FOX_TBL_TASK AS a ON a.TASK_SUB_TYPE_ID = b.TASK_SUB_TYPE_ID;  
     END;  
