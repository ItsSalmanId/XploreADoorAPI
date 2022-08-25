IF (OBJECT_ID('FOX_PROC_GET_PSD_RESULTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PSD_RESULTS  
GO       
-- =============================================              
-- AUTHOR:  <DEVELOPER, YOUSAF>              
-- CREATE DATE: <CREATE DATE, 05/01/2018>              
-- DESCRIPTION: <GET PATIENT SURVEY DASHBOARD; DASHBOARD DATA RESULTS>              
              
-- [DBO].[FOX_PROC_GET_PSD_RESULTS] 1011163, '2021-01-10 16:54:32.117', '2021-11-16 16:54:32.117', '', '', '', 'ALL'             
             
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PSD_RESULTS]              
 @PRACTICE_CODE BIGINT              
 ,@DATE_FROM DATETIME              
 ,@DATE_TO DATETIME              
 ,@REGION VARCHAR(100)              
 ,@STATE VARCHAR(2)              
 ,@DISCIPLINE VARCHAR(2)              
 ,@FORMAT VARCHAR(10)              
AS              
BEGIN              
IF (@FORMAT = 'ALL')              
BEGIN              
SET @FORMAT = 'FORMAT'              
END              
 SELECT (              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'NOT CONVERTED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND SURVEY_STATUS_CHILD = 'NOT INTERESTED'              
   ) AS NOT_INTERESTED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'INCOMPLETE'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' +@FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND SURVEY_STATUS_CHILD = 'CALLBACK'              
   ) AS CALLBACK              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'INCOMPLETE'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND SURVEY_STATUS_CHILD = 'NOT ANSWERED'              
   ) AS NOT_ANSWERED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND SURVEY_STATUS_CHILD = 'DECEASED'              
   ) AS DECEASED              
 ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1            
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
   ) AS COMPLETED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'INCOMPLETE'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
   ) AS INCOMPLETED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'NOT CONVERTED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
   ) AS NOT_CONVERTED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_REFERABLE = 1              
   ) AS RECOMMENDED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_REFERABLE = 0            
   ) AS NOT_RECOMMENDED              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'         
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_IMPROVED_SETISFACTION = 1              
   ) AS IS_IMPROVED_SETISFACTION_YES              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_IMPROVED_SETISFACTION = 0              
   ) AS IS_IMPROVED_SETISFACTION_NO              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_REFERABLE = 1              
   ) AS IS_REFERABLE_YES              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_REFERABLE = 0              
   ) AS IS_REFERABLE_NO              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)          
    AND IS_CONTACT_HQ = 1              
   ) AS IS_CONTACT_HQ_YES              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_CONTACT_HQ = 0              
   ) AS IS_CONTACT_HQ_NO              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0       
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_CONTACT_HQ = 1              
    AND IS_RESPONSED_BY_HQ = 1              
   ) AS IS_RESPONSED_BY_HQ_YES              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
 AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_CONTACT_HQ = 1              
    AND IS_RESPONSED_BY_HQ = 0              
   ) AS IS_RESPONSED_BY_HQ_NO              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND SURVEY_FORMAT_TYPE = 'NEW FORMAT'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_CONTACT_HQ = 1              
    AND IS_QUESTION_ANSWERED = 1              
   ) AS IS_QUESTION_ANSWERED_YES              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND SURVEY_FORMAT_TYPE = 'NEW FORMAT'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_CONTACT_HQ = 1              
    AND IS_QUESTION_ANSWERED = 0              
   ) AS IS_QUESTION_ANSWERED_NO             
   ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_PROTECTIVE_EQUIPMENT = 1              
   ) AS IS_PROTECTIVE_EQUIPMENT_YES              
  ,(              
   SELECT COUNT(*)              
   FROM FOX_TBL_PATIENT_SURVEY              
   WHERE ISNULL(DELETED, 0) = 0              
    AND ISNULL(IS_SURVEYED, 0) = 1              
    AND PRACTICE_CODE = @PRACTICE_CODE              
    AND SURVEY_STATUS_BASE = 'COMPLETED'              
    AND REGION <> ''              
    AND REGION LIKE '%' + @REGION + '%'              
    AND PATIENT_STATE LIKE '%' + @STATE + '%'              
    AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'              
    AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'              
    AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)              
     AND CONVERT(DATE, @DATE_TO)              
    AND IS_PROTECTIVE_EQUIPMENT = 0              
   ) AS IS_PROTECTIVE_EQUIPMENT_NO              
             
END       
