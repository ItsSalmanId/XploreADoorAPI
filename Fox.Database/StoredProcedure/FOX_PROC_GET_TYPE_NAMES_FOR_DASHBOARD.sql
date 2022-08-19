IF (OBJECT_ID('FOX_PROC_GET_TYPE_NAMES_FOR_DASHBOARD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TYPE_NAMES_FOR_DASHBOARD  
GO 
CREATE PROCEDURE FOX_PROC_GET_TYPE_NAMES_FOR_DASHBOARD --1011163, 'ALL','ALL',NULL,NULL, 'LAST_THREE_MONTHS'     
(                      
@PRACTICE_CODE   BIGINT,                            
@GROUP_IDs VARCHAR(8000),                          
@TASK_TYPE_IDs  VARCHAR(8000),                            
@DATE_FROM DATETIME,                     
@DATE_TO DATETIME,                          
@TIME_FRAME VARCHAR(50)                       
)       
AS    
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
     set @DATE_TO =  DateAdd(DAY,-1, CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0,GETDATE()), 0)) );                      
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
BEGIN    
    
select  DISTINCT TT.NAME,TT.TASK_TYPE_ID from FOX_TBL_TASK AS T    
 INNER JOIN FOX_TBL_TASK_TYPE AS TT    
ON T.TASK_TYPE_ID = TT.TASK_TYPE_ID    
 where convert(date,T.CREATED_DATE) BETWEEN @DATE_FROM AND @DATE_TO       
   AND T.TASK_TYPE_ID IS NOT NULL    
   AND ISNULL(T.DELETED,0)=0                      
   AND T.PRACTICE_CODE = @PRACTICE_CODE                      
   AND(@TASK_TYPE_IDs IS NULL OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                             
   AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_GROUP WHERE GROUP_ID IN (select val from dbo.f_split(@GROUP_IDs, ',')) )))     
   --GROUP BY T.TASK_TYPE_ID    
    
END    
END    
