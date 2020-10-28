using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FOX.DataModels.Models.FoxPHD
{
    public class CallDetailsSearchRequest
    {
        public DateTime? CALL_DATE_FROM { get; set; }
        public string CALL_DATE_FROM_STR { get; set; }
        public DateTime? CALL_DATE_TO { get; set; }
        public string CALL_DATE_TO_STR { get; set; }
        public string CALL_ATTENDED_BY { get; set; }
        public string CALL_REASON { get; set; }
        public string MRN { get; set; }
        public string SEARCH_TEXT { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
    }
    public class PatientsSearchRequest
    {
        public string PATIENT_ACCOUNT { get; set; }
        public string MRN { get; set; }
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string SSN { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public string DATE_OF_BIRTH_STR { get; set; }
        public string HOME_PHONE { get; set; }
        public string WORK_PHONE { get; set; }
        public string CELL_PHONE { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
    }

    [Table("FOX_TBL_PHD_CALL_DETAILS")]
    public class PHDCallDetail : BaseModel
    {
        [Key]
        public long FOX_PHD_CALL_DETAILS_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                long _lg;
                PATIENT_ACCOUNT = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        public long PRACTICE_CODE { get; set; }
        public string MRN { get; set; }
        public string SSCM_CASE_ID { get; set; }

        public DateTime DOS { get; set; }
        [NotMapped]
        public string DOS_STR { get; set; }
        public DateTime CALL_DATE { get; set; }
        [NotMapped]
        public string CALL_DATE_STR { get; set; }
        public DateTime CALL_TIME { get; set; }
        [NotMapped]
        public string CALL_TIME_STR { get; set; }
        public string CALL_DURATION { get; set; }
        public string CALLER_NAME { get; set; }
        public string RELATIONSHIP { get; set; }
        public string CALL_SCENARIO { get; set; }
        [NotMapped]
        public string CALL_SCENARIO_NAME { get; set; }
        public string CALL_REASON { get; set; }
        [NotMapped]
        public string CALL_REASON_NAME { get; set; }
        public string CALL_DETAILS { get; set; }
        [NotMapped]
        public string CurrencyAmount { get; set; }
        public decimal? AMOUNT { get; set; }
        [NotMapped]
        public string FOLLOW_UP_DATE_STR { get; set; }
        public DateTime? FOLLOW_UP_DATE { get; set; }
        [NotMapped]
        public string DOCUMENT_TYPE_NAME { get; set; }
        public string REQUEST { get; set; }
        [NotMapped]
        public string REQUEST_NAME { get; set; }
        public string CALL_ATTENDED_BY { get; set; }
        [NotMapped]
        public string CALL_ATTENDED_BY_NAME { get; set; }
        public string INCOMING_CALL_NO { get; set; }
        public string PATIENT_EMAIL_ADDRESS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public bool IS_CALL_DETAIL_EDIT { get; set; }
        public string CURRENT_EXTENSION { get; set; }
        public string CALL_RECORDING_PATH { get; set; }
        public long? GENERAL_NOTE_ID { get; set; }
        [NotMapped]
        public bool IsRecordingMapped { get; set; }
        [NotMapped]
        public bool IsNewPatient { get; set; }
        public string PRIORITY { get; set; }
        [NotMapped]
        public bool IS_FOLLOW_UP { get; set; }
        [NotMapped]
        public HttpFileCollection ATTACHMENT { get; set; }
        [NotMapped]
        public string ATTACHMENT_PATH { get; set; }
        [NotMapped]
        public string firstName { get; set; }
        [NotMapped]
        public string lastName { get; set; }
        [NotMapped]
        public string DOCUMENT_TYPE { get; set; }
        [NotMapped]
        public string ATTACHMENT_NAME { get; set; }
        [NotMapped]
        public bool _IsSSCM { get; set; }
    }
    [Table("FOX_TBL_PHD_CALL_PATIENT_VERIFICATION")]
    public class PhdPatientVerification
    {
        [Key]
        public long FOX_PHD_CALL_PATIENT_VERIFICATION_ID { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                long _lg;
                PATIENT_ACCOUNT = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        public bool IS_PATIENT_ACCOUNT_VERIFIED { get; set; }
        public bool IS_PATIENT_MRN_VERIFIED { get; set; }
        public bool IS_PATIENT_LAST_NAME_VERIFIED { get; set; }
        public bool IS_PATIENT_FIRST_NAME_VERIFIED { get; set; }
        public bool IS_PATIENT_DOB_VERIFIED { get; set; }
        public bool IS_PATIENT_AGE_VERIFIED { get; set; }
        public bool IS_PATIENT_SSN_VERIFIED { get; set; }
        public bool IS_PATIENT_ADDRESS_VERIFIED { get; set; }
        public bool IS_PATIENT_EMAIL_ADDRESS_VERIFIED { get; set; }
        public bool IS_PATIENT_HOME_PHONE_VERIFIED { get; set; }
        public bool IS_PATIENT_CELL_PHONE_VERIFIED { get; set; }
        public DateTime LAST_VERIFIED_DATE { get; set; }
        [NotMapped]
        public string LAST_VERIFIED_DATE_STR { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
        //not mapped
        //---FOR DEMOGRAPHICS UPDATE
        [NotMapped]
        public string EMAIL_ADDRESS { get; set; }
        [NotMapped]
        public string NEW_EMAIL_ADDRESS { get; set; }
        [NotMapped]
        public string HOME_PHONE { get; set; }
        [NotMapped]
        public string NEW_HOME_PHONE { get; set; }
        [NotMapped]
        public string CELL_PHONE { get; set; }
        [NotMapped]
        public string NEW_CELL_PHONE { get; set; }
        //---FOR DEMOGRAPHICS UPDATE
    }
    [Table("FOX_TBL_PHD_CALL_SCENARIO")]
    public class PhdCallScenario : BaseModel
    {
        [Key]
        public int PHD_CALL_SCENARIO_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_PHD_CALL_CATEGORY")]
    public class PhdCallCategory : BaseModel
    {
        [Key]
        public long CALL_CATEGORY_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class SscmCaseDetail : BaseModel
    {
        public string CS_Rectified_Case_Type { get; set; }
        public string Priority_Level { get; set; }
        public string CS_Case_Status { get; set; }
        public string CS_CASE_NO { get; set; }
    }
    [Table("Maintenance")]
    public class Maintenance : BaseModel
    {
        [Key]
        public int Office_Id { get; set; }
        public long Claim_No { get; set; }
        public string rowguid { get; set; }
        public long Case_No { get; set; }
        public long Case_Response_id { get; set; }
        public string password_change_days { get; set; }
        public string Current_Version { get; set; }
        public int Formulary_Office_id { get; set; }
    }
    [Table("CS_Customer_Support_Info")]
    public class CSCustomerSupportInfo : BaseModel
    {
        [Key]
        public string CS_Case_No { get; set; }
        public string CS_Case_From_Rep { get; set; }
        public string CS_Case_Title { get; set; }
        public string CS_Case_Detail { get; set; }
        public Nullable<System.DateTime> CS_Received_DateTime { get; set; }
        public string CS_Mail_ID { get; set; }
        public string CS_User_Name { get; set; }
        public string CS_Case_Category { get; set; }
        public Nullable<System.DateTime> CS_Created_Date { get; set; }
        public Nullable<int> CS_Number_Of_Times_Called { get; set; }
        public string CS_Case_Type_ID { get; set; }
        public string CS_REPLY_MAIL_ID { get; set; }
        public string Priority_Level { get; set; }
        public char CS_Case_Subcategory { get; set; }
        public Nullable<bool> cs_deleted { get; set; }
        public string CS_Billing_Compliance_id { get; set; }
        public Nullable<int> CS_SUB_CATEGORY_ID { get; set; }
        public string cs_Task_comments { get; set; }
        public string cs_case_Task { get; set; }
        public Nullable<System.DateTime> cs_Task_date { get; set; }
        public string cs_Task_assigned { get; set; }
        public string cs_Task_remarks { get; set; }
        public Nullable<int> Cs_task_status { get; set; }
        public string cs_Task_Time { get; set; }
        public string cs_case_Resp_Department { get; set; }
        public Nullable<bool> CRMFOLLOWUP { get; set; }
        public Nullable<bool> TECHFOLLOWUP { get; set; }
        public Nullable<long> CS_Task_Category_ID { get; set; }
        public Nullable<long> CS_Sub_Task_Category_ID { get; set; }
        public string CLIENT { get; set; }
        public char CS_Case_PHRcategory { get; set; }
        public string Cs_Case_Type { get; set; }
        public string CS_Parent_Case_No { get; set; }
        public Nullable<bool> Appeal_Sent { get; set; }
        public string Appeal_Source { get; set; }
        public System.Guid rowguid { get; set; }
        public Nullable<bool> NotifyProvider { get; set; }
        public string CS_SMS_Status { get; set; }
        public string Provider_Cellno { get; set; }
    }
    [Table("CS_Case_Progress")]
    public class CSCaseProgress : BaseModel
    {
        [Key]
        public string CS_Case_No { get; set; }
        public string CS_Case_Assigned_To { get; set; }
        public string CS_Case_Assigned_By { get; set; }
        public Nullable<System.DateTime> CS_Assigned_datetime { get; set; }
        public string CS_Case_Status { get; set; }
        public string CS_User_Comments { get; set; }
        public string CS_Assigned_Person_Comments { get; set; }
        public string CS_Pri_Resp_Person { get; set; }
        public string CS_Reply_Mail_Subject { get; set; }
        public string CS_Reply_Mail_Text { get; set; }
        public string CS_Reply_Mail_To { get; set; }
        public string CS_Reply_Mail_From { get; set; }
        public Nullable<System.DateTime> CS_Reply_Date { get; set; }
        public Nullable<long> CS_Practice_Code { get; set; }
        public Nullable<long> CS_Provider_Code { get; set; }
        public string CS_Patient_Account { get; set; }
        public string CS_Claim_No { get; set; }
        public string CS_Contact_Number { get; set; }
        public Nullable<System.DateTime> CS_Call_Check_Date { get; set; }
        public Nullable<System.DateTime> CS_Call_Time { get; set; }
        public string CS_Created_By { get; set; }
        public Nullable<System.DateTime> CS_Created_Date { get; set; }
        public string CS_Modified_By { get; set; }
        public Nullable<System.DateTime> CS_Modified_Date { get; set; }
        public string CS_Record_Updated_From { get; set; }
        public string CS_Insurance_Code { get; set; }
        public Nullable<System.DateTime> CS_Response_Date { get; set; }
        public string CS_Rep_Phone { get; set; }
        public Nullable<int> CS_Rep_Phone_Type { get; set; }
        public string CS_Rep_Email { get; set; }
        public string CS_Reply_Mail_CC { get; set; }
        public string CS_Reply_Mail_BCC { get; set; }
        public System.Guid rowguid { get; set; }
        public Nullable<bool> cs_deleted { get; set; }
        public string Cs_Send_Mail_To { get; set; }
        public Nullable<bool> Cs_is_Send { get; set; }
        public Nullable<System.DateTime> Cs_Case_Reopen_Date { get; set; }
        public Nullable<bool> Show_on_Web { get; set; }
        public string New_Message { get; set; }
        public string Cs_Authorize { get; set; }
        public Nullable<short> Cs_Web_Aging { get; set; }
        public Nullable<bool> CS_BILLING_COMPLIANCE { get; set; }
        public string VERIFIED_BY { get; set; }
        public string CS_IS_COMPLAIN { get; set; }
        public string CS_SubCategory { get; set; }
        public string CS_Rectified_Case_Type { get; set; }
        public string Problem_Type { get; set; }
        public Nullable<int> CS_FOLDER_ID { get; set; }
        public string cs_Case_remarks { get; set; }
        public Nullable<bool> isNotify { get; set; }
        public string Complaint_Status { get; set; }
        public string COMPLAINT_RESPONSIBLE { get; set; }
        public string COMPLAINT_RESPONSIBLE_REASON { get; set; }
        public string COMPLAINT_VALIDITY { get; set; }
    }
    [Table("CS_Case_History")]
    public class CSCaseHistory : BaseModel
    {
        [Key]
        public long CS_Track_ID { get; set; }
        public string CS_Case_No { get; set; }
        public string CS_Case_Assigned_To { get; set; }
        public string CS_Case_Assigned_By { get; set; }
        public Nullable<System.DateTime> CS_Assigned_datetime { get; set; }
        public string CS_Case_Status { get; set; }
        public string CS_User_Comments { get; set; }
        public string CS_Assigned_Person_Comments { get; set; }
        public string CS_Pri_Resp_Person { get; set; }
        public string CS_Reply_Mail_Subject { get; set; }
        public string CS_Reply_Mail_Text { get; set; }
        public string CS_Reply_Mail_To { get; set; }
        public string CS_Reply_Mail_From { get; set; }
        public Nullable<System.DateTime> CS_Reply_Date { get; set; }
        public Nullable<long> CS_Practice_Code { get; set; }
        public Nullable<long> CS_Provider_Code { get; set; }
        public string CS_Patient_Account { get; set; }
        public string CS_Claim_No { get; set; }
        public string CS_Contact_Number { get; set; }
        public Nullable<System.DateTime> CS_Call_Check_Date { get; set; }
        public Nullable<System.DateTime> CS_Call_Time { get; set; }
        public Nullable<System.DateTime> CS_Moved_Date { get; set; }
        public string CS_Modified_By { get; set; }
        public Nullable<System.DateTime> CS_Modified_Date { get; set; }
        public string CS_Record_Updated_From { get; set; }
        public string CS_Insurance_Code { get; set; }
        public Nullable<System.DateTime> CS_Response_Date { get; set; }
        public string CS_Rep_Phone { get; set; }
        public Nullable<int> CS_Rep_Phone_Type { get; set; }
        public string CS_Rep_Email { get; set; }
        public string CS_Reply_Mail_CC { get; set; }
        public string CS_Reply_Mail_BCC { get; set; }
        public System.Guid rowguid { get; set; }
        public Nullable<bool> cs_deleted { get; set; }
        public string New_Message { get; set; }
        public Nullable<int> CS_SUB_CATEGORY_ID { get; set; }
        public string Complaint_Status { get; set; }
        public string COMPLAINT_RESPONSIBLE { get; set; }
        public string COMPLAINT_RESPONSIBLE_REASON { get; set; }
        public string COMPLAINT_VALIDITY { get; set; }
    }

    [Table("FOX_TBL_PHD_CALL_REASON")]
    public class PhdCallReason : BaseModel
    {
        [Key]
        public int PHD_CALL_REASON_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_PHD_CALL_REQUEST")]
    public class PhdCallRequest : BaseModel
    {
        [Key]
        public int PHD_CALL_REQUEST_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class FoxApplicationUsersViewModel
    {
        public long USER_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string USER_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string CURRENT_USER_ID { get; set; }
        public string CURRENT_USER_NAME { get; set; }
        public string FullName { get; set; }
    }
    public class FoxUserDetails
    {
        public long USER_ID { get; set; }
        public string InternalUserID { get; set; }
        public string User_FName { get; set; }
        public string User_LName { get; set; }
        public string EMAIL { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
    }

    public class ExportAdvancedDailyReport
    {
        public int ROW { get; set; }
        public string CALL_DATE_STR { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public string NAME { get; set; }
        public string CALL_DETAILS { get; set; }
        public decimal? AMOUNT { get; set; }
        public string CurrencyAmount { get; set; }
        public string CALL_USER_ID { get; set; }
    }
    public class DropdownLists
    {
        public List<PhdCallScenario> PhdCallScenarios { get; set; }
        public List<PhdCallReason> PhdCallReasons { get; set; }
        public List<PhdCallRequest> PhdCallRequests { get; set; }
        public List<FoxApplicationUsersViewModel> foxApplicationUsersViewModel { get; set; }
    }
    [Table("FOX_TBL_PHD_CALL_UNMAPPED")]
    public class PHDUnmappedCalls : BaseModel
    {
        [Key]
        public long? PHD_CALL_UNMAPPED_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public DateTime? CALL_DATE { get; set; }
        public DateTime? CALL_START_TIME { get; set; }
        public DateTime? CALL_END_TIME { get; set; }
        public string CALL_NO { get; set; }
        public string CALL_BY { get; set; }
        public bool IS_INCOMING { get; set; }
        public string OFFICE_NAME { get; set; }
        public string EXTENSION { get; set; }
        public string CALL_RECORDING_PATH { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string LENGTH { get; set; }
    }
    public class UnmappedCallsSearchRequest : BaseModel
    {
        public string CALL_NO { get; set; }
        public DateTime? CALL_DATE { get; set; }
        public string CALL_DATE_STR { get; set; }
}
}