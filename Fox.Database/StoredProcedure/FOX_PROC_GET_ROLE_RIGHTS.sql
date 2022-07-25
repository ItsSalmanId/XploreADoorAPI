IF (OBJECT_ID('FOX_PROC_GET_ROLE_RIGHTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ROLE_RIGHTS  
GO  
-- =============================================    
-- Author:  <Author,Mehmood ul Hassan>    
-- Create date: <Create Date,12/10/2017>    
-- Description: <get user data to create profile>    
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ROLE_RIGHTS] --1011163    
 @PRACTICE_CODE BIGINT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT R.ROLE_ID  
  ,ROLE_NAME  
  ,RI.RIGHT_ID  
  ,RI.RIGHT_NAME  
  ,RT.RIGHT_TYPE_NAME,  
   RI.DISPLAY_ORDER as OrderId  
  ,ISNULL(CHECKED, 0) AS IS_CHECKED  
  ,ISNULL(PR.RIGHTS_OF_ROLE_ID, ROR.RIGHTS_OF_ROLE_ID) RIGHTS_OF_ROLE_ID  
 FROM FOX_TBL_USER_RIGHTS RI  
 LEFT JOIN FOX_TBL_USER_RIGHTS_TYPE RT ON RI.RIGHT_TYPE_ID = RT.RIGHT_TYPE_ID  
 INNER JOIN FOX_TBL_RIGHTS_OF_ROLE ROR ON ROR.RIGHT_ID = RI.RIGHT_ID  
  AND ISNULL(ROR.DELETED, 0) = 0  
 INNER JOIN FOX_TBL_ROLE R ON ROR.ROLE_ID = R.ROLE_ID  
  AND (  
   R.PRACTICE_CODE IS NULL  
   OR R.PRACTICE_CODE = @PRACTICE_CODE  
   )  
 LEFT JOIN FOX_TBL_PRACTICE_ROLE_RIGHTS PR ON PR.RIGHTS_OF_ROLE_ID = ROR.RIGHTS_OF_ROLE_ID  
  AND PR.PRACTICE_CODE = @PRACTICE_CODE  
  AND ISNULL(PR.DELETED, 0) = 0  
 WHERE ISNULL(RI.DELETED, 0) = 0  
  AND ISNULL(R.DELETED, 0) = 0  
END  
  
