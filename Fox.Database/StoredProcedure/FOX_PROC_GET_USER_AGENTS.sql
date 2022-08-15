IF (OBJECT_ID('FOX_PROC_GET_USER_AGENTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USER_AGENTS  
GO 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,Mehmood ul Hassan>
-- Create date: <Create Date,12/10/2017>
-- DESCRIPTION:	<GET USER AGENTS>
create PROCEDURE [dbo].[FOX_PROC_GET_USER_AGENTS] --1011163,2,0,10,'','','',''
(
 @PRACTICE_CODE BIGINT
)
AS
BEGIN
SET NOCOUNT ON;

SELECT USER_NAME, FIRST_NAME, LAST_NAME FROM FOX_TBL_APPLICATION_USER
WHERE PRACTICE_CODE = @PRACTICE_CODE AND ROLE_ID=101

END




