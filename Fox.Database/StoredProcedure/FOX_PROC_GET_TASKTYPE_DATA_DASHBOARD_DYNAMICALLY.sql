IF (OBJECT_ID('FOX_PROC_GET_TASKTYPE_DATA_DASHBOARD_DYNAMICALLY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASKTYPE_DATA_DASHBOARD_DYNAMICALLY  
GO     
CREATE proc FOX_PROC_GET_TASKTYPE_DATA_DASHBOARD_DYNAMICALLY                   
                         
(                        
@PRACTICE_CODE   BIGINT,                              
@GROUP_IDs VARCHAR(8000),                            
@TASK_TYPE_IDs  VARCHAR(8000),                              
@DATE_FROM DATETIME,                       
@DATE_TO DATETIME,                            
@TIME_FRAME VARCHAR(50)                         
)              
With Recompile                    
AS                        
                      
------DECLARE @PRACTICE_CODE   BIGINT = 1012714,                              
------@GROUP_IDs VARCHAR(8000) = 'ALL',                            
------@TASK_TYPE_IDs  VARCHAR(8000) = 'ALL',                              
------@DATE_FROM DATETIME = null,                        
------@DATE_TO DATETIME = null,                            
------@TIME_FRAME VARCHAR(50)   =  'LAST_THREE_MONTHS'                 
          
          
IF (@GROUP_IDs = 'ALL')                        
BEGIN                        
SET @GROUP_IDs = NULL                        
END                        
ELSE                        
BEGIN                        
SET @GROUP_IDs = @GROUP_IDs                        
END                        
IF (@TASK_TYPE_IDs = 'ALL')                        
BEGIN                        
SET @TASK_TYPE_IDs = NULL                        
END                        
ELSE                        
BEGIN                        
SET @TASK_TYPE_IDs = @TASK_TYPE_IDs                        
END                        
                        
                           
                        
   IF(@TIME_FRAME = 'TODAY')                          
             BEGIN                                 
                 --This Month                                 
                 SET @DATE_FROM =  convert(date,GETDATE());                        
     set @DATE_TO =  CONVERT(DATE, GETDATE());                        
             END;                          
             ELSE                          
         IF(@TIME_FRAME = 'YESTERDAY')                          
             BEGIN                                             
                 SET @DATE_FROM = CONVERT(DATE, DATEADD(DAY,-1,GETDATE()));                        
                 set @DATE_TO =  CONVERT(DATE, DATEADD(DAY,-1,GETDATE()));                        
             END;                          
             ELSE                          
         IF(@TIME_FRAME = 'THIS_WEEK')                          
             BEGIN                                 
                 --Last 14 days                                 
                 --SET @DATE_FROM = select CONVERT(DATE,dateadd(day,1-datepart(dw, getdate()), getdate()));                      
     SET @DATE_FROM =  CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate()));                               
     set @DATE_TO =  CONVERT(DATE, GETDATE());                        
             END;                          
             ELSE                          
         IF(@TIME_FRAME = 'THIS_MONTH')                          
             BEGIN                                 
                 --Last 30 days                                 
                 SET @DATE_FROM =  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0));                          
     set @DATE_TO =  CONVERT(DATE, DATEADD(DAY,0,GETDATE()));                        
             END;                          
             ELSE                        
     IF(@TIME_FRAME = 'LAST_MONTH')                          
             BEGIN                                 
                 --Last 30 days                                 
                 SET @DATE_FROM =  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), 0));                         
     set @DATE_TO =  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MON
TH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );                        
             END;                        
             ELSE                        
    IF(@TIME_FRAME = 'LAST_THREE_MONTHS')                          
     BEGIN                                 
                 --Last 30 days                                 
                 SET @DATE_FROM =  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE())-2, 0)) )), 0));                          
     set @DATE_TO = DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );                        
             END;                          
             ELSE                        
 IF(@TIME_FRAME = 'DATE_RANGE')                          
             BEGIN                   
                       
                 SET @DATE_FROM = CONVERT(DATE, @DATE_FROM);                            
                 --SET @DATE_TO = DATEADD(DAY, 1, @DATE_TO);                    
     SET @DATE_TO = DATEADD(DAY, 0, @DATE_TO);                            
             END;                          
                        
   SET NOCOUNT ON;           
         --                        
   IF OBJECT_ID('tempdb..#maintable', 'U') IS NOT NULL DROP TABLE #maintable;                         
      IF OBJECT_ID('TEMPDB.DBO.#TASKDATABYTYPE6', 'U') IS NOT NULL DROP TABLE #TASKDATABYTYPE6;                        
         IF OBJECT_ID('TEMPDB.DBO.#TASKDATABYTYPE7', 'U') IS NOT NULL DROP TABLE #TASKDATABYTYPE7;                        
         CREATE TABLE #maintable (dates datetime);           
         DECLARE @currentdate datetime;                                 
         DECLARE date_series_CURSOR CURSOR                              
         FOR SELECT DATEADD(day, number - 1, @DATE_FROM) AS [Date_series]                        
