IF (OBJECT_ID('FOX_PROC_GET_INDEXER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXER 
GO  
--------------------------------------------------------------------------------------   
-- Author:  <Author,Mehmood ul Hassan>    
-- Create date: <Create Date,12/10/2017>    
-- DESCRIPTION: <GET USER AGENTS>    
CREATE Procedure [dbo].[FOX_PROC_GET_INDEXER] --1011163,2,0,10,'','','',''    
(    
 @PRACTICE_CODE BIGINT    
)    
AS    
BEGIN    
SET NOCOUNT ON;    
    
SELECT USER_NAME, UPPER(FIRST_NAME +' - '+ r.ROLE_NAME) FIRST_NAME, UPPER(LAST_NAME) LAST_NAME FROM FOX_TBL_APPLICATION_USER au    
inner join FOX_TBL_ROLE r on au.ROLE_ID=r.ROLE_ID    
WHERE au.PRACTICE_CODE = @PRACTICE_CODE AND AU.ROLE_ID =101 AND IS_ACTIVE=1 AND AU.DELETED=0    
    
END   
