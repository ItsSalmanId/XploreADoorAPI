IF (OBJECT_ID('FOX_PROC_GET_DOCUMENT_TYPE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DOCUMENT_TYPE 
GO    
-- =============================================    
-- Author:  <Muhammad Imran>    
-- Create date: <09/26/2019>    
-- Description: <Description,,>    
-- =============================================    
  
CREATE PROCEDURE FOX_PROC_GET_DOCUMENT_TYPE    
 @DOCUMENT_TYPE_ID BIGINT     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 SET NOCOUNT ON;    
 SELECT TOP 1 * FROM FOX_TBL_DOCUMENT_TYPE     
 WHERE DOCUMENT_TYPE_ID = @DOCUMENT_TYPE_ID    
 AND ISNULL(DELETED,0) = 0    
END    
