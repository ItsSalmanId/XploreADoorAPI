-- =============================================                                                  
-- AUTHOR:  <DEVELOPER,Unknown >                                                  
-- Modified by: <Taseer Iqbal, 01/27/2023>       
-- Modified by: <Abdul Sattar, 10/27/2023>   
-- DESCRIPTION:  THIS PROCEDURE IS use to get Task details                                           
-- =============================================                            
ALTER PROCEDURE [dbo].[FOX_PROC_GET_TASK_DETAIL_LIST]                             
@PRACTICE_CODE   BIGINT,                                                   
@PATIENT_ACCOUNT BIGINT,                                                   
@CASE_ID         BIGINT,                                                   
@OPTION          VARCHAR(20),                                                   
@USER_ID         BIGINT,                                                   
@CURRENT_PAGE    INT,                                                   
@RECORD_PER_PAGE INT,                                                   
@SEARCH_TEXT     VARCHAR(30),                                                   
@SORT_BY         VARCHAR(50),                                                   
@SORT_ORDER      VARCHAR(5),                                                
@INSURANCE_ID    BIGINT,    
@TASK_TYPE_ID    BIGINT,                                                   
@TASK_SUB_TYPE_ID  BIGINT,                                                   
@PROVIDER_ID         BIGINT,                                                   
@REGION          VARCHAR(50),    
@LOC_ID         BIGINT,    
@CERTIFYING_REF_SOURCE_ID        BIGINT,    
@CERTIFYING_REF_SOURCE_FAX       VARCHAR(50),    
@PATIENT_ZIP_CODE       VARCHAR(50),                                                   
@DUE_DATE_TIME DATETIME         ,                                                
@DATE_FROM DATETIME,                                                
@DATE_TO DATETIME,                                                
@OWNER_ID        BIGINT,                                              
@MODIFIED_BY VARCHAR(70),                            
@IS_USER_LEVEL bit                            
AS                                                  
                          
BEGIN                                                  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED    
--declare @PRACTICE_CODE   BIGINT= 1012714,                                                   
--@PATIENT_ACCOUNT BIGINT= null,                                                   
--@CASE_ID         BIGINT= null,                                                   
--@OPTION          VARCHAR(20)= 'All',                                                   
--@USER_ID         BIGINT= 6006242,                                                   
--@CURRENT_PAGE    INT= 1,                                                   
--@RECORD_PER_PAGE INT= 500,                                                   
--@SEARCH_TEXT     VARCHAR(30)= '',                                                   
--@SORT_BY         VARCHAR(50)= 'created_date',                                                   
--@SORT_ORDER      VARCHAR(5)= 'desc',                                                
--@INSURANCE_ID    BIGINT= null,                                                   
--@TASK_TYPE_ID    BIGINT= null,                                                   
--@TASK_SUB_TYPE_ID  BIGINT= null,                                                   
--@PROVIDER_ID         BIGINT=null ,                                                   
--@REGION          VARCHAR(50)= '',                                                   
--@LOC_ID         BIGINT= null,                                                   
--@CERTIFYING_REF_SOURCE_ID        BIGINT= null,                                                   
--@CERTIFYING_REF_SOURCE_FAX       VARCHAR(50)= '',                                                              
--@PATIENT_ZIP_CODE       VARCHAR(50)= '',                                                   
--@DUE_DATE_TIME DATETIME        =  null,                                                
--@DATE_FROM DATETIME= '2023-09-01 03:22:44.470',                                                
--@DATE_TO DATETIME= '2023-09-05 03:22:44.470',                                                
--@OWNER_ID        BIGINT= 6006242,                                              
--@MODIFIED_BY VARCHAR(70)= '',     
--@IS_USER_LEVEL bit=1;                                                        
DECLARE                    
@PRACTICECODE BIGINT= @PRACTICE_CODE,                             
@PATIENTACCOUNT BIGINT= @PATIENT_ACCOUNT,                             
@CASEID BIGINT= @CASE_ID,                             
@OPTIION VARCHAR(20)= @OPTION,                             
@USERID BIGINT= @USER_ID,                             
@CURRENTPAGE INT= @CURRENT_PAGE,                             
@RECORDPER_PAGE INT= @RECORD_PER_PAGE,                                 
@SEARCHTEXT VARCHAR(30)= @SEARCH_TEXT,                             
@SORTBY VARCHAR(50)= @SORT_BY,                             
@SORTORDER VARCHAR(5)= @SORT_ORDER,                            
@USER_NAME VARCHAR(100),                            
@OWNER_NAME VARCHAR(100);    
-------FOR Year    
declare @YEAR bigint = 2022--year(getdate());                             
IF (isnull(@OWNER_ID,'') <> '')                                          
BEGIN                                          
 SET @USERID = @OWNER_ID                                          
