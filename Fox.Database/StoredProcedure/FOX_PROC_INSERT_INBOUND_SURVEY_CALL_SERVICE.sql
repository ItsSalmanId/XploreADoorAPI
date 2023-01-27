-- =============================================        
-- AUTHOR:  <DEVELOPER, MUHAMMAD ARSLAN>        
-- CREATE DATE: <CREATE DATE, 01/02/2023>        
-- DESCRIPTION: <INSERT INBOUND SURVEY CALLS RECORDINGS>        
        
CREATE PROCEDURE FOX_PROC_INSERT_INBOUND_SURVEY_CALL_SERVICE   
@SURVEY_INBOUND_CALL_ID  BIGINT,        
@CALL_DATE varchar (500) NULL,        
@CALL_START_TIME varchar (500) NULL,        
@CALL_END_TIME varchar (500) NULL,        
@CALL_NO varchar(10) NULL,        
@CALL_BY varchar(70) NULL,        
@IS_INCOMING BIT  NULL,        
@OFFICE_NAME VARCHAR(20) NULL,        
@EXTENSION VARCHAR (5) NULL,        
@CALL_RECORDING_PATH varchar (500) NULL,  
@PRACTICE_CODE BIGINT NULL         
        
AS        
BEGIN        
    INSERT INTO FOX_TBL_PATIENT_SURVEY_INBOUND_CALL        
    (        
     SURVEY_INBOUND_CALL_ID,         
     CALL_DATE,        
     CALL_START_TIME,         
     CALL_END_TIME,         
     CALL_NO,         
     CALL_BY,        
     IS_INCOMING,         
     OFFICE_NAME,         
     EXTENSION,         
     CALL_RECORDING_PATH,      
  PRACTICE_CODE,            
     CREATED_BY,         
     CREATED_DATE,         
     MODIFIED_BY,         
     MODIFIED_DATE,         
     DELETED    
  )        
     VALUES        
     (    
  @SURVEY_INBOUND_CALL_ID,        
     @CALL_DATE,           
     CAST(@CALL_START_TIME AS DATETIME),        
     CAST(@CALL_END_TIME AS DATETIME),        
     @CALL_NO,        
     @CALL_BY,        
     @IS_INCOMING,        
     @OFFICE_NAME,        
     @EXTENSION,        
     @CALL_RECORDING_PATH,     
  @PRACTICE_CODE,           
     'FOX TEAM',        
     GETDATE(),        
     'FOX TEAM',        
     GETDATE(),        
     0    
  );        
END 