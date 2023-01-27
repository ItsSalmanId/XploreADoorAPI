-- =============================================                                              
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                                              
-- CREATE DATE: <CREATE DATE, 22/09/2022>                                              
-- DESCRIPTION:  THIS PROCEDURE IS GET QA DASHBOARD PIE CHART DATA                                          
-- =============================================                                                  
-- FOX_PROC_GET_QA_DASHBOARD_DATA_BKP 1011163,'544110,544109,544103,544114,544112,544113,544108,544115,544106,544111,54410118','PHD', '1163TESTING', '08/01/2022 12:00:00 AM' , '08/31/2022 11:59:59 PM'                                                                           
CREATE PROCEDURE FOX_PROC_GET_QA_DASHBOARD_DATA                                    
  @PRACTICE_CODE BIGINT,                                                  
  @CALL_SCANRIO_ID VARCHAR(200),                                  
  @CALL_TYPE VARCHAR(20),                                 
  @AGENT_NAME VARCHAR(MAX),                  
  @CRITERIA_NAME VARCHAR(50),                                
  @START_DATE DATETIME,                              
  @END_DATE DATETIME                                 
AS                                      
BEGIN                        
 IF(@CALL_TYPE = 'PHD')                      
  BEGIN                                
                   
  IF OBJECT_ID('TEMPDB.DBO.#TEMP_PHD_CLIENTEXPERIENCE', 'U') IS NOT NULL DROP TABLE TEMP_PHD_CLIENTEXPERIENCE;                     
  DECLARE @PERCENTAGE_VALUE INT,                
  @CLIENT_PERCENTAGE_VALUE INT,                
  @SYSTEM_PERCENTAGE_VALUE INT,                
  @SURVEY_CLIENT_PERCENTAGE_VALUE INT,                
  @SURVEY_SYSTEM_PERCENTAGE_VALUE INT                  
                  
  SELECT CLIENT_EXPERIENCE_TOTAL,                       
  SYSTEM_PROCESS_TOTAL,                       
  TOTAL_POINTS,                      
  CALL_TYPE,                      
  PHD_CALL_SCENARIO_ID,                      
  PHD_CALL_ID,                      
  AGENT_NAME,                      
  MODIFIED_DATE                   
  INTO #TEMP_PHD_CLIENTEXPERIENCE                             
  FROM                       
  FOX_TBL_SURVEY_AUDIT_SCORES WITH (NOLOCK)                    
  WHERE                           
  PHD_CALL_SCENARIO_ID IN (SELECT VAL FROM F_SPLIT(@CALL_SCANRIO_ID, ','))                     
  AND CALL_TYPE = @CALL_TYPE                             
  AND (@AGENT_NAME IS NULL OR AGENT_NAME IN (SELECT VAL FROM DBO.F_SPLIT(@AGENT_NAME, ',')))                           
  AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @START_DATE) AND CONVERT(DATE, @END_DATE)                            
  AND PRACTICE_CODE = @PRACTICE_CODE                              
  AND ISNULL(DELETED, 0) = 0                  
                  
  SET @PERCENTAGE_VALUE =  (SELECT (PERCENTAGE * 0.75) AS PERCENTAGE_VALUE                   
  FROM FOX_TBL_EVALUATION_CRITERIA WITH (NOLOCK)   WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND CALL_TYPE = @CALL_TYPE AND CRITERIA_NAME = @CRITERIA_NAME)                  
                  
  IF(@CRITERIA_NAME = 'Client Experience')                  
  BEGIN                  
   SELECT                   
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (CLIENT_EXPERIENCE_TOTAL > @PERCENTAGE_VALUE)) AS GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (CLIENT_EXPERIENCE_TOTAL <= @PERCENTAGE_VALUE)) AS LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE) AS TOTAL_COUNT                  
  END                  
  IF(@CRITERIA_NAME = 'System Product and Process')                  
  BEGIN       
     SELECT                   
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (SYSTEM_PROCESS_TOTAL > @PERCENTAGE_VALUE)) AS GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (SYSTEM_PROCESS_TOTAL <= @PERCENTAGE_VALUE)) AS LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE) AS TOTAL_COUNT                  
  END                  
    IF(@CRITERIA_NAME = 'Both')                  
  BEGIN              
     SELECT                   
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (TOTAL_POINTS > 75)) AS GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (TOTAL_POINTS <= 75)) AS LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE) AS TOTAL_COUNT                  
  END                  
     IF(@CRITERIA_NAME = 'System and Client')                  
  BEGIN                  
  SET @CLIENT_PERCENTAGE_VALUE = (SELECT (PERCENTAGE * 0.75) AS PERCENTAGE_VALUE                   
  FROM FOX_TBL_EVALUATION_CRITERIA WITH (NOLOCK)   WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND CALL_TYPE = @CALL_TYPE AND CRITERIA_NAME = 'Client Experience')                   
  SET @SYSTEM_PERCENTAGE_VALUE = (SELECT (PERCENTAGE * 0.75) AS PERCENTAGE_VALUE                   
  FROM FOX_TBL_EVALUATION_CRITERIA WITH (NOLOCK)   WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND CALL_TYPE =  @CALL_TYPE AND CRITERIA_NAME = 'System Product and Process')                
   SELECT                   
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (CLIENT_EXPERIENCE_TOTAL > @CLIENT_PERCENTAGE_VALUE)) AS CLIENT_GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (CLIENT_EXPERIENCE_TOTAL <= @CLIENT_PERCENTAGE_VALUE)) AS CLIENT_LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (SYSTEM_PROCESS_TOTAL > @SYSTEM_PERCENTAGE_VALUE)) AS SYSTEM_GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE (SYSTEM_PROCESS_TOTAL <= @SYSTEM_PERCENTAGE_VALUE)) AS SYSTEM_LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_PHD_CLIENTEXPERIENCE WHERE CALL_TYPE = @CALL_TYPE) AS TOTAL_COUNT                  
  END                 
 END                      
 ----------------------------------------------------------------------------------------------------------------------                
 IF(@CALL_TYPE = 'SURVEY')                      
  BEGIN                  
   IF OBJECT_ID('TEMPDB.DBO.#TEMP_SURVEY_CLIENTEXPERIENCE', 'U') IS NOT NULL DROP TABLE TEMP_SURVEY_CLIENTEXPERIENCE;                     
                
  SELECT CLIENT_EXPERIENCE_TOTAL,                       
  SYSTEM_PROCESS_TOTAL,                       
  TOTAL_POINTS,                      
  CALL_TYPE,                      
  PHD_CALL_SCENARIO_ID,                      
  PHD_CALL_ID,                      
  AGENT_NAME,                      
  MODIFIED_DATE                  
  INTO #TEMP_SURVEY_CLIENTEXPERIENCE                    
  FROM                       
  FOX_TBL_SURVEY_AUDIT_SCORES  WITH (NOLOCK)                    
  WHERE                       
  CALL_TYPE = @CALL_TYPE   and        
  PHD_CALL_SCENARIO_ID IS NULL        
  AND (@AGENT_NAME IS NULL OR AGENT_NAME IN (SELECT VAL FROM DBO.F_SPLIT(@AGENT_NAME, ',')))                           
  AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @START_DATE) AND CONVERT(DATE, @END_DATE)                            
  AND PRACTICE_CODE = @PRACTICE_CODE                              
  AND ISNULL(DELETED,0) = 0                    
                  
                  
  SELECT @PERCENTAGE_VALUE = (SELECT (PERCENTAGE * 0.75) AS PERCENTAGE_VALUE                  
  FROM FOX_TBL_EVALUATION_CRITERIA WITH (NOLOCK)   WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0)=0 AND CALL_TYPE = @CALL_TYPE AND CRITERIA_NAME =@CRITERIA_NAME)                
                  
  IF(@CRITERIA_NAME = 'Client Experience')                
  BEGIN                
  SELECT                 
  (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE where (CLIENT_EXPERIENCE_TOTAL > @PERCENTAGE_VALUE)) as GREATER_THAN,                
  (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE where (CLIENT_EXPERIENCE_TOTAL <= @PERCENTAGE_VALUE)) as LESS_THAN,                
  (select COUNT(*) from #TEMP_SURVEY_CLIENTEXPERIENCE) as TOTAL_COUNT                
  END                
IF(@CRITERIA_NAME = 'System Product and Process')                  
  begin                
  select                 
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE where (SYSTEM_PROCESS_TOTAL > @PERCENTAGE_VALUE)) as GREATER_THAN,                
  (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE where (SYSTEM_PROCESS_TOTAL <= @PERCENTAGE_VALUE)) as LESS_THAN,                
  (select COUNT(*) from #TEMP_SURVEY_CLIENTEXPERIENCE) as TOTAL_COUNT                
  end                
      IF(@CRITERIA_NAME = 'Both')                  
  BEGIN                  
     SELECT                   
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE (TOTAL_POINTS > 75)) AS GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE (TOTAL_POINTS <= 75)) AS LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE) AS TOTAL_COUNT                  
  END                  
       IF(@CRITERIA_NAME = 'System and Client')                  
  BEGIN                  
                   
  SET @SURVEY_CLIENT_PERCENTAGE_VALUE = (SELECT (PERCENTAGE * 0.75) AS PERCENTAGE_VALUE                   
  FROM FOX_TBL_EVALUATION_CRITERIA WITH (NOLOCK)   WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND CALL_TYPE = @CALL_TYPE AND CRITERIA_NAME = 'Client Experience')                
     SET @SURVEY_SYSTEM_PERCENTAGE_VALUE = (SELECT (PERCENTAGE * 0.75) AS PERCENTAGE_VALUE                   
  FROM FOX_TBL_EVALUATION_CRITERIA WITH (NOLOCK)   WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND CALL_TYPE =  @CALL_TYPE AND CRITERIA_NAME = 'System Product and Process')                
   SELECT                   
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE (CLIENT_EXPERIENCE_TOTAL > @SURVEY_CLIENT_PERCENTAGE_VALUE)) AS SURVEY_CLIENT_GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE (CLIENT_EXPERIENCE_TOTAL <= @SURVEY_CLIENT_PERCENTAGE_VALUE)) AS SURVEY_CLIENT_LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE (SYSTEM_PROCESS_TOTAL > @SURVEY_SYSTEM_PERCENTAGE_VALUE)) AS SURVEY_SYS_GREATER_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE (SYSTEM_PROCESS_TOTAL <= @SURVEY_SYSTEM_PERCENTAGE_VALUE)) AS SURVEY_SYS_LESS_THAN,                  
   (SELECT COUNT(*) FROM #TEMP_SURVEY_CLIENTEXPERIENCE WHERE CALL_TYPE = @CALL_TYPE) AS TOTAL_COUNT                  
  END                 
 END                    
 --========================================================================================-----                
 IF(@CALL_TYPE = 'Phd and Survey')                      
  BEGIN                       
    IF OBJECT_ID('TEMPDB.DBO.#TEMP_QA_DASHBOARD', 'U') IS NOT NULL DROP TABLE TEMP_QA_DASHBOARD;                   
  SELECT CLIENT_EXPERIENCE_TOTAL,                       
  SYSTEM_PROCESS_TOTAL,                       
  TOTAL_POINTS,                      
  CALL_TYPE,                      
  PHD_CALL_SCENARIO_ID,                      
  PHD_CALL_ID,                      
  AGENT_NAME,                      
  MODIFIED_DATE                     
  INTO #TEMP_QA_DASHBOARD                
  FROM                       
  FOX_TBL_SURVEY_AUDIT_SCORES WITH (NOLOCK)                     
  WHERE               
          
  (PHD_CALL_SCENARIO_ID IS NULL OR PHD_CALL_SCENARIO_ID IN (SELECT VAL FROM F_SPLIT(@CALL_SCANRIO_ID, ',')))                      
  AND (@AGENT_NAME IS NULL OR AGENT_NAME IN (SELECT VAL FROM DBO.F_SPLIT(@AGENT_NAME, ',')))     
  AND (CALL_TYPE ='phd' and isnull(PHD_CALL_SCENARIO_ID,0) <> 0 or CALL_TYPE = 'survey' )    
  AND CONVERT(DATE, MODIFIED_DATE) BETWEEN CONVERT(DATE, @START_DATE) AND CONVERT(DATE, @END_DATE)                            
  AND PRACTICE_CODE = @PRACTICE_CODE                              
  AND ISNULL(DELETED,0) = 0                   
                
 ( SELECT                 
  (SELECT COUNT(*) FROM #TEMP_QA_DASHBOARD where (TOTAL_POINTS > 75)) as GREATER_THAN,                
  (SELECT COUNT(*) FROM #TEMP_QA_DASHBOARD where (TOTAL_POINTS <= 75)) as LESS_THAN,                
  (select COUNT(*) from #TEMP_QA_DASHBOARD) as TOTAL_COUNT)                
                
 END                             
END   
  