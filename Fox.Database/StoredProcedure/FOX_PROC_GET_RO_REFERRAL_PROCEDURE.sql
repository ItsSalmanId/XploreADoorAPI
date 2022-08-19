IF (OBJECT_ID('FOX_PROC_GET_RO_REFERRAL_PROCEDURE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RO_REFERRAL_PROCEDURE  
GO
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================         
CREATE PROCEDURE FOX_PROC_GET_RO_REFERRAL_PROCEDURE    
 @WORK_ID BIGINT    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT TOP 1 * FROM FOX_TBL_PATIENT_PROCEDURE    
 WHERE WORK_ID = @WORK_ID     
 AND ISNULL(DELETED,0) = 0    
END   
