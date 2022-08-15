IF (OBJECT_ID('FOX_PROC_REPORT_REFERRAL_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_REPORT_REFERRAL_LIST  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_REPORT_REFERRAL_LIST] --1011163, NULL, NULL, NULL, NULL, '', '', '', '', '', 1, 50, '', 'WORK_ID', 'ASC','1'         
 (        
   @PRACTICE_CODE BIGINT              
  ,@INDEXED_DATE_FROM DATETIME              
  ,@INDEXED_DATE_TO DATETIME              
  ,@RECEIVED_DATE_FROM DATETIME              
  ,@RECEIVED_DATE_TO DATETIME              
  ,@INDEXED_STATUS VARCHAR(100)              
  ,@STATUS VARCHAR(100)              
  ,@DOCUMENT_TYPE VARCHAR(100)              
  ,@SOURCE_TYPE VARCHAR(100)              
  ,@ASSIGNED_PERSON_NAME VARCHAR(100)              
  ,@CURRENT_PAGE INT              
  ,@RECORD_PER_PAGE INT              
  ,@SEARCH_TEXT VARCHAR(100)              
  ,@SORT_BY VARCHAR(20)              
  ,@SORT_ORDER VARCHAR(5)              
  ,@PRIORITY varchar(20)              
 )              
AS              
BEGIN              
---WHEN PRIORITY IS HIGH              
If(@PRIORITY = '1')              
begin              
 IF OBJECT_ID('TEMPDB..#TEMP_TABLE_REFERRAL_REPORT1') IS NOT NULL              
  DROP TABLE #TEMP_TABLE_REFERRAL_REPORT1              
              
 SELECT TEMP1.*              
   INTO #TEMP_TABLE_REFERRAL_REPORT1              
 FROM (              
    SELECT WQ.WORK_ID AS WORK_ID,case when IS_EMERGENCY_ORDER =1 then  'Emergency' when IS_EMERGENCY_ORDER =0 then 'Normal'end as [PRIORITY], WQ.UNIQUE_ID AS UNIQUE_ID, WQ.FAX_ID,WQ.IS_EMERGENCY_ORDER,              
                  
   ISNULL(dt.NAME, '') AS DOCUMENT_TYPE, dt.DOCUMENT_TYPE_ID,            
   --CASE              
      -- WHEN ISNULL(dt.DOCUMENT_TYPE_ID, 0) = 1 THEN 'POC'              
      -- WHEN ISNULL(dt.DOCUMENT_TYPE_ID, 0) = 2 THEN 'Referral Order'              
      -- WHEN ISNULL(dt.DOCUMENT_TYPE_ID, 0) = 3 THEN 'Other'              
      -- ELSE ''              
      --END AS DOCUMENT_TYPE,              
      CASE              
       WHEN WQ.INDEXED_BY IS NOT NULL AND (WQ.WORK_STATUS = 'INDEXED' OR WQ.WORK_STATUS = 'COMPLETED') THEN 'INDEXED'              
       WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS = 'INDEX PENDING' THEN 'INDEX PENDING'              
       WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS != 'INDEX PENDING' THEN 'IN PROGRESS'              
       ELSE ''              
      END AS INDEXING_STATUS,              
      (IB.LAST_NAME + ', ' + IB.FIRST_NAME) AS INDEXED_BY,           
   WQ.INDEXED_DATE AS INDEXED_DATE,           
          
   ((convert(varchar, WQ.INDEXED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.INDEXED_DATE as time), 0))) AS Indexed_Date_Str,           
          
          
   WQ.SORCE_TYPE AS SOURCE_TYPE,           
   WQ.RECEIVE_DATE,             
             
   ((convert(varchar, WQ.RECEIVE_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.RECEIVE_DATE as time), 0))) AS Received_Date_Str,           
          
              
      (AT.FIRST_NAME + ', ' + AT.LAST_NAME) AS ASSIGNED_TO, TR.ROLE_NAME, (CB.LAST_NAME + ', ' + CB.FIRST_NAME) AS COMPLETED_BY,              
      CASE              
       WHEN WQ.INDEXED_BY IS NULL AND (WORK_STATUS = 'CREATED' OR UPPER(ISNULL(WORK_STATUS,'INDEX PENDING')) = 'INDEX PENDING') THEN 'PENDING'              
       WHEN WQ.INDEXED_BY IS NOT NULL AND WQ.WORK_STATUS = 'COMPLETED' THEN 'COMPLETED'              
       ELSE ''              
      END AS WORK_STATUS,              
       WQ.COMPLETED_DATE AS COMPLETED_DATE,          
              
   ((convert(varchar, WQ.COMPLETED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.COMPLETED_DATE as time), 0))) AS Completed_Date_Str          
          
               
    FROM FOX_TBL_WORK_QUEUE WQ              
      LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME              
              
      LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME              
      LEFT JOIN FOX_TBL_APPLICATION_USER IB ON WQ.ASSIGNED_BY = IB.USER_NAME          
      LEFT JOIN FOX_TBL_ROLE TR ON AT.ROLE_ID = TR.ROLE_ID              
  left join fox_tbl_document_type dt on dt.DOCUMENT_TYPE_ID=wq.DOCUMENT_TYPE            
    WHERE ISNULL(wq.DELETED, 0) = 0 AND wq.PRACTICE_CODE = @PRACTICE_CODE and wq.IS_EMERGENCY_ORDER = 1              
 and ISNULL(dt.DELETED, 0) = 0            
 and ISNULL(at.DELETED, 0) = 0            
 and ISNULL(cb.DELETED, 0) = 0            
 and ISNULL(ib.DELETED, 0) = 0           
 and ISNULL(tr.DELETED, 0) = 0            
            
   ) TEMP1              
              
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1              
              
 DECLARE @START_FROM1 INT = @CURRENT_PAGE * @RECORD_PER_PAGE              
 DECLARE @TOATL_PAGESUDM1 FLOAT              
              
 SELECT @TOATL_PAGESUDM1 = COUNT(*)              
 FROM #TEMP_TABLE_REFERRAL_REPORT1              
 WHERE (              
    WORK_ID LIKE @SEARCH_TEXT + '%'              
    OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'              
    OR INDEXED_BY LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
    OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
    OR ROLE_NAME LIKE @SEARCH_TEXT + '%'              
    OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'              
    OR WORK_STATUS LIKE @SEARCH_TEXT + '%'              
    OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
   )              
   AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'              
   AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'              
   AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'              
   AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )              
   AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )              
   AND WORK_STATUS LIKE '%' + @STATUS + '%'              
   AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'              
              
 IF (@RECORD_PER_PAGE = 0)              
 BEGIN              
  SET @RECORD_PER_PAGE = @TOATL_PAGESUDM1              
 END              
 ELSE              
 BEGIN              
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE              
 END              
              
 DECLARE @TOTAL_RECORDS1 INT = @TOATL_PAGESUDM1              
              
 SET @TOATL_PAGESUDM1 = CEILING(@TOATL_PAGESUDM1 / @RECORD_PER_PAGE)              
              
 SELECT *, @TOATL_PAGESUDM1 AS TOTAL_ROCORD_PAGES, @TOTAL_RECORDS1 TOTAL_RECORDS              
 FROM              
  (              
   SELECT WORK_ID ,PRIORITY,UNIQUE_ID, FAX_ID,DOCUMENT_TYPE, INDEXING_STATUS, INDEXED_BY ,INDEXED_DATE, Indexed_Date_Str, SOURCE_TYPE, RECEIVE_DATE, Received_Date_Str, ROLE_NAME,              
     ASSIGNED_TO, WORK_STATUS, COMPLETED_BY, COMPLETED_DATE, Completed_Date_Str              
   FROM #TEMP_TABLE_REFERRAL_REPORT1              
   WHERE (              
         WORK_ID LIKE @SEARCH_TEXT + '%'              
      OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'              
      OR INDEXED_BY LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
      OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
      OR ROLE_NAME LIKE @SEARCH_TEXT + '%'              
      OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'              
      OR WORK_STATUS LIKE @SEARCH_TEXT + '%'              
      OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
     )             
     AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'              
     AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'              
     AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'              
     AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )              
     AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )              
     AND WORK_STATUS LIKE '%' + @STATUS + '%'        
     AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'              
  ) AS REPORT_REFERRAL_LIST                  
                   
 ORDER BY                       
  case when @SORT_BY = 'WORK_ID' and @SORT_ORDER = 'ASC' then WORK_ID end ASC,                      
  case when @SORT_BY = 'WORK_ID'  and @SORT_ORDER = 'DESC' then WORK_ID end DESC,                      
  case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'ASC' then UNIQUE_ID end ASC,                      
  case when @SORT_BY = 'UNIQUE_ID'  and @SORT_ORDER = 'DESC' then UNIQUE_ID  end DESC,               
  case when @SORT_BY = 'DOCUMENT_TYPE' and @SORT_ORDER = 'ASC' then DOCUMENT_TYPE end ASC,                      
  case when @SORT_BY = 'DOCUMENT_TYPE'  and @SORT_ORDER = 'DESC' then DOCUMENT_TYPE end DESC,                      
  case when @SORT_BY = 'INDEXED_BY' and @SORT_ORDER = 'ASC' then INDEXED_BY end ASC,                      
  case when @SORT_BY = 'INDEXED_BY'  and @SORT_ORDER = 'DESC' then INDEXED_BY  end DESC,                      
  case when @SORT_BY = 'INDEXED_DATE' and @SORT_ORDER = 'ASC' then INDEXED_DATE end ASC,                      
  case when @SORT_BY = 'INDEXED_DATE'  and @SORT_ORDER = 'DESC' then INDEXED_DATE  end DESC,              
  case when @SORT_BY = 'SOURCE_TYPE' and @SORT_ORDER = 'ASC' then SOURCE_TYPE end ASC,                      
  case when @SORT_BY = 'SOURCE_TYPE'  and @SORT_ORDER = 'DESC' then SOURCE_TYPE end DESC,              
  case when @SORT_BY = 'ROLE_NAME' and @SORT_ORDER = 'ASC' then ROLE_NAME end ASC,                      
  case when @SORT_BY = 'ROLE_NAME'  and @SORT_ORDER = 'DESC' then ROLE_NAME  end DESC,                      
  case when @SORT_BY = 'ASSIGNED_TO' and @SORT_ORDER = 'ASC' then ASSIGNED_TO end ASC,                      
  case when @SORT_BY = 'ASSIGNED_TO'  and @SORT_ORDER = 'DESC' then ASSIGNED_TO  end DESC,              
  case when @SORT_BY = 'RECEIVE_DATE' and @SORT_ORDER = 'ASC' then RECEIVE_DATE end ASC,                      
  case when @SORT_BY = 'RECEIVE_DATE'  and @SORT_ORDER = 'DESC' then RECEIVE_DATE  end DESC,              
  case when @SORT_BY = 'WORK_STATUS' and @SORT_ORDER = 'ASC' then WORK_STATUS end ASC,                      
  case when @SORT_BY = 'WORK_STATUS'  and @SORT_ORDER = 'DESC' then WORK_STATUS end DESC,              
  case when @SORT_BY = 'COMPLETED_BY' and @SORT_ORDER = 'ASC' then COMPLETED_BY end ASC,                      
  case when @SORT_BY = 'COMPLETED_BY'  and @SORT_ORDER = 'DESC' then COMPLETED_BY end DESC,              
  case when @SORT_BY = 'COMPLETED_DATE' and @SORT_ORDER = 'ASC' then COMPLETED_DATE end ASC,                      
  case when @SORT_BY = 'COMPLETED_DATE'  and @SORT_ORDER = 'DESC' then COMPLETED_DATE end DESC,              
  case when @SORT_BY = 'PRIORITY' and @SORT_ORDER = 'ASC' then PRIORITY end ASC,                      
  case when @SORT_BY = 'PRIORITY'  and @SORT_ORDER = 'DESC' then PRIORITY end DESC                
                       
 OFFSET @START_FROM1 ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY              