END                            
else                            
begin                            
SET @OWNER_ID = @USERID                            
end                            
IF (@PATIENTACCOUNT = '')                                          
BEGIN                                          
 SET @PATIENTACCOUNT = NULL                                          
END                            
SELECT @USER_NAME = USER_NAME FROM FOX_TBL_APPLICATION_USER  with (nolock, nowait) WHERE USER_ID = @USERID;                                                                      
SELECT @OWNER_NAME = USER_NAME FROM FOX_TBL_APPLICATION_USER  with (nolock, nowait) WHERE USER_ID = @OWNER_ID;    
IF (@MODIFIED_BY = '')                                          
BEGIN                                          
 SET @MODIFIED_BY = NULL                                          
END                   
    
if object_id('tempdb..#tasks') is not null drop table #tasks    
select * into #tasks from FOX_TBL_TASK with (nolock, nowait) where PRACTICE_CODE = @PRACTICECODE AND ISNULL(DELETED, 0) = 0 AND ISNULL(IS_TEMPLATE, 0) = 0 AND YEAR(Created_Date)>=@YEAR    
AND (@PATIENTACCOUNT IS NULL OR PATIENT_ACCOUNT = @PATIENTACCOUNT)    
AND (@CASEID IS NULL OR CASE_ID = @CASEID)    
AND (@TASK_TYPE_ID IS NULL OR TASK_TYPE_ID = @TASK_TYPE_ID)    
AND (@PROVIDER_ID IS NULL OR PROVIDER_ID = @PROVIDER_ID)    
AND (@LOC_ID IS NULL OR LOC_ID = @LOC_ID)    
AND (@MODIFIED_BY IS NULL OR MODIFIED_BY = @MODIFIED_BY)    
AND (@DUE_DATE_TIME IS NULL OR CONVERT(DATE, DUE_DATE_TIME,101) =CONVERT(DATE, @DUE_DATE_TIME,101))    
AND ( CONVERT(DATE, CREATED_DATE,101) BETWEEN CONVERT(DATE, @DATE_FROM,101) AND CONVERT(DATE, @DATE_TO,101))    
AND ((@OPTIION = 'ALL' AND ((@IS_USER_LEVEL = 1 AND IS_SENDTO_MARK_COMPLETE <> 1) or @IS_USER_LEVEL = 0))    
OR (    
@OPTIION = 'OPEN' AND ((IS_SEND_TO_USER = 1 AND SEND_TO_ID = @USERID AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)                                         
OR (IS_SEND_TO_USER = 0 AND SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_USER_GROUP with (nolock, nowait) WHERE USER_NAME = @USER_NAME AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0)= 0) AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)               
OR (IS_SEND_TO_USER = 0 AND SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_GROUP  with (nolock, nowait) where ISNULL(DELETED,0)= 0 AND PRACTICE_CODE = @PRACTICE_CODE) AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)    
))    
OR (@OPTIION = 'OPEN SENT'  AND (CREATED_BY = @USER_NAME AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)))    
AND ((@OWNER_ID IS NULL OR SEND_TO_ID = @OWNER_ID  or SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_USER_GROUP  with (nolock, nowait) WHERE USER_NAME = @OWNER_NAME AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0)= 0))     
or ((@OPTIION = 'OPEN SENT' or @OPTIION = 'ALL') and CREATED_BY = @USER_NAME))    
              
