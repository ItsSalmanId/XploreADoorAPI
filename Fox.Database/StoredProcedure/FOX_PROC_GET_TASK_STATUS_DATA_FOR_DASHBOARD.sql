IF (OBJECT_ID('FOX_PROC_GET_TASK_STATUS_DATA_FOR_DASHBOARD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_STATUS_DATA_FOR_DASHBOARD  
GO 
CREATE PROCEDURE [FOX_PROC_GET_TASK_STATUS_DATA_FOR_DASHBOARD]                        
               @PRACTICE_CODE   BIGINT,                        
               @GROUP_IDs VARCHAR(8000),                      
      @TASK_TYPE_IDs  VARCHAR(8000),                        
               @DATE_FROM DATETIME,                                            
               @DATE_TO DATETIME,                      
               @TIME_FRAME VARCHAR(50)                      
                                
                        
AS                      
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
 if(@TIME_FRAME='TODAY')                              
BEGIN                      
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA1', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA1;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA2', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA2;                       
                       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA1                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0             
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0          
AND T.PRACTICE_CODE = @PRACTICE_CODE                      
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                         
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND   CONVERT(DATE,T.CREATED_DATE)= CONVERT(Date,GETDATE())                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA2                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON       
T.SEND_TO_ID = GP.GROUP_ID                  WHERE  T.IS_SEND_TO_USER = 0             
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0           
AND T.PRACTICE_CODE = @PRACTICE_CODE                         
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND CONVERT(DATE,T.CREATED_DATE)= CONVERT(Date,GETDATE())                       
GROUP BY (GP.GROUP_NAME)                      
                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
 left JOIN #TASKDATA1 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA2 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
 WHERE AA.CLOSE_TASK IS NOT NULL OR                       
 BB.OPEN_TASK IS NOT NULL                      
END                      
 if(@TIME_FRAME='YESTERDAY')                              
BEGIN                      
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA3', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA3;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA4', 'U') IS NOT NULL                        
     DROP TABLE #TASKDATA4;                       
                       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA3                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0             
AND ISNULL(T.DELETED , 0) = 0 AND ISNULL(GP.GROUP_ID,0)=0                       
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0          
AND T.PRACTICE_CODE = @PRACTICE_CODE                           
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                        
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND CONVERT(DATE,T.CREATED_DATE) = CONVERT(DATE, DATEADD(DAY,-1,GETDATE()))                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA4                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0           
AND T.PRACTICE_CODE = @PRACTICE_CODE                           
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND CONVERT(DATE,T.CREATED_DATE) = CONVERT(DATE, DATEADD(DAY,-1,GETDATE()))                    
GROUP BY (GP.GROUP_NAME)                     
                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
left JOIN #TASKDATA3 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA4 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
 WHERE AA.CLOSE_TASK IS NOT NULL OR                       
 BB.OPEN_TASK IS NOT NULL                          
END                      
                      
 if(@TIME_FRAME='THIS_WEEK')          
BEGIN                      
 IF OBJECT_ID('TEMPDB.DBO.#TASKDATA5', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA5;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA6', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA6;                       
                       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA5                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0            
AND T.PRACTICE_CODE = @PRACTICE_CODE                        
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                        
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN   CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate())) AND   CONVERT(DATE, DATEADD(DAY,0,GETDATE()))                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA6                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0           
AND T.PRACTICE_CODE = @PRACTICE_CODE                          
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate())) AND  CONVERT(DATE, DATEADD(DAY,0,GETDATE()))                     
GROUP BY (GP.GROUP_NAME)                      
                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
 left JOIN #TASKDATA5 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA6 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
 WHERE AA.CLOSE_TASK IS NOT NULL OR                       
 BB.OPEN_TASK IS NOT NULL                          
                      
END                      
 if(@TIME_FRAME='THIS_MONTH')                              
BEGIN                      
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA7', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA7;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA8', 'U') IS NOT NULL                      
             DROP TABLE #TASKDATA8;                       
                       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA7                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0           
AND T.PRACTICE_CODE = @PRACTICE_CODE                        
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                   
            
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0))  AND  CONVERT(DATE, DATEADD(DAY,0,GETDATE()))                      
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT                       
GP.GROUP_NAME                 
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA8                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0           
AND T.PRACTICE_CODE = @PRACTICE_CODE                          
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0))  AND  CONVERT(DATE, DATEADD(DAY,0,GETDATE()))                          
GROUP BY (GP.GROUP_NAME)                      
                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
 left JOIN #TASKDATA7 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA8 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
 WHERE AA.CLOSE_TASK IS NOT NULL OR                       
 BB.OPEN_TASK IS NOT NULL                          
END                            
 if(@TIME_FRAME='LAST_MONTH')                              
