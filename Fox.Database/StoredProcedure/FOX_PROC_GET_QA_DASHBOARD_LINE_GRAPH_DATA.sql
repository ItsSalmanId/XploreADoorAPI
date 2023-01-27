-- =============================================                                                
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                                                
-- CREATE DATE: <CREATE DATE, 22/09/2022>                                                
-- DESCRIPTION:  THIS PROCEDURE IS GET QA DASHBOARD LINE GRAPH DATA                                            
-- =============================================                          
--  FOX_PROC_GET_QA_DASHBOARD_LINE_GRAPH_DATA 1012714,'54410118,605123','Survey', null,'Both' ,'10/13/2022 12:00:00 AM' , '10/29/2022 11:59:00 AM'              
CREATE PROCEDURE FOX_PROC_GET_QA_DASHBOARD_LINE_GRAPH_DATA                                
  @PRACTICE_CODE BIGINT,                                                      
  @CALL_SCANRIO_ID VARCHAR(200),                                      
  @CALL_TYPE VARCHAR(20),                                     
  @AGENT_NAME VARCHAR(700),                      
  @CRITERIA_NAME VARCHAR(50),                                    
  @START_DATE DATETIME,                                  
  @END_DATE DATETIME                                       
AS                      
--DECLARE     
  --@PRACTICE_CODE BIGINT = 1012714,                                                      
  --@CALL_SCANRIO_ID VARCHAR(200)='605122,605121,605128,605129,605127,605120,605130,605113,605123,54410118',                                      
  --@CALL_TYPE VARCHAR(20)='Survey',                                     
  --@AGENT_NAME VARCHAR(700)=NULL,                      
  --@CRITERIA_NAME VARCHAR(50)='Both',                                    
  --@START_DATE DATETIME='12/01/2022 12:00:00 AM',                                  
  --@END_DATE DATETIME='1/14/2023 12:00:00 AM'    
BEGIN                       
                
IF OBJECT_ID('TEMPDB..#tempdatasurvey') IS NOT NULL DROP TABLE #tempdatasurvey                
DECLARE @d1 DATETIME, @d2 DATETIME, @years bigint, @yeare bigint,@PERCENTAGE_VALUE INT, @PERCENTAGE_VALUE1 INT                   
  IF(@CRITERIA_NAME = 'Both')                      
  BEGIN                      
    SELECT @PERCENTAGE_VALUE = 100                    
  END  ELSE                
   SELECT @PERCENTAGE_VALUE = (SELECT PERCENTAGE AS  PERCENTAGE_VALUE                        
  FROM FOX_TBL_EVALUATION_CRITERIA With (nolock)        
  WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0)=0 AND CALL_TYPE = @CALL_TYPE AND CRITERIA_NAME =@CRITERIA_NAME)                            
SELECT @d1 = @START_DATE ,@d2= @END_DATE, @Years = YEAR(CAST(@d1 AS DATE)), @Yeare = Year(CAST(@d2 AS DATE))              
;WITH dates ( [date] )                
AS                
(                
SELECT @d1                
UNION all                
SELECT dateadd(d,1,date)                
FROM dates                
where date < @d2                
)                       
select  DISTINCT DATEPART(week,[date]) AS WEEK_NUMBER,    
case    
when year(@START_DATE) < year(@END_DATE) and DATEPART(week,[date]) < 12    
then    
convert(datetime,isnull(M.WeekStart,DATEADD(wk,DATEDIFF(wk, 5,CAST(RTRIM(@yeare * 10000 + 1 * 100 + 1) AS DATETIME)) + (DATEPART(week,[date]) - 1 ), 0)),10)     
else    
   convert(datetime, isnull(M.WeekStart,DATEADD(wk,DATEDIFF(wk, 5,CAST(RTRIM(@years * 10000 + 1 * 100 + 1) AS DATETIME)) + (DATEPART(week,[date]) - 1 ), 0)),10)    
END AS WEEKSTART,      
case    
when year(@START_DATE) < year(@END_DATE) and DATEPART(week,[date]) < 12    
then    
   convert(varchar(50), isnull(M.WeekStart,DATEADD(wk,DATEDIFF(wk, 5,CAST(RTRIM(@yeare * 10000 + 1 * 100 + 1) AS DATETIME)) + (DATEPART(week,[date]) - 1 ), 0)),10) + '  to  '                
   +convert(varchar(50), isnull(m.WeekEnd,DATEADD(second, -1,DATEADD(day,DATEDIFF(day, 0,DATEADD(wk,DATEDIFF(wk, 5,CAST(RTRIM(@yeare * 10000+ 1 * 100 + 1) AS DATETIME)) +     
   (DATEPART(week,[date]) + -1 ), 5)) + 1, 1))),10)    
   else    
   convert(varchar(50), isnull(M.WeekStart,DATEADD(wk,DATEDIFF(wk, 5,CAST(RTRIM(@years * 10000 + 1 * 100 + 1) AS DATETIME)) + (DATEPART(week,[date]) - 1 ), 0)),10) + '  to  '                
   +convert(varchar(50), isnull(m.WeekEnd,DATEADD(second, -1,DATEADD(day,DATEDIFF(day, 0,DATEADD(wk,DATEDIFF(wk, 5,CAST(RTRIM(@years * 10000+ 1 * 100 + 1) AS DATETIME)) +     
   (DATEPART(week,[date]) + -1 ), 5)) + 1, 1))),10)     
   end as DATE_RANGE                
       , ISNULL(M.perc,0) AS EVALUATION_PERCENTAGE                     
    ,M.NAME as TEAM_NAME                
    ,M.AGENT_NAME                 
    INTO #tempdatasurvey                
