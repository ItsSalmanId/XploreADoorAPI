IF (OBJECT_ID('FOX_PROC_GET_COMPLETED_QUEUE_TASK') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_COMPLETED_QUEUE_TASK 
GO
CREATE procedure [dbo].[FOX_PROC_GET_COMPLETED_QUEUE_TASK] --1011163,'',1,10,'','','','','','','',0,'','','','','','',''            
 (        
 @PRACTICE_CODE BIGINT        
 ,@UNIQUE_ID VARCHAR(50)        
 ,@CURRENT_PAGE INT        
 ,@RECORD_PER_PAGE INT        
 ,@SOURCE_TYPE VARCHAR(50)        
 ,@SOURCE_NAME VARCHAR(50)        
 ,@SEARCH_TEXT VARCHAR(30)        
 ,@SSN VARCHAR(50)        
 ,@FIRST_NAME VARCHAR(50)        
 ,@LAST_NAME VARCHAR(50)        
 ,@ASSIGN_TO VARCHAR(50)        
 ,@SORT_BY VARCHAR(50)        
 ,@SORT_ORDER VARCHAR(20)        
 ,@INDEXED_BY VARCHAR(100)        
 ,@INCLUDE_ARCHIVE BIT        
 ,@MEDICAL_RECORD_NUMBER VARCHAR(100)        
 ,@USER_NAME VARCHAR(50) = NULL        
 ,@DATE_FROM DATETIME        
 ,@DATE_TO DATETIME        
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
 --DECLARE @ISSN VARCHAR(50);            
 --DECLARE @IUNIQUE_ID VARCHAR(50);            
 DECLARE @USERROLE VARCHAR(50);        
        
 SET @USERROLE = (        
   SELECT R.ROLE_NAME        
   FROM FOX_TBL_APPLICATION_USER UR        
   INNER JOIN FOX_TBL_ROLE R ON R.ROLE_ID = UR.ROLE_ID        
   WHERE UPPER(USER_NAME) = UPPER(@USER_NAME)        
   )        
        
 DECLARE @ARCHIVEDATE DATE = getdate() - 30        
        
 IF (@INCLUDE_ARCHIVE = 0)        
 BEGIN        
  SET @ARCHIVEDATE = GETDATE() - 120        
 END        
        
 IF (@ASSIGN_TO = '')        
 BEGIN        
  SET @ASSIGN_TO = NULL        
 END        
 ELSE        
 BEGIN        
  SET @ASSIGN_TO = '%' + @ASSIGN_TO + '%'        
 END        
        
 SET NOCOUNT ON;        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1        
        
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE        
 DECLARE @TOATL_PAGESUDM FLOAT        
        
 SELECT @TOATL_PAGESUDM = COUNT(*)        
 FROM FOX_TBL_WORK_QUEUE FOX            
 LEFT JOIN fox_tbl_document_type DT ON DT.DOCUMENT_TYPE_ID=FOX.DOCUMENT_TYPE AND ISNULL(DT.DELETED,0)=0        
 LEFT JOIN PATIENT P ON FOX.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT             AND ISNULL(P.DELETED,0)=0        
 LEFT JOIN FOX_TBL_SENDER  SENDER ON FOX.SENDER_ID = SENDER.SENDER_ID     AND ISNULL(SENDER.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER au ON FOX.ASSIGNED_TO = au.USER_NAME    AND ISNULL(AU.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER cb ON FOX.COMPLETED_BY = cb.USER_NAME    AND ISNULL(CB.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER ab ON FOX.ASSIGNED_BY = ab.USER_NAME    AND ISNULL(AB.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER mb ON FOX.MODIFIED_BY = mb.USER_NAME    AND ISNULL(MB.DELETED,0)=0        
 WHERE assigned_to = @USER_NAME and convert(datetime,RECEIVE_DATE) between convert(datetime,@DATE_FROM) and convert(datetime,@DATE_TO) and FOX.PRACTICE_CODE = @PRACTICE_CODE and WORK_STATUS ='completed'         
 --WHERE FOX.PRACTICE_CODE = @PRACTICE_CODE        
  AND fox.DELETED = 0        
  --AND WORK_STATUS = 'completed'        
  AND (        
   @SEARCH_TEXT IS NULL        
   OR ISNULL(SORCE_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(SORCE_TYPE, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(p.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(p.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(WORK_ID, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(p.First_Name, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(p.Last_Name, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(convert(VARCHAR, P.Chart_Id), '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(au.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(au.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(ACCOUNT_NUMBER, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(UNIT_CASE_NO, '') LIKE '%' + @SEARCH_TEXT + '%'       
   OR ISNULL(UNIQUE_ID, '') LIKE '%' + @SEARCH_TEXT + '%'        
   or ISNULL(dt.NAME, '') LIKE '%' + @SEARCH_TEXT + '%'       
   --OR (        
   -- CASE         
   --  WHEN isnull(FOX.DOCUMENT_TYPE, 0) = 1        
   --   THEN 'POC'        
   --  WHEN isnull(FOX.DOCUMENT_TYPE, 0) = 2        
   --   THEN 'Referral Order'        
   --  WHEN isnull(FOX.DOCUMENT_TYPE, 0) = 3        
   --   THEN 'Other'        
   --  ELSE ''        
   --  END        
   -- ) LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(SENDER.SENDER_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(cb.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(cb.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(ab.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(ab.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(mb.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR ISNULL(mb.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
   OR TOTAL_PAGES LIKE '%' + @SEARCH_TEXT + '%'        
   OR convert(VARCHAR, FOX.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
   OR (CONVERT(VARCHAR, DATEDIFF(HOUR, FOX.RECEIVE_DATE, FOX.COMPLETED_DATE)) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, FOX.RECEIVE_DATE, FOX.COMPLETED_DATE) % 60)) LIKE '%' + @SEARCH_TEXT + '%'        
   OR (CONVERT(VARCHAR, DATEDIFF(HOUR, FOX.RECEIVE_DATE, FOX.ASSIGNED_DATE)) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, FOX.RECEIVE_DATE, FOX.ASSIGNED_DATE) % 60)) LIKE '%' + @SEARCH_TEXT + '%'        
   OR convert(VARCHAR, FOX.COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
   OR convert(VARCHAR, FOX.ASSIGNED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
   OR convert(VARCHAR, FOX.MODIFIED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
   )        
  AND (        
   @MEDICAL_RECORD_NUMBER IS NULL        
   OR ISNULL(convert(VARCHAR, P.Chart_Id), '') LIKE '%' + @MEDICAL_RECORD_NUMBER + '%'        
   )        
  AND (        
   INDEXED_BY LIKE ISNULL(@INDEXED_BY, INDEXED_BY)        
   OR INDEXED_BY IS NULL        
   )        
  AND (        
   @SOURCE_NAME IS NULL        
   OR ISNULL(SORCE_NAME, '') LIKE '%' + @SOURCE_NAME + '%'        
   )        
  AND (        
   @SOURCE_TYPE IS NULL        
   OR ISNULL(SORCE_TYPE, '') LIKE '%' + @SOURCE_TYPE + '%'        
   )        
  AND (        
   (        
    @ASSIGN_TO IS NULL        
    OR ISNULL(au.FIRST_NAME, '') LIKE '%' + @ASSIGN_TO + '%'        
    )        
   OR (        
    @ASSIGN_TO IS NULL        
    OR ISNULL(au.LAST_NAME, '') LIKE '%' + @ASSIGN_TO + '%'        
    )        
   )        
  AND (        
   @FIRST_NAME IS NULL        
   OR ISNULL(p.FIRST_NAME, '') LIKE '%' + @FIRST_NAME + '%'        
   )        
  AND (        
   @LAST_NAME IS NULL        
   OR ISNULL(p.LAST_NAME, '') LIKE '%' + @LAST_NAME + '%'        
   )        
  AND (        
   @SSN IS NULL        
   OR ISNULL(SSN, '') LIKE '%' + @SSN + '%'        
   )        
  AND (        
   @UNIQUE_ID IS NULL        
   OR ISNULL(UNIQUE_ID, '') LIKE '%' + @UNIQUE_ID + '%'        
   )        
   AND CONVERT(date ,FOX.Created_Date) >= '06/08/2019'    
  --AND cast(fox.RECEIVE_DATE AS DATE) > @ARCHIVEDATE        
  --AND (        
  -- @USERROLE = 'SUPERVISOR'        
  -- OR @USERROLE = 'ADMINISTRATOR'        
  -- OR fox.ASSIGNED_TO = @USER_NAME        
  -- )        
        
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
        
 SELECT *        
  ,@TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES        
  ,@TOTAL_RECORDS TOTAL_RECORDS        
 FROM (        
  SELECT WORK_ID        
   ,UNIQUE_ID        
   ,p.FIRST_NAME        
   ,p.LAST_NAME        
   ,p.DATE_OF_BIRTH        
   ,SSN        
   ,SORCE_TYPE        
   ,SORCE_NAME        
   ,ACCOUNT_NUMBER        
   ,FOX.UNIT_CASE_NO        
   ,FOX.CREATED_DATE        
   ,ISNULL(convert(VARCHAR, P.Chart_Id), '') AS MEDICAL_RECORD_NUMBER        
   ,au.LAST_NAME + ', ' + au.FIRST_NAME AS ASSIGNED_TO        
   , ISNULL(dt.NAME, '') DOCUMENT_TYPE,DT.DOCUMENT_TYPE_ID,         
   --,CASE         
   -- WHEN isnull(DOCUMENT_TYPE, 0) = 1        
   --  THEN 'POC'        
   -- WHEN isnull(DOCUMENT_TYPE, 0) = 2        
   --  THEN 'Referral Order'        
   -- WHEN isnull(DOCUMENT_TYPE, 0) = 3        
   --  THEN 'Other'        
   -- ELSE ''        
   -- END AS DOCUMENT_TYPE        
   cb.LAST_NAME + ', ' + cb.FIRST_NAME AS COMPLETED_BY        
   ,SENDER.SENDER_NAME        
   ,TOTAL_PAGES        
   ,FOX.RECEIVE_DATE        
   ,convert(varchar,FOX.RECEIVE_DATE) AS Received_Date_Str        
   ,CONVERT(VARCHAR, DATEDIFF(HOUR, FOX.RECEIVE_DATE, FOX.COMPLETED_DATE)) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, FOX.RECEIVE_DATE, FOX.COMPLETED_DATE) % 60) AS ELAPSECOMPLETIONTIME        
   ,CONVERT(VARCHAR, DATEDIFF(HOUR, FOX.RECEIVE_DATE, FOX.ASSIGNED_DATE)) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, FOX.RECEIVE_DATE, FOX.ASSIGNED_DATE) % 60) AS ELAPSEASSIGNTIME        
   ,FOX.COMPLETED_DATE        
   ,convert(varchar,FOX.COMPLETED_DATE) AS Completed_Date_Str        
   ,ab.LAST_NAME + ', ' + ab.FIRST_NAME AS ASSIGNED_BY        
   ,mb.LAST_NAME + ', ' + mb.FIRST_NAME AS MODIFIED_BY        
   ,FOX.ASSIGNED_DATE        
   ,convert(varchar,FOX.ASSIGNED_DATE) AS Assigned_Date_Str        
   ,FOX.MODIFIED_DATE        
   ,convert(varchar,FOX.MODIFIED_DATE) AS Modified_Date_Str        
   ,ROW_NUMBER() OVER (        
    ORDER BY UNIQUE_ID ASC        
    ) AS ACTIVEROW        
   ,IS_EMERGENCY_ORDER IS_EMERGENCY        
   ,CONVERT(BIT, CASE         
     WHEN ISNULL(IsSigned, 0) = 1        
      OR FOX.CREATED_BY LIKE 'FOX TEAM'        
      OR CHARINDEX('_', FOX.UNIQUE_ID) > 0        
      THEN 0        
     ELSE 1        
     END) AS IS_UNSIGNED        
  FROM FOX_TBL_WORK_QUEUE FOX            
 LEFT JOIN fox_tbl_document_type DT ON DT.DOCUMENT_TYPE_ID=FOX.DOCUMENT_TYPE AND ISNULL(DT.DELETED,0)=0        
 LEFT JOIN PATIENT P ON FOX.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT             AND ISNULL(P.DELETED,0)=0        
 LEFT JOIN FOX_TBL_SENDER  SENDER ON FOX.SENDER_ID = SENDER.SENDER_ID     AND ISNULL(SENDER.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER au ON FOX.ASSIGNED_TO = au.USER_NAME    AND ISNULL(AU.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER cb ON FOX.COMPLETED_BY = cb.USER_NAME    AND ISNULL(CB.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER ab ON FOX.ASSIGNED_BY = ab.USER_NAME    AND ISNULL(AB.DELETED,0)=0        
 LEFT JOIN FOX_TBL_APPLICATION_USER mb ON FOX.MODIFIED_BY = mb.USER_NAME    AND ISNULL(MB.DELETED,0)=0        
     WHERE assigned_to = @USER_NAME and convert(datetime,RECEIVE_DATE) between convert(datetime,@DATE_FROM) and convert(datetime,@DATE_TO) and FOX.PRACTICE_CODE = @PRACTICE_CODE and WORK_STATUS ='completed'         
        
  --WHERE FOX.PRACTICE_CODE = @PRACTICE_CODE        
   AND fox.DELETED = 0        
  -- AND WORK_STATUS = 'completed'        
   AND (        
    @SEARCH_TEXT IS NULL        
    OR ISNULL(SORCE_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(SORCE_TYPE, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(p.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(p.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(WORK_ID, '') LIKE '%' + @SEARCH_TEXT + '%'       
    OR ISNULL(p.First_Name, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(p.Last_Name, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(convert(VARCHAR, P.Chart_Id), '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(au.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(au.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(ACCOUNT_NUMBER, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(UNIT_CASE_NO, '') LIKE '%' + @SEARCH_TEXT + '%'        
OR ISNULL(UNIQUE_ID, '') LIKE '%' + @SEARCH_TEXT + '%'        
 or ISNULL(dt.NAME, '') LIKE '%' + @SEARCH_TEXT + '%'       
    --OR (        
    -- CASE         
    --  WHEN isnull(FOX.DOCUMENT_TYPE, 0) = 1        
    --   THEN 'POC'        
    --  WHEN isnull(FOX.DOCUMENT_TYPE, 0) = 2        
    --   THEN 'Referral Order'        
    --  WHEN isnull(FOX.DOCUMENT_TYPE, 0) = 3        
    --   THEN 'Other'        
    --  ELSE ''        
    --  END        
    -- ) LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(SENDER.SENDER_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(cb.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(cb.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(ab.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(ab.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(mb.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR ISNULL(mb.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'        
    OR TOTAL_PAGES LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'        
    OR (CONVERT(VARCHAR, DATEDIFF(HOUR, FOX.RECEIVE_DATE, FOX.COMPLETED_DATE)) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, FOX.RECEIVE_DATE, FOX.COMPLETED_DATE) % 60)) LIKE '%' + @SEARCH_TEXT + '%'        
    OR (CONVERT(VARCHAR, DATEDIFF(HOUR, FOX.RECEIVE_DATE, FOX.ASSIGNED_DATE)) + ':' + CONVERT(VARCHAR, DATEDIFF(MINUTE, FOX.RECEIVE_DATE, FOX.ASSIGNED_DATE) % 60)) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.ASSIGNED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.MODIFIED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.COMPLETED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.ASSIGNED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'        
    OR convert(VARCHAR, FOX.MODIFIED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'        
    )        
   AND (        
    (        
     @ASSIGN_TO IS NULL        
     OR ISNULL(au.FIRST_NAME, '') LIKE '%' + @ASSIGN_TO + '%'        
     )        
    OR (        
     @ASSIGN_TO IS NULL        
     OR ISNULL(au.LAST_NAME, '') LIKE '%' + @ASSIGN_TO + '%'        
     )        
    )        
   AND (        
    @MEDICAL_RECORD_NUMBER IS NULL        
    OR ISNULL(convert(VARCHAR, P.Chart_Id), '') LIKE '%' + @MEDICAL_RECORD_NUMBER + '%'        
    )        
   AND (        
    INDEXED_BY LIKE ISNULL(@INDEXED_BY, INDEXED_BY)        
    OR INDEXED_BY IS NULL        
    )        
   AND (        
    @SOURCE_NAME IS NULL        
    OR ISNULL(SORCE_NAME, '') LIKE '%' + @SOURCE_NAME + '%'        
    )        
   AND (        
    @SOURCE_TYPE IS NULL        
    OR ISNULL(SORCE_TYPE, '') LIKE '%' + @SOURCE_TYPE + '%'        
    )        
   --AND  (@ASSIGN_TO IS NULL    OR     ISNULL(ASSIGNED_TO,'') LIKE '%'+ @ASSIGN_TO + '%')            
   AND (        
    @FIRST_NAME IS NULL        
    OR ISNULL(p.FIRST_NAME, '') LIKE '%' + @FIRST_NAME + '%'        
    )        
   AND (        
    @LAST_NAME IS NULL        
    OR ISNULL(p.LAST_NAME, '') LIKE '%' + @LAST_NAME + '%'        
    )        
   AND (   
    @SSN IS NULL        
    OR ISNULL(SSN, '') LIKE '%' + @SSN + '%'        
    )        
   AND (        
    @UNIQUE_ID IS NULL        
    OR ISNULL(UNIQUE_ID, '') LIKE '%' + @UNIQUE_ID + '%'        
    )    
 AND CONVERT(date ,FOX.Created_Date) >= '06/08/2019'        
   --AND cast(fox.RECEIVE_DATE AS DATE) > @ARCHIVEDATE        
   --AND (        
   -- @USERROLE = 'SUPERVISOR'        
   -- OR @USERROLE = 'ADMINISTRATOR'        
   -- OR fox.ASSIGNED_TO = @USER_NAME        
   -- )        
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
   WHEN @SORT_BY = 'DOB'        
    AND @SORT_ORDER = 'ASC'        
    THEN Date_Of_Birth        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'DOB'        
    AND @SORT_ORDER = 'DESC'        
    THEN Date_Of_Birth        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'PatientFirstName'        
    AND @SORT_ORDER = 'ASC'        
    THEN FIRST_NAME        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'PatientFirstName'        
    AND @SORT_ORDER = 'DESC'        
    THEN FIRST_NAME        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'PatientLastName'        
    AND @SORT_ORDER = 'ASC'        
    THEN LAST_NAME        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'PatientLastName'        
    AND @SORT_ORDER = 'DESC'        
    THEN LAST_NAME        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'AssignTo'        
    AND @SORT_ORDER = 'DESC'        
    THEN ASSIGNED_TO        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'AssignTo'        
    AND @SORT_ORDER = 'ASC'        
    THEN ASSIGNED_TO        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'SourceType'        
    AND @SORT_ORDER = 'ASC'        
    THEN SORCE_TYPE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'SourceType'        
    AND @SORT_ORDER = 'DESC'        
    THEN SORCE_TYPE        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'SourceName'        
    AND @SORT_ORDER = 'ASC'        
    THEN SORCE_NAME        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'SourceName'        
    AND @SORT_ORDER = 'DESC'        
    THEN SORCE_NAME        
   END DESC        
  ,CASE         
WHEN @SORT_BY = 'AccountNumber'        
    AND @SORT_ORDER = 'ASC'        
    THEN ACCOUNT_NUMBER        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'AccountNumber'        
    AND @SORT_ORDER = 'DESC'        
    THEN ACCOUNT_NUMBER        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'UnitCaseNumber'        
    AND @SORT_ORDER = 'ASC'        
    THEN UNIT_CASE_NO        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'UnitCaseNumber'        
    AND @SORT_ORDER = 'DESC'        
    THEN UNIT_CASE_NO        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'DocumentType'        
    AND @SORT_ORDER = 'ASC'        
    THEN DOCUMENT_TYPE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'DocumentType'        
    AND @SORT_ORDER = 'DESC'        
    THEN DOCUMENT_TYPE        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'SenderName'        
    AND @SORT_ORDER = 'ASC'        
    THEN SENDER_NAME        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'SenderName'        
    AND @SORT_ORDER = 'DESC'        
    THEN SENDER_NAME        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'Pages'        
    AND @SORT_ORDER = 'ASC'        
    THEN TOTAL_PAGES        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'Pages'        
    AND @SORT_ORDER = 'DESC'        
    THEN TOTAL_PAGES        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'DateTimeReceived'        
    AND @SORT_ORDER = 'ASC'        
    THEN RECEIVE_DATE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'DateTimeReceived'        
    AND @SORT_ORDER = 'DESC'        
    THEN RECEIVE_DATE        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'ElapsedCompletionTime'        
    AND @SORT_ORDER = 'ASC'        
    THEN (DATEDIFF(MINUTE, WORK_QUEUE.RECEIVE_DATE, WORK_QUEUE.COMPLETED_DATE))        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'ElapsedCompletionTime'        
    AND @SORT_ORDER = 'DESC'        
    THEN (DATEDIFF(MINUTE, WORK_QUEUE.RECEIVE_DATE, WORK_QUEUE.COMPLETED_DATE))        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'ElapsedAssignedTime'        
    AND @SORT_ORDER = 'ASC'        
    THEN (DATEDIFF(MINUTE, WORK_QUEUE.RECEIVE_DATE, WORK_QUEUE.ASSIGNED_DATE))        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'ElapsedAssignedTime'        
    AND @SORT_ORDER = 'DESC'        
    THEN (DATEDIFF(MINUTE, WORK_QUEUE.RECEIVE_DATE, WORK_QUEUE.ASSIGNED_DATE))        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'CompletedBy'        
    AND @SORT_ORDER = 'ASC'        
    THEN COMPLETED_BY        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'CompletedBy'        
    AND @SORT_ORDER = 'DESC'        
    THEN COMPLETED_BY        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'CompletedDateTime'        
    AND @SORT_ORDER = 'ASC'        
    THEN COMPLETED_DATE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'CompletedDateTime'        
    AND @SORT_ORDER = 'DESC'        
    THEN COMPLETED_DATE        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'AssignedBy'        
    AND @SORT_ORDER = 'ASC'        
    THEN ASSIGNED_BY        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'AssignedBy'        
    AND @SORT_ORDER = 'DESC'        
    THEN ASSIGNED_BY        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'AssignedDateTime'        
    AND @SORT_ORDER = 'ASC'        
    THEN ASSIGNED_DATE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'AssignedDateTime'        
    AND @SORT_ORDER = 'DESC'        
    THEN ASSIGNED_DATE        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'UpdatedBy'        
    AND @SORT_ORDER = 'ASC'        
    THEN MODIFIED_BY        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'UpdatedBy'        
    AND @SORT_ORDER = 'DESC'        
    THEN MODIFIED_BY        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'UpdatedDateTime'        
    AND @SORT_ORDER = 'ASC'        
    THEN MODIFIED_DATE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'UpdatedDateTime'        
    AND @SORT_ORDER = 'DESC'        
    THEN MODIFIED_DATE        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'MedicalRecordNumber'        
    AND @SORT_ORDER = 'ASC'        
    THEN MEDICAL_RECORD_NUMBER        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'MedicalRecordNumber'        
    AND @SORT_ORDER = 'DESC'        
    THEN MEDICAL_RECORD_NUMBER        
   END DESC        
  ,CASE         
   WHEN isnull(@SORT_BY, '') = ''        
    AND isnull(@SORT_ORDER, '') = ''        
    THEN COMPLETED_DATE        
   END DESC OFFSET @START_FROM ROWS        
        
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY        
END   
  
