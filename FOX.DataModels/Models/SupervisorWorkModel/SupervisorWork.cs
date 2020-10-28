using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FOX.DataModels.Models.SupervisorWorkModel
{
 
    public class SupervisorWork
    {
        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public string SUPERVISOR_NAME { get; set; }
        public string WORK_STATUS { get; set; }
        public string TRANSFER_REASON { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_NAMEFormat
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SORCE_NAME))
                {
                    if (SORCE_NAME.Contains("+1"))
                    {
                        SORCE_NAME = SORCE_NAME.Replace("+1", "");
                        if (!string.IsNullOrEmpty(SORCE_NAME))
                        {
                            SORCE_NAME = String.Format("{0:(###) ###-####}", Int64.Parse(SORCE_NAME));
                        }
                    }
                    if (!string.IsNullOrEmpty(SORCE_NAME) && (SORCE_NAME.ToLower().Equals("anonymous") || Regex.IsMatch(SORCE_NAME, @"^[a-zA-Z0-9_]+$")))
                    {
                        SORCE_NAME = "";
                    }

                }
                return SORCE_NAME;
            }
        }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        [NotMapped]
        public string Assigned_Date_Str { get; set; }
        public string COMPLETED_BY { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public long? SENDER_ID { get; set; }
        public string FACILITY_NAME { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public bool? IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public string ElapseTime { get; set; }
        public bool IS_EMERGENCY { get; set; }
        public bool? IS_UNSIGNED { get; set; }
        public bool? IS_STRATEGIC { get; set; }

    }

    public class SupervisorWorkRequest: BaseModel
    {
        public int RecordPerPage { get; set; }      
        public string SearchText { get; set; }
        public string SourceName{ get; set; }
        public string SourceType { get; set; }
        public string SourceString {get; set; }           
        public int CurrentPage { get; set; }
        public string UniqueId{ get; set; }
        public string SortBy{ get; set; }
        public string SortOrder { get; set; }
        public string TransferReason { get; set; }
        public string TransferComments { get; set; }
        public string SupervisorName { get; set; }
        public string IndexBy { get; set; }
        public string Status { get; set; }
        public string CalledFrom { get; set; }
    }

    [Table("FOX_TBL_WORK_TRANSFER")]
    public class WorkTransfer
    {
        [Key]
        public long WORK_TRANFER_ID { get; set; }
        public long? WORK_ID { get; set; }
        public string TRANSFER_COMMENTS { get; set; }
        public string TRANSFER_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string TRANSFER_REASON { get; set; }
        public bool DELETED { get; set; }

    }
}