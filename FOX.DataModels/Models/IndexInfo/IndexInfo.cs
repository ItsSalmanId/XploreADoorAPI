using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Settings.ReferralSource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using FOX.DataModels.Models.GroupsModel;

namespace FOX.DataModels.Models.IndexInfo
{
    public class IndexInfo
    {

    }

    [Table("PATIENT")]
    public class IndexPatReq : BaseModel
    {
        [Key]
        public long Patient_Account { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string SSN { get; set; }
        public string Chart_Id { get; set; }
        public string Gender { get; set; }
        public Nullable<DateTime> Date_Of_Birth { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<DateTime> MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
        [NotMapped]
        public string Date_Of_Birth_In_String { get; set; }
    }

    public class getPatientReq : BaseModel
    {

        public long Patient_Account { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string SSN { get; set; }
        public string Chart_Id { get; set; }
        public string Gender { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public long Practice_Code { get; set; }
        public string Date_Of_Birth_In_String { get; set; }
        public long WORK_ID { get; set; }
        public bool INCLUDE_ALIAS { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
    }

    public class IndexPatRes
    {
        public long ROW { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public string Date_Of_Birth { get; set; }
        public string MRN { get; set; }
        public string Patient_Account { get; set; }
        public string HomeAddress { get; set; }
        public string Cell_Phone { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string STATE { get; set; }
        //public string Cell_PhoneFormat
        //{
        //    get
        //    {
        //        if (!string.IsNullOrWhiteSpace(Cell_Phone))
        //        {
        //            Cell_Phone = Cell_Phone.Replace("+1", "");
        //            long tempCell_Phone;
        //            bool Islong = long.TryParse(Cell_Phone, out tempCell_Phone);
        //            Cell_Phone = Islong ? String.Format("{0:(###) ###-####}", tempCell_Phone) : "";

        //        }
        //        return Cell_Phone;
        //    }
        //}
        public string Business_Phone { get; set; }
        //public string Business_PhoneFormat
        //{
        //    get
        //    {
        //        if (!string.IsNullOrWhiteSpace(Business_Phone))
        //        {
        //            Business_Phone = Business_Phone.Replace("+1", "");
        //            long temp;
        //            bool Islong = long.TryParse(Business_Phone, out temp);
        //            Cell_Phone = Islong ? String.Format("{0:(###) ###-####}", temp) : "";
        //        }
        //        return Business_Phone;
        //    }
        //}
        public string Home_Phone { get; set; }
        //public string Home_PhoneFormat
        //{
        //    get
        //    {
        //        if (!string.IsNullOrWhiteSpace(Home_Phone))
        //        {
        //            Home_Phone = Home_Phone.Replace("+1", "");
        //            long temp;
        //            bool Islong = long.TryParse(Home_Phone, out temp);
        //            Cell_Phone = Islong ? String.Format("{0:(###) ###-####}", temp) : "";
        //        }
        //        return Home_Phone;
        //    }
        //}
        public string PrimaryInsurance { get; set; }
        public string Email_Address { get; set; }
        public string Title { get; set; }
        public string Fax_Number { get; set; }
        public List<FoxDocumentType> FoxDocumentTypeList { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public bool IS_PATIENT_INTERFACE_SYNCED { get; set; }
        public bool IS_WORK_ORDER_INTERFACE_SYNCED { get; set; }
        public bool IS_PATIENT_OLD_OR_SYNCED { get; set; }
        public decimal? Patient_Balance { get; set; }
        public string FINANCIAL_CLASS_NAME { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        public bool Is_Patient_Alias { get; set; }
        public long? PATIENT_ALIAS_ID { get; set; }
        public string ALIAS_TRACKING_NUMBER { get; set; }
        public string RT_ALIAS_TRACKING { get; set; }
        public string FIRST_NAME_ALIAS { get; set; }
        public string MIDDLE_INITIALS_ALIAS { get; set; }
        public string LAST_NAME_ALIAS { get; set; }
        [NotMapped]
        public int? TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int? TOTAL_RECORDS { get; set; }
        public string NPI { get; set; }
        public long? SOURCE_ID { get; set; }
    }
    public class PatLastORS
    {
        public string NPI { get; set; }
        public long? SOURCE_ID { get; set; }
    }

        public class GetDiagnosisReq
    {
        public string Diag_Code { get; set; }
    }

    public class GetDiagnosisRes
    {
        public string DIAG_CODE { get; set; }
        public string DIAG_DESC { get; set; }
    }

    public class SmartDiagnosisReq : BaseModel
    {
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string OPTIONSEARCH { get; set; }
        public string PRACTICE_CODE { get; set; }
        public string PROVIDER_CODE { get; set; }
        public string CODE_TYPE { get; set; }
        public string CATEGORY { get; set; }
        public string DOS { get; set; }
        public long PracticeCode { get; set; }
    }

    public class GetSmartDiagnosisRes
    {
        public string CODE { get; set; }
        public string CODE_DESCRIPTION { get; set; }
        public string CODE_TYPE { get; set; }
        public string DIAG_EFFECTIVE_DATE { get; set; }
        public string DIAG_EXPIRY_DATE { get; set; }
        public int? Is_expired { get; set; }
        public string NAME
        {
            get
            {
                return $"[{CODE}] {CODE_DESCRIPTION} ";
            }
        }

    }

    public class GetSmartProceduresRes
    {

        public string Proc_Code { get; set; }
        public string proc_pos_code { get; set; }
        public DateTime? cpt_deleted_expiry_date { get; set; }
        public string LONG_DESCRIPTION { get; set; }
        public string proc_description { get; set; }
        public int? Is_expired { get; set; }
        public string Name
        {
            get
            {
                return $"[{Proc_Code}] {proc_description}";
            }
        }
    }

    public class GetProceduresReq
    {
        public string Proc_Code { get; set; }
    }

    public class SmartProceduresReq : BaseModel
    {

        public string PROCCODE { get; set; }
        public string PROCDESCRIPTION { get; set; }
        public string CATEGORY { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long PROVIDER_CODE { get; set; }
        public long LOCATION_CODE { get; set; }
        public string DOS { get; set; }
        public long PracticeCode { get; set; }
    }

    public class GetProceduresRes
    {
        public string PROC_CODE { get; set; }
        public string CPT_DESC { get; set; }

    }

    //[Table("FOX_TBL_SENDER")]
    //public class FOX_TBL_SENDER
    //{
    //    [Key]
    //    public long SENDER_ID { get; set; }
    //    public string SENDER_NAME { get; set; }
    //    public long PRACTICE_CODE { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public DateTime CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public DateTime MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }
    //}

    public class SourceSenderRes
    {
        public long SENDER_ID { get; set; }
        public string SENDER_NAME { get; set; }
    }

    public class FOX_TBL_NOTES_HISTORY : BaseModel
    {
        [NotMapped]
        public int ROW { get; set; }
        [Key]
        public long NOTE_ID { get; set; }
        public long WORK_ID { get; set; }
        public string CREATED_BY { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string NOTE_DESC { get; set; }
        public long PRACTICE_CODE { get; set; }

        public string CREATED_DATE { get; set; }

        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class FOX_GET_NOTES_HISTORY
    {
        public long NOTE_ID { get; set; }
        public long? WORK_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string NOTE_DESC { get; set; }
        public string MODIFIED_BY { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

    }

    public class FOX_TBL_PATIENT_DIAGNOSIS : BaseModel
    {
        [Key]
        public long PAT_DIAG_ID { get; set; }
        public long WORK_ID { get; set; }
        public string DIAG_CODE { get; set; }
        public string DIAG_DESC { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class FOX_TBL_PATIENT_PROCEDURE : BaseModel
    {
        [Key]
        public long PAT_PROC_ID { get; set; }
        public long WORK_ID { get; set; }
        public string PROC_CODE { get; set; }
        public string SPECIALITY_PROGRAM { get; set; }
        public string CPT_DESC { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    //public class FOX_TBL_WORK_QUEUE
    //{
    //    [Key]
    //    public long WORK_ID { get; set; }
    //    public string UNIQUE_ID { get; set; }
    //    public Nullable<long> PRACTICE_CODE { get; set; }
    //    public Nullable<long> PATIENT_ACCOUNT { get; set; }
    //    public string SORCE_TYPE { get; set; }
    //    public string SORCE_NAME { get; set; }
    //    public string WORK_STATUS { get; set; }
    //    public Nullable<System.DateTime> RECEIVE_DATE { get; set; }
    //    public Nullable<int> TOTAL_PAGES { get; set; }
    //    public Nullable<int> NO_OF_SPLITS { get; set; }
    //    public string FILE_PATH { get; set; }
    //    public string ASSIGNED_TO { get; set; }
    //    public string ASSIGNED_BY { get; set; }
    //    public Nullable<System.DateTime> ASSIGNED_DATE { get; set; }
    //    public string COMPLETED_BY { get; set; }
    //    public Nullable<System.DateTime> COMPLETED_DATE { get; set; }
    //    public string INDEXED_BY { get; set; }
    //    public Nullable<System.DateTime> INDEXED_DATE { get; set; }
    //    public Nullable<long> DOCUMENT_TYPE { get; set; }
    //    public Nullable<long> SENDER_ID { get; set; }
    //    public string FACILITY_NAME { get; set; }
    //    public Nullable<long> DEPARTMENT_ID { get; set; }
    //    public bool IS_EMERGENCY_ORDER { get; set; }
    //    public string REASON_FOR_VISIT { get; set; }
    //    public string ACCOUNT_NUMBER { get; set; }
    //    public string UNIT_CASE_NO { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public System.DateTime CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public System.DateTime MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }
    //    public Nullable<System.DateTime> TRANSFER_DATE { get; set; }
    //    public bool supervisor_status { get; set; }
    //    public string FAX_ID { get; set; }
    //    public Nullable<System.DateTime> INDEXER_ASSIGN_DATE { get; set; }
    //    public Nullable<System.DateTime> AGENT_ASSIGN_DATE { get; set; }
    //    public Nullable<bool> IS_VERIFIED_BY_RECIPIENT { get; set; }
    //    public Nullable<bool> IsSigned { get; set; }
    //    public Nullable<long> SignedBy { get; set; }
    //    public Nullable<long> FOX_TBL_SENDER_TYPE_ID { get; set; }
    //    public Nullable<long> FOX_TBL_SENDER_NAME_ID { get; set; }
    //    public string GuestID { get; set; }

    //    [NotMapped]
    //    public bool IsCompleted { get; set; }
    //    [NotMapped]
    //    public bool IsSaved { get; set; }
    //}

    //public class FOX_TBL_WORK_QUEUE
    //{
    //    [Key]
    //    public long WORK_ID { get; set; }
    //    public long PRACTICE_CODE { get; set; }
    //    public string COMPLETED_BY { get; set; }
    //    public DateTime COMPLETED_DATE { get; set; }
    //    public string INDEXED_BY { get; set; }
    //    public DateTime INDEXED_DATE { get; set; }
    //    public string WORK_STATUS { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public DateTime CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public DateTime MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }
    //    public long DOCUMENT_TYPE { get; set; }
    //    public long SENDER_ID { get; set; }
    //    public string FACILITY_NAME { get; set; }
    //    public long DEPARTMENT_ID { get; set; }
    //    public bool IS_EMERGENCY_ORDER { get; set; }
    //    public string REASON_FOR_VISIT { get; set; }
    //    public string ACCOUNT_NUMBER { get; set; }
    //    public string UNIT_CASE_NO { get; set; }

    //    [NotMapped]
    //    public bool IsCompleted { get; set; }
    //    [NotMapped]
    //    public bool IsSaved { get; set; }
    //}

    public class GETtAll_IndexifoRes
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public bool? IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public List<FOX_TBL_PATIENT_PROCEDURE> CPTS { get; set; }
        public List<FOX_TBL_PATIENT_DIAGNOSIS> DIAGNOSIS { get; set; }
        public List<FilePath> FilePaths { get; set; }
        public List<FOX_TBL_PATIENT_DOCUMENTS> Documents { get; set; }
        public string FACILITY_NAME { get; set; }
        public long? FACILITY_ID { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_NAME_FaxFormat
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SORCE_NAME) && SORCE_NAME.Contains("+1"))
                {
                    SORCE_NAME = SORCE_NAME.Replace("+1", "");
                    if (!string.IsNullOrEmpty(SORCE_NAME))
                    {
                        SORCE_NAME = String.Format("{0:(###) ###-####}", Int64.Parse(SORCE_NAME));
                    }
                }
                if (!string.IsNullOrWhiteSpace(SORCE_NAME) && (SORCE_NAME.ToLower().Equals("anonymous") || Regex.IsMatch(SORCE_NAME, @"^[a-zA-Z0-9]+$")))
                {
                    SORCE_NAME = "";
                }
                return SORCE_NAME;
            }
        }
        public string SORCE_TYPE { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public long? SENDER_ID { get; set; }
        public string SENDER_NAME { get; set; }
        public string Last_Name { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string SSN { get; set; }
        public string Chart_Id { get; set; }
        public string Gender { get; set; }
        public string Email_Address { get; set; }
        public string Date_Of_Birth { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string Patient_Account_Str
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
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public long? FOX_TBL_SENDER_NAME_ID { get; set; }
        public bool? Is_Temp_Patient { get; set; }
        public GuestLogin Guest_Login { get; set; }
        public string REASON_FOR_THE_URGENCY { get; set; }
        public bool? IS_POST_ACUTE { get; set; }
        public string EXPECTED_DISCHARGE_DATE { get; set; }
        public bool? IS_EVALUATE_TREAT { get; set; }
        public string HEALTH_NAME { get; set; }
        public string HEALTH_NUMBER { get; set; }

        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool? IS_VERBAL_ORDER { get; set; }
        public long? VO_ON_BEHALF_OF { get; set; }
        public string VO_RECIEVED_BY { get; set; }
        public DateTime? VO_DATE_TIME { get; set; }
        public ReferralSource OnBehalfOfSource { get; set; }
        public string Cell_Phone { get; set; }
        public string Business_Phone { get; set; }
        public string Home_Phone { get; set; }
        public string Fax_Number { get; set; }
        public string Title { get; set; }
        public List<FoxDocumentType> FoxDocumentTypeList { get; set; }

        public string WORK_STATUS { get; set; }

        public bool IS_PATIENT_INTERFACE_SYNCED { get; set; }

        public bool IS_WORK_ORDER_INTERFACE_SYNCED { get; set; }
        public bool IS_PATIENT_OLD_OR_SYNCED { get; set; }

        public decimal? Patient_Balance { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        [NotMapped]
        public bool IS_STRATEGIC { get; set; }
        public string Zip_File_Path { get; set; }
        public string FINANCIAL_CLASS_NAME { get; set; }
        public string RFO_Type { get; set; }
        public long? OCR_STATUS_ID { get; set; }
        public string OCR_STATUS { get; set; }
        public decimal fileSize { get; set; } //Nouman
        public bool IS_SYNC { get; set; }
        public bool IS_Error { get; set; }
        public string ERROR_MSG { get; set; }
        public string SENDER_TYPE { get; set; }
        public long? FOX_SOURCE_CATEGORY_ID { get; set; }
        public bool? IS_OCR { get; set; }
    }

    public class Index_infoReq : BaseModel
    {
        public long? WORK_ID { get; set; }
    }

    public class FilePath
    {
        public bool Checked { get; set; }
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string file_path1 { get; set; }

    }

    public class FOX_TBL_PATIENT_DOCUMENTS : BaseModel
    {
        [Key]
        public long PAT_DOC_ID { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public Nullable<long> WORK_ID { get; set; }
        public string FILE_PATH { get; set; }
        public Nullable<long> PROC_CODE { get; set; }
        public string COMENTS { get; set; }
        public string FILE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class PatientListResponse
    {
        public long ROW { get; set; }
        public string FINANCIAL_CLASS_NAME { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        public long? Patient_Account { get; set; }
        public string MRN { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string FIRST_NAME_ALIAS { get; set; }
        public string LAST_NAME_ALIAS { get; set; }
        public string MIDDLE_INITIALS_ALIAS { get; set; }
        public bool? Is_Patient_Alias { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int? TOTAL_RECORDS { get; set; }
    }
    public class Documents
    {
        [Key]
        public long PAT_DOC_ID { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public Nullable<long> WORK_ID { get; set; }
        public string FILE_PATH { get; set; }
        public Nullable<long> PROC_CODE { get; set; }
        public string COMENTS { get; set; }
        public string FILE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class SmartReq : BaseModel
    {
        public string SEARCHVALUE { get; set; }
        public long PracticeCode { get; set; }
        public bool Is_From_RFO { get; set; }

    }

    public class SmartLocationRes
    {
        public long LOC_ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }

        public string Template_1
        {
            get
            {
                return $"[{CODE}] {this.NAME}";
            }
        }

        public string Template
        {
            get
            {
                if (!string.IsNullOrEmpty(Address))

                {
                    string zipcode = string.IsNullOrEmpty(Zip) ? "" : Zip.Length > 5 ? Zip.Substring(0, 5) + '-' + Zip.Substring(5) : Zip;
                    return $@"<p> [{CODE}] {this.NAME} </br> {Address}, {City},</br> {State} {zipcode} </p> ";
                    //return $"<p> [{CODE}] {this.NAME} </br> {Address}, {City},</br> {State} {} </p> ";
                }
                else
                {
                    return $"<p> [{CODE}] {this.NAME} </p> ";
                }
            }
        }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string REGION { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        //newly created prop
        public string Email_Address { get; set; }
        public string Work_Phone { get; set; }
        public string Cell_Phone { get; set; }
        public string Fax { get; set; }
        public long? FACILITY_TYPE_ID { get; set; }
    }

    //[Table("FOX_TBL_ORDERING_REF_SOURCE")]
    //public class FOX_TBL_ORDERING_REF_SOURCE : BaseModel
    //{

    //[Key]
    //public long SOURCE_ID { get; set; }
    //public long? PRACTICE_CODE { get; set; }
    //public string CODE { get; set; }
    //public string FIRST_NAME { get; set; }
    //public string LAST_NAME { get; set; }
    //public string TITLE { get; set; }
    //public string ADDRESS { get; set; }
    //public string ADDRESS_2 { get; set; }
    //public string CITY { get; set; }
    //public string STATE { get; set; }
    //public string ZIP { get; set; }
    //public string PHONE { get; set; }
    //public string FAX { get; set; }
    //public string REFERRAL_REGION { get; set; }
    //public string NPI { get; set; }
    //public string ORGANIZATION { get; set; }
    //public string ACO { get; set; }
    //public string CREATED_BY { get; set; }
    //public DateTime? CREATED_DATE { get; set; }
    //public string MODIFIED_BY { get; set; }
    //public DateTime? MODIFIED_DATE { get; set; }
    //public bool DELETED { get; set; }
    //[NotMapped]
    //public string Practice_Name { get; set; }
    //}

    public class SmartOrderSource
    {
        public long SOURCE_ID { get; set; }
        public string Name { get; set; }
        public string CODE { get; set; }
        public string ADDRESS { get; set; }
        public string ZIP { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string NPI { get; set; }
        public long? INACTIVE_REASON_ID { get; set; }
        public string NOTES { get; set; }
        public string Template
        {
            get
            {
                //return $"<p><b>[{ CODE.ToTitleCase() }] {LAST_NAME.ToTitleCase()}, {FIRST_NAME.ToTitleCase()} </b><br /> { ADDRESS.ToTitleCase() }, { CITY.ToTitleCase() }, {STATE.ToTitleCase() }<br />Phone: {PHONE}</p>";
                return $"<p>[{ CODE.ToTitleCase() }] {LAST_NAME.ToTitleCase()}, {FIRST_NAME.ToTitleCase()}&nbsp;&nbsp;&nbsp;NPI: {NPI}<br /> { ADDRESS.ToTitleCase() }, { CITY.ToTitleCase() }, {STATE.ToTitleCase() }<br />Phone: {PHONE.ApplyPhoneMask()}</p>";
            }
        }   

        public bool IsRed
        {
            get
            {
                return INACTIVE_REASON_ID.HasValue ? true : false;
            }
        }
        public string REFERRAL_SOURCE_ACO { get; set; }
        public long? REFERRAL_SOURCE_ACO_ID { get; set; }
        public string ACO_NAME { get; set; }
        public long? ROLE_ID { get; set; }
        public string USER_TYPE { get; set; }
        public long? ACO { get; set; }
        public string ACO_NAME_TEXT { get; set; }
        public string ACO_DESCRIPTION { get; set; }
        public string ACO_CODE { get; set; }
        public long? HOSPITAL { get; set; }
        public string HOSPITAL_NAME_TEXT { get; set; }
        public string HOSPITAL_CODE { get; set; }
        public string HOSPITAL_DESCRIPTION { get; set; }
        public long? SNF { get; set; }
        public string SNF_NAME_TEXT { get; set; }
        public string SNF_CODE { get; set; }
        public string SNF_DESCRIPTION { get; set; }
        public long? HHH { get; set; }
        public string HHH_NAME_TEXT { get; set; }
        public string HHH_CODE { get; set; }
        public string HHH_DESCRIPTION { get; set; }
        public string Email { get; set; }
        public string REFERRAL_REGION { get; set; }
    }

    public class SmartRefRegion
    {
        public long REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string Template { get { return $"[{REFERRAL_REGION_CODE}] {REFERRAL_REGION_NAME.ToTitleCase()}"; } }
    }

    public class AnalaysisReportRes
    {
        public long ROW { get; set; }
        public string RECEIVE_DATE { get; set; }
        public int? ZEROTO_FIFTEEN { get; set; }
        public int? SIXTEENTO_THIRTY { get; set; }
        public int? THIRTYONETO_FOURTFIVE { get; set; }
        public int? FOURTYSIXTO_SIXTY { get; set; }
        public int? GREATERTHAN_HOUR { get; set; }
        public int? GREATERTHAN_TWOHOUR { get; set; }
        public int? GRANDTOTAL { get; set; }
        public long? TOTAL_RECORDS { get; set; }
        public long? TOTAL_PAGE { get; set; }
        public string INDEXER_ASSIGNMENT_TOTAL_TIME { get; set; }
        public string INDEXER_TOTAL_TIME { get; set; }
        public string AGENT_TOTAL_TIME { get; set; }
        public string SUPERVISOR_TOTAL_TIME { get; set; }
        public string TOTALTIME_INSECONDS_TOCOMPLETE { get; set; }


    }

    public class AnalaysisReportReq : BaseModel
    {
        public DateTime? DATEFROM { get; set; }
        public DateTime? DATETO { get; set; }
        public string TIMEFRAME { get; set; }
        public string DATEFROM_In_String { get; set; }
        public string DATETO_In_String { get; set; }
        public long PAGEINDEX { get; set; }
        public string CalledFrom { get; set; }
        public long PAGESIZE { get; set; }
        public string BUSINESS_HOURS9A { get; set; }
        public string BUSINESS_HOURS5A { get; set; }
        public string SATURDAYSA { get; set; }
        public string SUNDAYSA { get; set; }
        public bool? EXCLUDEWEEKEND { get; set; }
    }

    public class SlotAnalysisReq : BaseModel
    {
        public DateTime? DATE { get; set; }
        public int? START_VALUE { get; set; }
        public int? END_VALUE { get; set; }
        public string BUSINESS_HOURS8A { get; set; }
        public string BUSINESS_HOURS5A { get; set; }
        public string SATURDAYSA { get; set; }
        public string SUNDAYSA { get; set; }
        public bool? EXCLUDEWEEKEND { get; set; }
    }

    public class SlotAnalysisRes
    {
        [Key]
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_NAME_FaxFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(SORCE_NAME) && (SORCE_NAME.ToLower().Equals("anonymous") || Regex.IsMatch(SORCE_NAME, @"^[a-zA-Z0-9_]+$")))
                {
                    SORCE_NAME = "";
                }
                if (!string.IsNullOrWhiteSpace(SORCE_NAME) && SORCE_NAME.Contains("+1"))
                {
                    SORCE_NAME = SORCE_NAME.Replace("+1", "");
                    if (!string.IsNullOrEmpty(SORCE_NAME))
                    {
                        SORCE_NAME = String.Format("{0:(###) ###-####}", Int64.Parse(SORCE_NAME));
                    }
                }
                return SORCE_NAME;
            }
        }
        public string WORK_STATUS { get; set; }
        public string RECEIVE_DATE { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public string ASSIGNED_DATE { get; set; }
        public string COMPLETED_BY { get; set; }
        public string COMPLETED_DATE { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public long? SENDER_ID { get; set; }
        public string FACILITY_NAME { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool IsSaved { get; set; }
        public string INDEXED_BY { get; set; }
        public string INDEXED_DATE { get; set; }
        public string FAX_ID { get; set; }
        public bool supervisor_status { get; set; }
        public string INDEXER_ASSIGN_DATE { get; set; }
        public string AGENT_ASSIGN_DATE { get; set; }
        public string TIME_TO_COMPLETE { get; set; }
        public long? TOTAL_SECONDSS { get; set; }
        public long? TOTAL_MINS { get; set; }
        public string SUPERVISOR_ASSIGN_DATE { get; set; }
        public string INDEXER_TOTAL_TIME { get; set; }
        public string AGENT_TOTAL_TIME { get; set; }
        public string SUPERVISOR_TOTAL_TIME { get; set; }
    }

    public class AccessTokenInfo
    {
        public string userName { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public DateTime expires_in { get; set; }
        public DateTime issued { get; set; }
        public DateTime expires { get; set; }
    }

    public class RespWSGetAccessToken
    {
        public string Status { get; set; }
        public string Details { get; set; }
        public AccessTokenInfo AccessTokenInfo { get; set; }
    }

    public class ReqWSSendNotification
    {
        public string GuestID { get; set; }
        public string Message { get; set; }
    }

    [Table("WS_TBL_FOX_GUEST_LOGIN")]
    public class GuestLogin
    {
        [Key]
        public long? GuestID { get; set; }
        public string pincode { get; set; }
        public Nullable<DateTime> created_Date { get; set; }
        public string Phone { get; set; }
        public string Email_Address { get; set; }
        public Nullable<int> login_Count { get; set; }
        public Nullable<long> WORK_ID { get; set; }
        public string DeviceToken { get; set; }
        public string First_Name { get; set; }
        public string Last_name { get; set; }
        public string sender_type { get; set; }
        public string NPI { get; set; }
        public string sender_typeID { get; set; }
        public string SenderEmail { get; set; }
    }

    public class QRCodeModel : BaseModel
    {
        public long WORK_ID { get; set; }
        public string SignPath { get; set; }
        public string ENCODED_IMAGE_BYTES { get; set; }
        public string SIGNATURE_IMAGE_BYTES { get; set; }
        public string AbsolutePath { get; set; }
    }

    public class EmailFaxToSender : BaseModel
    {
        public long work_id { get; set; }
        public string UNIQUE_ID { get; set; }
        public string SUBJECT { get; set; }
        public string BODY { get; set; }
        public string EMAIL { get; set; }
        public string FAX { get; set; }
        public bool IS_EMAIL { get; set; }
        public bool From_Index_Info { get; set; }
    }

    public class AttachmentData
    {
        public string FILE_PATH { get; set; }
        public string FILE_NAME { get; set; }
    }

    public class IndexInfoInitialData
    {
        public GETtAll_IndexifoRes IndexIfoRes { get; set; }
        public List<FOX_TBL_NOTES_HISTORY> NotesHistory { get; set; }
        public List<FOX_TBL_PATIENT_PROCEDURE> proceduresList { get; set; }
        public List<FOX_TBL_PATIENT_DIAGNOSIS> DiagnosisList { get; set; }
    }

    [Table("FOX_TBL_DOCUMENT_TYPE")]
    public class FoxDocumentType
    {
        [Key]
        public int DOCUMENT_TYPE_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string RT_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public bool IS_ONLINE_ORDER_LIST { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool DELETED { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    [Table("FOX_TBL_SPECIALITY_PROGRAM")]
    public class FoxSpecialityProgram
    {
        [Key]
        public long SPECIALITY_PROGRAM_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string PROGRAM_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class PatientInfoChecklist
    {
        public bool FIRSTNAME { get; set; }
        public bool LASTNAME { get; set; }
        public bool GENDER { get; set; }
        public bool DOB { get; set; }
        public bool HomePhone { get; set; }
        public bool CellPhone { get; set; }
        public bool PlaceofService { get; set; }
        public bool ContactPOA { get; set; }
        public bool PrimaryIns { get; set; }
        public bool SecondaryIns { get; set; }
        public bool TertiaryIns { get; set; }
        public bool PatientPay { get; set; }
        public bool CheckEligibilty { get; set; }
        public bool PriEligibilityChecked { get; set; }
        public bool SecEligibilityChecked { get; set; }
        public bool TerEligibilityChecked { get; set; }
    }

    public class pendingBalanceAmount
    {
        public decimal? Patient_Balance { get; set; }
        public decimal? Statement_Patient_Balance { get; set; }
        public string PROCESS_DATE { get; set; }
        public int NoOfDays { get; set; }
    }

    public class PreviousEmailInfo
    {
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string SEND_TO { get; set; }
        public string DESCRIPTION { get; set; }
    }
    public class ReferralPatientInfo
    {
        public OriginalQueue originalQueue { get; set; }
        public FOX.DataModels.Models.Patient.Patient patientData { get; set; }
        public string DocumentTypeName { get; set; }
        public string SenderName { get; set; }
        public ReferralSource referralSource { get; set; }
    }
    [Table("FOX_TBL_REFERRAL_SOURCE")]
    public class FOX_TBL_REFERRAL_SOURCE : BaseModel
    {
        [Key]
        public long FOX_SOURCE_CATEGORY_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public long ASSIGNED_TO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class ReferralSourceAndGroups: BaseModel
    {
        public List <FOX_TBL_REFERRAL_SOURCE> ReferralSource { get; set; }
        public List <GROUP> Groups { get; set; }
    }
    public class DuplicateReferralInfo
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string WORK_STATUS { get; set; }
        public string ORS { get; set; }
        public string SENDER_SOURCE { get; set; }
        public string RECEIVE_DATE { get; set; }
        public string DEPARTMENT_ID { get; set; }
    }
    public class checkDuplicateReferralRequest
    {
        public string splitedIDs { get; set; }
        public long workID { get; set; }
    }
}