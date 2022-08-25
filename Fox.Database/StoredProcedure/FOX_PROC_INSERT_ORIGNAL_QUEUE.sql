IF (OBJECT_ID('FOX_PROC_INSERT_ORIGNAL_QUEUE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_ORIGNAL_QUEUE  
GO 
-------------------------------------------------------------------------------  
-- =============================================                  
-- Author:  <Muhammad Imran>                  
-- Create date: <09/25/2019>                  
-- Description: <Description,,>                  
-- =============================================                  
-- FOX_PROC_INSERT_ORIGNAL_QUEUE_usama null, 54815220, 1011163, '1163testing', 0, 0,'"abdurrafay@mtbc.com"', 'email', 'created',0 ,'544103'                 
CREATE PROCEDURE FOX_PROC_INSERT_ORIGNAL_QUEUE           
 @ID BIGINT NULL,                 
 @WORK_ID BIGINT NULL,                  
 @PRACTICE_CODE BIGINT NULL,                  
 @CREATED_BY VARCHAR(200) NULL,                  
 @IS_EMERGENGENCY_ORDER BIT NULL,                  
 @SUPERVISOR_STATUS BIT NULL,                  
 @SORCE_NAME VARCHAR(200) NULL,                  
 @SORCE_TYPE VARCHAR(50) NULL,                  
 @WORK_STATUS VARCHAR(50) NULL,                  
 @IS_VERIFIED_BY_RECEPIENT BIT NULL,              
  @FOX_SOURCE_CATEGORY_ID BIGINT NULL                   
AS                  
BEGIN                  
 --DECLARE @ID   BIGINT             
             
 IF(@FOX_SOURCE_CATEGORY_ID = '0' OR @FOX_SOURCE_CATEGORY_ID = NULL)            
 BEGIN             
 SET @FOX_SOURCE_CATEGORY_ID = ''            
 END            
                      
 --                           
 IF @ID IS NOT NULL AND @ID <> 0                  
 BEGIN                  
 -- EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'WORK_ID', @ID output               
                    
  INSERT INTO FOX_TBL_WORK_QUEUE (WORK_ID, UNIQUE_ID, PRACTICE_CODE, CREATED_BY, CREATED_DATE, IS_EMERGENCY_ORDER, supervisor_status, DELETED, RECEIVE_DATE, SORCE_NAME, SORCE_TYPE, WORK_STATUS, IS_VERIFIED_BY_RECIPIENT, MODIFIED_BY, MODIFIED_DATE,FOX_SOURCE_CATEGORY_ID)                  
     VALUES (@ID, CONVERT(VARCHAR(50), @ID), @PRACTICE_CODE, @CREATED_BY, GETDATE(), @IS_EMERGENGENCY_ORDER, @SUPERVISOR_STATUS, 0, GETDATE(), @SORCE_NAME, @SORCE_TYPE, @WORK_STATUS, @IS_VERIFIED_BY_RECEPIENT, @CREATED_BY, GETDATE(),@FOX_SOURCE_CATEGORY_ID)                  
                  
  SET @WORK_ID = @ID                  
 END             
           
                  
 SELECT * FROM FOX_TBL_WORK_QUEUE           
 WHERE WORK_ID =  @WORK_ID                  
                     
                  
                  
END 