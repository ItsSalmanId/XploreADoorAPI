  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_TASK_DETAIL_LIST]                         
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
--DECLARE                   
--@PRACTICE_CODE BIGINT  = 1012714                                                
--,@PATIENT_ACCOUNT BIGINT   = null                                                
--,@CASE_ID BIGINT  = NULL                                                
--,@OPTION  VARCHAR(20)  = 'OPEN'                                                
--,@USER_ID BIGINT  = 605101                                                
--,@CURRENT_PAGE INT  = 1                                                
--,@RECORD_PER_PAGE INT   = 500                                                
--,@SEARCH_TEXT VARCHAR(30)   = ''                                                
--,@SORT_BY VARCHAR(50)  = 'CREATED_DATE'                                                
--,@SORT_ORDER VARCHAR(5)  = 'DESC'                   
--,@INSURANCE_ID         BIGINT=NULL                                        
--,@TASK_TYPE_ID        BIGINT=NULL                                         
--,@TASK_SUB_TYPE_ID         BIGINT= NULL                                        
--,@PROVIDER_ID         BIGINT=  NULL                    
--,@REGION          VARCHAR(50)=''                                         
--,@LOC_ID         BIGINT=   NULL                                     
--,@CERTIFYING_REF_SOURCE_ID        BIGINT=     NULL                              
--,@CERTIFYING_REF_SOURCE_FAX       VARCHAR(50)=   ''                                               
--,@PATIENT_ZIP_CODE       VARCHAR(50)=  ''                                       
--,@DUE_DATE_TIME DATETIME  =   NULL                                  
--,@DATE_FROM DATETIME=  '2000-10-15 03:22:44.470'                  
--,@DATE_TO DATETIME=  '2020-10-23 03:22:44.470'                  
--,@OWNER_ID        BIGINT= 60558217288--02MTB                                   
--,@MODIFIED_BY VARCHAR(70)=''                  
--,@IS_USER_LEVEL bit = 1                 
BEGIN                                              
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                                                    
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
SELECT @USER_NAME = USER_NAME FROM FOX_TBL_APPLICATION_USER  with (nolock) WHERE USER_ID = @USERID;                                                                  
SELECT @OWNER_NAME = USER_NAME FROM FOX_TBL_APPLICATION_USER  with (nolock) WHERE USER_ID = @OWNER_ID;                                                                    
IF (@MODIFIED_BY = '')                                      
BEGIN                                      
 SET @MODIFIED_BY = NULL                                      
END               
          
if object_id('tempdb..#a') is not null drop table #a          
SELECT TST_I.[NAME],t_I.TASK_ID into #a          
FROM FOX_TBL_TASK AS T_I   with (nolock, nowait)          
                  INNER JOIN FOX_TBL_TASK_TYPE AS TT_I  with (nolock) ON TT_I.TASK_TYPE_ID = T_I.TASK_TYPE_ID              
                                                          AND TT_I.PRACTICE_CODE = @PRACTICECODE AND ISNULL(TT_I.DELETED, 0) = 0              
                                                                      
                  INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS TTST_I with (nolock) ON TTST_I.TASK_ID = T_I.TASK_ID           
                                                          AND TTST_I.PRACTICE_CODE = @PRACTICECODE AND ISNULL(TTST_I.DELETED, 0) = 0            
                                                                      
                  INNER JOIN FOX_TBL_TASK_SUB_TYPE AS TST_I with (nolock) ON TST_I.TASK_SUB_TYPE_ID = TTST_I.TASK_SUB_TYPE_ID             
                                                          AND TST_I.PRACTICE_CODE = @PRACTICECODE AND TST_I.TASK_TYPE_ID = TT_I.TASK_TYPE_ID             
                                                          AND ISNULL(TST_I.DELETED, 0) = 0                 
                                                                        
             WHERE  YEAR(T_I.Created_Date)>=2022 AND T_I.PRACTICE_CODE = @PRACTICECODE AND  ISNULL(T_I.DELETED, 0) = 0             
                     
                                    