from dates                 
LEFT OUTER JOIN(                
SELECT                
  MIN(DATEPART(WEEK, ps.MODIFIED_DATE)-1) AS Week,                
  MIN(DATEADD(wk,(DATEPART(WEEK, ps.MODIFIED_DATE)-1)-1,DATEADD(yy, 100 + cast(FORMAT(cast(ps.MODIFIED_DATE as date), 'yy') as int) ,0))+2) AS WeekStart,                
  MIN(DATEADD(wk,(DATEPART(WEEK, ps.MODIFIED_DATE)-1),DATEADD(yy, 100 + cast(FORMAT(cast(ps.MODIFIED_DATE as date), 'yy') as int) ,0))+1) AS WeekEnd,                       
   CASE WHEN  @CRITERIA_NAME = 'Both' THEN cast(((sum(TOTAL_POINTS)*100)/(count(TOTAL_POINTS)* @PERCENTAGE_VALUE)) as bigint)                 
   WHEN  @CRITERIA_NAME = 'System Product and Process' THEN cast(((sum(SYSTEM_PROCESS_TOTAL)*100)/(count(SYSTEM_PROCESS_TOTAL)* @PERCENTAGE_VALUE)) as bigint)                 
   ELSE cast(((sum(CLIENT_EXPERIENCE_TOTAL)*100)/(count(CLIENT_EXPERIENCE_TOTAL)* @PERCENTAGE_VALUE)) as bigint) END AS perc,                
  cs.NAME,                
  min(au.FIRST_NAME + ' ' + au.last_name) as AGENT_NAME,                
  ec.PERCENTAGE,                
  DATEPART(week,ps.MODIFIED_DATE) AS WEEKNUMBER                
FROM FOX_TBL_SURVEY_AUDIT_SCORES ps With (nolock)        
left join FOX_TBL_PHD_CALL_SCENARIO cs  With (nolock)  on cs.PHD_CALL_SCENARIO_ID = ps.PHD_CALL_SCENARIO_ID              
left join fox_tbl_application_user au  With (nolock)  on au.user_name = ps.AGENT_NAME    left join FOX_TBL_EVALUATION_CRITERIA ec on ec.CALL_TYPE = ps.CALL_TYPE                
WHERE                
  ( (@CALL_TYPE  = 'phd'  and (@CALL_SCANRIO_ID IS NULL OR cs.PHD_CALL_SCENARIO_ID IN (SELECT VAL FROM F_SPLIT(@CALL_SCANRIO_ID, ',')))) or ( @CALL_TYPE = 'survey' and cs.PHD_CALL_SCENARIO_ID is null) or              
  (@CALL_TYPE  = 'Phd and Survey'  and (cs.PHD_CALL_SCENARIO_ID IS NULL OR cs.PHD_CALL_SCENARIO_ID IN (SELECT VAL FROM F_SPLIT(@CALL_SCANRIO_ID, ',')))))                   
 AND (@AGENT_NAME IS NULL OR AGENT_NAME IN (SELECT VAL FROM DBO.F_SPLIT(@AGENT_NAME, ',')))                
 and (( @CALL_TYPE= 'phd' and ps.CALL_TYPE = @CALL_TYPE) or ( @CALL_TYPE= 'Survey' and ps.CALL_TYPE = @CALL_TYPE) or ( @CALL_TYPE= 'Phd and Survey' and ps.CALL_TYPE in('phd','survey')))                   
  and  CONVERT(DATE,ps.MODIFIED_DATE) between CONVERT(DATE, @START_DATE) AND CONVERT(DATE, @END_DATE)             
  and ps.PRACTICE_CODE = @PRACTICE_CODE          
  and ISNULL(ps.DELETED , 0) = 0          
GROUP BY DATEPART(week, ps.MODIFIED_DATE),cs.NAME,ec.PERCENTAGE                       
)AS M  ON M.WEEKNUMBER=DATEPART(week,[date])                 
select * from #tempdatasurvey order by WEEKSTART asc              
END   