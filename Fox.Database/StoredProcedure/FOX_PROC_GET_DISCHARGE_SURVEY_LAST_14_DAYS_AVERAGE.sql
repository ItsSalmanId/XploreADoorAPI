-- =============================================        
-- AUTHOR:  <DEVELOPER, ARSLAN TUFAIL>        
-- CREATE DATE: <CREATE DATE, 12/29/2021>        
-- DESCRIPTION: <GET DISCHARGED AVG. DAYS>        
        
-- FOX_PROC_GET_DISCHARGE_SURVEY_LAST_14_DAYS_AVERAGE_ARSLAN 1011163,'12/14/2021', '12/29/2021', '', '', '', '' , 'Completed Survey,Unable to Complete Survey', ''       
-- FOX_PROC_GET_DISCHARGE_SURVEY_LAST_14_DAYS_AVERAGE_ARSLAN 1011163,'12/14/2021', '12/29/2021', 'NY', '', '', '' , '', ''       
      
ALTER PROCEDURE FOX_PROC_GET_DISCHARGE_SURVEY_LAST_14_DAYS_AVERAGE                                                               
 (@PRACTICE_CODE   BIGINT ,        
  @DATE_FROM       DATETIME,                                                                 
  @DATE_TO         DATETIME,      
  @STATE           VARCHAR(200),      
  @REGION     VARCHAR(100),      
  @PROVIDER     VARCHAR(100),      
  @SURVEYED_BY     VARCHAR(100),        
  @SURVEYED_STATUS VARCHAR(500),                                                                                                                                                                                          
  @FLAG            VARCHAR(10)                                                                                                                                                   
 )                                                                          
 AS                                                    
       
 BEGIN      
                                                                       
 IF(@FLAG = '')                                          
  BEGIN                                          
  SET @FLAG =  NULL                                    
 END        
              
   IF(@STATE = '')      
 BEGIN         
    SELECT                 
    --DISCHARGE_DATE,                 
    --SURVEY_COMPLETED_DATE,                
    --DATEDIFF(DAY, DISCHARGE_DATE, SURVEY_COMPLETED_DATE) as DAYS_DIFFERENCE,                
    isnull(AVG(DATEDIFF(DAY, PS.DISCHARGE_DATE, PS.SURVEY_COMPLETED_DATE)),0) AS AVERAGE_DAY        
    --PS.SURVEY_COMPLETED_DATE, PS.IS_SURVEYED, PS.SURVEY_FLAG,PS.MODIFIED_BY,PS.REGION,PS.SURVEY_STATUS_BASE, PS.PROVIDER, PS.PATIENT_STATE,*              
    FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)       
    LEFT JOIN FOX_TBL_APPLICATION_USER AS AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY      
    WHERE                 
    PS.PRACTICE_CODE = @PRACTICE_CODE        
    AND PS.PATIENT_STATE LIKE '%' + @STATE + '%'      
    AND PS.REGION LIKE '%' + @REGION + '%'      
    AND PS.PROVIDER LIKE '%' + @PROVIDER + '%'      
    AND AU.USER_NAME LIKE '%' + @SURVEYED_BY + '%'      
    AND ((@FLAG IS NULL) OR (@FLAG IS NOT NULL AND PS.SURVEY_FLAG = @FLAG) OR (@FLAG = 'Both' AND PS.SURVEY_FLAG <> ''))  --AND (PS.SURVEY_FLAG <> '')         
    AND PS.SURVEY_STATUS_BASE = 'COMPLETED'      
    AND PS.SURVEY_STATUS_CHILD IN      
    (      
    SELECT ITEM FROM DBO.SplitStrings_CTE(@SURVEYED_STATUS, N',')      
    )      
    AND ISNULL(PS.DELETED, 0) = 0       
    AND CONVERT(DATE,PS.SURVEY_COMPLETED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM, 101) AND CONVERT(DATE, @DATE_TO, 101)                            
   --GROUP BY DISCHARGE_DATE, SURVEY_COMPLETED_DATE        
  END       
     ELSE      
    BEGIN        
    SELECT                 
    --DISCHARGE_DATE,                 
    --SURVEY_COMPLETED_DATE,                
    --DATEDIFF(DAY, DISCHARGE_DATE, SURVEY_COMPLETED_DATE) as DAYS_DIFFERENCE,                
    isnull(AVG(DATEDIFF(DAY, PS.DISCHARGE_DATE, PS.SURVEY_COMPLETED_DATE)),0) AS AVERAGE_DAY        
    --PS.SURVEY_COMPLETED_DATE, PS.IS_SURVEYED, PS.SURVEY_FLAG,PS.MODIFIED_BY,PS.REGION,PS.SURVEY_STATUS_BASE, PS.PROVIDER, PS.PATIENT_STATE,*              
    FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)       
    LEFT JOIN FOX_TBL_APPLICATION_USER AS AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY      
    WHERE                 
    PS.PRACTICE_CODE = @PRACTICE_CODE        
    AND PS.PATIENT_STATE IN       
    (      
SELECT ITEM FROM DBO.SplitStrings_CTE(@STATE, N',')      
    )        
       AND PS.REGION LIKE '%' + @REGION + '%'      
    AND PS.PROVIDER LIKE '%' + @PROVIDER + '%'      
    AND AU.USER_NAME LIKE '%' + @SURVEYED_BY + '%'      
    AND ((@FLAG IS NULL) OR (@FLAG IS NOT NULL AND PS.SURVEY_FLAG = @FLAG) OR (@FLAG = 'Both' AND PS.SURVEY_FLAG <> ''))  --AND (PS.SURVEY_FLAG <> '')         
    AND PS.SURVEY_STATUS_BASE = 'COMPLETED'       
    AND PS.SURVEY_STATUS_CHILD IN      
    (      
    SELECT ITEM FROM DBO.SplitStrings_CTE(@SURVEYED_STATUS, N',')      
    )      
    AND ISNULL(PS.DELETED, 0) = 0       
    AND CONVERT(DATE,PS.SURVEY_COMPLETED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM, 101) AND CONVERT(DATE, @DATE_TO, 101)                            
   --GROUP BY DISCHARGE_DATE, SURVEY_COMPLETED_DATE        
   END         
 END        
	GO
