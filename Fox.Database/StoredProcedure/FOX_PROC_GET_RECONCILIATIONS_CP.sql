IF (OBJECT_ID('FOX_PROC_GET_RECONCILIATIONS_CP') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECONCILIATIONS_CP  
GO                    
 -- EXEC [dbo].[FOX_PROC_GET_RECONCILIATIONS_CP_TEST] 1012714, 0, 1, 0, '','11/19/2020','','','Admin_5651352','0441342245412', 1,500,'Status','ASC' , 2,''                                    
CREATE PROCEDURE [dbo].[FOX_PROC_GET_RECONCILIATIONS_CP]                                      
(@PRACTICE_CODE           BIGINT,                                         
 @IS_FOR_REPORT           BIT,                                         
 @IS_DEPOSIT_DATE_SEARCH  BIT,                                         
 @IS_ASSIGNED_DATE_SEARCH BIT,                                         
 @DATE_FROM               VARCHAR(15),                                         
 @DATE_TO                 VARCHAR(15),                                         
 @FOX_TBL_INSURANCE_NAME    VARCHAR(100),                                         
 --@CATEGORY_IDS            VARCHAR(MAX),                                         
 @STATUS_ID               VARCHAR(50),                                         
 --@DEPOSIT_TYPE_IDS        VARCHAR(MAX),                                         
 --@CHECK_NOS               VARCHAR(MAX),                                         
 --@AMOUNT                  VARCHAR(50),                                         
 --@AMOUNT_POSTED           VARCHAR(50),                                         
 --@AMOUNT_NOT_POSTED       VARCHAR(50),                                         
 @CURRENT_USER            VARCHAR(225),                                         
 @SEARCH_TEXT             VARCHAR(30),                                         
 @CURRENT_PAGE            INT,                                         
 @RECORD_PER_PAGE         INT,                                     
 @SORT_BY         VARCHAR(50),                                       
 @SORT_ORDER      VARCHAR(5)  ,                                 
 @CP_TYPE  INT,    
 @STATE    VARCHAR(100)    
)                                        
WITH RECOMPILE                                        
AS                                        
    ---------------------------------------------                                        
    --DECLARE                                         
    --@PRACTICE_CODE           BIGINT,                                         
    --@IS_FOR_REPORT           BIT=1,                                        
    --@IS_DEPOSIT_DATE_SEARCH  BIT=0,                                         
    --@IS_ASSIGNED_DATE_SEARCH BIT=0,                                         
    --@DATE_FROM               VARCHAR(15) = '',                                         
    --@DATE_TO                 VARCHAR(15) = '',                                         
    --@FOX_TBL_INSURANCE_ID    VARCHAR(50) = '',                                         
    ----@CATEGORY_IDS      VARCHAR(500) = '',                                          
    --@STATUS_ID      VARCHAR(50) = '',                                          
    ----@DEPOSIT_TYPE_IDS        VARCHAR(500) = '',                                         
    ----@CHECK_NO       VARCHAR(20) = '',                                        
    ----@AMOUNT                  VARCHAR(50) = '',                                         
    ----@AMOUNT_POSTED           VARCHAR(50) = '',                                         
    ----@AMOUNT_NOT_POSTED       VARCHAR(50) = '',                                         
    --@CURRENT_USER            VARCHAR(225) = '1163testing',                                        
    --@SEARCH_TEXT             VARCHAR(30) = '',                                         
    --@CURRENT_PAGE            INT = 1,                                         
    --@RECORD_PER_PAGE         INT = 10                                        
    --------------------------------------------                                        
     BEGIN                                        
  set @CURRENT_PAGE = 1          
         IF(@DATE_FROM = '')                                        
             BEGIN                                   
                 SET @DATE_FROM = NULL;         
             END;                                        
             ELSE                                        
         BEGIN                                        
     SET @DATE_FROM = @DATE_FROM+'%';                 
     END;                                        
         IF(@DATE_TO = '')                                        
      BEGIN                              
                 SET @DATE_TO = NULL;                                        
             END;                                        
             ELSE                                        
             BEGIN                                        
                 SET @DATE_TO = @DATE_TO+'%';                                        
             END;                                     
         IF(@FOX_TBL_INSURANCE_NAME = '')                                        
             BEGIN                                        
                 SET @FOX_TBL_INSURANCE_NAME = NULL;                                        
             END;    
   IF(@STATE = '')                                        
             BEGIN                                        
                 SET @STATE = NULL;                                        
             END;     
         --IF(@CATEGORY_IDS = '')                             
         --    BEGIN                                        
         --        SET @CATEGORY_IDS = NULL;                                        
         --    END;                      
   IF(@STATUS_ID = '')                                        
             BEGIN                                        
                 SET @STATUS_ID = NULL;                                        
             END;                                        
             ELSE                                        
             BEGIN                                        
                 SET @STATUS_ID = @STATUS_ID+'%';                                        
  END;                               
                                  
   DECLARE @CURRENT_USER_ROLE_ID BIGINT;                                      
                                      
   SET @CURRENT_USER_ROLE_ID = (SELECT TOP 1 ROLE_ID FROM FOX_TBL_APPLICATION_USER WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND USER_NAME = @CURRENT_USER)                                  
         IF(@CURRENT_USER = '')                                        
             BEGIN                                        
                 SET @CURRENT_USER = NULL;                                        
             END;                                  
                                      
         IF(@RECORD_PER_PAGE = 0)                                        
             BEGIN                                        
                 SELECT @RECORD_PER_PAGE = COUNT(*)                                        
     FROM dbo.FOX_TBL_RECONCILIATION_CP;                                        
             END;                                        
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                                        
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                                        
         DECLARE @TOATL_PAGESUDM FLOAT;                                   
   DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                                             
   --IF (@CP_TYPE = 0)                                  
    --  BEGIN                                  
      SELECT @TOATL_PAGESUDM = COUNT(*)                                        
      FROM dbo.FOX_TBL_RECONCILIATION_CP ftrcp WITH(NOLOCK)                                        
       -- LEFT JOIN dbo.FOX_TBL_INSURANCE fti ON ftrcp.FOX_TBL_INSURANCE_NAME = fti.INSURANCE_NAME AND fti.PRACTICE_CODE = @PRACTICE_CODE                        
        --LEFT JOIN dbo.FOX_TBL_INSURANCE fti ON ftrcp.FOX_TBL_INSURANCE_ID = fti.INSURANCE_ID   AND fti.PRACTICE_CODE = @PRACTICE_CODE                                     
        LEFT JOIN dbo.FOX_TBL_RECONCILIATION_CATEGORY ftrc ON ftrcp.CATEGORY_ID = ftrc.CATEGORY_ID AND  ftrc.PRACTICE_CODE = @PRACTICE_CODE AND ftrcp.DELETED = 0                                        
        LEFT JOIN dbo.FOX_TBL_RECONCILIATION_STATUS ftrs ON ftrcp.RECONCILIATION_STATUS_ID = ftrs.RECONCILIATION_STATUS_ID  AND  ftrs.PRACTICE_CODE = @PRACTICE_CODE                                       
                     AND ftrcp.DELETED = 0                                        
        LEFT JOIN dbo.FOX_TBL_RECONCILIATION_DEPOSIT_TYPE ftrdt ON ftrcp.DEPOSIT_TYPE_ID = ftrdt.DEPOSIT_TYPE_ID  AND  ftrdt.PRACTICE_CODE = @PRACTICE_CODE                                      
                      AND ftrcp.DELETED = 0                         
        LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON ftrcp.ASSIGNED_TO = ftau.USER_NAME                                        
                   AND ftau.IS_ACTIVE = 1                                        
 AND ftau.DELETED = 0                                        
        LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau1 ON ftrcp.COMPLETED_BY = ftau1.USER_NAME                                        
        AND ftau1.IS_ACTIVE = 1                                        
                    AND ftau1.DELETED = 0                                        
        LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau2 ON ftrcp.CREATED_BY = ftau2.USER_NAME                             
                    AND ftau2.IS_ACTIVE = 1                                        
                    AND ftau2.DELETED = 0                                        
        LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau3 ON @CURRENT_USER = ftau3.USER_NAME                                        
 AND ftau3.IS_ACTIVE = 1                                        
                    AND ftau3.DELETED = 0                                        
      WHERE ftrcp.PRACTICE_CODE = @Practice_Code                                
   AND (@CURRENT_USER_ROLE_ID IN(102, 103)                               
      OR ASSIGNED_TO = @CURRENT_USER)                     
   AND (                    
    (@CP_TYPE = 0)                    
    OR (@CP_TYPE = 1 AND ISNULL(ftrcp.IS_RECONCILIED,0) = 1)                    
    OR (@CP_TYPE = 2 AND ISNULL(ftrcp.IS_RECONCILIED,0) = 0)                    
   )                                        
       --AND (@CURRENT_USER = CASE                                        
       --                              WHEN ftau3.ROLE_ID IN(103, 110, 111) OR @IS_FOR_REPORT = 1                                        
       --                              THEN @CURRENT_USER                                        
       --                              ELSE CASE                                        
       --                          WHEN ftau3.ROLE_ID = 105                                        
       --                                       THEN ftrcp.CREATED_BY                                        
       --                                       WHEN ftau3.ROLE_ID = 112                           
       --                                       THEN ftrcp.ASSIGNED_TO                                        
       --                                   END                                        
       --                          END)                                        
        AND (ftrcp.CHECK_NO LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.AMOUNT LIKE '%'+@SEARCH_TEXT+'%'                                        
     OR ftrcp.AMOUNT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.AMOUNT_NOT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.ASSIGNED_TO LIKE '%'+@SEARCH_TEXT+'%'                                        
        OR ftrcp.COMPLETED_BY LIKE '%'+@SEARCH_TEXT+'%'                        
  OR ftrcp.ASSIGNED_GROUP LIKE '%'+@SEARCH_TEXT+'%'                        
  OR ftrcp.ASSIGNED_GROUP_DATE LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.CREATED_BY LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.CREATED_BY LIKE '%'+@SEARCH_TEXT+'%'         
          OR ftrcp.FOX_TBL_INSURANCE_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrc.CATEGORY_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrs.STATUS_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrdt.DEPOSIT_TYPE_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR CONVERT(VARCHAR, ftrcp.ASSIGNED_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                    
          OR CONVERT(VARCHAR, ftrcp.COMPLETED_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
          OR CONVERT(VARCHAR, ftrcp.DEPOSIT_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
          OR CONVERT(VARCHAR, ftrcp.CREATED_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.AMOUNT LIKE '%'+@SEARCH_TEXT+'%'                                        
OR ftrcp.AMOUNT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftrcp.AMOUNT_NOT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftau.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftau.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftau2.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
          OR ftau2.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                           
          OR ftau3.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'     
    OR ftrcp.STATE LIKE '%'+@SEARCH_TEXT+'%'    
          OR ftau3.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%')                                        
        AND (@DATE_FROM IS NULL                                        
          OR ((CASE                                        
          WHEN @IS_DEPOSIT_DATE_SEARCH = 1                                  
          THEN CAST(ftrcp.DEPOSIT_DATE AS DATE)                                        
          WHEN @IS_ASSIGNED_DATE_SEARCH = 1                                        
          THEN CAST(ftrcp.ASSIGNED_DATE AS DATE)                                        
          ELSE CAST(ftrcp.COMPLETED_DATE AS DATE)                                        
         END) >= CONVERT(DATE, Replace(@DATE_FROM, '%', ''), 101)))                                        
        AND (@DATE_TO IS NULL                                        
          OR ((CASE                                        
          WHEN @IS_DEPOSIT_DATE_SEARCH = 1                                        
          THEN CAST(ftrcp.DEPOSIT_DATE AS DATE)                                        
          WHEN @IS_ASSIGNED_DATE_SEARCH = 1                                        
          THEN CAST(ftrcp.ASSIGNED_DATE AS DATE)                                        
          ELSE CAST(ftrcp.COMPLETED_DATE AS DATE)                                        
         END) <= CONVERT(DATE, Replace(@DATE_TO, '%', ''), 101)))                                        
        AND (@FOX_TBL_INSURANCE_NAME IS NULL                                        
          OR ftrcp.FOX_TBL_INSURANCE_NAME LIKE '%'+@FOX_TBL_INSURANCE_NAME+'%')     
     AND (@STATE IS NULL                                        
          OR ftrcp.STATE LIKE '%'+@STATE+'%')       
        AND (@STATUS_ID IS NULL                                        
          OR ftrcp.RECONCILIATION_STATUS_ID LIKE '%'+@STATUS_ID+'%')                                        
                                         
      AND ftrcp.DELETED = 0;                            
                             
   IF(@RECORD_PER_PAGE IS NULL OR @RECORD_PER_PAGE = 0)                          
   BEGIN         
  SET @RECORD_PER_PAGE = 1                          
   END                                      
                                        
      SET @TOATL_PAGESUDM = CEILING(IsNull(@TOATL_PAGESUDM,0) / IsNull(@RECORD_PER_PAGE,1));                                    
      SET @TOTAL_RECORDS = @TOATL_PAGESUDM      
      SELECT *,                                         
    ROW_NUMBER() over(order by RECONCILIATIONS.CREATED_DATE desc) as RowId,                                   
       @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                         
       @TOTAL_RECORDS TOTAL_RECORDS                                        
      FROM                                        
      (                                        
       SELECT ftrcp.RECONCILIATION_CP_ID,                                         
        ftrcp.PRACTICE_CODE,                                        
        isnull(ftrs.STATUS_NAME, '') AS STATUS_NAME,                                         
        ftrcp.DEPOSIT_DATE,                                         
        ftrcp.DEPOSIT_TYPE_ID,                                         
        ftrcp.CATEGORY_ID,                                         
        ftrcp.FOX_TBL_INSURANCE_NAME,                                                 
  ftrcp.CHECK_NO,                                         
        ftrcp.AMOUNT,                                         
        ftrcp.AMOUNT_POSTED,                                         
        ftrcp.AMOUNT_NOT_POSTED,                                         
        ftrcp.RECONCILIATION_STATUS_ID,                                         
        ftrcp.ASSIGNED_TO,                                         
        ftrcp.ASSIGNED_DATE,                                         
        ftrcp.COMPLETED_DATE,                                         
        ftrcp.COMPLETED_BY,                                         
        ftrcp.CREATED_BY,                                         
        ftrcp.CREATED_DATE,                                         
        ftrcp.MODIFIED_BY,                                         
        ftrcp.MODIFIED_DATE,                             
        ftrcp.DELETED,                                  
        ISNULL(ftrcp.IS_RECONCILIED,0) AS IS_RECONCILIED,                                  
        isnull(REASON.REASON,'') [REASON_NAME],                                  
        isnull(ftrcp.REASON,0) [REASON],                                  
        isnull(ftrcp.REMARKS,'') REMARKS,                                  
        isnull(ftrcp.BATCH_NO,'') BATCH_NO,                                  
        ftrcp.date_posted DATE_POSTED,                  
  --isnull(ftrcp.date_posted,getdate()) DATE_POSTED,             
        ISNULL(ftrcp.LEDGER_NAME, '') AS LEDGER_NAME,                                        
        ISNULL(ftrcp.LEDGER_PATH, '') AS LEDGER_PATH,                                        
        --ISNULL(ftrcp.LEDGER_BASE64, '') AS LEDGER_BASE64,                                        
        ISNULL(ftrcp.TOTAL_LEDGER_PAGES, 0) AS TOTAL_LEDGER_PAGES,                                        
                             
        ----------------------------------------------                                        
        --Not Mapped Fields                                        
        ----------------------------------------------                                        
        isnull(ftrcp.FOX_TBL_INSURANCE_NAME, '') AS INSURANCE_NAME,                                         
        isnull(ftrc.CATEGORY_NAME, '') AS CATEGORY_NAME,                                                              
        isnull(ftrdt.DEPOSIT_TYPE_NAME, '') AS DEPOSIT_TYPE_NAME,                                        
  CASE                                        
         WHEN ftau.FIRST_NAME IS NULL                                        
         THEN ''                                        
         ELSE ftau.LAST_NAME+', '+ftau.FIRST_NAME   
        END AS ASSIGNED_TO_NAME,                                        
        CASE                                        
         WHEN ftau1.FIRST_NAME IS NULL                                        
         THEN ''                                        
         ELSE ftau1.LAST_NAME+', '+ftau1.FIRST_NAME                                        
        END AS COMPLETED_BY_NAME,                                        
     CASE                                        
         WHEN ftau2.FIRST_NAME IS NULL                                        
         THEN ''                                        
         ELSE ftau2.LAST_NAME+', '+ftau2.FIRST_NAME                                        
        END AS CREATED_BY_NAME,                        
    ASSIGNED_GROUP,                        
    ASSIGNED_GROUP_DATE,    
 ftrcp.[STATE]    
       FROM dbo.FOX_TBL_RECONCILIATION_CP ftrcp WITH(NOLOCK)                                        
        -- LEFT JOIN dbo.FOX_TBL_INSURANCE fti ON ftrcp.FOX_TBL_INSURANCE_NAME = fti.INSURANCE_NAME AND fti.PRACTICE_CODE = @PRACTICE_CODE                                       
      --LEFT JOIN dbo.FOX_TBL_INSURANCE fti ON ftrcp.FOX_TBL_INSURANCE_ID = fti.INSURANCE_ID AND fti.PRACTICE_CODE = @PRACTICE_CODE                                           
         LEFT JOIN dbo.FOX_TBL_RECONCILIATION_CATEGORY ftrc ON ftrcp.CATEGORY_ID = ftrc.CATEGORY_ID  AND ftrc.PRACTICE_CODE = @PRACTICE_CODE                                       
                     AND ftrcp.DELETED = 0                                        
         LEFT JOIN dbo.FOX_TBL_RECONCILIATION_STATUS ftrs ON ftrcp.RECONCILIATION_STATUS_ID = ftrs.RECONCILIATION_STATUS_ID AND ftrs.PRACTICE_CODE = @PRACTICE_CODE                             
                      AND ftrcp.DELETED = 0                                        
         LEFT JOIN dbo.FOX_TBL_RECONCILIATION_DEPOSIT_TYPE ftrdt ON ftrcp.DEPOSIT_TYPE_ID = ftrdt.DEPOSIT_TYPE_ID AND ftrdt.PRACTICE_CODE = @PRACTICE_CODE                          
                       AND ftrcp.DELETED = 0                                   
        LEFT JOIN dbo.FOX_TBL_RECONCILIATION_REASON REASON ON REASON.FOX_TBL_RECONCILIATION_REASON_ID = ftrcp.REASON AND REASON.PRACTICE_CODE = @PRACTICE_CODE                                    
                       AND ftrcp.DELETED = 0                                       
         LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON ftrcp.ASSIGNED_TO = ftau.USER_NAME                                        
               AND ftau.IS_ACTIVE = 1                                        
                    AND ftau.DELETED = 0                                        
         LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau1 ON ftrcp.COMPLETED_BY = ftau1.USER_NAME                                        
                     AND ftau1.IS_ACTIVE = 1                                        
                     AND ftau1.DELETED = 0                                        
         LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau2 ON ftrcp.CREATED_BY = ftau2.USER_NAME                                        
                     AND ftau2.IS_ACTIVE = 1                                        
                     AND ftau2.DELETED = 0                                        
         LEFT JOIN dbo.FOX_TBL_APPLICATION_USER ftau3 ON @CURRENT_USER = ftau3.USER_NAME                              
                     AND ftau3.IS_ACTIVE = 1                                        
                    AND ftau3.DELETED = 0                                    
                                                        
       WHERE ftrcp.PRACTICE_CODE = @Practice_Code                                
    AND (@CURRENT_USER_ROLE_ID IN(102, 103)                               
      OR ASSIGNED_TO = @CURRENT_USER)                      
   AND (                    
    (@CP_TYPE = 0)                    
    OR (@CP_TYPE = 1 AND ISNULL(ftrcp.IS_RECONCILIED,0) = 1)                    
    OR (@CP_TYPE = 2 AND ISNULL(ftrcp.IS_RECONCILIED,0) = 0)                    
   )                                     
         --AND (@CURRENT_USER = CASE                                        
         --                         WHEN ftau3.ROLE_ID IN(103, 110, 111) OR @IS_FOR_REPORT = 1                                        
         --                         THEN @CURRENT_USER                                        
         --                         ELSE CASE                                        
         --                             WHEN ftau3.ROLE_ID = 105                                        
         --                                  THEN ftrcp.CREATED_BY                                        
         --                                  WHEN ftau3.ROLE_ID = 112                                  
         --                                  THEN ftrcp.ASSIGNED_TO                                        
         --                              END                                        
         --            END)                                        
         AND (ftrcp.CHECK_NO LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.AMOUNT LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.AMOUNT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.AMOUNT_NOT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                
           OR ftrcp.ASSIGNED_TO LIKE '%'+@SEARCH_TEXT+'%'                                      
           OR ftrcp.COMPLETED_BY LIKE '%'+@SEARCH_TEXT+'%'                         
     OR ftrcp.ASSIGNED_GROUP LIKE '%'+@SEARCH_TEXT+'%'                        
     OR ftrcp.ASSIGNED_GROUP_DATE LIKE '%'+@SEARCH_TEXT+'%'                                       
           OR ftrcp.CREATED_BY LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.CREATED_BY LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.FOX_TBL_INSURANCE_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrc.CATEGORY_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrs.STATUS_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrdt.DEPOSIT_TYPE_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR CONVERT(VARCHAR, ftrcp.ASSIGNED_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
         OR CONVERT(VARCHAR, ftrcp.COMPLETED_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
           OR CONVERT(VARCHAR, ftrcp.DEPOSIT_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
           OR CONVERT(VARCHAR, ftrcp.CREATED_DATE, 101) LIKE+'%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.AMOUNT LIKE '%'+@SEARCH_TEXT+'%'                            
           OR ftrcp.AMOUNT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftrcp.AMOUNT_NOT_POSTED LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftau.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftau.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                 
           OR ftau2.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
    OR ftau2.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                                        
           OR ftau3.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'    
     OR ftrcp.STATE LIKE '%'+@SEARCH_TEXT+'%'     
           OR ftau3.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%')                                        
         AND (@DATE_FROM IS NULL                                        
           OR ((CASE                                        
           WHEN @IS_DEPOSIT_DATE_SEARCH = 1                                        
           THEN CAST(ftrcp.DEPOSIT_DATE AS DATE)                                        
           WHEN @IS_ASSIGNED_DATE_SEARCH = 1                                        
           THEN CAST(ftrcp.ASSIGNED_DATE AS DATE)                                        
           ELSE CAST(ftrcp.COMPLETED_DATE AS DATE)                                        
          END) >= CONVERT(DATE, Replace(@DATE_FROM, '%', ''), 101)))                                        
         AND (@DATE_TO IS NULL                                        
           OR ((CASE                                        
           WHEN @IS_DEPOSIT_DATE_SEARCH = 1                                        
           THEN CAST(ftrcp.DEPOSIT_DATE AS DATE)                                        
           WHEN @IS_ASSIGNED_DATE_SEARCH = 1                                    
  THEN CAST(ftrcp.ASSIGNED_DATE AS DATE)                                        
           ELSE CAST(ftrcp.COMPLETED_DATE AS DATE)                                        
          END) <= CONVERT(DATE, Replace(@DATE_TO, '%', ''), 101)))                                        
         AND (@FOX_TBL_INSURANCE_NAME IS NULL                                        
           OR ftrcp.FOX_TBL_INSURANCE_NAME LIKE '%'+@FOX_TBL_INSURANCE_NAME+'%')        
     AND (@STATE IS NULL                                        
          OR ftrcp.STATE LIKE '%'+@STATE+'%')    
         AND (@STATUS_ID IS NULL                                        
           OR ftrcp.RECONCILIATION_STATUS_ID LIKE '%'+@STATUS_ID+'%')                                        
      AND ftrcp.DELETED = 0                                        
      ) AS RECONCILIATIONS                                        
       
                                     
    ORDER BY CASE                                      
          WHEN @SORT_BY = 'Status'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN STATUS_NAME                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Status'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN STATUS_NAME                                      
         END DESC,                                    
      CASE                                      
     WHEN @SORT_BY = 'Deposit'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN DEPOSIT_DATE                                      
         END ASC,                                      
     CASE                                      
          WHEN @SORT_BY = 'Deposit'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN DEPOSIT_DATE                                      
         END DESC,                                    
      CASE                                      
          WHEN @SORT_BY = 'Deposit_Type'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN DEPOSIT_TYPE_ID                                      
         END ASC,                               
         CASE  
          WHEN @SORT_BY = 'Deposit_Type'                                      
            AND @SORT_ORDER = 'DESC'                        
          THEN DEPOSIT_TYPE_ID                                      
         END DESC,                                    
       CASE                                      
          WHEN @SORT_BY = 'Category'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN CATEGORY_ID                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Category'                                     
            AND @SORT_ORDER = 'DESC'                                      
          THEN CATEGORY_ID                                      
         END DESC,                                     
       CASE                         
          WHEN @SORT_BY = 'Insurance'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN FOX_TBL_INSURANCE_NAME                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Insurance'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN FOX_TBL_INSURANCE_NAME                                    
         END DESC,                     
      CASE                                      
          WHEN @SORT_BY = 'Check'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN CHECK_NO                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Check'                                    
            AND @SORT_ORDER = 'DESC'                                      
          THEN CHECK_NO                                      
         END DESC,                                    
      CASE                                      
          WHEN @SORT_BY = 'Amount'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN AMOUNT                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Amount'                                      
            AND @SORT_ORDER = 'DESC'                                
          THEN AMOUNT                                      
         END DESC,                                    
      CASE                                      
          WHEN @SORT_BY = 'Assigned'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN ASSIGNED_DATE                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Assigned'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN ASSIGNED_DATE                                      
         END DESC,                                    
      CASE                                      
          WHEN @SORT_BY = 'Assigned_To'                                   
            AND @SORT_ORDER = 'ASC'                                      
          THEN ASSIGNED_TO_NAME                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Assigned_To'                                      
            AND @SORT_ORDER = 'DESC'                            
          THEN ASSIGNED_TO_NAME                                      
         END DESC,                                    
       CASE                                      
          WHEN @SORT_BY = 'Amount_Posted'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN AMOUNT_POSTED                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Amount_Posted'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN AMOUNT_POSTED                                      
         END DESC,                                    
       CASE                                 
       WHEN @SORT_BY = 'Amount_Not_Posted'                                      
            AND @SORT_ORDER = 'ASC'                           
          THEN AMOUNT_NOT_POSTED                                      
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Amount_Not_Posted'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN AMOUNT_NOT_POSTED                                      
         END DESC,                                    
      CASE                                      
          WHEN @SORT_BY = 'Completed_Date'                                      
        AND @SORT_ORDER = 'ASC'                                      
          THEN COMPLETED_DATE        
         END ASC,                                      
         CASE                                      
          WHEN @SORT_BY = 'Completed_Date'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN COMPLETED_DATE                                      
         END DESC ,                            
 CASE                                      
          WHEN @SORT_BY = 'DATE_POSTED'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN DATE_POSTED                                      
         END DESC,                             
  CASE                                      
          WHEN @SORT_BY = 'DATE_POSTED'                                      
            AND @SORT_ORDER = 'ASC'                                   
          THEN DATE_POSTED                                      
         END ASC,                            
   CASE                                      
          WHEN @SORT_BY = 'REASON_NAME'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN REASON_NAME                                      
         END DESC,                              
  CASE                                      
          WHEN @SORT_BY = 'REASON_NAME'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN REASON_NAME                                      
         END ASC,                        
 CASE                                      
          WHEN @SORT_BY = 'ASSIGNED_GROUP'                                      
            AND @SORT_ORDER = 'DESC'                                      
          THEN ASSIGNED_GROUP                                      
         END DESC,                              
  CASE                                      
          WHEN @SORT_BY = 'ASSIGNED_GROUP'                                      
            AND @SORT_ORDER = 'ASC'                                     
          THEN ASSIGNED_GROUP                                      
         END ASC , 
 CASE                                      
          WHEN @SORT_BY = 'ASSIGNED_GROUP_DATE'                        
            AND @SORT_ORDER = 'DESC'                                      
          THEN ASSIGNED_GROUP_DATE                                      
         END DESC,         CASE                                      
          WHEN @SORT_BY = 'ASSIGNED_GROUP_DATE'                                      
            AND @SORT_ORDER = 'ASC'                                      
          THEN ASSIGNED_GROUP_DATE                                      
         END ASC,    
  CASE                                      
          WHEN @SORT_BY = 'State'                        
            AND @SORT_ORDER = 'DESC'                                      
          THEN [STATE]                                      
         END DESC,         CASE                                      
          WHEN @SORT_BY = 'State'                                      
   AND @SORT_ORDER = 'ASC'                                      
          THEN [STATE]                                      
         END ASC    
      OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                                    
--  END                          
END; 

