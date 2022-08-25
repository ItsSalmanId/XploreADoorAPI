IF (OBJECT_ID('FOX_PROC_GET_GROUP_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_GROUP_ID 
GO   
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================      
  
CREATE PROCEDURE FOX_PROC_GET_GROUP_ID      
 @PRACTICE_CODE BIGINT,      
 @GROUP_NAME VARCHAR(100)    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
   
 SET NOCOUNT ON;      
 SELECT TOP 1 * FROM FOX_TBL_GROUP       
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
 AND LOWER(GROUP_NAME) = LOWER(@GROUP_NAME)    
 AND ISNULL(DELETED,0) = 0      
END   
