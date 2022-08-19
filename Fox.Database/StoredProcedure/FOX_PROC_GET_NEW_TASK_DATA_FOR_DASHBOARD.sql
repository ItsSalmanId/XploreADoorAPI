IF (OBJECT_ID('FOX_PROC_GET_NEW_TASK_DATA_FOR_DASHBOARD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_NEW_TASK_DATA_FOR_DASHBOARD  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_NEW_TASK_DATA_FOR_DASHBOARD] -- 0 , 'null' ,'' ,'ALL','ALL', 'LAST_THREE_MONTHS'             
                  
                                             @PRACTICE_CODE BIGINT,                       
                                             @DateFromUser  DATETIME,                   
                                             @DateToUser    DATETIME ,              
                                             @GROUP_IDs VARCHAR(8000),                  
                                             @TASK_TYPE_IDs  VARCHAR(8000),              
                                             @TIME_FRAME VARCHAR(50)                     
 AS            
 IF(@DateFromUser = null)            
 BEGIN            
 SET @DateFromUser = ''            
 END            
 ELSE            
 BEGIN            
 SET @DateFromUser = @DateFromUser             
 END              
            
  IF(@DateToUser = null)            
 BEGIN            
 SET @DateToUser = ''            
 END            
 ELSE            
 BEGIN            
 SET @DateToUser = @DateToUser             
 END              
 if(@TIME_FRAME ='')                  
BEGIN                  
SET @TIME_FRAME = 'LAST_THREE_MONTHS'                  
END                   
ELSE                  
BEGIN                  
SET @TIME_FRAME = @TIME_FRAME                  
END                   
BEGIN                    
if(@TASK_TYPE_IDs = 'ALL')                  
BEGIN                  
SET @TASK_TYPE_IDs = NULL                  
END                  
ELSE                  
BEGIN                  
SET @TASK_TYPE_IDs = @TASK_TYPE_IDs                  
END                  
if(@GROUP_IDs = 'ALL')                  
BEGIN                  
SET @GROUP_IDs = NULL                  
END                  
ELSE                  
BEGIN                  
SET @GROUP_IDs = @GROUP_IDs                  
END                    
     BEGIN                  
         DECLARE @DATE_FROM DATETIME;                  
         DECLARE @DATE_TO DATETIME;                  
         DECLARE @DATE_TO_I DATETIME;                  
         DECLARE @DATE_FROM_I DATETIME;                  
         DECLARE @Working_Days INT;                  
         DECLARE @Total_Days INT;                  
         --SET @DATE_TO = SELECT CONVERT(DATE, DATEADD(day, 1, GETDATE()));                  
         --SET @DATE_TO_I = CONVERT(DATE, DATEADD(day, 1, GETDATE()));         
      IF(@TIME_FRAME = 'TODAY')              
             BEGIN                              
                    
            
      SET @DATE_FROM = convert(date,GETDATE());                  
                 SET @DATE_FROM_I = convert(date,GETDATE());        
     --SET @DATE_TO =  CONVERT(DATE, DATEADD(day, 1, GETDATE()));         
     --SET @DATE_TO_I = CONVERT(DATE, DATEADD(day, 1, GETDATE()));         
     SET @DATE_TO = CONVERT(DATE, GETDATE());                  
                 SET @DATE_TO_I = CONVERT(DATE, GETDATE());                   
                  --SELECT @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);        
      SET @Total_Days = CASE WHEN  DATEDIFF(day, @DATE_FROM, @DATE_TO) = 0 THEN 1 ELSE DATEDIFF(day, @DATE_FROM, @DATE_TO) END;              
             END;        
    ELSE                 
         IF(@TIME_FRAME ='THIS_MONTH' )                  
             BEGIN                         
                 --This Month                  
                   SET @DATE_FROM =    CONVERT(DATE, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0));                  
                   SET @DATE_FROM_I = CONVERT(DATE, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0));         
       SET @DATE_TO =      CONVERT(DATE, DATEADD(DAY,0,GETDATE()))         
        SET @DATE_TO_I =   CONVERT(DATE, DATEADD(DAY,0,GETDATE()))         
                  --SELECT @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);        
   SET @Total_Days = CASE WHEN  DATEDIFF(day, @DATE_FROM, @DATE_TO) = 0 THEN 1 ELSE DATEDIFF(day, @DATE_FROM, @DATE_TO) END;         
                         
       END;                  
             ELSE                  
         IF(@TIME_FRAME ='YESTERDAY')                  
             BEGIN                         
                 --YESTERDAY                 
                    
                 SET @DATE_FROM = (DATEADD(dd, -1, CONVERT(DATE, GETDATE())));                  
                 SET @DATE_FROM_I = (DATEADD(dd, -1, CONVERT(DATE, GETDATE())));        
     --SET @DATE_TO =   CONVERT(DATE, DATEADD(day, 1, GETDATE()));         
     --SET @DATE_TO_I = CONVERT(DATE, DATEADD(day, 1, GETDATE()));        
        
     SET @DATE_TO =  (DATEADD(dd, -1, CONVERT(DATE, GETDATE())));                  
                 SET @DATE_TO_I = (DATEADD(dd, -1, CONVERT(DATE, GETDATE())));                   
                 --SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);        
      SET @Total_Days = CASE WHEN  DATEDIFF(day, @DATE_FROM, @DATE_TO) = 0 THEN 1 ELSE DATEDIFF(day, @DATE_FROM, @DATE_TO) END;                   
             END;                  
             ELSE                  
         IF(@TIME_FRAME ='THIS_WEEK')                  
             BEGIN                         
                 --This Week                
                     
                -- SET @DATE_FROM = SELECT (DATEADD(dd, -7, CONVERT(DATE, GETDATE())));           
       SET @DATE_FROM =  CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate()));                  
                 --SET @DATE_FROM_I =  (DATEADD(dd, -17, CONVERT(DATE, GETDATE())));          
                   SET @DATE_FROM_I = CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate()));         
                SET @DATE_TO =  CONVERT(DATE, GETDATE());        
                SET @DATE_TO_I =  CONVERT(DATE, GETDATE());                 
                 SET @Total_Days = CASE WHEN DATEDIFF(day, @DATE_FROM, @DATE_TO) = 0 THEN 1 ELSE DATEDIFF(day, @DATE_FROM, @DATE_TO) END;         
                    
             END;                  
             ELSE                  
         IF(@TIME_FRAME ='LAST_MONTH')                  
             BEGIN                                      
                 --SET @DATE_FROM =  (DATEADD(dd, -30, CONVERT(DATE, GETDATE())));          
     SET @DATE_FROM = CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), 0));                    
                 SET @DATE_FROM_I = CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), 0));        
      set @DATE_TO =  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );        
       set @DATE_TO_I =  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );                       
                 --SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);        
      SET @Total_Days = CASE WHEN DATEDIFF(day, @DATE_FROM, @DATE_TO) = 0 THEN 1 ELSE DATEDIFF(day, @DATE_FROM, @DATE_TO) END;                   
             END;                  
             ELSE                  
    IF(@TIME_FRAME ='LAST_THREE_MONTHS')                  
             BEGIN                         
                 --Last 3 months          
     SET @DATE_FROM =   CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE())-2, 0)) )), 0));                          
                 --SET @DATE_FROM = DATEADD(MONTH, -3, GETDATE());                  
                 SET @DATE_FROM_I = CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE())-2, 0)) )), 0)); ;       
     set @DATE_TO = DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );        
     set @DATE_TO_I = DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );                
                       
                 SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);            
             END;                  
             ELSE                  
         IF(@TIME_FRAME='DATE_RANGE')                  
             BEGIN                  
                 SET @DATE_FROM = @DateFromUser;                  
                 SET @DATE_FROM_I = @DateFromUser;                  
                 --SET @DATE_TO = DATEADD(DAY, 1, @DateToUser);                  
                 --SET @DATE_TO_I = DATEADD(DAY, 1, @DateToUser);        
     SET @DATE_TO = DATEADD(DAY, 0, @DateToUser);                  
                 SET @DATE_TO_I = DATEADD(DAY, 0, @DateToUser);                  
                 --SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);        
      SET @Total_Days = CASE WHEN DATEDIFF(day, @DATE_FROM, @DATE_TO) = 0 THEN 1 ELSE DATEDIFF(day, @DATE_FROM, @DATE_TO) END;                     
             END;                  
         DECLARE @daysdifference INT;                  
         SET @daysdifference =                  
         (                  
             SELECT DATEDIFF(day, @DATE_FROM_I, @DATE_TO_I)                
         );        
              
         IF OBJECT_ID('tempdb..#NoOfDays') IS NOT NULL                  
             DROP TABLE #noofdays;                  
         CREATE TABLE #noofdays                  
         (day       INT,                   
          month     INT,                   
          year      INT,                   
          monthName VARCHAR(5)                  
         );                  
         WHILE(@daysdifference >= 0)                  
             BEGIN                  
                 INSERT INTO #noofdays                  
                 (day,                   
                  month,                   
                  year,                   
                  monthName                  
                 )                  
                 VALUES                  
                 (DAY(@DATE_FROM_I),                   
                  MONTH(@DATE_FROM_I),                   
                  YEAR(@DATE_FROM_I),                   
       CONVERT(CHAR(3), CONVERT(DATE, @DATE_FROM_I), 0)                  
                 );                  
                 SET @DATE_FROM_I = DATEADD(day, 1, @DATE_FROM_I);                  
                 SET @daysdifference = @daysdifference - 1;                  
             END;                  
         IF OBJECT_ID('tempdb..#Table1') IS NOT NULL                  
             DROP TABLE #table1;                  
         SELECT DAY(t1.CREATED_DATE) day,                   
                MONTH(t1.CREATED_DATE) month,                   
                YEAR(t1.CREATED_DATE) year,                   
                COUNT(*) totalRecord                  
         INTO #table1                  
 FROM FOX_TBL_TASK t1              
