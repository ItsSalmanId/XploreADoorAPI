IF (OBJECT_ID('FOX_PROC_GET_TASK_AVERAGE_CLOSURE_TIME_DATA_FOR_DASHBOARD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_AVERAGE_CLOSURE_TIME_DATA_FOR_DASHBOARD  
GO 
CREATE PROCEDURE [FOX_PROC_GET_TASK_AVERAGE_CLOSURE_TIME_DATA_FOR_DASHBOARD]                                          
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
,  --ISNULL(AVG(DATEDIFF(MINUTE, T.CREATED_DATE, T.COMPLETED_DATE)),0)  AS AVERAGE_CLOSURE                   
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE                                     
                                        
FROM FOX_TBL_TASK AS T                                       
LEFT JOIN FOX_TBL_GROUP AS GP ON                                         
T.SEND_TO_ID = GP.GROUP_ID                              
                                      
WHERE                              
 T.IS_SEND_TO_USER = 0   AND                                  
  (@TASK_TYPE_IDs IS NULL                                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID               
 IN                                        
(                            
SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP             
WHERE GROUP_ID IN                                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                              
--AND T.CREATED_DATE = GETDATE()                           
AND  CONVERT(DATE,T.CREATED_DATE)= CONVERT(Date,GETDATE())                           
AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                    
AND T.IS_SENDTO_MARK_COMPLETE = 1            
AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                   
AND T.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(T.DELETED,0)=0                                     
--ORDER BY GROUP_NAME                            
 AND GP.GROUP_NAME <> ''                                            
GROUP BY (GP.GROUP_NAME)                            
                            
                                                
                                    
END                                        
 if(@TIME_FRAME='YESTERDAY')                                                
BEGIN                                        
 SELECT                                         
GP.GROUP_NAME                                        
, --ISNULL (AVG (DATEDIFF(MINUTE, T.CREATED_DATE, T.COMPLETED_DATE)),0)  AS AVERAGE_CLOSURE                  
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE                                            
                                        
FROM FOX_TBL_TASK AS T                                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                                         
T.SEND_TO_ID = GP.GROUP_ID                               
WHERE                                          
 T.IS_SEND_TO_USER = 0   AND                                         
(@TASK_TYPE_IDs IS NULL                                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                                         
 IN                                        
(SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP                                        
WHERE GROUP_ID IN                                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                                        
AND CONVERT(DATE,T.CREATED_DATE)=  CONVERT(DATE, DATEADD(DAY,-1,GETDATE()))                              
AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                             
 AND GP.GROUP_NAME <> ''                     
 AND T.IS_SENDTO_MARK_COMPLETE = 1             
 AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                                               
GROUP BY (GP.GROUP_NAME)                                        
END                                        
                                        
 if(@TIME_FRAME='THIS_WEEK')                                                
BEGIN                                        
 SELECT                   
GP.GROUP_NAME                                  
,--ISNULL( AVG (DATEDIFF(day, T.CREATED_DATE, T.COMPLETED_DATE)),0)  AS AVERAGE_CLOSURE                   
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE                                          
                                        
FROM FOX_TBL_TASK AS T                         
LEFT JOIN FOX_TBL_GROUP AS GP ON                                     
T.SEND_TO_ID = GP.GROUP_ID                                        
WHERE                                          
 T.IS_SEND_TO_USER = 0   AND                                         
 (@TASK_TYPE_IDs IS NULL                    
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                                         
 IN                                        
(SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP                                        
WHERE GROUP_ID IN                                    
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                                     
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN    CONVERT(DATE,dateadd(day,2-datepart(dw, getdate()), getdate())) AND    CONVERT(DATE, DATEADD(DAY,0,GETDATE()))                            
AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                             
 AND GP.GROUP_NAME <> ''                     
 AND T.IS_SENDTO_MARK_COMPLETE = 1             
 AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                                                    
GROUP BY (GP.GROUP_NAME)                                         
                                        
END                                        
 if(@TIME_FRAME='THIS_MONTH')                                                
BEGIN                                        
 SELECT                                         
GP.GROUP_NAME                                        
,--ISNULL( AVG (DATEDIFF(day, T.CREATED_DATE, T.COMPLETED_DATE)),0)  AS AVERAGE_CLOSURE                    
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE                                          
                                        
FROM FOX_TBL_TASK AS T                                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                                         
T.SEND_TO_ID = GP.GROUP_ID                                        
WHERE                                          
 T.IS_SEND_TO_USER = 0   AND                                        
 (@TASK_TYPE_IDs IS NULL                                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                                         
 IN                                        
(SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP                                        
WHERE GROUP_ID IN                                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                                        
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN  CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0))   AND  CONVERT(DATE, DATEADD(DAY,0,GETDATE()))                            
AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                             
                            
 AND GP.GROUP_NAME <> ''                    
 AND T.IS_SENDTO_MARK_COMPLETE = 1            
 AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                                             
GROUP BY (GP.GROUP_NAME)                                         
END                                              
 if(@TIME_FRAME='LAST_MONTH')                                                
BEGIN                                                        
   BEGIN                      
 SELECT                                         
GP.GROUP_NAME                               
,--ISNULL (AVG (DATEDIFF(day, T.CREATED_DATE, T.COMPLETED_DATE)), 0)  AS AVERAGE_CLOSURE                   
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE                                             
                                        
FROM FOX_TBL_TASK AS T                                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                                         
T.SEND_TO_ID = GP.GROUP_ID                                     
WHERE                                          
 T.IS_SEND_TO_USER = 0   AND                                        
 (@TASK_TYPE_IDs IS NULL                                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                                         
 IN                                        
(SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP                             
WHERE GROUP_ID IN                                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                             
--AND T.CREATED_DATE  BETWEEN CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) )), -1))                                        
 --AND DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 1)) )                                
AND CONVERT (DATE,T.CREATED_DATE)  BETWEEN  DATEADD(DAY,1,EOMONTH(GETDATE(),-2))                                    
 AND  EOMONTH(GETDATE(),-1)                            
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                            
   AND GP.GROUP_NAME <> ''                    
   AND T.IS_SENDTO_MARK_COMPLETE = 1            
   AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                                                  
GROUP BY (GP.GROUP_NAME)                                         
END                                         
 END                                                
 if(@TIME_FRAME='LAST_THREE_MONTHS')                                                
BEGIN                                                
SELECT                                         
                                        
GP.GROUP_NAME                                        
,--ISNULL (AVG (DATEDIFF(day, T.CREATED_DATE, T.COMPLETED_DATE)), 0)  AS AVERAGE_CLOSURE                  
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE                    
                
                                        
FROM FOX_TBL_TASK AS T                                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                                         
T.SEND_TO_ID = GP.GROUP_ID              WHERE                                          
 T.IS_SEND_TO_USER = 0   AND                                       
 (@TASK_TYPE_IDs IS NULL                                                              
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))              
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                                         
 IN                                        
(SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP                                        
WHERE GROUP_ID IN                                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                                        
--AND T.CREATED_DATE BETWEEN CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE())-2, 0)) )), -1))                                          
-- AND  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 1)) )                                
AND CONVERT(DATE,T.CREATED_DATE) BETWEEN DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)                                     
 AND  DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1)                            
 AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                            
  AND GP.GROUP_NAME <> ''                    
  AND T.IS_SENDTO_MARK_COMPLETE = 1             
  AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                                                        
