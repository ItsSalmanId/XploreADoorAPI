IF (OBJECT_ID('FOX_PROC_GET_INDEXERS_AND_SUPERVISORS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXERS_AND_SUPERVISORS 
GO   
---------------------------------------------------------------------------------------------------------------------            
-- =============================================            
-- Author:  <Author,Mehmood ul Hassan>            
-- Create date: <Create Date,12/10/2017>            
-- DESCRIPTION: <GET USER AGENTS>            
CREATE Procedure [dbo].[FOX_PROC_GET_INDEXERS_AND_SUPERVISORS] --1011163            
(            
 @PRACTICE_CODE BIGINT,            
 @CURRENT_USER varchar(225)            
)            
AS            
BEGIN            
SET NOCOUNT ON;            
            
SELECT USER_NAME, upper(FIRST_NAME +' - '+ r.ROLE_NAME) FIRST_NAME, upper(LAST_NAME) LAST_NAME, upper(r.ROLE_NAME) ROLE_NAME FROM FOX_TBL_APPLICATION_USER au            
inner join FOX_TBL_ROLE r on au.ROLE_ID=r.ROLE_ID            
WHERE au.PRACTICE_CODE = @PRACTICE_CODE AND AU.ROLE_ID IN (101,102) AND IS_ACTIVE=1 AND  USER_NAME not like  @CURRENT_USER   AND  AU.DELETED=0            
  ORDER BY LAST_NAME ASC          
END 