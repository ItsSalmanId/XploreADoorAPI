IF (OBJECT_ID('FOX_PROC_GET_LOGIN_LOG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_LOGIN_LOG  
GO                 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_LOGIN_LOG]                  
  --@PRACTICE_CODE BIGINT,                  
  @EMAIL VARCHAR(50)                  
                   
  AS                  
  BEGIN                  
                    
   SELECT * FROM  (                   
 select  top 1 log.Password,log.DeviceInfo, log.CreatedDate,log.CreatedBy,log.AdLoginResponse, 0 AS is_selected from ws_tbl_fox_login_logs as log                
                  
 where AdLoginResponse  like '%%User Login Successfully%%'                  
 AND UserName=  @EMAIL                  
 AND CreatedBy ='Fox_Portal'                  
order by CreatedDate desc        
        
)AS A                  
 union        
 SELECT * FROM (                 
 select  top 1 log.Password,log.DeviceInfo, log.CreatedDate,log.CreatedBy,log.AdLoginResponse, 1 AS is_selected from ws_tbl_fox_login_logs as log                 
 where AdLoginResponse like '%%Invalid Credential%%'                  
 AND UserName=  @EMAIL                  
  AND CreatedBy ='Fox_Portal'                
          
order by CreatedDate desc        
) AS B                    
  union all                
                
SELECT * FROM (         
 select  top 1 log.Password,log.DeviceInfo, log.CreatedDate,log.CreatedBy,log.AdLoginResponse, 2 AS is_selected from ws_tbl_fox_login_logs as log                
 where AdLoginResponse like '%%User Login Successfully%%'                 
 AND UserName=  @EMAIL                  
 AND CreatedBy ='Fox Web Service Team'         
         
order by CreatedDate desc        
        
) AS C                    
 union         
SELECT * FROM (                
 select  top 1 log.Password,log.DeviceInfo, log.CreatedDate,log.CreatedBy,log.AdLoginResponse, 3 AS is_selected from ws_tbl_fox_login_logs as log                 
 where AdLoginResponse like '%%Invalid Credential%%'                  
 AND UserName=  @EMAIL                  
  AND CreatedBy ='Fox Web Service Team'                      
 order by CreatedDate desc        
             
) AS D                  
 order by is_selected asc                          
  END 