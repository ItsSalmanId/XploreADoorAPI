IF (OBJECT_ID('FOX_PROC_CREATED_TASK_TYPE_DATA_ALTERNATIVE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CREATED_TASK_TYPE_DATA_ALTERNATIVE
GO
----------------------------------------------------------------  
--CREATED_TASK_TYPE_DATA_ALTERNATIVE 1011163 , 'all','ALL','11/30/2020' 
CREATE PROCEDURE FOX_PROC_CREATED_TASK_TYPE_DATA_ALTERNATIVE --1011163 , 'all','ALL',''         
        
(                          
@PRACTICE_CODE   BIGINT,                                
@GROUP_IDs VARCHAR(8000),                              
@TASK_TYPE_IDs  VARCHAR(8000),                                
@CREATED_DATE DATETIME                                                                
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
BEGIN        
        
   select T.TASK_TYPE_ID,MAX(tt.NAME) as TASK_TYPE_NAME,COUNT(T.TASK_TYPE_ID) AS TASK_COUNT        
   from fox_tbl_task T        
   LEFT JOIN FOX_TBL_TASK_TYPE TT ON TT.TASK_TYPE_ID = T.TASK_TYPE_ID        
   --LEFT JOIN FOX_TBL_GROUP AS G        
   --ON G.GROUP_ID = T.SEND_TO_ID        
   where CONVERT(DATE,T.CREATED_DATE)= CONVERT(Date,@CREATED_DATE)          
   AND T.TASK_TYPE_ID IS NOT NULL        
   AND ISNULL(T.DELETED,0)=0                          
   AND T.PRACTICE_CODE = @PRACTICE_CODE                          
   AND(@TASK_TYPE_IDs IS NULL OR T.TASK_TYPE_ID IN (select val from dbo.f_split(@TASK_TYPE_IDs, ',')))                                 
   AND (@GROUP_IDs IS NULL OR (T.SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_GROUP WHERE GROUP_ID IN (select val from dbo.f_split(@GROUP_IDs, ',')) )))         
   GROUP BY T.TASK_TYPE_ID        
   END        
   END 