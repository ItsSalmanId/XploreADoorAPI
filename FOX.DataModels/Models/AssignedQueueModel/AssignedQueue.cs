using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FOX.DataModels.Models.AssignedQueueModel
{
    public class AssignedQueue
    {
        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        [NotMapped]
        public string PATIENT_FIRST_NAME { get; set; }
        [NotMapped]
        public string PATIENT_LAST_NAME { get; set; }
        [NotMapped]
        public string MRN { get; set; }
        public string WORK_STATUS { get; set; }
        public string ASSIGNED_TO { get; set; }
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

                    if (!string.IsNullOrEmpty(SORCE_NAME) && (SORCE_NAME.ToLower().Equals("anonymous")|| Regex.IsMatch(SORCE_NAME, @"^[a-zA-Z0-9_]+$")))
                    {
                        SORCE_NAME = "";
                    }
                }
                return SORCE_NAME;
            }
        }


        public int? TOTAL_PAGES { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public string TIME_ELASPE_IN_QUEUE { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        [NotMapped]
        public string Assigned_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }

        public long? PATIENT_ACCOUNT { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string FILE_PATH { get; set; }
        public string RE_ASSIGNED_TO { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public string INDEXED_BY { get; set; }
        public DateTime INDEXED_DATE { get; set; }
        public int TOTAL_RECORDS { get; set; }
        
        public bool IS_EMERGENCY { get; set; }
        public bool? IS_UNSIGNED { get; set; }
        public bool? IS_TRASH_REFERRAL { get; set; }
        [NotMapped]
        public bool IS_STRATEGIC { get; set; }
        public long? OCR_STATUS_ID { get; set; }
        public string OCR_STATUS { get; set; }
        public bool IS_SYNC { get; set; }       
        public bool IS_ERROR { get; set; }
        public string ERROR_MSG { get; set; }
    }
    public class AssignedQueueRequest: BaseModel
    {
        public DateTime? DATE_FROM { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public string DATE_FROM_STR { get; set; }
        public int RecordPerPage { get; set; }
        public string SearchText { get; set; }
        public string SorceType { get; set; }
        public string SorceName { get; set; }
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string WorkID { get; set; }
        public string AssignTo { get; set; }
        public string Status { get; set; }
        public string reportUser { get; set; }
        public bool IncludeArchive { get; set; }
        public bool Is_Trash { get; set; }
    }

    public class BlacklistWhiteListSourceModel
    {
        public List<long> Work_Ids { get; set; }
        public bool Is_Black_List { get; set; }
        public bool Mark_All_Orders_As_White_Listed { get; set; }   
    }

    public class MarkReferralValidOrTrashedModel
    {
        public long Work_Id { get; set; }
        public bool Is_Trash { get; set; }
    }
}