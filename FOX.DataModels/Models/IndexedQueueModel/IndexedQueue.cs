using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
namespace FOX.DataModels.Models.IndexedQueueModel
{
    public class IndexedQueue
    {
        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public string INDEXED_BY { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public string INDEX_COMPLETION_TIME { get; set; }
        public string TIME_ELASPE_IN_QUEUE { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? PRACTICE_CODE { get; set; }
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

        public string FILE_PATH { get; set; }
        public string RE_ASSIGNED_TO { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public int NO_OF_COMMENTS { get; set; }
        public DateTime INDEXED_DATE { get; set; }
        public string file_path1 { get; set; }
        public bool Checked { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public bool IS_EMERGENCY { get; set; }
        public string ASSIGNTO_MEMBER { get; set; }
        public bool? IS_UNSIGNED { get; set; }
    }

    public class IndexedQueueRequest : BaseModel
    {
        public int RecordPerPage { get; set; }
        public bool IncludeArchive { get; set; }
        public string SearchText { get; set; }
        public string SorceType { get; set; }
        public string SorceName { get; set; }
        public string IndexedBy { get; set; }
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string CalledFrom { get; set; }
    }

    public class IndexedQueueFileRequest
    {
        public string id { get; set; }
        public long WORK_ID { get; set; }

    }


    public class IndexedQueueFileSplitRequest
    {
        public string UNIQUE_ID { get; set; }
        public string UNIQUE_ID_1 { get; set; }
        public string File_Path_1 { get; set; }
        public string File_path { get; set; }

    }



    public class FilePages
    {
        public string file_path1 { get; set; }
        public bool Checked { get; set; }
        public string UNIQUE_ID { get; set; }
        public string FILE_PATH { get; set; }
        public long WORK_ID { get; set; }

    }


    public class FilePages_Spilt
    {
        public string STATUS { get; set; }


    }

    [Table("FOX_TBL_WORK_TRANSFER")]
    public class WorkTransfer : BaseModel
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

    [Table("FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS")]
    public class Referral_Assignment_details
    {
        [Key]
        public long FOX_REFRRAL_ASSIGNMENT_ID { get; set; }
        public long WORK_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string ASSIGNED_BY { get; set; }
        public string ASSIGNED_BY_DESIGNATION { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_TO_DESIGNATION { get; set; }
        public DateTime? ASSIGNED_TIME { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
    }
    public class AssignedQueueMultipleModel : BaseModel
    {
        public List<IndexedQueue> MultipleQueue { get; set; }
    }
    public class SetSplitPagesRequestModel : BaseModel
    {
        public List<FilePages> filePages { get; set; }
    }
}