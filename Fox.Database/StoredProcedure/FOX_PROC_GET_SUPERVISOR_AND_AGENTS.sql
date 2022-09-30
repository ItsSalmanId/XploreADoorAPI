IF (OBJECT_ID('FOX_PROC_GET_SUPERVISOR_AND_AGENTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SUPERVISOR_AND_AGENTS  
GO 
--------------------------------------------------------------------------------------   
-- Author:  <Author,Mehmood ul Hassan>  
-- Create date: <Create Date,12/10/2017>  
-- DESCRIPTION: <GET USER AGENTS>  
CREATE Procedure [dbo].[FOX_PROC_GET_SUPERVISOR_AND_AGENTS] --1011163  
(  
 @PRACTICE_CODE BIGINT  
)  
AS  
BEGIN  
SET NOCOUNT ON;  
  
  
SELECT USER_NAME, FIRST_NAME +' - '+ ROLE_NAME FIRST_NAME, LAST_NAME FROM FOX_TBL_APPLICATION_USER au  
join fox_tbl_Role r on au.ROLE_ID=r.ROLE_ID  
WHERE au.PRACTICE_CODE = @PRACTICE_CODE AND au.ROLE_ID IN (100,101,102) and au.deleted=0 and is_active=1  
  
END  
