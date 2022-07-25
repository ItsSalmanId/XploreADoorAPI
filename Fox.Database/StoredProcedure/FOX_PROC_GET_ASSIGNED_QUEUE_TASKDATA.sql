IF (OBJECT_ID('FOX_PROC_GET_ASSIGNED_QUEUE_TASKDATA') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ASSIGNED_QUEUE_TASKDATA 
GO   
 --4  
 --1           
---------------------------------------------------- -----------------------------------------------------------------                          
-- =============================================                          
-- AUTHOR:  <AUTHOR,MEHMOOD UL HASSAN>                          
-- CREATE DATE: <CREATE DATE,12/10/2017>                          
-- DESCRIPTION: <GET INDEXED QUEUE>                
-- exec [FOX_PROC_GET_ASSIGNED_QUEUE_TASKDATA_RAFAY]  1011163, 1, 50, '', '', '','Ahmed_544530','2018-07-19 20:0:28.270','2019-07-19 20:0:28.270',0                  
CREATE PROCEDURE [DBO].[FOX_PROC_GET_ASSIGNED_QUEUE_TASKDATA]-- 1011163, 1, 50, '', '', '','Ahmed_544530','2018-07-19 20:0:28.270',''                          
 (            
 --DECLARE                       
 @PRACTICE_CODE BIGINT            
 ,@CURRENT_PAGE INT            
 ,@RECORD_PER_PAGE INT            
 ,@SEARCH_TEXT VARCHAR(30)            
 ,@SORT_BY VARCHAR(50)            
 ,@SORT_ORDER VARCHAR(20)            
 ,@USER_NAME VARCHAR(50)            
 ,@DATE_FROM DATETIME            
 ,@DATE_TO DATETIME            
  ,@Is_Trash BIT            
 --DECLARE                  
 --@PRACTICE_CODE BIGINT=1011163,                          
 --@CURRENT_PAGE INT=1,                          
 --@RECORD_PER_PAGE INT=100,                          
 --@SEARCH_TEXT VARCHAR(30)='',                          
 --@SORCE_TYPE VARCHAR(50)='',                          
 --@SORCE_NAME VARCHAR(50)='',                          
 --@WORK_ID VARCHAR(100)='',                          
 --@SORT_BY VARCHAR(50)='',                          
 --@SORT_ORDER VARCHAR(20)='',                          
 --@ASSIGNED_TO VARCHAR(100)='',                          
 --@STATUS VARCHAR(50)='',                        
 --@USER_NAME VARCHAR(50) = '1163TESTING'                        
 )            
AS            
BEGIN            
IF(@DATE_FROM is null)            
begin            
    SET @DATE_FROM = DATEADD(YEAR,-2,DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) -6, 0))              
end            
IF(@DATE_TO is null)            
begin            
 SET  @DATE_TO = GETDATE()            
