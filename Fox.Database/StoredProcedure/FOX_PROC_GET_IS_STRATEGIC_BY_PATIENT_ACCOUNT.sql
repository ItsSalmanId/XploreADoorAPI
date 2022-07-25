IF (OBJECT_ID('FOX_PROC_GET_IS_STRATEGIC_BY_PATIENT_ACCOUNT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_IS_STRATEGIC_BY_PATIENT_ACCOUNT  
GO          
-- =============================================            
-- Author:  <Abdul Sattar>            
-- Create date: <03/30/2021>            
-- Description: <Check patient has special acccount by patient_account id>            
-- =============================================            
--FOX_PROC_GET_IS_STRATEGIC_BY_PATIENT_ACCOUNT  101116354412374,1011163         
CREATE PROCEDURE FOX_PROC_GET_IS_STRATEGIC_BY_PATIENT_ACCOUNT          
 @PATIENT_ACCOUNT BIGINT,        
 @PRACTICE_CODE BIGINT        
AS            
BEGIN            
--declare @PATIENT_ACCOUNT BIGINT = 101116354813758        
--declare @PRACTICE_CODE BIGINT = 1011163        
 SET NOCOUNT ON;            
 SELECT         
 case        
when lower(fc.[NAME]) = lower('SA- Special Account') then cast(1 as bit)        
else cast(0 as bit)        
 end AS IS_STRATEGIC_ACCOUNT        
  FROM FOX_TBL_PATIENT P        
 INNER JOIN FOX_TBL_FINANCIAL_CLASS fc on fc.FINANCIAL_CLASS_ID = p.FINANCIAL_CLASS_ID        
 WHERE p.PATIENT_ACCOUNT = @PATIENT_ACCOUNT        
 AND fc.PRACTICE_CODE = @PRACTICE_CODE            
 AND ISNULL(p.DELETED,0) = 0        
 AND ISNULL(fc.DELETED,0) = 0        
END  
