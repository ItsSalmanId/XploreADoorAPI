IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_SOURCE_FACILITIES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_SOURCE_FACILITIES  
GO
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================      
    
CREATE PROCEDURE FOX_PROC_GET_REFERRAL_SOURCE_FACILITIES    
 @LOC_ID BIGINT,    
 @PRACTICE_CODE BIGINT    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT TOP 1 * FROM FOX_TBL_ACTIVE_LOCATIONS    
 WHERE LOC_ID = @LOC_ID     
 AND PRACTICE_CODE = @PRACTICE_CODE    
 AND ISNULL(DELETED,0) = 0    
END   
