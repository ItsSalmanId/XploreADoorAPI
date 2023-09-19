IF (OBJECT_ID('FOX_PROC_GENERATE_INSERT_TOKEN') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GENERATE_INSERT_TOKEN
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,Mehmood ul Hassan>
-- Create date: <Create Date,12/10/2017>
-- Description:	<insert token in db for first login>
CREATE Procedure [dbo].[FOX_PROC_GENERATE_INSERT_TOKEN]
	@USERNAME BIGINT,
	@TOKEN VARCHAR(100),
	@USER_PROFILE VARCHAR(MAX)

AS
BEGIN 
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
		
	DECLARE @ISSUE_DATE DATETIME = GETDATE(); 
	DECLARE @EXPIRE_DATE DATETIME = DATEADD(ss, 600 , GETDATE());

	INSERT INTO	FOX_TBL_PROFILE_TOKENS( IssuedOn , ExpiresOn , AuthToken , UserId , Profile  )
	VALUES (@ISSUE_DATE , @EXPIRE_DATE, @TOKEN , @USERNAME, @USER_PROFILE )

	SELECT IssuedOn ,  ExpiresOn ,  AuthToken , UserId , Profile
	FROM FOX_TBL_PROFILE_TOKENS
	WHERE AUTHTOKEN = @TOKEN

END





