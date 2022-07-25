IF (OBJECT_ID('FOX_PROC_GET_REGIONAL_LIAISON_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REGIONAL_LIAISON_SERVICE  
GO 
-- =============================================      
-- Author:  Muhammad Arslan Tufail      
-- Create date: 11/20/2021      
-- Description: This Procedure trigger to get User Emails      
-- =============================================    
-- [DBO].[FOX_PROC_GET_REGIONAL_LIAISON_SERVICE]  1012714, 605175  
CREATE PROCEDURE FOX_PROC_GET_REGIONAL_LIAISON_SERVICE       
 @PRACTICE_CODE BIGINT,      
 @ROLE_ID BIGINT      
AS      
BEGIN      
 SELECT EMAIL FROM FOX_TBL_APPLICATION_USER      
 WHERE DELETED = 0 AND PRACTICE_CODE = @PRACTICE_CODE AND ROLE_ID = @ROLE_ID AND IS_ACTIVE = 1    
END 