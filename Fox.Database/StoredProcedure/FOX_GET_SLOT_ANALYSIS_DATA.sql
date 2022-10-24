IF (OBJECT_ID('FOX_GET_SLOT_ANALYSIS_DATA') IS NOT NULL ) DROP PROCEDURE FOX_GET_SLOT_ANALYSIS_DATA  
GO 
-- FOX_GET_SLOT_ANALYSIS_DATA '08/24/2018',0,15,'','','','',0

Create Procedure [dbo].[FOX_GET_SLOT_ANALYSIS_DATA]
(
@DATE varchar(15),
@START_VALUE int,
@END_VALUE int,
@BUSINESS_HOURS8A VARCHAR(50),
@BUSINESS_HOURS5A VARCHAR(50),
@SATURDAYSA VARCHAR(500),
@SUNDAYSA VARCHAR(500),
@EXCLUDEWEEKEND BIT
)
as
begin

DECLARE @BUSINESS_HOURS8 TIME 
DECLARE @BUSINESS_HOURS5 TIME 
DECLARE @SATURDAYS VARCHAR(500) 
DECLARE @SUNDAYS VARCHAR(500) 

IF(@SATURDAYSA = '') BEGIN
SET @SATURDAYS = NULL
END
ELSE BEGIN
SET @SATURDAYS=@SATURDAYSA
END

IF(@SUNDAYSA = '')
BEGIN
SET @SUNDAYSA = NULL
END
ELSE BEGIN
SET @SUNDAYS=@SUNDAYSA
END

IF(@BUSINESS_HOURS8A = '')
BEGIN
SET @BUSINESS_HOURS8 = NULL
END
ELSE BEGIN
SET @BUSINESS_HOURS8=@BUSINESS_HOURS8A
END

IF(@BUSINESS_HOURS5A = '')
BEGIN
SET @BUSINESS_HOURS5 = NULL
END
ELSE BEGIN
SET @BUSINESS_HOURS5=@BUSINESS_HOURS5A
END
IF(@START_VALUE = 1)
begin



IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA', 'U') IS NOT NULL DROP TABLE #TEMP2DATA
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA', 'U') IS NOT NULL DROP TABLE #TEMP3DATA
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA', 'U') IS NOT NULL DROP TABLE #TEMP5DATA
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA', 'U') IS NOT NULL DROP TABLE #TEMP8DATA
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA1', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA1
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA1', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA1


IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS 
 SELECT * INTO #WORKQUEUEFILTEREDRECORDS 
FROM FOX_TBL_WORK_QUEUE Where
CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS8,RECEIVE_DATE))
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))
AND (DATENAME(dw,RECEIVE_DATE) = ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) 
OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))

IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS 
 SELECT (CONVERT(BIGINT,  
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) 
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE,WORK_ID INTO #TOTALTIMEINSECONDS 
FROM #WORKQUEUEFILTEREDRECORDS  
WHERE COMPLETED_DATE IS NOT NULL  
AND WORK_STATUS=UPPER('COMPLETED')

SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,W.DELETED,
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,
--CASE WHEN ISNULL((CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))),0) >0 THEN 1 ELSE 0 END COMPLETEFILESCOUNT,
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE
 INTO #WORKQUEUE_DATA FROM #WORKQUEUEFILTEREDRECORDS W
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO 
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID
WHERE COMPLETED_DATE IS NOT NULL  
AND ISNULL(W.DELETED,0)=0 
AND W.WORK_STATUS=UPPER('COMPLETED')

SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,DELETED,
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME
ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA
FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS  AS MAIN
ORDER BY WORK_ID,CREATED_DATE ASC

SELECT  Q.RECEIVE_DATE, A.* INTO    #TEMP3DATA FROM #LOGQUEUE_DATA A
JOIN #WORKQUEUEFILTEREDRECORDS Q ON A.WORK_ID = Q.WORK_ID AND WORK_STATUS=UPPER('COMPLETED') AND ISNULL(Q.DELETED,0)=0



