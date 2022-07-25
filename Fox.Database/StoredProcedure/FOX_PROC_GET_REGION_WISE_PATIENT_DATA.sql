IF (OBJECT_ID('FOX_PROC_GET_REGION_WISE_PATIENT_DATA') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REGION_WISE_PATIENT_DATA  
GO     
-- =============================================          
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>          
-- CREATE DATE: <CREATE DATE, 30/04/2018>          
-- DESCRIPTION: <FOX PROC GET REGION WISE PATIENT DATA>              
-- EXEC [FOX_PROC_GET_REGION_WISE_PATIENT_DATA] 1011163,null,null,'Amar Sonar','','Adirondack Mountains','','','',1,10,'Name','ASC'                  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REGION_WISE_PATIENT_DATA]           
@PRACTICE_CODE BIGINT        
,@DATE_FROM DATETIME        
,@DATE_TO DATETIME        
,@PROVIDER VARCHAR(50)        
,@STATE VARCHAR(10)        
,@REGION VARCHAR(50)        
,@DISCIPLINE VARCHAR(50)        
,@STATUS VARCHAR(50)        
,@SEARCH_TEXT VARCHAR(100)        
,@CURRENT_PAGE    INT           
,@RECORD_PER_PAGE INT           
,@SORT_BY         VARCHAR(50)           
,@SORT_ORDER      VARCHAR(50)         
        
AS           
          
