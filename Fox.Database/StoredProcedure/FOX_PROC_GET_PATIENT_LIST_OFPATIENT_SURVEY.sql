IF (OBJECT_ID('FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY  
GO            
--[DBO].[FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY] 1012714, '2045', 1            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY] --1011163, '2045', 0,'06/10/2019','07/10/2019'              
 @PRACTICE_CODE BIGINT              
 ,@PATIENT_ACCOUNT VARCHAR(100)              
 ,@IS_INCLUDE_SURVEYED BIT              
              
AS              
BEGIN              
 DECLARE @DOB DATETIME              
              
 IF ISDATE(@PATIENT_ACCOUNT) = 1              
 BEGIN              
  SET @DOB = CONVERT(DATETIME, @PATIENT_ACCOUNT)              
 END              
 ELSE              
 BEGIN              
  SET @DOB = NULL              
 END              
              
 SELECT DISTINCT PATIENT_ACCOUNT_NUMBER              
  ,PATIENT_FIRST_NAME              
  ,PATIENT_LAST_NAME              
  ,PATIENT_MIDDLE_INITIAL              
  ,PATIENT_CITY              
  ,PATIENT_STATE              
  ,PATIENT_ZIP_CODE              
  ,PATIENT_GENDER            
  ,SURVEY_STATUS_CHILD        
 --,SURVEY_ID          
  ,IS_SURVEYED              
  ,CONVERT(INT, ROUND(DATEDIFF(hour, PATIENT_DATE_OF_BIRTH, GETDATE()) / 8766.0, 0)) AS PATIENT_AGE              
  ,PATIENT_DATE_OF_BIRTH              
 FROM FOX_TBL_PATIENT_SURVEY              
 WHERE ISNULL(DELETED, 0) = 0              
  AND ISNULL(IN_PROGRESS, 0) = 0              
  AND PRACTICE_CODE = @PRACTICE_CODE              
  AND              
  (              
   @IS_INCLUDE_SURVEYED = 1              
   OR ISNULL(IS_SURVEYED, 0) = @IS_INCLUDE_SURVEYED              
  )              
  AND @PATIENT_ACCOUNT IS NOT NULL              
  AND              
  (              
   CAST(PATIENT_ACCOUNT_NUMBER AS VARCHAR(30)) LIKE '%' + @PATIENT_ACCOUNT + '%'              
   OR PATIENT_FIRST_NAME LIKE '%' + @PATIENT_ACCOUNT + '%'              
   OR PATIENT_LAST_NAME LIKE '%' + @PATIENT_ACCOUNT + '%'              
   OR              
   (              
    @DOB IS NOT NULL              
    AND PATIENT_DATE_OF_BIRTH = @DOB              
   )              
  )              
  AND PATIENT_ACCOUNT_NUMBER IS NOT NULL               
END 