LEFT JOIN FOX_TBL_GROUP AS GP ON                   
t1.SEND_TO_ID = GP.GROUP_ID                  
WHERE                    
                
 (@TASK_TYPE_IDs IS NULL                                        
      OR T1.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                     
AND (@GROUP_IDs IS NULL OR (T1.SEND_TO_ID                   
 IN                  
(SELECT GROUP_ID                  
 FROM FOX_TBL_GROUP                  
WHERE GROUP_ID IN                  
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                  
AND        CONVERT(DATE, t1.CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO)                  
               AND ISNULL(t1.DELETED, 0) = 0                  
               AND t1.PRACTICE_CODE = @PRACTICE_CODE                  
   GROUP BY DAY(T1.CREATED_DATE),                   
                  MONTH(T1.CREATED_DATE),                   
                  YEAR(T1.CREATED_DATE)                  
         ORDER BY 1 ASC;                  
       SELECT t2.day,                   
                t2.month,                   
                t2.monthName,                   
                t2.year,                   
        Isnull(t1.totalrecord, 0) totalRecord,                   
         (                  
             SELECT COUNT(*) / @Total_Days                  
             FROM FOX_TBL_TASK AS T1              
     JOIN FOX_TBL_GROUP AS GP ON                   
t1.SEND_TO_ID = GP.GROUP_ID                  
WHERE                    
                
 (@TASK_TYPE_IDs IS NULL                                        
      OR T1.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                     
AND (@GROUP_IDs IS NULL OR (T1.SEND_TO_ID                   
 IN                  
(SELECT GROUP_ID                  
 FROM FOX_TBL_GROUP                  
WHERE GROUP_ID IN                  
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                    
            AND(CONVERT(DATE, T1.CREATED_DATE)) BETWEEN(CONVERT(DATE, @DATE_FROM)) AND(CONVERT(DATE, @DATE_TO))                  
                  AND ISNULL(T1.DELETED, 0) = 0                  
                  AND T1.PRACTICE_CODE = @PRACTICE_CODE                  
         ) average                  
         FROM #table1 t1                  
              RIGHT JOIN #noofdays t2 ON t1.day = t2.day                  
                                         AND t1.month = t2.month                  
                                         AND t1.year = t2.year                  
         ORDER BY t2.year,                   
                  t2.month,                   
                  t2.day;                  
     END;              
  END  
