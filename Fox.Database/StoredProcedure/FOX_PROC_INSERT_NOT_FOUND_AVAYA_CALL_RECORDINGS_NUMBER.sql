IF (OBJECT_ID('FOX_PROC_INSERT_NOT_FOUND_AVAYA_CALL_RECORDINGS_NUMBER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_NOT_FOUND_AVAYA_CALL_RECORDINGS_NUMBER  
GO   
-- FOX_PROC_INSERT_NOT_FOUND_AVAYA_CALL_RECORDINGS_NUMBER '','',''  
CREATE PROCEDURE FOX_PROC_INSERT_NOT_FOUND_AVAYA_CALL_RECORDINGS_NUMBER           
@FOX_CRAWLER_LOG_ID  BIGINT,            
@INCOMING_CALL_NO VARCHAR(10) NULL,            
@RECORDING_FOR_DATE varchar (500) NULL           
            
AS            
BEGIN               
    INSERT INTO FOX_TBL_CRAWLER_AVAYA_RECORDING_NOT_FOUND_LOGS(FOX_CRAWLER_LOG_ID,INCOMING_CALL_NO,RECORDING_FOR_DATE)            
       VALUES(@FOX_CRAWLER_LOG_ID,@INCOMING_CALL_NO,@RECORDING_FOR_DATE);           
END     