IF (OBJECT_ID('FOX_PROC_GET_PATIENT_ELIG_DETAIL') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_ELIG_DETAIL  
GO   
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================          
CREATE PROCEDURE FOX_PROC_GET_PATIENT_ELIG_DETAIL    
 @PRACTICE_CODE BIGINT,    
 @PATIENT_INSURACE_ID BIGINT,    
 @PATIENT_ACCOUNT BIGINT    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT TOP 1 * FROM FOX_TBL_ELIG_HTML    
 WHERE PRACTICE_CODE = @PRACTICE_CODE    
 AND PATIENT_INSURANCE_ID = @PATIENT_INSURACE_ID    
 AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
 AND ISNULL(DELETED,0) = 0    
END   