end              
---WHEN PRIORITY IS LOW              
If(@PRIORITY = '0')              
begin              
 IF OBJECT_ID('TEMPDB..#TEMP_TABLE_REFERRAL_REPORT2') IS NOT NULL              
  DROP TABLE #TEMP_TABLE_REFERRAL_REPORT2              
              
 SELECT TEMP2.*              
   INTO #TEMP_TABLE_REFERRAL_REPORT2              
 FROM (              
    SELECT WQ.WORK_ID AS WORK_ID,case when IS_EMERGENCY_ORDER =1 then  'Emergency' when IS_EMERGENCY_ORDER =0 then 'Normal'end as PRIORITY, WQ.UNIQUE_ID AS UNIQUE_ID, WQ.FAX_ID,WQ.IS_EMERGENCY_ORDER,              
      ISNULL(dt.NAME, '') AS DOCUMENT_TYPE, dt.DOCUMENT_TYPE_ID,          
      CASE              
       WHEN WQ.INDEXED_BY IS NOT NULL AND (WQ.WORK_STATUS = 'INDEXED' OR WQ.WORK_STATUS = 'COMPLETED') THEN 'INDEXED'              
       WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS = 'INDEX PENDING' THEN 'INDEX PENDING'              
       WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS != 'INDEX PENDING' THEN 'IN PROGRESS'              
       ELSE ''              
      END AS INDEXING_STATUS,              
      (IB.LAST_NAME + ', ' + IB.FIRST_NAME) AS INDEXED_BY,          
   WQ.INDEXED_DATE AS INDEXED_DATE,          
          
   ((convert(varchar, WQ.INDEXED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.INDEXED_DATE as time), 0))) AS Indexed_Date_Str,          
             
    WQ.SORCE_TYPE AS SOURCE_TYPE,           
              
    WQ.RECEIVE_DATE,              
          
   ((convert(varchar, WQ.RECEIVE_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.RECEIVE_DATE as time), 0))) AS Received_Date_Str,           
          
      (AT.FIRST_NAME + ', ' + AT.LAST_NAME) AS ASSIGNED_TO, TR.ROLE_NAME, (CB.LAST_NAME + ', ' + CB.FIRST_NAME) AS COMPLETED_BY,              
      CASE              
       WHEN WQ.INDEXED_BY IS NULL AND (WORK_STATUS = 'CREATED' OR UPPER(ISNULL(WORK_STATUS,'INDEX PENDING')) = 'INDEX PENDING') THEN 'PENDING'              
       WHEN WQ.INDEXED_BY IS NOT NULL AND WQ.WORK_STATUS = 'COMPLETED' THEN 'COMPLETED'              
       ELSE ''              
      END AS WORK_STATUS,              
      WQ.COMPLETED_DATE AS COMPLETED_DATE,          
              
    ((convert(varchar, WQ.COMPLETED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.COMPLETED_DATE as time), 0))) AS Completed_Date_Str          
                  
    FROM FOX_TBL_WORK_QUEUE WQ              
 left join fox_tbl_document_type dt on dt.DOCUMENT_TYPE_ID=wq.DOCUMENT_TYPE            
      LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME              
      LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME              
      LEFT JOIN FOX_TBL_APPLICATION_USER IB ON WQ.ASSIGNED_BY = IB.USER_NAME              
      LEFT JOIN FOX_TBL_ROLE TR ON AT.ROLE_ID = TR.ROLE_ID              
    WHERE ISNULL(wq.DELETED, 0) = 0 AND wq.PRACTICE_CODE = @PRACTICE_CODE and wq.IS_EMERGENCY_ORDER = 0              
 and ISNULL(dt.DELETED, 0) = 0            
 and ISNULL(at.DELETED, 0) = 0            
 and ISNULL(cb.DELETED, 0) = 0            
 and ISNULL(ib.DELETED, 0) = 0            
 and ISNULL(tr.DELETED, 0) = 0            
            
   ) TEMP2              
              
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1              
              
 DECLARE @START_FROM2 INT = @CURRENT_PAGE * @RECORD_PER_PAGE              
 DECLARE @TOATL_PAGESUDM2 FLOAT              
              
 SELECT @TOATL_PAGESUDM2 = COUNT(*)              
 FROM #TEMP_TABLE_REFERRAL_REPORT2              
 WHERE (              
    WORK_ID LIKE @SEARCH_TEXT + '%'              
    OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'              
    OR INDEXED_BY LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
    OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'             
    OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
    OR ROLE_NAME LIKE @SEARCH_TEXT + '%'              
    OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'              
    OR WORK_STATUS LIKE @SEARCH_TEXT + '%'              
    OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%' 
   )              
   AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'              
   AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'              
   AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'              
   AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )              
   AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )              
   AND WORK_STATUS LIKE '%' + @STATUS + '%'              
   AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'              
              
 IF (@RECORD_PER_PAGE = 0)              
 BEGIN              
  SET @RECORD_PER_PAGE = @TOATL_PAGESUDM2              
 END              
 ELSE              
 BEGIN              
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE              
 END              
              
 DECLARE @TOTAL_RECORDS2 INT = @TOATL_PAGESUDM2              
              
 SET @TOATL_PAGESUDM2 = CEILING(@TOATL_PAGESUDM2 / @RECORD_PER_PAGE)              
              
 SELECT *, @TOATL_PAGESUDM2 AS TOTAL_ROCORD_PAGES, @TOTAL_RECORDS2 TOTAL_RECORDS              
 FROM              
  (              
   SELECT WORK_ID ,PRIORITY,UNIQUE_ID, FAX_ID,IS_EMERGENCY_ORDER, DOCUMENT_TYPE, INDEXING_STATUS, INDEXED_BY ,INDEXED_DATE, Indexed_Date_Str, SOURCE_TYPE, RECEIVE_DATE, Received_Date_Str, ROLE_NAME,              
     ASSIGNED_TO, WORK_STATUS, COMPLETED_BY, COMPLETED_DATE, Completed_Date_Str             
   FROM #TEMP_TABLE_REFERRAL_REPORT2              
   WHERE (              
         WORK_ID LIKE @SEARCH_TEXT + '%'              
      OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'              
      OR INDEXED_BY LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
      OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
      OR ROLE_NAME LIKE @SEARCH_TEXT + '%'              
      OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'              
      OR WORK_STATUS LIKE @SEARCH_TEXT + '%'              
      OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
     )              
     AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'              
     AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'              
     AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'              
     AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )              
     AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )              
     AND WORK_STATUS LIKE '%' + @STATUS + '%'              
     AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'              
  ) AS REPORT_REFERRAL_LIST                  
                   
 ORDER BY                       
  case when @SORT_BY = 'WORK_ID' and @SORT_ORDER = 'ASC' then WORK_ID end ASC,                      
  case when @SORT_BY = 'WORK_ID'  and @SORT_ORDER = 'DESC' then WORK_ID end DESC,                      
  case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'ASC' then UNIQUE_ID end ASC,                      
  case when @SORT_BY = 'UNIQUE_ID'  and @SORT_ORDER = 'DESC' then UNIQUE_ID  end DESC,               
  case when @SORT_BY = 'DOCUMENT_TYPE' and @SORT_ORDER = 'ASC' then DOCUMENT_TYPE end ASC,                      
  case when @SORT_BY = 'DOCUMENT_TYPE'  and @SORT_ORDER = 'DESC' then DOCUMENT_TYPE end DESC,                      
  case when @SORT_BY = 'INDEXED_BY' and @SORT_ORDER = 'ASC' then INDEXED_BY end ASC,                      
  case when @SORT_BY = 'INDEXED_BY'  and @SORT_ORDER = 'DESC' then INDEXED_BY  end DESC,              
  case when @SORT_BY = 'INDEXED_DATE' and @SORT_ORDER = 'ASC' then INDEXED_DATE end ASC,                      
  case when @SORT_BY = 'INDEXED_DATE'  and @SORT_ORDER = 'DESC' then INDEXED_DATE  end DESC,              
  case when @SORT_BY = 'SOURCE_TYPE' and @SORT_ORDER = 'ASC' then SOURCE_TYPE end ASC,                      
  case when @SORT_BY = 'SOURCE_TYPE'  and @SORT_ORDER = 'DESC' then SOURCE_TYPE end DESC,              
  case when @SORT_BY = 'ROLE_NAME' and @SORT_ORDER = 'ASC' then ROLE_NAME end ASC,                      
  case when @SORT_BY = 'ROLE_NAME'  and @SORT_ORDER = 'DESC' then ROLE_NAME  end DESC,                      
  case when @SORT_BY = 'ASSIGNED_TO' and @SORT_ORDER = 'ASC' then ASSIGNED_TO end ASC,                      
  case when @SORT_BY = 'ASSIGNED_TO'  and @SORT_ORDER = 'DESC' then ASSIGNED_TO  end DESC,              
  case when @SORT_BY = 'RECEIVE_DATE' and @SORT_ORDER = 'ASC' then RECEIVE_DATE end ASC,                      
  case when @SORT_BY = 'RECEIVE_DATE'  and @SORT_ORDER = 'DESC' then RECEIVE_DATE  end DESC,              
  case when @SORT_BY = 'WORK_STATUS' and @SORT_ORDER = 'ASC' then WORK_STATUS end ASC,                      
  case when @SORT_BY = 'WORK_STATUS'  and @SORT_ORDER = 'DESC' then WORK_STATUS end DESC,              
  case when @SORT_BY = 'COMPLETED_BY' and @SORT_ORDER = 'ASC' then COMPLETED_BY end ASC,                      
  case when @SORT_BY = 'COMPLETED_BY'  and @SORT_ORDER = 'DESC' then COMPLETED_BY end DESC,              
  case when @SORT_BY = 'COMPLETED_DATE' and @SORT_ORDER = 'ASC' then COMPLETED_DATE end ASC,                      
  case when @SORT_BY = 'COMPLETED_DATE'  and @SORT_ORDER = 'DESC' then COMPLETED_DATE end DESC,              
  case when @SORT_BY = 'PRIORITY' and @SORT_ORDER = 'ASC' then PRIORITY end ASC,                      
  case when @SORT_BY = 'PRIORITY'  and @SORT_ORDER = 'DESC' then PRIORITY end DESC                
                       
 OFFSET @START_FROM2 ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY              
