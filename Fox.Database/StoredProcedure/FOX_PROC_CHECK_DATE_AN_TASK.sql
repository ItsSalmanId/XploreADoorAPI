IF (OBJECT_ID('FOX_PROC_CHECK_DATE_AN_TASK') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHECK_DATE_AN_TASK  
GO 
-- =============================================                                              
-- Author:                                                
-- Create date: <12/14/2021>                                              
-- Description:                                               
-- =============================================                                              
-- EXEC   FOX_PROC_CHECK_DATE_AN_TASK 1012714600133324 , 1012714               
CREATE PROCEDURE FOX_PROC_CHECK_DATE_AN_TASK @PATIENT_ACCOUNT BIGINT,          
                                             @PRACTICE_CODE   BIGINT          
AS          
  BEGIN          
              
   IF Object_id('TEMPDB.DBO.#TASK_EXIST', 'U') IS NOT NULL          
        DROP TABLE #TASK_EXIST;         
                 
IF((SELECT COUNT(*) FROM FOX_TBL_CASE cas   
INNER JOIN  FOX_TBL_CASE_STATUS stat ON cas.CASE_STATUS_ID = stat.CASE_STATUS_ID AND NAME = 'ACT'   
WHERE cas.PATIENT_ACCOUNt = @PATIENT_ACCOUNT) > 0)        
BEGIN        
  
SELECT TOP 1 task_id + '' AS TASK_ID         
INTO #TASK_EXIST        
FROM fox_tbl_task          
WHERE  task_type_id = (SELECT TOP 1 task_type_id FROM fox_tbl_task_type WHERE  practice_code = @PRACTICE_CODE   
AND Lower(NAME) = Lower('billing review required'))          
AND patient_account = @PATIENT_ACCOUNT          
AND is_finalroute_mark_complete <> 1          
AND is_sendto_mark_complete <> 1          
ORDER  BY created_date DESC         
  
IF EXISTS(SELECT TASK_ID FROM #TASK_EXIST)          
BEGIN          
SELECT TASK_ID  FROM   #TASK_EXIST        
END        
ELSE          
BEGIN          
  
SELECT 'task_not_present' AS TASK_ID          
  
END        
  
END        
  
ELSE          
BEGIN          
  
SELECT 'case_not_present' AS TASK_ID          
END            
        
END    