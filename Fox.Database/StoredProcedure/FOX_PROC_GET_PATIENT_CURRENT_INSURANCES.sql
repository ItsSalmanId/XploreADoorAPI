IF (OBJECT_ID('FOX_PROC_GET_PATIENT_CURRENT_INSURANCES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_CURRENT_INSURANCES  
GO  
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================         
CREATE PROCEDURE FOX_PROC_GET_PATIENT_CURRENT_INSURANCES    
 @PATIENT_ACCOUNT BIGINT    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
  SELECT  * FROM FOX_TBL_PATIENT_INSURANCE       
  WHERE Patient_Account = @PATIENT_ACCOUNT    
  AND ISNULL(DELETED,0) = 0    
  AND (ISNULL(FOX_INSURANCE_STATUS,'') = ''    
  OR UPPER(FOX_INSURANCE_STATUS) = 'C')    
END   
