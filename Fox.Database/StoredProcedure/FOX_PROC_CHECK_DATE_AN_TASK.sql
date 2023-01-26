IF (OBJECT_ID('FOX_PROC_CHECK_DATE_AN_TASK') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHECK_DATE_AN_TASK  
GO 
 -- =============================================                                                
-- Author:                                                  
-- Create date: <12/14/2021>                                                
-- Description:                                                 
-- =============================================         
---* DATE: 04/011/2022--- irfan ullah ---sub task type logic  
--**************************************************************************/                                         
-- EXEC   FOX_PROC_CHECK_DATE_AN_TASK 101271460086877 , 1012714                 
alter PROCEDURE [dbo].[FOX_PROC_CHECK_DATE_AN_TASK]   
@PATIENT_ACCOUNT BIGINT,  
@PRACTICE_CODE   BIGINT,   
@SUB_TYPE   varchar(50)          
AS            
  BEGIN            
      IF Object_id('TEMPDB.DBO.#CALCULATED_DIFF', 'U') IS NOT NULL            
        DROP TABLE #calculated_diff;           
   IF Object_id('TEMPDB.DBO.#TASK_EXIST', 'U') IS NOT NULL            
        DROP TABLE #TASK_EXIST;           
            
      SELECT TOP 1 Datediff(day, CONVERT(VARCHAR, process_date, 23), CONVERT(VARCHAR, Getdate(), 23)) AS CALCULATED_DIFF,            
                   patient_account,            
                   process_date            
      INTO   #calculated_diff            
      FROM   patient_statement_history HIS   WITH (NOLOCK)          
      WHERE  HIS.patient_account = @PATIENT_ACCOUNT           
             AND submission_type = 'PAT'            
             AND Datediff(day, CONVERT(VARCHAR, process_date, 23), CONVERT(VARCHAR, Getdate(), 23)) > 45            
      ORDER  BY process_date DESC            
        
 IF EXISTS(SELECT calculated_diff   FROM   #calculated_diff  WITH (NOLOCK))            
  BEGIN            
   IF((SELECT calculated_diff FROM   #calculated_diff  WITH (NOLOCK))  > 45 )            
      BEGIN            
      IF((SELECT COUNT(*) FROM FOX_TBL_CASE cas  WITH (NOLOCK)          
       INNER JOIN  FOX_TBL_CASE_STATUS stat ON cas.CASE_STATUS_ID = stat.CASE_STATUS_ID AND NAME = 'ACT'          
       WHERE cas.PATIENT_ACCOUNt = @PATIENT_ACCOUNT) > 0)          
      BEGIN          
       SELECT TOP 1 t.task_id + '' AS TASK_ID           
       INTO   #TASK_EXIST          
         FROM   fox_tbl_task t  WITH (NOLOCK) inner join FOX_TBL_TASK_TASK_SUB_TYPE s on t.TASK_ID=s.TASK_ID           
         WHERE  task_type_id = (SELECT TOP 1 task_type_id FROM fox_tbl_task_type  WITH (NOLOCK) WHERE  practice_code = @PRACTICE_CODE AND Lower(NAME) = Lower('Billing Review - High Balance'))            
            AND TASK_SUB_TYPE_ID = (SELECT TOP 1 TASK_SUB_TYPE_ID FROM FOX_TBL_TASK_SUB_TYPE  WITH (NOLOCK) WHERE  practice_code = @PRACTICE_CODE AND Lower(NAME) = Lower(@SUB_TYPE))            
   AND patient_account = @PATIENT_ACCOUNT            
            AND is_finalroute_mark_complete <> 1            
            AND is_sendto_mark_complete <> 1            
             ORDER  BY t.created_date DESC           
         IF EXISTS(SELECT TASK_ID FROM #TASK_EXIST  WITH (NOLOCK))            
        BEGIN            
         SELECT TASK_ID  FROM   #TASK_EXIST   WITH (NOLOCK)        
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
   ELSE            
    BEGIN            
       SELECT 'date_not_exceeded' AS TASK_ID            
     END            
   END            
   ELSE            
        BEGIN            
            SELECT 'Patient has no Statement History'+ '' AS TASK_ID            
        END            
  END 