IF (OBJECT_ID('FOX_PROC_GET_NOTIFICATIONS_OF_TASKS_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_NOTIFICATIONS_OF_TASKS_LIST  
GO  
-- =============================================                          
-- Author:  <Author,Abdur Rafey>                          
-- Create date: <Create Date,04/21/2021>                          
-- DESCRIPTION: <GET NOTIFICATIONS OF TASKS>             
-- EXEC FOX_PROC_GET_NOTIFICATIONS_OF_TASKS_LIST   5651352, 1011163, '07/19/2020', '07/26/2021'             
CREATE PROCEDURE [dbo].[FOX_PROC_GET_NOTIFICATIONS_OF_TASKS_LIST]          
@USER_ID BIGINT,          
@PRACTICE_CODE BIGINT,              
@DATE_FROM       DATETIME,                                                                 
@DATE_TO         DATETIME         
AS                            
BEGIN                            
select --Convert(varchar, SENT_ON ,101) AS SENT_ON_STR ,      
 (DATENAME(DW,SENT_ON)+ ', ' + CAST(DATEPART(d, SENT_ON) AS VARCHAR)+' '+ convert(char(3), SENT_ON, 0)+' '+ CAST(year(SENT_ON) AS VARCHAR)) AS SENT_ON_STR      
from Fox_Tbl_Notifications      
      
WHERE SENT_TO_USER = @USER_ID         
AND (@DATE_FROM IS NULL                                      
         OR @DATE_TO IS NULL                                                                
         OR CONVERT(DATE, SENT_ON) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))      
--AND task.TASK_ID IS NOT NULL        
AND ISNULL(DELETED, 0) = 0        
GROUP BY Convert(varchar, SENT_ON ,101), (DATENAME(DW,SENT_ON)+ ', ' + CAST(DATEPART(d, SENT_ON) AS VARCHAR)+' '+ convert(char(3), SENT_ON, 0)+' '+ CAST(year(SENT_ON) AS VARCHAR))      
ORDER BY Convert(varchar, SENT_ON ,101) DESC       
END;   
