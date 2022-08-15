IF (OBJECT_ID('FOX_PROC_GET_USER_INDEXER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USER_INDEXER  
GO
---------------------------------------------------------------------------------------------------------------------          
-- =============================================          
-- Author:  <Author,Mehmood ul Hassan>          
-- Create date: <Create Date,12/10/2017>          
-- DESCRIPTION: <GET USER AGENTS>        
--[dbo].[FOX_PROC_GET_USER_INDEXER] 1011163,'1163testing'           
CREATE Procedure [dbo].[FOX_PROC_GET_USER_INDEXER] --1011163,'1163testing'         
(          
 @PRACTICE_CODE BIGINT,          
 @CURRENT_USER varchar(225)          
)          
AS          
BEGIN          
SET NOCOUNT ON;          
          
SELECT au.USER_ID,r.ROLE_NAME, USER_NAME, UPPER(FIRST_NAME +' - '+ r.ROLE_NAME) FIRST_NAME, UPPER(LAST_NAME) LAST_NAME FROM FOX_TBL_APPLICATION_USER au          
inner join FOX_TBL_ROLE r on au.ROLE_ID=r.ROLE_ID          
WHERE au.PRACTICE_CODE = @PRACTICE_CODE AND au.ROLE_ID=101 and au.deleted=0 and is_active=1      
AND  USER_NAME not like  @CURRENT_USER     
ORDER BY LAST_NAME ASC       
END 
