IF (OBJECT_ID('FOX_PROC_GET_SUPERVISOR_WORK') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SUPERVISOR_WORK  
GO  
---------------------------------------------- SP_HELPTEXT FOX_PROC_GET_SUPERVISOR_WORK --------------------------------   
/*                          
EXEC [DBO].[FOX_PROC_GET_SUPERVISOR_WORK] 1011163,5488608,1,1000,null,null,null,'WORK_STATUS','asc',null,null,null,null,null                           
EXEC [dbo].[FOX_PROC_GET_SUPERVISOR_WORK_ARSLAN] 1011163,'',1,1000,null,null,null,'receivedate','desc',null,null,null,null,null,''                           
*/                            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SUPERVISOR_WORK] --1011163,'',1,1000,null,null,null,'WORK_STATUS','asc',null,null,null,null,null,'true'                              
 (                              
 @PRACTICE_CODE BIGINT                              
 ,@UNIQUE_ID VARCHAR(50)                              
 ,@CURRENT_PAGE INT                              
 ,@RECORD_PER_PAGE INT                              
 ,@SOURCE_TYPE VARCHAR(50)                              
 ,@SOURCE_NAME VARCHAR(50)                              
 ,@SEARCH_TEXT VARCHAR(30)                              
 ,@SORT_BY VARCHAR(50)                              
 ,@SORT_ORDER VARCHAR(20)                              
 ,@INDEXED_BY VARCHAR(100)                              
 ,@SUPERVISORNAME VARCHAR(50)                              
 ,@TRANSFER_REASON VARCHAR(50)                              
 ,@TRANSFER_COMMENTS VARCHAR(50)                              
 ,@STATUS VARCHAR(50)            
 ,@IS_TRASH BIT                            
 )                              
