IF (OBJECT_ID('FOX_PROC_GET_ORIGINAL_QUEUE_UNASSIGNED') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORIGINAL_QUEUE_UNASSIGNED  
GO  
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  
CREATE Procedure [dbo].[FOX_PROC_GET_ORIGINAL_QUEUE_UNASSIGNED] --1011163    
(   
 @PRACTICE_CODE BIGINT=null,  
 @CURRENT_PAGE int=1,  
 @RECORD_PER_PAGE int=10,  
 @SEARCH_GO varchar(30)=null,  
 @SORCE_STRING varchar(100)=null,  
 @SORCE_TYPE varchar(50)=null,  
 @UNIQUE_ID  bigInt=null,  
 @SORT_BY varchar(50)=null,  
 @SORT_ORDER varchar(50)=null  
)  
  
AS  
  
BEGIN  
  
SET NOCOUNT ON;  
  
  
  
IF(@SORCE_STRING='')  
  
BEGIN  
  
 SET @SORCE_STRING =NULL  
  
 END  
  
 ELSE  
  
 BEGIN  
  
 SET @SORCE_STRING ='%'+@SORCE_STRING + '%'  
  
 END  
  
 IF(@SORCE_TYPE='')  
  
BEGIN  
  
 SET @SORCE_TYPE =NULL  
  
 END  
  
 ELSE  
  
 BEGIN  
  
 SET @SORCE_TYPE ='%'+@SORCE_TYPE + '%'  
  
 END  
  
  IF(@UNIQUE_ID='')  
  
BEGIN  
  
 SET @UNIQUE_ID =NULL  
  
 END  
  
 ELSE  
  
 BEGIN  
  
 SET @UNIQUE_ID ='%'+@UNIQUE_ID + '%'  
  
 END  
  
   
  
set @CURRENT_PAGE = @CURRENT_PAGE-1  
  
DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE  
  
DECLARE @TOATL_PAGESUDM FLOAT  
  
SELECT  @TOATL_PAGESUDM=COUNT(*)  
  
FROM FOX_TBL_WORK_QUEUE  
  
WHERE  (SORCE_NAME LIKE  '%' + @SEARCH_GO+'%' OR SORCE_TYPE LIKE '%' + @SEARCH_GO+'%')   
  
AND ISNULL(DELETED,0) = 0   
  
    and  SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME) and SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)  
and UNIQUE_ID LIKE ISNULL(@UNIQUE_ID, UNIQUE_ID)    
 and  ASSIGNED_TO is null  
  
SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM/@RECORD_PER_PAGE)  
  
  
SELECT *,@TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES  FROM   
  
(SELECT WORK_ID,UNIQUE_ID,PRACTICE_CODE,SORCE_TYPE,SORCE_NAME,WORK_STATUS,RECEIVE_DATE,TOTAL_PAGES,  
  
NO_OF_SPLITS,FILE_PATH, ASSIGNED_TO,ASSIGNED_BY,ASSIGNED_DATE,COMPLETED_BY,COMPLETED_DATE,CREATED_BY,  
Convert(varchar,DateDiff(HOUR, ASSIGNED_DATE, GETDATE()))+':'+ Convert(varchar,DateDiff(MINUTE, ASSIGNED_DATE, GETDATE())%60) as ElapseTime,  
CREATED_DATE,MODIFIED_BY,MODIFIED_DATE,DELETED,  ROW_NUMBER() OVER(ORDER BY CREATED_DATE ASC) AS ACTIVEROW  
FROM FOX_TBL_WORK_QUEUE  
WHERE (SORCE_NAME LIKE  '%' + @SEARCH_GO+'%' OR SORCE_TYPE LIKE '%' + @SEARCH_GO+'%')   
  
AND ISNULL(DELETED,0) = 0  
  
and SORCE_NAME LIKE ISNULL(@SORCE_STRING, SORCE_NAME) and SORCE_TYPE LIKE ISNULL(@SORCE_TYPE, SORCE_TYPE)  
  
and UNIQUE_ID LIKE ISNULL(@UNIQUE_ID, UNIQUE_ID)    
  
and ASSIGNED_TO is null  
  
)   
  
AS WORK_QUEUE  
  
ORDER BY case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'ASC' then UNIQUE_ID end ASC,  
  
   case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'DESC' then UNIQUE_ID end ASC,  
  
case when @SORT_BY = 'SourceType'  and @SORT_ORDER = 'DESC' then SORCE_TYPE end DESC,  
  
case when @SORT_BY = 'SourceType'  and @SORT_ORDER = 'ASC' then SORCE_TYPE end DESC,  
  
case when @SORT_BY = 'SourceName' and @SORT_ORDER = 'ASC' then SORCE_NAME end ASC,  
  
case when @SORT_BY = 'SourceName'  and @SORT_ORDER = 'DESC' then SORCE_NAME  end DESC,  
  
case when @SORT_BY = 'DateTimeReceived' and @SORT_ORDER = 'ASC' then RECEIVE_DATE end ASC,  
  
case when @SORT_BY = 'DateTimeReceived'  and @SORT_ORDER = 'DESC' then RECEIVE_DATE end DESC,  
  
case when @SORT_BY = 'Progress' and @SORT_ORDER = 'ASC' then NO_OF_SPLITS end ASC,  
  
case when @SORT_BY = 'Progress'  and @SORT_ORDER = 'DESC' then NO_OF_SPLITS end DESC,  
  
case when @SORT_BY = 'ElaspSeTime' and @SORT_ORDER = 'ASC' then ElapseTime end ASC,  
  
case when @SORT_BY = 'ElaspSeTime'  and @SORT_ORDER = 'DESC' then ElapseTime end DESC  
  
  
OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY  
  
END  
  
  
  
