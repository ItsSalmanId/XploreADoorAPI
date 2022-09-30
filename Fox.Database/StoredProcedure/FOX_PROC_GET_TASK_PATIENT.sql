IF (OBJECT_ID('FOX_PROC_GET_TASK_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_PATIENT  
GO 
-- =============================================    
-- Author:  <Muhammad Imran>    
-- Create date: <09/30/2019>    
-- Description: <Description,,>    
-- =============================================     
CREATE PROCEDURE  FOX_PROC_GET_TASK_PATIENT    
 @PATIENT_ACCOUNT BIGINT,    
 @PRACTICE_CODE BIGINT    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SELECT * FROM PATIENT    
 WHERE Patient_Account = @PATIENT_ACCOUNT    
 AND PRACTICE_CODE = @PRACTICE_CODE    
 AND ISNULL(DELETED,0) = 0    
END   
