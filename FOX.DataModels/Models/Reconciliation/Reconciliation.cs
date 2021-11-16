using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace FOX.DataModels.Models.Reconciliation
{
    [Table("FOX_TBL_RECONCILIATION_STATUS")]
    public class ReconciliationStatus
    {
        [Key]
        public int RECONCILIATION_STATUS_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string STATUS_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public bool Selected { get; set; }
    }

    [Table("FOX_TBL_RECONCILIATION_DEPOSIT_TYPE")]
    public class ReconciliationDepositType
    {
        [Key]
        public long DEPOSIT_TYPE_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string DEPOSIT_TYPE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public bool Selected { get; set; }
    }

    [Table("FOX_TBL_RECONCILIATION_CATEGORY")]
    public class ReconciliationCategory
    {
        [Key]
        public long CATEGORY_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public bool Selected { get; set; }
    }

    [Table("FOX_TBL_RECONCILIATION_FILES")]
    public class ReconciliationFiles
    {
        [Key]
        public long RECONCILIATION_FILE_ID { get; set; }
        public long? RECONCILIATION_ID { get; set; }
        public string RECONCILIATION_CATEGORY { get; set; }
        public string LOGO_PATH { get; set; }
        public string IMAGE_PATH { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_RECONCILIATION_CP")]
    public class ReconciliationCP : ICloneable
    {
        [Key]
        public long RECONCILIATION_CP_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long PRACTICE_CODE { get; set; }
        [NotMapped]
        public string STATUS_NAME { get; set; }
        public DateTime? DEPOSIT_DATE { get; set; }
        [NotMapped]
        public string DEPOSIT_TYPE_NAME { get; set; }
        [NotMapped]
        public string CATEGORY_NAME { get; set; }
        [NotMapped]
        public string INSURANCE_NAME { get; set; }
        public string CHECK_NO { get; set; }
        public decimal? AMOUNT { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        [NotMapped]
        public string ASSIGNED_TO_NAME { get; set; }
        public string LEDGER_NAME { get; set; }
        public decimal? AMOUNT_POSTED { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? AMOUNT_NOT_POSTED { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        [NotMapped]
        public string CREATED_BY_NAME { get; set; }
        public long? DEPOSIT_TYPE_ID { get; set; }
        public long? CATEGORY_ID { get; set; }
        //public long? FOX_TBL_INSURANCE_ID { get; set; }
        public string FOX_TBL_INSURANCE_NAME { get; set; }
        public int? RECONCILIATION_STATUS_ID { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string COMPLETED_BY { get; set; }
        public string LEDGER_PATH { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        //public string LEDGER_BASE64 { get; set; }

        public int? TOTAL_LEDGER_PAGES { get; set; }
        //---------------------------------------
        ////////****NOT MAPPED FIELDS****////////
        //---------------------------------------
        [NotMapped]
        public string DEPOSIT_DATE_STR { get; set; }
        [NotMapped]
        public string ASSIGNED_DATE_STR { get; set; }
        [NotMapped]
        public string COMPLETED_DATE_STR { get; set; }
        [NotMapped]
        public string COMPLETED_BY_NAME { get; set; }

        //---------***SUMMARY***----------
        [NotMapped]
        public decimal? TOTAL_AMOUNT { get; set; }
        [NotMapped]
        public decimal? TOTAL_POSTED_AMOUNT { get; set; }
        [NotMapped]
        public decimal? TOTAL_UNPOSTED_AMOUNT { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public Int64 RowId { get; set; }
        //---------***SUMMARY***----------
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        public string BATCH_NO { get; set; }
        public DateTime? DATE_POSTED { get; set; }
        [NotMapped]
        public string POSTED_DATE_STR { get; set; }
        public string REMARKS { get; set; }

        public long? REASON { get; set; }

        [NotMapped]
        public string REASON_NAME { get; set; }
        [NotMapped]
        public int Row_No { get; set; }
        public string ASSIGNED_GROUP { get; set; }
        public DateTime? ASSIGNED_GROUP_DATE { get; set; }
        public bool? IS_RECONCILIED { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
        [NotMapped]
        public bool Selected { get; set; }
        public string STATE { get; set; }
    }

    public class ReconciliationCPExportModel
    {
        [DisplayName("Sr. #")]
        public int ROW { get; set; }
        [DisplayName("Status")]
        public string STATUS_NAME { get; set; }
        [DisplayName("Deposit Date")]
        public string DEPOSIT_DATE { get; set; }
        [DisplayName("Deposit Type")]
        public string DEPOSIT_TYPE_NAME { get; set; }
        [DisplayName("State")]
        public string STATE { get; set; }
        [DisplayName("Category/Account")]
        public string CATEGORY_NAME { get; set; }
        [DisplayName("Insurance")]
        public string INSURANCE_NAME { get; set; }
        [DisplayName("Check #/Batch #")]
        public string CHECK_NO { get; set; }
        [DisplayName("Total Amount")]
        public string AMOUNT { get; set; }
        [DisplayName("Date Assigned")]
        public string ASSIGNED_DATE { get; set; }
        [DisplayName("Assigned To")]
        public string ASSIGNED_TO_NAME { get; set; }
        [DisplayName("Assigned Group")]
        public string ASSIGNED_GROUP { get; set; }
        [DisplayName("Assigned Group Date")]
        public string ASSIGNED_GROUP_DATE { get; set; }
        //[DisplayName("Ledger Attached")]
        //public string Has_Ledger { get; set; }
        [DisplayName("Date Posted")]
        public string DATE_POSTED { get; set; }
        [DisplayName("Total Posted")]
        public string AMOUNT_POSTED { get; set; }
        [DisplayName("Not Posted Amount")]
        public string AMOUNT_NOT_POSTED { get; set; }
        [DisplayName("Reason")]
        public string REASON_NAME { get; set; }
    }

    public class ReconciliationCPSearchReq
    {
        public bool IsForReport { get; set; }
        public bool IS_DEPOSIT_DATE_SEARCH { get; set; }
        public bool IS_ASSIGNED_DATE_SEARCH { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_FROM_Str { get; set; }
        public string DATE_TO_Str { get; set; }
        //public long? FOX_TBL_INSURANCE_ID { get; set; }
        public string INSURANCE_NAME { get; set; }
        public List<ReconciliationCategory> CATEGORIES { get; set; }
        public List<ReconciliationCP> FOX_TBL_INSURANCE_NAME { get; set; }
        public List<ReconciliationCP> STATES { get; set; }
        public string STATUS_ID { get; set; }
        public List<ReconciliationStatus> Statuses { get; set; }
        public List<ReconciliationDepositType> DEPOSIT_TYPES { get; set; }
        public List<CheckNoSelectionModel> CHECK_NOS { get; set; }
        public List<AmountSelectionModel> AMOUNTS { get; set; }
        public List<AmountPostedSelectionModel> AMOUNTS_POSTED { get; set; }
        public List<AmountNotPostedSelectionModel> AMOUNTS_NOT_POSTED { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string CalledFrom { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public int CP_Type { get; set; }
        public int TIME_FRAME { get; set; }
        public string STATE { get; set; }
    }

    [Table("FOX_TBL_RECONCILIATION_CP_LOGS")]
    public class ReconciliationCPLogs
    {
        [Key]
        public long RECONCILIATION_LOG_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long? RECONCILIATION_CP_ID { get; set; }
        public string LOG_MESSAGE { get; set; }
        public string FIELD_NAME { get; set; }
        public string PREVIOUS_VALUE { get; set; }
        public string NEW_VALUE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        //---------------------------------------
        ////////****NOT MAPPED FIELDS****////////
        //---------------------------------------
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public string CREATED_BY_NAME { get; set; }
        [NotMapped]
        public string ROLE_NAME { get; set; }

    }

    public class ReconciliationCPLogExportModel
    {
        [DisplayName("Created Date")]
        public string CREATED_DATE { get; set; }
        [DisplayName("Log Message")]
        public string LOG_MESSAGE { get; set; }
        [DisplayName("Created By")]
        public string CREATED_BY_NAME { get; set; }
    }

    public class ReconciliationCPLogSearchReq
    {
        public long RECONCILIATION_CP_ID { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public bool LogDetail { get; set; }
        public bool RemarkDetail { get; set; }
    }

    public class DDValues
    {
        public List<ReconciliationStatus> StatusDDValues { get; set; }
        public List<UsersForDropdown> UserDDValues { get; set; }
        public List<ReconciliationCategory> Category { get; set; }
        public List<ReconciliationDepositType> DepositType { get; set; }
        public List<FOX_TBL_RECONCILIATION_REASON> Reasons { get; set; }
    }

    public class ReconciliationCPToDelete
    {
        public long RECONCILIATION_CP_ID { get; set; }
    }
    public class UserAssignmentModel
    {
        public List<long> RECONCILIATION_CP_IDS { get; set; }
        public UsersForDropdown UserToAssign { get; set; }
    }

    public class DownloadLedgerModel
    {
        public long RECONCILIATION_CP_ID { get; set; }
    }
    public class LedgerModel
    {
        public long RECONCILIATION_CP_ID { get; set; }
        public string BASE_64_DOCUMENT { get; set; }
        public string FILE_NAME { get; set; }
        public string AbsolutePath { get; set; }
        public HttpFileCollection file { get; set; }
    }

    public class ReconciliationFilesSearchReq
    {
        public long RECONCILIATION_CP_ID { get; set; }
    }

    public class ReconciliationDDValue
    {
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    public class ReconciliationDDValueResponse
    {
        public long? NewId { get; set; }
        public string ValueType { get; set; }
    }

    public class CheckNoSelectionModel
    {
        public CheckNoSelectionModel()
        {
            Selected = false;
        }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }

    public class AmountSelectionModel
    {
        public AmountSelectionModel()
        {
            Selected = false;
        }
        public decimal Value { get; set; }
        public string StringValue { get; set; }
        public bool Selected { get; set; }
    }

    public class AmountPostedSelectionModel
    {
        public AmountPostedSelectionModel()
        {
            Selected = false;
        }
        public decimal Value { get; set; }
        public string StringValue { get; set; }
        public bool Selected { get; set; }
    }

    public class AmountNotPostedSelectionModel
    {
        public AmountNotPostedSelectionModel()
        {
            Selected = false;
        }
        public decimal Value { get; set; }
        public string StringValue { get; set; }
        public bool Selected { get; set; }
    }

    public class ReconsiliationCategoryDepositType
    {
        public List<ReconciliationCategory> Category { get; set; }
        public List<ReconciliationDepositType> DepositType { get; set; }

    }

    public class ReconcialtionImport
    {
        public string Day { get; set; }
        public string DepositDate { get; set; }
        public string DepositType { get; set; }
        public string CategoryAccount { get; set; }
        public string PayerName { get; set; }
        public string CheckNoBatchNo { get; set; }
        public string Amount { get; set; }
        public string DateAssigned { get; set; }
        public string AssignedTo { get; set; }
        public string DatePosted { get; set; }
        public string DateEntered { get; set; }
        public string TotalPosted { get; set; }
        public string NotPosted { get; set; }  
        public string CheckNo { get; set; }
        public string BatchNo { get; set; }
        public string State { get; set; }
    }

    public class ReconsiliationTemp
    {
        public DateTime DEPOSIT_DATE { get; set; }
        public long DEPOSIT_TYPE_ID { get; set; }
        public long CATEGORY_ID { get; set; }
        public long INSURANCE_ID { get; set; }
        public string CHECK_NO { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal AMOUNT_NOT_POSTED { get; set; }
        public decimal AMOUNT_POSTED { get; set; }
        public DateTime DATE_ASSIGNED { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string BATCH_NO { get; set; }

        public long? RECORD_EXISTED { get; set; }
        public long? RECORD_INSERTED { get; set; }

        public long? TOTAL_RECORDS { get; set; }


    }

    public class ReconciliationUploadResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public long? RECORD_EXISTED { get; set; }
        public long? RECORD_INSERTED { get; set; }
        public long? TOTAL_RECORDS { get; set; }
        public string ERROR_MESSAGE { get; set; }
        public DateTime? Last_UPLAOD_DATE { get; set; }
    }

    public class ReconsiliaitonUploadStatus
    {
        public DateTime? LastUploadDate { get; set; }
        public long? TotalRecords { get; set; }
        public long? UpdatedSucess { get; set; }
        public long? DuplicateRecords { get; set; }
        public long? FailedRecords { get; set; }
    }

    public class FOX_TBL_RECONCILIATION_UPLOAD_LOG
    {
        public long? ID { get; set; }
        public DateTime? LAST_UPDATED_DATE { get; set; }
        public long? TOTAL_RECORDS { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? SUCCESSFULLY_ADDED { get; set; }
    }

    public class SOFT_RECONCILIATION_PAYMENT
    {
        public string PATIENT_ACCOUNT { get; set; }
        public string PATIENT_NAME { get; set; }
        public long? CLAIM_NO { get; set; }
        public string INSPAYER_DESCRIPTION { get; set; }
        public string CHECK_NO { get; set; }
        public string CHECK_DATE { get; set; }
        public decimal? AMOUNT_POSTED { get; set; }
        public double? TOTAL_AMOUNT_POSTED { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int Row_No { get; set; }
        public string POSTED_DATE { get; set; }
    }

    public class SOFT_RECONCILIATION_SERACH_REQUEST
    {
        public string PATIENT_ACCOUNT { get; set; }
        public string PATIENT_NAME { get; set; }
        public long? CLAIM_NO { get; set; }
        public string CHECK_NO { get; set; }
        public string SearchText { get; set; }
        public int RecordPerPage { get; set; }

        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int AMOUNT_POSTED { get; set; }

    }

    public class FOX_TBL_RECONCILIATION_REASON
    {
        public long? FOX_TBL_RECONCILIATION_REASON_ID { get; set; }
        public string REASON { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public bool? DELETED { get; set; }

    }

    public class MTBC_Category_Description_Count
    {
        public int MatchCategoriesCount { get; set; }
        public int UnmatchCategoriesCount { get; set; }

    }

    [Table("FOX_TBL_MTBC_CREDENTIALS_AUTOMATION")]
    public class MTBC_Credentials_Fox_Automation
    {
        [Key]
        public long MTBC_CREDENTIALS_AUTOMATION_ID { get; set; }
        public string ASSOCIATION_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string WORK_EMAIL { get; set; }
        public string PERSONAL_MOBILE { get; set; }
        public string CERTIFICATION_DESCRIPTION { get; set; }
        public string CATEGORY_DESCRIPTION { get; set; }
        public DateTime? EFFECTIVE_DATE { get; set; }
        public DateTime? EXPIRATION_DATE { get; set; }
        public string ISSUING_STATE { get; set; }
        public string EMPLOYEE_NAME_DESCRIPTION { get; set; }
        public string UNIVERSITY_DESCRIPTION { get; set; }
        public string MENTOR { get; set; }
        public string DATA_TYPE { get; set; }
        public string HR_FILE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public DateTime? LAST_UPLOAD_DATE { get; set; }
        [NotMapped]
        public long RECORDS_ADDED_SUCCESSFULLY { get; set; }


    }
    public class HRAutoEmailsUploadResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public long? RECORD_EXISTED { get; set; }
        public long? RECORD_INSERTED { get; set; }
        public long? TOTAL_RECORDS { get; set; }
        public string ERROR_MESSAGE { get; set; }
        public DateTime? Last_UPLAOD_DATE { get; set; }
        public string Upload_by { get; set; }
        public long Failled_Record { get; set; }
        public string HR_FILE_NAME { get; set; }
        public string File_Path { get; set; }

    }

    public class FOX_TBL_HR_AUTO_EMAILS_UPLOAD_LOG
    {
        public long? ID { get; set; }
        public DateTime? LAST_UPDATED_DATE { get; set; }
        public long? TOTAL_RECORDS { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? SUCCESSFULLY_ADDED { get; set; }
    }

}
