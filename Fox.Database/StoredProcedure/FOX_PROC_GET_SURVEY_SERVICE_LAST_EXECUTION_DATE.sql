IF (OBJECT_ID('FOX_PROC_GET_SURVEY_SERVICE_LAST_EXECUTION_DATE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SURVEY_SERVICE_LAST_EXECUTION_DATE  
GO   
CREATE PROC [dbo].[FOX_PROC_GET_SURVEY_SERVICE_LAST_EXECUTION_DATE]    
  AS    
  SELECT LAST_EXECUTION_DATE FROM [FOX_TBL_SURVEY_DATA_EXPORT_SERVICE_CONFIG]    
