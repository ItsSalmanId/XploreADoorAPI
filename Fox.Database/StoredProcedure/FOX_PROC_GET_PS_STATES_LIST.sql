IF (OBJECT_ID('FOX_PROC_GET_PS_STATES_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PS_STATES_LIST  
GO 
-- =============================================
-- AUTHOR:		<DEVELOPER, YOUSAF>
-- CREATE DATE: <CREATE DATE, 05/03/2018>
-- DESCRIPTION:	<GET STATES LIST>
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PS_STATES_LIST] --''
	--(@SEARCH_TEXT VARCHAR(100))
AS
BEGIN
	SELECT STATE_CODE --, STATE_NAME
	FROM STATES
	WHERE ISNULL(DELETED, 0) = 0
		--AND STATE_CODE LIKE '%' + @SEARCH_TEXT + '%'
	ORDER BY State_Code
END
