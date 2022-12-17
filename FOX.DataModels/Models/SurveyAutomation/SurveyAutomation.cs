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
            public string PATIENT_ACCOUNT_STR { get; set; }
            public string PROVIDER { get; set; }
            public string REGION { get; set; }
            public string PT_OT_SLP { get; set; }
            public string SERVICE_OR_PAYMENT_DESCRIPTION { get; set; }

        }
        [Table("FOX_TBL_SURVEY_QUESTION")]
        public class SurveyQuestions
        {
            [Key]
            public long SURVEY_QUESTIONS_ID { get; set; }
            public string SURVEY_QUESTIONS { get; set; }
        }
    }
}
