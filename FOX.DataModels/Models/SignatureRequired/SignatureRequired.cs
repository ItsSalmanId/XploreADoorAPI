using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.SignatureRequired
{

    public class SignatureRequiredRequest
    {
        public int CURRENT_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public string USER_NAME { get; set; }
        public string USER_TYPE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public bool IsSignatureRequired { get; set; }
    }

    public class SignatureRequiredReposne : BaseModel
    {
        public string UNIQUE_ID { get; set; }
        public List<WorkDetails> WorkDetails { get; set; }
        [NotMapped]
        public List<string> FileNameList { get; set; }
        public string SORCE_NAME { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public int? TOTAL_RECORDS { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public string WORK_STATUS { get; set; }       
        public string First_Name { get; set; } 
        public string Last_name { get; set; }
        [NotMapped]
        public string REFERRAL_EMAIL_SENT_TO { get; set; }
        public bool? IsSigned { get; set; }
        public int ROW { get; set; }

    }
    public class ResSignReqModel
    {
        public List<SignatureRequiredReposne> SignatureReqList { get; set; }
        public bool IsUniqueIdExist { get; set; } = false;
        public List<string> Unique_IdList { get; set; }
    }

        public class WorkDetails
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string WORK_STATUS { get; set; }
        public int? NO_OF_PAGES { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string COMPLETED_BY { get; set; }
        public string FILE_PATH { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
    }
    public class ReqsignatureModel : BaseModel
    {
        public List<string> Unique_IdList { get; set; }
    }
}
