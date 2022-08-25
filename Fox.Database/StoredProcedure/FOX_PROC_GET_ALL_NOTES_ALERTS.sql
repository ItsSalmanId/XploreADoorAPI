IF (OBJECT_ID('FOX_PROC_GET_ALL_NOTES_ALERTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ALL_NOTES_ALERTS
GO
--FOX_PROC_GET_ALL_NOTES_ALERTS_TEST 1011163,1,10,'','101116354814877','CREATED_DATE','DESC'           
CREATE PROC FOX_PROC_GET_ALL_NOTES_ALERTS @PRACTICE_CODE    BIGINT,         
                                         @CURRENT_PAGE     INT,         
                                         @RECORDS_PER_PAGE INT,         
                                         @SEARCH_TEXT      VARCHAR(20),         
                                         @PATIENT_ACCOUNT  BIGINT,         
                                         @SORT_BY          VARCHAR(50),         
                                         @SORT_ORDER       VARCHAR(5)        
AS        
     BEGIN        
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;        
         DECLARE @TOTAL_RECORDS FLOAT;        
         DECLARE @TOTAL_PAGES FLOAT;        
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORDS_PER_PAGE;        
         SELECT @TOTAL_RECORDS =        
         (        
             SELECT COUNT(fta.FOX_TBL_ALERT_ID)        
             FROM dbo.FOX_TBL_ALERT fta        
             WHERE fta.PATIENT_ACCOUNT = @PATIENT_ACCOUNT        
                   AND fta.DELETED = 0        
                   AND fta.PRACTICE_CODE = @PRACTICE_CODE        
                   AND fta.NOTE_DETAIL LIKE '%'+@SEARCH_TEXT+'%'        
         );        
         IF(@RECORDS_PER_PAGE = 0)        
             BEGIN        
                 SET @RECORDS_PER_PAGE = @TOTAL_RECORDS;        
             END;        
             ELSE        
             BEGIN        
                 SET @RECORDS_PER_PAGE = @RECORDS_PER_PAGE;        
             END;        
         SET @TOTAL_PAGES = CEILING(@TOTAL_RECORDS / @RECORDS_PER_PAGE);        
         SELECT *,         
                @TOTAL_PAGES AS TOTAL_PAGES,         
                @TOTAL_RECORDS AS TOTAL_RECORDS        
         FROM        
         (        
             SELECT fta.*,         
                    ftat.DESCRIPTION AS ALERT_TYPE_NAME,         
                    CASE                     
     WHEN ISNULL(ftau.LAST_NAME, '') = '' THEN ISNULL(ftau.FIRST_NAME, '')                    
     ELSE ISNULL(ftau.LAST_NAME, '') + ', ' + ISNULL(ftau.FIRST_NAME, '')                    
     END AS CREATED_BY_FULL_NAME       
     ,CASE                     
     WHEN ISNULL(ftaui.LAST_NAME, '') = '' THEN ISNULL(ftaui.FIRST_NAME, '')                    
     ELSE ISNULL(ftaui.LAST_NAME, '') + ', ' + ISNULL(ftaui.FIRST_NAME, '')                    
     END AS MODIFIED_BY_FULL_NAME      
                    --ftau.LAST_NAME+', '+ftau.FIRST_NAME AS CREATED_BY_FULL_NAME        
             FROM FOX_TBL_ALERT fta        
                  LEFT OUTER JOIN dbo.FOX_TBL_ALERT_TYPE ftat ON fta.ALERT_TYPE_ID = ftat.ALERT_TYPE_ID        
                  LEFT OUTER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON fta.CREATED_BY = ftau.USER_NAME        
      LEFT OUTER JOIN dbo.FOX_TBL_APPLICATION_USER ftaui ON fta.MODIFIED_BY = ftaui.USER_NAME        
             WHERE fta.PATIENT_ACCOUNT = @PATIENT_ACCOUNT        
                   AND fta.DELETED = 0        
                   AND fta.PRACTICE_CODE = @PRACTICE_CODE        
                   AND fta.NOTE_DETAIL LIKE '%'+@SEARCH_TEXT+'%'        
         ) AS NoteAlerts        
         ORDER BY CASE        
                      WHEN @SORT_BY = 'NOTE_DETAIL'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN NOTE_DETAIL        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'EFFECTIVE_TILL'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN EFFECTIVE_TILL        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'CREATED_BY'        
                           AND @SORT_ORDER = 'ASC'        
  THEN CREATED_BY        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'CREATED_DATE'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN CREATED_DATE        
                  END ASC,        
        CASE        
                      WHEN @SORT_BY = 'NOTE_DETAIL'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN NOTE_DETAIL        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'EFFECTIVE_TILL'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN EFFECTIVE_TILL        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'CREATED_BY'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN CREATED_BY        
              END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'CREATED_DATE'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN CREATED_DATE        
                  END DESC        
         OFFSET @START_FROM ROWS FETCH NEXT @RECORDS_PER_PAGE ROWS ONLY;        
     END;  
