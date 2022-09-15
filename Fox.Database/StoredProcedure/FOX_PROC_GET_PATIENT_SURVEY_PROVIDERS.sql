IF (OBJECT_ID('FOX_PROC_GET_PATIENT_SURVEY_PROVIDERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_SURVEY_PROVIDERS  
GO     
-- =============================================      
-- Author:  Muhammad Arslan      
-- Create date: 11/16/2021      
-- Description: This Procedure is used to get distinct details of patient states      
-- =============================================           
-- [DBO].[FOX_PROC_GET_PATIENT_SURVEY_PROVIDERS] 1011163            
CREATE PROCEDURE FOX_PROC_GET_PATIENT_SURVEY_PROVIDERS      
 -- Add the parameters for the stored procedure here      
 @PRACTICE_CODE BIGINT      
AS      
BEGIN      
    -- Insert statements for procedure here      
 SELECT DISTINCT [PROVIDER] AS STATE FROM FOX_TBL_PATIENT_SURVEY      
 WHERE PRACTICE_CODE = @PRACTICE_CODE AND [PROVIDER] <> ''      
 ORDER BY [PROVIDER] DESC       
END 