AS                              
BEGIN                    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                   
 DECLARE @IUNIQUE_ID VARCHAR(50);                              
                              
 IF (                              
   @UNIQUE_ID = ''                              
   OR @UNIQUE_ID = 0                              
   )                              
 BEGIN                              
  SET @IUNIQUE_ID = NULL                              
 END                              
 ELSE                              
 BEGIN                              
  SET @IUNIQUE_ID = convert(VARCHAR(5), @UNIQUE_ID);                              
 END                              
                              
 IF (@TRANSFER_COMMENTS = '')                              
 BEGIN                              
  SET @TRANSFER_COMMENTS = NULL                              
 END                              
                              
 IF (@TRANSFER_REASON = '')                              
 BEGIN                              
  SET @TRANSFER_REASON = NULL                              
 END                              
                              
 IF (                              
   @PRACTICE_CODE = ''                              
   OR @PRACTICE_CODE = 0                              
   )                              
 BEGIN                              
  SET @PRACTICE_CODE = NULL                              
 END                              
                              
 SET NOCOUNT ON;                              
                              
 IF (@SOURCE_NAME = '')                              
 BEGIN                              
  SET @SOURCE_NAME = NULL                              
 END                              
                              
 IF (@SOURCE_TYPE = '')                              
 BEGIN                              
  SET @SOURCE_TYPE = NULL                              
 END                              
                              
 IF (@INDEXED_BY = '')                              
 BEGIN                              
  SET @INDEXED_BY = NULL                              
 END                              
 ELSE                              
 BEGIN                              
  SET @INDEXED_BY = '%' + @INDEXED_BY + '%'                              
 END                          
                              
 --IF (@SOURCE_TYPE = '')                              
 --BEGIN                              
 -- SET @SOURCE_TYPE = NULL                              
 --END                              
                         
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
 --IF (@SOURCE_NAME = '')                              
 --BEGIN                              
 -- SET @SOURCE_NAME = NULL                              
 --END                              
                          
                          
                        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                              
                              
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                              
 DECLARE @TOATL_PAGESUDM FLOAT                              
                              
 SELECT @TOATL_PAGESUDM = COUNT(*)                              
 FROM FOX_TBL_WORK_QUEUE AS WORK                              
 LEFT JOIN FOX_TBL_WORK_ORDER_ADDTIONAL_INFO WQAI ON WQAI.WORK_ID = WORK.WORK_ID AND WQAI.DELETED = 0                            
 INNER JOIN FOX_TBL_APPLICATION_USER AS FOXUSER ON WORK.ASSIGNED_TO = FOXUSER.USER_NAME                             
 LEFT JOIN FOX_TBL_REFERRAL_SENDER RS ON RS.SENDER = WORK.SORCE_NAME                              
                                         and ISNULL(RS.FOR_STRATEGIC, 0) = 1                              
              AND ISNULL(RS.DELETED, 0) = 0                               
  AND ROLE_ID = 102                         
  AND WORK.DELETED = 0                   
 LEFT JOIN FOX_TBL_APPLICATION_USER AS ab ON WORK.ASSIGNED_BY = ab.USER_NAME                              
  AND WORK.DELETED = 0                              
 LEFT JOIN (                              
  SELECT *                              
   ,ROW_NUMBER() OVER (                              
    PARTITION BY WORK_ID ORDER BY CREATED_DATE DESC                              
    ) AS ROW_NO                              
  FROM FOX_TBL_WORK_TRANSFER                              
  ) AS WORKTRANFER ON WORK.WORK_ID = WORKTRANFER.WORK_ID                              
  AND ROW_NO = 1                              
 WHERE WORK.PRACTICE_CODE = @PRACTICE_CODE  AND WORK.supervisor_status = 1                           
 ------------------                            
 AND ISNULL(WORK.IS_TRASH_REFERRAL, cast (0 as bit)) = @IS_TRASH                           
 ------------------                            
  AND (                              
   @SEARCH_TEXT IS NULL                              
   OR ISNULL(Replace(SORCE_NAME,' ',''), '') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(Replace(SORCE_TYPE,' ',''),'') LIKE '%' + @SEARCH_TEXT + '%'                               
   OR ISNULL(TRANSFER_COMMENTS, '') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(TRANSFER_REASON, '') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(WORK.WORK_ID, '') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(Replace(FOXUSER.FIRST_NAME,' ',''),'') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(Replace(FOXUSER.LAST_NAME,' ',''),'') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(ab.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(ab.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'                              
   OR ISNULL(UNIQUE_ID, '') LIKE '%' + @SEARCH_TEXT + '%'                      
   OR ISNULL(Replace(WORK_STATUS,' ',''), '') LIKE '%' + @SEARCH_TEXT + '%'                      
   OR convert(VARCHAR, WORK.ASSIGNED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                              
   OR convert(VARCHAR, WORK.ASSIGNED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'                              
   OR WORK.TOTAL_PAGES LIKE '%' + @SEARCH_TEXT + '%'                              
   OR convert(VARCHAR, WORK.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                              
   OR convert(VARCHAR, WORK.RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'                              
   OR (Convert(VARCHAR, DateDiff(HOUR, ASSIGNED_DATE, GETDATE())) + ':' + Convert(VARCHAR, DateDiff(MINUTE, ASSIGNED_DATE, GETDATE()) % 60)) LIKE '%' + @SEARCH_TEXT + '%'                              
   )                             
  AND upper(WORK_STATUS) <> 'COMPLETED'                              
  AND (                              
   @STATUS IS NULL                              
   OR WORK_STATUS = @STATUS                              
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
   @UNIQUE_ID IS NULL                              
   OR ISNULL(WORK.UNIQUE_ID, '') LIKE '%' + @UNIQUE_ID + '%'                
   )                              
  AND (                              
   @TRANSFER_COMMENTS IS NULL                              
   OR ISNULL(TRANSFER_COMMENTS, '') LIKE '%' + @TRANSFER_COMMENTS + '%'                              
   )                              
  AND (                              
   @TRANSFER_REASON IS NULL                              
   OR ISNULL(TRANSFER_REASON, '') LIKE '%' + @TRANSFER_REASON + '%'                            
   )                              
  AND (                              
   (                              
    @SUPERVISORNAME IS NULL                              
    OR ISNULL(FOXUSER.FIRST_NAME, '') LIKE '%' + @SUPERVISORNAME + '%'                              
    )                              
   OR (                              
    @SUPERVISORNAME IS NULL                              
    OR ISNULL(FOXUSER.LAST_NAME, '') LIKE '%' + @SUPERVISORNAME + '%'                              
    )                              
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
  SELECT WORK.WORK_ID           
   ,ISNULL(WORK.IS_TRASH_REFERRAL,0) IS_TRASH_REFERRAL                                                              
   ,WORK.UNIQUE_ID                              
   ,WORKTRANFER.TRANSFER_REASON                              
   ,WORKTRANFER.TRANSFER_COMMENTS                              
   ,WORK.SORCE_TYPE                              
   ,WORK.SORCE_NAME                              
   ,ab.LAST_NAME + ', ' + ab.FIRST_NAME AS ASSIGNED_BY        
   ,WORK.supervisor_status                              
   ,WORK.ASSIGNED_DATE                              
   ,convert(varchar,WORK.ASSIGNED_DATE) AS Assigned_Date_Str                              
   ,WORK.TRANSFER_DATE                              
   ,WORK.TOTAL_PAGES                              
   ,WORK.RECEIVE_DATE                              
   ,convert(varchar,WORK.RECEIVE_DATE) AS Received_Date_Str                              
   ,FOXUSER.LAST_NAME + ', ' + foxuser.FIRST_NAME AS SUPERVISOR_NAME                              
   ,WORK.WORK_STATUS                              
   ,WORK.PRACTICE_CODE              
   ,WORK.MODIFIED_DATE                              
   ,WORK.CREATED_BY                              
   ,WORK.CREATED_DATE                              
   ,                              
   -- Convert(varchar,DateDiff(HOUR, ASSIGNED_DATE, GETDATE()))                              
   (CONVERT(VARCHAR, DATEDIFF(MINUTE, ASSIGNED_DATE, GETDATE()) / 60) + ':' + Convert(VARCHAR, DateDiff(MINUTE, ASSIGNED_DATE, GETDATE()) % 60)) AS ElapseTime                              
   ,ROW_NUMBER() OVER (                              
    ORDER BY WORK.UNIQUE_ID ASC                              
    ) AS ACTIVEROW                              
   ,IS_EMERGENCY_ORDER IS_EMERGENCY                              
   ,CONVERT(BIT, CASE                               
     WHEN ISNULL(IsSigned, 0) = 1                              
      OR WORK.CREATED_BY LIKE 'FOX TEAM'                              
      OR CHARINDEX('_', WORK.UNIQUE_ID) > 0                              
      THEN 0                              
     ELSE 1                              
     END) AS IS_UNSIGNED             
  , CONVERT(BIT,                              
      CASE                               
    WHEN  ISNULL(RS.FOR_STRATEGIC, 0) = 1                              
    THEN 1                              
    ELSE 0                              
    END) AS IS_STRATEGIC                               
  FROM FOX_TBL_WORK_QUEUE AS WORK                             
  LEFT JOIN FOX_TBL_WORK_ORDER_ADDTIONAL_INFO WQAI ON WQAI.WORK_ID = WORK.WORK_ID AND WQAI.DELETED = 0                             
  INNER JOIN FOX_TBL_APPLICATION_USER AS FOXUSER ON WORK.ASSIGNED_TO = FOXUSER.USER_NAME                              
  --LEFT join fox_tbl_patient pt on WORK.PATIENT_ACCOUNT=pt.Patient_Account AND WORK.PRACTICE_CODE = @PRACTICE_CODE                            
  LEFT JOIN FOX_TBL_REFERRAL_SENDER RS ON RS.SENDER = WORK.SORCE_NAME             
                                         and ISNULL(RS.FOR_STRATEGIC, 0) = 1                              
              AND ISNULL(RS.DELETED, 0) = 0                                 
   AND FOXUSER.ROLE_ID = 102                        
   AND WORK.DELETED = 0                              
  LEFT JOIN FOX_TBL_APPLICATION_USER AS ab ON WORK.ASSIGNED_BY = ab.USER_NAME                              
   AND WORK.DELETED = 0                              
  LEFT JOIN (                              
   SELECT *                              
    ,ROW_NUMBER() OVER (                              
     PARTITION BY WORK_ID ORDER BY CREATED_DATE DESC                              
     ) AS ROW_NO                              
   FROM FOX_TBL_WORK_TRANSFER                              
   ) AS WORKTRANFER ON WORK.WORK_ID = WORKTRANFER.WORK_ID                              
   AND ROW_NO = 1                              
  WHERE WORK.PRACTICE_CODE = @PRACTICE_CODE    AND WORK.supervisor_status = 1                            
 ------------------                            
 AND ISNULL(WORK.IS_TRASH_REFERRAL,  cast(0 as bit))= @IS_TRASH                      
 ------------------                            
  --AND ISNULL(WORK.IS_TRASH_REFERRAL,0)= 0                          
 ------------------                            
   AND (                              
    @SEARCH_TEXT IS NULL                              
    OR ISNULL(Replace(SORCE_NAME,' ',''), '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(Replace(SORCE_TYPE,' ',''), '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(TRANSFER_COMMENTS, '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(TRANSFER_REASON, '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(WORK.WORK_ID, '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(Replace(FOXUSER.FIRST_NAME,' ',''),'') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(Replace(FOXUSER.LAST_NAME,' ',''),'') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(ab.FIRST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(ab.LAST_NAME, '') LIKE '%' + @SEARCH_TEXT + '%'                              
    OR ISNULL(UNIQUE_ID, '') LIKE '%' + @SEARCH_TEXT + '%'                       
 OR ISNULL(Replace(WORK_STATUS,' ',''), '') LIKE '%' + @SEARCH_TEXT + '%'                       
    OR convert(VARCHAR, WORK.ASSIGNED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                              
    OR convert(VARCHAR, WORK.ASSIGNED_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'                              
    OR WORK.TOTAL_PAGES LIKE '%' + @SEARCH_TEXT + '%'                            
    OR convert(VARCHAR, WORK.RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                              
    OR convert(VARCHAR, WORK.RECEIVE_DATE, 100) LIKE '%' + @SEARCH_TEXT + '%'                              
    OR (Convert(VARCHAR, DateDiff(HOUR, ASSIGNED_DATE, GETDATE())) + ':' + Convert(VARCHAR, DateDiff(MINUTE, ASSIGNED_DATE, GETDATE()) % 60)) LIKE '%' + @SEARCH_TEXT + '%'                              
    )                              
   AND upper(WORK_STATUS) <> 'COMPLETED'                              
   AND (                              
    @STATUS IS NULL                              
    OR WORK_STATUS = @STATUS                              
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
    @UNIQUE_ID IS NULL                              
    OR ISNULL(WORK.UNIQUE_ID, '') LIKE '%' + @UNIQUE_ID + '%'                              
    )                              
   AND (                              
    @TRANSFER_COMMENTS IS NULL                              
    OR ISNULL(TRANSFER_COMMENTS, '') LIKE '%' + @TRANSFER_COMMENTS + '%'                              
    )                              
   AND (                              
    @TRANSFER_REASON IS NULL                              
    OR ISNULL(TRANSFER_REASON, '') LIKE '%' + @TRANSFER_REASON + '%'                              
    )                              
   AND (                              
    (                              
     @SUPERVISORNAME IS NULL                              
  OR ISNULL(FOXUSER.FIRST_NAME, '') LIKE '%' + @SUPERVISORNAME + '%'                              
     )                              
    OR (                              
     @SUPERVISORNAME IS NULL                              
     OR ISNULL(FOXUSER.LAST_NAME, '') LIKE '%' + @SUPERVISORNAME + '%'                              
     )                              
    )                              
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
   WHEN @SORT_BY = 'SupervisorName'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN SUPERVISOR_NAME                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'SupervisorName'                              
    AND @SORT_ORDER = 'DESC'                              
    THEN SUPERVISOR_NAME                              
   END DESC                              
  ,CASE                               
   WHEN @SORT_BY = 'TransferReason'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN TRANSFER_REASON                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'TransferReason'                              
    AND @SORT_ORDER = 'DESC'                              
    THEN TRANSFER_REASON                              
   END DESC                              
  ,CASE                               
   WHEN @SORT_BY = 'WORK_STATUS'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN WORK_STATUS                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'WORK_STATUS'                              
    AND @SORT_ORDER = 'DESC'                              
    THEN WORK_STATUS                              
   END DESC                              
  ,                              
  case when @SORT_BY = 'TransferDate' and  @SORT_ORDER = 'ASC' then   ASSIGNED_DATE  end ASC,                                   
  case when @SORT_BY = 'TransferDate'  and @SORT_ORDER = 'DESC' then  ASSIGNED_DATE  end DESC,                                  
  CASE                               
   WHEN @SORT_BY = 'assignedby'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN ASSIGNED_BY                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'assignedby'                              
    AND @SORT_ORDER = 'DESC'                              
    THEN ASSIGNED_BY                              
   END DESC                      
  ,CASE                               
   WHEN @SORT_BY = 'assigneddate'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN ASSIGNED_DATE                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'assigneddate'                              
    AND @SORT_ORDER = 'DESC'                              
    THEN ASSIGNED_DATE                              
   END DESC                              
  ,CASE                               
   WHEN @SORT_BY = 'pages'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN TOTAL_PAGES                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'pages'                              
   AND @SORT_ORDER = 'DESC'                              
   THEN TOTAL_PAGES                              
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
   WHEN @SORT_BY = 'elapsetime'                              
    AND @SORT_ORDER = 'ASC'                              
    THEN DateDiff(MINUTE, ASSIGNED_DATE, GETDATE())                              
   END ASC                              
  ,CASE                               
   WHEN @SORT_BY = 'elapsetime'                              
   AND @SORT_ORDER = 'DESC'                              
    THEN DateDiff(MINUTE, ASSIGNED_DATE, GETDATE())                              
   END DESC OFFSET @START_FROM ROWS                              
                              
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                              
END 