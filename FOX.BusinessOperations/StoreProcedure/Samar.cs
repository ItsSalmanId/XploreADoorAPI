using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.StoreProcedure
{
    public class Samar
    {

        // ALTER PROCEDURE[DBO].[FOX_PROC_GET_ORIGINAL_QUEUE_UNASSIGNED] --1011163 
        //( @PRACTICE_CODE BIGINT = NULL, @CLIENT VARCHAR(100)=NULL, @CURRENT_PAGE INT = NULL,
        //  @RECORD_PER_PAGE INT=NULL, @SEARCH_TEXT VARCHAR(30)=NULL, @SORCE_STRING VARCHAR( 
        //100)=NULL, @SORCE_TYPE VARCHAR(50)=NULL, @UNIQUE_ID BIGINT = NULL, @SORT_BY
        //VARCHAR(50)=NULL, @SORT_ORDER VARCHAR(50)=NULL ) AS begin SET nocount ON; 
        //IF(@SORCE_STRING= '') begin SET @SORCE_STRING =NULL end ELSE begin SET
        // @SORCE_STRING = '%' + @SORCE_STRING + '%' end IF(@SORCE_TYPE= '') begin SET
        //@SORCE_TYPE =NULL end ELSE begin SET @SORCE_TYPE = '%' + @SORCE_TYPE + '%' end SET
        //@CURRENT_PAGE = @CURRENT_PAGE-1 DECLARE @START_FROM INT= @CURRENT_PAGE*
        //@RECORD_PER_PAGE DECLARE @TOATL_PAGESUDM FLOAT SELECT @TOATL_PAGESUDM=count(*)
        //FROM fox_tbl_work_queue WHERE(sorce_name LIKE '%' + @SEARCH_TEXT+'%' OR
        //sorce_type LIKE '%' + @SEARCH_TEXT+'%') AND isnull(deleted, 0) = 0 AND
        //sorce_name LIKE isnull(@SORCE_STRING, sorce_name) AND sorce_type LIKE isnull(
        //@SORCE_TYPE, sorce_type) AND assigned_to IS NULL SET @TOATL_PAGESUDM = ceiling(
        //@TOATL_PAGESUDM / @RECORD_PER_PAGE) SELECT*, @TOATL_PAGESUDM AS
        //total_rocord_pages FROM(SELECT work_id, unique_id, practice_code, sorce_type,
        //sorce_name, work_status, receive_date, total_pages, no_of_splits, file_path,
        //assigned_to, assigned_by, assigned_date, completed_by, completed_date,
        //created_by, CONVERT(VARCHAR, datediff(hour, assigned_date, getdate()))+':'+ 
        //CONVERT(VARCHAR, datediff(minute, assigned_date, getdate())%60) AS elapsetime,
        //created_date, modified_by, modified_date, deleted, row_number() over(ORDER BY
        //created_date ASC) AS activerow FROM fox_tbl_work_queue WHERE(sorce_name LIKE
        //'%' + @SEARCH_TEXT + '%' OR sorce_type LIKE '%' + @SEARCH_TEXT + '%') AND isnull(
        //deleted, 0) = 0 AND sorce_name LIKE isnull(@SORCE_STRING, sorce_name) AND
        //sorce_type LIKE isnull(@SORCE_TYPE, sorce_type) AND unique_id LIKE isnull(
        //@UNIQUE_ID, unique_id) AND assigned_to IS NULL ) AS work_queue ORDER BY CASE
        //WHEN @SORT_BY = 'UNIQUE_ID' AND @SORT_ORDER = 'ASC' THEN unique_id end ASC, CASE
        //WHEN @SORT_BY = 'UNIQUE_ID' AND @SORT_ORDER = 'DESC' THEN unique_id end ASC,
        //CASE WHEN @SORT_BY = 'SourceType' AND @SORT_ORDER = 'DESC' THEN sorce_type end
        //DESC, CASE WHEN @SORT_BY = 'SourceType' AND @SORT_ORDER = 'ASC' THEN sorce_type
        //end DESC, CASE WHEN @SORT_BY = 'SourceName' AND @SORT_ORDER = 'ASC' THEN
        //sorce_name end ASC, CASE WHEN @SORT_BY = 'SourceName' AND @SORT_ORDER = 'DESC'
        //THEN sorce_name end DESC, CASE WHEN @SORT_BY = 'DateTimeReceived' AND
        //@SORT_ORDER = 'ASC' THEN receive_date end ASC, CASE WHEN @SORT_BY =
        //'DateTimeReceived' AND @SORT_ORDER = 'DESC' THEN receive_date end DESC, CASE
        //WHEN @SORT_BY = 'Progress' AND @SORT_ORDER = 'ASC' THEN no_of_splits end ASC,
        //CASE WHEN @SORT_BY = 'Progress' AND @SORT_ORDER = 'DESC' THEN no_of_splits end
        //DESC, CASE WHEN @SORT_BY = 'ElaspSeTime' AND @SORT_ORDER = 'ASC' THEN elapsetime
        //end ASC, CASE WHEN @SORT_BY = 'ElaspSeTime' AND @SORT_ORDER = 'DESC' THEN
        //elapsetime end DESC offset @START_FROM rows FETCH next @RECORD_PER_PAGE rows
        //only end
        //////////////////////////////////////////////////////////////
        // CREATE PROCEDURE[dbo].[Fox_proc_get_patient_list] 
        //--10, 1, null, null, null, null, null, null, null  
        //(@PageSize AS INT=NULL, 
        // @PageIndex AS INT=NULL, 
        // @SearchString AS VARCHAR(50)=NULL, 
        // @First_Name AS VARCHAR(50)=NULL, 
        // @Last_Name AS VARCHAR(50)=NULL, 
        // @Middle_Name AS VARCHAR(50)=NULL, 
        // @TotalRecord AS INT out, 
        // @SortColumn VARCHAR(20) =NULL, 
        // @SortOrder VARCHAR(4)=NULL) 
        //AS
        //  BEGIN
        //      IF(@First_Name = '')
        //        BEGIN
        //            SET @First_Name=NULL
        //        END

        //      IF(@Last_Name = '')
        //        BEGIN
        //            SET @Last_Name=NULL
        //        END

        //      IF(@Middle_Name = '')
        //        BEGIN
        //            SET @Middle_Name=NULL
        //        END

        //      IF(@SearchString = '')
        //        BEGIN
        //            SET @SearchString=NULL
        //        END

        //      IF(@SortColumn = '')
        //        BEGIN
        //            SET @SortColumn=NULL
        //        END

        //      IF(@SortOrder = '')
        //        BEGIN
        //            SET @SortOrder=NULL
        //        END

        //      SET nocount ON

        //      DECLARE @IFirstRecord INT = NULL,
        //              @ILastRecord  INT = NULL

        //      SET @IFirstRecord = ( @PageIndex - 1 ) * @PageSize
        //      SET @ILastRecord = ( @PageIndex* @PageSize + 1 ) 
        //      SET @TotalRecord = @IFirstRecord - @ILastRecord + 1;

        //        WITH cte_results
        //           AS(SELECT Row_number()
        //                        OVER(
        //                          ORDER BY CASE WHEN (@SortColumn = 'First_Name' AND
        //                        @SortOrder =
        //                        'ASC') THEN
        //                       first_name END ASC, CASE WHEN(@SortColumn = 'First_Name'

        //                       AND
        //                       @SortOrder =
        //                       'DESC') THEN first_name END DESC, CASE WHEN(@SortColumn

        //                       =
        //                       'Last_Name'

        //                       AND
        //                       @SortOrder = 'ASC') THEN last_name END ASC, CASE WHEN
        //                       @SortColumn =
        //                       'Last_Name' AND @SortOrder = 'DESC' THEN last_name END
        //                       DESC,
        //                       CASE
        //                        WHEN
        //                        @SortColumn = 'created_date' AND @SortOrder = 'DESC'
        //                        THEN
        //                        created_date
        //                        END
        //                        DESC, CASE WHEN @SortColumn = 'created_date' AND
        //                        @SortOrder =
        //                        'ASC'
        //                        THEN
        //                        created_date END ASC)                     AS ROWNUM,
        //                      Count(*)
        //                        OVER()                                    AS TotalRecord
        //                      ,
        //                      patient.patient_account,
        //                      patient.first_name,
        //                      patient.last_name,
        //                      patient.middle_name,
        //                      patient.ssn,
        //                      patient.date_of_birth,
        //                      patient.gender,
        //                      patient.created_by,
        //                      patient.modified_by,
        //                      CONVERT(VARCHAR, patient.created_date, 101) AS
        //                      created_date,
        //                      patient.email_address,
        //                      patient.modified_date,
        //                      patient.deleted,
        //                      patient.home_phone,
        //                      patient.cell_phone,
        //                      patient.business_phone,
        //                      patient.address,
        //                      patient.zip,
        //                      patient.city,
        //                      patient.state,
        //                      patient.address_type
        //               FROM   patient
        //               WHERE(@First_Name IS NULL
        //                        OR first_name LIKE '%' + @First_Name + '%')
        //                     AND(@Last_Name IS NULL
        //                            OR last_name LIKE '%' + @Last_Name + '%')
        //                     AND(@MIDDLE_NAME IS NULL
        //                            OR middle_name LIKE'%' + @MIDDLE_NAME + '%')
        //                     AND(@SearchString IS NULL
        //                            OR first_name LIKE '%' + @SearchString + '%'
        //                            OR last_name LIKE '%' + @SearchString + '%'
        //                            OR middle_name LIKE '%' + @SearchString + '%')
        //                     AND deleted != 1) 
        //      SELECT CPC.patient_account,
        //             CPC.first_name,
        //             CPC.last_name,
        //             CPC.middle_name,
        //             CPC.ssn,
        //             CPC.date_of_birth,
        //             CPC.gender,
        //             CPC.email_address,
        //             CPC.home_phone,
        //             CPC.cell_phone,
        //             CPC.business_phone,
        //             CPC.address,
        //             CPC.zip,
        //             CPC.city,
        //             CPC.state,
        //             CPC.address_type,
        //             adr.address AS Address2,
        //             adr.zip AS ZIP2,
        //             adr.city AS City2,
        //             adr.state AS STATE2,
        //             adr.address_type AS Address_Type2,
        //             CPC.created_by,
        //             CPC.created_date,
        //             CPC.modified_by,
        //             CPC.modified_date,
        //             CPC.deleted,
        //             totalrecord
        //      FROM   cte_results AS CPC
        //             LEFT JOIN fox_tbl_patient_address adr
        //                    ON CPC.patient_account = adr.patient_account
        //      WHERE rownum > @IFirstRecord
        //             AND rownum<@ILastRecord
        //  END
        //////////////////////////////////////////////////////////////

  // ALTER PROCEDURE FOX_PROC_GET_ORIGINAL_QUEUE_COMPLETE --1011163,2,0,10,'','','',''

//(

// @PRACTICE_CODE BIGINT,
// @CURRENT_PAGE int,
// @SSN bigint,
// @FIRST_NAME varchar(50),
// @LAST_NAME varchar(50),
// @ASSIGN_TO varchar(50), 
// @SOURCE_TYPE varchar(50),
// @SOURCE_NAME varchar(50),
// @RECORD_PER_PAGE int,
// @SEARCH_TEXT varchar(30),
// @INDEXED_BY varchar(100),
// @SORT_BY varchar(50),
// @SORT_ORDER varchar(20)

 

//)

//AS

//BEGIN

//SET NOCOUNT ON;

//IF(@FIRST_NAME= '') BEGIN SET @FIRST_NAME =NULL END



//IF(@LAST_NAME= '') BEGIN SET @LAST_NAME =NULL END




//IF(@ASSIGN_TO= '') BEGIN SET @ASSIGN_TO =NULL END

//IF(@SSN= '') BEGIN SET @SSN =NULL END

//IF(@SOURCE_NAME= '') BEGIN SET @SOURCE_NAME =NULL END



//IF(@SOURCE_TYPE= '') BEGIN SET @SOURCE_TYPE =NULL END




//IF(@SOURCE_TYPE= '') BEGIN SET @SOURCE_TYPE =NULL END


//IF(@INDEXED_BY= '') BEGIN SET @INDEXED_BY =NULL END

//ELSE BEGIN SET @INDEXED_BY = '%' + @INDEXED_BY + '%' END



//    IF(@SOURCE_TYPE= '') BEGIN SET @SOURCE_TYPE =NULL END

// IF(@ASSIGN_TO = '') BEGIN SET @ASSIGN_TO = null END

// IF(@SOURCE_NAME = '') BEGIN SET @SOURCE_NAME = null END


// SET @CURRENT_PAGE = @CURRENT_PAGE-1

//DECLARE @START_FROM INT= @CURRENT_PAGE* @RECORD_PER_PAGE

//DECLARE @TOATL_PAGESUDM FLOAT

//SELECT  @TOATL_PAGESUDM=COUNT(*)

//FROM FOX_TBL_WORK_QUEUE

//WHERE(SORCE_NAME LIKE  '%' + @SEARCH_TEXT+'%' OR SORCE_TYPE LIKE '%' + @SEARCH_TEXT+'%') AND ISNULL(DELETED,0) = 0 

//AND(INDEXED_BY LIKE ISNULL(@INDEXED_BY, INDEXED_BY) OR INDEXED_BY IS NULL)
//and(SORCE_TYPE LIKE ISNULL(@SOURCE_TYPE, SORCE_TYPE) OR SORCE_TYPE IS null)
//and(SORCE_NAME Like ISnull(@SOURCE_NAME, SORCE_NAME) OR SORCE_NAME IS null)



//SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)


//SELECT*,@TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES FROM

//(SELECT UNIQUE_ID, SORCE_TYPE, SORCE_NAME, TOTAL_PAGES, First_Name, Last_Name, SSN, ASSIGNED_TO, Date_Of_Birth,

// ROW_NUMBER() OVER(ORDER BY UNIQUE_ID ASC) AS ACTIVEROW


//  FROM FOX_TBL_WORK_QUEUE fox LEFT join Patient p on fox.PATIENT_ACCOUNT=p.Patient_Account

// WHERE(SORCE_NAME LIKE  '%' + @SEARCH_TEXT+'%' OR SORCE_TYPE LIKE '%' + @SEARCH_TEXT+'%' OR First_Name Like '%' + @SEARCH_TEXT+'%' Or Last_Name LIKE '%' + @SEARCH_TEXT+'%')
//AND(INDEXED_BY LIKE ISNULL(@INDEXED_BY, INDEXED_BY)   OR INDEXED_BY IS NULL)
//--and(SORCE_NAME LIKE ISNULL(@SOURCE_NAME, SORCE_NAME)   OR SORCE_NAME is null)
//--and(SORCE_TYPE LIKE ISNULL(@SOURCE_TYPE, SORCE_TYPE)   OR SORCE_TYPE is null)
//--and(First_Name LIKE ISNULL(@FIRST_NAME, FIRST_NAME)   OR First_Name is null)
//--and(Last_Name LIKE ISNULL(@LAST_NAME, Last_Name)    OR Last_Name   is null)
//--and(ASSIGNED_TO LIKE ISNULL(@ASSIGN_TO, ASSIGNED_TO)    OR @ASSIGN_TO   is null)

//AND(@SOURCE_NAME IS NULL OR     SORCE_NAME LIKE '%' + @SOURCE_NAME + '%')
//AND(@SOURCE_TYPE IS NULL OR     SORCE_TYPE LIKE '%' + @SOURCE_TYPE + '%')
//AND(@ASSIGN_TO IS NULL OR     ASSIGNED_TO LIKE '%'+ @ASSIGN_TO + '%')
//AND(@FIRST_NAME IS NULL OR    First_Name LIKE '%' + @FIRST_NAME + '%')
//AND(@LAST_NAME IS NULL OR    Last_Name LIKE '%'   + @LAST_NAME + '%')

//) 


//AS WORK_QUEUE

//ORDER BY 

//case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'ASC' then UNIQUE_ID end ASC,

//case when @SORT_BY = 'UNIQUE_ID'  and @SORT_ORDER = 'DESC' then UNIQUE_ID  end DESC,

//case when @SORT_BY = 'FirstName' and @SORT_ORDER = 'ASC' then First_Name end ASC,

//case when @SORT_BY = 'FirstName'  and @SORT_ORDER = 'DESC' then First_Name end DESC,

//case when @SORT_BY = 'LastName' and @SORT_ORDER = 'ASC' then Last_Name end ASC,
//case when @SORT_BY = 'LastName' and @SORT_ORDER = 'DESC' then Last_Name end DESC,

//case when @SORT_BY = 'AssignTo'  and @SORT_ORDER = 'DESC' then ASSIGNED_TO end DESC,

//case when @SORT_BY = 'AssignTo' and @SORT_ORDER = 'ASC' then ASSIGNED_TO end ASC,

//case when @SORT_BY = 'AssignTo'  and @SORT_ORDER = 'DESC' then Last_Name end DESC,

//case when @SORT_BY = 'SourceType' and @SORT_ORDER = 'ASC' then SORCE_TYPE end ASC,

//case when @SORT_BY = 'SourceType'  and @SORT_ORDER = 'DESC' then SORCE_TYPE end DESC,

//case when @SORT_BY = 'SourceName' and @SORT_ORDER = 'ASC' then SORCE_NAME end ASC,

//case when @SORT_BY = 'SourceName'  and @SORT_ORDER = 'DESC' then SORCE_NAME  end DESC


//OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY


//END



    }
}