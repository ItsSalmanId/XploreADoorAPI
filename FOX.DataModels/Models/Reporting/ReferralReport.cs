using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Reporting
{
    public class ReferralReport
    {

        [NotMapped]
        public int ROW { get; set; }
        public long WORK_ID { get; set; }
        public string PRIORITY { get; set; }
        public string UNIQUE_ID { get; set; }
        public string FAX_ID { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string INDEXING_STATUS { get; set; }
        public string INDEXED_BY { get; set; }
        public DateTime? INDEXED_DATE { get; set; }
        [NotMapped]
        public string Indexed_Date_Str { get; set; }
        public string SOURCE_TYPE { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public string ROLE_NAME { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string WORK_STATUS { get; set; }
        public string COMPLETED_BY { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        [NotMapped]
        public string Completed_Date_Str { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }
    public class ReferralReportRequest: BaseModel
    {
        public DateTime? INDEXED_DATE_FROM { get; set; }
        public DateTime? INDEXED_DATE_TO { get; set; }
        public DateTime? RECEIVED_DATE_FROM { get; set; }
        public DateTime? RECEIVED_DATE_TO { get; set; }
        public string INDEXED_DATE_FROM_STR { get; set; }
        public string INDEXED_DATE_TO_STR { get; set; }
        public string RECEIVED_DATE_FROM_STR { get; set; }
        public string RECEIVED_DATE_TO_STR { get; set; }
        public string INDEXED_STATUS { get; set; }
        public string ASSIGNED_PERSON_NAME { get; set; }
        public string STATUS { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string SOURCE_TYPE { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public string PRIORITY { get; set; }
    }
    public class ApplicatinUserName
    {
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string Full_NAME { get; set; }
        public string Full_NAME1 { get; set; }
        public string Full_NAME2 { get; set; }
        public string Full_NAME3 { get; set; }
        public string Full_NAME4 { get; set; }
    }

    public class TaskListResponse
    {
        public long ROW { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_USER { get; set; }
        public string ROLE_NAME { get; set; }
        public int ASSIGNED { get; set; }
        public int PENDING { get; set; }
        public int TRASHED_REFERRAL { get; set; }
        public int VALID_REFERRAL { get; set; }
        public int COMPLETED { get; set; }
        public string AVERAGE_RECIEVETO_COMLETIONTIME { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }

    }
    public class TaskListRequest: BaseModel
    {
        public DateTime? DATE_FROM { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string ROLE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public int CURRENT_PAGE { get; set; }
        public string SEARCH_STRING { get; set; }
        public string SORT_ORDER { get; set; }
        public string SORT_BY { get; set; }
        public string DATE_TO_STR { get; set; }
        public string DATE_FROM_STR { get; set; }
        public string CalledFrom{ get; set; }

    }

    public class HighBalanceReportRes : BaseModel
    {

        [NotMapped]
        public int ROW { get; set; }
        public long WORK_ID { get; set; }
        public long Patient_Account { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PHONE { get; set; }
        public decimal Patient_Balance { get; set; }
        public string DEFAULT_POS { get; set; }
        public string REGION { get; set; }
        public string PRIMARY_INSURANCE { get; set; }
        public string SECONDARY_INSURANCE { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string ORS { get; set; }
        public string WORK_ORDER_SOURCE { get; set; }
        public string FACILITY_NAME { get; set; }
        public string DISCIPLINE_NO { get; set; }
       
        public int TOTAL_RECORDS { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }

    }

    public class HighBalanceReportReq : BaseModel
    {
        public string SEARCH_STRING { get; set; }
        public string SORT_ORDER { get; set; }
        public string SORT_BY { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
    }


    public class InterfaceLogReportReq : BaseModel
    {
        public string SEARCH_STRING { get; set; }
        public string SORT_ORDER { get; set; }
        public string SORT_BY { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string STATUS { get; set; }
        public string Application { get; set; }
        public string TYPE { get; set; }
        public string DateFromInStringDOS { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string DateToInStringDOS { get; set; }
        public DateTime? DATE_TO { get; set; }
        public int TIME_FRAME { get; set; }

    }



    public class InterfaceLogReportRes : BaseModel
    {

        public long FOX_INTERFACE_LOG_ID { get; set; }
        public int ROW { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string PATIENT_ACCOUNT_str { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string MRN { get; set; }
        public string PATIENT_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string Created_Date_Str { get; set; }
        public string ERROR { get; set; }
        public string APPLICATION { get; set; }
        public string ACK { get; set; }
        public string TYPE { get; set; }
        public bool? IS_ERROR { get; set; }
        public string LOG_MESSAGE { get; set; }
     
        public bool? IS_INCOMMING { get; set; }
        public bool? IS_OUTGOING { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public int SUCCESSFUL_INTERFACED { get; set; }
        public int FAILED_INTERFACED { get; set; }
        
    }
    public class PHRReportReq : BaseModel
    {
        public string SEARCH_STRING { get; set; }
        public string SORT_ORDER { get; set; }
        public string SORT_BY { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string Invitation_STATUS { get; set; }
        public string Patient_STATUS { get; set; }
    }

    public class PHRReportRes : BaseModel
    {
        public long? USER_ID { get; set; }
        public string USER_PHONE { get; set; }
        public int ROW { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string PATIENT_ACCOUNT_str { get; set; }
        public string MRN { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER { get; set; }
        public string EMAIL { get; set; }
        public string REGION { get; set; }
        public string INVITATION_STATUS { get; set; }
        public string PATIENT_STATUS { get; set; }
        public Nullable<bool> DELETED { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public int Response_Awaited { get; set; }
        public int Active { get; set; }
        public bool IS_ACTIVE { get; set; }
    }
    public class PHRUserLastLoginReponse : BaseModel
    {
        public int ROW { get; set; }
        public long USER_ID { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public string MRN { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public long PRACTICE_CODE { get; set; }
        public Nullable<DateTime> LAST_LOGIN_DATE { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }
    public class PHRUserLastLoginRequest : BaseModel
    {
        public DateTime? DATE_FROM { get; set; }
        public string DATE_FROM_STR { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public int TIME_FRAME { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
    }
}