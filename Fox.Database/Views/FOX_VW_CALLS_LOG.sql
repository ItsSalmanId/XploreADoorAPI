USE [MIS_DB]
GO

/****** Object:  View [dbo].[FOX_VW_CALLS_LOG]    Script Date: 7/25/2022 1:59:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--------------------------------------------------------------------------------------------
--2:Alter View Table FOX_VW_CALLS_LOG
CREATE VIEW  [dbo].[FOX_VW_CALLS_LOG]
AS
SELECT cl.FOX_CALLS_LOG_ID
	,cl.PRACTICE_CODE
	,cl.CASE_ID
	,cl.FOX_CALL_TYPE_ID
	,clt.CALL_TYPE_NAME
	,cl.DISCHARGE_DATE
	,cl.PATIENT_STATUS
	,cl.GROUP_IDENTIFIER_ID
	,cl.RESULT_OF_CALL
	,cl.CALL_DATE
	,cl.CASE_NO
	,cl.ADMISSION_DATE
	,cts.NAME AS CASE_STATUS
	,cl.PROVIDER_ID
	,p.LAST_NAME + '.' + p.FIRST_NAME AS PROVIDER_NAME
	,cl.REGION_ID
	,cl.LOCATION_ID
	,cl.FOX_CALL_STATUS_ID
	,cl.FOX_CARE_STATUS_ID
	,cs.CALL_STATUS_NAME AS STATUS_OF_CALL
	,cr.CARE_STATUS_NAME AS STATUS_OF_CARE
	,cl.IS_WORK_CALL
	,cl.IS_CELL_CALL
	,cl.IS_HOME_CALL
	,r.FOX_CALL_RESULT_ID
	,r.CALL_RESULT_NAME
	,cl.COMMENTS
	,cl.REMARKABLE_REPORT_COMMENTS
	,cl.COMPLETED_DATE
	,cl.CREATED_BY
	,cl.CREATED_DATE
	,cl.MODIFIED_BY
	,cl.MODIFIED_DATE
	,cl.DELETED
FROM FOX_TBL_CALLS_LOG AS cl
LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE AS p ON p.SOURCE_ID = cl.PROVIDER_ID
LEFT JOIN FOX_TBL_COMMUNICATION_STATUS_OF_CARE AS cr ON cr.FOX_CARE_STATUS_ID = cl.FOX_CARE_STATUS_ID
LEFT JOIN FOX_TBL_COMMUNICATION_CALL_STATUS AS cs ON cs.FOX_CALL_STATUS_ID = cl.FOX_CALL_STATUS_ID
LEFT JOIN fox_tbl_case c ON c.CASE_ID = cl.CASE_ID
LEFT JOIN FOX_TBL_CASE_STATUS AS cts ON cts.CASE_STATUS_ID = c.CASE_STATUS_ID
LEFT JOIN FOX_TBL_COMMUNICATION_CALL_RESULT r ON r.FOX_CALL_RESULT_ID = cl.FOX_CALL_RESULT_ID
LEFT JOIN FOX_TBL_COMMUNICATION_CALL_TYPE clt ON cl.FOX_CALL_TYPE_ID = clt.FOX_CALL_TYPE_ID



GO

