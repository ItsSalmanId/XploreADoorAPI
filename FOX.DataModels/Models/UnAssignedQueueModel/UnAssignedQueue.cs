using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FOX.DataModels.Models.UnAssignedQueueModel
{
    public class UnAssignedQueue
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
                        if (!string.IsNullOrEmpty(SORCE_NAME) && (SORCE_NAME.ToLower().Equals("anonymous") || Regex.IsMatch(SORCE_NAME, @"^[a-zA-Z0-9_]+$")))
                        {
                            SORCE_NAME = "";
                        }
                    }

                }
                return SORCE_NAME;
            }
        }

        public string WORK_STATUS { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public string ElapseTime { get; set;}
        public int? TOTAL_PAGES { get; set; }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public bool IS_EMRERGENCY { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public bool? IS_UNSIGNED { get; set; }
        [NotMapped]
        public bool IS_STRATEGIC { get; set; }
        public long? OCR_STATUS_ID { get; set; }
        public string OCR_STATUS { get; set; }
    }

    public class UnAssignedQueueRequest: BaseModel
    {
        public int RecordPerPage { get; set; }      
        public string SearchText { get; set; }
        public string SorceType { get; set; }
        public string SorceName { get; set; }
        public string Client { get; set; }            
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortOrder{ get; set; }

        public string ID { get; set; }
        public string CalledFrom { get; set; }
        public bool IncludeArchive { get; set; }
    }

    public class UnAssignedQueueExportModel
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string WORK_STATUS { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public string ElapseTime { get; set; }
        public bool IS_EMRERGENCY { get; set; }
        
    }

    public class UnAssignedQueueAndUsersForDropdown
    {
        public List<UnAssignedQueue> unassignedQueueData { get; set; }
        public List<UsersForDropdown> usersForDropdownData { get; set; }
    }
}
