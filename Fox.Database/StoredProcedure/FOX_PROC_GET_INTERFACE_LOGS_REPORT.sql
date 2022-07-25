IF (OBJECT_ID('FOX_PROC_GET_INTERFACE_LOGS_REPORT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INTERFACE_LOGS_REPORT  
GO          
-- =============================================        
-- AUTHOR:  <DEVELOPER, USMAN>        
-- CREATE DATE: <CREATE DATE, 12/20/2018>        
-- DESCRIPTION: <GET ALL INTERFACE LOGS LIST>        
        
--EXEC [FOX_PROC_GET_INTERFACE_LOGS_REPORT_8958] 1012714,'ALL','',1,0, 'CREATED_DATE', 'DESC','ALL' ,'PORTAL' ,'09/01/2019' ,'10/03/2019'      
CREATE PROCEDURE [DBO].[FOX_PROC_GET_INTERFACE_LOGS_REPORT]         
 @PRACTICE_CODE        BIGINT,        
 @OPTION  VARCHAR(20),                              
 @SEARCH_STRING        VARCHAR(100),                     
 @CURRENT_PAGE         INT,                     
 @RECORD_PER_PAGE      INT,        
 @SORT_BY VARCHAR(20),        
 @SORT_ORDER VARCHAR(10),      
 @STATUS VARCHAR(20),    
 @APPLICATION  VARCHAR(20),      
 @DATE_FROM DATETIME,      
 @DATE_TO DATETIME      
AS        
BEGIN        
        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1        
        
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE        
 DECLARE @TOATL_PAGESUDM FLOAT        
 DECLARE @DOB DATETIME        
 DECLARE @SUCCESSFULINTERFACED INT        
 DECLARE @FAILEDINTERFACED INT        
      
 --DECLARE @SEARCH_STRING VARCHAR(100) = '%ING%'      
 DECLARE @IS_INCOMMING BIT = NULL, @IS_OUTGOING BIT = NULL      
 IF 'INCOMING' LIKE '%' + @SEARCH_STRING + '%'       
 BEGIN      
 --SELECT 1      
 SET @IS_INCOMMING = 1      
 END      
 IF 'OUTGOING' LIKE '%' + @SEARCH_STRING + '%'       
 BEGIN      
 --SELECT 2      
 SET @IS_OUTGOING = 1      
 END      
      
        
       
select @SUCCESSFULINTERFACED = COUNT(*)      
FROM FOX_TBL_INTERFACE_LOG  IT      
LEFT JOIN FOX_TBL_INTERFACE_SYNCH INS ON IT.FOX_INTERFACE_SYNCH_ID = INS.FOX_INTERFACE_SYNCH_ID AND  ISNULL(INS.DELETED, 0) = 0     AND INS.PRACTICE_CODE = @PRACTICE_CODE        
 JOIN PATIENT P ON IT.PATIENT_ACCOUNT = P.Patient_Account      
 WHERE   ISNULL(IT.DELETED, 0) = 0         
   AND IT.PRACTICE_CODE = @PRACTICE_CODE        
    AND      
        
    CAST(IT.CREATED_DATE AS DATE)  >= CONVERT(DATE, REPLACE(@DATE_FROM, '%', ''), 101) AND          
      CAST(IT.CREATED_DATE AS DATE)   <= CONVERT(DATE, REPLACE(@DATE_TO, '%', ''), 101)      
   AND (@APPLICATION = 'ALL' OR INS.APPLICATION LIKE @APPLICATION)    
  -- -- INCOMING/OUTGOING      
     AND        
     (        
      @STATUS = 'ALL'          
    OR        
    (        
      @STATUS = 'SUCCESS'        
      AND        
      (        
   IS_ERROR =0                    
      )        
                )        
    OR        
    (        
      @STATUS = 'FAILURE'        
      AND        
      (        
  IS_ERROR =1                
      )        
                )        
     )                
  -- STATUS      
     AND        
     (        
      @OPTION = 'ALL'          
    OR        
    (        
      @OPTION = 'INCOMING'        
      AND        
      (        
   IT.IS_INCOMMING = 1                    
      )        
                )        
    OR        
    (        
      @OPTION = 'OUTGOING'        
      AND        
      (        
  IT.IS_OUTGOING = 1                    
      )        
                )        
     )    
                  
  AND      
  (@SEARCH_STRING = '' OR(      
   CONVERT(VARCHAR(100),IT.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'    
   OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'          
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                       
    OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                         
     OR IT.ERROR LIKE  '%' + @SEARCH_STRING + '%'     
  OR INS.APPLICATION LIKE  '%' + @SEARCH_STRING + '%'             
   OR CONVERT(VARCHAR(20),IT.CREATED_DATE) LIKE  '%' + @SEARCH_STRING + '%'      
   OR (IT.IS_INCOMMING = @IS_INCOMMING AND @IS_INCOMMING IS NOT NULL)      
 OR (IT.IS_OUTGOING = @IS_OUTGOING AND @IS_OUTGOING IS NOT NULL)        )  )       
        
   AND IS_ERROR =0;      
       
         
         
         
select @FAILEDINTERFACED = COUNT(*)      
FROM FOX_TBL_INTERFACE_LOG  IT      
LEFT JOIN FOX_TBL_INTERFACE_SYNCH INS ON IT.FOX_INTERFACE_SYNCH_ID = INS.FOX_INTERFACE_SYNCH_ID AND  ISNULL(INS.DELETED, 0) = 0     AND INS.PRACTICE_CODE = @PRACTICE_CODE        
 JOIN PATIENT P ON IT.PATIENT_ACCOUNT = P.Patient_Account      
 WHERE   ISNULL(IT.DELETED, 0) = 0         
   AND IT.PRACTICE_CODE = @PRACTICE_CODE        
    AND      
        
    CAST(IT.CREATED_DATE AS DATE)  >= CONVERT(DATE, REPLACE(@DATE_FROM, '%', ''), 101) AND          
      CAST(IT.CREATED_DATE AS DATE)   <= CONVERT(DATE, REPLACE(@DATE_TO, '%', ''), 101)      
    AND (@APPLICATION = 'ALL' OR INS.APPLICATION LIKE @APPLICATION)    
  -- -- INCOMING/OUTGOING      
     AND        
     (        
      @STATUS = 'ALL'          
    OR        
    (        
      @STATUS = 'SUCCESS'        
      AND        
      (        
   IS_ERROR =0                    
      )        
                )        
    OR        
    (        
      @STATUS = 'FAILURE'        
    AND        
      (        
  IS_ERROR =1                
      )        
                )        
     )                
  -- STATUS      
     AND        
     (        
      @OPTION = 'ALL'          
    OR        
    (        
      @OPTION = 'INCOMING'        
      AND        
      (        
   IT.IS_INCOMMING = 1                    
      )        
                )        
    OR        
    (        
     @OPTION = 'OUTGOING'        
      AND        
      (        
  IT.IS_OUTGOING = 1                    
      )        
                )        
     )    
  AND      
  (@SEARCH_STRING = '' OR(      
    CONVERT(VARCHAR(100),IT.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'    
 OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                        
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                       
    OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                         
     OR IT.ERROR LIKE  '%' + @SEARCH_STRING + '%'       
  OR INS.APPLICATION LIKE  '%' + @SEARCH_STRING + '%'           
   OR CONVERT(VARCHAR(20),IT.CREATED_DATE) LIKE  '%' + @SEARCH_STRING + '%'        
    OR (IT.IS_INCOMMING = @IS_INCOMMING AND @IS_INCOMMING IS NOT NULL)      
   OR (IT.IS_OUTGOING = @IS_OUTGOING AND @IS_OUTGOING IS NOT NULL)       )     )       
      
   AND IS_ERROR =1;       
      
      
 SELECT @TOATL_PAGESUDM = COUNT(*)        
 FROM FOX_TBL_INTERFACE_LOG  IT      
 LEFT JOIN FOX_TBL_INTERFACE_SYNCH INS ON IT.FOX_INTERFACE_SYNCH_ID = INS.FOX_INTERFACE_SYNCH_ID AND  ISNULL(INS.DELETED, 0) = 0     AND INS.PRACTICE_CODE = @PRACTICE_CODE        
 JOIN PATIENT P ON IT.PATIENT_ACCOUNT = P.Patient_Account      
 WHERE   ISNULL(IT.DELETED, 0) = 0         
   AND IT.PRACTICE_CODE = @PRACTICE_CODE        
    AND      
        
    CAST(IT.CREATED_DATE AS DATE)  >= CONVERT(DATE, REPLACE(@DATE_FROM, '%', ''), 101) AND          
      CAST(IT.CREATED_DATE AS DATE)   <= CONVERT(DATE, REPLACE(@DATE_TO, '%', ''), 101)      
    AND (@APPLICATION = 'ALL' OR INS.APPLICATION LIKE @APPLICATION)    
  -- -- INCOMING/OUTGOING      
     AND        
     (        
      @STATUS = 'ALL'          
    OR        
    (        
      @STATUS = 'SUCCESS'        
      AND        
      (        
   IS_ERROR =0                    
      )        
                )        
    OR        
    (        
      @STATUS = 'FAILURE'        
      AND        
      (        
  IS_ERROR =1                
      )        
                )        
     )                
  -- STATUS      
     AND        
     (        
      @OPTION = 'ALL'          
    OR        
    (        
      @OPTION = 'INCOMING'        
AND        
 (        
   IT.IS_INCOMMING = 1                    
      )        
    )        
    OR        
    (        
      @OPTION = 'OUTGOING'        
      AND        
      (        
  IT.IS_OUTGOING = 1                    
      )        
                )        
     )    
  AND      
  (@SEARCH_STRING = '' OR(      
    CONVERT(VARCHAR(100),IT.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'    
 OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                          
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'            
    OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                         
     OR IT.ERROR LIKE  '%' + @SEARCH_STRING + '%'      
  OR INS.APPLICATION LIKE  '%' + @SEARCH_STRING + '%'            
   OR CONVERT(VARCHAR(20),IT.CREATED_DATE) LIKE  '%' + @SEARCH_STRING + '%'       
    OR (IT.IS_INCOMMING = @IS_INCOMMING AND @IS_INCOMMING IS NOT NULL)      
   OR (IT.IS_OUTGOING = @IS_OUTGOING AND @IS_OUTGOING IS NOT NULL)        )     )       
      
         
      
        
 IF (@RECORD_PER_PAGE = 0)        
  BEGIN        
   SET @RECORD_PER_PAGE = @TOATL_PAGESUDM        
  END        
 ELSE        
  BEGIN        
   SET @RECORD_PER_PAGE = @RECORD_PER_PAGE        
  END        
        
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM        
        
  IF (@RECORD_PER_PAGE <> 0)        
  BEGIN      
 SET @TOATL_PAGESUDM = CEILING(IsNull(@TOATL_PAGESUDM,0) / IsNull(@RECORD_PER_PAGE,1))        
        
 SELECT *,                     
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                     
                @TOTAL_RECORDS AS TOTAL_RECORDS,      
    @FAILEDINTERFACED AS FAILED_INTERFACED,      
    @SUCCESSFULINTERFACED AS SUCCESSFUL_INTERFACED      
                        
         FROM                    
         (                    
             SELECT --*      
   P.Last_Name + ', ' + P.First_Name AS PATIENT_NAME,      
   IT.FOX_INTERFACE_LOG_ID,       
   IT.PRACTICE_CODE,       
   IT.PATIENT_ACCOUNT AS PATIENT_ACCOUNT,       
   CONVERT(VARCHAR(30),IT.PATIENT_ACCOUNT) AS PATIENT_ACCOUNT_str,       
   P.Chart_Id AS MRN,    
   '"' + IT.ERROR + '"' AS ERROR,       
   '"' + IT.ACK + '"' AS ACK,       
   IT.IS_ERROR,       
   '"' + IT.LOG_MESSAGE + '"' AS LOG_MESSAGE,       
   IT.IS_INCOMMING,       
   IT.IS_OUTGOING,       
   IT.CREATED_BY,       
   IT.CREATED_DATE,      
   INS.APPLICATION,    
   CASE       
   WHEN IT.IS_OUTGOING  = 1 AND IT.IS_INCOMMING = 0       
   THEN 'Outgoing'      
   ELSE 'Incoming'      
   END AS TYPE,      
   convert(varchar,IT.CREATED_DATE) AS Created_Date_Str,      
   IT.MODIFIED_BY,       
   IT.MODIFIED_DATE,       
   IT.DELETED,      
   ROW_NUMBER() OVER ( ORDER BY IT.CREATED_DATE DESC ) AS ACTIVEROW      
   FROM FOX_TBL_INTERFACE_LOG IT      
   LEFT JOIN FOX_TBL_INTERFACE_SYNCH INS ON IT.FOX_INTERFACE_SYNCH_ID = INS.FOX_INTERFACE_SYNCH_ID AND  ISNULL(INS.DELETED, 0) = 0     AND INS.PRACTICE_CODE = @PRACTICE_CODE        
   JOIN PATIENT P ON IT.PATIENT_ACCOUNT = P.Patient_Account      
    WHERE   ISNULL(IT.DELETED, 0) = 0         
     AND IT.PRACTICE_CODE = @PRACTICE_CODE        
    
     --AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
  AND      
     
    CAST(IT.CREATED_DATE AS DATE)  >= CONVERT(DATE, REPLACE(@DATE_FROM, '%', ''), 101) AND          
      CAST(IT.CREATED_DATE AS DATE)   <= CONVERT(DATE, REPLACE(@DATE_TO, '%', ''), 101)      
    AND (@APPLICATION = 'ALL' OR INS.APPLICATION LIKE @APPLICATION)    
      -- -- INCOMING/OUTGOING      
     AND        
     (        
      @STATUS = 'ALL'          
    OR        
    (        
      @STATUS = 'SUCCESS'        
      AND        
      (        
   IS_ERROR = 0                    
      )        
                )        
    OR        
    (        
      @STATUS = 'FAILURE'        
      AND        
      (        
   IS_ERROR = 1                    
      )        
                )        
     )                
  -- STATUS      
   AND        
     (        
      @OPTION = 'ALL'          
    OR        
    (        
      @OPTION = 'INCOMING'        
      AND        
      (        
   IT.IS_INCOMMING = 1                    
      )        
                )        
    OR        
    (        
      @OPTION = 'OUTGOING'        
      AND        
      (        
  IT.IS_OUTGOING = 1                    
      )        
                )        
     )    
      
  AND      
 ( @SEARCH_STRING = '' OR(      
  CONVERT(VARCHAR(100),IT.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'    
  OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                       
    OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                         
      OR IT.ERROR LIKE  '%' + @SEARCH_STRING + '%'          
      OR INS.APPLICATION LIKE  '%' + @SEARCH_STRING + '%'          
   OR CONVERT(VARCHAR(20),IT.CREATED_DATE) LIKE  '%' + @SEARCH_STRING + '%'          
    OR (IT.IS_INCOMMING = @IS_INCOMMING AND @IS_INCOMMING IS NOT NULL)      
   OR (IT.IS_OUTGOING = @IS_OUTGOING AND @IS_OUTGOING IS NOT NULL)      )        )      
      
      
         ) AS FOX_TBL_INTERFACE_LOG        
             
 ORDER BY        
  CASE WHEN @SORT_BY LIKE 'APPLICATION' AND @SORT_ORDER = 'ASC' THEN [APPLICATION]END ASC,        
  CASE WHEN @SORT_BY LIKE 'APPLICATION' AND @SORT_ORDER = 'DESC' THEN [APPLICATION] END DESC,      
  CASE WHEN @SORT_BY LIKE 'CREATED_DATE' AND @SORT_ORDER = 'ASC' THEN CREATED_DATE END ASC,        
  CASE WHEN @SORT_BY LIKE 'CREATED_DATE' AND @SORT_ORDER = 'DESC' THEN CREATED_DATE END DESC,      
    CASE WHEN @SORT_BY LIKE 'PATIENT_ACCOUNT' AND @SORT_ORDER = 'ASC' THEN PATIENT_ACCOUNT END ASC,        
  CASE WHEN @SORT_BY LIKE 'PATIENT_ACCOUNT' AND @SORT_ORDER = 'DESC' THEN PATIENT_ACCOUNT END DESC,    
  CASE WHEN @SORT_BY LIKE 'MRN' AND @SORT_ORDER = 'ASC' THEN MRN END ASC,        
  CASE WHEN @SORT_BY LIKE 'MRN' AND @SORT_ORDER = 'DESC' THEN MRN END DESC,      
    CASE WHEN @SORT_BY LIKE 'REASON' AND @SORT_ORDER = 'ASC' THEN LOG_MESSAGE END ASC,        
  CASE WHEN @SORT_BY LIKE 'REASON' AND @SORT_ORDER = 'DESC' THEN LOG_MESSAGE END DESC,      
    CASE WHEN @SORT_BY LIKE 'TYPE' AND @SORT_ORDER = 'ASC' THEN IS_INCOMMING END ASC,        
  CASE WHEN @SORT_BY LIKE 'TYPE' AND @SORT_ORDER = 'DESC' THEN IS_INCOMMING END DESC ,      
  CASE WHEN @SORT_BY LIKE 'PATIENT_NAME' AND @SORT_ORDER = 'ASC' THEN PATIENT_NAME END ASC,      
  CASE WHEN @SORT_BY LIKE 'PATIENT_NAME' AND @SORT_ORDER = 'DESC' THEN PATIENT_NAME END       
      
 DESC OFFSET @START_FROM ROWS        
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY        
        
END       
END   
  
