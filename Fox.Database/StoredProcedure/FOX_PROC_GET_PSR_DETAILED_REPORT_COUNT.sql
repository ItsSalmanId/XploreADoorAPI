-- =============================================         
-- Modified By :  Aftab Khan         
-- Modified date: 05/05/2023         
-- =============================================            
-- EXEC [DBO].[FOX_PROC_GET_PSR_DETAILED_REPORT_AFTAB1]   '1011163', '8/20/2021', '9/7/2021', '', '', '', 'Both', 'ALL', '', 'Completed Survey ,Deceased,Unable to Complete Survey,Not Interested', 1,30, '', 'SURVEYCOMPLETEDDATE', 'DESC'                       
  -- EXEC [DBO].[FOX_PROC_GET_PSR_DETAILED_REPORT_TEMp]  '1012714', '04/01/2023', '05/20/2023', '', '', '', 'Both', 'ALL', '', 'Completed Survey,Deceased,Unable to complete survey,Callback,Not Answered,Not Interested,Not Enough Services Provided','MailBoxFull,VM Left,Wrong PH#,Line Busy',1, 5000,'','SURVEYCOMPLETEDDATE', 'DESC'            
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PSR_DETAILED_REPORT_COUNT]        
 (@PRACTICE_CODE BIGINT,          
  @DATE_FROM DATETIME,         
  @DATE_TO DATETIME,         
  @PROVIDER VARCHAR(100),         
  @REGION VARCHAR(100),         
  @STATE VARCHAR(200),         
  @FLAG VARCHAR(10),         
  @FORMAT VARCHAR(10),             
  @SURVEYED_BY VARCHAR(100),         
  @SURVEYED_STATUS VARCHAR(500),          
  @NOT_ANSWERED_REASON VARCHAR(150),          
  @CURRENT_PAGE    INT,         
  @RECORD_PER_PAGE INT,         
  @SEARCH_TEXT VARCHAR(100),         
  @SORT_BY  VARCHAR(50),        
  @SORT_ORDER VARCHAR(5))        