if object_id('tempdb..#a') is not null drop table #a              
SELECT TST_I.[NAME],t_I.TASK_ID into #a       
FROM #tasks AS T_I  -- with (nolock, nowait, forceseek)              
                  INNER JOIN FOX_TBL_TASK_TYPE AS TT_I  with (nolock, nowait, forceseek) ON TT_I.TASK_TYPE_ID = T_I.TASK_TYPE_ID                  
                                                          AND TT_I.PRACTICE_CODE = @PRACTICECODE AND ISNULL(TT_I.DELETED, 0) = 0                  
                                                                          
                  INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS TTST_I with (nolock, nowait, forceseek) ON TTST_I.TASK_ID = T_I.TASK_ID               
                                                          AND TTST_I.PRACTICE_CODE = @PRACTICECODE AND ISNULL(TTST_I.DELETED, 0) = 0                
               AND YEAR(TTST_I.Created_Date)>=@YEAR    
                  INNER JOIN FOX_TBL_TASK_SUB_TYPE AS TST_I with (nolock, nowait, forceseek) ON TST_I.TASK_SUB_TYPE_ID = TTST_I.TASK_SUB_TYPE_ID                 
                                                          AND TST_I.PRACTICE_CODE = @PRACTICECODE AND TST_I.TASK_TYPE_ID = TT_I.TASK_TYPE_ID                 
                                                          AND ISNULL(TST_I.DELETED, 0) = 0                     
                                                                            
             WHERE  YEAR(T_I.Created_Date)>=@YEAR AND T_I.PRACTICE_CODE = @PRACTICECODE AND  ISNULL(T_I.DELETED, 0) = 0                 
                         
                                        
IF OBJECT_ID('TEMPDB..#TASK_SUB_TYPES') IS NOT NULL DROP TABLE #TASK_SUB_TYPES                            
SELECT                   
T.TASK_ID,                                                  
TASK_SUBTYPES = STUFF(                                                  
(                            
SELECT DISTINCT ', '+T_I.[NAME] FROM  #a as T_I               
                   where T_I.TASK_ID = T.TASK_ID FOR XML PATH, TYPE                                                  
         ).value(N'.[1]', N'nvarchar(max)'), 1, 1, '')                                    
         INTO #TASK_SUB_TYPES                                                  
         FROM #tasks AS T-- with (nolock,nowait, forceseek)                                                 
              INNER JOIN FOX_TBL_TASK_TYPE AS TT with (nolock, nowait, forceseek) ON TT.TASK_TYPE_ID = T.TASK_TYPE_ID                                
                                                                                                           
                                                          AND TT.PRACTICE_CODE = @PRACTICECODE          
                AND ISNULL(TT.DELETED, 0) = 0       
              INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS TTST with (nolock, nowait) ON TTST.TASK_ID = T.TASK_ID                                                  
             AND YEAR(TTST.Created_Date)>=@YEAR AND TTST.PRACTICE_CODE = @PRACTICECODE AND ISNULL(TTST.DELETED, 0) = 0                                                  
                                                          AND TTST.PRACTICE_CODE = @PRACTICECODE                                                  
              INNER JOIN FOX_TBL_TASK_SUB_TYPE AS TST with (nolock, nowait) ON TST.TASK_SUB_TYPE_ID = TTST.TASK_SUB_TYPE_ID                                                  
                                                          AND TST.PRACTICE_CODE = @PRACTICECODE      
                AND TST.TASK_TYPE_ID = TT.TASK_TYPE_ID                                                  
                                                          AND ISNULL(TST.DELETED, 0) = 0                                       
   WHERE T.PRACTICE_CODE = @PRACTICECODE                                                   
               AND ISNULL(T.DELETED, 0) = 0                                                  
   GROUP BY T.TASK_ID;                                    
         --                            
         SET @CURRENTPAGE = @CURRENTPAGE - 1;                            
                               