GROUP BY (GP.GROUP_NAME)                                         
                                        
                                       
 END                                    
 if(@TIME_FRAME='DATE_RANGE')                                                
BEGIN                                                
SELECT                                         
                                        
GP.GROUP_NAME                                        
,--CASE WHEN DATEDIFF(DAY, T.CREATED_DATE, T.COMPLETED_DATE) = 0 THEN ISNULL (AVG(DATEDIFF(HOUR, T.CREATED_DATE, T.COMPLETED_DATE)),0) ELSE                         
 --ISNULL (AVG(DATEDIFF(DAY, T.CREATED_DATE, T.COMPLETED_DATE)),0) END AS AVERAGE_CLOSURE                     
CASE WHEN ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)),0) > 1  AND  ISNULL (AVG(DATEDIFF(DAY,  T.CREATED_DATE, T.COMPLETED_DATE)),0) = 0               
THEN                  
CONVERT (INT,CEILING (ISNULL (AVG(DATEDIFF(HOUR,  T.CREATED_DATE, T.COMPLETED_DATE)*1.0),0)))                  
ELSE                           
CONVERT(INT,CEILING(ISNULL(AVG(DATEDIFF(DAY,  T.CREATED_DATE, CONVERT (DATE,T.COMPLETED_DATE))*1.0),0)))END AS AVERAGE_CLOSURE               
                                                                       
FROM FOX_TBL_TASK AS T                                        
LEFT JOIN FOX_TBL_GROUP AS GP ON                                         
T.SEND_TO_ID = GP.GROUP_ID                                        
WHERE                                       
 T.IS_SEND_TO_USER = 0   AND                                         
 (@TASK_TYPE_IDs IS NULL                           
      OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                           
AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID                                         
 IN                                        
(SELECT GROUP_ID                                        
 FROM FOX_TBL_GROUP                                        
WHERE GROUP_ID IN                                        
(select val from dbo.f_split(@GROUP_IDs, ',')) )))                                        
AND CONVERT(DATE,T.CREATED_DATE) Between Convert(Date,@DATE_FROM) and Convert(Date,@DATE_TO)                            
AND T.PRACTICE_CODE = @PRACTICE_CODE AND T.DELETED = 0                             
 AND GP.GROUP_NAME <> ''                    
 AND T.IS_SENDTO_MARK_COMPLETE = 1             
 AND CONVERT(DATE, T.COMPLETED_DATE) >= CONVERT (DATE, T.CREATED_DATE)                                                     
GROUP BY (GP.GROUP_NAME)                                                 
 END                                                
                                         
END   
