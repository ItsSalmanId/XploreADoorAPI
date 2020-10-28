using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.CasesModel;

namespace FOX.DataModels.Models.PatientDocuments
{
    class PatientDocuments
    {
    }
    public class PatientDocumentRequest
    {
        public string PATIENT_ACCOUNT { get; set; }
        public string DOCUMENT_TYPE_ID { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public int SELECTED_DOCUMENT_TYPE { get; set; }
        public bool IS_DOCUMENT_CLICKED { get; set; }
    }
    public class PatientDocument
    {
        public int ROW { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public long? PAT_DOCUMENT_ID { get; set; }
        public long? PARENT_DOCUMENT_ID { get; set; }
        public string Patient_AccountStr
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                PATIENT_ACCOUNT = Convert.ToInt64(value);
            }
        }
        public long? CASE_ID { get; set; }
        public List<caseViewDataModelView> CASE_LIST { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public int? CASE_STATUS_ID { get; set; }
        public string CASE_STATUS { get; set; }
        public long? WORK_ID { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public bool? SHOW_ON_PATIENT_PORTAL { get; set; }
        public string COMMENTS { get; set; }
        public long? IMAGE_FILE_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_PATH1 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string START_DATE_str { get; set; }
        public string END_DATE_str { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_TYPE { get; set; }
    }
    [Table("FOX_TBL_PATIENT_PAT_DOCUMENT")]
    public class PatientPATDocument
    {
        [Key]
        public long PAT_DOCUMENT_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_str { get; set; }
        public long? PARENT_DOCUMENT_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? WORK_ID { get; set; }
        public int? DOCUMENT_TYPE { get; set; }
        public long? CASE_ID { get; set; }
        [NotMapped]
        public List<caseViewDataModelView> CASE_LIST { get; set; }
        [NotMapped]
        public string CASE_NO { get; set; }
        [NotMapped]
        public int? CASE_STATUS_ID { get; set; }
        public DateTime? START_DATE { get; set; }
        [NotMapped]
        public string START_DATE_str { get; set; }
        public DateTime? END_DATE { get; set; }
        [NotMapped]
        public string END_DATE_str { get; set; }
        public bool SHOW_ON_PATIENT_PORTAL { get; set; }
        public string COMMENTS { get; set; }
        [NotMapped]
        public List<PatientDocumentFiles> DOCUMENT_PATH_LIST { get; set; }
        //public string DOCUMENT_PATH { get; set; }
        //public string DOCUMENT_LOGO_PATH { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long FOX_PHD_CALL_DETAILS_ID { get; set; }
    }
    public class DocumenttypeAndpatientcases
    {
        public List<FoxDocumentType> DocumentTypes { get; set; }
        public List<caseViewDataModelView> PatientCasesList { get; set; }
    }

    public class caseViewDataModelView
    {
        public long CASE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> PATIENT_ACCOUNT { get; set; }
        public Nullable<int> CASE_TYPE_ID { get; set; }
        public string CASE_TYPE_NAME { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public Nullable<int> CASE_STATUS_ID { get; set; }
        public string CASE_STATUS_NAME { get; set; }
        public Nullable<long> WORK_ID { get; set; }
        public bool IS_CHECKED { get; set; }
        public bool IS_DISABLED { get; set; }
    }
    [Table("FOX_TBL_PATIENT_DOCUMENT_FILE_ALL")]
    public class PatientDocumentFiles
    {
        [Key]
        public long PATIENT_DOCUMENT_FILE_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long PAT_DOCUMENT_ID { get; set; }
        public string DOCUMENT_PATH { get; set; }
        public string DOCUMENT_LOGO_PATH { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
}