end            
 SET NOCOUNT ON;            
            
 DECLARE @USERROLE VARCHAR(50);            
            
 SET @USERROLE = (            
   SELECT R.ROLE_NAME            
   FROM FOX_TBL_APPLICATION_USER UR            
   INNER JOIN FOX_TBL_ROLE R ON R.ROLE_ID = UR.ROLE_ID            
   WHERE UPPER(USER_NAME) = UPPER(@USER_NAME)            
   )            
            
 DECLARE @ARCHIVEDATE DATE = '1800/01/01'            
            
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1            
            
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE            
 DECLARE @TOATL_PAGESUDM FLOAT            
            
 IF OBJECT_ID('TEMPDB..#TEMPRECORDS') IS NOT NULL            
  DROP TABLE #TEMPRECORDS            
            
 SELECT SORCE_NAME            
  ,Q.DELETED            
  ,Q.WORK_ID            
  ,UNIQUE_ID            
  ,Q.PRACTICE_CODE            
  ,Q.PATIENT_ACCOUNT              
  ,SORCE_TYPE            
  ,RECEIVE_DATE            
  ,convert(varchar,RECEIVE_DATE) AS Received_Date_Str            
  ,TOTAL_PAGES            
  ,NO_OF_SPLITS            
  ,FILE_PATH            
  ,ASSIGNED_TO            
  ,ASSIGNED_DATE            
  ,convert(varchar,ASSIGNED_DATE) AS Assigned_Date_Str            
  ,COMPLETED_BY            
  ,COMPLETED_DATE            
  ,Q.CREATED_BY            
  ,Q.CREATED_DATE            
  ,Q.MODIFIED_DATE            
  ,convert(varchar,Q.MODIFIED_DATE) AS Modified_Date_Str            
  ,Q.SUPERVISOR_STATUS            
  ,ASSIGNED_BY            
  ,Q.MODIFIED_BY,            
  Q.WORK_STATUS            
  ,IS_EMERGENCY_ORDER IS_EMERGENCY            
  ,Q.IS_TRASH_REFERRAL            
  ,CONVERT(BIT, CASE             
    WHEN ISNULL(IsSigned, 0) = 1            
     OR Q.CREATED_BY LIKE 'FOX TEAM'            
     OR CHARINDEX('_', Q.UNIQUE_ID) > 0            
     THEN 0            
    ELSE 1            
    END) AS IS_UNSIGNED           --,CONVERT(BIT,            
 --     CASE             
 --   WHEN  ISNULL(Q.IS_OCR, 0) = 1            
 --   THEN 1            
 --   ELSE 0            
 --   END) AS IS_OCR        
  ,Q.OCR_STATUS_ID,        
 OC.OCR_STATUS AS OCR_STATUS        
 INTO #TEMPRECORDS            
 FROM FOX_TBL_WORK_QUEUE Q            
 --LEFT JOIN FOX_TBL_WORK_ORDER_ADDTIONAL_INFO WQAI ON WQAI.WORK_ID = Q.WORK_ID AND WQAI.DELETED = 0            
 LEFT JOIN FOX_TBL_APPLICATION_USER AU ON Q.ASSIGNED_TO = AU.USER_NAME           
  LEFT JOIN FOX_TBL_OCR_STATUS OC ON OC.OCR_STATUS_ID = Q.OCR_STATUS_ID        
    AND ISNULL(OC.DELETED, 0) = 0        
 --WHERE CONVERT(DATE, Q.CREATED_DATE) > CONVERT(DATE, @ARCHIVEDATE)            
 -- AND ASSIGNED_TO IS NOT NULL            
 -- AND (            
 --  @WORK_ID IS NULL            
 --  OR ISNULL(WORK_ID, '') LIKE '%' + @WORK_ID + '%'            
 --  )            
 -- AND ISNULL(Q.SUPERVISOR_STATUS, 0) <> 1            
 -- AND (            
 --  @USERROLE = 'SUPERVISOR'            
 --  OR @USERROLE = 'ADMINISTRATOR'            
 --  OR UPPER(ISNULL(Q.WORK_STATUS, '')) <> 'COMPLETED'            
 --  )            
 -- --AND UPPER(ISNULL(Q.WORK_STATUS, '')) <> 'COMPLETED'            
            
 SELECT @TOATL_PAGESUDM = COUNT(*)            
 FROM #TEMPRECORDS Q            
 LEFT JOIN FOX_TBL_APPLICATION_USER AU ON Q.ASSIGNED_TO = AU.USER_NAME            
 LEFT JOIN FOX_TBL_APPLICATION_USER AB ON Q.ASSIGNED_BY = AB.USER_NAME            
 LEFT JOIN FOX_TBL_APPLICATION_USER MB ON Q.MODIFIED_BY = MB.USER_NAME            
 LEFT JOIN Patient p ON Q.PATIENT_ACCOUNT = p.Patient_Account            
  WHERE assigned_to = @USER_NAME and convert(datetime,RECEIVE_DATE) between convert(datetime,@DATE_FROM) and convert(datetime,@DATE_TO) and Q.PRACTICE_CODE = @PRACTICE_CODE and upper(WORK_STATUS) !='COMPLETED' AND Q.ASSIGNED_TO is not null            
  AND ISNULL(Q.IS_TRASH_REFERRAL,cast (0 as bit)) = @Is_Trash               
  AND (            
   SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR SORCE_TYPE LIKE '%' + @SEARCH_TEXT + '%'            
   OR WORK_ID LIKE '%' + @SEARCH_TEXT + '%'            
   OR AU.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR AU.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR AB.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR AB.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR MB.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR MB.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
   OR UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, COMPLETED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, Q.CREATED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, Q.CREATED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, Q.MODIFIED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
   OR CONVERT(VARCHAR, Q.MODIFIED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
   OR ASSIGNED_BY LIKE '%' + @SEARCH_TEXT + '%'            
   OR Q.MODIFIED_BY LIKE '%' + @SEARCH_TEXT + '%'            
   OR Q.WORK_STATUS LIKE '%' + @SEARCH_TEXT + '%'            
   OR COMPLETED_BY LIKE '%' + @SEARCH_TEXT + '%'            
   OR TOTAL_PAGES LIKE '%' + @SEARCH_TEXT + '%'            
   OR NO_OF_SPLITS LIKE '%' + @SEARCH_TEXT + '%'            
   OR p.First_Name LIKE '%' + @SEARCH_TEXT + '%'            
   OR p.Last_Name LIKE '%' + @SEARCH_TEXT + '%'            
   OR p.Chart_Id LIKE '%' + @SEARCH_TEXT + '%'            
   )            
  AND ISNULL(Q.DELETED, 0) = 0            
            
 IF (@RECORD_PER_PAGE = 0)            
 BEGIN            
  SET @RECORD_PER_PAGE = @TOATL_PAGESUDM            
 END            
 ELSE            
 BEGIN            
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE            
 END            
            
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM            
            
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)            
            
 --IF OBJECT_ID('TEMPDB..#TEMP') IS NOT NULL DROP TABLE #TEMP                          
 -- INTO #TEMP                  
 SELECT *            
  ,@TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES            
  ,@TOTAL_RECORDS TOTAL_RECORDS            
 FROM (            
  SELECT WORK_ID            
   ,UNIQUE_ID            
   ,p.First_Name AS PATIENT_FIRST_NAME            
   ,p.Last_Name AS PATIENT_LAST_NAME            
   ,p.Chart_Id AS MRN             
   ,Q.PRACTICE_CODE            
   ,SORCE_TYPE            
   ,SORCE_NAME            
   ,WORK_STATUS            
   ,RECEIVE_DATE            
   ,convert(varchar,RECEIVE_DATE) AS Received_Date_Str            
   ,TOTAL_PAGES            
   ,NO_OF_SPLITS            
   ,FILE_PATH            
   ,AU.LAST_NAME + ', ' + AU.FIRST_NAME AS ASSIGNED_TO            
   ,ASSIGNED_TO ASSIGNED_TO_ID            
   ,AB.LAST_NAME + ', ' + AB.FIRST_NAME AS ASSIGNED_BY            
   ,ASSIGNED_DATE            
   ,convert(varchar,Q.ASSIGNED_DATE) AS Assigned_Date_Str            
   ,COMPLETED_BY            
   ,COMPLETED_DATE            
   ,Q.CREATED_BY            
   ,Q.CREATED_DATE            
   ,MB.LAST_NAME + ', ' + MB.FIRST_NAME AS MODIFIED_BY            
   ,Q.MODIFIED_DATE            
   ,convert(varchar,Q.MODIFIED_DATE) AS Modified_Date_Str            
   ,Q.DELETED            
   --,(CONVERT(VARCHAR,DATEDIFF(HOUR, ASSIGNED_DATE, GETDATE()))            
   ,(CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, GETDATE()) / 60) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, GETDATE()) % 60)) AS TIME_ELASPE_IN_QUEUE            
   ,ROW_NUMBER() OVER (            
    ORDER BY Q.ASSIGNED_DATE DESC            
    ) AS ACTIVEROW            
   ,IS_EMERGENCY            
   ,CONVERT(BIT, IS_UNSIGNED) AS IS_UNSIGNED            
    ,Q.OCR_STATUS_ID,        
 Q.OCR_STATUS AS OCR_STATUS        
  FROM #TEMPRECORDS Q            
  LEFT JOIN FOX_TBL_APPLICATION_USER AU ON Q.ASSIGNED_TO = AU.USER_NAME            
  LEFT JOIN FOX_TBL_APPLICATION_USER AB ON Q.ASSIGNED_BY = AB.USER_NAME            
  LEFT JOIN FOX_TBL_APPLICATION_USER MB ON Q.MODIFIED_BY = MB.USER_NAME            
  LEFT JOIN Patient p ON Q.PATIENT_ACCOUNT = p.Patient_Account            
    WHERE assigned_to = @USER_NAME and convert(datetime,RECEIVE_DATE) between convert(datetime,@DATE_FROM) and convert(datetime,@DATE_TO) and Q.PRACTICE_CODE = @PRACTICE_CODE and upper(WORK_STATUS) !='COMPLETED' AND Q.ASSIGNED_TO is not null            



      AND ISNULL(Q.IS_TRASH_REFERRAL,cast (0 as bit)) = @Is_Trash               
   AND (            
    SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR SORCE_TYPE LIKE '%' + @SEARCH_TEXT + '%'            
    OR Q.WORK_ID LIKE '%' + @SEARCH_TEXT + '%'            
    OR AU.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR AU.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR AB.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR AB.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR MB.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR MB.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'            
    OR UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'       
    OR CONVERT(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, COMPLETED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, Q.CREATED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, Q.CREATED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, Q.MODIFIED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'            
    OR CONVERT(VARCHAR, Q.MODIFIED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'            
    OR ASSIGNED_BY LIKE '%' + @SEARCH_TEXT + '%'            
    OR Q.MODIFIED_BY LIKE '%' + @SEARCH_TEXT + '%'            
    OR Q.WORK_STATUS LIKE '%' + @SEARCH_TEXT + '%'            
    OR COMPLETED_BY LIKE '%' + @SEARCH_TEXT + '%'            
    OR TOTAL_PAGES LIKE '%' + @SEARCH_TEXT + '%'            
    OR NO_OF_SPLITS LIKE '%' + @SEARCH_TEXT + '%'            
    OR (CONVERT(VARCHAR, DATEDIFF(HOUR, ASSIGNED_DATE, GETDATE()))) LIKE '%' + @SEARCH_TEXT + '%'            
    OR (CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, GETDATE()) % 60)) LIKE '%' + @SEARCH_TEXT + '%'            
    OR p.First_Name LIKE '%' + @SEARCH_TEXT + '%'            
    OR p.Last_Name LIKE '%' + @SEARCH_TEXT + '%'            
    OR p.Chart_Id LIKE '%' + @SEARCH_TEXT + '%'            
    )            
   AND ISNULL(Q.DELETED, 0) = 0            
   AND ISNULL(Q.IS_TRASH_REFERRAL,cast (0 as bit)) = @Is_Trash               
               
  ) AS WORK_QUEUE            
 ORDER BY CASE             
   WHEN @SORT_BY = 'ID'            
    AND @SORT_ORDER = 'ASC'            
    THEN UNIQUE_ID            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'ID'            
    AND @SORT_ORDER = 'DESC'            
    THEN UNIQUE_ID            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'ASSIGNTO'            
    AND @SORT_ORDER = 'ASC'            
    THEN ASSIGNED_TO            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'ASSIGNTO'            
    AND @SORT_ORDER = 'DESC'            
    THEN ASSIGNED_TO            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'SOURCETYPE'            
    AND @SORT_ORDER = 'ASC'            
    THEN SORCE_TYPE           
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'SOURCETYPE'            
    AND @SORT_ORDER = 'DESC'            
    THEN SORCE_TYPE            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'SOURCENAME'            
    AND @SORT_ORDER = 'ASC'            
    THEN SORCE_NAME            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'SOURCENAME'            
    AND @SORT_ORDER = 'DESC'            
    THEN SORCE_NAME            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'DATETIMERECEIVED'            
    AND @SORT_ORDER = 'ASC'            
    THEN RECEIVE_DATE            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'DATETIMERECEIVED'            
    AND @SORT_ORDER = 'DESC'            
    THEN RECEIVE_DATE            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'PAGES'            
    AND @SORT_ORDER = 'ASC'            
    THEN TOTAL_PAGES            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'PAGES'            
    AND @SORT_ORDER = 'DESC'            
    THEN TOTAL_PAGES            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'ELAPSEDQUEUE'            
    AND @SORT_ORDER = 'ASC'            
    THEN (DATEDIFF(MINUTE, ASSIGNED_DATE, GETDATE()))            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'ELAPSEDQUEUE'            
    AND @SORT_ORDER = 'DESC'            
    THEN (DATEDIFF(MINUTE, ASSIGNED_DATE, GETDATE()))            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = ''            
    AND @SORT_ORDER = ''      
    THEN ASSIGNED_DATE            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'ASSIGNEDBY'            
    AND @SORT_ORDER = 'ASC'            
    THEN ASSIGNED_BY            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'ASSIGNEDBY'            
    AND @SORT_ORDER = 'DESC'            
    THEN ASSIGNED_BY            
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'DATETIMEASSIGNED'            
    AND @SORT_ORDER = 'ASC'            
    THEN ASSIGNED_DATE            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'DATETIMEASSIGNED'            
    AND @SORT_ORDER = 'DESC'            
    THEN ASSIGNED_DATE        
   END DESC            
  ,CASE             
   WHEN @SORT_BY = 'UPDATEDBY'            
    AND @SORT_ORDER = 'ASC'            
    THEN MODIFIED_BY            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'UPDATEDBY'            
    AND @SORT_ORDER = 'DESC'            
    THEN MODIFIED_BY            
   END DESC            
  ,CASE               
   WHEN @SORT_BY = 'PATIENT_FIRST_NAME'              
    AND @SORT_ORDER = 'ASC'              
   THEN PATIENT_FIRST_NAME              
   END ASC              
  ,CASE               
   WHEN @SORT_BY = 'PATIENT_FIRST_NAME'              
    AND @SORT_ORDER = 'DESC'              
   THEN PATIENT_FIRST_NAME              
   END DESC            
  ,CASE               
   WHEN @SORT_BY = 'PATIENT_LAST_NAME'        
    AND @SORT_ORDER = 'ASC'              
   THEN PATIENT_LAST_NAME              
   END ASC              
  ,CASE               
   WHEN @SORT_BY = 'PATIENT_LAST_NAME'              
    AND @SORT_ORDER = 'DESC'              
   THEN PATIENT_LAST_NAME              
   END DESC             
  ,CASE               
   WHEN @SORT_BY = 'MRN'              
    AND @SORT_ORDER = 'ASC'              
   THEN MRN              
    END ASC              
  ,CASE               
   WHEN @SORT_BY = 'MRN'              
    AND @SORT_ORDER = 'DESC'              
   THEN MRN              
   END DESC              
  ,CASE             
   WHEN @SORT_BY = 'DATETIMEUPDATED'            
    AND @SORT_ORDER = 'ASC'            
   THEN MODIFIED_DATE            
   END ASC            
  ,CASE             
   WHEN @SORT_BY = 'DATETIMEUPDATED'            
    AND @SORT_ORDER = 'DESC'            
 THEN MODIFIED_DATE            
   END DESC OFFSET @START_FROM ROWS            
            
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY            
  --IF(@USERROLE='INDEXER' OR @USERROLE='AGENT')                        
  -- BEGIN                        
  -- SELECT * FROM #TEMP WHERE UPPER(ASSIGNED_TO_ID)=UPPER(@USER_NAME)                        
  --  END                           
  --  ELSE BEGIN                        
  --  SELECT * FROM #TEMP                         
  --  END                        
END       
