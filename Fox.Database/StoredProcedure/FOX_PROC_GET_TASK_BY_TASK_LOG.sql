IF (OBJECT_ID('FOX_PROC_GET_TASK_BY_TASK_LOG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_BY_TASK_LOG  
GO 
-- =============================================          
-- AUTHOR:  <DEVELOPER, SATTAR>          
-- CREATE DATE: <CREATE DATE, 07/26/2018>          
-- DESCRIPTION: <TASK LOG; LIST OF TASK LOG>          
CREATE PROCEDURE [DBO].[FOX_PROC_GET_TASK_BY_TASK_LOG] --1011163, 5483380, -1, 0          
@PRACTICE_CODE BIGINT,     
@TASK_ID       BIGINT,     
@TASK_TYPE_ID  INT,     
@IS_TEMPLATE   BIT           
--DECLARE @PRACTICE_CODE BIGINT= 1011163, @TASK_ID BIGINT= 5483380, @TASK_TYPE_ID INT= -1, @IS_TEMPLATE BIT= 0;    
AS    
     BEGIN    
         --Drop temp tables if exists    
         IF OBJECT_ID('tempdb..#NOTES') IS NOT NULL    
             DROP TABLE #NOTES;    
         IF OBJECT_ID('tempdb..#TASK_LOG') IS NOT NULL    
             DROP TABLE #TASK_LOG;    
         --    
         --    
         SELECT N.TASK_ID AS taskId,     
                'notes' AS [type],     
                NOTES AS detail,     
                N.CREATED_BY,     
                N.CREATED_DATE,     
                NULL AS TASK_LOG_ID    
         INTO #NOTES    
         FROM FOX_TBL_NOTES AS N    
         WHERE ISNULL(N.DELETED, 0) = 0    
               AND N.PRACTICE_CODE = @PRACTICE_CODE    
               AND N.TASK_ID = @TASK_ID    
               AND ISNULL(N.IS_ACTIVE, 0) = 0    
         ORDER BY CREATED_DATE DESC;    
    
         --    
         SELECT TL.TASK_ID AS taskId,     
                'log' AS [type],     
                TL.ACTION_DETAIL AS detail,     
                TL.CREATED_BY,     
                TL.CREATED_DATE,     
                TL.TASK_LOG_ID    
         INTO #TASK_LOG    
         FROM FOX_TBL_TASK AS T    
              JOIN FOX_TBL_TASK_LOG AS TL ON T.TASK_ID = TL.TASK_ID    
         WHERE ISNULL(T.DELETED, 0) = 0    
               AND T.PRACTICE_CODE = @PRACTICE_CODE    
               AND ISNULL(T.IS_TEMPLATE, 0) = @IS_TEMPLATE    
               AND (@TASK_ID = -1    
                    OR T.TASK_ID = @TASK_ID)    
               AND (@TASK_TYPE_ID = -1    
                    OR T.TASK_TYPE_ID = @TASK_TYPE_ID)    
         ORDER BY TL.TASK_LOG_ID DESC;    
    
         --    
         SELECT ROW_NUMBER() OVER(ORDER BY CREATED_DATE,     
                                           TASK_LOG_ID) AS RowNumber,     
                *    
         FROM    
         (    
             SELECT *    
             FROM #NOTES    
             UNION ALL    
             SELECT *    
             FROM #TASK_LOG    
         ) t    
         ORDER BY RowNumber DESC;    
    
         --SELECT T.TASK_ID,         
         --               T.PRACTICE_CODE,         
         --               T.PATIENT_ACCOUNT,         
         --               T.TASK_TYPE_ID,         
         --               TL.[ACTION],         
         --               TL.ACTION_DETAIL,         
         --               TL.CREATED_BY,         
         --               TL.CREATED_DATE,         
         --               TL.MODIFIED_BY,         
         --               TL.MODIFIED_DATE        
         --        FROM FOX_TBL_TASK T        
         --             JOIN FOX_TBL_TASK_LOG TL ON T.TASK_ID = TL.TASK_ID        
         --        WHERE ISNULL(T.DELETED, 0) = 0        
         --              AND ISNULL(T.IS_TEMPLATE, 0) = @IS_TEMPLATE        
         --              AND T.PRACTICE_CODE = @PRACTICE_CODE        
         --              AND (@TASK_ID = -1        
         --                   OR T.TASK_ID = @TASK_ID)        
         --              AND (@TASK_TYPE_ID = -1        
         --                   OR T.TASK_TYPE_ID = @TASK_TYPE_ID)        
         --        ORDER BY CREATED_DATE DESC;        
     END;  
