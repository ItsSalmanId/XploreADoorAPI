IF (OBJECT_ID('FOX_PROC_INSERT_DUPLICATE_CALL_RECORD') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_DUPLICATE_CALL_RECORD  
GO 
  -- =============================================        
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>        
-- CREATE DATE: <CREATE DATE, 15/10/2019>        
-- DESCRIPTION: <INSERT DUPLICATE CALL RECORD>              
-- EXEC FOX_PROC_INSERT_DUPLICATE_CALL_RECORD 544101, 1011163,'12/26/2019','10/22/2019 3:17:09 AM','10/22/2019 3:17:09 AM','8795468795','Shery',1,'MTBC','1235','ps_1011163_112233445_1530963927631'         
CREATE PROCEDURE FOX_PROC_INSERT_DUPLICATE_CALL_RECORD        
@PHD_CALL_UNMAPPED_ID  BIGINT,        
@PRACTICE_CODE BIGINT NULL,        
@CALL_DATE varchar (500) NULL,        
@CALL_START_TIME varchar (500) NULL,        
@CALL_END_TIME varchar (500) NULL,        
@CALL_NO varchar(10) NULL,        
@CALL_BY varchar(70) NULL,        
@IS_INCOMING BIT  NULL,        
@OFFICE_NAME VARCHAR(20) NULL,        
@EXTENSION VARCHAR (5) NULL,        
@CALL_RECORDING_PATH varchar (500) NULL        
        
AS        
BEGIN        
        
--DECLARE @PHD_CALL_UNMAPPED_ID  BIGINT;        
----SELECT @PHD_CALL_UNMAPPED_ID = MAX(PHD_CALL_UNMAPPED_ID) FROM FOX_TBL_PHD_CALL_UNMAPPED;        
--EXEC @PHD_CALL_UNMAPPED_ID = [dbo].[Web_GetMaxColumnID] 'PHD_CALL_UNMAPPED_ID'        
        
     --  SET NOCOUNT ON;        
    INSERT INTO FOX_TBL_PHD_CALL_UNMAPPED        
              (        
     PHD_CALL_UNMAPPED_ID,         
     PRACTICE_CODE,         
     CALL_DATE,        
     CALL_START_TIME,         
     CALL_END_TIME,         
     CALL_NO,         
     CALL_BY,        
     IS_INCOMING,         
     OFFICE_NAME,         
     EXTENSION,         
     CALL_RECORDING_PATH,         
     CREATED_BY,         
     CREATED_DATE,         
     MODIFIED_BY,         
     MODIFIED_DATE,         
     DELETED)        
       VALUES        
              (@PHD_CALL_UNMAPPED_ID,        
     @PRACTICE_CODE,        
     --GETDATE(),        
     --GETDATE(),        
     --GETDATE(),        
     CAST(@CALL_DATE AS DATETIME),        
     CAST(@CALL_START_TIME AS DATETIME),        
     CAST(@CALL_END_TIME AS DATETIME),        
              @CALL_NO,        
     @CALL_BY,        
              @IS_INCOMING,        
              @OFFICE_NAME,        
              @EXTENSION,        
              @CALL_RECORDING_PATH,        
     'FOX TEAM',        
              GETDATE(),        
              'FOX TEAM',        
              GETDATE(),        
              0);        
        
     SELECT TOP 1 * FROM FOX_TBL_PHD_CALL_UNMAPPED ORDER BY CREATED_DATE        
END   
