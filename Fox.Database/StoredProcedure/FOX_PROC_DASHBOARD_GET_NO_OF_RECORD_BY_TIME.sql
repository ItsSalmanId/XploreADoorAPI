IF (OBJECT_ID('FOX_PROC_DASHBOARD_GET_NO_OF_RECORD_BY_TIME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_DASHBOARD_GET_NO_OF_RECORD_BY_TIME
GO
CREATE PROCEDURE [dbo].[FOX_PROC_DASHBOARD_GET_NO_OF_RECORD_BY_TIME]
(@PRACTICE_CODE BIGINT,   
 @HOUR_FROM     INT,   
 @HOUR_TO       INT,   
 @DATE_FROM     VARCHAR(MAX) NULL,   
 @DATE_TO       VARCHAR(MAX) NULL  
)  
AS  
     BEGIN  
         SET @DATE_FROM = ISNULL(@DATE_FROM, GETDATE());  
         SET @DATE_TO = ISNULL(@DATE_TO, GETDATE());  
         IF OBJECT_ID('TEMPDB..#NOOFHOURS') IS NOT NULL  
             DROP TABLE #NOOFHOURS;  
         CREATE TABLE #NOOFHOURS  
         ([HOUR]        INT,   
          NO_OF_RECORDS INT,   
          RECEIVED_DATE VARCHAR(MAX)  
         );  
         DECLARE @START INT;  
         DECLARE @END INT;  
         DECLARE @UNCHAGED_DATE_FROM DATE= CONVERT(DATE, @DATE_FROM);  
         DECLARE @UNCHAGED_DATE_TO DATE= CONVERT(DATE, @DATE_TO);  
         WHILE(CONVERT(DATE, @DATE_FROM) <= CONVERT(DATE, @DATE_TO))  
             BEGIN  
                 SET @START = 0;  
                 SET @END = 23;  
                 IF CONVERT(DATE, @DATE_FROM) = @UNCHAGED_DATE_FROM  
                     BEGIN  
                         SET @START = @HOUR_FROM;  
                         SET @END = 23;  
                     END;  
                 IF CONVERT(DATE, @DATE_FROM) = @UNCHAGED_DATE_TO  
                     BEGIN  
                         SET @START = 0;  
                         SET @END = @HOUR_TO;  
                     END;  
                 IF @UNCHAGED_DATE_FROM = @UNCHAGED_DATE_TO  
                     BEGIN  
                         SET @START = @HOUR_FROM;  
                         SET @END = @HOUR_TO;  
                     END;  
                 WHILE(@START <= @END)  
                     BEGIN  
                         INSERT INTO #NOOFHOURS  
                         ([HOUR],   
                          NO_OF_RECORDS,   
                          RECEIVED_DATE  
                         )  
                         VALUES  
                         (@START,   
                         (  
                             SELECT COUNT(*) NOOFRECORDS  
                             FROM FOX_TBL_WORK_QUEUE  
                             WHERE CONVERT(DATE, RECEIVE_DATE) = CONVERT(DATE, @DATE_FROM)  
                                   AND DATEPART(HOUR, RECEIVE_DATE) = @START  
                                   AND PRACTICE_CODE = @PRACTICE_CODE  
                                   AND DELETED = 0  
                         ),   
                          CONVERT(VARCHAR(10), CONVERT(DATE, @DATE_FROM), 101)  
                         );  
                         SET @START = @START + 1;  
                     END;  
                 SET @DATE_FROM = DATEADD(day, 1, CONVERT(DATE, @DATE_FROM));  
             END;  
         SELECT *
         FROM #NOOFHOURS;  
     END;