BEGIN                                      
 IF OBJECT_ID('TEMPDB.DBO.#TASKDATA9', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA9;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA10', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA10;                       
                       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA9                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID          
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0                         
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1           
AND T.PRACTICE_CODE = @PRACTICE_CODE            
--AND T.CREATED_DATE  BETWEEN  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), -1))                      
-- AND DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 1)) )                  
--AND CONVERT(DATE,T.CREATED_DATE)  BETWEEN   CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), 0))                       
 --AND   CONVERT(DATE, DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) ))           
           
 AND CONVERT(DATE,T.CREATED_DATE)  BETWEEN    DATEADD(DAY,1,EOMONTH(GETDATE(),-2))                        
 AND  EOMONTH(GETDATE(),-1)                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                        
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA10                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0          
AND T.PRACTICE_CODE = @PRACTICE_CODE                          
--AND CONVERT(DATE,T.CREATED_DATE)  BETWEEN   CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), 0))                       
 --AND    DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )          
  AND CONVERT(DATE,T.CREATED_DATE)  BETWEEN    DATEADD(DAY,1,EOMONTH(GETDATE(),-2))                        
 AND  EOMONTH(GETDATE(),-1)                        
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
                      
                      
GROUP BY (GP.GROUP_NAME)                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
 left JOIN #TASKDATA9 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA10 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
 WHERE AA.CLOSE_TASK IS NOT NULL OR                       
 BB.OPEN_TASK IS NOT NULL                          
 END                              
 if(@TIME_FRAME='LAST_THREE_MONTHS')                              
BEGIN                              
 IF OBJECT_ID('TEMPDB.DBO.#TASKDATA11', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA11;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA12', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA12;                       
                       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA11                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0           
AND T.PRACTICE_CODE = @PRACTICE_CODE                         
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1      
--AND T.CREATED_DATE BETWEEN select CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE())-2, 0)) )), -1))                        
-- AND  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 1)) )                        
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)                       
 AND  DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1)                      
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                        
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA12                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0            
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0          
AND T.PRACTICE_CODE = @PRACTICE_CODE                          
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)                        
 AND  DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1)                      
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) ))                       
GROUP BY (GP.GROUP_NAME)                    
                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
 left JOIN #TASKDATA11 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA12 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
WHERE AA.CLOSE_TASK IS NOT NULL OR BB.OPEN_TASK IS NOT NULL                    
 END                                      
 if(@TIME_FRAME='DATE_RANGE')                              
BEGIN                              
  IF OBJECT_ID('TEMPDB.DBO.#TASKDATA13', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA13;                       
IF OBJECT_ID('TEMPDB.DBO.#TASKDATA14', 'U') IS NOT NULL                        
             DROP TABLE #TASKDATA14;                       
       
SELECT                       
GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS CLOSE_TASK                       
INTO #TASKDATA13                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0             
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0          
AND T.PRACTICE_CODE = @PRACTICE_CODE                        
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 1                      
AND CONVERT(DATE,T.CREATED_DATE) Between CONVERT(DATE,@DATE_FROM) and CONVERT(DATE,@DATE_TO )                          
AND (@TASK_TYPE_IDs IS NULL                          
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                        
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
                      
GROUP BY (GP.GROUP_NAME)                       
                      
SELECT       GP.GROUP_NAME                      
,ISNULL (COUNT(T.IS_SENDTO_MARK_COMPLETE),0) AS OPEN_TASK                      
INTO #TASKDATA14                       
FROM FOX_TBL_TASK AS T                      
LEFT JOIN FOX_TBL_GROUP AS GP ON                       
T.SEND_TO_ID = GP.GROUP_ID                      
WHERE  T.IS_SEND_TO_USER = 0          
AND ISNULL(T.DELETED , 0) = 0             
AND ISNULL(GP.DELETED,0) = 0          
AND T.PRACTICE_CODE = @PRACTICE_CODE                         
AND CONVERT(DATE,T.CREATED_DATE) Between CONVERT(DATE,@DATE_FROM) and CONVERT(DATE,@DATE_TO )                     
AND ISNULL(T.IS_SENDTO_MARK_COMPLETE, 0) = 0                       
AND (@TASK_TYPE_IDs IS NULL                                            
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                       
 IN                      
(SELECT GROUP_ID                      
 FROM FOX_TBL_GROUP                      
WHERE GROUP_ID IN                      
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                      
                      
                      
GROUP BY (GP.GROUP_NAME)                      
SELECT DISTINCT G.GROUP_NAME,ISNULL(AA.CLOSE_TASK,0) AS CLOSE_TASK,ISNULL(BB.OPEN_TASK,0) AS OPEN_TASK                      
 FROM FOX_TBL_GROUP AS G                      
 left JOIN #TASKDATA13 AA ON AA.GROUP_NAME = G.GROUP_NAME                      
 left JOIN  #TASKDATA14 BB ON BB.GROUP_NAME = G.GROUP_NAME                      
 WHERE AA.CLOSE_TASK IS NOT NULL OR                       
 BB.OPEN_TASK IS NOT NULL                              
 END                              
                       
END   
