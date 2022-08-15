IF (OBJECT_ID('FOX_PROC_AUTHENTICATE_USER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_AUTHENTICATE_USER
GO
---------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,Mehmood ul Hassan>
-- Create date: <Create Date,12/10/2017>
-- Description:	<Description,Authenticate User on login>
-- =============================================
CREATE PROCEDURE [dbo].[FOX_PROC_AUTHENTICATE_USER] --'muhammadsaleem1@mtbc.com' , '495C6CEBDC4B779EBC371DBEE8C5712E8EF1868540AF049E8A34D75445E2012D00E110A8D12CED59DD66415F1C0377E618CF073ADCABD720038E5BB678CAB313'
	-- Add the parameters for the stored procedure here
	@USERNAME VARCHAR(255)
	,@PASSWORD VARCHAR(5000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from 
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE @LOCKEDUSER BIT
		,@USER BIT
		,@PASS BIT
		,@STATUS INT

	SELECT @LOCKEDUSER = COUNT(USER_NAME)
	FROM fox_tbl_application_user U
	INNER JOIN PRACTICES P ON U.PRACTICE_CODE = P.PRACTICE_CODE
		AND ISNULL(P.DELETED, 0) = 0
	WHERE (
			USER_NAME = @USERNAME
			OR EMAIL = @USERNAME
			)
		AND ISNULL(IS_LOCKED_OUT, 0) = 0

	IF (@LOCKEDUSER != 0)
	BEGIN
		SELECT @USER = COUNT(USER_NAME)
		FROM fox_tbl_application_user U
		INNER JOIN PRACTICES P ON U.PRACTICE_CODE = P.PRACTICE_CODE
			AND ISNULL(P.DELETED, 0) = 0
		WHERE (
				USER_NAME = @USERNAME
				OR EMAIL = @USERNAME
				)

		IF (@USER != 0)
		BEGIN
			SELECT @USER = COUNT(USER_NAME)
			FROM fox_tbl_application_user U
			INNER JOIN PRACTICES P ON U.PRACTICE_CODE = P.PRACTICE_CODE
				AND ISNULL(P.DELETED, 0) = 0
			WHERE (
					USER_NAME = @USERNAME
					OR EMAIL = @USERNAME
					)
				AND ISNULL(U.IS_ACTIVE, 0) = 1

			IF (@USER != 0)
			BEGIN
				SELECT @PASS = COUNT(USER_NAME)
				FROM fox_tbl_application_user U
				INNER JOIN PRACTICES P ON U.PRACTICE_CODE = P.PRACTICE_CODE
					AND ISNULL(P.DELETED, 0) = 0
				WHERE (
						USER_NAME = @USERNAME
						OR EMAIL = @USERNAME
						)
					AND PASSWORD = @PASSWORD
					AND ISNULL(U.IS_ACTIVE, 0) = 1

				IF (@PASS != 0)
				BEGIN
					SET @STATUS = 200 --  VALID USER	
				END
				ELSE
				BEGIN
					SET @STATUS = 202 --  INVALID PASSWORD
				END
			END
			ELSE
			BEGIN
				SET @STATUS = 203 -- INACTIVE USER
			END
		END
		ELSE
		BEGIN
			SET @STATUS = 201 -- INVALID USERID
		END
	END
	ELSE
	BEGIN
		SET @STATUS = 204 -- LOCKED USER
	END

	IF (@STATUS = 200)
	BEGIN
		SELECT @STATUS AS STATUS
			,USER_ID
			,U.PRACTICE_CODE
			,USER_NAME
			,PASSWORD
			,PASSWORD_CHANGED_DATE
			,FIRST_NAME
			,LAST_NAME
			,USER_DISPLAY_NAME
			,DESIGNATION NO_OFDAYS
			,DATE_OF_BIRTH
			,EMAIL
			,RESET_PASS
			,SECURITY_QUESTION
			,SECURITY_QUESTION_ANSWER
			,LOCKEDBY
			,LAST_LOGIN_DATE
			,FAILED_PASSWORD_ATTEMPT_COUNT
			,IS_LOCKED_OUT
			,COMMENTS
			,PASS_RESET_CODE
			,ACTIVATION_CODE
			,U.IS_ACTIVE
			,U.CREATED_DATE
			,U.CREATED_BY
			,U.MODIFIED_DATE
			,U.MODIFIED_BY
			,U.DELETED
			,ROLE_ID
			,IS_ADMIN
		FROM fox_tbl_application_user U
		INNER JOIN PRACTICES P ON U.PRACTICE_CODE = P.PRACTICE_CODE
			AND ISNULL(P.DELETED, 0) = 0
		WHERE (
				USER_NAME = @USERNAME
				OR EMAIL = @USERNAME
				)
			AND PASSWORD = @PASSWORD
			AND ISNULL(U.DELETED, 0) = 0
			AND ISNULL(U.IS_ACTIVE, 1) = 1
			AND ISNULL(U.IS_LOCKED_OUT, 0) = 0
	END
	ELSE
	BEGIN
		SELECT @STATUS AS STATUS
	END
END
