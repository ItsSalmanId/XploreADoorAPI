using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FOX.DataModels.Models.PatientSurvey
{

    [Table("FOX_TBL_PATIENT_SURVEY")]
    public class PatientSurvey : BaseModel
    {
        [NotMapped]
        public int ROW { get; set; }
        public string SURVEY_FLAG { get; set; }
        [NotMapped]
        public string MONTH { get; set; }
        public long? PATIENT_ACCOUNT_NUMBER { get; set; }
        [NotMapped]
        public string PATIENT_FULL_NAME { get { return PATIENT_LAST_NAME + ", " + PATIENT_FIRST_NAME; } }
        public string PATIENT_STATE { get; set; }
        public string PT_OT_SLP { get; set; }
        public string REGION { get; set; }
        public string PROVIDER { get; set; }
        public string ATTENDING_DOCTOR_NAME { get; set; }
        public string SURVEY_STATUS_BASE { get; set; }
        public string SURVEY_STATUS_CHILD { get; set; }
        public string FEEDBACK { get; set; }
        [NotMapped]
        public string SURVEY_COMPLETED_DATE_STR { get; set; }
        [NotMapped]
        public string SURVEY_COMPLETED_TIME_STR { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        [NotMapped]
        public string Surveyed_BY_FULL_NAME { get { return Surveyed_BY_LNAME + ", " + Surveyed_BY_FNAME; } }
        [Key]
        public long SURVEY_ID { get; set; }
        [NotMapped]
        public string SURVEY_ID_Str
        {
            get
            {
                return SURVEY_ID.ToString();
            }
            set
            {
                SURVEY_ID = Convert.ToInt64(value);
            }
        }
        public long? PRACTICE_CODE { get; set; }
        public string FACILITY_OR_CLIENT_ID { get; set; }
        public string RESPONSIBLE_PARTY_LAST_NAME { get; set; }
        public string RESPONSIBLE_PARTY_FIRST_NAME { get; set; }
        public string RESPONSIBLE_PARTY_MIDDLE_INITIAL { get; set; }
        public string RESPONSIBLE_PARTY_ADDRESS { get; set; }
        public string RESPONSIBLE_PARTY_CITY { get; set; }
        public string RESPONSIBLE_PARTY_STATE { get; set; }
        public string RESPONSIBLE_PARTY_ZIP_CODE { get; set; }
        public string RESPONSIBLE_PARTY_TELEPHONE { get; set; }
        public string RESPONSIBLE_PARTY_SSN { get; set; }
        public string RESPONSIBLE_PARTY_SEX { get; set; }
        public DateTime? RESPONSIBLE_PARTY_DATE_OF_BIRTH { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_MIDDLE_INITIAL { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        public string PATIENT_CITY { get; set; }
        public string PATIENT_ZIP_CODE { get; set; }
        public string PATIENT_TELEPHONE_NUMBER { get; set; }
        public string PATIENT_SOCIAL_SECURITY_NUMBER { get; set; }
        public string PATIENT_GENDER { get; set; }
        public DateTime? PATIENT_DATE_OF_BIRTH { get; set; }
        public string ALTERNATE_CONTACT_LAST_NAME { get; set; }
        public string ALTERNATE_CONTACT_FIRST_NAME { get; set; }
        public string ALTERNATE_CONTACT_MIDDLE_INITIAL { get; set; }
        public string ALTERNATE_CONTACT_TELEPHONE { get; set; }
        public string EMR_LOCATION_CODE { get; set; }
        public string EMR_LOCATION_DESCRIPTION { get; set; }
        public string SERVICE_OR_PAYMENT_DESCRIPTION { get; set; }
        public DateTime? LAST_VISIT_DATE { get; set; }
        public DateTime? DISCHARGE_DATE { get; set; }
        public DateTime? REFERRAL_DATE { get; set; }
        public string PROCEDURE_OR_TRAN_CODE { get; set; }
        public decimal? SERVICE_OR_PAYMENT_AMOUNT { get; set; }
        public bool? IS_IMPROVED_SETISFACTION { get; set; }
        [NotMapped]
        public string Is_improved_Satisfaction_Str { get; set; }
        public bool? IS_REFERABLE { get; set; }
        [NotMapped]
        public string Is_Referrable_Str { get; set; }
        public bool? IS_CONTACT_HQ { get; set; }
        [NotMapped]
        public string Is_Contact_HQ_Str { get; set; }
        public bool? IS_RESPONSED_BY_HQ { get; set; }
        [NotMapped]
        public string Is_Responsed_By_HQ_Str { get; set; }
        public bool? IS_QUESTION_ANSWERED { get; set; }
        [NotMapped]
        public string Is_Questioned_Answered_Str { get; set; }
        public string SURVEY_FORMAT_TYPE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool? IS_SURVEYED { get; set; }
        public string FILE_NAME { get; set; }
        public string SHEET_NAME { get; set; }
        public long? TOTAL_RECORD_IN_FILE { get; set; }
        public bool DELETED { get; set; }
        public bool? IN_PROGRESS { get; set; }
        [NotMapped]
        public int? PATIENT_AGE { get; set; }
        [NotMapped]
        public int? RESPONSIBLE_PARTY_AGE { get; set; }
        [NotMapped]
        public string DAY_NAME { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public string Surveyed_BY_LNAME { get; set; }
        [NotMapped]
        public string Surveyed_BY_FNAME { get; set; }
        [NotMapped]
        public string ACTIVE_FORMAT { get; set; }
        public bool? IS_EXCEPTIONAL { get; set; }
        public bool? IS_PROTECTIVE_EQUIPMENT { get; set; }
        [NotMapped]
        public string Is_exceptional_Str { get; set; }
        [NotMapped]
        public string Is_protective_equipment_Str { get; set; }
        public DateTime? SURVEY_COMPLETED_DATE { get; set; }
        [NotMapped]
        public int? HAS_CALL_PATH { get; set; }
        [NotMapped]
        public bool? IS_SMS { get; set; }
        [NotMapped]
        public bool? IS_EMAIL { get; set; }
    }
    [Table("FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG")]
    public class SurveyServiceLog
    {
        [Key]
        public long SURVEY_AUTOMATION_LOG_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_PATIENT_SURVEY_HISTORY")]
    public class PatientSurveyHistory
    {
        [Key]
        public long SURVEY_HISTORY_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? SURVEY_ID { get; set; }
        [NotMapped]
        public string SURVEY_ID_Str
        {
            get
            {
                return SURVEY_ID.ToString();
            }
            set
            {
                SURVEY_ID = Convert.ToInt64(value);
            }
        }
        public long? PATIENT_ACCOUNT { get; set; }
        public bool? IS_CONTACT_HQ { get; set; }
        public bool? IS_RESPONSED_BY_HQ { get; set; }
        public bool? IS_REFERABLE { get; set; }
        public bool? IS_IMPROVED_SETISFACTION { get; set; }
        public bool? IS_QUESTION_ANSWERED { get; set; }
        public string FEEDBACK { get; set; }
        public string SURVEY_FLAG { get; set; }
        public string SURVEY_STATUS_BASE { get; set; }
        public string SURVEY_STATUS_CHILD { get; set; }
        public string SURVEY_BY { get; set; }
        public DateTime? SURVEY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool? IS_EXCEPTIONAL { get; set; }
        public bool? IS_PROTECTIVE_EQUIPMENT { get; set; }
        [NotMapped]
        public bool? IS_SMS { get; set; }
        [NotMapped]
        public bool? IS_EMAIL { get; set; }
    }

    [Table("FOX_TBL_PATIENT_SURVEY_CALL_LOG")]
    public class PatientSurveyCallLog: BaseModel
    {
        [Key]
        public long SURVEY_CALL_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? SURVEY_ID { get; set; }
        [NotMapped]
        public string SURVEY_ID_Str
        {
            get
            {
                return SURVEY_ID.ToString();
            }
            set
            {
                SURVEY_ID = Convert.ToInt64(value);
            }
        }
        public long? ACU_CALL_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string FILE_NAME { get; set; }
        public bool? IS_RECEIVED { get; set; }
        public string CALL_OUT_COME { get; set; }
        public string CALL_DURATION { get; set; }
        public bool? IS_TO_PATIENT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string CALL_TO { get; set; }
        [NotMapped]
        public string CALL_BY { get; set; }
        [NotMapped]
        public string SERVICE_OR_PAYMENT_DESCRIPTION { get; set; }

    }

    [Table("FOX_TBL_PATIENT_SURVEY_FORMAT_TYPE")]
    public class PatientSurveyFormatType
    {
        [Key]
        public long SURVEY_FORMAT_TYPE_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string FORMAT_TYPE { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

    }

    public class PatientSurveyExcel
    {
        public string FacilityorClientID { get; set; }
        public string PatientAccountNumber { get; set; }
        public string ResponsiblePartyLastName { get; set; }
        public string ResponsiblePartyFirstName { get; set; }
        public string ResponsiblePartyMiddleInitial { get; set; }
        public string ResponsiblePartyAddress { get; set; }
        public string ResponsiblePartyCity { get; set; }
        public string ResponsiblePartyState { get; set; }
        public string ResponsiblePartyZipCode { get; set; }
        public string ResponsiblePartyTelephone { get; set; }
        public string ResponsiblePartySSN { get; set; }
        public string ResponsiblePartySex { get; set; }
        public string ResponsiblePartyDateofBirth { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string PatientTelephoneNumber { get; set; }
        public string PatientSocialSecurityNumber { get; set; }
        public string PatientGender { get; set; }
        public string PatientDateofBirth { get; set; }
        public string AlternateContactLastName { get; set; }
        public string AlternateContactFirstName { get; set; }
        public string AlternateContactMiddleInitial { get; set; }
        public string AlternateContactTelephone { get; set; }
        public string EMRLocationCode { get; set; }
        public string EMRLocationDescription { get; set; }
        public string ServiceorPaymentDescription { get; set; }
        public string Provider { get; set; }
        public string Region { get; set; }
        public string LastVisitDate { get; set; }
        public string DischargeDate { get; set; }
        public string AttendingDoctorName { get; set; }
        public string PTOTSLP { get; set; }
        public string REFERRALDATEthedatethefilewassenttoSHS { get; set; }
        public string ProcedureCodeTranCode { get; set; }
        public string ServiceorPaymentAmount { get; set; }
    }

    public class PSRRegionAndQuestionWise
    {
        public string REGION { get; set; }
        public int? IS_IMPROVED_SETISFACTION_YES { get; set; }
        public int? IS_IMPROVED_SETISFACTION_NO { get; set; }
        public int? IS_REFERABLE_YES { get; set; }
        public int? IS_REFERABLE_NO { get; set; }
        public int? IS_CONTACT_HQ_YES { get; set; }
        public int? IS_CONTACT_HQ_NO { get; set; }
        public int? IS_RESPONSED_BY_HQ_YES { get; set; }
        public int? IS_RESPONSED_BY_HQ_NO { get; set; }
        public int? IS_QUESTION_ANSWERED_YES { get; set; }
        public int? IS_QUESTION_ANSWERED_NO { get; set; }
        public int? IS_CONTACT_HQ_YES_AVG { get; set; }
        public int? IS_CONTACT_HQ_NO_AVG { get; set; }
        public int? IS_RESPONSED_BY_HQ_YES_AVG { get; set; }
        public int? IS_RESPONSED_BY_HQ_NO_AVG { get; set; }
        public int? IS_QUESTION_ANSWERED_YES_AVG { get; set; }
        public int? IS_QUESTION_ANSWERED_NO_AVG { get; set; }
        public int? IS_REFERABLE_YES_AVG { get; set; }
        public int? IS_REFERABLE_NO_AVG { get; set; }
        public int? IS_IMPROVED_SETISFACTION_YES_AVG { get; set; }
        public int? IS_IMPROVED_SETISFACTION_NO_AVG { get; set; }
        public int? IS_PROTECTIVE_EQUIPMENT_YES { get; set; }
        public int? IS_PROTECTIVE_EQUIPMENT_NO { get; set; }
        public int? IS_PROTECTIVE_EQUIPMENT_YES_AVG { get; set; }
        public int? IS_PROTECTIVE_EQUIPMENT_NO_AVG { get; set; }
    }

    public class PSRProviderAndQuestionWise
    {
        public string PROVIDER { get; set; }
        public int? IS_CONTACT_HQ_YES { get; set; }
        public int? IS_CONTACT_HQ_NO { get; set; }
        public int? IS_CONTACT_HQ_YES_AVG { get; set; }
        public int? IS_CONTACT_HQ_NO_AVG { get; set; }
        public int? IS_RESPONSED_BY_HQ_YES { get; set; }
        public int? IS_RESPONSED_BY_HQ_NO { get; set; }
        public int? IS_RESPONSED_BY_HQ_YES_AVG { get; set; }
        public int? IS_RESPONSED_BY_HQ_NO_AVG { get; set; }
        public int? IS_QUESTION_ANSWERED_YES { get; set; }
        public int? IS_QUESTION_ANSWERED_NO { get; set; }
        public int? IS_QUESTION_ANSWERED_YES_AVG { get; set; }
        public int? IS_QUESTION_ANSWERED_NO_AVG { get; set; }
        public int? IS_REFERABLE_YES { get; set; }
        public int? IS_REFERABLE_NO { get; set; }
        public int? IS_REFERABLE_YES_AVG { get; set; }
        public int? IS_REFERABLE_NO_AVG { get; set; }
        public int? IS_IMPROVED_SETISFACTION_YES { get; set; }
        public int? IS_IMPROVED_SETISFACTION_NO { get; set; }
        public int? IS_IMPROVED_SETISFACTION_YES_AVG { get; set; }
        public int? IS_IMPROVED_SETISFACTION_NO_AVG { get; set; }
        public int? IS_PROTECTIVE_EQUIPMENT_YES { get; set; }
        public int? IS_PROTECTIVE_EQUIPMENT_NO { get; set; }
    }

    public class PSRRegionAndRecommendationWise
    {
        public string REGION { get; set; }
        public int RECOMMENDED { get; set; }
        public int NOT_RECOMMENDED { get; set; }
        public int RECOMMENDED_AVG { get; set; }
        public int NOT_RECOMMENDED_AVG { get; set; }
    }
    public class RegionWisePatientData
    {
        public int ROW { get; set; }
        public string NAME { get; set; }
        public string SERVICE_OR_PAYMENT_DESCRIPTION { get; set; }
        public string FEEDBACK { get; set; }
        public DateTime? DISCHARGE_DATE { get; set; }
        public string DISCHARGE_DATE_STR { get; set; }
        public DateTime? REFERRAL_DATE { get; set; }
        public string REFERRAL_DATE_STR { get; set; }
        public string PATIENT_STATE { get; set; }
        public string REGION { get; set; }
        public int TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }
    public class PSRProviderAndRecommendationWise
    {
        public string PROVIDER { get; set; }
        public int RECOMMENDED { get; set; }
        public int NOT_RECOMMENDED { get; set; }
        public int RECOMMENDED_AVG { get; set; }
        public int NOT_RECOMMENDED_AVG { get; set; }
    }

    public class PSDResults
    {
        public int NOT_INTERESTED { get; set; }
        public int CALLBACK { get; set; }
        public int NOT_ANSWERED { get; set; }
        public int DECEASED { get; set; }
        public int COMPLETED { get; set; }
        public int INCOMPLETED { get; set; }
        public int NOT_CONVERTED { get; set; }
        public int RECOMMENDED { get; set; }
        public int NOT_RECOMMENDED { get; set; }
        public int IS_IMPROVED_SETISFACTION_YES { get; set; }
        public int IS_IMPROVED_SETISFACTION_NO { get; set; }
        public int IS_REFERABLE_YES { get; set; }
        public int IS_REFERABLE_NO { get; set; }
        public int IS_CONTACT_HQ_YES { get; set; }
        public int IS_CONTACT_HQ_NO { get; set; }
        public int IS_RESPONSED_BY_HQ_YES { get; set; }
        public int IS_RESPONSED_BY_HQ_NO { get; set; }
        public int IS_QUESTION_ANSWERED_YES { get; set; }
        public int IS_QUESTION_ANSWERED_NO { get; set; }
        public int IS_PROTECTIVE_EQUIPMENT_YES { get; set; }
        public int IS_PROTECTIVE_EQUIPMENT_NO { get; set; }

        public List<PSDStateAndRegionRecommendationWise> recommendationData { get; set; }
    }
    
    public class PSUserList
    {
        public string FULL_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string USER_NAME { get; set; }
    }

    public class PatientSurveySearchRequest: BaseModel
    {
        public long SURVEY_ID { get; set; }
        public string PATIENT_ACCOUNT_NUMBER { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_MIDDLE_INITIAL { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string DATE_FROM_STR { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public string PROVIDER { get; set; }
        public string ATTENDING_DOCTOR_NAME { get; set; }
        public string REGION { get; set; }
        public string STATE { get; set; }
        public string FLAG { get; set; }
        public string DISCIPLINE { get; set; }
        public string FORMAT { get; set; }
        public string SURVEYED_BY { get; set; }
        public string SURVEYED_STATUS_BASE { get; set; }
        public string SURVEYED_STATUS_CHILD { get; set; }
        public int RECOMMENDED { get; set; }
        public int NOT_RECOMMENDED { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public int TIME_FRAME { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public int IS_SURVEYED { get; set; }
    }

    public class PatientSurveyCall
    {
        public string Number { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
    }

    public class PSSearchData
    {
        public List<string> Providers { get; set; }
        public List<string> States { get; set; }
        public List<string> Regions { get; set; }
        public List<PSUserList> Users { get; set; }
    }

    public class PSInitialData
    {
        public PSDResults psdData { get; set; }
        public PSSearchData psSearchDataResult { get; set; }
    }

    public class PSDStateAndRegionRecommendationWise
    {
        public string STATE { get; set; }
        public string REGION { get; set; }
        public int? RECOMMENDED { get; set; }
        public int? NOT_RECOMMENDED { get; set; }
    }
    public class PSDRChartData
    {
        public int COMPLETED { get; set; }
        public int COMPLETED_SURVEY { get; set; }
        public int RECOMMENDED { get; set; }
        public int NOT_RECOMMENDED { get; set; }
        public int DECEASED { get; set; }
        public int INCOMPLETE { get; set; }
        public int CALL_BACK { get; set; }
        public int NOT_ANSWERED { get; set; }
        public int NEW_CASE_SAME_DISCIPLINE { get; set; }
        public int NOT_INTERESTED { get; set; }
        public int PENDING_30 { get; set; }
        public int PENDING_ALL { get; set; }
        public int NOT_ENOUGH_SERVICES_PROVIDE { get; set; }
        public int DISCHARGE_TO_SURVEY_TIME_DAYS_AVERAGE { get; set; }
    }
    public class AverageDaysSurveyCompleted
    {
        public int AVERAGE_DAY { get; set; }
    }
   
}