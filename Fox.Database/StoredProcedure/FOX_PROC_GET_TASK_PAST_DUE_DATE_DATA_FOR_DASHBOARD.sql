IF (OBJECT_ID('FOX_PROC_GET_TASK_PAST_DUE_DATE_DATA_FOR_DASHBOARD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_PAST_DUE_DATE_DATA_FOR_DASHBOARD  
GO 
CREATE PROCEDURE [FOX_PROC_GET_TASK_PAST_DUE_DATE_DATA_FOR_DASHBOARD]                          
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
                        
                                     
                        
                         
SELECT                         
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                
AND T.IS_SEND_TO_USER = 0      
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                       
AND (@TASK_TYPE_IDs IS NULL                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
 FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                        
AND CONVERT(DATE, T.CREATED_DATE) = CONVERT(DATE, GETDATE())          
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                           
GROUP BY (GP.GROUP_NAME)                         
                        
                        
                        
                        
END                        
 if(@TIME_FRAME='YESTERDAY')                                
BEGIN                        
 SELECT                         
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                
AND T.IS_SEND_TO_USER = 0      
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                         
AND (@TASK_TYPE_IDs IS NULL                                              
OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                        
AND CONVERT(DATE, T.CREATED_DATE) =  CONVERT(DATE, DATEADD(DAY,-1,GETDATE()))           
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                          
GROUP BY (GP.GROUP_NAME)                        
END                        
                        
 if(@TIME_FRAME='THIS_WEEK')                                
BEGIN                        
 SELECT                         
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                
AND T.IS_SEND_TO_USER = 0       
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                      
AND (@TASK_TYPE_IDs IS NULL                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
 FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                        
AND CONVERT(DATE, T.CREATED_DATE) BETWEEN   CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate())) AND   CONVERT(DATE, DATEADD(DAY,0,GETDATE()))           
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                           
GROUP BY (GP.GROUP_NAME)                         
                        
END                        
 if(@TIME_FRAME='THIS_MONTH')                                
BEGIN                        
 SELECT                        
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                
AND T.IS_SEND_TO_USER = 0      
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                        
AND (@TASK_TYPE_IDs IS NULL                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
 FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                        
AND CONVERT(DATE, T.CREATED_DATE) BETWEEN  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0))  AND  CONVERT(DATE, DATEADD(DAY,0,GETDATE()))          
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                           
GROUP BY (GP.GROUP_NAME)                         
END                              
 if(@TIME_FRAME='LAST_MONTH')                                
BEGIN                                        
   BEGIN                        
 SELECT                         
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T              
LEFT JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                 
AND T.IS_SEND_TO_USER = 0              
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                        
AND (@TASK_TYPE_IDs IS NULL                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
 FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                        
AND CONVERT(DATE, T.CREATED_DATE)  BETWEEN   DATEADD(DAY,1,EOMONTH(GETDATE(),-2))                        
 AND  EOMONTH(GETDATE(),-1)           
  AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                         
GROUP BY (GP.GROUP_NAME)                         
END                         
 END                                
 if(@TIME_FRAME='LAST_THREE_MONTHS')                                
BEGIN                               
SELECT                         
                        
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                
AND T.IS_SEND_TO_USER = 0              
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                
 --AND GP.GROUP_NAME <> NULL                          
AND (@TASK_TYPE_IDs IS NULL                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
 FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                        
--AND T.CREATED_DATE BETWEEN  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE())-2, 0)) )), -1))                          
-- AND  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 1)) )                        
AND  CONVERT(DATE, T.CREATED_DATE) BETWEEN  DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)                           
 AND  DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1)                
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                           
GROUP BY (GP.GROUP_NAME)                         
                        
                        
                        
                        
                        
 END                                        
 if(@TIME_FRAME='DATE_RANGE')                                
BEGIN                                
SELECT                         
                         
GP.GROUP_NAME                        
,COUNT(*) AS DUE_DATE_TIME                         
                        
FROM FOX_TBL_TASK AS T                        
JOIN FOX_TBL_GROUP AS GP ON                         
T.SEND_TO_ID = GP.GROUP_ID                        
WHERE                          
CAST(GETDATE() AS DATE) > CAST(T.DUE_DATE_TIME AS DATE)                
AND T.IS_SEND_TO_USER = 0               
AND T.SEND_TO_ID IS NOT NULL    
AND T.IS_SENDTO_MARK_COMPLETE = 0                       
AND (@TASK_TYPE_IDs IS NULL                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                         
 IN                        
(SELECT GROUP_ID                        
 FROM FOX_TBL_GROUP                        
WHERE GROUP_ID IN                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                       
AND CONVERT(DATE, T.CREATED_DATE) Between Convert(Date,@DATE_FROM) and Convert(Date,@DATE_TO)                
--AND GP.GROUP_NAME <> NULL           
   AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                   
  AND ISNULL(GP.DELETED,0) = 0                           
GROUP BY (GP.GROUP_NAME)                                 
 END                                
                         
END   
