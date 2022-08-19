IF (OBJECT_ID('FOX_PROC_GET_NOTIFICATIONS_OF_TASKS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_NOTIFICATIONS_OF_TASKS  
GO       
create PROCEDURE [dbo].[FOX_PROC_GET_NOTIFICATIONS_OF_TASKS]            
@USER_ID BIGINT,            
@PRACTICE_CODE BIGINT,                
@DATE_FROM       DATETIME,                                                                   
@DATE_TO         DATETIME           
AS                              
BEGIN                              
select         
  (DATENAME(DW,SENT_ON)+ ', ' + CAST(DATEPART(d, SENT_ON) AS VARCHAR)+' '+ convert(char(3), SENT_ON, 0)+' '+ CAST(year(SENT_ON) AS VARCHAR)) AS SENT_ON_STR, *         
from Fox_Tbl_Notifications        
        
WHERE SENT_TO_USER = @USER_ID           
AND (@DATE_FROM IS NULL                                        
         OR @DATE_TO IS NULL                                                                  
         OR CONVERT(DATE, SENT_ON) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))        
--AND task.TASK_ID IS NOT NULL          
AND ISNULL(DELETED, 0) = 0          
ORDER BY SENT_ON DESC          
END; 