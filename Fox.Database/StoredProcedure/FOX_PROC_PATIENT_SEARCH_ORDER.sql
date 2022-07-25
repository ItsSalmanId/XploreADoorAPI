IF (OBJECT_ID('FOX_PROC_PATIENT_SEARCH_ORDER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_PATIENT_SEARCH_ORDER  
GO      
--exec FOX_PROC_PATIENT_SEARCH_ORDER '','','',null,'','','','',null,null,1011163 , 1, 100 ,'','POC'                        
--[dbo].[FOX_PROC_PATIENT_SEARCH_ORDER_USAMA] NULL,NULL,null, null, null, null, null, null, null, null, null, null,1011163 , 1, 10000, '','','status','DESC'                        
--EXEC FOX_PROC_PATIENT_SEARCH_ORDER_SATTAR_03012019 '','','',NULL,'','','','',null,null,1011163,1,1000,'','','status','ASC'                            
--exec [FOX_PROC_PATIENT_SEARCH_ORDER_ARSLAN_TEST] '','','','','',null,'','','','',null,null,1011163 , 1, 100 ,'','1', 'receivedate'   , 'desc'                                  
CREATE PROCEDURE [dbo].[FOX_PROC_PATIENT_SEARCH_ORDER] --null, null, null, null, null, null, null, null, null, null,1011163 , 1, 100, '',''                       
               
(@PATIENT_ACCOUNT  VARCHAR(50),                        
 @MRN                VARCHAR(100),                      
 @FIRST_NAME        VARCHAR(50),                        
 @LAST_NAME         VARCHAR(50),                        
 @SSN               VARCHAR(10),                        
 @DATE_OF_BRITH     DATETIME,                        
 @SOURCE_NAME       VARCHAR(100),                        
 --,@SOURCE_TYPE VARCHAR(100)                        
 @REFERRAL_REGION   VARCHAR(100),                        
 @SENDER_FIRST_NAME VARCHAR(100),                        
 @SENDER_LAST_NAME  VARCHAR(100),                        
 @RECEIVED_FROM     DATETIME,                        
 @RECEIVED_TO       DATETIME,                        
 @PRACTICE_CODE     BIGINT,                        
 @CURRENT_PAGE      INT,                        
 @RECORD_PER_PAGE   INT,                        
 @SEARCH_TEXT       VARCHAR(30),                        
 @DOCUMENT_TYPE     VARCHAR(100),                        
 @SORT_BY VARCHAR(50),                        
 @SORT_ORDER VARCHAR(5)                        
)                        
AS                        
     BEGIN                       
  IF(@PATIENT_ACCOUNT = '')                        
             BEGIN                        
                 SET @PATIENT_ACCOUNT = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @PATIENT_ACCOUNT = @PATIENT_ACCOUNT+'%';                        
             END;                      
  IF(@MRN = '')                        
             BEGIN                        
                 SET @MRN = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @MRN = @MRN+'%';                        
             END;                        
         IF(@FIRST_NAME = '')                        
             BEGIN                        
                 SET @FIRST_NAME = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @FIRST_NAME = @FIRST_NAME+'%';                        
             END;                        
         IF(@LAST_NAME = '')                        
             BEGIN                        
                 SET @LAST_NAME = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @LAST_NAME = @LAST_NAME+'%';                        
             END;                        
         IF(@SSN = '')                        
             BEGIN                        
                 SET @SSN = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @SSN = @SSN+'%';                        
             END;                        
         IF(@SOURCE_NAME = '')                        
             BEGIN                        
                 SET @SOURCE_NAME = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @SOURCE_NAME = @SOURCE_NAME+'%';                 
             END;                        
                        
         --IF (@SOURCE_TYPE = '')                            
         --BEGIN           
         -- SET @SOURCE_TYPE = NULL                            
         --END                            
         --ELSE                            
         --BEGIN                
         -- SET @SOURCE_TYPE = @SOURCE_TYPE + '%'                            
         --END                            
                        
         IF(@REFERRAL_REGION = '')                        
             BEGIN                      
                 SET @REFERRAL_REGION = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @REFERRAL_REGION = @REFERRAL_REGION;                        
             END;                        
         IF(@SENDER_FIRST_NAME = '')                        
             BEGIN                        
                 SET @SENDER_FIRST_NAME = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @SENDER_FIRST_NAME = @SENDER_FIRST_NAME+'%';                        
             END;                     
         IF(@SENDER_LAST_NAME = '')                        
             BEGIN                        
                 SET @SENDER_LAST_NAME = NULL;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @SENDER_LAST_NAME = @SENDER_LAST_NAME+'%';                        
             END;                        
         IF(@DOCUMENT_TYPE = '')                        
             BEGIN                        
                 SET @DOCUMENT_TYPE = NULL;                        
     END;                        
             ELSE                        
             BEGIN                        
                 SET @DOCUMENT_TYPE = @DOCUMENT_TYPE;                        
             END;                        
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                        
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                        
         DECLARE @TOATL_PAGESUDM FLOAT;                        
         SELECT @TOATL_PAGESUDM = COUNT(*)                        
         FROM FOX_TBL_WORK_QUEUE wq                        
              LEFT JOIN Patient p ON wq.PATIENT_ACCOUNT = p.Patient_Account                        
                                     AND ISNULL(wq.DELETED, 0) = 0                        
              LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE rs ON wq.SENDER_ID = rs.SOURCE_ID                        
                                                          AND ISNULL(rs.DELETED, 0) = 0                        
              LEFT JOIN FOX_TBL_REFERRAL_REGION AS rr ON rr.REFERRAL_REGION_ID =                        
             (                        
                 SELECT TOP 1 REFERRAL_REGION_ID                        
                 FROM FOX_TBL_REFERRAL_REGION AS ftrr                        
                 WHERE ftrr.REFERRAL_REGION_CODE = rs.REFERRAL_REGION                        
                       AND ISNULL(ftrr.DELETED, 0) = 0                        
              AND ftrr.PRACTICE_CODE = @PRACTICE_CODE                        
 AND ISNULL(ftrr.IS_INACTIVE, 0) = 0                        
             )                        
              --LEFT JOIN FOX_TBL_REFERRAL_REGION rr ON rs.REFERRAL_REGION = rr.REFERRAL_REGION_CODE    
              --                                        AND ISNULL(rr.IS_INACTIVE, 0) = 0                        
              --                                        AND ISNULL(rr.DELETED, 0) = 0                        
                        
              LEFT JOIN FOX_TBL_DOCUMENT_TYPE AS dt ON dt.DOCUMENT_TYPE_ID = wq.DOCUMENT_TYPE                        
                                                       AND ISNULL(dt.DELETED, 0) = 0                        
         WHERE(wq.PRACTICE_CODE = @PRACTICE_CODE)                        
              AND (p.Patient_Account LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR p.Chart_Id LIKE '%'+@SEARCH_TEXT+'%'                       
          OR p.First_Name LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR p.Last_Name LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR p.SSN LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR wq.SORCE_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR wq.SORCE_TYPE LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR rs.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                  OR rs.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR rr.REFERRAL_REGION_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR wq.WORK_ID LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR wq.WORK_STATUS LIKE '%'+@SEARCH_TEXT+'%'                        
  OR dt.NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR CONVERT(VARCHAR, p.Date_Of_Birth, 101) LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'                        
                   OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%')                        
              AND isnull(wq.DELETED, 0) = 0                        
              AND (@RECEIVED_FROM IS NULL                        
                   OR @RECEIVED_TO IS NULL                        
                   OR wq.RECEIVE_DATE BETWEEN @RECEIVED_FROM AND @RECEIVED_TO)                      
     AND (@PATIENT_ACCOUNT IS NULL                  
                   OR p.Patient_Account LIKE '%'+@PATIENT_ACCOUNT+'%')                      
     AND (@MRN IS NULL                        
                   OR p.Chart_Id LIKE '%'+@MRN+'%')                         
              AND (@FIRST_NAME IS NULL                        
                   OR p.First_Name LIKE '%'+@FIRST_NAME+'%')                        
              AND (@LAST_NAME IS NULL                      
                   OR p.Last_Name LIKE '%'+@LAST_NAME+'%')                        
              AND (@SSN IS NULL                        
                   OR p.SSN LIKE '%'+@SSN+'%')                        
              AND (@DATE_OF_BRITH IS NULL                        
                   OR CAST(p.Date_Of_Birth AS DATE) = CAST(@DATE_OF_BRITH AS DATE))                            
              --AND (                            
  -- @SOURCE_NAME IS NULL                            
              -- OR wq.SORCE_NAME LIKE @SOURCE_NAME + '%'                            
              -- )                            
              AND (@SOURCE_NAME IS NULL                        
        OR wq.SORCE_TYPE LIKE '%'+@SOURCE_NAME+'%')                        
              AND (@DOCUMENT_TYPE IS NULL                        
                   OR wq.DOCUMENT_TYPE = @DOCUMENT_TYPE)                        
      AND (@SENDER_FIRST_NAME IS NULL                        
                   OR rs.FIRST_NAME LIKE '%'+@SENDER_FIRST_NAME+'%')                        
             AND (@SENDER_LAST_NAME IS NULL                        
                   OR rs.LAST_NAME LIKE '%'+@SENDER_LAST_NAME+'%')                        
              AND (@REFERRAL_REGION IS NULL                        
                   OR rr.REFERRAL_REGION_CODE = @REFERRAL_REGION                        
                   OR rr.REFERRAL_REGION_NAME = @REFERRAL_REGION);                        
         IF(@RECORD_PER_PAGE = 0)                        
             BEGIN                        
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;                        
             END;                        
             ELSE                        
             BEGIN                        
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                        
             END;                        
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                        
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                        
         SELECT *,                         
                @TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES,                         
                @TOTAL_RECORDS TOTAL_RECORDS                        
         FROM                        
         (               
             SELECT wq.WORK_ID,                         
                    wq.UNIQUE_ID,                      
     P.Patient_Account,                      
     P.Chart_Id as MRN,                      
                    p.First_Name,                         
                    p.Last_Name,                         
                    RTRIM(LTRIM(RIGHT(p.SSN,4))) AS SSN,                            
                    --,p.SSN                         
        p.Date_Of_Birth,                        
     convert(varchar, p.Date_Of_Birth) AS Date_of_Birth_Str,                          
                    wq.SORCE_TYPE AS SOURCE_NAME,                         
                    wq.RECEIVE_DATE,                         
     convert(varchar, wq.RECEIVE_DATE) AS Received_Date_Str,                        
                    wq.WORK_STATUS,                         
                    wq.COMPLETED_DATE,                         
     convert(varchar, wq.COMPLETED_DATE) AS Completed_Date_Str,            
                    Convert(varchar,wq.DOCUMENT_TYPE) AS DOCUMENT_TYPE,          
     dt.NAME AS DOCUMENT_TYPE_NAME,                         
                    rs.FIRST_NAME AS SENDER_FIRST_NAME,                         
                    rs.LAST_NAME AS SENDER_LAST_NAME,                         
                    rr.REFERRAL_REGION_NAME AS REGION_NAME,                         
                    ROW_NUMBER() OVER(ORDER BY wq.RECEIVE_DATE ASC) AS ACTIVEROW                        
             FROM FOX_TBL_WORK_QUEUE wq                      
                  LEFT JOIN Patient p ON wq.PATIENT_ACCOUNT = p.Patient_Account                        
                                         AND ISNULL(wq.DELETED, 0) = 0                        
                  LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE rs ON wq.SENDER_ID = rs.SOURCE_ID                        
                                                              AND ISNULL(rs.DELETED, 0) = 0                        
                  --LEFT JOIN FOX_TBL_REFERRAL_REGION rr ON rs.REFERRAL_REGION = rr.REFERRAL_REGION_CODE                        
                  --                                        AND ISNULL(rr.IS_INACTIVE, 0) = 0                        
                  --                AND ISNULL(rr.DELETED, 0) = 0                        
                  LEFT JOIN FOX_TBL_REFERRAL_REGION AS rr ON rr.REFERRAL_REGION_ID =                        
             (                        
                 SELECT TOP 1 REFERRAL_REGION_ID                        
                 FROM FOX_TBL_REFERRAL_REGION AS ftrr                        
                 WHERE ftrr.REFERRAL_REGION_CODE = rs.REFERRAL_REGION                        
                       AND ISNULL(ftrr.DELETED, 0) = 0                        
                       AND ftrr.PRACTICE_CODE = @PRACTICE_CODE                        
                       AND ISNULL(ftrr.IS_INACTIVE, 0) = 0                        
             )                        
                  LEFT JOIN FOX_TBL_DOCUMENT_TYPE AS dt ON dt.DOCUMENT_TYPE_ID = wq.DOCUMENT_TYPE                        
                                                           AND ISNULL(dt.DELETED, 0) = 0                        
             WHERE(wq.PRACTICE_CODE = @PRACTICE_CODE)                        
                  AND (p.Patient_Account LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR p.Chart_Id LIKE '%'+@SEARCH_TEXT+'%'                       
           OR p.First_Name LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR p.Last_Name LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR p.SSN LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR wq.SORCE_NAME LIKE '%'+@SEARCH_TEXT+'%'              
                       OR wq.SORCE_TYPE LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR rs.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR rs.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR rr.REFERRAL_REGION_NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR wq.WORK_ID LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR wq.WORK_STATUS LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR dt.NAME LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR CONVERT(VARCHAR, p.Date_Of_Birth, 101) LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR CONVERT(VARCHAR, wq.RECEIVE_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 101) LIKE '%'+@SEARCH_TEXT+'%'                        
                       OR CONVERT(VARCHAR, wq.COMPLETED_DATE, 100) LIKE '%'+@SEARCH_TEXT+'%')                        
                  AND isnull(wq.DELETED, 0) = 0                        
                  AND (@RECEIVED_FROM IS NULL                        
                       OR @RECEIVED_TO IS NULL                        
                       OR wq.RECEIVE_DATE BETWEEN @RECEIVED_FROM AND @RECEIVED_TO)                      
      AND (@PATIENT_ACCOUNT IS NULL                        
                       OR p.Patient_Account LIKE '%'+@PATIENT_ACCOUNT+'%')               
         AND (@MRN IS NULL                        
                       OR p.Chart_Id LIKE '%'+@MRN+'%')                         
                  AND (@FIRST_NAME IS NULL                        
                       OR p.First_Name LIKE '%'+@FIRST_NAME+'%')                        
                  AND (@LAST_NAME IS NULL                        
                       OR p.Last_Name LIKE '%'+@LAST_NAME+'%')                        
                  AND (@SSN IS NULL                        
                       OR p.SSN LIKE '%'+@SSN+'%')                        
                  AND (@DATE_OF_BRITH IS NULL                        
                       OR CAST(p.Date_Of_Birth AS DATE) = CAST(@DATE_OF_BRITH AS DATE))                            
                  --AND (                            
                  -- @SOURCE_NAME IS NULL                          
                  -- OR wq.SORCE_NAME LIKE @SOURCE_NAME + '%'                            
                  -- )                            
                  AND (@SOURCE_NAME IS NULL                        
                       OR wq.SORCE_TYPE LIKE '%'+@SOURCE_NAME+'%')                        
   AND (@DOCUMENT_TYPE IS NULL                        
                       OR wq.DOCUMENT_TYPE = @DOCUMENT_TYPE)              
                  AND (@SENDER_FIRST_NAME IS NULL                        
                       OR rs.FIRST_NAME LIKE '%'+@SENDER_FIRST_NAME+'%')                        
                  AND (@SENDER_LAST_NAME IS NULL                        
                       OR rs.LAST_NAME LIKE '%'+@SENDER_LAST_NAME+'%')                        
                  AND (@REFERRAL_REGION IS NULL                        
                       OR rr.REFERRAL_REGION_CODE = @REFERRAL_REGION                        
                       OR rr.REFERRAL_REGION_NAME = @REFERRAL_REGION)                        
         ) AS SEARCH_ORDER                        
   ORDER BY CASE                        
 WHEN @SORT_BY = 'uniqueid'                        
    AND @SORT_ORDER = 'ASC'                        
    THEN UNIQUE_ID                        
   END ASC                        
  ,CASE                        
   WHEN @SORT_BY = 'uniqueid'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN UNIQUE_ID                        
   END DESC                        
   ,CASE                           
 WHEN @SORT_BY = 'pfirstname'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN First_Name                                 
   END ASC                      
   ,CASE                           
   WHEN @SORT_BY = 'Patient_Account'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN Patient_Account                                  
   END ASC                      
   ,CASE                        
   WHEN @SORT_BY = 'Patient_Account'                        
    AND @SORT_ORDER = 'DESC'                        
    THEN Patient_Account                        
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
   WHEN @SORT_BY = 'pfirstname'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN First_Name                                  
   END DESC                           
   ,CASE                                
   WHEN @SORT_BY = 'plastname'                                  
    AND @SORT_ORDER = 'ASC'                                 
 THEN Last_Name                                  
   END ASC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'plastname'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN Last_Name                                  
   END DESC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'SSN'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN SSN                           
 END ASC                                
  ,CASE                                   
   WHEN @SORT_BY = 'SSN'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN SSN                                  
   END DESC                           
   ,CASE                                   
   WHEN @SORT_BY = 'dateofbirth'                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN Date_Of_Birth                        
 END ASC                                
  ,CASE                                   
   WHEN @SORT_BY = 'dateofbirth'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN Date_Of_Birth                        
   END DESC                           
   ,CASE                             WHEN @SORT_BY = 'senderfname'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN SENDER_FIRST_NAME                             
  END ASC                               
  ,CASE                                   
   WHEN @SORT_BY = 'senderfname'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN SENDER_FIRST_NAME                                  
   END DESC                          
   ,CASE                          
   WHEN @SORT_BY = 'senderlname'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN SENDER_LAST_NAME                             
  END ASC                               
  ,CASE                                   
  WHEN @SORT_BY = 'senderlname'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN SENDER_LAST_NAME                                  
   END DESC                          
   ,CASE                              
    WHEN @SORT_BY = 'sourcetype'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN SOURCE_NAME                                  
   END ASC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'sourcetype'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN SOURCE_NAME                                  
   END DESC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'doctype'                               
    AND @SORT_ORDER = 'ASC'                                  
    THEN DOCUMENT_TYPE                                  
   END ASC                                  
,CASE                                   
   WHEN @SORT_BY = 'doctype'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN DOCUMENT_TYPE                                  
   END DESC                          
   ,CASE                          
   WHEN @SORT_BY = 'regionname'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN REGION_NAME                                  
   END ASC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'regionname'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN REGION_NAME                                  
   END DESC                          
   ,CASE                          
   WHEN @SORT_BY = 'receivedate'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN RECEIVE_DATE                                  
   END ASC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'receivedate'                                  
  AND @SORT_ORDER = 'DESC'                                  
    THEN RECEIVE_DATE                                  
   END DESC                                 
  ,CASE                               
  WHEN @SORT_BY = 'status'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN WORK_STATUS                                  
   END ASC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'status'                                  
    AND @SORT_ORDER = 'DESC'                                  
    THEN WORK_STATUS                                  
   END DESC                                 
  ,CASE                                       
  WHEN @SORT_BY = 'completedate'                                  
    AND @SORT_ORDER = 'ASC'                                  
    THEN COMPLETED_DATE                                  
   END ASC                                  
  ,CASE                                   
   WHEN @SORT_BY = 'completedate'         
    AND @SORT_ORDER = 'DESC'                                  
    THEN COMPLETED_DATE                                  
   END DESC                                 
  ,CASE                        
  WHEN @SORT_BY = ''                                  
    AND @SORT_ORDER = ''                                  
    THEN SEARCH_ORDER.RECEIVE_DATE                        
 END DESC OFFSET @START_FROM ROWS                         
        FETCH NEXT @RECORD_PER_PAGE ROWS ONLY               
END;
