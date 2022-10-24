IF (OBJECT_ID('FOX_PROC_GET_PSR_PROVIDER_AND_QUESTION_WISE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PSR_PROVIDER_AND_QUESTION_WISE  
GO  
-- =============================================          
-- AUTHOR:  <DEVELOPER, YOUSAF>          
-- CREATE DATE: <CREATE DATE, 04/26/2018>          
-- DESCRIPTION: <GET PATIENT SURVEY REPORT; PROVIDER AND QUESTION WISE>          
          
--FOX_PROC_GET_PSR_PROVIDER_AND_QUESTION_WISE 1011163, 'Northern California', '2018-10-29 19:15:50.000', '2021-11-13 19:15:50.000', '', '', 'New Format', ''          
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PSR_PROVIDER_AND_QUESTION_WISE]          
(          
@PRACTICE_CODE BIGINT,          
@REGION VARCHAR(100),          
@DATE_FROM DATETIME,          
@DATE_TO DATETIME,          
@PROVIDER VARCHAR(50),          
@STATE VARCHAR(10),          
@FORMAT VARCHAR(10),          
@SEARCH_TEXT VARCHAR(100)          
)          
AS          
BEGIN          
          
IF OBJECT_ID('TEMPDB..#TEMP_TBL_PSR_PROVIDER_AND_QUESTION_WISE') IS NOT NULL          
DROP TABLE #TEMP_TBL_PSR_PROVIDER_AND_QUESTION_WISE          
          
SELECT          
PROVIDER,           
SUM(CASE WHEN IS_CONTACT_HQ = 1 THEN 1 ELSE 0 END ) IS_CONTACT_HQ_YES,          
SUM(CASE WHEN IS_CONTACT_HQ = 0 THEN 1 ELSE 0 END ) IS_CONTACT_HQ_NO,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_CONTACT_HQ = 1 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_CONTACT_HQ = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_CONTACT_HQ = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_CONTACT_HQ_YES_AVG,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_CONTACT_HQ = 0 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_CONTACT_HQ = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_CONTACT_HQ = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_CONTACT_HQ_NO_AVG,          
SUM(CASE WHEN IS_RESPONSED_BY_HQ = 1 THEN 1 ELSE 0 END) IS_RESPONSED_BY_HQ_YES,          
SUM(CASE WHEN IS_RESPONSED_BY_HQ = 0 THEN 1 ELSE 0 END) IS_RESPONSED_BY_HQ_NO,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_RESPONSED_BY_HQ = 1 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_RESPONSED_BY_HQ = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_RESPONSED_BY_HQ = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_RESPONSED_BY_HQ_YES_AVG,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_RESPONSED_BY_HQ = 0 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_RESPONSED_BY_HQ = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_RESPONSED_BY_HQ = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_RESPONSED_BY_HQ_NO_AVG,          
          
              
SUM(CASE WHEN IS_QUESTION_ANSWERED = 1 THEN 1 ELSE 0 END) IS_QUESTION_ANSWERED_YES,          
SUM(CASE WHEN IS_QUESTION_ANSWERED = 0 THEN 1 ELSE 0 END) IS_QUESTION_ANSWERED_NO,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_QUESTION_ANSWERED = 1 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_QUESTION_ANSWERED = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_QUESTION_ANSWERED = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_QUESTION_ANSWERED_YES_AVG,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_QUESTION_ANSWERED = 0 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_QUESTION_ANSWERED = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_QUESTION_ANSWERED = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_QUESTION_ANSWERED_NO_AVG,          
          
SUM(CASE WHEN IS_REFERABLE = 1 THEN 1 ELSE 0 END ) IS_REFERABLE_YES,          
SUM(CASE WHEN IS_REFERABLE = 0 THEN 1 ELSE 0 END) IS_REFERABLE_NO,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_REFERABLE = 1 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_REFERABLE = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_REFERABLE = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_REFERABLE_YES_AVG,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_REFERABLE = 0 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_REFERABLE = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_REFERABLE = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_REFERABLE_NO_AVG,          
SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 1 THEN 1 ELSE 0 END) IS_IMPROVED_SETISFACTION_YES,          
SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 0 THEN 1 ELSE 0 END) IS_IMPROVED_SETISFACTION_NO,        
             
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 1 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_IMPROVED_SETISFACTION_YES_AVG,        
           
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 0 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_IMPROVED_SETISFACTION = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_IMPROVED_SETISFACTION_NO_AVG,         
         
SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 1 THEN 1 ELSE 0 END ) IS_PROTECTIVE_EQUIPMENT_YES,          
SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 0 THEN 1 ELSE 0 END) IS_PROTECTIVE_EQUIPMENT_NO,        
        
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 1 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_PROTECTIVE_EQUIPMENT_YES_AVG,          
CONVERT(INT,ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 0 THEN 1 ELSE 0 END)))          
/ NULLIF(((SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 1 THEN 1 ELSE 0 END)) + (SUM(CASE WHEN IS_PROTECTIVE_EQUIPMENT = 0 THEN 1 ELSE 0 END))), 0)),0)) AS IS_PROTECTIVE_EQUIPMENT_NO_AVG        
           
           
INTO #TEMP_TBL_PSR_PROVIDER_AND_QUESTION_WISE          
FROM FOX_TBL_PATIENT_SURVEY          
WHERE ISNULL(DELETED, 0) = 0 AND ISNULL(IS_SURVEYED, 0) = 1 AND PRACTICE_CODE = @PRACTICE_CODE AND REGION = @REGION          
AND ( @DATE_FROM IS NULL OR @DATE_TO IS NULL OR CONVERT(DATE,MODIFIED_DATE) BETWEEN CONVERT(DATE,@DATE_FROM) AND CONVERT(DATE,@DATE_TO) )          
AND PROVIDER LIKE '%' + @PROVIDER + '%'          
AND PATIENT_STATE LIKE '%' + @STATE + '%'          
AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'          
GROUP BY PROVIDER          
ORDER BY PROVIDER          
          
SELECT *          
FROM #TEMP_TBL_PSR_PROVIDER_AND_QUESTION_WISE          
          
END 