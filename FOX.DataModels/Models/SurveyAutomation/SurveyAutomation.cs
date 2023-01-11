using FOX.DataModels.Models.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.SurveyAutomation
{
    public class SurveyAutomations
    {
        public class SurveyAutomation
        {
            public string PATIENT_ACCOUNT { get; set; }
            public string PROVIDER { get; set; }
            public string REGION { get; set; }
            public string PT_OT_SLP { get; set; }
            public string SERVICE_OR_PAYMENT_DESCRIPTION { get; set; }
            public long SURVEY_ID { get; set; }

        }
        public class SurveyLink
        {
            public string ENCRYPTED_PATIENT_ACCOUNT { get; set; }
        }
        [Table("FOX_TBL_SURVEY_QUESTION")]
        public class SurveyQuestions
        {
            [Key]
            public long SURVEY_QUESTIONS_ID { get; set; }
            public string SURVEY_QUESTIONS { get; set; }
        }
        [Table("FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG")]
        public class SurveyAutomationLog
        {
            [Key]
            public long SURVEY_AUTOMATION_LOG_ID { get; set; }
            public long PATIENT_ACCOUNT { get; set; }
            public long SURVEY_ID { get; set; }
            public string FILE_NAME { get; set; }
            public long PRACTICE_CODE { get; set; }
            public string CREATED_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool DELETED { get; set; }
        }
    }
}