SELECT WORK_ID,RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE,DELETED INTO   #TEMP2DATA FROM (
 SELECT  * FROM #WORKQUEUE_DATA UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE  FROM #TEMP3DATA
)A
GROUP BY A.ROLE_NAME,A.WORK_ID,RECEIVE_DATE,A.DELETED
-------------------------------------------------------------------
SELECT DISTINCT A.WORK_ID, CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,
(CASE WHEN ISNULL(B.ROLE_NAME,'')=UPPER('INDEXER') THEN B.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,
(CASE WHEN ISNULL(B.ROLE_NAME,'')=UPPER('AGENT') THEN B.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,
(CASE WHEN ISNULL(B.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN B.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME
 INTO  #TEMP5DATAA1
 FROM #TEMP2DATA A
LEFT JOIN #TEMP2DATA B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)  and a.WORK_ID=b.work_id

select 
 CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,

SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME
 INTO  #TEMP5DATAA
 FROM #TEMP5DATAA1 A
GROUP BY A.WORK_ID,CONVERT(DATE,a.RECEIVE_DATE)
ORDER BY 1

------------------------------------------------------------------------------

SELECT WORK_ID, CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB FROM #WORKQUEUE_DATA A
GROUP BY WORK_ID,CONVERT(DATE,A.RECEIVE_DATE)
ORDER BY 1 

SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA FROM #TEMP5DATAA A
LEFT JOIN #TEMP5DATAB B ON A.RECEIVE_DATE=B.RECEIVE_DATE
ORDER BY 1

  
 SELECT WORK_ID,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME INTO     #TEMP8DATA FROM #TEMP5DATA
 UNION ALL
 SELECT WORK_ID,CONVERT(INT, SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT, SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, 
  CONVERT(INT, SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME
 FROM #TEMP5DATA group by WORK_ID 

 IF OBJECT_ID('TEMPDB.DBO.#FinalRecords', 'U') IS NOT NULL DROP TABLE  #FinalRecords
 SELECT distinct
 WORK_ID,
CONVERT(VARCHAR, (INDEXER_TOTAL_TIME)/600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, INDEXER_TOTAL_TIME)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, INDEXER_TOTAL_TIME)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, INDEXER_TOTAL_TIME)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME', 

CONVERT(VARCHAR, (AGENT_TOTAL_TIME)/3600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, AGENT_TOTAL_TIME)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',

CONVERT(VARCHAR, (SUPERVISOR_TOTAL_TIME)/3600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME'
into  #FinalRecords
FROM #TEMP8DATA  

IF OBJECT_ID('TEMPDB.DBO.#SupervisorData', 'U') IS NOT NULL DROP TABLE  #SupervisorData
select min(CREATED_DATE) as CREATED_DATE, WORK_ID into #SupervisorData from FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS
where Upper(ASSIGNED_TO_DESIGNATION) = 'Supervisor' 
and isnull(deleted,0) = 0 group by WORK_ID

IF OBJECT_ID('TEMPDB.DBO.#SupervisorDataFinal', 'U') IS NOT NULL DROP TABLE  #SupervisorDataFinal
select s.*,u.FIRST_NAME+' '+u.LAST_NAME as NAME into #SupervisorDataFinal from #SupervisorData s left join  FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS r on s.CREATED_DATE = r.CREATED_DATE
left join FOX_TBL_APPLICATION_USER  u on u.USER_NAME = r.ASSIGNED_TO
and isnull(u.deleted,0) = 0 

IF OBJECT_ID('TEMPDB.DBO.#FinalRecordsUsers', 'U') IS NOT NULL DROP TABLE  #FinalRecordsUsers
select  o.*,p.CREATED_DATE  ,p.NAME  into #FinalRecordsUsers  from #FinalRecords o left join #SupervisorDataFinal p on o.WORK_ID = p.WORK_ID



IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS_SLOT', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS_SLOT 
 SELECT 
 WORK_ID,UNIQUE_ID,PRACTICE_CODE,PATIENT_ACCOUNT,SORCE_TYPE,SORCE_NAME,WORK_STATUS
,Convert(varchar,CONVERT(DateTime,RECEIVE_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,RECEIVE_DATE),108 ) AS RECEIVE_DATE
,TOTAL_PAGES,NO_OF_SPLITS,FILE_PATH,
(select FIRST_NAME +' '+LAST_NAME  NAME from FOX_TBL_APPLICATION_USER where USER_NAME = ASSIGNED_TO)  ASSIGNED_TO
,ASSIGNED_BY
,Convert(varchar,CONVERT(DateTime,ASSIGNED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,ASSIGNED_DATE),108 ) AS ASSIGNED_DATE
,(select FIRST_NAME +' '+LAST_NAME  NAME from FOX_TBL_APPLICATION_USER where USER_NAME = COMPLETED_BY) COMPLETED_BY
,Convert(varchar,CONVERT(DateTime,COMPLETED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,COMPLETED_DATE),108 ) AS COMPLETED_DATE
,INDEXED_BY 
, Convert(varchar,CONVERT(DateTime,INDEXED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,INDEXED_DATE),108 ) AS INDEXED_DATE
,DOCUMENT_TYPE,(select NAME from fox_tbl_document_type where DOCUMENT_TYPE = DOCUMENT_TYPE_ID) as DOCUMENT_NAME ,SENDER_ID,FACILITY_NAME,DEPARTMENT_ID,IS_EMERGENCY_ORDER,REASON_FOR_VISIT,ACCOUNT_NUMBER,UNIT_CASE_NO,CREATED_BY,MODIFIED_BY
, Convert(varchar,CONVERT(DateTime,TRANSFER_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,TRANSFER_DATE),108 ) AS TRANSFER_DATE
,supervisor_status,FAX_ID
, Convert(varchar,CONVERT(DateTime,INDEXER_ASSIGN_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,INDEXER_ASSIGN_DATE),108 ) AS INDEXER_ASSIGN_DATE
, Convert(varchar,CONVERT(DateTime,AGENT_ASSIGN_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,AGENT_ASSIGN_DATE),108 ) AS AGENT_ASSIGN_DATE

 INTO #WORKQUEUEFILTEREDRECORDS_SLOT  FROM #WORKQUEUEFILTEREDRECORDS where CONVERT(DATE,RECEIVE_DATE) =@DATE

IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS_SLOT', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS_SLOT 
 SELECT * ,(CONVERT(BIGINT,DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) 
TOTAL_MINS,(CONVERT(BIGINT,DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTAL_SECONDSS
INTO  #TOTALTIMEINSECONDS_SLOT FROM  #WORKQUEUEFILTEREDRECORDS_SLOT 

IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME_SLOT', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME_SLOT 
SELECT x.* ,Convert(varchar,CONVERT(DateTime,y.CREATED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,y.CREATED_DATE),108 ) AS SUPERVISOR_ASSIGN_DATE,

y.INDEXER_TOTAL_TIME,

y.AGENT_TOTAL_TIME,

y.SUPERVISOR_TOTAL_TIME,


 CONVERT(VARCHAR, (TOTAL_SECONDSS)/3600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, TOTAL_SECONDSS)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, TOTAL_SECONDSS)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, TOTAL_SECONDSS)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, TOTAL_SECONDSS)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, TOTAL_SECONDSS)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, TOTAL_SECONDSS)%60) END 'TIME_TO_COMPLETE'
  FROM #TOTALTIMEINSECONDS_SLOT x left join #FinalRecordsUsers y  on y.WORK_ID =x.WORK_ID  where TOTAL_MINS  > @END_VALUE 
  end

---------------------------------------------------------------
else
begin


IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA_E', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA_E
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA_E', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA_E', 'U') IS NOT NULL DROP TABLE #TEMP2DATA_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA_E', 'U') IS NOT NULL DROP TABLE #TEMP3DATA_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_E', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB_E', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_E', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA_E', 'U') IS NOT NULL DROP TABLE #TEMP8DATA_E
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA_E', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA_E
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA_E', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA_E
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA_E', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA_E
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA1_E', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA1_E
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_E1', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA_E1

IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS_E', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS_E
 SELECT * INTO #WORKQUEUEFILTEREDRECORDS_E 
FROM FOX_TBL_WORK_QUEUE Where
CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS8,RECEIVE_DATE))
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))
AND (DATENAME(dw,RECEIVE_DATE) = ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) 
OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))

IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS_E', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS_E 
 SELECT (CONVERT(BIGINT,  
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) 
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE,WORK_ID INTO #TOTALTIMEINSECONDS_E 
FROM #WORKQUEUEFILTEREDRECORDS_E 
WHERE COMPLETED_DATE IS NOT NULL  
AND WORK_STATUS=UPPER('COMPLETED')

SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,
--CASE WHEN ISNULL((CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))),0) >0 THEN 1 ELSE 0 END COMPLETEFILESCOUNT,
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE
 INTO #WORKQUEUE_DATA_E FROM #WORKQUEUEFILTEREDRECORDS_E W
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO 
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID
WHERE COMPLETED_DATE IS NOT NULL  
AND ISNULL(W.DELETED,0)=0 
AND W.WORK_STATUS=UPPER('COMPLETED')



SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME
 AND   ISNULL(MAIN.DELETED,0)=0 ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA_E
FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS  AS MAIN
ORDER BY WORK_ID,CREATED_DATE ASC

SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA_E FROM #LOGQUEUE_DATA_E A
JOIN #WORKQUEUEFILTEREDRECORDS_E Q ON A.WORK_ID = Q.WORK_ID AND WORK_STATUS=UPPER('COMPLETED') AND ISNULL(DELETED,0)=0



