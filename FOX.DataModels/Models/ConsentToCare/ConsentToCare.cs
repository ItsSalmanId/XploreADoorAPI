using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.ConsentToCare
{
    public class ConsentToCare
    {
        public class ConsentToCareList : BaseModel
        {
            public List<FoxTblConsentToCare> ConsentToCareDbList { get; set; }
        }
        [Table("FOX_TBL_CONSENT_TO_CARE")]
        public class FoxTblConsentToCare
        {
            [Key]
            public long CONSENT_TO_CARE_ID { get; set; }
            public string SEND_TO { get; set; }
            public string SIGNATORY { get; set; }
            public long? CASE_ID { get; set; }
            public long? STATUS_ID { get; set; }
            public long? PATIENT_ACCOUNT { get; set; }
            public string SIGNATURE_PATH { get; set; }
            public string SOURCE_TYPE { get; set; }
            public DateTime? EXPIRY_DATE_UTC { get; set; }
            public string TEMPLATE_HTML { get; set; }
            public int? FAILED_ATTEMPTS { get; set; }
            public long? SENT_TO_ID { get; set; }
            public string SIGNED_PDF_PATH { get; set; }
            public long? TASK_ID { get; set; }
            public long? PRACTICE_CODE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool DELETED { get; set; }
            [NotMapped]
            public string DESCRIPTION { get; set; }
            [NotMapped]
            public string STATUS { get; set; }
            [NotMapped]
            public DateTime? Date_Of_Birth { get; set; }
            [NotMapped]
            public string PatientLastName { get; set; }
            [NotMapped]
            public string LOGO_PATH { get; set; }
            [NotMapped]
            public string IMAGE_PATH { get; set; }
            [NotMapped]
            public string PATIENT_ACCOUNT_Str { get; set; }
            [NotMapped]
            public string FrpHomePhone { get; set; }
            [NotMapped]
            public string FrpEmailAddress { get; set; }
            [NotMapped]
            public string PoaHomePhone { get; set; }
            [NotMapped]
            public string PoaEmailAddress { get; set; }
            [NotMapped]
            public string PatientEmailAddress { get; set; }
            [NotMapped]
            public string PatientHomePhone { get; set; }
            [NotMapped]
            public string encryptedCaseId { get; set; }
            [NotMapped]
            public string Last_Name { get; set; }
            [NotMapped]
            public string First_Name { get; set; }
            [NotMapped]
            public string Policy_Number { get; set; }
            [NotMapped]
            public string INSURANCE_PAYERS_ID { get; set; }
            [NotMapped]
            public string Gender { get; set; }
            [NotMapped]
            public string PATIENT_FULL_NAME { get; set; }
            [NotMapped]
            public string PROVIDER_FULL_NAME { get; set; }
            [NotMapped]
            public string CONTACTS_FULL_NAME { get; set; }
            [NotMapped]
            public long? TREATING_PROVIDER_ID { get; set; }
            [NotMapped]
            public long? POS_ID { get; set; }
            [NotMapped]
            public string STATUS_NAME { get; set; }
            [NotMapped]
            public string PRI_SEC_OTH_TYPE { get; set; }
            [NotMapped]
            public string lastConsentreceiver { get; set; }
            [NotMapped]
            public string PatientGender { get; set; }
            [NotMapped]
            public string PatientFirstName { get; set; }
            [NotMapped]
            public string PATIENT_NAME { get; set; }
            [NotMapped]
            public string TemplateHtmlWithInsuranceDetails { get; set; }
            [NotMapped]
            public string OrderingRefNotes { get; set; }
            [NotMapped]
            public string lastConsentReceiverName { get; set; }
            [NotMapped]
            public string disciplineName { get; set; }
        }

        [Table("FOX_TBL_SURVEY_QUESTION")]
        public class SurveyQuestions
        {
            [Key]
            public long SURVEY_QUESTIONS_ID { get; set; }
            public string SURVEY_QUESTIONS { get; set; }
        }
        [Table("FOX_TBL_CONSENT_TO_CARE_DOCUMENTS")]
        public class ConsentToCareDocument
        {
            [Key]
            public long DOCUMENTS_ID { get; set; }
            public long CONSENT_TO_CARE_ID { get; set; }
            public string LOGO_PATH { get; set; }
            public string IMAGE_PATH { get; set; }
            public long PRACTICE_CODE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool? DELETED { get; set; } = false;
            public long? DOCUMENT_TYPE_ID { get; set; }
            public bool? IsSigned { get; set; } = false;
        }

        [Table("FOX_TBL_CONSENT_TO_CARE_STATUS")]
        public class ConsentToCareStatus
        {
            [Key]
            public long CONSENT_TO_CARE_STATUS_ID { get; set; }
            public string STATUS_NAME { get; set; }
            public long PRACTICE_CODE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime CREATED_DATE { get; set; }
            public DateTime MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool DELETED { get; set; } = false;
        }
        public class ConsentToCareResponse
        {
            public FoxTblConsentToCare consentToCareObj { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
        }
        public class AddInvalidAttemptRequest
        {
            public long CONSENT_TO_CARE_ID { get; set; }
        }
        public class InsuranceDetails
        {
            public string INSURANCE_NAME { get; set; }
            public long? Co_Payment { get; set; }
            public bool? IS_COPAY_PER { get; set; }
            public string GENERAL_COMMENTS { get; set; }
            public long? PT_ST_TOT_AMT_USED { get; set; }
            public long? OT_TOT_AMT_USED { get; set; }
            public long? YEARLY_DED_AMT { get; set; }
            public bool? Is_Authorization_Required { get; set; } = false;
            public long? DED_REMAINING { get; set; }
            public DateTime? EFFECTIVE_FROM { get; set; }
            public DateTime? EFFECTIVE_TO { get; set; }
            public long? MULT_USED { get; set; }
            public long? MULT_REMAINING { get; set; }
            public long? MULT_VALUE { get; set; }
            public long? DED_MET { get; set; }
            public string AUTH_NUMBER { get; set; }
            public string Pri_Sec_Oth_Type { get; set; }
            public string BENEFIT_COMMENTS { get; set; }

        }
        public class PatientContactDetails
        {
            public string First_Name { get; set; }
            public string Type_Name { get; set; }
            public string Last_Name { get; set; }
        }
    }
}
