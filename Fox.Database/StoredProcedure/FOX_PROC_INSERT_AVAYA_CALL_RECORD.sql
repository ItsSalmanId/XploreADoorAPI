IF (OBJECT_ID('FOX_PROC_INSERT_AVAYA_CALL_RECORD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_AVAYA_CALL_RECORD  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_AVAYA_CALL_RECORD]            
@AVAYA_CALL_RECORDINGS_LOGS_ID BIGINT,      
@RECORDING_NAME VARCHAR (100) ,      
@CURRENT_FOLDER VARCHAR (100) ,      
@CALL_DATE varchar (500) NULL,      
@CALL_START_TIME varchar (500) NULL,      
@CALL_END_TIME varchar (500) NULL,      
@CALL_NO varchar(10) NULL,      
@CALL_BY varchar(70) NULL,      
@CALL_DIRECTION VARCHAR(20) NULL,      
@OFFICE_NAME VARCHAR(20) NULL,      
@EXTENSION VARCHAR (5) NULL,      
@CORE_QUEUE VARCHAR (100) NULL,      
@CREATED_DATE_ON_SERVER varchar (500) NULL,      
@SERVICE_READ_DATE varchar (500) NULL,      
@DS_READING_TIME VARCHAR (100) NULL,      
@SUCCESS_OR_ERROR VARCHAR (100) NULL,      
@INITIAL_SIZE VARCHAR (100) NULL,      
@FINAL_SIZE VARCHAR (100) NULL      
            
AS            
BEGIN            
          
INSERT INTO FOX_TBL_AVAYA_CALL_RECORDINGS_LOGS            
(            
AVAYA_CALL_RECORDINGS_LOGS_ID,      
RECORDING_NAME ,      
CURRENT_FOLDER ,      
CALL_DATE ,      
CALL_START_TIME ,      
CALL_END_TIME ,      
CALL_NO,      
CALL_BY ,      
CALL_DIRECTION ,      
OFFICE_NAME ,      
EXTENSION ,      
CORE_QUEUE ,      
CREATED_DATE_ON_SERVER ,      
SERVICE_READ_DATE ,      
DS_READING_TIME,      
SUCCESS_OR_ERROR,      
INITIAL_SIZE,      
FINAL_SIZE,      
CREATED_BY,      
CREATED_DATE,      
MODIFIED_BY ,      
MODIFIED_DATE,      
DELETED)            
       VALUES            
     (@AVAYA_CALL_RECORDINGS_LOGS_ID,            
     @RECORDING_NAME,       
  @CURRENT_FOLDER,      
     --GETDATE(),            
     --GETDATE(),            
     --GETDATE(),            
     CAST(@CALL_DATE AS DATETIME),            
     CAST(@CALL_START_TIME AS DATETIME),            
     CAST(@CALL_END_TIME AS DATETIME),            
              @CALL_NO,            
              @CALL_BY,            
              @CALL_DIRECTION,            
              @OFFICE_NAME,            
              @EXTENSION,            
              @CORE_QUEUE,        
  --GETDATE(),            
     --GETDATE(),            
    CAST(@CREATED_DATE_ON_SERVER AS DATETIME),            
    CAST(@SERVICE_READ_DATE AS DATETIME),       
     @DS_READING_TIME,      
     @SUCCESS_OR_ERROR ,      
     @INITIAL_SIZE,      
     @FINAL_SIZE,      
              'FOX TEAM',            
              GETDATE(),            
              'FOX TEAM',            
              GETDATE(),            
              0);            
            
     SELECT TOP 1 * FROM FOX_TBL_AVAYA_CALL_RECORDINGS_LOGS ORDER BY CREATED_DATE            
END 