IF OBJECT_ID('TEMPDB..#TASK_DATA') IS NOT NULL DROP TABLE #TASK_DATA                            
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                            
   SELECT T.TASK_ID,                                                   
                    CS.CASE_ID,                                                   
                    T.PATIENT_ACCOUNT,                                                   
                    T.SEND_TO_ID,                                           
                    TT.CATEGORY_CODE,                              
        isnull(T.IS_SENDTO_MARK_COMPLETE,0) as IS_SENDTO_MARK_COMPLETE,                            
                    ISNULL(CS.CASE_NO, '') AS CASE_NO,                                                   
                    ISNULL(CS.RT_CASE_NO, '') AS RT_CASE_NO,                                                  
                    CASE                                                  
                        WHEN ISNULL(AU.LAST_NAME, '') = ''                                                  
                        THEN ISNULL(AU.FIRST_NAME, '')                                                  
                        ELSE ISNULL(AU.LAST_NAME, '')+', '+ISNULL(AU.FIRST_NAME, '')                                                  
                    END AS CREATED_BY_FULL_NAME,     
     AU.FIRST_NAME AS CREATED_FIRST_NAME,    
     AU.LAST_NAME AS CREATED_LAST_NAME,                                                 
                    CASE                                                  
                        WHEN ISNULL(AU_MODIFIED.LAST_NAME, '') = ''                                                  
                        THEN ISNULL(AU_MODIFIED.FIRST_NAME, '')                                                  
                        ELSE ISNULL(AU_MODIFIED.LAST_NAME, '')+', '+ISNULL(AU_MODIFIED.FIRST_NAME, '')                                                  
                    END AS MODIFIED_BY_FULL_NAME,                                                  
                                        
                   CASE                                                 
                        WHEN T.IS_SEND_TO_USER = 1                                                
                        THEN                                                 
                           CASE                                                  
                       WHEN ISNULL(send_to_user.LAST_NAME, '') = ''                                                  
                           THEN ISNULL(send_to_user.FIRST_NAME, '')                
                           ELSE ISNULL(send_to_user.LAST_NAME, '')+', '+ISNULL(send_to_user.FIRST_NAME, '')                                                  
                           END                                                  
                                                
                   ELSE  send_to_group.GROUP_NAME                                                
                   END AS SENT_TO,                                               
                           
                                            
                                                                    
                                                     
                  CASE                                                  
   WHEN ISNULL(FP.LAST_NAME, '') = ''                                                  
                        THEN ISNULL(FP.FIRST_NAME, '')                                            
                        ELSE ISNULL(FP.LAST_NAME, '')+', '+ISNULL(FP.FIRST_NAME, '')                                                  
                    END AS PROVIDER_FULL_NAME,    
     FP.FIRST_NAME AS PROVIDER_FIRST_NAME,    
     FP.LAST_NAME AS PROVIDER_LAST_NAME,                                                     
                    TT.NAME AS TASK_TYPE_NAME,                                                   
                    TT.DESCRIPTION AS TASK_TYPE_DESCRIPTION,                                                  
                    CASE                                                  
   WHEN ISNULL(AL.CODE, '') <> ''                                             
                             AND ISNULL(AL.NAME, '') <> ''                                                  
                 THEN ISNULL(AL.CODE, '')+' - '+ISNULL(AL.NAME, '')                                                  
                        ELSE ISNULL(AL.NAME, '')                                                  
                    END AS LOCATION_NAME,    
     AL.NAME AS ACTIVE_LOCATION_NAME,    
     AL.CODE AS ACTIVE_LOCATION_CODE,                                                   
                    ISNULL(T.PRIORITY, '') AS PRIORITY,                                                   
                    T.DUE_DATE_TIME,                                                                                  
                    CASE                                                  
                        WHEN AL.NAME = 'Private Home'                                                  
                        THEN ISNULL(PA.POS_REGION, '')                                                  
                        ELSE ISNULL(AL.REGION, '')                                                  
                    END AS REGION,    
     AL.REGION AS ACTIVE_LOCATION_REGION,    
     PA.POS_REGION AS PATIENT_ADDRESS_REGION,                                                   
                    ISNULL(AL.STATE, '') AS [STATE],                                                   
                    T.CREATED_DATE,                                                   
                    T.MODIFIED_DATE,                                           
                    T.CREATED_BY,                                                  
                    T.MODIFIED_BY,                                                
                    CASE    
                        WHEN ISNULL(P.LAST_NAME, '') = ''    
                        THEN ISNULL(P.FIRST_NAME, '')    
                        ELSE ISNULL(P.LAST_NAME, '')+', '+ISNULL(P.FIRST_NAME, '')    
                    END AS PATIENT_FULL_NAME,    
     P.FIRST_NAME AS PATIENT_FIRST_NAME,    
     P.LAST_NAME AS PATIENT_LAST_NAME,                                                   
                    P.DATE_OF_BIRTH AS DATE_OF_BIRTH,                                                   
                    P.GENDER AS GENDER,                                                   
                    ISNULL(tst.TASK_SUBTYPES, '') AS TASK_SUBTYPES,                                            
     (select count(TASK_LOG_ID) from FOX_TBL_TASK_LOG log with (nolock, nowait, forceseek) WHERE log.TASK_ID =  T.TASK_ID and ISNULL(DELETED,0) = 0 AND log.PRACTICE_CODE = @PRACTICECODE) AS NO_OF_TIMES_MODIFIED,                                           
  
   
     ISNULL(P.CHART_ID  ,'') AS MRN    
  INTO #TASK_DATA    
             FROM #tasks T -- with (nolock, nowait, forceseek)    
                      LEFT JOIN FOX_TBL_CASE CS with (nolock, nowait) ON T.CASE_ID = CS.CASE_ID  AND YEAR(CS.Created_Date)>=@YEAR                                                
      LEFT JOIN FOX_TBL_PROVIDER FP with (nolock, nowait) ON T.PROVIDER_ID = FP.FOX_PROVIDER_ID                                                  
                  LEFT JOIN FOX_TBL_APPLICATION_USER AU with (nolock, nowait) ON T.CREATED_BY = AU.USER_NAME                    
      AND ISNULL(AU.DELETED, 0) = 0                    
                  LEFT JOIN FOX_TBL_TASK_TYPE TT with (nolock, nowait, forceseek) ON T.TASK_TYPE_ID = TT.TASK_TYPE_ID                                  
                                              AND TT.PRACTICE_CODE = @PRACTICECODE                                        
                  LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS AL with (nolock, nowait) ON T.LOC_ID = AL.LOC_ID                                                  
                  LEFT JOIN PATIENT P with (nolock, nowait) ON T.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT     AND P.PRACTICE_CODE = @PRACTICECODE                                                
                  LEFT JOIN #TASK_SUB_TYPES AS tst ON tst.TASK_ID = T.TASK_ID                                             
                  LEFT JOIN FOX_TBL_PATIENT_POS PPOS with (nolock, nowait) ON PPOS.LOC_ID = T.LOC_ID                                                  
                  AND PPOS.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT                                                  
                  AND ISNULL(PPOS.DELETED, 0) = 0             
      AND Is_Default = 1                                                
                  LEFT JOIN FOX_TBL_PATIENT_ADDRESS PA  with (nolock, nowait) ON PA.PATIENT_POS_ID = PPOS.PATIENT_POS_ID                                                  
                  AND ISNULL(PA.DELETED, 0) = 0                               
                  LEFT JOIN FOX_TBL_APPLICATION_USER send_to_user  with (nolock, nowait) ON t.SEND_TO_ID = send_to_user.USER_ID                                                
                  AND ISNULL(send_to_user.DELETED, 0) = 0                
                                                                                            
                  LEFT JOIN FOX_TBL_GROUP send_to_group  with (nolock, nowait) ON t.SEND_TO_ID = send_to_group.GROUP_ID                                                
                  AND ISNULL(send_to_group.DELETED, 0) = 0                    
      AND send_to_group.PRACTICE_CODE = @PRACTICE_CODE                    
                      
                  LEFT JOIN FOX_TBL_APPLICATION_USER AU_MODIFIED  with (nolock, nowait) ON T.MODIFIED_BY = AU_MODIFIED.USER_NAME                                                                 
                  LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS   with (nolock, nowait) ON INS.Patient_Account = T.Patient_Account                             
                AND ISNULL(INS.DELETED, 0) = 0                              
                AND INS.Pri_Sec_Oth_Type='P'                             
                AND INS.FOX_INSURANCE_STATUS = 'C'                                                
               LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS  with (nolock, nowait) ON CS.CERTIFYING_REF_SOURCE_ID = ORS.SOURCE_ID                             
                  AND ISNULL(ORS.DELETED,0) = 0                             
                  AND ORS.PRACTICE_CODE = @PRACTICECODE                                                  
             WHERE TT.CATEGORY_CODE IS NOT NULL                                                  
                   AND (@TASK_SUB_TYPE_ID IS NULL OR (SELECT COUNT(TASK_TASK_SUB_TYPE_ID)                             
       FROM FOX_TBL_task_TASK_SUB_TYPE  with (nolock,nowait)                            
       WHERE   send_to_user.PRACTICE_CODE = @PRACTICE_CODE  AND   YEAR(Created_Date)>=@YEAR   AND ISNULL(DELETED,0) = 0       
                 AND AU.PRACTICE_CODE = @PRACTICE_CODE                    
                 AND TASK_ID = T.TASK_ID                             
                 AND PRACTICE_CODE =  @PRACTICECODE                             
                 AND TASK_SUB_TYPE_ID = @TASK_SUB_TYPE_ID) > 0)                                                  
                   AND (@CERTIFYING_REF_SOURCE_ID IS NULL OR CS.CERTIFYING_REF_SOURCE_ID = @CERTIFYING_REF_SOURCE_ID)       
     AND (@PATIENT_ZIP_CODE IS NULL OR @PATIENT_ZIP_CODE = '' OR PA.ZIP = @PATIENT_ZIP_CODE)                                                
                   AND (@INSURANCE_ID IS NULL OR (INS.FOX_TBL_INSURANCE_ID = @INSURANCE_ID AND INS.Pri_Sec_Oth_Type='P' AND INS.FOX_INSURANCE_STATUS = 'C' ))                                                
       AND ( @CERTIFYING_REF_SOURCE_FAX = '' OR( ORS.FAX = @CERTIFYING_REF_SOURCE_FAX ) )                                            
                   AND ( @REGION = '' OR( PA.POS_REGION LIKE @REGION OR AL.REGION LIKE @REGION))                                                
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                            
 DECLARE @START_FROM INT= @CURRENTPAGE * @RECORDPER_PAGE;                                                
         DECLARE @TOATL_PAGESUDM FLOAT;    
   IF OBJECT_ID('TEMPDB..#TASK_DATA_FILTERED') IS NOT NULL DROP TABLE #TASK_DATA_FILTERED    
   SELECT * INTO #TASK_DATA_FILTERED FROM #TASK_DATA     
   WHERE (CATEGORY_CODE LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR PATIENT_ACCOUNT LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR PATIENT_FIRST_NAME LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR PATIENT_LAST_NAME LIKE '%'+@SEARCHTEXT+'%'                                          
                        OR MRN LIKE '%'+@SEARCHTEXT+'%'                                                     
                        OR CASE_NO LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR RT_CASE_NO LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR CREATED_FIRST_NAME LIKE '%'+@SEARCHTEXT+'%'                                         
                        OR CREATED_LAST_NAME LIKE '%'+@SEARCHTEXT+'%'                                                  
      OR PROVIDER_FIRST_NAME LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR PROVIDER_LAST_NAME LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR TASK_TYPE_NAME LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR PRIORITY LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR PATIENT_ADDRESS_REGION LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR ACTIVE_LOCATION_REGION LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR ACTIVE_LOCATION_NAME LIKE '%'+@SEARCHTEXT+'%'                                
                        OR [STATE] LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR ACTIVE_LOCATION_CODE LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR TASK_SUBTYPES LIKE '%'+@SEARCHTEXT+'%'                                                  
                        OR CONVERT(VARCHAR, CREATED_DATE, 101) LIKE+'%'+@SEARCHTEXT+'%'                                                  
                        OR CONVERT(VARCHAR, MODIFIED_DATE, 100) LIKE+'%'+@SEARCHTEXT+'%');                                                  
         SELECT @TOATL_PAGESUDM = COUNT(*)                                                  
         FROM #TASK_DATA_FILTERED;                            
         IF(@RECORDPER_PAGE = 0)                                                  
             BEGIN                                                  
                 SET @RECORDPER_PAGE = @TOATL_PAGESUDM;                                                  
            END;                                                  
             ELSE                                                  
             BEGIN                                                  
                 SET @RECORDPER_PAGE = @RECORDPER_PAGE;                                                  
             END;                      
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                                                  
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORDPER_PAGE);                                                  
         SELECT *,                                                   
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                                 
                @TOTAL_RECORDS AS TOTAL_RECORDS                                                  
         FROM #TASK_DATA_FILTERED AS FOX_TBL_TASK_DETAIL                               
         ORDER BY CASE                                                  
                      WHEN @SORTBY = 'CREATED_DATE'                                                  
                           AND @SORTORDER = 'ASC'                                                  
                      THEN CREATED_DATE                                                  
                  END ASC,                                                  
                  CASE                                                  
                      WHEN @SORTBY = 'CREATED_DATE'                                                  
                           AND @SORTORDER = 'DESC'                                                  
                      THEN CREATED_DATE                                                  
                  END DESC,                                                  
                  CASE           
                      WHEN @SORTBY = 'MODIFIED_DATE'                                                  
                   AND @SORTORDER = 'ASC'                                                  
                      THEN MODIFIED_DATE                                                  
                  END ASC,                                                  
                  CASE                                                  
                      WHEN @SORTBY = 'MODIFIED_DATE'                                                  
                           AND @SORTORDER = 'DESC'                             
                      THEN MODIFIED_DATE                                                  
                  END DESC                                                  
         OFFSET @START_FROM ROWS FETCH NEXT @RECORDPER_PAGE ROWS ONLY;                                                  
     END; 