end              
---WHEN PRIORITY IS NONE              
If(@PRIORITY = '')              
begin              
 IF OBJECT_ID('TEMPDB..#TEMP_TABLE_REFERRAL_REPORT') IS NOT NULL              
  DROP TABLE #TEMP_TABLE_REFERRAL_REPORT              
              
 SELECT TEMP.*              
   INTO #TEMP_TABLE_REFERRAL_REPORT              
 FROM (              
    SELECT WQ.WORK_ID AS WORK_ID,case when IS_EMERGENCY_ORDER =1 then  'Emergency' when IS_EMERGENCY_ORDER =0 then 'Normal'end as PRIORITY, WQ.UNIQUE_ID AS UNIQUE_ID, WQ.FAX_ID,WQ.IS_EMERGENCY_ORDER,              
  ISNULL(dt.NAME, '') AS DOCUMENT_TYPE, dt.DOCUMENT_TYPE_ID,            
      --CASE              
      -- WHEN ISNULL(WQ.DOCUMENT_TYPE, 0) = 1 THEN 'POC'              
      -- WHEN ISNULL(WQ.DOCUMENT_TYPE, 0) = 2 THEN 'Referral Order'              
      -- WHEN ISNULL(WQ.DOCUMENT_TYPE, 0) = 3 THEN 'Other'              
      -- ELSE ''              
      --END AS DOCUMENT_TYPE,              
      CASE              
       WHEN WQ.INDEXED_BY IS NOT NULL AND (WQ.WORK_STATUS = 'INDEXED' OR WQ.WORK_STATUS = 'COMPLETED') THEN 'INDEXED'              
       WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS = 'INDEX PENDING' THEN 'INDEX PENDING'              
       WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS != 'INDEX PENDING' THEN 'IN PROGRESS'              
       ELSE ''              
      END AS INDEXING_STATUS,              
      (IB.LAST_NAME + ', ' + IB.FIRST_NAME) AS INDEXED_BY,           
   WQ.INDEXED_DATE AS INDEXED_DATE,           
   ((convert(varchar, WQ.INDEXED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.INDEXED_DATE as time), 0))) AS Indexed_Date_Str,           
   WQ.SORCE_TYPE AS SOURCE_TYPE,           
   WQ.RECEIVE_DATE,              
   ((convert(varchar, WQ.RECEIVE_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.RECEIVE_DATE as time), 0))) AS Received_Date_Str,           
      (AT.FIRST_NAME + ', ' + AT.LAST_NAME) AS ASSIGNED_TO, TR.ROLE_NAME, (CB.LAST_NAME + ', ' + CB.FIRST_NAME) AS COMPLETED_BY,              
      CASE              
       WHEN WQ.INDEXED_BY IS NULL AND (WORK_STATUS = 'CREATED' OR UPPER(ISNULL(WORK_STATUS,'INDEX PENDING')) = 'INDEX PENDING') THEN 'PENDING'              
       WHEN WQ.INDEXED_BY IS NOT NULL AND WQ.WORK_STATUS = 'COMPLETED' THEN 'COMPLETED'              
       ELSE ''              
      END AS WORK_STATUS,              
       WQ.COMPLETED_DATE AS COMPLETED_DATE,          
    ((convert(varchar, WQ.COMPLETED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.COMPLETED_DATE as time), 0))) AS Completed_Date_Str          
    FROM FOX_TBL_WORK_QUEUE WQ              
   left join fox_tbl_document_type dt on dt.DOCUMENT_TYPE_ID=wq.DOCUMENT_TYPE   and ISNULL(dt.DELETED, 0) = 0            
      LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME       and ISNULL(at.DELETED, 0) = 0            
      LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME      and ISNULL(cb.DELETED, 0) = 0            
      LEFT JOIN FOX_TBL_APPLICATION_USER IB ON WQ.ASSIGNED_BY = IB.USER_NAME       and ISNULL(ib.DELETED, 0) = 0            
      LEFT JOIN FOX_TBL_ROLE TR ON AT.ROLE_ID = TR.ROLE_ID                         and ISNULL(tr.DELETED, 0) = 0            
    WHERE ISNULL(wq.DELETED, 0) = 0 AND wq.PRACTICE_CODE = @PRACTICE_CODE               
   ) TEMP              
    
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1              
              
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE              
 DECLARE @TOATL_PAGESUDM FLOAT              
              
 SELECT @TOATL_PAGESUDM = COUNT(*)              
 FROM #TEMP_TABLE_REFERRAL_REPORT              
 WHERE (              
    WORK_ID LIKE @SEARCH_TEXT + '%'              
    OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'              
    OR INDEXED_BY LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
    OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
    OR ROLE_NAME LIKE @SEARCH_TEXT + '%'              
    OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'              
    OR WORK_STATUS LIKE @SEARCH_TEXT + '%'              
    OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'              
    OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
   )              
   AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'              
   AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'              
   AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'              
   AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )              
   AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )              
   AND WORK_STATUS LIKE '%' + @STATUS + '%'              
   AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'              
              
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
              
 SELECT *, @TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES, @TOTAL_RECORDS TOTAL_RECORDS              
 FROM              
  (              
   SELECT WORK_ID ,PRIORITY,UNIQUE_ID, FAX_ID,IS_EMERGENCY_ORDER, DOCUMENT_TYPE, INDEXING_STATUS, INDEXED_BY ,INDEXED_DATE, Indexed_Date_Str, SOURCE_TYPE, RECEIVE_DATE, Received_Date_Str,          
   ROLE_NAME,           
     ASSIGNED_TO, WORK_STATUS, COMPLETED_BY, COMPLETED_DATE, Completed_Date_Str              
   FROM #TEMP_TABLE_REFERRAL_REPORT              
   WHERE (              
         WORK_ID LIKE @SEARCH_TEXT + '%'              
      OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'              
      OR INDEXED_BY LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
      OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
      OR ROLE_NAME LIKE @SEARCH_TEXT + '%'              
      OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'              
      OR WORK_STATUS LIKE @SEARCH_TEXT + '%'              
      OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'              
      OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'              
     )              
     AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'              
     AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'              
     AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'              
     AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )              
     AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )              
     AND WORK_STATUS LIKE '%' + @STATUS + '%'              
     AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'              
  ) AS REPORT_REFERRAL_LIST                  
                   
 ORDER BY                       
  case when @SORT_BY = 'WORK_ID' and @SORT_ORDER = 'ASC' then WORK_ID end ASC,                      
  case when @SORT_BY = 'WORK_ID'  and @SORT_ORDER = 'DESC' then WORK_ID end DESC,                      
  case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'ASC' then UNIQUE_ID end ASC,                      
  case when @SORT_BY = 'UNIQUE_ID'  and @SORT_ORDER = 'DESC' then UNIQUE_ID  end DESC,               
  case when @SORT_BY = 'DOCUMENT_TYPE' and @SORT_ORDER = 'ASC' then DOCUMENT_TYPE end ASC,                      
  case when @SORT_BY = 'DOCUMENT_TYPE'  and @SORT_ORDER = 'DESC' then DOCUMENT_TYPE end DESC,                      
  case when @SORT_BY = 'INDEXED_BY' and @SORT_ORDER = 'ASC' then INDEXED_BY end ASC,                
  case when @SORT_BY = 'INDEXED_BY'  and @SORT_ORDER = 'DESC' then INDEXED_BY  end DESC,                      
  case when @SORT_BY = 'INDEXED_DATE' and @SORT_ORDER = 'ASC' then INDEXED_DATE end ASC,                      
  case when @SORT_BY = 'INDEXED_DATE'  and @SORT_ORDER = 'DESC' then INDEXED_DATE  end DESC,              
  case when @SORT_BY = 'SOURCE_TYPE' and @SORT_ORDER = 'ASC' then SOURCE_TYPE end ASC,                      
  case when @SORT_BY = 'SOURCE_TYPE'  and @SORT_ORDER = 'DESC' then SOURCE_TYPE end DESC,              
  case when @SORT_BY = 'ROLE_NAME' and @SORT_ORDER = 'ASC' then ROLE_NAME end ASC,                      
  case when @SORT_BY = 'ROLE_NAME'  and @SORT_ORDER = 'DESC' then ROLE_NAME  end DESC,                      
  case when @SORT_BY = 'ASSIGNED_TO' and @SORT_ORDER = 'ASC' then ASSIGNED_TO end ASC,                      
  case when @SORT_BY = 'ASSIGNED_TO'  and @SORT_ORDER = 'DESC' then ASSIGNED_TO  end DESC,              
  case when @SORT_BY = 'RECEIVE_DATE' and @SORT_ORDER = 'ASC' then RECEIVE_DATE end ASC,                      
  case when @SORT_BY = 'RECEIVE_DATE'  and @SORT_ORDER = 'DESC' then RECEIVE_DATE  end DESC,              
  case when @SORT_BY = 'WORK_STATUS' and @SORT_ORDER = 'ASC' then WORK_STATUS end ASC,                      
  case when @SORT_BY = 'WORK_STATUS'  and @SORT_ORDER = 'DESC' then WORK_STATUS end DESC,              
  case when @SORT_BY = 'COMPLETED_BY' and @SORT_ORDER = 'ASC' then COMPLETED_BY end ASC,                      
  case when @SORT_BY = 'COMPLETED_BY'  and @SORT_ORDER = 'DESC' then COMPLETED_BY end DESC,              
  case when @SORT_BY = 'COMPLETED_DATE' and @SORT_ORDER = 'ASC' then COMPLETED_DATE end ASC,                      
  case when @SORT_BY = 'COMPLETED_DATE'  and @SORT_ORDER = 'DESC' then COMPLETED_DATE end DESC,              
  case when @SORT_BY = 'PRIORITY' and @SORT_ORDER = 'ASC' then PRIORITY end ASC,                      
  case when @SORT_BY = 'PRIORITY'  and @SORT_ORDER = 'DESC' then PRIORITY end DESC                
                       
 OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY              
end              
END 

