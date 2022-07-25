IF (OBJECT_ID('FOX_PROC_GET_MEDICARE_LIMIT_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_MEDICARE_LIMIT_ID  
GO    
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================          
CREATE PROCEDURE FOX_PROC_GET_MEDICARE_LIMIT_ID    
 @PRACTICE_CODE BIGINT,    
 @MEDICARE_LIMIT_ID BIGINT,    
 @PATIENT_ACCOUNT BIGINT    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT TOP 1 * FROM FOX_TBL_MEDICARE_LIMIT    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
 AND MEDICARE_LIMIT_ID = @MEDICARE_LIMIT_ID    
 AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
 AND ISNULL(DELETED,0) = 0    
END   
