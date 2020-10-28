using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.GeneralNotesModel
{
    public class GeneralNoteResponseModel : ResponseModel
    {
        public FOX_TBL_GENERAL_NOTE Note { get; set; }
        public string Case { get; set; }
        public IEnumerable<FOX_TBL_GENERAL_NOTE> NoteHistory { get; set; }
    }
    public class GeneralNoteCreateUpdateRequestModel : BaseModel
    {
        public FOX_TBL_GENERAL_NOTE GENERAL_NOTE { get; set; }
        public FOX_TBL_GENERAL_NOTE GENERAL_NOTE_HISTORY { get; set; }
    }
    public class GeneralNoteDeleteRequestModel : BaseModel
    {
        public string GENERAL_NOTE_ID_AS_STRING { get; set; }
        public bool IS_TASK_DELETE { get; set; }
    }
    public class GeneralNoteRequestModel : BaseModel
    {
        public long GENERAL_NOTE_ID { get; set; }
        public string PATIENT_ACCOUNT_AS_STRING { get; set; }
    }
    public class GeneralNotesSearchRequest : BaseModel
    {
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string Sort_By { get; set; }
        public string Sort_Order { get; set; }
        public string SearchText { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
    }
    public class SmartSearchCasesRequestModel : BaseModel
    {
        public string Keyword { get; set; }
        public string Patient_Account { get; set; }
        public long PracticeCode { get; set; }
    }
    public class GeneralNoteHistoryRequestModel : BaseModel
    {
        public long PARIENT_GENERAL_NOTE_ID { get; set; }
        public string PATIENT_ACCOUNT_AS_STRING { get; set; }
    }
    public class GeneralNoteHistoryResponseModel : ResponseModel
    {
        public List<FOX_TBL_GENERAL_NOTE> History { get; set; }
    }
    [Table("FOX_TBL_GENERAL_NOTE")]
    public class FOX_TBL_GENERAL_NOTE
    {
        [Key]
        public long GENERAL_NOTE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> PATIENT_ACCOUNT { get; set; }
        public Nullable<long> PARENT_GENERAL_NOTE_ID { get; set; }
        public Nullable<long> CASE_ID { get; set; }
        public Nullable<long> TASK_ID { get; set; }
        public string NOTE_DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool IS_PATIENT_ALERT { get; set; }
        public DateTime? PATIENT_ALERT_EFFECTIVE_TO { get; set; }

        public long? ALERT_TYPE_ID { get; set; }
        [NotMapped]
        public string CASE_NO { get; set; }
        [NotMapped]
        public string RT_CASE_NO { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_AS_STRING { get; set; }
        [NotMapped]
        public double TOTAL_RECORDS { get; set; }
        [NotMapped]
        public double TOTAL_PAGES { get; set; }
        [NotMapped]
        public string LAST_REPLY_BY { get; set; }
        [NotMapped]
        public DateTime? LAST_REPLY_ON { get; set; }
        [NotMapped]
        public int HISTORY_COUNT { get; set; }
        [NotMapped]
        public int TASK_COUNT { get; set; }
        [NotMapped]
        public string CREATED_BY_FULL_NAME { get; set; }
        [NotMapped]
        public string CREAETED_BY_FIRST_NAME { get; set; }
        [NotMapped]
        public string CREATED_BY_LAST_NAME { get; set; }
        [NotMapped]
        public bool IS_GREEN { get; set; }
        [NotMapped]
        public bool IS_YELLOW { get; set; }
        [NotMapped]
        public string ATTACHMENT_PATH { get; set; }
        [NotMapped]
        public string ATTACHMENT_TITLE { get; set; }
        [NotMapped]
        public List<FOX_TBL_GENERAL_NOTE> NOTE_REPLIES { get; set; }
        public string Note_Description1 {
            get { if (this.NOTE_DESCRIPTION != null && this.NOTE_DESCRIPTION.Contains("\n") && !this.NOTE_DESCRIPTION.Contains("DOCTYPE html PUBLIC"))
                    {
                    string[] temp;
                    temp = this.NOTE_DESCRIPTION.Split( new char[] { '\n' });
                    return temp[0];
                    }
                    else
                    {
                        return this.NOTE_DESCRIPTION;
                    }
                }
        }
        public string Note_Description2
        {
            get
            {
                if (this.NOTE_DESCRIPTION != null && this.NOTE_DESCRIPTION.Contains("\n") && !this.NOTE_DESCRIPTION.Contains("DOCTYPE html PUBLIC"))
                {
                    string[] temp;
                    temp = this.NOTE_DESCRIPTION.Split(new char[] {'\n' });
                    return temp[1];
                    //return temp[temp.Length -1];
                }
                else
                {
                    return this.NOTE_DESCRIPTION;
                }
            }
        }
        [NotMapped]
        public bool IsNoteFromPHD {
            get
            {
                if (this.NOTE_DESCRIPTION != null && this.NOTE_DESCRIPTION.Contains("\n") && !this.NOTE_DESCRIPTION.Contains("DOCTYPE html PUBLIC"))
                {
                   return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    public class GeneralNoteTaskRequestModel
    {
        public string GENERAL_NOTE_ID_AS_STRING { get; set; }
    }
    public class GeneralNoteTaskResponseModel : ResponseModel
    {
        public FOX_TBL_TASK Task { get; set; }
    }
    public class GeneralNoteTaskViewModel
    {

        public string TASK_TYPE_NAME { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_DESCRIPTION { get; set; }
        public string PROVIDER_LAST_NAME { get; set; }
        public string PROVIDER_FIRST_NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string SEND_TO_USER_ID { get; set; }
        public string SEND_TO_USER_NAME { get; set; }
        public string SEND_TO_FIRST_NAME { get; set; }
        public string FINAL_ROUTE_USER_NAME { get; set; }
        public string FINAL_ROUTE_USER_ID { get; set; }
        public string FINAL_ROUTE_FIRST_NAME { get; set; }
        public string FINAL_ROUTE_LAST_NAME { get; set; }
        public string SEND_CONTEXT_NAME { get; set; }
        public string DELIVERY_METHOD_NAME { get; set; }
        public string COMMENT { get; set; }
    }
    [Table("FOX_TBL_ALERT_TYPE")]
    public class AlertType
    {
        [Key]
        public long ALERT_TYPE_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public long? PRACTICE_CODE { get; set; }
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
    [Table("FOX_TBL_ALERT")]
    public class NoteAlert
    {
        [Key]
        public long FOX_TBL_ALERT_ID { get; set; }
        public long ALERT_TYPE_ID { get; set; }
        public string NOTE_DETAIL { get; set; }
        public DateTime? EFFECTIVE_TILL { get; set; }
        [NotMapped]
        public string EFFECTIVE_TILL_str { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_str { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_RECORDS { get; set; }
        [NotMapped]
        public double TOTAL_PAGES { get; set; }
        [NotMapped]
        public string ALERT_TYPE_NAME { get; set; }
        [NotMapped]
        public string CREATED_BY_FULL_NAME { get; set; }
        [NotMapped]
        public string MODIFIED_BY_FULL_NAME { get; set; }

    }
    public class AlertSearchRequest
    {
        public string CurrentPage { get; set; }
        public string RecordPerPage { get; set; }
        public string Sort_By { get; set; }
        public string Sort_Order { get; set; }
        public string SearchText { get; set; }
        public string PATIENT_ACCOUNT { get; set; }

    }
    [Table("FOX_TBL_INTERFACE_LOG")]
    public class GeneralNotesInterfaceLog
    {
        [Key]
        public long FOX_INTERFACE_LOG_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string PATIENT_ACCOUNT_str { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string ERROR { get; set; }
        public string ACK { get; set; }
        public bool? IS_ERROR { get; set; }
        public string LOG_MESSAGE { get; set; }
        public bool? IS_INCOMMING { get; set; }
        public bool? IS_OUTGOING { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public long? FOX_INTERFACE_SYNCH_ID { get; set; }
        public string APPLICATION { get; set; }
        [NotMapped]
        public long? Work_ID { get; set; }
    }
    public class InterfaceLogSearchRequest
    {
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string Option { get; set; }
        public string SearchText { get; set; }
        public string Sort_By { get; set; }
        public string Sort_Order { get; set; }
        public string PATIENT_ACCOUNT { get; set; }

    }
}