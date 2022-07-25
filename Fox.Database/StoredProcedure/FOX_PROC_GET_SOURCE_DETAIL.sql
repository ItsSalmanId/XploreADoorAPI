IF (OBJECT_ID('FOX_PROC_GET_SOURCE_DETAIL') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SOURCE_DETAIL  
GO 
-- =============================================    
-- Author:  <Muhammad Imran>    
-- Create date: <09/30/2019>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE  FOX_PROC_GET_SOURCE_DETAIL    
 @PRACTICE_CODE BIGINT,    
 @WORK_ID BIGINT    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SELECT * FROM FOX_TBL_WORK_QUEUE    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
 AND WORK_ID = @WORK_ID    
 AND ISNULL(DELETED,0) = 0    
 AND WORK_ID <> 0    
END   
