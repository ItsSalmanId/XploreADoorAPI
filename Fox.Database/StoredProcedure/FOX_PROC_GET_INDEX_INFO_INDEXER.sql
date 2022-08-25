IF (OBJECT_ID('FOX_PROC_GET_INDEX_INFO_INDEXER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEX_INFO_INDEXER 
GO  
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================             
CREATE PROCEDURE FOX_PROC_GET_INDEX_INFO_INDEXER      
 @USER_NAME VARCHAR(70),      
 @PRACTICE_CODE BIGINT      
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
  SELECT TOP 1 * FROM FOX_TBL_APPLICATION_USER         
  WHERE USER_NAME = @USER_NAME      
  AND PRACTICE_CODE = @PRACTICE_CODE      
  AND ISNULL(DELETED,0) = 0        
END   
