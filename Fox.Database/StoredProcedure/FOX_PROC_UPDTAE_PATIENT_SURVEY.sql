IF (OBJECT_ID('FOX_PROC_UPDTAE_PATIENT_SURVEY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDTAE_PATIENT_SURVEY  
GO 
-------------------------------------------------------------- [FOX_PROC_UPDTAE_PATIENT_SURVEY] -------------------------------------------------------------------      
-- =============================================                            
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>                            
-- CREATE DATE: <CREATE DATE, 05/11/2020>                            
-- DESCRIPTION: <UPDTAE PATIENT SURVEY>                            
                            
 --EXEC FOX_PROC_UPDTAE_PATIENT_SURVEY                     
 --    101116354446569,                       
 --    1011163,                      
 --    'FACILITY_OR_CLIENT_ID',                      
 --     1011,                      
 --    'RESPONSIBLE_PARTY_LAST_NAME',                      
 --    'RESPONSIBLE_PARTY_FIRST_NAME',                      
 --    'A',                      
 --    'RESPONSIBLE_PARTY_ADDRESS',                      
 --    'RESPONSIBLE_PARTY_CITY',                      
 --    'ST  ',                      
 --    'ZIPCODE',                      
 --    'PHONE' ,                      
 --    'SSN',                    
 -- 'SEX',                    
 -- '10/22/2019 3:17:09 AM',                    
 -- 'PATIENT_LAST_NAME',                    
 -- 'PATIENT_FIRST_NAME',                    
 -- 'A',                    
 -- 'PATIENT_ADDRESS',                    
 -- 'PATIENT_CITY',                    
 -- 'ST',                    
 -- 'ZIP',                    
 -- 'PHONE',                    
 -- 'SSN',                    
 -- 'GENDER',                    
 -- '10/22/2019 3:17:09 AM',                    
 -- 'ALTERNATE_CONTACT_LAST_NAME',                    
 -- 'ALTERNATE_CONTACT_FIRST_NAME',                    
 -- 'T',                    
 -- 'Phone',                    
 -- 'EMR_LOCATION_CODE',                    
 -- 'EMR_LOCATION_DESCRIPTION',                    
 -- 'SERVICE_OR_PAYMENT_DESCRIPTION',                    
 -- 'PROVIDER',                    
 -- 'REGION',                    
 -- '10/22/2019 3:17:09 AM',                    
 -- '10/22/2019 3:17:09 AM',                    
 -- 'ATTENDING_DOCTOR_NAME',                    
 -- 'PT_OT_SLP',                    
 -- '10/22/2019 3:17:09 AM',                    
 -- 'PROCEDURE_OR_TRAN_CODE',                    
 --  2.2,                    
 --  1,                    
 --  1,                    
 --  1,                    
 --  1,                    
 --  1,                    
 -- 'FEEDBACK',                    
 -- 'SURVEY_FLAG',                    
 -- 'SURVEY_STATUS_BASE',                    
 -- 'SURVEY_STATUS_CHILD',                    
 -- 'SURVEY_FORMAT_TYPE',                    
 --  0,                    
 --  0,                    
 -- 'FILE_NAME',                    
 -- 'SHEET_NAME',                    
 -- 20,                    
 --    'RAFAY',                    
 --'10/22/2019 3:17:09 AM',                    
 -- 'RAFAY',                    
 -- '10/22/2019 3:17:09 AM',                    
 --  0     
 --  10/22/2020 3:17:09 AM                      
                                                    
CREATE PROCEDURE [dbo].[FOX_PROC_UPDTAE_PATIENT_SURVEY]                        
@SURVEY_ID BIGINT NULL                    
,@PRACTICE_CODE BIGINT NULL                    
,@FACILITY_OR_CLIENT_ID VARCHAR(100) NULL                    
,@PATIENT_ACCOUNT_NUMBER BIGINT NULL                    
,@RESPONSIBLE_PARTY_LAST_NAME VARCHAR(50) NULL                    
,@RESPONSIBLE_PARTY_FIRST_NAME VARCHAR(50) NULL                    
,@RESPONSIBLE_PARTY_MIDDLE_INITIAL CHAR(1) NULL                    
,@RESPONSIBLE_PARTY_ADDRESS VARCHAR(500) NULL                    
,@RESPONSIBLE_PARTY_CITY VARCHAR(50) NULL                    
,@RESPONSIBLE_PARTY_STATE CHAR(2) NULL                    
,@RESPONSIBLE_PARTY_ZIP_CODE VARCHAR(10) NULL                    
,@RESPONSIBLE_PARTY_TELEPHONE VARCHAR(20) NULL                    
,@RESPONSIBLE_PARTY_SSN CHAR(20) NULL                    
,@RESPONSIBLE_PARTY_SEX VARCHAR(15) NULL                    
,@RESPONSIBLE_PARTY_DATE_OF_BIRTH DATETIME NULL                    
,@PATIENT_LAST_NAME VARCHAR(50) NULL                    
,@PATIENT_FIRST_NAME VARCHAR(50) NULL                    
,@PATIENT_MIDDLE_INITIAL CHAR(1) NULL                    
                    
,@PATIENT_ADDRESS VARCHAR(500) NULL                    
,@PATIENT_CITY VARCHAR(50) NULL                    
,@PATIENT_STATE CHAR(2) NULL                    
,@PATIENT_ZIP_CODE VARCHAR(20) NULL                    
,@PATIENT_TELEPHONE_NUMBER VARCHAR(20) NULL                    
,@PATIENT_SOCIAL_SECURITY_NUMBER CHAR(20) NULL                    
,@PATIENT_GENDER VARCHAR(15) NULL                    
,@PATIENT_DATE_OF_BIRTH DATETIME NULL                    
,@ALTERNATE_CONTACT_LAST_NAME VARCHAR(50) NULL                    
,@ALTERNATE_CONTACT_FIRST_NAME VARCHAR(50) NULL                    
,@ALTERNATE_CONTACT_MIDDLE_INITIAL CHAR(1) NULL                    
,@ALTERNATE_CONTACT_TELEPHONE VARCHAR(10) NULL                    
,@EMR_LOCATION_CODE VARCHAR(50) NULL                     
,@EMR_LOCATION_DESCRIPTION VARCHAR(200) NULL                    
,@SERVICE_OR_PAYMENT_DESCRIPTION VARCHAR(200) NULL                    
,@PROVIDER VARCHAR(50) NULL                     
,@REGION VARCHAR(50) NULL                    
,@LAST_VISIT_DATE DATETIME NULL                    
                    
,@DISCHARGE_DATE DATETIME NULL                    
,@ATTENDING_DOCTOR_NAME VARCHAR(50) NULL                    
,@PT_OT_SLP VARCHAR(10) NULL                    
,@REFERRAL_DATE DATETIME NULL                     
,@PROCEDURE_OR_TRAN_CODE VARCHAR(50) NULL                    
,@SERVICE_OR_PAYMENT_AMOUNT MONEY NULL                    
,@IS_CONTACT_HQ BIT NULL                    
,@IS_RESPONSED_BY_HQ BIT NULL                    
,@IS_QUESTION_ANSWERED BIT NULL                    
,@IS_REFERABLE BIT NULL                    
,@IS_IMPROVED_SETISFACTION BIT NULL                    
,@FEEDBACK VARCHAR(3000) NULL                    
,@SURVEY_FLAG VARCHAR(50) NULL                    
,@SURVEY_STATUS_BASE VARCHAR(50) NULL                    
,@SURVEY_STATUS_CHILD VARCHAR(50) NULL                    
,@SURVEY_FORMAT_TYPE VARCHAR(100) NULL                    
,@IS_SURVEYED BIT NULL                     
,@IN_PROGRESS BIT NULL                    
,@FILE_NAME VARCHAR(200) NULL                    
,@SHEET_NAME VARCHAR(200) NULL                    
                    
,@TOTAL_RECORD_IN_FILE BIGINT NULL                    
,@CREATED_BY VARCHAR(70) NULL                    
,@CREATED_DATE DATETIME NULL                    
,@MODIFIED_BY VARCHAR(70) NULL                    
,@MODIFIED_DATE DATETIME NULL                    
,@DELETED BIT NULL                
,@IS_EXCEPTIONAL BIT               
,@IS_PROTECTIVE_EQUIPMENT BIT            
,@SURVEY_COMPLETED_DATE DATETIME NULL                
                    
AS                       
BEGIN                            
                          
UPDATE FOX_TBL_PATIENT_SURVEY                      
SET                    
 SURVEY_ID = @SURVEY_ID                                                
,PRACTICE_CODE = @PRACTICE_CODE                    
,FACILITY_OR_CLIENT_ID = @FACILITY_OR_CLIENT_ID                    
,PATIENT_ACCOUNT_NUMBER = @PATIENT_ACCOUNT_NUMBER                    
,RESPONSIBLE_PARTY_LAST_NAME = @RESPONSIBLE_PARTY_LAST_NAME                    
,RESPONSIBLE_PARTY_FIRST_NAME = @RESPONSIBLE_PARTY_FIRST_NAME                    
,RESPONSIBLE_PARTY_MIDDLE_INITIAL = @RESPONSIBLE_PARTY_MIDDLE_INITIAL                    
,RESPONSIBLE_PARTY_ADDRESS = @RESPONSIBLE_PARTY_ADDRESS                    
,RESPONSIBLE_PARTY_CITY = @RESPONSIBLE_PARTY_CITY                    
,RESPONSIBLE_PARTY_STATE = @RESPONSIBLE_PARTY_STATE                    
,RESPONSIBLE_PARTY_ZIP_CODE = @RESPONSIBLE_PARTY_ZIP_CODE                    
,RESPONSIBLE_PARTY_TELEPHONE = @RESPONSIBLE_PARTY_TELEPHONE                    
,RESPONSIBLE_PARTY_SSN = @RESPONSIBLE_PARTY_SSN                    
,RESPONSIBLE_PARTY_SEX = @RESPONSIBLE_PARTY_SEX                    
,RESPONSIBLE_PARTY_DATE_OF_BIRTH = CAST(@RESPONSIBLE_PARTY_DATE_OF_BIRTH AS DATETIME)                    
,PATIENT_LAST_NAME = @PATIENT_LAST_NAME                    
,PATIENT_FIRST_NAME = @PATIENT_FIRST_NAME   
,PATIENT_MIDDLE_INITIAL = @PATIENT_MIDDLE_INITIAL                    
                    
,PATIENT_ADDRESS = @PATIENT_ADDRESS                    
,PATIENT_CITY = @PATIENT_CITY                    
,PATIENT_STATE = @PATIENT_STATE                    
,PATIENT_ZIP_CODE = @PATIENT_ZIP_CODE                    
,PATIENT_TELEPHONE_NUMBER = @PATIENT_TELEPHONE_NUMBER                    
,PATIENT_SOCIAL_SECURITY_NUMBER = @PATIENT_SOCIAL_SECURITY_NUMBER                    
,PATIENT_GENDER = @PATIENT_GENDER                    
,PATIENT_DATE_OF_BIRTH = CAST(@PATIENT_DATE_OF_BIRTH AS DATETIME)                    
,ALTERNATE_CONTACT_LAST_NAME = @ALTERNATE_CONTACT_LAST_NAME                    
,ALTERNATE_CONTACT_FIRST_NAME = @ALTERNATE_CONTACT_FIRST_NAME                    
,ALTERNATE_CONTACT_MIDDLE_INITIAL = @ALTERNATE_CONTACT_MIDDLE_INITIAL                    
,ALTERNATE_CONTACT_TELEPHONE = @ALTERNATE_CONTACT_TELEPHONE                    
,EMR_LOCATION_CODE = @EMR_LOCATION_CODE                    
,EMR_LOCATION_DESCRIPTION = @EMR_LOCATION_DESCRIPTION                    
,SERVICE_OR_PAYMENT_DESCRIPTION = @SERVICE_OR_PAYMENT_DESCRIPTION                    
,PROVIDER = @PROVIDER                     
,REGION = @REGION                    
,LAST_VISIT_DATE = CAST(@LAST_VISIT_DATE AS DATETIME)                     
                    
,DISCHARGE_DATE = CAST(@DISCHARGE_DATE AS DATETIME)                    
,ATTENDING_DOCTOR_NAME = @ATTENDING_DOCTOR_NAME                    
,PT_OT_SLP = @PT_OT_SLP                    
,REFERRAL_DATE = CAST(@REFERRAL_DATE AS DATETIME)                   
,PROCEDURE_OR_TRAN_CODE = @PROCEDURE_OR_TRAN_CODE                    
,SERVICE_OR_PAYMENT_AMOUNT = @SERVICE_OR_PAYMENT_AMOUNT                    
,IS_CONTACT_HQ = @IS_CONTACT_HQ                    
,IS_RESPONSED_BY_HQ = @IS_RESPONSED_BY_HQ                    
,IS_QUESTION_ANSWERED = @IS_QUESTION_ANSWERED                    
,IS_REFERABLE = @IS_REFERABLE                    
,IS_IMPROVED_SETISFACTION = @IS_IMPROVED_SETISFACTION                    
,FEEDBACK = @FEEDBACK                    
,SURVEY_FLAG = @SURVEY_FLAG                    
,SURVEY_STATUS_BASE = @SURVEY_STATUS_BASE                    
,SURVEY_STATUS_CHILD = @SURVEY_STATUS_CHILD                    
,SURVEY_FORMAT_TYPE = @SURVEY_FORMAT_TYPE                    
,IS_SURVEYED = @IS_SURVEYED                    
,IN_PROGRESS = @IN_PROGRESS                    
,FILE_NAME = @FILE_NAME                    
,SHEET_NAME = @SHEET_NAME                    
                    
,TOTAL_RECORD_IN_FILE = @TOTAL_RECORD_IN_FILE                    
,CREATED_BY = @CREATED_BY                    
,CREATED_DATE = CAST(@CREATED_DATE AS DATETIME)                    
,MODIFIED_BY = @MODIFIED_BY                    
,MODIFIED_DATE =  CAST(@MODIFIED_DATE AS DATETIME)                    
,DELETED = @DELETED                
,IS_EXCEPTIONAL = @IS_EXCEPTIONAL              
,IS_PROTECTIVE_EQUIPMENT = @IS_PROTECTIVE_EQUIPMENT            
,SURVEY_COMPLETED_DATE = CAST(@SURVEY_COMPLETED_DATE  AS DATETIME)           
WHERE SURVEY_ID = @SURVEY_ID                    
END 
