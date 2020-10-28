using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.AdjustmentApproval
{
    [Table("FOX_TBL_ADJUSTMENT_AMOUNT")]
    public class AdjustmentAmount
    {
        [Key]
        public int ADJUSTMENT_AMOUNT_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string ADJUSTMENT_AMOUNT { get; set; }
        public string EXPRESSION { get; set; }
        public decimal? RANGE_FROM { get; set; }
        public decimal? RANGE_TO { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_ADJUSTMENT_CLAIM_STATUS")]
    public class AdjustmentClaimStatus
    {
        [Key]
        public int STATUS_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string STATUS_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string STATUS_CATEGORY { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_PATIENT_ADJUSTMENT_DETAILS")]
    public class PatientAdjustmentDetails : ICloneable
    {
        [Key]
        public long ADJUSTMENT_DETAIL_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public DateTime REQUESTED_DATE { get; set; }
        public string REQUESTED_BY { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public DateTime? DOS_FROM { get; set; }
        public DateTime? DOS_TO { get; set; }
        public int? DISCIPLINE_ID { get; set; }
        public decimal? ADJUSTMENT_AMOUNT { get; set; }
        public bool FLAG_17B9W { get; set; }
        public bool FLAG_17BAN { get; set; }
        public bool FLAG_17BDW { get; set; }
        public bool FLAG_17BEA { get; set; }
        public bool FLAG_17BER { get; set; }
        public bool FLAG_17CHP { get; set; }
        public bool FLAG_17CO { get; set; }
        public bool FLAG_17FUA { get; set; }
        public bool FLAG_17FUO { get; set; }
        public bool FLAG_17HHE { get; set; }
        public bool FLAG_17INC { get; set; }
        public bool FLAG_17INS { get; set; }
        public bool FLAG_17LTC { get; set; }
        public bool FLAG_17MCR { get; set; }
        public bool FLAG_17MDW { get; set; }
        public bool FLAG_17MED { get; set; }
        public bool FLAG_17NOA { get; set; }
        public bool FLAG_17PTF { get; set; }
        public bool FLAG_17SBW { get; set; }
        public bool FLAG_17SCW { get; set; }
        public bool FLAG_17WCW { get; set; }
        public bool FLAG_17PEC { get; set; }
        public bool FLAG_17PED { get; set; }
        public bool FLAG_OTHER { get; set; }
        public string OTHER_DESCRIPTION { get; set; }
        public string REASON { get; set; }
        public int? ADJUSTMENT_STATUS_ID { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime? APPROVED_DATE { get; set; }
        public string APPROVED_SIGN_PATH { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string CLOSED_BY { get; set; }
        public DateTime? CLOSED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        //-----------------------------------------//
        //***Not Mapped Fields***//
        [NotMapped]
        public string PATIENT_ACCOUNT_STR
        {
            get
            {
                return PATIENT_ACCOUNT.HasValue ? PATIENT_ACCOUNT.Value.ToString() : "";
            }
            set
            {
                PATIENT_ACCOUNT = Convert.ToInt64(value);
            }
        }
        [NotMapped]
        public string DISCIPLINE_NAME { get; set; }
        [NotMapped]
        public string PATIENT_FIRST_NAME { get; set; }
        [NotMapped]
        public string PATIENT_LAST_NAME { get; set; }
        [NotMapped]
        public string PATIENT_NAME
        {
            get
            {
                return string.IsNullOrWhiteSpace(PATIENT_FIRST_NAME) ? "" : $@"{PATIENT_LAST_NAME}, {PATIENT_FIRST_NAME}";
            }
        }

        [NotMapped]
        public string PATIENT_MIDDLE_NAME { get; set; }

        [NotMapped]
        public string PATIENT_GENDER { get; set; }

        [NotMapped]
        public DateTime? PATIENT_DOB { get; set; }

        [NotMapped]
        public string PATIENT_SSN { get; set; }

        [NotMapped]
        public DateTime? PATIENT_CREATED_DATE { get; set; }

        [NotMapped]
        public string STATUS_NAME { get; set; }
        [NotMapped]
        public string REQUESTED_BY_NAME { get; set; }
        [NotMapped]
        public string MRN { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public string APPROVED_BY_NAME { get; set; }
        [NotMapped]
        public string ASSIGNED_TO_NAME { get; set; }
        [NotMapped]
        public string CLOSED_BY_NAME { get; set; }
        [NotMapped]
        public string CREATED_BY_NAME { get; set; }
        [NotMapped]
        public string REQUESTED_DATE_STR { get; set; }
        [NotMapped]
        public string DOS_FROM_STR { get; set; }
        [NotMapped]
        public string DOS_TO_STR { get; set; }
        //-----------------------------------------//
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    [Table("FOX_TBL_ADJUSTMENT_LOGS")]
    public class AdjustmentLog
    {
        [Key]
        public long ADJUSTMENT_LOG_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long? ADJUSTMENT_DETAIL_ID { get; set; }
        public string LOG_MESSAGE { get; set; }
        public string FIELD_NAME { get; set; }
        public string PREVIOUS_VALUE { get; set; }
        public string NEW_VALUE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public string CREATED_BY_NAME { get; set; }
    }
    public class AdjustmentLogExportModel
    {
        [DisplayName("Sr. #")]
        public long SERIAL_NUMBER { get; set; }
        [DisplayName("Created Date")]
        public string CREATED_DATE { get; set; }
        [DisplayName("Log Message")]
        public string LOG_MESSAGE { get; set; }
        [DisplayName("Created By")]
        public string CREATED_BY_NAME { get; set; }
    }

    public class AdjustmentLogSearchReq
    {
        public long ADJUSTMENT_DETAIL_ID { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
    }

    public class AdjustmentsSearchReq
    {
        public bool IsForReport { get; set; }
        public bool IS_ACTIVE_DOS_DATE_TAB { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_FROM_Str { get; set; }
        public string DATE_TO_Str { get; set; }
        public string PatientAccount { get; set; }
        public string PATIENT_NAME { get; set; }
        public int? AdjustmentAmountId { get; set; }
        public int? StatusId { get; set; }
        public int? DisciplineId { get; set; }
        public string SearchString { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }

    public class DDValues
    {
        public List<AdjustmentAmount> AdjustmentAmountDDValues { get; set; }
        public List<AdjustmentClaimStatus> AdjustmentStatusDDValues { get; set; }
        public List<UsersForDropdown> UserDDValues { get; set; }
    }

    public class StatusCounterSearch
    {
        public bool IsForReport { get; set; }
    }

    public class StatusCounter
    {
        public int All { get; set; }
        public int Open { get; set; }
        public int InProgress { get; set; }
        public int Pending { get; set; }
        public int Closed { get; set; }
        public int ReviewRequired { get; set; }
    }

    public class AdjustmentToDelete
    {
        public long ADJUSTMENT_DETAIL_ID { get; set; }
    }
    public class UserAssignmentModel
    {
        public long ADJUSTMENT_DETAIL_ID { get; set; }
        public UsersForDropdown UserToAssign { get; set; }
    }

    public class PatientAdjustmentDetailsExportModel
    {
        [DisplayName("Sr. #")]
        public long SERIAL_NUMBER { get; set; }
        [DisplayName("Status")]
        public string STATUS_NAME { get; set; }
        [DisplayName("Patient")]
        public string PATIENT_NAME { get; set; }
        [DisplayName("MRN")]
        public string MRN { get; set; }
        [DisplayName("DOS From")]
        public string DOS_FROM { get; set; }
        [DisplayName("DOS To")]
        public string DOS_TO { get; set; }
        [DisplayName("Discipline(s)")]
        public string DISCIPLINE_NAME { get; set; }
        [DisplayName("Amount")]
        public string ADJUSTMENT_AMOUNT { get; set; }
        [DisplayName("Reason")]
        public string REASON { get; set; }
        [DisplayName("Requested By")]
        public string REQUESTED_BY { get; set; }
        [DisplayName("Requested Date")]
        public string REQUESTED_DATE { get; set; }
        [DisplayName("Approved By")]
        public string APPROVED_BY { get; set; }
        [DisplayName("Approved Date")]
        public string APPROVED_DATE { get; set; }
        [DisplayName("Assigned To")]
        public string ASSIGNED_TO { get; set; }
    }

    public class SignRequestDetails
    {
        public long ADJUSTMENT_DETAIL_ID { get; set; }
    }

    public class DownloadAdjustmentModel
    {
        public long ADJUSTMENT_DETAIL_ID { get; set; }
        public string ATTACHMENT_HTML { get; set; }
    }
}
