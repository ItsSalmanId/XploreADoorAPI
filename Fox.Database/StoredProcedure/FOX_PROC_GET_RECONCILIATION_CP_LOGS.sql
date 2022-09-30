IF (OBJECT_ID('FOX_PROC_GET_RECONCILIATION_CP_LOGS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECONCILIATION_CP_LOGS  
GO   
  --FOX_PROC_GET_RECONCILIATION_CP_LOGS 1011163,5487587,'',1,10,1,0             
CREATE PROCEDURE [dbo].[FOX_PROC_GET_RECONCILIATION_CP_LOGS]              
(@PRACTICE_CODE        BIGINT,                 
 @RECONCILIATION_CP_ID BIGINT,                 
 @SEARCH_STRING        VARCHAR(50),                 
 @CURRENT_PAGE         INT,                 
 @RECORD_PER_PAGE      INT,              
 @LOG_DETAIL           BIT,              
 @REMARK_DETAIL        BIT              
)                
WITH RECOMPILE                
AS                
    ---------------------------------------------                
    --DECLARE                 
    -- @PRACTICE_CODE        BIGINT = 1011163,                 
    -- @RECONCILIATION_CP_ID BIGINT = 544100,                 
    -- @SEARCH_STRING        VARCHAR(50) = '',                
    -- @CURRENT_PAGE         INT = 1,                 
    -- @RECORD_PER_PAGE      INT = 10                
    --------------------------------------------                
     BEGIN                
         IF(@SEARCH_STRING = '')                
             BEGIN                
                 SET @SEARCH_STRING = NULL;                
             END;                
             ELSE                
             BEGIN                
                 SET @SEARCH_STRING = @SEARCH_STRING+'%';                
             END;                
         IF(@RECORD_PER_PAGE = 0)                
             BEGIN                
                 SELECT @RECORD_PER_PAGE = COUNT(*)                
                 FROM FOX_TBL_RECONCILIATION_CP_LOGS;                
             END;                
             ELSE                
             BEGIN                
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                
             END;                
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                
         DECLARE @TOATL_PAGESUDM FLOAT;                
         SELECT @TOATL_PAGESUDM = COUNT(*)                
         FROM dbo.FOX_TBL_RECONCILIATION_CP_LOGS ftrcl                
              INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON ftrcl.CREATED_BY = ftau.USER_NAME                
                                                              AND ftau.IS_ACTIVE = 1                
                                                              AND ftau.DELETED = 0                
         WHERE(ftrcl.RECONCILIATION_CP_ID = @RECONCILIATION_CP_ID)                
              AND (ftrcl.PRACTICE_CODE = @PRACTICE_CODE)                
              AND (@SEARCH_STRING IS NULL                
                   OR (ftrcl.CREATED_BY LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftrcl.LOG_MESSAGE LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftrcl.CREATED_DATE LIKE '%'+@SEARCH_STRING+'%'                
                       OR CONVERT(VARCHAR(50), isnull(ftrcl.CREATED_DATE, 0), 101) LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftau.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftau.Last_Name LIKE+'%'+@SEARCH_STRING+'%'))                
              AND ftrcl.DELETED = 0              
     AND(@REMARK_DETAIL=0 OR (@REMARK_DETAIL=1 AND ftrcl.FIELD_NAME LIKE 'REMARKS'))              
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                
                      
  SELECT *,                 
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                 
                @TOTAL_RECORDS TOTAL_RECORDS                
         FROM                
         (                
             SELECT ftrcl.RECONCILIATION_LOG_ID,            
     ftrcl.MODIFIED_DATE,            
     ftrcl.MODIFIED_BY,     
                    ftrcl.PRACTICE_CODE,                 
                    ftrcl.RECONCILIATION_CP_ID,                 
         ftrcl.LOG_MESSAGE,                 
                    ISNULL(ftrcl.PREVIOUS_VALUE, '') PREVIOUS_VALUE,                 
                 ISNULL(ftrcl.NEW_VALUE, '') NEW_VALUE,               
     ftrcl.FIELD_NAME,              
     ftau.USER_NAME,              
     (select ROLE_NAME from Fox_tbl_role RL               
                          Left Join fox_tbl_application_user US on US.ROLE_ID=RL.ROLE_ID where USER_NAME=ftau.USER_NAME) AS ROLE_NAME,                     
                    ftrcl.CREATED_BY,                 
                    ftrcl.CREATED_DATE,                 
                    ftrcl.DELETED,                
            CASE                
                        WHEN ftau.FIRST_NAME IS NULL                
                        THEN ''                
                        ELSE ftau.FIRST_NAME+' '+ftau.LAST_NAME                
                    END AS CREATED_BY_NAME                
             FROM dbo.FOX_TBL_RECONCILIATION_CP_LOGS ftrcl                
              INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON ftrcl.CREATED_BY = ftau.USER_NAME                
                                                              AND ftau.IS_ACTIVE = 1                
                                                              AND ftau.DELETED = 0                
         WHERE(ftrcl.RECONCILIATION_CP_ID = @RECONCILIATION_CP_ID)                
              AND (ftrcl.PRACTICE_CODE = @PRACTICE_CODE)                
              AND (@SEARCH_STRING IS NULL                
                   OR (ftrcl.CREATED_BY LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftrcl.LOG_MESSAGE LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftrcl.CREATED_DATE LIKE '%'+@SEARCH_STRING+'%'                
              OR CONVERT(VARCHAR(50), isnull(ftrcl.CREATED_DATE, 0), 101) LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftau.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'                
                       OR ftau.Last_Name LIKE+'%'+@SEARCH_STRING+'%'))                
              AND ftrcl.DELETED = 0                
     AND(@REMARK_DETAIL=0 OR (@REMARK_DETAIL=1 AND ftrcl.FIELD_NAME LIKE 'REMARKS'))              
                 
         ) AS RECONCILIATION_CP_LOG                
         ORDER BY RECONCILIATION_CP_LOG.CREATED_DATE ASC                
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                
     END; 