AS        
BEGIN          
     
    
    
 --declare          
 -- @PRACTICE_CODE   BIGINT = '1012714',         
 -- @DATE_FROM       DATETIME = '7/14/1923 3:37:35 PM',         
 -- @DATE_TO DATETIME = '7/14/2023 3:37:35 PM',         
 -- @PROVIDER VARCHAR(100) = '',         
 -- @REGION  VARCHAR(100) = '',         
 -- @STATE VARCHAR(10) = '',        
 -- @FLAG  VARCHAR(10) = '',             
 -- @FORMAT  VARCHAR(10) = 'ALL',          
 -- @SURVEYED_BY     VARCHAR(100) = '',         
 -- @SURVEYED_STATUS VARCHAR(500) = 'Completed Survey ,Deceased,Unable to Complete Survey,Not Interested,Callback,Not Answered,New Case Same Discipline,New Case Same Discipline',     
 -- @NOT_ANSWERED_REASON VARCHAR(150) = 'Line Busy,MailBox Full,Wrong PH#,VM Left',          
 -- @CURRENT_PAGE    INT = 1,        
 -- @RECORD_PER_PAGE INT = 5000,               
 -- @SEARCH_TEXT     VARCHAR(100) = '',        
 -- @SORT_BY VARCHAR(50) = 'SURVEYCOMPLETEDDATE',        
 -- @SORT_ORDER      VARCHAR(5)  = 'Desc'             
   IF(@SORT_ORDER = '' )          
   BEGIN        
 SET @SORT_ORDER =  'DESC'          
 END         
  IF(@SORT_BY = '' )          
   BEGIN        
 SET @SORT_BY =  'SURVEYDATE'          
 END          
 IF(@FLAG = '')         
  BEGIN         
 SET @FLAG =  NULL              
 END        
 IF(@NOT_ANSWERED_REASON = '' )        
   BEGIN         
 SET @NOT_ANSWERED_REASON =  null              
 END          
            
    SET @CURRENT_PAGE = @CURRENT_PAGE - 1        
    DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE        
    DECLARE @TOATL_PAGESUDM FLOAT        
            
 if(@SURVEYED_STATUS = 'Pending')        
 begin        
 set @DATE_FROM = null        
 set @DATE_TO = null        
 end           
    
 --drop table #SURVEYDATATEMP    
 --drop table #SURVEYDATATEMP_NOTANSWERED    
 IF OBJECT_ID('TEMPDB.DBO.#SURVEYDATATEMP', 'U') IS NOT NULL DROP TABLE #SURVEYDATATEMP;        
 SELECT          
 NA.NOT_ANSWERED_REASON,                    
 PS.SURVEY_ID,             
 PS.FEEDBACK,        
      CASE        
       WHEN ISNULL(PS.SURVEY_STATUS_CHILD, '') = ''        
       THEN 'Pending'        
       ELSE PS.SURVEY_STATUS_CHILD        
      END AS SURVEY_STATUS_CHILD,             
      PS.SURVEY_STATUS_BASE AS SURVEY_STATUS_BASE,        
         
 PS.MODIFIED_DATE,        
 PS.SURVEY_COMPLETED_DATE,               
 --SL.IS_SMS AS IS_SMS,              
 --SL.IS_EMAIL AS IS_EMAIL,         
 CONVERT(VARCHAR(10), CAST( PS.SURVEY_COMPLETED_DATE AS TIME), 0) AS SURVEY_COMPLETED_TIME_STR,        
 CONVERT(VARCHAR(15), CAST( PS.SURVEY_COMPLETED_DATE AS DATE), 0) AS SURVEY_COMPLETED_DATE_STR,            
 PS.Created_Date,         
 CONVERT(VARCHAR, PS.MODIFIED_DATE) AS Modified_Date_Str,         
 PS.SURVEY_FLAG,        
    
         
 (select Count(*) from FOX_TBL_PATIENT_SURVEY_CALL_LOG WITH (NOLOCK) where PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER AND PS.PRACTICE_CODE = @PRACTICE_CODE  ) AS HAS_CALL_PATH,         
 ROW_NUMBER() OVER(ORDER BY PS.MODIFIED_DATE DESC) AS ACTIVEROW        
 INTO #SURVEYDATATEMP              
 FROM         
 FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)        
  left JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID         
                   AND ISNULL(SL.DELETED, 0) = 0        
  left JOIN FOX_TBL_PATIENT_SURVEY_NOT_ANSWERED_REASON NA WITH (NOLOCK) ON PS.SURVEY_ID = NA.SURVEY_ID         
                   AND ISNULL(NA.DELETED, 0) = 0        
  LEFT JOIN FOX_TBL_APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY          
  --LEFT join FOX_TBL_PATIENT_SURVEY_CALL_LOG as CL   on cl.PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER            
     WHERE         
  ISNULL(PS.DELETED, 0) = 0        
  AND PS.PRACTICE_CODE = @PRACTICE_CODE        
  AND PS.REGION <> ''          
  --AND ((@IS_SURVEYED IS NULL AND ISNULL(SURVEY_COMPLETED_DATE, '') = '') OR ISNULL(SURVEY_COMPLETED_DATE, '') <> @IS_SURVEYED)              
  AND (@SURVEYED_STATUS = 'Pending' OR PS.SURVEY_FORMAT_TYPE LIKE CASE WHEN @FORMAT = 'ALL' THEN '%Format%' ELSE @FORMAT END)         
  AND PS.REGION LIKE '%'+@REGION+'%'         
  AND PS.PROVIDER LIKE '%'+@PROVIDER+'%'        
  AND AU.USER_NAME LIKE '%'+@SURVEYED_BY+'%'        
  AND ((PS.SURVEY_STATUS_CHILD IN (SELECT Item FROM dbo.SplitStrings_CTE(@SURVEYED_STATUS, N','))        
  --AND (@NOT_ANSWERED_REASON IS NULL OR NA.NOT_ANSWERED_REASON IN (SELECT Item FROM dbo.SplitStrings_CTE(@NOT_ANSWERED_REASON, N',')))          
  AND @SURVEYED_STATUS <> 'Pending'        
  --AND ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED         
     )         
     OR (@SURVEYED_STATUS = 'Pending' AND ISNULL(PS.IS_SURVEYED, 0) = 0)        
     --OR ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED        
    )        
       AND (@DATE_FROM IS NULL OR @DATE_TO IS NULL OR CONVERT(DATE, PS.SURVEY_COMPLETED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))          
     AND (PS.PATIENT_ACCOUNT_NUMBER LIKE @SEARCH_TEXT+'%'          
 OR PS.PATIENT_LAST_NAME LIKE @SEARCH_TEXT+'%'          
 OR PS.PATIENT_FIRST_NAME LIKE @SEARCH_TEXT+'%'        
 OR PS.PATIENT_STATE LIKE @SEARCH_TEXT+'%'              
 OR PS.PT_OT_SLP LIKE @SEARCH_TEXT+'%'        
 OR PS.REGION LIKE '%' + @SEARCH_TEXT+'%'          
 OR PS.PROVIDER LIKE '%' + @SEARCH_TEXT+'%'         
 OR PS.ATTENDING_DOCTOR_NAME LIKE '%' + @SEARCH_TEXT+'%'            
 OR PS.SURVEY_STATUS_CHILD LIKE @SEARCH_TEXT+'%'        
 OR PS.SURVEY_STATUS_BASE LIKE @SEARCH_TEXT+'%'        
 OR PS.FEEDBACK LIKE '%' + @SEARCH_TEXT+'%'        
 OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'        
 OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'         
 OR AU.FIRST_NAME LIKE @SEARCH_TEXT+'%'        
 OR AU.LAST_NAME LIKE @SEARCH_TEXT+'%')         
 AND ((@FLAG IS NULL) OR (@FLAG IS NOT NULL AND PS.SURVEY_FLAG = @FLAG) OR (@FLAG = 'Both' AND PS.SURVEY_FLAG <> ''))        
 AND(@STATE = '' OR (@STATE <> '' AND PATIENT_STATE IN (SELECT Item FROM dbo.SplitStrings_CTE(@STATE, N','))))        
        
  ---SELECT *  FROM #SURVEYDATATEMP    
  --- group by SURVEY_STATUS_CHILD        
    
 IF OBJECT_ID('TEMPDB.DBO.#SURVEYDATATEMP_NOTANSWERED', 'U') IS NOT NULL DROP TABLE #SURVEYDATATEMP_NOTANSWERED;        
 SELECT          
 NA.NOT_ANSWERED_REASON,                    
 PS.SURVEY_ID,             
 PS.FEEDBACK,        
      CASE        
       WHEN ISNULL(PS.SURVEY_STATUS_CHILD, '') = ''        
       THEN 'Pending'        
       --ELSE PS.SURVEY_STATUS_CHILD        
    ELSE  NA.NOT_ANSWERED_REASON    
      END AS SURVEY_STATUS_CHILD,     
      PS.SURVEY_STATUS_BASE AS SURVEY_STATUS_BASE,        
         
 PS.MODIFIED_DATE,        
 PS.SURVEY_COMPLETED_DATE,               
 --SL.IS_SMS AS IS_SMS,              
 --SL.IS_EMAIL AS IS_EMAIL,         
 CONVERT(VARCHAR(10), CAST( PS.SURVEY_COMPLETED_DATE AS TIME), 0) AS SURVEY_COMPLETED_TIME_STR,        
 CONVERT(VARCHAR(15), CAST( PS.SURVEY_COMPLETED_DATE AS DATE), 0) AS SURVEY_COMPLETED_DATE_STR,            
 PS.Created_Date,         
 CONVERT(VARCHAR, PS.MODIFIED_DATE) AS Modified_Date_Str,         
 PS.SURVEY_FLAG,        
    
         
 (select Count(*) from FOX_TBL_PATIENT_SURVEY_CALL_LOG WITH (NOLOCK) where PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER AND PS.PRACTICE_CODE = @PRACTICE_CODE  ) AS HAS_CALL_PATH,         
 ROW_NUMBER() OVER(ORDER BY PS.MODIFIED_DATE DESC) AS ACTIVEROW        
 INTO #SURVEYDATATEMP_NOTANSWERED              
 FROM         
 FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)        
  left JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID         
                   AND ISNULL(SL.DELETED, 0) = 0        
  left JOIN FOX_TBL_PATIENT_SURVEY_NOT_ANSWERED_REASON NA WITH (NOLOCK) ON PS.SURVEY_ID = NA.SURVEY_ID         
                   AND ISNULL(NA.DELETED, 0) = 0        
  LEFT JOIN FOX_TBL_APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY          
  --LEFT join FOX_TBL_PATIENT_SURVEY_CALL_LOG as CL   on cl.PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER            
     WHERE         
  ISNULL(PS.DELETED, 0) = 0        
  AND PS.PRACTICE_CODE = @PRACTICE_CODE        
  AND PS.REGION <> ''          
  --AND ((@IS_SURVEYED IS NULL AND ISNULL(SURVEY_COMPLETED_DATE, '') = '') OR ISNULL(SURVEY_COMPLETED_DATE, '') <> @IS_SURVEYED)              
  AND (@SURVEYED_STATUS = 'Pending' OR PS.SURVEY_FORMAT_TYPE LIKE CASE WHEN @FORMAT = 'ALL' THEN '%Format%' ELSE @FORMAT END)         
  AND PS.REGION LIKE '%'+@REGION+'%'         
  AND PS.PROVIDER LIKE '%'+@PROVIDER+'%'        
  AND AU.USER_NAME LIKE '%'+@SURVEYED_BY+'%'        
  --AND ((PS.SURVEY_STATUS_CHILD IN (SELECT Item FROM dbo.SplitStrings_CTE(@SURVEYED_STATUS, N','))        
  AND ((PS.SURVEY_STATUS_CHILD = 'Not Answered'      
  AND (@NOT_ANSWERED_REASON IS NULL OR NA.NOT_ANSWERED_REASON IN (SELECT Item FROM dbo.SplitStrings_CTE(@NOT_ANSWERED_REASON, N',')))          
  AND @SURVEYED_STATUS <> 'Pending'        
  --AND ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED         
     )         
     OR (@SURVEYED_STATUS = 'Pending' AND ISNULL(PS.IS_SURVEYED, 0) = 0)        
     --OR ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED        
    )        
       AND (@DATE_FROM IS NULL OR @DATE_TO IS NULL OR CONVERT(DATE, PS.SURVEY_COMPLETED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))          
     AND (PS.PATIENT_ACCOUNT_NUMBER LIKE @SEARCH_TEXT+'%'          
 OR PS.PATIENT_LAST_NAME LIKE @SEARCH_TEXT+'%'          
 OR PS.PATIENT_FIRST_NAME LIKE @SEARCH_TEXT+'%'        
 OR PS.PATIENT_STATE LIKE @SEARCH_TEXT+'%'              
 OR PS.PT_OT_SLP LIKE @SEARCH_TEXT+'%'        
 OR PS.REGION LIKE '%' + @SEARCH_TEXT+'%'          
 OR PS.PROVIDER LIKE '%' + @SEARCH_TEXT+'%'         
 OR PS.ATTENDING_DOCTOR_NAME LIKE '%' + @SEARCH_TEXT+'%'            
 OR PS.SURVEY_STATUS_CHILD LIKE @SEARCH_TEXT+'%'        
 OR PS.SURVEY_STATUS_BASE LIKE @SEARCH_TEXT+'%'        
 OR PS.FEEDBACK LIKE '%' + @SEARCH_TEXT+'%'        
 OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'        
 OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'         
 OR AU.FIRST_NAME LIKE @SEARCH_TEXT+'%'        
 OR AU.LAST_NAME LIKE @SEARCH_TEXT+'%')         
 AND ((@FLAG IS NULL) OR (@FLAG IS NOT NULL AND PS.SURVEY_FLAG = @FLAG) OR (@FLAG = 'Both' AND PS.SURVEY_FLAG <> ''))        
 AND(@STATE = '' OR (@STATE <> '' AND PATIENT_STATE IN (SELECT Item FROM dbo.SplitStrings_CTE(@STATE, N','))))      
    
     
 --Select * From #SURVEYCOUNT_INCOMPLETE    
 IF OBJECT_ID('TEMPDB.DBO.#SURVEYCOUNT_INCOMPLETE', 'U') IS NOT NULL DROP TABLE #SURVEYCOUNT_INCOMPLETE;     
 SELECT 'INCOMPLETE' AS SURVEY_STATUS_CHILD, SUM(CountNo) AS CountNo    
 INTO #SURVEYCOUNT_INCOMPLETE     
FROM    
(    
  SELECT Convert(BIGINT,COUNT(SURVEY_STATUS_CHILD)) AS CountNo    
  FROM #SURVEYDATATEMP    
  WHERE SURVEY_STATUS_CHILD IN (SELECT Item FROM dbo.SplitStrings_CTE('Callback,Not Answered,New Case Same Discipline', N','))    
) AS subquery;    
    
 IF OBJECT_ID('TEMPDB.DBO.#SURVEYCOUNT_COMPLETED', 'U') IS NOT NULL DROP TABLE #SURVEYCOUNT_COMPLETED;     
 SELECT 'COMPLETED' AS SURVEY_STATUS_CHILD, SUM(CountNo) AS CountNo    
 INTO #SURVEYCOUNT_COMPLETED     
FROM    
(    
  SELECT COUNT(SURVEY_STATUS_CHILD) AS CountNo    
  FROM #SURVEYDATATEMP    
  WHERE SURVEY_STATUS_CHILD IN (SELECT Item FROM dbo.SplitStrings_CTE('Completed Survey ,Deceased,Unable to Complete Survey,Not Interested', N','))    
) AS subquery;    
    
 IF OBJECT_ID('TEMPDB.DBO.#SURVEYCOUNT_SERVICES_PROVIDE', 'U') IS NOT NULL DROP TABLE #SURVEYCOUNT_SERVICES_PROVIDE;     
 SELECT 'NOT_ENOUGH_SERVICES_PROVIDE' AS SURVEY_STATUS_CHILD, SUM(CountNo) AS CountNo    
 INTO #SURVEYCOUNT_SERVICES_PROVIDE     
FROM    
(    
  SELECT COUNT(SURVEY_STATUS_CHILD) AS CountNo    
  FROM #SURVEYDATATEMP    
  WHERE SURVEY_STATUS_CHILD IN (SELECT Item FROM dbo.SplitStrings_CTE('Unable to Complete Survey', N','))    
) AS subquery;    
    
-------------------------------Check Pending---------------------------    
    
    IF OBJECT_ID('TEMPDB.DBO.#SURVEYDATA_PENDING', 'U') IS NOT NULL DROP TABLE #SURVEYDATA_PENDING;       
   set @SURVEYED_STATUS = 'Pending'     
   if(@SURVEYED_STATUS = 'Pending')        
 begin        
 set @DATE_FROM = null        
 set @DATE_TO = null        
 end       
 SELECT          
 NA.NOT_ANSWERED_REASON,                    
 PS.SURVEY_ID,             
 PS.FEEDBACK,        
      CASE        
       WHEN ISNULL(PS.SURVEY_STATUS_CHILD, '') = ''        
       THEN 'Pending'        
       ELSE PS.SURVEY_STATUS_CHILD        
      END AS SURVEY_STATUS_CHILD,             
      PS.SURVEY_STATUS_BASE AS SURVEY_STATUS_BASE,        
         
 PS.MODIFIED_DATE,        
 PS.SURVEY_COMPLETED_DATE,               
 --SL.IS_SMS AS IS_SMS,              
 --SL.IS_EMAIL AS IS_EMAIL,         
 CONVERT(VARCHAR(10), CAST( PS.SURVEY_COMPLETED_DATE AS TIME), 0) AS SURVEY_COMPLETED_TIME_STR,        
 CONVERT(VARCHAR(15), CAST( PS.SURVEY_COMPLETED_DATE AS DATE), 0) AS SURVEY_COMPLETED_DATE_STR,            
 PS.Created_Date,         
 CONVERT(VARCHAR, PS.MODIFIED_DATE) AS Modified_Date_Str,         
 PS.SURVEY_FLAG,        
    
         
 (select Count(*) from FOX_TBL_PATIENT_SURVEY_CALL_LOG WITH (NOLOCK) where PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER AND PS.PRACTICE_CODE = @PRACTICE_CODE  ) AS HAS_CALL_PATH,         
 ROW_NUMBER() OVER(ORDER BY PS.MODIFIED_DATE DESC) AS ACTIVEROW        
 INTO #SURVEYDATA_PENDING              
 FROM         
 FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)        
  left JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID         
                   AND ISNULL(SL.DELETED, 0) = 0        
  left JOIN FOX_TBL_PATIENT_SURVEY_NOT_ANSWERED_REASON NA WITH (NOLOCK) ON PS.SURVEY_ID = NA.SURVEY_ID         
                   AND ISNULL(NA.DELETED, 0) = 0        
  LEFT JOIN FOX_TBL_APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY          
  --LEFT join FOX_TBL_PATIENT_SURVEY_CALL_LOG as CL   on cl.PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER            
     WHERE ISNULL(PS.DELETED, 0) = 0                                                            
        AND PS.PRACTICE_CODE = @PRACTICE_CODE                                         
        AND PS.REGION <> ''                                         
     --AND (@IS_SURVEYED IS NULL OR ISNULL(SURVEY_COMPLETED_DATE, '') <> @IS_SURVEYED)                                    
     AND (@SURVEYED_STATUS = 'Pending'                                                          
     OR PS.SURVEY_FORMAT_TYPE LIKE                                            
     CASE                                           
     WHEN @FORMAT = 'ALL'                                                          
     THEN '%Format%'                                                          
     ELSE @FORMAT                                                          
     END                                                          
     )                                               
        AND PS.REGION LIKE '%'+@REGION+'%'                                                            
        AND PS.PROVIDER LIKE '%'+@PROVIDER+'%'                                                            
        AND PS.PATIENT_STATE LIKE '%'+@STATE+'%'                                                            
        AND AU.USER_NAME LIKE '%'+@SURVEYED_BY+'%'                        
        AND (                                                            
     (                                                            
   PS.SURVEY_STATUS_CHILD IN                                                            
    (                                                            
     SELECT Item                                                            
     FROM dbo.SplitStrings_CTE(@SURVEYED_STATUS, N',')                                                            
    )                                                            
    AND @SURVEYED_STATUS <> 'Pending'                                                            
   ----AND ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                            
    )                                                            
     OR (@SURVEYED_STATUS = 'Pending' AND ISNULL(PS.IS_SURVEYED, 0) = 0)                                                            
   --  --OR ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                            
   )                                                            
       AND (@DATE_FROM IS NULL              
         OR @DATE_TO IS NULL                                                            
         OR CONVERT(DATE, PS.CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))                                                            
       AND (PS.PATIENT_ACCOUNT_NUMBER LIKE @SEARCH_TEXT+'%'                                                            
         OR PS.PATIENT_LAST_NAME LIKE @SEARCH_TEXT+'%'                                                        
         OR PS.PATIENT_FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                            
         OR PS.PATIENT_STATE LIKE @SEARCH_TEXT+'%'                            
         OR PS.PT_OT_SLP LIKE @SEARCH_TEXT+'%'                                                            
         OR PS.REGION LIKE '%' + @SEARCH_TEXT+'%'                     
  OR PS.PROVIDER LIKE '%' + @SEARCH_TEXT+'%'                                                            
         OR PS.SURVEY_STATUS_CHILD LIKE @SEARCH_TEXT+'%'                           
   OR PS.SURVEY_STATUS_BASE LIKE @SEARCH_TEXT+'%'                                                 
   OR PS.FEEDBACK LIKE '%' +@SEARCH_TEXT+'%'                     
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'                         
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'                                                           
         OR AU.FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                            
         OR AU.LAST_NAME LIKE @SEARCH_TEXT+'%'                                                                                   
         OR PS.ATTENDING_DOCTOR_NAME LIKE  '%' + @SEARCH_TEXT+'%')          
    
 -------------------------------Check Pending for 30 days---------------------------    
    
    IF OBJECT_ID('TEMPDB.DBO.#SURVEYDATA_PENDING_FOR_30_DAYS', 'U') IS NOT NULL DROP TABLE #SURVEYDATA_PENDING_FOR_30_DAYS;       
   set @SURVEYED_STATUS = 'Pending'     
 --  if(@SURVEYED_STATUS = 'Pending')        
 --begin        
 --set @DATE_FROM = null        
 --set @DATE_TO = null        
 --end       
 set @DATE_FROM =  DATEADD(DAY, -30, GETDATE());    
  set @DATE_TO =  GETDATE();    
    
 SELECT          
 NA.NOT_ANSWERED_REASON,                    
 PS.SURVEY_ID,             
 PS.FEEDBACK,        
      CASE        
        WHEN ISNULL(PS.SURVEY_STATUS_CHILD, '') = ''    
       THEN 'Pending30'        
       ELSE PS.SURVEY_STATUS_CHILD        
      END AS SURVEY_STATUS_CHILD,             
      PS.SURVEY_STATUS_BASE AS SURVEY_STATUS_BASE,        
         
 PS.MODIFIED_DATE,        
 PS.SURVEY_COMPLETED_DATE,               
 --SL.IS_SMS AS IS_SMS,              
 --SL.IS_EMAIL AS IS_EMAIL,         
 CONVERT(VARCHAR(10), CAST( PS.SURVEY_COMPLETED_DATE AS TIME), 0) AS SURVEY_COMPLETED_TIME_STR,        
 CONVERT(VARCHAR(15), CAST( PS.SURVEY_COMPLETED_DATE AS DATE), 0) AS SURVEY_COMPLETED_DATE_STR,            
 PS.Created_Date,         
 CONVERT(VARCHAR, PS.MODIFIED_DATE) AS Modified_Date_Str,         
 PS.SURVEY_FLAG,        
    
         
 (select Count(*) from FOX_TBL_PATIENT_SURVEY_CALL_LOG WITH (NOLOCK) where PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER AND PS.PRACTICE_CODE = @PRACTICE_CODE  ) AS HAS_CALL_PATH,         
 ROW_NUMBER() OVER(ORDER BY PS.MODIFIED_DATE DESC) AS ACTIVEROW        
 INTO #SURVEYDATA_PENDING_FOR_30_DAYS              
 FROM         
 FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)        
  left JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID         
                   AND ISNULL(SL.DELETED, 0) = 0        
  left JOIN FOX_TBL_PATIENT_SURVEY_NOT_ANSWERED_REASON NA WITH (NOLOCK) ON PS.SURVEY_ID = NA.SURVEY_ID         
                   AND ISNULL(NA.DELETED, 0) = 0        
  LEFT JOIN FOX_TBL_APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY          
  --LEFT join FOX_TBL_PATIENT_SURVEY_CALL_LOG as CL   on cl.PATIENT_ACCOUNT = ps.PATIENT_ACCOUNT_NUMBER            
     WHERE ISNULL(PS.DELETED, 0) = 0                                                            
        AND PS.PRACTICE_CODE = @PRACTICE_CODE                                         
        AND PS.REGION <> ''                                         
     --AND (@IS_SURVEYED IS NULL OR ISNULL(SURVEY_COMPLETED_DATE, '') <> @IS_SURVEYED)                                    
     AND (@SURVEYED_STATUS = 'Pending'                                                          
     OR PS.SURVEY_FORMAT_TYPE LIKE                                            
     CASE                                                           
     WHEN @FORMAT = 'ALL'                                                          
     THEN '%Format%'                                                          
     ELSE @FORMAT                                                          
     END                                                          
     )                                               
        AND PS.REGION LIKE '%'+@REGION+'%'                                                            
        AND PS.PROVIDER LIKE '%'+@PROVIDER+'%'                                                            
        AND PS.PATIENT_STATE LIKE '%'+@STATE+'%'                                                            
        AND AU.USER_NAME LIKE '%'+@SURVEYED_BY+'%'                        
        AND (                                                            
     (                                                            
   PS.SURVEY_STATUS_CHILD IN                                                            
    (                                                            
     SELECT Item                                                            
     FROM dbo.SplitStrings_CTE(@SURVEYED_STATUS, N',')                                                            
    )                                                            
    AND @SURVEYED_STATUS <> 'Pending'                                                            
   ----AND ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                            
    )                                                            
     OR (@SURVEYED_STATUS = 'Pending' AND ISNULL(PS.IS_SURVEYED, 0) = 0)                                                            
   --  --OR ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                            
   )                                                            
       AND (@DATE_FROM IS NULL              
         OR @DATE_TO IS NULL                                                            
         OR CONVERT(DATE, PS.CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))                                                            
       AND (PS.PATIENT_ACCOUNT_NUMBER LIKE @SEARCH_TEXT+'%'                                                            
         OR PS.PATIENT_LAST_NAME LIKE @SEARCH_TEXT+'%'                                                        
         OR PS.PATIENT_FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                            
         OR PS.PATIENT_STATE LIKE @SEARCH_TEXT+'%'                            
         OR PS.PT_OT_SLP LIKE @SEARCH_TEXT+'%'                                                            
         OR PS.REGION LIKE '%' + @SEARCH_TEXT+'%'                     
  OR PS.PROVIDER LIKE '%' + @SEARCH_TEXT+'%'                                                            
         OR PS.SURVEY_STATUS_CHILD LIKE @SEARCH_TEXT+'%'                           
   OR PS.SURVEY_STATUS_BASE LIKE @SEARCH_TEXT+'%'                                                 
   OR PS.FEEDBACK LIKE '%' +@SEARCH_TEXT+'%'                     
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'                         
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'                                                           
         OR AU.FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                            
         OR AU.LAST_NAME LIKE @SEARCH_TEXT+'%'                                                                                   
         OR PS.ATTENDING_DOCTOR_NAME LIKE  '%' + @SEARCH_TEXT+'%')         
    
  SELECT cONVERT(varchar,SURVEY_STATUS_CHILD) AS SURVEY_STATUS_CHILD,conVERT(bigint,count(SURVEY_STATUS_CHILD)) as CountNo  FROM #SURVEYDATATEMP WITH (NOLOCK) group by SURVEY_STATUS_CHILD       
  UNion     
 SELECT * FROM #SURVEYCOUNT_INCOMPLETE WITH (NOLOCK) --group by SURVEY_STATUS_CHILD       
   UNion     
  SELECT * FROM #SURVEYCOUNT_COMPLETED WITH (NOLOCK) -- group by SURVEY_STATUS_CHILD       
    UNion     
   SELECT *  FROM #SURVEYCOUNT_SERVICES_PROVIDE WITH (NOLOCK) ---group by SURVEY_STATUS_CHILD       
  UNion     
   SELECT cONVERT(varchar,SURVEY_STATUS_CHILD) AS SURVEY_STATUS_CHILD,conVERT(bigint,count(SURVEY_STATUS_CHILD)) as CountNo FROM #SURVEYDATATEMP_NOTANSWERED WITH (NOLOCK) group by SURVEY_STATUS_CHILD       
     UNion     
   SELECT cONVERT(varchar,SURVEY_STATUS_CHILD) AS SURVEY_STATUS_CHILD,conVERT(bigint,count(SURVEY_STATUS_CHILD)) as CountNo  FROM #SURVEYDATA_PENDING WITH (NOLOCK) group by SURVEY_STATUS_CHILD     
     UNion     
   SELECT cONVERT(varchar,SURVEY_STATUS_CHILD) AS SURVEY_STATUS_CHILD,conVERT(bigint,count(SURVEY_STATUS_CHILD)) as CountNo FROM #SURVEYDATA_PENDING_FOR_30_DAYS WITH (NOLOCK) group by SURVEY_STATUS_CHILD       
       
  END
  GO