BEGIN          
 IF(@SEARCH_TEXT = '')          
             BEGIN          
                 SET @SEARCH_TEXT = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @SEARCH_TEXT = '%'+@SEARCH_TEXT+'%';          
             END;          
        IF(@STATE = '')          
             BEGIN          
                 SET @STATE = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @STATE = '%'+@STATE+'%';          
             END;          
          IF(@PROVIDER = '')          
             BEGIN          
                 SET @PROVIDER = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @PROVIDER = '%'+@PROVIDER+'%';          
             END;        
  IF(@REGION = '')          
             BEGIN          
                 SET @REGION = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @REGION = '%'+@REGION+'%';          
             END;        
   IF(@DISCIPLINE = '')          
             BEGIN          
                 SET @DISCIPLINE = NULL;          
             END;          
             ELSE          
             BEGIN          
                 SET @DISCIPLINE = '%'+@DISCIPLINE+'%';          
             END;        
    IF(@STATUS = '')          
             BEGIN          
                 SET @STATUS = Null;          
             END;          
             ELSE          
             BEGIN          
                 SET @STATUS = @STATUS;          
             END;                   
         --                  
        
          
 SELECT         
    (PATIENT_FIRST_NAME + ' ' + PATIENT_LAST_NAME) AS NAME           
    ,ROW_NUMBER() OVER(ORDER BY PATIENT_FIRST_NAME ASC) AS ACTIVEROW         
 ,SERVICE_OR_PAYMENT_DESCRIPTION        
 ,FEEDBACK        
 ,DISCHARGE_DATE        
 ,convert(varchar,DISCHARGE_DATE) AS DISCHARGE_DATE_STR          
 ,REFERRAL_DATE        
 ,convert(varchar,REFERRAL_DATE) AS REFERRAL_DATE_STR          
 ,PATIENT_STATE        
 ,REGION          
  INTO #TEMPAGENTDATA          
  FROM FOX_TBL_PATIENT_SURVEY          
  WHERE (@SEARCH_TEXT IS NULL          
          OR PATIENT_FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'        
    OR PATIENT_LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'        
    OR (PATIENT_FIRST_NAME + ' ' + PATIENT_LAST_NAME) LIKE '%'+@SEARCH_TEXT+'%'        
    OR SERVICE_OR_PAYMENT_DESCRIPTION LIKE '%'+@SEARCH_TEXT+'%'        
    OR FEEDBACK LIKE '%'+@SEARCH_TEXT+'%'        
    OR CONVERT(DATE, DISCHARGE_DATE ) LIKE '%'+@SEARCH_TEXT+'%'        
    OR CONVERT(DATE, REFERRAL_DATE) LIKE '%'+@SEARCH_TEXT+'%'        
    OR REGION LIKE '%'+@SEARCH_TEXT+'%'        
    OR PATIENT_STATE LIKE '%'+@SEARCH_TEXT+'%'        
    )          
   AND  PRACTICE_CODE = @PRACTICE_CODE         
      AND ISNULL(DELETED, 0) = 0          
   AND (@PROVIDER IS NULL          
     OR PROVIDER LIKE '%' + @PROVIDER + '%')          
      AND (@STATE IS NULL          
     OR PATIENT_STATE LIKE '%' + @STATE + '%')         
   AND (@REGION IS NULL          
     OR REGION LIKE '%' + @REGION + '%')          
      AND (@DISCIPLINE IS NULL          
     OR SERVICE_OR_PAYMENT_DESCRIPTION LIKE '%' + @DISCIPLINE + '%')         
 --AND (@STATUS IS NULL          
   --  OR SURVEY_STATUS_CHILD = @STATUS)        
  --AND   IS_REFERABLE IN          
  --       (          
  --        SELECT Item          
  --        FROM dbo.SplitStrings_CTE(@STATUS, N',')          
  --       )      
    AND (@STATUS IS NULL          
     OR IS_REFERABLE =  @STATUS  )       
      AND (@DATE_FROM IS NULL            
           OR @DATE_TO IS NULL            
           OR CONVERT(DATE, MODIFIED_DATE)           
     BETWEEN CONVERT(DATE, @DATE_FROM)           
     AND CONVERT(DATE, @DATE_TO))          
          
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1;          
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;          
         DECLARE @TOATL_PAGESUDM FLOAT;          
          
          
          
    SELECT @TOATL_PAGESUDM = COUNT(*)          
         FROM #TEMPAGENTDATA           
            
          
      IF(@RECORD_PER_PAGE = 0)          
             BEGIN          
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;          
             END;          
             ELSE          
             BEGIN          
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;          
             END;          
          
         --                  
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;          
   SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);          
          
 SELECT *          
  ,CAST(@TOATL_PAGESUDM AS INT) AS TOTAL_RECORD_PAGES          
        ,CAST(@TOTAL_RECORDS AS INT) AS TOTAL_RECORDS           
        
 FROM #TEMPAGENTDATA            
          
  ORDER BY CASE          
                      WHEN @SORT_BY = 'Name'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN NAME          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'Name'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN NAME          
                  END DESC,          
      CASE          
                      WHEN @SORT_BY = 'Case'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN SERVICE_OR_PAYMENT_DESCRIPTION          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'Case'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN SERVICE_OR_PAYMENT_DESCRIPTION          
                  END DESC,          
      CASE          
                      WHEN @SORT_BY = 'FeedBack'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN FEEDBACK          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'FeedBack'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN FEEDBACK          
                  END DESC,          
      CASE          
                      WHEN @SORT_BY = 'Discharge'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN DISCHARGE_DATE          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'Discharge'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN DISCHARGE_DATE          
                 END DESC,          
      CASE          
                      WHEN @SORT_BY = 'Referral'          
                           AND @SORT_ORDER = 'ASC'          
                      THEN REFERRAL_DATE          
                  END ASC,          
                  CASE          
                      WHEN @SORT_BY = 'Referral'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN REFERRAL_DATE          
                  END DESC,          
      CASE          
                      WHEN @SORT_BY = 'State'          
                           AND @SORT_ORDER = 'ASC'          
      THEN PATIENT_STATE          
                  END ASC,          
      CASE          
                      WHEN @SORT_BY = 'State'          
                           AND @SORT_ORDER = 'DESC'          
                      THEN PATIENT_STATE          
                  END DESC,         
  CASE          
                      WHEN @SORT_BY = 'Region'          
                           AND @SORT_ORDER = 'ASC'          
      THEN REGION          
                  END ASC,          
      CASE          
                      WHEN @SORT_BY = 'Region'          
                           AND @SORT_ORDER = 'DESC'          
    THEN REGION          
                  END DESC         
  OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;          
        
END          
          
  

