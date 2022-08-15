IF (OBJECT_ID('FOX_PROC_GET_UNASSIGNED_QUEUE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_UNASSIGNED_QUEUE  
GO 
  -- EXEC [dbo].[FOX_PROC_GET_UNASSIGNED_QUEUE_RAFAY_TEST] 1011163,1,10,'','','','','','',''                       
  -- EXEC [dbo].[FOX_PROC_GET_UNASSIGNED_QUEUE_RAFAY_TEST1] 1011163,1,10,'','','','','','',''                  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_UNASSIGNED_QUEUE]                      
  (                        
 @PRACTICE_CODE BIGINT                        
 ,@CURRENT_PAGE INT                        
 ,@RECORD_PER_PAGE INT = 10                        
 ,@SEARCH_TEXT VARCHAR(30)                        
 ,@SORCE_STRING VARCHAR(100)                        
 ,@SORCE_TYPE VARCHAR(50)                        
 ,@UNIQUE_ID VARCHAR(100)                        
 ,@SORT_BY VARCHAR(50)                
 ,@SORT_ORDER VARCHAR(50)                        
 ,@INCLUDE_ARCHIVE BIT                        
 )                      
WITH RECOMPILE                      
AS                        
BEGIN                        
 SET NOCOUNT ON;                        
                        
 IF (@SORCE_STRING = '')                        
 BEGIN                        
  SET @SORCE_STRING = NULL                        
 END                        
 ELSE                        
 BEGIN                        
  SET @SORCE_STRING = '%' + @SORCE_STRING + '%'                        
 END                        
                        
 IF (@SORCE_TYPE = '')                        
 BEGIN                        
  SET @SORCE_TYPE = NULL                        
 END                        
 ELSE                        
 BEGIN                        
  SET @SORCE_TYPE = '%' + @SORCE_TYPE + '%'                        
 END                        
                        
 IF (@UNIQUE_ID = '')                        
 BEGIN                        
  SET @UNIQUE_ID = NULL                        
 END                        
 ELSE                        
 BEGIN                        
  SET @UNIQUE_ID = '%' + @UNIQUE_ID + '%'                        
 END                       
   IF CHARINDEX('(',@SEARCH_TEXT) > 0                    
 BEGIN                    
 SET @SEARCH_TEXT = REPLACE(@SEARCH_TEXT,'(', '')                    
 END                    
 IF CHARINDEX(')',@SEARCH_TEXT) > 0                    
 BEGIN                    
 SET @SEARCH_TEXT = REPLACE(@SEARCH_TEXT,')', '')                    
 END                    
   IF CHARINDEX(' ',@SEARCH_TEXT) > 0                    
 BEGIN                    
 SET @SEARCH_TEXT = REPLACE(@SEARCH_TEXT,' ', '')                    
 END                
   IF CHARINDEX('-',@SEARCH_TEXT) > 0                    
 BEGIN                    
 SET @SEARCH_TEXT = REPLACE(@SEARCH_TEXT,'-', '')                    
 END                 
                
              
 DECLARE @ArchiveDate DATE= GETDATE() - 30;                        
    IF(@INCLUDE_ARCHIVE = 1)                        
        BEGIN                        
            SET @ArchiveDate = GETDATE() - 120;                        
      END;                        
                        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                        
                        
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                        
 DECLARE @TOATL_PAGESUDM FLOAT                        
                        
 SET @TOATL_PAGESUDM = (                        
   SELECT COUNT(*)                        
   FROM FOX_TBL_WORK_QUEUE WQ                      
   LEFT JOIN Patient p ON WQ.PATIENT_ACCOUNT = p.Patient_Account                      
   LEFT JOIN FOX_TBL_REFERRAL_SENDER RS ON RS.SENDER = WQ.SORCE_NAME                      
                                         and ISNULL(RS.FOR_STRATEGIC, 0) = 1                      
              AND ISNULL(RS.DELETED, 0) = 0                  
   LEFT JOIN FOX_TBL_OCR_STATUS OC ON OC.OCR_STATUS_ID = WQ.OCR_STATUS_ID                  
    AND ISNULL(OC.DELETED, 0) = 0           
  LEFT JOIN dbo.FOX_TBL_PATIENT AS ftp ON WQ.Patient_Account = ftp.Patient_Account                        
   LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC ON ftp.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                      
              AND WQ.Practice_Code = FC.PRACTICE_CODE                      
              AND ISNULL(FC.DELETED, 0) = 0    
   WHERE WQ.PRACTICE_CODE = @PRACTICE_CODE                        
    AND (                        
     WQ.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                        
     OR Replace(WQ.SORCE_TYPE,' ','') LIKE '%' + @SEARCH_TEXT + '%'                        
     OR WQ.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                        
     OR CONVERT(VARCHAR, WQ.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                        
     OR CONVERT(VARCHAR, WQ.RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'                        
     OR (Convert(VARCHAR, DateDiff(HOUR, WQ.CREATED_DATE, GETDATE())) + ':' + Convert(VARCHAR, DateDiff(MINUTE, WQ.CREATED_DATE, GETDATE()) % 60)) LIKE '%' + @SEARCH_TEXT + '%'                        
     OR Replace(p.First_Name,' ','') LIKE '%' + @SEARCH_TEXT + '%'                      
     OR Replace(p.Last_Name,' ','') LIKE '%' + @SEARCH_TEXT + '%'                      
     OR p.Chart_Id LIKE '%' + @SEARCH_TEXT + '%'                      
  )                        
    AND ISNULL(WQ.DELETED, 0) = 0                        
    AND WQ.SORCE_NAME LIKE ISNULL(@SORCE_STRING, WQ.SORCE_NAME)                        
    AND WQ.SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, WQ.SORCE_TYPE)                        
    AND WQ.UNIQUE_ID LIKE ISNULL(@UNIQUE_ID, WQ.UNIQUE_ID)                        
 AND CAST(WQ.RECEIVE_DATE AS DATE) > @ArchiveDate                      
  --  AND (                        
  -- (@INCLUDE_ARCHIVE = 0 AND CAST(RECEIVE_DATE AS DATE) >= @ArchiveDate)                        
  --OR                        
  -- (@INCLUDE_ARCHIVE = 1)                        
  --)                        
    AND WQ.ASSIGNED_TO IS NULL                        
   )                        
                        
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
  SELECT WQ.WORK_ID                        
   ,WQ.UNIQUE_ID                        
   ,WQ.PRACTICE_CODE                        
   ,WQ.SORCE_TYPE                        
   ,WQ.SORCE_NAME                        
   ,WQ.WORK_STATUS                        
   ,WQ.RECEIVE_DATE                      
   ,p.First_Name AS PATIENT_FIRST_NAME                      
   ,p.Last_Name AS PATIENT_LAST_NAME                      
   ,p.Chart_Id AS MRN                      
   ,convert(varchar,WQ.RECEIVE_DATE) AS Received_Date_Str                        
   ,WQ.TOTAL_PAGES                        
   ,WQ.NO_OF_SPLITS                        
   ,WQ.FILE_PATH                        
   ,WQ.ASSIGNED_TO                        
   ,WQ.ASSIGNED_BY                        
   ,WQ.ASSIGNED_DATE                        
   ,WQ.COMPLETED_BY                        
   ,WQ.COMPLETED_DATE                        
   ,WQ.CREATED_BY                        
 ,                        
   --Convert(varchar,DateDiff(HOUR, CREATED_DATE, GETDATE()))                        
   (convert(VARCHAR, datediff(minute, WQ.CREATED_DATE, getdate()) / 60) + ':' + Convert(VARCHAR, DateDiff(MINUTE, WQ.CREATED_DATE, GETDATE()) % 60)) AS ElapseTime                        
   ,WQ.CREATED_DATE                        
   ,WQ.MODIFIED_BY                        
   ,WQ.MODIFIED_DATE                        
   ,WQ.DELETED                        
   ,ROW_NUMBER() OVER (                        
    ORDER BY WQ.CREATED_DATE DESC                        
    ) AS ACTIVEROW                        
   ,WQ.IS_EMERGENCY_ORDER AS IS_EMRERGENCY                        
   ,CONVERT(BIT, CASE                         
     WHEN ISNULL(WQ.IsSigned, 0) = 1                        
  OR WQ.CREATED_BY LIKE 'FOX TEAM'                        
      OR CHARINDEX('_', WQ.UNIQUE_ID) > 0                        
      THEN 0                        
     ELSE 1                        
     END) AS IS_UNSIGNED,                      
  CONVERT(BIT,                      
      CASE                       
    WHEN  ISNULL(RS.FOR_STRATEGIC, 0) = 1                      
    THEN 1           
 WHEN  ISNULL(FC.CODE, '') = 'SA'                      
    THEN 1     
    ELSE 0                      
    END) AS IS_STRATEGIC,                    
 --CONVERT(BIT,                      
 --     CASE                       
 --   WHEN  ISNULL(WQ.IS_OCR, 0) = 1                      
 --   THEN 1                      
 --   ELSE 0                      
 --   END) AS IS_OCR                   
 WQ.OCR_STATUS_ID,       
  OC.OCR_STATUS AS OCR_STATUS                  
  FROM FOX_TBL_WORK_QUEUE WQ                      
   LEFT JOIN Patient p ON WQ.PATIENT_ACCOUNT = p.Patient_Account                      
   LEFT JOIN FOX_TBL_REFERRAL_SENDER RS ON RS.SENDER = WQ.SORCE_NAME                      
                                         and ISNULL(RS.FOR_STRATEGIC, 0) = 1                      
              AND ISNULL(RS.DELETED, 0) = 0                       
   LEFT JOIN FOX_TBL_OCR_STATUS OC ON OC.OCR_STATUS_ID = WQ.OCR_STATUS_ID                  
    AND ISNULL(OC.DELETED, 0) = 0          
   LEFT JOIN dbo.FOX_TBL_PATIENT AS ftp ON WQ.Patient_Account = ftp.Patient_Account                        
   LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC ON ftp.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                      
              AND WQ.Practice_Code = FC.PRACTICE_CODE                      
              AND ISNULL(FC.DELETED, 0) = 0    
  WHERE WQ.PRACTICE_CODE = @PRACTICE_CODE                        
   AND (                        
   WQ.SORCE_NAME LIKE '%' + @SEARCH_TEXT + '%'                        
    OR Replace(WQ.SORCE_TYPE,' ','') LIKE '%' + @SEARCH_TEXT + '%'                        
    OR WQ.UNIQUE_ID LIKE '%' + @SEARCH_TEXT + '%'                        
    OR CONVERT(VARCHAR, WQ.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                        
    OR CONVERT(VARCHAR, WQ.RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'                        
    OR (Convert(VARCHAR, DateDiff(HOUR, WQ.CREATED_DATE, GETDATE())) + ':' + Convert(VARCHAR, DateDiff(MINUTE, WQ.CREATED_DATE, GETDATE()) % 60)) LIKE '%' + @SEARCH_TEXT + '%'                        
    OR Replace(p.First_Name,' ','') LIKE '%' + @SEARCH_TEXT + '%'                      
    OR Replace(p.Last_Name,' ','') LIKE '%' + @SEARCH_TEXT + '%'                       
    OR p.Chart_Id LIKE '%' + @SEARCH_TEXT + '%'                      
    )                        
   AND ISNULL(WQ.DELETED, 0) = 0                        
   AND WQ.SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME)                        
   AND WQ.SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)                        
   AND WQ.UNIQUE_ID LIKE ISNULL(@UNIQUE_ID, UNIQUE_ID)                        
   AND CAST(WQ.RECEIVE_DATE AS DATE) > @ArchiveDate                      
  -- AND (                        
  -- (@INCLUDE_ARCHIVE = 0 AND CAST(RECEIVE_DATE AS DATE) >= @ArchiveDate)                        
  --  OR                        
  -- (@INCLUDE_ARCHIVE = 1 )                        
  --)                        
   AND WQ.ASSIGNED_TO IS NULL                        
  ) AS WORK_QUEUE                        
 ORDER BY CASE                         
   WHEN @SORT_BY = 'ID'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN WORK_QUEUE.UNIQUE_ID                        
   END ASC                        
,CASE                         
   WHEN @SORT_BY = 'ID'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN WORK_QUEUE.UNIQUE_ID                        
   END DESC                        
  ,CASE                         
   WHEN @SORT_BY = 'SourceType'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN WORK_QUEUE.SORCE_TYPE                        
   END ASC                        
  ,CASE           
   WHEN @SORT_BY = 'SourceType'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN WORK_QUEUE.SORCE_TYPE                        
   END DESC                        
  ,CASE                         
   WHEN @SORT_BY = 'SourceName'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN WORK_QUEUE.SORCE_NAME                        
   END ASC                        
  ,CASE                         
   WHEN @SORT_BY = 'SourceName'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN WORK_QUEUE.SORCE_NAME                        
   END DESC                        
,CASE                         
   WHEN @SORT_BY = 'DateTimeReceived'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN WORK_QUEUE.RECEIVE_DATE                        
   END ASC                        
  ,CASE                         
   WHEN @SORT_BY = 'DateTimeReceived'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN WORK_QUEUE.RECEIVE_DATE                        
   END DESC                        
  ,CASE                         
   WHEN @SORT_BY = 'Elapsed'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN DateDiff(MINUTE, CREATED_DATE, GETDATE())                   
   END ASC                        
  ,CASE                         
   WHEN @SORT_BY = 'Elapsed'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN DateDiff(MINUTE, CREATED_DATE, GETDATE())                        
   END DESC                        
  ,CASE                         
   WHEN @SORT_BY = 'Pages'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN WORK_QUEUE.TOTAL_PAGES                      
   END ASC                        
  ,CASE                         
   WHEN @SORT_BY = 'Pages'                        
  AND @SORT_ORDER = 'DESC'                        
    THEN WORK_QUEUE.TOTAL_PAGES                        
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
   WHEN @SORT_BY = ''                        
    AND @SORT_ORDER = ''                        
    THEN WORK_QUEUE.CREATED_DATE          
   END DESC OFFSET @START_FROM ROWS                        
                        
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                        
END       
  

