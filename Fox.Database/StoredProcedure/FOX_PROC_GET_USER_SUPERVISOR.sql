IF (OBJECT_ID('FOX_PROC_GET_USER_SUPERVISOR') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USER_SUPERVISOR  
GO 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------  
-- =============================================  
-- Author:  <Author,Mehmood ul Hassan>  
-- Create date: <Create Date,12/10/2017>  
-- DESCRIPTION: <GET USER AGENTS>  
CREATE Procedure [dbo].[FOX_PROC_GET_USER_SUPERVISOR] --1011163  
(  
 @PRACTICE_CODE BIGINT  
)  
AS  
BEGIN  
SET NOCOUNT ON;  
  
SELECT USER_NAME, UPPER(FIRST_NAME)  FIRST_NAME, UPPER(LAST_NAME) LAST_NAME FROM FOX_TBL_APPLICATION_USER  
WHERE PRACTICE_CODE = @PRACTICE_CODE AND ROLE_ID=102 and deleted=0 and is_active=1  
  
END  
  
  


