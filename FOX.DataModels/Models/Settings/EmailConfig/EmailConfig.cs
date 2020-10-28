using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Settings.EmailConfig
{
    [Table("FOX_TBL_EMAIL_CONFIG")]
    public class EmailConfig
    {
        [Key]
        public long ID { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public bool ON_INDEX { get; set; }
        public bool ON_COMPELETED { get; set; }
        public bool SERVICE_EXCEPTION { get; set; }
        public bool ON_REFERRAL_RECEIVE { get; set; }         
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool IsUrgentEmail { get; set; }

    }
    public class IndexInfoEmail
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public string SORCE_NAME { get; set; }
        public string REF_SOURCE_FIRST_NAME { get; set; }
        public string REF_SOURCE_LAST_NAME { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string TREATMENT_LOCATION { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public string PATIENT_GENDER { get; set; }
        public DateTime? PATIENT_DOB { get; set; }
        public string PATIENT_MRN { get; set; }
        public string PATIENT_PHONE_NO { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string COUNTRY { get; set; }
        public string INS_NAME { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public bool? IS_EMERGENCY_ORDER { get; set; }
    }

    [Table("FOX_TBL_NOTES_HISTORY")]
    public class NotesHistory
    {
        [Key]
        public long NOTE_ID { get; set; }
        public long WORK_ID { get; set; }
        public string NOTE_DESC { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
    }

}