IF (OBJECT_ID('FOX_PROC_GET_PASSWORD_FROM_LOGS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PASSWORD_FROM_LOGS  
GO  
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PASSWORD_FROM_LOGS]                 
(              
@EMAIL VARCHAR(100)               
)          
AS          
    BEGIN          
       
     select top 1 Password, createddate from ws_tbl_fox_login_logs where adloginresponse = 'User Login Successfully' and username = @EMAIL and CreatedBy = 'Fox_Portal' order by createddate desc        
        
     END;    
