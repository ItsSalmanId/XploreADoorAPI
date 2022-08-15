IF (OBJECT_ID('FOX_PROC_GET_PSR_REGION_AND_RECOMMENDATION_WISE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PSR_REGION_AND_RECOMMENDATION_WISE  
GO
-- =============================================          
-- AUTHOR:  <DEVELOPER, YOUSAF>          
-- CREATE DATE: <CREATE DATE, 05/01/2018>          
-- DESCRIPTION: <GET PATIENT SURVEY REPORT; REGION AND RECOMMENDATION WISE>          
          
--           [DBO].[FOX_PROC_GET_PSR_REGION_AND_RECOMMENDATION_WISE] 1011163,'04/05/2019', '04/05/2021', '', '','','','',0, 0, ''             
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PSR_REGION_AND_RECOMMENDATION_WISE] --1011163, null, null, '', '',0, 0, ''          
 (          
 @PRACTICE_CODE BIGINT          
 ,@DATE_FROM DATETIME          
 ,@DATE_TO DATETIME          
 ,@PROVIDER VARCHAR(50)          
 ,@STATE VARCHAR(10)          
 ,@REGION VARCHAR(50)          
 ,@DISCIPLINE VARCHAR(50)          
 ,@STATUS VARCHAR(50)          
 ,@RECOMMENDED INT          
 ,@NOT_RECOMMENDED INT          
 ,@SEARCH_TEXT VARCHAR(100)          
 )          
AS          
BEGIN          
 DECLARE @RECOMMENDED_FROM INT;          
 DECLARE @RECOMMENDED_TO INT;          
 DECLARE @NOT_RECOMMENDED_FROM INT;          
 DECLARE @NOT_RECOMMENDED_TO INT;          
          
 IF(@STATUS = '')            
             BEGIN            
                 SET @STATUS = NULL;            
             END;            
             ELSE            
             BEGIN            
                 SET @STATUS = @STATUS;            
             END;            
        IF(@REGION = '')            
             BEGIN            
                 SET @REGION = NULL;            
             END;            
             ELSE            
             BEGIN            
                 SET @REGION = @REGION;            
             END;            
          IF(@DISCIPLINE = '')            
             BEGIN            
                 SET @DISCIPLINE = NULL;            
             END;            
             ELSE            
             BEGIN            
                 SET @DISCIPLINE = @DISCIPLINE;            
             END;          
 IF (@RECOMMENDED = 0) --RECOMMENDED          
 BEGIN          
  --ALL          
  SET @RECOMMENDED_FROM = 0;          
  SET @RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@RECOMMENDED = 1)          
 BEGIN          
  --GREATER THAN 15%           
  SET @RECOMMENDED_FROM = 15;          
  SET @RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@RECOMMENDED = 2)          
 BEGIN          
  --GREATER THAN 50%          
  SET @RECOMMENDED_FROM = 50;          
  SET @RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@RECOMMENDED = 3)          
 BEGIN          
  --GREATER THAN 75%          
  SET @RECOMMENDED_FROM = 75;          
  SET @RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@RECOMMENDED = 4)          
 BEGIN          
  --15% - 25%          
  SET @RECOMMENDED_FROM = 15;          
  SET @RECOMMENDED_TO = 25;          
 END          
 ELSE IF (@RECOMMENDED = 5)          
 BEGIN          
  --25% - 50%          
  SET @RECOMMENDED_FROM = 25;          
  SET @RECOMMENDED_TO = 50;          
 END          
 ELSE IF (@RECOMMENDED = 6)          
 BEGIN          
  --50% - 75%          
  SET @RECOMMENDED_FROM = 50;          
  SET @RECOMMENDED_TO = 75;          
 END          
 ELSE IF (@RECOMMENDED = 7)          
 BEGIN          
  --75% - 100%          
  SET @RECOMMENDED_FROM = 75;          
  SET @RECOMMENDED_TO = 100;          
 END          
          
 IF (@NOT_RECOMMENDED = 0) --NOT RECOMMENDED          
 BEGIN          
  --ALL          
  SET @NOT_RECOMMENDED_FROM = 0;          
  SET @NOT_RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 1)          
 BEGIN          
  --GREATER THAN 15%           
  SET @NOT_RECOMMENDED_FROM = 15;          
  SET @NOT_RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 2)          
 BEGIN          
  --GREATER THAN 50%      
  SET @NOT_RECOMMENDED_FROM = 50;          
  SET @NOT_RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 3)          
 BEGIN          
  --GREATER THAN 75%          
  SET @NOT_RECOMMENDED_FROM = 75;     
  SET @NOT_RECOMMENDED_TO = 100;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 4)          
 BEGIN          
  --15% - 25%          
  SET @NOT_RECOMMENDED_FROM = 15;          
  SET @NOT_RECOMMENDED_TO = 25;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 5)          
 BEGIN          
  --25% - 50%          
  SET @NOT_RECOMMENDED_FROM = 25;          
  SET @NOT_RECOMMENDED_TO = 50;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 6)          
 BEGIN          
  --50% - 75%          
  SET @NOT_RECOMMENDED_FROM = 50;          
  SET @NOT_RECOMMENDED_TO = 75;          
 END          
 ELSE IF (@NOT_RECOMMENDED = 7)          
 BEGIN          
  --75% - 100%          
  SET @NOT_RECOMMENDED_FROM = 75;          
  SET @NOT_RECOMMENDED_TO = 100;          
 END          
          
 IF OBJECT_ID('TEMPDB..#TEMP_TBL_PATIENT_SURVEY') IS NOT NULL          
  DROP TABLE #TEMP_TBL_PATIENT_SURVEY         
          
 IF OBJECT_ID('TEMPDB..#TEMP_TBL_PATIENT_SURVEY2') IS NOT NULL          
  DROP TABLE #TEMP_TBL_PATIENT_SURVEY2          
          
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
  ,ISNULL(CONVERT(INT, ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (          
        SUM(CASE           
          WHEN IS_REFERABLE = 1         
           THEN 1          
          ELSE 0          
          END)          
        )) / NULLIF((          
        (          
         SUM(CASE           
           WHEN IS_REFERABLE = 1          
            THEN 1          
           ELSE 0          
           END)          
         ) + (          
         SUM(CASE           
           WHEN IS_REFERABLE = 0          
            THEN 1          
           ELSE 0          
           END)          
         )          
        ), 0)), 0)), 0) AS RECOMMENDED_AVG          
  ,ISNULL(CONVERT(INT, ROUND(CONVERT(DECIMAL(5, 2), 100 * CONVERT(DECIMAL, (          
        SUM(CASE           
          WHEN IS_REFERABLE = 0          
           THEN 1          
          ELSE 0          
          END)          
        )) / NULLIF((          
        (          
         SUM(CASE           
           WHEN IS_REFERABLE = 1         
            THEN 1          
           ELSE 0          
           END)          
         ) + (          
         SUM(CASE           
           WHEN IS_REFERABLE = 0        
            THEN 1          
           ELSE 0          
           END)          
         )          
        ), 0)), 0)), 0) AS NOT_RECOMMENDED_AVG          
        --PROVIDER AS PROVIDER,          
        --SURVEY_STATUS_CHILD AS SURVEY_STATUS_CHILD,          
        --PATIENT_STATE AS PATIENT_STATE,          
        --PT_OT_SLP AS PT_OT_SLP          
 INTO #TEMP_TBL_PATIENT_SURVEY          
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
  AND PROVIDER LIKE '%' + @PROVIDER + '%'          
  AND PATIENT_STATE LIKE '%' + @STATE + '%'          
  AND (@REGION IS NULL            
     OR  REGION LIKE '%' + @REGION + '%' )            
     AND (@STATUS IS NULL            
     OR IS_REFERABLE =  @STATUS  )            
      AND (@DISCIPLINE IS NULL            
     OR SERVICE_OR_PAYMENT_DESCRIPTION LIKE '%' + @DISCIPLINE + '%' )            
          
 GROUP BY REGION           
 --, PROVIDER, SURVEY_STATUS_CHILD, PATIENT_STATE, PT_OT_SLP          
 ORDER BY REGION          
          
 SELECT *          
 INTO #TEMP_TBL_PATIENT_SURVEY2          
 FROM #TEMP_TBL_PATIENT_SURVEY          
 WHERE (          
   RECOMMENDED_AVG BETWEEN @RECOMMENDED_FROM          
    AND @RECOMMENDED_TO          
   )          
  AND (          
   NOT_RECOMMENDED_AVG BETWEEN @NOT_RECOMMENDED_FROM          
    AND @NOT_RECOMMENDED_TO          
   )          
  AND (          
   REGION LIKE '%' + @SEARCH_TEXT + '%'          
   OR RECOMMENDED LIKE '%' + @SEARCH_TEXT + '%'          
   OR NOT_RECOMMENDED LIKE '%' + @SEARCH_TEXT + '%'          
   OR RECOMMENDED_AVG LIKE '%' + @SEARCH_TEXT + '%'          
  OR NOT_RECOMMENDED_AVG LIKE '%' + @SEARCH_TEXT + '%'          
   )          
          
 SELECT *          
 FROM #TEMP_TBL_PATIENT_SURVEY2          
           
 UNION ALL          
           
 SELECT 'Total'          
  ,ISNULL(SUM(RECOMMENDED), 0)          
  ,ISNULL(SUM(NOT_RECOMMENDED), 0)          
  ,ISNULL(SUM(RECOMMENDED_AVG), 0)          
  ,ISNULL(SUM(NOT_RECOMMENDED_AVG), 0)          
 FROM #TEMP_TBL_PATIENT_SURVEY2          
END       
      
    
