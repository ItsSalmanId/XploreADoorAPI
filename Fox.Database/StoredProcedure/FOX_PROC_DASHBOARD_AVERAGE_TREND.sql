IF (OBJECT_ID('Fox_proc_dashboard_average_trend') IS NOT NULL ) DROP PROCEDURE Fox_proc_dashboard_average_trend
GO
CREATE PROCEDURE [dbo].[Fox_proc_dashboard_average_trend]
@PRACTICE_CODE BIGINT,   
@values        INT,   
@DateFromUser  DATETIME = NULL,   
@DateToUser    DATETIME = NULL  
AS  
     BEGIN  
         DECLARE @DATE_FROM VARCHAR(MAX);  
         DECLARE @DATE_TO VARCHAR(MAX);  
         DECLARE @DATE_TO_I VARCHAR(MAX);  
         DECLARE @DATE_FROM_I VARCHAR(MAX);  
         DECLARE @Working_Days INT;  
         DECLARE @Total_Days INT;  
         SET @DATE_TO = CONVERT(DATE, DATEADD(day, 1, GETDATE()));  
         SET @DATE_TO_I = CONVERT(DATE, DATEADD(day, 1, GETDATE()));  
         IF(@values = 1)  
             BEGIN         
                 --This Month         
                 SET @DATE_FROM = CONVERT(DATE, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0));  
                 SET @DATE_FROM_I = CONVERT(DATE, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0));  
                 SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);  
             END;  
             ELSE  
         IF(@values = 2)  
             BEGIN         
                 --Last 7 days         
                 SET @DATE_FROM = (DATEADD(dd, -7, CONVERT(DATE, GETDATE())));  
                 SET @DATE_FROM_I = (DATEADD(dd, -7, CONVERT(DATE, GETDATE())));  
                 SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);  
             END;  
             ELSE  
         IF(@values = 3)  
             BEGIN         
                 --Last 14 days         
                 SET @DATE_FROM = (DATEADD(dd, -14, CONVERT(DATE, GETDATE())));  
                 SET @DATE_FROM_I = (DATEADD(dd, -14, CONVERT(DATE, GETDATE())));  
                 SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);  
             END;  
             ELSE  
         IF(@values = 4)  
             BEGIN         
                 --Last 30 days         
                 SET @DATE_FROM = (DATEADD(dd, -30, CONVERT(DATE, GETDATE())));  
                 SET @DATE_FROM_I = (DATEADD(dd, -30, CONVERT(DATE, GETDATE())));  
                 SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);  
             END;  
             ELSE  
         IF(@values = 5)  
             BEGIN  
                 SET @DATE_FROM = @DateFromUser;  
                 SET @DATE_FROM_I = @DateFromUser;  
                 SET @DATE_TO = DATEADD(DAY, 1, @DateToUser);  
                 SET @DATE_TO_I = DATEADD(DAY, 1, @DateToUser);  
                 SET @Total_Days = DATEDIFF(day, @DATE_FROM, @DATE_TO);  
             END;  
         DECLARE @daysdifference INT;  
         SET @daysdifference =  
         (  
             SELECT DATEDIFF(day, @DATE_FROM_I, @DATE_TO_I)  
         );  
         IF OBJECT_ID('tempdb..#NoOfDays') IS NOT NULL  
             DROP TABLE #noofdays;  
         CREATE TABLE #noofdays  
         (day       INT,   
          month     INT,   
          year      INT,   
          monthName VARCHAR(5)  
         );  
         WHILE(@daysdifference > 0)  
             BEGIN  
                 INSERT INTO #noofdays  
                 (day,   
                  month,   
                  year,   
                  monthName  
                 )  
                 VALUES  
                 (DAY(@DATE_FROM_I),   
                  MONTH(@DATE_FROM_I),   
                  YEAR(@DATE_FROM_I),   
                  CONVERT(CHAR(3), CONVERT(DATE, @DATE_FROM_I), 0)  
                 );  
                 SET @DATE_FROM_I = DATEADD(day, 1, @DATE_FROM_I);  
                 SET @daysdifference = @daysdifference - 1;  
             END;  
         IF OBJECT_ID('tempdb..#Table1') IS NOT NULL  
             DROP TABLE #table1;  
         SELECT DAY(receive_date) day,   
                MONTH(receive_date) month,   
                YEAR(receive_date) year,   
                COUNT(*) totalRecord  
         INTO #table1  
         FROM fox_tbl_work_queue t1  
         WHERE CONVERT(DATE, RECEIVE_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO)  
               AND CHARINDEX('_', UNIQUE_ID) <= 0  
               AND ISNULL(DELETED, 0) = 0  
               AND t1.PRACTICE_CODE = @PRACTICE_CODE  
         GROUP BY DAY(receive_date),   
                  MONTH(receive_date),   
                  YEAR(receive_date)  
         ORDER BY 1 ASC;  
         SELECT t2.day,   
                t2.month,   
                t2.monthName,   
                t2.year,   
                Isnull(t1.totalrecord, 0) totalRecord,   
         (  
             SELECT COUNT(*) / @Total_Days  
             FROM fox_tbl_work_queue  
             WHERE(CONVERT(DATE, receive_date)) BETWEEN(CONVERT(DATE, @DATE_FROM)) AND(CONVERT(DATE, @DATE_TO))  
                  AND CHARINDEX('_', UNIQUE_ID) <= 0  
                  AND ISNULL(DELETED, 0) = 0  
                  AND fox_tbl_work_queue.PRACTICE_CODE = @PRACTICE_CODE  
         ) average  
         FROM #table1 t1  
              RIGHT JOIN #noofdays t2 ON t1.day = t2.day  
                                         AND t1.month = t2.month  
                                         AND t1.year = t2.year  
         ORDER BY t2.year,   
                  t2.month,   
                  t2.day;  
     END;
