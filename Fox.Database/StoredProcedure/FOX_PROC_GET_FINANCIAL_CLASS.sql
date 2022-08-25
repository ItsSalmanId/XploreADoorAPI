IF (OBJECT_ID('FOX_PROC_GET_FINANCIAL_CLASS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_FINANCIAL_CLASS 
GO   
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/26/2019>        
-- Description: <Description,,>        
-- =============================================        
      
CREATE PROCEDURE FOX_PROC_GET_FINANCIAL_CLASS      
 @FC_CODE VARCHAR(10),      
 @PRACTICE_CODE BIGINT      
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
 SELECT TOP 1 * FROM FOX_TBL_FINANCIAL_CLASS      
 WHERE LOWER(CODE) = @FC_CODE       
 AND PRACTICE_CODE = @PRACTICE_CODE      
 AND ISNULL(DELETED,0) = 0      
END   
