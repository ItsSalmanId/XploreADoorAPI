IF (OBJECT_ID('FOX_GET_TOTAL_DISCIPLINE') IS NOT NULL) DROP PROCEDURE FOX_GET_TOTAL_DISCIPLINE
GO
CREATE PROCEDURE [dbo].[FOX_GET_TOTAL_DISCIPLINE] -- 101116354444401377,1011163
	(
	--@DISCIPLINE_ID int,
	@PATIENT_ACCOUNT BIGINT
	,@PRACTICE_CODE BIGINT
	)
AS
BEGIN
	SELECT C.DISCIPLINE_ID
		,D.NAME
		,COUNT(C.DISCIPLINE_ID) AS TOTAL
	FROM FOX_TBL_CASE AS C
	JOIN FOX_TBL_DISCIPLINE AS D ON C.DISCIPLINE_ID = D.DISCIPLINE_ID --AND D.DISCIPLINE_ID = @DISCIPLINE_ID
		AND ISNULL(D.DELETED, 0) = 0
		AND C.PRACTICE_CODE = @PRACTICE_CODE
	WHERE ISNULL(C.DELETED, 0) = 0
		AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT
	GROUP BY C.DISCIPLINE_ID
		,D.NAME
END

