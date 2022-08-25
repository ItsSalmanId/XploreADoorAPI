using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.QualityAsuranceModel
{
    public class RequestModelForCallType
    {
        public string Call_Type { get; set; }
    }

    [Table("FOX_TBL_EVALUATION_CRITERIA")]

    public class EvaluationCriteria : BaseModel
    {
        [Key]
        public long EVALUATION_CRITERIA_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CRITERIA_NAME { get; set; }
        public int? PERCENTAGE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string CALL_TYPE { get; set; }
    }

    [Table("FOX_TBL_EVALUATION_CRITERIA_CATEGORIES")]

    public class EvaluationCriteriaCategories : BaseModel
    {
        [Key]
        public long? EVALUATION_CRITERIA_CATEGORIES_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CATEGORIES_NAME { get; set; }
        public long? EVALUATION_CRITERIA_ID { get; set; }
        public int? CATEGORIES_POINTS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string CALL_TYPE { get; set; }
    }

    [Table("FOX_TBL_WOW_FACTOR_NEGATIVE_FEEDBACK")]

    public class WowFactor : BaseModel
    {
        [Key]
        public long? WOW_FACTOR_NEGATIVE_FEEDBACK_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public bool STATUS { get; set; }
        public int? BONUS_POINTS { get; set; }
        public int? PERFORMANCE_KILLER { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string CALL_TYPE { get; set; }
    }

    [Table("FOX_TBL_GRADING_SETUP")]
    public class GradingSetup : BaseModel
    {
        [Key]
        public long? GRADING_SETUP_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string GRADE { get; set; }
        public int? OVERALL_MIN { get; set; }
        public int? OVERALL_MAX { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string CALL_TYPE { get; set; }
    }

    public class EvaluationSetupResponseModel
    {
        public List<EvaluationCriteria> EvaluationCriteria { get; set; }
        public List<EvaluationCriteriaCategories> EvaluationCriteriaCategories { get; set; }
        public List<WowFactor> WowFactor { get; set; }
        public List<GradingSetup> GradingSetup { get; set; }
    }

    public class RequestModelOverallWeightage
    {
        public int Client_Experience { get; set; }
        public int System_Process { get; set; }
        public string CALL_TYPE { get; set; }
       
    }

    public class RequestModelClientExperience
    {
        public int GREETINGS { get; set; }
        public int CALL_HANDLING_SKILLS { get; set; }
        public int GRAMMER_PRONOUNCIATION_VOCABULARY { get; set; }
        public string CALL_TYPE { get; set; }
        public int FOLLOWED_SCRIPTED_SURVEY { get; set; }
        public int FOLLOWED_CLOSING_SCRIPT { get; set; }
        public int VERIFIED_PATIENT_ACCOUNT { get; set; }
        public int CORRECTLY_SEPARATE_QUESTION { get; set; }
        public int APPROPRIATE_GREETING { get; set; }
        public int TONE_OF_PATIENT { get; set; }
        public int COMPASSION_AND_EMPATHY { get; set; }
        public int GRAMMAR_AND_PRONUNCIATION { get; set; }
    }


    public class RequestModelSystemProcess
    {
        public int VERIFIED_PATIENT_ACCOUNT { get; set; }
        public int ACCOUNT_AND_SCRIPT_READINESS { get; set; }
        //public int PATIENT_VERIFICATION_AND_INSURANCE { get; set; }
        public int CASE_HANDLING { get; set; }
        public int EMAIL_TO_STAKE_HOLDERS { get; set; }
        public int ESCALATION { get; set; }
        public int PRODUCT_KNOWLEDGE { get; set; }
        public int CORRECT_NOTES_AND_TRACKING { get; set; }
        public string CALL_TYPE { get; set; }
        public int DOCUMENTED_DATE_TIME { get; set; }
        public int DOCUMENTED_RELEVANT_DETAILS { get; set; }
        public int MARKED_QUSTIONS_CORRECTLY { get; set; }
        public int MARKED_SURVEY_STATUS { get; set; }
        public int CORRECTLY_IDENTIFY_FLAG { get; set; }
        //public int CORRECTLY_SEPARATE_QUESTION { get; set; }
        public int PATIENT_IDENTITY { get; set; }
        public int ANSWER_PATIENT_QUESTIONS { get; set; }
        public int STRONG_PRODUCT_KNOWLEDGE { get; set; }
        public int COMMUNICATE_INFORMATION { get; set; }
        public int DOCUMENTATION_COMPLETED_COMMUNICATED { get; set; }
        public int APPROPRIATE_CLOSING { get; set; }
    }
        public class RequestModelWowfactor
    {
        public bool STATUS { get; set; }
        public int BONUS_POINTS { get; set; }
        public int PERFORMANCE_KILLER { get; set; }
        public string CALL_TYPE { get; set; }
    }

    public class RequestModelGradingSetup
    {
        public int A_MIN { get; set; }
        public int B_MAX { get; set; }
        public int B_MIN { get; set; }
        public int U_MAX { get; set; }
        public string CALL_TYPE { get; set; }
    }
}