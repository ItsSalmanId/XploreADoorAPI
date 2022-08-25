IF (OBJECT_ID('FOX_PROC_INSERT_ORIGNAL_QUEUE_ADDITIONAL_INFO') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_ORIGNAL_QUEUE_ADDITIONAL_INFO  
GO 
CREATE PROCEDURE FOX_PROC_INSERT_ORIGNAL_QUEUE_ADDITIONAL_INFO                 
 @WORK_ID BIGINT NULL,                  
 @DEPARTMENT_ID VARCHAR(50) NULL,                  
 @DOCUMENT_TYPE BIGINT ,                  
 @SENDER_ID BIGINT NULL,                  
 @FACILITY_NAME VARCHAR(100) NULL,                  
 @INDEXED_BY VARCHAR(70) NULL,                  
 @INDEXED_DATE DATETIME NULL,              
 @COMPLETED_BY VARCHAR(70) NULL,                 
 @COMPLETED_DATE DATETIME NULL,                  
 @WORK_STATUS VARCHAR(15) NULL,                  
 @PATIENT_ACCOUNT BIGINT NULL,                  
 @ACCOUNT_NUMBER VARCHAR(100) NULL,                  
 @FACILITY_ID BIGINT NULL,                  
 @IS_EMERGENCY_ORDER BIT NULL,                  
 @REASON_FOR_VISIT VARCHAR(1000) NULL,                  
 @UNIT_CASE_NO VARCHAR(100) NULL,                  
 @DELETED BIT NULL,                  
 @MODIFIED_BY VARCHAR(70) NULL,                  
 @RFO_Type VARCHAR(50) NULL,                  
 @IsSigned BIT NULL,                  
 @SignedBy BIGINT NULL,                  
 @FOX_TBL_SENDER_TYPE_ID BIGINT NULL,                  
 @FOX_TBL_SENDER_NAME_ID BIGINT NULL,                  
 @REASON_FOR_THE_URGENCY VARCHAR(200) NULL,                  
 @IS_POST_ACUTE BIT NULL,                  
 @ASSIGNED_TO VARCHAR(70) NULL,                  
 @ASSIGNED_BY VARCHAR(70) NULL,                  
 @ASSIGNED_DATE DATETIME NULL,                  
 @IS_EVALUATE_TREAT BIT NULL,                  
 @HEALTH_NAME VARCHAR(100) NULL,                  
 @HEALTH_NUMBER VARCHAR(100) NULL,                  
 @IS_VERBAL_ORDER BIT NULL,                  
 @VO_ON_BEHALF_OF BIGINT NULL,                  
 @VO_RECIEVED_BY VARCHAR(70) NULL,                  
 @VO_DATE_TIME DATETIME NULL,          
 @EXPECTED_DISCHARGE_DATE DATETIME NULL,        
 @FOX_SOURCE_CATEGORY_ID BIGINT NULL                 
AS                  
BEGIN                  
                   
 IF (@DEPARTMENT_ID IS NULL OR LTRIM(RTRIM(@DEPARTMENT_ID)) = '')                  
 BEGIN                  
  SET @DEPARTMENT_ID = NULL                  
 END                  
                   
 IF (@DOCUMENT_TYPE IS NULL OR @DOCUMENT_TYPE= 0)                  
 BEGIN                  
  SET @DOCUMENT_TYPE = NULL                  
 END                  
                  
 IF (@SENDER_ID IS NULL OR @SENDER_ID= 0)                  
 BEGIN                  
  SET @SENDER_ID = NULL                  
 END                  
                   
 IF (@FACILITY_NAME IS NULL OR LTRIM(RTRIM(@FACILITY_NAME)) = '')                  
 BEGIN                  
  SET @FACILITY_NAME = NULL                  
 END                  
                   
 IF (@INDEXED_DATE IS NULL OR @INDEXED_DATE = '')                  
 BEGIN                  
  SET @INDEXED_DATE = NULL                  
 END                  
                  
 IF (@COMPLETED_BY IS NULL OR LTRIM(RTRIM(@COMPLETED_BY)) = '')                  
 BEGIN                  
  SET @COMPLETED_BY = NULL                  
 END                  
                  
 IF (@COMPLETED_DATE IS NULL OR @COMPLETED_DATE = '')                  
 BEGIN                  
  SET @COMPLETED_DATE = NULL                  
 END                  
                  
 IF (@WORK_STATUS IS NULL OR LTRIM(RTRIM(@WORK_STATUS)) = '')                  
 BEGIN                  
  SET @WORK_STATUS = NULL                  
 END                  
                  
 IF (@PATIENT_ACCOUNT IS NULL OR @PATIENT_ACCOUNT= 0)                  
 BEGIN                  
  SET @PATIENT_ACCOUNT = NULL                  
 END                  
                  
 IF (@ACCOUNT_NUMBER IS NULL OR LTRIM(RTRIM(@ACCOUNT_NUMBER)) = '')                  
 BEGIN                  
  SET @ACCOUNT_NUMBER = NULL                  
 END                  
                  
 IF (@FACILITY_ID IS NULL OR @FACILITY_ID= 0)                  
 BEGIN                  
  SET @FACILITY_ID = NULL                  
 END                  
                  
 IF (@REASON_FOR_VISIT IS NULL OR LTRIM(RTRIM(@REASON_FOR_VISIT)) = '')      
 BEGIN                  
  SET @REASON_FOR_VISIT = NULL                  
 END                  
                  
 IF (@UNIT_CASE_NO IS NULL OR LTRIM(RTRIM(@UNIT_CASE_NO)) = '')                  
 BEGIN                  
  SET @UNIT_CASE_NO = NULL                  
 END                  
                  
 IF (@RFO_Type IS NULL OR LTRIM(RTRIM(@RFO_Type)) = '')                  
 BEGIN                  
  SET @RFO_Type = NULL                  
 END                  
                  
 IF (@IsSigned IS NULL )                  
 BEGIN                  
  SET @IsSigned = NULL                  
 END                  
                  
 IF (@SignedBy IS NULL OR @SignedBy = 0)                  
 BEGIN                  
  SET @SignedBy = NULL                  
 END                  
                  
 IF (@FOX_TBL_SENDER_TYPE_ID IS NULL OR @FOX_TBL_SENDER_TYPE_ID = 0)                  
 BEGIN                  
  SET @FOX_TBL_SENDER_TYPE_ID = NULL                  
 END                  
                  
 IF (@FOX_TBL_SENDER_NAME_ID IS NULL OR @FOX_TBL_SENDER_NAME_ID = 0)                  
 BEGIN                  
  SET @FOX_TBL_SENDER_NAME_ID = NULL                  
 END                  
                  
 IF (@REASON_FOR_THE_URGENCY IS NULL OR LTRIM(RTRIM(@REASON_FOR_THE_URGENCY)) = '')                  
 BEGIN                  
  SET @REASON_FOR_THE_URGENCY = NULL                  
 END                  
                  
 IF (@IS_POST_ACUTE IS NULL )                  
 BEGIN                  
  SET @IS_POST_ACUTE = NULL                  
 END                  
                  
 IF (@ASSIGNED_TO IS NULL OR LTRIM(RTRIM(@ASSIGNED_TO)) = '')                  
 BEGIN                  
  SET @ASSIGNED_TO = NULL                  
 END                  
                  
 IF (@ASSIGNED_BY IS NULL OR LTRIM(RTRIM(@ASSIGNED_BY)) = '')                  
 BEGIN                  
  SET @ASSIGNED_BY = NULL                  
 END                  
                  
 IF (@ASSIGNED_DATE IS NULL OR @ASSIGNED_DATE = '')                  
 BEGIN                  
  SET @ASSIGNED_DATE = NULL                  
 END                  
                  
 IF (@IS_EVALUATE_TREAT IS NULL )                  
 BEGIN                  
  SET @IS_EVALUATE_TREAT = NULL                  
 END                  
                  
 IF (@HEALTH_NAME IS NULL OR LTRIM(RTRIM(@HEALTH_NAME)) = '')                  
 BEGIN                  
  SET @HEALTH_NAME = NULL                  
 END                  
                  
 IF (@HEALTH_NUMBER IS NULL OR LTRIM(RTRIM(@HEALTH_NUMBER)) = '')                  
 BEGIN                  
  SET @HEALTH_NUMBER = NULL                  
 END                  
                  
 IF (@IS_VERBAL_ORDER IS NULL )                  
 BEGIN                  
  SET @IS_VERBAL_ORDER = NULL                  
 END                  
                  
 IF (@VO_ON_BEHALF_OF IS NULL OR @VO_ON_BEHALF_OF = 0)                  
 BEGIN                  
  SET @VO_ON_BEHALF_OF = NULL                  
 END                  
                  
 IF (@VO_RECIEVED_BY IS NULL OR @VO_RECIEVED_BY = '')                  
 BEGIN                  
  SET @VO_RECIEVED_BY = NULL                  
 END                  
                  
 IF (@VO_DATE_TIME IS NULL OR @VO_DATE_TIME = '')                  
 BEGIN                  
  SET @VO_DATE_TIME = NULL                  
 END             
      
 IF (@EXPECTED_DISCHARGE_DATE IS NULL OR @EXPECTED_DISCHARGE_DATE = '')                  
 BEGIN                  
  SET @EXPECTED_DISCHARGE_DATE = NULL                  
 END         
      
 IF (@FOX_SOURCE_CATEGORY_ID IS NULL OR @FOX_SOURCE_CATEGORY_ID = '' OR @FOX_SOURCE_CATEGORY_ID = 0)            
 BEGIN            
 SET @FOX_SOURCE_CATEGORY_ID = NULL            
   END               
                  
                  
 --                        
         
 IF EXISTS(SELECT TOP 1 * FROM FOX_TBL_WORK_QUEUE WHERE WORK_ID = @WORK_ID AND ISNULL(DELETED,0) = 0)                  
 BEGIN                  
  UPDATE FOX_TBL_WORK_QUEUE                  
  SET                  
   DEPARTMENT_ID = @DEPARTMENT_ID,                  
   DOCUMENT_TYPE = @DOCUMENT_TYPE,                  
   FACILITY_NAME = @FACILITY_NAME,                  
   SENDER_ID = @SENDER_ID,                  
   INDEXED_BY = @INDEXED_BY,                  
   INDEXED_DATE = @INDEXED_DATE,                  
   COMPLETED_BY = @COMPLETED_BY,                  
   COMPLETED_DATE = @COMPLETED_DATE,                  
   WORK_STATUS = @WORK_STATUS,                  
   PATIENT_ACCOUNT = @PATIENT_ACCOUNT,                  
   ACCOUNT_NUMBER = @ACCOUNT_NUMBER,                  
   FACILITY_ID = @FACILITY_ID,                  
   IS_EMERGENCY_ORDER = @IS_EMERGENCY_ORDER,                  
   REASON_FOR_VISIT = @REASON_FOR_VISIT,                  
   UNIT_CASE_NO = @UNIT_CASE_NO,                  
   DELETED = @DELETED,                  
   MODIFIED_BY = @MODIFIED_BY,                  
   MODIFIED_DATE = GETDATE(),                  
   RFO_Type = @RFO_Type,           
   IsSigned = @IsSigned,                  
   SignedBy = @SignedBy,                  
   FOX_TBL_SENDER_TYPE_ID = @FOX_TBL_SENDER_TYPE_ID,                  
   FOX_TBL_SENDER_NAME_ID = @FOX_TBL_SENDER_NAME_ID,                  
   REASON_FOR_THE_URGENCY = @REASON_FOR_THE_URGENCY,                  
   IS_POST_ACUTE = @IS_POST_ACUTE,                  
   ASSIGNED_TO = @ASSIGNED_TO,                  
   ASSIGNED_BY = @ASSIGNED_BY,                  
   ASSIGNED_DATE = @ASSIGNED_DATE,                  
   IS_EVALUATE_TREAT = @IS_EVALUATE_TREAT,                  
   HEALTH_NAME = @HEALTH_NAME,                  
   HEALTH_NUMBER = @HEALTH_NUMBER,                  
   IS_VERBAL_ORDER = @IS_VERBAL_ORDER,                  
   VO_ON_BEHALF_OF =@VO_ON_BEHALF_OF,                  
   VO_RECIEVED_BY = @VO_RECIEVED_BY,                  
   VO_DATE_TIME = @VO_DATE_TIME,          
   EXPECTED_DISCHARGE_DATE= @EXPECTED_DISCHARGE_DATE,       
   FOX_SOURCE_CATEGORY_ID = @FOX_SOURCE_CATEGORY_ID                  
  WHERE WORK_ID = @WORK_ID                  
 END                  
                  
 SELECT * FROM FOX_TBL_WORK_QUEUE                   
 WHERE WORK_ID = @WORK_ID                  
                     
                  
                  
END 