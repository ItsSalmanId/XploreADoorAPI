IF (OBJECT_ID('FOX_PROC_GET_SURVEY_CALL_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SURVEY_CALL_LIST  
GO 
  --4
-- =============================================
-- AUTHOR:		<DEVELOPER, ABDUR RAFAY>
-- CREATE DATE: <CREATE DATE, 12/04/2018>
-- DESCRIPTION:	<GET SURVEY CALL LIST>

--EXEC FOX_PROC_GET_SURVEY_CALL_LIST 1011163,'Dev_544738',NULL,NULL

CREATE PROCEDURE [dbo].[FOX_PROC_GET_SURVEY_CALL_LIST] 
	 @PRACTICE_CODE BIGINT
	,@SURVEY_BY VARCHAR(100)
	,@DATE_FROM       DATETIME   
    ,@DATE_TO         DATETIME
	
AS
BEGIN
	SELECT SURVEY_CALL_ID
		,PRACTICE_CODE
		,ACU_CALL_ID
		,SURVEY_ID
		,PATIENT_ACCOUNT
		,FILE_NAME
		,IS_RECEIVED
		,CALL_DURATION
		,CREATED_BY
		,CREATED_DATE
		,MODIFIED_BY
		,MODIFIED_DATE
		,DELETED
	FROM FOX_TBL_PATIENT_SURVEY_CALL_LOG
	WHERE MODIFIED_BY = @SURVEY_BY
	    AND ISNULL(DELETED, 0) = 0
		AND ISNULL(IS_RECEIVED, 0) = 1
		AND PRACTICE_CODE = @PRACTICE_CODE
		AND (@DATE_FROM IS NULL  
              OR @DATE_TO IS NULL  
              OR CONVERT(DATE, CREATED_DATE) 
			  BETWEEN CONVERT(DATE, @DATE_FROM) 
			  AND CONVERT(DATE, @DATE_TO)) 
	ORDER BY CREATED_DATE DESC
END

