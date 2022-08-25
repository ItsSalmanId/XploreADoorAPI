IF (OBJECT_ID('FOX_PROC_GET_PSD_REGION_AND_RECOMMENDATION_WISE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PSD_REGION_AND_RECOMMENDATION_WISE  
GO      
-- =============================================        
-- AUTHOR:  <DEVELOPER, YOUSAF>        
-- CREATE DATE: <CREATE DATE, 13/08/2018>        
-- DESCRIPTION: <GET PATIENT SURVEY REPORT; STATE AND RECOMMENDATION WISE>        
--[DBO].[FOX_PROC_GET_PSD_REGION_AND_RECOMMENDATION_WISE] 1011163, null, null, '', '','','All'                  
CREATE  PROCEDURE [dbo].[FOX_PROC_GET_PSD_REGION_AND_RECOMMENDATION_WISE] --1011163, null, null, '', ''        
 (        
 @PRACTICE_CODE BIGINT        
 ,@DATE_FROM DATETIME        
 ,@DATE_TO DATETIME        
 ,@STATE VARCHAR(10)        
 ,@REGION VARCHAR(10)        
 ,@DISCIPLINE VARCHAR(10)        
 ,@FORMAT VARCHAR(50)        
 )        
AS        
BEGIN        
If (@FORMAT = 'All')        
BEGIN        
SET @FORMAT = 'Format'        
END        
 SELECT REGION        
  ,SUM(CASE         
    WHEN IS_REFERABLE = 1        
     THEN 1        
    ELSE 0        
    END) RECOMMENDED        
  ,SUM(CASE         
    WHEN IS_REFERABLE = 0       
     THEN 1        
    ELSE 0        
    END) NOT_RECOMMENDED        
 FROM FOX_TBL_PATIENT_SURVEY        
 WHERE ISNULL(DELETED, 0) = 0        
  AND PRACTICE_CODE = @PRACTICE_CODE        
  AND ISNULL(IS_SURVEYED, 0) = 1        
  AND SURVEY_STATUS_BASE = 'COMPLETED'        
  AND REGION <> ''        
  AND (        
   @DATE_FROM IS NULL        
   OR @DATE_TO IS NULL        
   OR CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM)        
    AND CONVERT(DATE, @DATE_TO)        
   )        
  --AND REGION LIKE '%' + @REGION + '%'        
  AND PATIENT_STATE LIKE '%' + @STATE + '%'        
  AND PT_OT_SLP LIKE '%' + @DISCIPLINE + '%'        
  AND SURVEY_FORMAT_TYPE LIKE '%' + @FORMAT + '%'        
 GROUP BY REGION        
 ORDER BY REGION        
        
END   

