IF (OBJECT_ID('FOX_PROC_GET_PATIENT_DOCUMENT_TYPES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_DOCUMENT_TYPES  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_DOCUMENT_TYPES] --'101116354444401377','1011163'  
(@PATIENT_ACCOUNT BIGINT,
 @PRACTICE_CODE BIGINT
)    
AS    
BEGIN    
    SELECT *
	FROM FOX_TBL_DOCUMENT_TYPE
	WHERE DOCUMENT_TYPE_ID IN
	(
		SELECT DISTINCT 
			   DOCUMENT_TYPE
		FROM FOX_TBL_WORK_QUEUE
		WHERE ISNULL(DOCUMENT_TYPE, 0) <> 0
			  AND DELETED <> 1
			  AND PRACTICE_CODE = @PRACTICE_CODE
			  AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT
		UNION ALL
		SELECT DOCUMENT_TYPE
		FROM FOX_TBL_PATIENT_PAT_DOCUMENT
		WHERE ISNULL(DOCUMENT_TYPE, 0) <> 0
			  AND DELETED <> 1
			  AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT
	)
	AND DELETED <> 1
END