IF OBJECT_ID('TEMPDB..#TASK_SUB_TYPES') IS NOT NULL DROP TABLE #TASK_SUB_TYPES                        
SELECT               
T.TASK_ID,                                              
TASK_SUBTYPES = STUFF(                                              
(                        
SELECT DISTINCT ', '+T_I.[NAME] FROM  #a as T_I           
                   where T_I.TASK_ID = T.TASK_ID FOR XML PATH, TYPE                                              
         ).value(N'.[1]', N'nvarchar(max)'), 1, 1, '')                                
         INTO #TASK_SUB_TYPES                                              
         FROM FOX_TBL_TASK AS T with (nolock,nowait)                                             
              INNER JOIN FOX_TBL_TASK_TYPE AS TT with (nolock) ON TT.TASK_TYPE_ID = T.TASK_TYPE_ID                            
                                                                                                       
                                                          AND TT.PRACTICE_CODE = @PRACTICECODE      
                AND ISNULL(TT.DELETED, 0) = 0   
              INNER JOIN FOX_TBL_TASK_TASK_SUB_TYPE AS TTST with (nolock) ON TTST.TASK_ID = T.TASK_ID                                              
             AND YEAR(TTST.Created_Date)>=2022 AND TTST.PRACTICE_CODE = @PRACTICECODE AND ISNULL(TTST.DELETED, 0) = 0                                              
                                                          AND TTST.PRACTICE_CODE = @PRACTICECODE                                              
              INNER JOIN FOX_TBL_TASK_SUB_TYPE AS TST with (nolock) ON TST.TASK_SUB_TYPE_ID = TTST.TASK_SUB_TYPE_ID                                              
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
                    TT.NAME AS TASK_TYPE_NAME,                                               
                    TT.DESCRIPTION AS TASK_TYPE_DESCRIPTION,                                              
                    CASE                                              
                        WHEN ISNULL(AL.CODE, '') <> ''                                         
                             AND ISNULL(AL.NAME, '') <> ''                                              
                        THEN ISNULL(AL.CODE, '')+' - '+ISNULL(AL.NAME, '')                                              
                        ELSE ISNULL(AL.NAME, '')                                              
                    END AS LOCATION_NAME,                                               
                    ISNULL(T.PRIORITY, '') AS PRIORITY,                                               
                    T.DUE_DATE_TIME,                                                                              
                    CASE                                              
                        WHEN AL.NAME = 'Private Home'                                              
                        THEN ISNULL(PA.POS_REGION, '')                                              
                        ELSE ISNULL(AL.REGION, '')                                              
                    END AS REGION,                                               
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
                    P.DATE_OF_BIRTH AS DATE_OF_BIRTH,                                               
                    P.GENDER AS GENDER,                                               
                    ISNULL(tst.TASK_SUBTYPES, '') AS TASK_SUBTYPES,                                        
     (select count(TASK_LOG_ID) from FOX_TBL_TASK_LOG log with (nolock) WHERE log.TASK_ID =  T.TASK_ID and ISNULL(DELETED,0) = 0 AND log.PRACTICE_CODE = @PRACTICECODE) AS NO_OF_TIMES_MODIFIED,                                          
     ISNULL(P.CHART_ID  ,'') AS MRN                        
  INTO #TASK_DATA                        
             FROM FOX_TBL_TASK T  with (nolock, nowait)                                            
                  LEFT JOIN FOX_TBL_CASE CS with (nolock) ON T.CASE_ID = CS.CASE_ID  AND YEAR(CS.Created_Date)>=2022                                            
      LEFT JOIN FOX_TBL_PROVIDER FP with (nolock) ON T.PROVIDER_ID = FP.FOX_PROVIDER_ID                                              
                  LEFT JOIN FOX_TBL_APPLICATION_USER AU with (nolock) ON T.CREATED_BY = AU.USER_NAME                
      AND ISNULL(AU.DELETED, 0) = 0                
                  LEFT JOIN FOX_TBL_TASK_TYPE TT with (nolock) ON T.TASK_TYPE_ID = TT.TASK_TYPE_ID                              
                                              AND TT.PRACTICE_CODE = @PRACTICECODE                                         
                  LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS AL with (nolock) ON T.LOC_ID = AL.LOC_ID                                              
                  LEFT JOIN PATIENT P with (nolock) ON T.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT     AND P.PRACTICE_CODE = @PRACTICECODE                                            
                  LEFT JOIN #TASK_SUB_TYPES AS tst ON tst.TASK_ID = T.TASK_ID                                         
                  LEFT JOIN FOX_TBL_PATIENT_POS PPOS with (nolock) ON PPOS.LOC_ID = T.LOC_ID                                              
                  AND PPOS.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT                                              
                  AND ISNULL(PPOS.DELETED, 0) = 0         
      AND Is_Default = 1                                            
                  LEFT JOIN FOX_TBL_PATIENT_ADDRESS PA  with (nolock) ON PA.PATIENT_POS_ID = PPOS.PATIENT_POS_ID                                              
                  AND ISNULL(PA.DELETED, 0) = 0                           
                  LEFT JOIN FOX_TBL_APPLICATION_USER send_to_user  with (nolock) ON t.SEND_TO_ID = send_to_user.USER_ID                                            
                  AND ISNULL(send_to_user.DELETED, 0) = 0            
                                                                                        
                  LEFT JOIN FOX_TBL_GROUP send_to_group  with (nolock) ON t.SEND_TO_ID = send_to_group.GROUP_ID                                            
                  AND ISNULL(send_to_group.DELETED, 0) = 0                
      AND send_to_group.PRACTICE_CODE = @PRACTICE_CODE                
                  
                  LEFT JOIN FOX_TBL_APPLICATION_USER AU_MODIFIED  with (nolock) ON T.MODIFIED_BY = AU_MODIFIED.USER_NAME                                                             
                  LEFT JOIN FOX_TBL_PATIENT_INSURANCE INS   with (nolock) ON INS.Patient_Account = T.Patient_Account                         
                AND ISNULL(INS.DELETED, 0) = 0                          
                AND INS.Pri_Sec_Oth_Type='P'                         
                AND INS.FOX_INSURANCE_STATUS = 'C'                                            
               LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS  with (nolock) ON CS.CERTIFYING_REF_SOURCE_ID = ORS.SOURCE_ID                         
                  AND ISNULL(ORS.DELETED,0) = 0                         
                  AND ORS.PRACTICE_CODE = @PRACTICECODE                                              
             WHERE T.PRACTICE_CODE = @PRACTICECODE AND YEAR(T.Created_Date)>=2022 AND ISNULL(T.DELETED, 0) = 0                                              
                   AND ISNULL(T.IS_TEMPLATE, 0) = 0                                                                             
                   AND TT.CATEGORY_CODE IS NOT NULL                                              
                   AND (@PATIENTACCOUNT IS NULL OR T.PATIENT_ACCOUNT = @PATIENTACCOUNT)                  
                   AND (@CASEID IS NULL OR T.CASE_ID = @CASEID)                        
                   AND (@TASK_TYPE_ID IS NULL OR T.TASK_TYPE_ID = @TASK_TYPE_ID)                                              
                   AND (@TASK_SUB_TYPE_ID IS NULL OR (SELECT COUNT(TASK_TASK_SUB_TYPE_ID)                         
       FROM FOX_TBL_task_TASK_SUB_TYPE  with (nolock,nowait)                        
       WHERE   send_to_user.PRACTICE_CODE = @PRACTICE_CODE  AND   YEAR(Created_Date)>=2022   AND ISNULL(DELETED,0) = 0                 
                      
                 AND AU.PRACTICE_CODE = @PRACTICE_CODE                
                 AND TASK_ID = T.TASK_ID                         
                 AND PRACTICE_CODE =  @PRACTICECODE                         
                 AND TASK_SUB_TYPE_ID = @TASK_SUB_TYPE_ID) > 0)                                              
                   AND (@PROVIDER_ID IS NULL OR T.PROVIDER_ID = @PROVIDER_ID)                                  
                   AND (@LOC_ID IS NULL OR T.LOC_ID = @LOC_ID)                                            
                   AND (@CERTIFYING_REF_SOURCE_ID IS NULL OR CS.CERTIFYING_REF_SOURCE_ID = @CERTIFYING_REF_SOURCE_ID)                                            
                   AND ((@OWNER_ID IS NULL OR T.SEND_TO_ID = @OWNER_ID  or T.SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_USER_GROUP  with (nolock) WHERE USER_NAME = @OWNER_NAME AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0)= 0)) or ((@OPTIION = 
 
 'OPEN SENT' or    
              
 @OPTIION = 'ALL') and T.CREATED_BY                
 = @USER_NAME)) --OR  T.FINAL_ROUTE_ID = @OWNER_ID                  
                     
                     
                   AND (@PATIENT_ZIP_CODE IS NULL OR @PATIENT_ZIP_CODE = '' OR PA.ZIP = @PATIENT_ZIP_CODE)                                            
                   AND (@INSURANCE_ID IS NULL OR (INS.FOX_TBL_INSURANCE_ID = @INSURANCE_ID AND INS.Pri_Sec_Oth_Type='P' AND INS.FOX_INSURANCE_STATUS = 'C' ))                                            
                   AND ( @CERTIFYING_REF_SOURCE_FAX = '' OR( ORS.FAX = @CERTIFYING_REF_SOURCE_FAX ) )                                        
                   AND (@MODIFIED_BY IS NULL OR T.MODIFIED_BY = @MODIFIED_BY )                                          
                   AND ( @REGION = '' OR( PA.POS_REGION LIKE @REGION OR AL.REGION LIKE @REGION))                                            
                   AND (@DUE_DATE_TIME IS NULL OR CONVERT(DATE, T.DUE_DATE_TIME,101) =CONVERT(DATE, @DUE_DATE_TIME,101) )                                             
                   AND ( CONVERT(DATE, T.CREATED_DATE,101) BETWEEN CONVERT(DATE, @DATE_FROM,101) AND CONVERT(DATE, @DATE_TO,101))         
                   AND (                         
           (@OPTIION = 'ALL'                         
        AND ((@IS_USER_LEVEL = 1 AND T.IS_SENDTO_MARK_COMPLETE <> 1) or @IS_USER_LEVEL = 0)                        
        ) -- OR (T.IS_SENDTO_MARK_COMPLETE  <> 1 AND T.IS_FINALROUTE_MARK_COMPLETE  <> 1))                                           
                    OR (                        
         @OPTIION = 'OPEN'                         
        AND (                        
             (T.IS_SEND_TO_USER = 1 AND T.SEND_TO_ID = @USERID AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)                                     
                OR (T.IS_SEND_TO_USER = 0 AND T.SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_USER_GROUP WHERE USER_NAME = @USER_NAME AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0)= 0) AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)           
    OR (T.IS_SEND_TO_USER = 0 AND T.SEND_TO_ID IN (SELECT GROUP_ID FROM FOX_TBL_GROUP  with (nolock) where ISNULL(DELETED,0)= 0 AND PRACTICE_CODE = @PRACTICE_CODE) AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0)                                              
                      
       )                        
        )                                              
                               
        --)                                              
                         OR (@OPTIION = 'OPEN SENT'  AND (T.CREATED_BY = @USER_NAME AND ISNULL(IS_SENDTO_MARK_COMPLETE, 0) = 0))                                              
                         --OR (@OPTIION = 'CLOSED SENT' AND (T.CREATED_BY = @USER_NAME AND ISNULL(IS_FINALROUTE_MARK_COMPLETE, 0) = 1))                        
      )                                              
                   AND (TT.CATEGORY_CODE LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR T.PATIENT_ACCOUNT LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR P.FIRST_NAME LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR P.LAST_NAME LIKE '%'+@SEARCHTEXT+'%'                                      
                        OR P.Chart_id LIKE '%'+@SEARCHTEXT+'%'                                                 
                        OR CS.CASE_NO LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR CS.RT_CASE_NO LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR AU.FIRST_NAME LIKE '%'+@SEARCHTEXT+'%'                                     
                        OR AU.LAST_NAME LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR FP.FIRST_NAME LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR FP.LAST_NAME LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR TT.NAME LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR T.PRIORITY LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR ISNULL(PA.POS_REGION, '') LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR ISNULL(AL.REGION, '') LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR AL.NAME LIKE '%'+@SEARCHTEXT+'%'                            
                        OR AL.STATE LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR AL.CODE LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR TASK_SUBTYPES LIKE '%'+@SEARCHTEXT+'%'                                              
                        OR CONVERT(VARCHAR, T.CREATED_DATE, 101) LIKE+'%'+@SEARCHTEXT+'%'                                              
                        OR CONVERT(VARCHAR, T.MODIFIED_DATE, 100) LIKE+'%'+@SEARCHTEXT+'%');             
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                        
         DECLARE @START_FROM INT= @CURRENTPAGE * @RECORDPER_PAGE;                                            
         DECLARE @TOATL_PAGESUDM FLOAT;                                              
         SELECT @TOATL_PAGESUDM = COUNT(*)                                              
         FROM #TASK_DATA;                        
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
         FROM #TASK_DATA AS FOX_TBL_TASK_DETAIL                                             
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