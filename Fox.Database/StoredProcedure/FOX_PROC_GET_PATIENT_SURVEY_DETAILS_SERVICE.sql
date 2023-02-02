-- =============================================      
-- Author:  Muhammad Arslan Tufail      
-- Create date: 12/03/2022      
-- Description: This SP trigger to get patient details for survey automation service      
-- =============================================      
-- FOX_PROC_GET_PATIENT_SURVEY_DETAILS_SERVICE 1011163      
CREATE PROCEDURE FOX_PROC_GET_PATIENT_SURVEY_DETAILS_SERVICE       
 -- Add the parameters for the stored procedure here      
 @PRACTICE_CODE BIGINT      
AS      
BEGIN      
      
 DECLARE @SURVEY_FILE_NAME VARCHAR(MAX)      
      
 SET @SURVEY_FILE_NAME = (SELECT TOP 1 FILE_NAME FROM FOX_TBL_PATIENT_SURVEY WITH (NOLOCK) WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 ORDER BY CREATED_DATE DESC)      
      
 SELECT       
 PS.PATIENT_ACCOUNT_NUMBER,      
 PS.SURVEY_ID,      
 PS.PATIENT_FIRST_NAME,      
 PS.PATIENT_LAST_NAME,      
 P.Email_Address,      
 P.Home_Phone,      
 PS.SURVEY_FORMAT_TYPE,      
 PS.REGION,      
 PS.PROVIDER,      
 PS.PT_OT_SLP,      
  PS.FILE_NAME         
 FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)      
 INNER JOIN PATIENT AS P WITH (NOLOCK) ON PS.PATIENT_ACCOUNT_NUMBER = + '00' + P.CHART_ID AND P.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(P.DELETED, 0) = 0      
 WHERE PS.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(PS.DELETED, 0) = 0 AND PS.FILE_NAME = @SURVEY_FILE_NAME            
END 