FROM (                        
    SELECT ROW_NUMBER() OVER (                        
        ORDER BY n.object_id                        
    )                        
    FROM sys.all_objects n                        
    ) S(number)                        
WHERE number <= DATEDIFF(day, @DATE_FROM, @DATE_TO) + 1;                            
                             
SELECT                             
tt.NAME AS TASK_TYPE_NAME                        
,COUNT(T.TASK_TYPE_ID) AS TASK_COUNT,                        
T.CREATED_DATE                        
into #TASKDATABYTYPE6                        
FROM FOX_TBL_TASK AS T                  
                  
                            
LEFT JOIN FOX_TBL_GROUP GP ON T.SEND_TO_ID = GP.GROUP_ID                            
LEFT JOIN FOX_TBL_TASK_TYPE tt on t.TASK_TYPE_ID = tt.TASK_TYPE_ID                        
         --and isnull(tt.DELETED,0)=0                        
         --and isnull(tt.IS_ACTIVE,1)=1                        
WHERE ISNULL(T.DELETED,0)=0                        
AND T.PRACTICE_CODE = @PRACTICE_CODE                        
AND(@TASK_TYPE_IDs IS NULL OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                               
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_GROUP WHERE GROUP_ID IN (select val from dbo.f_split(@GROUP_IDs, ',')) )))                            
AND convert(date,T.CREATED_DATE) BETWEEN @DATE_FROM AND @DATE_TO          
and isnull(tt.NAME,'')<>''          
GROUP BY t.TASK_TYPE_ID,T.CREATED_DATE,tt.NAME                         
order by T.CREATED_DATE desc                        
select distinct TASK_TYPE_NAME into #TASKDATABYTYPE7 FROM #TASKDATABYTYPE6;                        
                        
         OPEN date_series_CURSOR;                              
         FETCH NEXT FROM date_series_CURSOR INTO @currentdate;                         
         WHILE @@FETCH_STATUS = 0                        
BEGIN                        
   ----------------------------------------------------------------------------------------------------------------                        
    INSERT INTO #maintable ([dates]) values(@currentdate);                            
    DECLARE @TASK_TYPE varchar(100);                              
                     DECLARE task_type_CURSOR CURSOR                              
                     FOR select * from #TASKDATABYTYPE7;                              
           
                     OPEN task_type_CURSOR;                              
                     FETCH NEXT FROM task_type_CURSOR INTO @TASK_TYPE;                          
      --print @TASK_TYPE;                        
                     WHILE @@FETCH_STATUS = 0                        
   BEGIN                     
   set @TASK_TYPE = dbo.RemoveSpecialChars(@TASK_TYPE);           
   IF not EXISTS(SELECT * FROM TempDB.INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = REPLACE(@TASK_TYPE,' ','_') and TABLE_NAME like '#maintable%')                        
BEGIN                        
--print 'Column not exist'                        
  DECLARE @SQLAddColumn NVARCHAR(200);                        
SET @SQLAddColumn = 'ALTER TABLE #maintable ADD ' + REPLACE(@TASK_TYPE,' ','_') + ' int';                       
EXEC sys.sp_executesql @SQLAddColumn;                        
END              
   declare @CurrentTypeCount INT;                    
SET @CurrentTypeCount = (select count(*) from #TASKDATABYTYPE6 where cast(CREATED_DATE As date) = cast(@currentdate as date) and dbo.RemoveSpecialChars(TASK_TYPE_NAME) = @TASK_TYPE);                                   
  set @TASK_TYPE = REPLACE(@TASK_TYPE,' ','_')                        
   DECLARE @SQLUpdatedata NVARCHAR(max);            
SET @SQLUpdatedata = 'update #maintable set '+ cast(@TASK_TYPE as varchar(100)) + ' = ' + cast(@CurrentTypeCount as varchar) + ' where cast([dates] as date) = cast(''' + cast(Convert(Date,@currentdate) as varchar) + ''' as date)';          
EXEC (@SQLUpdatedata);                
  set @TASK_TYPE = REPLACE(@TASK_TYPE,'_',' ')                        
                      FETCH NEXT FROM task_type_CURSOR INTO @TASK_TYPE;                        
                   END;                              
                              CLOSE task_type_CURSOR;                              
                              DEALLOCATE task_type_CURSOR;                         
   ----------------------------------------------------------------------------------------------------------------                        
   --PRINT @currentdate;                        
      FETCH NEXT FROM date_series_CURSOR INTO @currentdate;                        
END;                              
         CLOSE date_series_CURSOR;                              
         DEALLOCATE date_series_CURSOR;                       
   select * from #maintable       
