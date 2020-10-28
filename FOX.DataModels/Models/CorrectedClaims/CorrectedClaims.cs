using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.CorrectedClaims
{
    [Table("FOX_TBL_CORRECTED_CLAIM")]
    public class CORRECTED_CLAIM
    {
        [Key]
        public long CORRECTED_CLAIM_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string Patient_AccountStr { get; set; }
        public string CHART_ID { get; set; }
        public DateTime? CHARGE_ENTRY_DATE { get; set; }
        [NotMapped]
        public string CHARGE_ENTRY_DATE_Str { get; set; }
        public string CASE_NO { get; set; }
        public long? FOX_TBL_INSURANCE_ID { get; set; }
        public DateTime? DOS_FROM { get; set; }
        [NotMapped]
        public string DOS_FROM_Str { get; set; }
        public DateTime? DOS_TO { get; set; }
        [NotMapped]
        public string DOS_TO_Str { get; set; }
        public long? SOURCE_ID { get; set; }
        public int? CORRECTED_CLAIM_TYPE_ID { get; set; }
        public string REQUESTED_BY { get; set; }
        public DateTime? REQUESTED_DATE { get; set; }
        [NotMapped]
        public string REQUESTED_DATE_Str { get; set; }
        public DateTime? WORK_DATE { get; set; }
        [NotMapped]
        public string WORK_DATE_Str { get; set; }
        public int? STATUS_ID { get; set; }
        public string REMARKS { get; set; }
        public string RESPONSE { get; set; }
        public string RESPONSE_BY { get; set; }
        public DateTime? RESPONSE_DATE { get; set; }
        [NotMapped]
        public string RESPONSE_DATE_Str { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public string CORRECTED_CLAIMS_TYPE_DESC { get; set; }
        [NotMapped]
        public string INSURANCE_NAME { get; set; }
        [NotMapped]
        public string Patient_Name { get; set; }
        [NotMapped]
        public string Theripist { get; set; }
        [NotMapped]
        public string STATUS_NAME { get; set; }
        [NotMapped]
        public DateTimeViewModal DOS_FROM_ACT { get; set; }
        [NotMapped]
        public DateTimeViewModal DOS_TO_ACT { get; set; }

    }
    [Table("FOX_TBL_CORRECTED_CLAIM_TYPE")]
    public class CORRECTED_CLAIM_TYPE
    {
        [Key]
        public int CORRECTED_CLAIM_TYPE_ID { get; set; }
        public string CORRECTED_CLAIMS_TYPE_DESC { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

    }

    public class ResponseModel : BaseModel
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public string ID { get; set; }
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

    public class CorrectedClaimResponse
    {
        public long ROW { get; set; }
        public string STATUS_NAME { get; set; }
        public DateTime? CHARGE_ENTRY_DATE { get; set; }
        public string CHARGE_ENTRY_DATE_str { get; set; }
        public long CORRECTED_CLAIM_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string Patient_Name { get; set; }
        public string CORRECTED_CLAIMS_TYPE_DESC { get; set; }
        public string Theripist { get; set; }
        public string INSURANCE_NAME { get; set; }
        public string DOS { get; set; }
        public string CASE_NO { get; set; }
        public string REQUESTED_BY { get; set; }
        public DateTime? REQUESTED_DATE { get; set; }
        public DateTime? WORK_DATE { get; set; }
        public string REQUESTED_DATE_Str { get; set; }
        public string WORK_DATE_Str { get; set; }
        public string REMARKS { get; set; }
        public string RESPONSE_BY { get; set; }
        public string RESPONSE { get; set; }
        public DateTime? RESPONSE_DATE { get; set; }
        public string RESPONSE_DATE_Str { get; set; }
        public string CHART_ID { get; set; }
        public DateTime? DOS_FROM { get; set; }
        public DateTime? DOS_TO { get; set; }
        public long? SOURCE_ID { get; set; }
        public int? CORRECTED_CLAIM_TYPE_ID { get; set; }
        public int? STATUS_ID { get; set; }

        public long? FOX_TBL_INSURANCE_ID { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public string Message { get; set; }


    }


    public class CorrectedClaimSummary

    {
        public int? TOTAL { get; set; }
        public string STATUS_NAME { get; set; }

    }


    public class CorrectedClaimSummaryReq

    {
        public long parmPracticeCode { get; set; }
        public int RecordPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string searchString { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public DateTime? dOS_FROM { get; set; }
        public DateTime? dOS_TO { get; set; }
        public DateTime? rEQ_FROM { get; set; }
        public DateTime? rEQ_TO { get; set; }
        public string DOS_FROM_STR { get; set; }
        public string DOS_TO_STR { get; set; }
        public string REQ_FROM_STR { get; set; }
        public string REQ_TO_STR { get; set; }
        public int cORRECTED_CLAIM_TYPE_ID { get; set; }
        public string cASE_NO { get; set; }
        public long sOURCE_ID { get; set; }
        public int sTATUS_ID { get; set; }
        public long pATIENT_ACCOUNT { get; set; }
        public string PATIENT_ACCOUNT_Str { get; set; }

    }
    public class CorrectedClaimSummaryModel
    {
        public int? InProgressTotal { get; set; }
        public int? OpenTotal { get; set; }
        public int? ReviewTotal { get; set; }
        public int? PendingTotal { get; set; }
        public int? ClosedTotal { get; set; }
        public long? Total { get; set; }
        public List<CorrectedClaimSummary> summary { get; set; }
    }

    public class CorrectedClaimSearch
    {
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string searchString { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public DateTime? DOS_FROM { get; set; }
        public DateTime? DOS_TO { get; set; }
        public DateTime? REQ_FROM { get; set; }
        public DateTime? REQ_TO { get; set; }
        public string DOS_FROM_STR { get; set; }
        public string DOS_TO_STR { get; set; }
        public string REQ_FROM_STR { get; set; }
        public string REQ_TO_STR { get; set; }
        public int CORRECTED_CLAIM_TYPE_ID { get; set; }
        public string CASE_NO { get; set; }
        public long SOURCE_ID { get; set; }
        public int STATUS_ID { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public long CORRECTED_CLAIM_ID { get; set; }
        public string PATIENT_ACCOUNT_Str { get; set; }

    }

    public class CorrectedClaimData
    {
        public List<CorrectedClaimResponse> CorrectedClaims { get; set; }
        public CorrectedClaimSummaryModel CorrectedClaimSummary { get; set; }

    }

    public class PatientCases
    {
        public long? CASE_ID { get; set; }
        public string CASE_NO { get; set; }
        public string CHART_ID { get; set; }
    }
    [Table("FOX_TBL_CORRECTED_CLAIM_LOG")]
    public class CorrectedClaimLog
    {
        [Key]
        public long CORRECTED_CLAIM_LOG_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long CORRECTED_CLAIM_ID { get; set; }
        public string ACTION { get; set; }
        public string ACTION_DETAIL { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

        public static implicit operator List<object>(CorrectedClaimLog v)
        {
            throw new NotImplementedException();
        }

    }

    public class DateTimeViewModal
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Date { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public int? Seconds { get; set; }

        public DateTime? GetSetDate
        {
            get
            {
                if (Year.HasValue && Month.HasValue && Date.HasValue && Hours.HasValue && Minutes.HasValue && Seconds.HasValue)
                {
                    return new DateTime(Year ?? 0, Month ?? 0, Date ?? 0, Hours ?? 0, Minutes ?? 0, Seconds ?? 0);
                }
                return null;
            }
            set
            {
                Year = value?.Year;
                Month = value?.Month;
                Date = value?.Day;
                Hours = value?.Hour;
                Minutes = value?.Minute;
                Seconds = value?.Second;
            }
        }
    }
}