SELECT WORK_ID,RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP2DATA_E FROM (
 SELECT * FROM #WORKQUEUE_DATA_E UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA_E
)A
GROUP BY A.ROLE_NAME,A.WORK_ID,RECEIVE_DATE

----------------------------

SELECT DISTINCT A.WORK_ID, CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,
(CASE WHEN ISNULL(B.ROLE_NAME,'')=UPPER('INDEXER') THEN B.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,
(CASE WHEN ISNULL(B.ROLE_NAME,'')=UPPER('AGENT') THEN B.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,
(CASE WHEN ISNULL(B.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN B.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME
 INTO  #TEMP5DATAA_E1
 FROM #TEMP2DATA_E A
LEFT JOIN #TEMP2DATA_E B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)  and a.WORK_ID=b.work_id

select 
 A.WORK_ID, CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,

SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME
 INTO  #TEMP5DATAA_E
 FROM #TEMP5DATAA_E1 A
GROUP BY A.WORK_ID,CONVERT(DATE,a.RECEIVE_DATE)
ORDER BY 1
---------------------------

SELECT WORK_ID, CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB_E FROM #WORKQUEUE_DATA_E A
GROUP BY WORK_ID,CONVERT(DATE,A.RECEIVE_DATE)
ORDER BY 1 

SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_E FROM #TEMP5DATAA_E A
LEFT JOIN #TEMP5DATAB_E B ON A.RECEIVE_DATE=B.RECEIVE_DATE  and A.WORK_ID = B.WORK_ID
ORDER BY 1

  
 SELECT WORK_ID,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME INTO     #TEMP8DATA_E FROM #TEMP5DATA_E
 UNION ALL
 SELECT WORK_ID,CONVERT(INT, SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT, SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, 
  CONVERT(INT, SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME
 FROM #TEMP5DATA_E group by WORK_ID 


 IF OBJECT_ID('TEMPDB.DBO.#FinalRecords_E', 'U') IS NOT NULL DROP TABLE  #FinalRecords_E
 SELECT distinct
 WORK_ID,
CONVERT(VARCHAR, (INDEXER_TOTAL_TIME)/600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, INDEXER_TOTAL_TIME)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, INDEXER_TOTAL_TIME)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, INDEXER_TOTAL_TIME)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME', 

CONVERT(VARCHAR, (AGENT_TOTAL_TIME)/3600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, AGENT_TOTAL_TIME)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',

CONVERT(VARCHAR, (SUPERVISOR_TOTAL_TIME)/3600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME'
into  #FinalRecords_E
FROM #TEMP8DATA_E  

IF OBJECT_ID('TEMPDB.DBO.#SupervisorDataE', 'U') IS NOT NULL DROP TABLE  #SupervisorDataE
select min(CREATED_DATE) as CREATED_DATE, WORK_ID into #SupervisorDataE from FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS
where Upper(ASSIGNED_TO_DESIGNATION) = 'Supervisor' 
and isnull(deleted,0) = 0 group by WORK_ID

IF OBJECT_ID('TEMPDB.DBO.#SupervisorDataFinal_E', 'U') IS NOT NULL DROP TABLE  #SupervisorDataFinal_E
select s.*,u.FIRST_NAME+' '+u.LAST_NAME as NAME into #SupervisorDataFinal_E from #SupervisorDataE s left join  FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS r on s.CREATED_DATE = r.CREATED_DATE
left join FOX_TBL_APPLICATION_USER  u on u.USER_NAME = r.ASSIGNED_TO
and isnull(u.deleted,0) = 0 

IF OBJECT_ID('TEMPDB.DBO.#FinalRecordsUsers_E', 'U') IS NOT NULL DROP TABLE  #FinalRecordsUsers_E
select  o.*,p.CREATED_DATE  ,p.NAME  into #FinalRecordsUsers_E  from #FinalRecords_E o left join #SupervisorDataFinal_E p on o.WORK_ID = p.WORK_ID



IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS2', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS2 
 SELECT 
 WORK_ID,UNIQUE_ID,PRACTICE_CODE,PATIENT_ACCOUNT,SORCE_TYPE,SORCE_NAME,WORK_STATUS
,Convert(varchar,CONVERT(DateTime,RECEIVE_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,RECEIVE_DATE),108 ) AS RECEIVE_DATE
,TOTAL_PAGES,NO_OF_SPLITS,FILE_PATH,
(select FIRST_NAME +' '+LAST_NAME  NAME from FOX_TBL_APPLICATION_USER where USER_NAME = ASSIGNED_TO)  ASSIGNED_TO,
ASSIGNED_BY
,Convert(varchar,CONVERT(DateTime,ASSIGNED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,ASSIGNED_DATE),108 ) AS ASSIGNED_DATE
,(select FIRST_NAME +' '+LAST_NAME  NAME from FOX_TBL_APPLICATION_USER where USER_NAME = COMPLETED_BY) COMPLETED_BY
,Convert(varchar,CONVERT(DateTime,COMPLETED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,COMPLETED_DATE),108 ) AS COMPLETED_DATE
,INDEXED_BY 
, Convert(varchar,CONVERT(DateTime,INDEXED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,INDEXED_DATE),108 ) AS INDEXED_DATE
,DOCUMENT_TYPE,(select NAME from fox_tbl_document_type where DOCUMENT_TYPE = DOCUMENT_TYPE_ID) as DOCUMENT_NAME ,SENDER_ID,FACILITY_NAME,DEPARTMENT_ID,IS_EMERGENCY_ORDER,REASON_FOR_VISIT,ACCOUNT_NUMBER,UNIT_CASE_NO,CREATED_BY,MODIFIED_BY
, Convert(varchar,CONVERT(DateTime,TRANSFER_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,TRANSFER_DATE),108 ) AS TRANSFER_DATE
,supervisor_status,FAX_ID
, Convert(varchar,CONVERT(DateTime,INDEXER_ASSIGN_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,INDEXER_ASSIGN_DATE),108 ) AS INDEXER_ASSIGN_DATE
, Convert(varchar,CONVERT(DateTime,AGENT_ASSIGN_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,AGENT_ASSIGN_DATE),108 ) AS AGENT_ASSIGN_DATE
 
 INTO #WORKQUEUEFILTEREDRECORDS2  FROM #WORKQUEUEFILTEREDRECORDS_E where CONVERT(DATE,RECEIVE_DATE)  = @DATE

IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS2', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS2 
 SELECT * ,(CONVERT(BIGINT,DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) 
TOTAL_MINS,(CONVERT(BIGINT,DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTAL_SECONDSS
INTO  #TOTALTIMEINSECONDS2 FROM  #WORKQUEUEFILTEREDRECORDS2 

IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME2', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME2 
SELECT  x.* ,Convert(varchar,CONVERT(DateTime,y.CREATED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,y.CREATED_DATE),108 ) AS SUPERVISOR_ASSIGN_DATE,
y.INDEXER_TOTAL_TIME,
y.AGENT_TOTAL_TIME,
y.SUPERVISOR_TOTAL_TIME,
 CONVERT(VARCHAR, (x.TOTAL_SECONDSS)/3600 ) +':'+
CASE WHEN 
CONVERT(VARCHAR, CONVERT(BIGINT, x.TOTAL_SECONDSS)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, x.TOTAL_SECONDSS)%3600/60) ELSE 
CONVERT(VARCHAR, CONVERT(BIGINT, x.TOTAL_SECONDSS)%3600/60) END
   +':'+
   CASE WHEN 
CONVERT(VARCHAR,CONVERT(BIGINT, x.TOTAL_SECONDSS)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, x.TOTAL_SECONDSS)%60) ELSE 
CONVERT(VARCHAR,CONVERT(BIGINT, x.TOTAL_SECONDSS)%60) END 'TIME_TO_COMPLETE'
   FROM #TOTALTIMEINSECONDS2  x left join #FinalRecordsUsers_E y  on y.WORK_ID =x.WORK_ID   where TOTAL_MINS  between  @START_VALUE and @END_VALUE

end
end


/********************************************************************************************************************************************************************************************************/
