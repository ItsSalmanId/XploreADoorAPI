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
    public class SurveyAutomation
    {
        public string Patient_AccountStr { get; set; }
        public string PROVIDER { get; set; }
        public string REGION { get; set; }
        public string PT_OT_SLP { get; set; }
        public string SERVICE_OR_PAYMENT_DESCRIPTION { get; set; }

    }
    [Table("FOX_TBL_PATIENT_SURVEY_QUESTIONS")]
    public class SurveyQuestions
    {
        [Key]
        public long PATIENT_SURVEY_QUESTIONS_ID { get; set; }
        public string QUESTION_ONE_DESCRIPTION { get; set; }
        public string QUESTION_TWO_DESCRIPTION { get; set; }
        public string QUESTION_THREE_DESCRIPTION { get; set; }
        public bool? DELETED { get; set; }

    }
    class SurveyAutomations
    {
        [Table("Patient")]
        public class Patient : BaseModel
        {
            [Key]
            public long Patient_Account { get; set; }
            [NotMapped]
            public int ROW { get; set; }
            [NotMapped]
            public string Patient_AccountStr
            {
                get
                {
                    return Patient_Account.ToString();
                }
                set
                {
                    Patient_Account = Convert.ToInt64(value);
                }
            }
            public string City { get; set; }
            [NotMapped]
            public string HomeAddress { get; set; }
            [NotMapped]
            public bool? IS_ACQUISITION { get; set; }
            public string State { get; set; }
            public string ZIP { get; set; }
            public long Practice_Code { get; set; }
            public string Chart_Id { get; set; }
            //public string FirstName;
            [NotMapped]
            public string FirstName { get; set; }

            public string First_Name
            {
                get { return FirstName; }
                set
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    FirstName = !string.IsNullOrEmpty(value) ? textInfo.ToTitleCase(value.ToLower()) : "";
                }
            }

            [NotMapped]
            public string LastName { get; set; }
            public string Last_Name
            {
                get
                {
                    return LastName;
                }
                set
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    LastName = !string.IsNullOrEmpty(value) ? textInfo.ToTitleCase(value.ToLower()) : "";
                }
            }
            [NotMapped]
            public string MIDDLENAME { get; set; }

            public string MIDDLE_NAME
            {
                get
                {
                    return MIDDLENAME;
                }
                set
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    MIDDLENAME = !string.IsNullOrEmpty(value) ? textInfo.ToTitleCase(value.ToLower()) : "";
                }
            }

            public string SSN { get; set; }
            public DateTime? Date_Of_Birth { get; set; }
            public string Gender { get; set; }
            public string Email_Address { get; set; }
            public string Home_Phone { get; set; }
            public string cell_phone { get; set; }
            public string Address { get; set; }
            public string Business_Phone { get; set; }
            public long? Financial_Guarantor { get; set; }

            [NotMapped]
            public string CreatedBy { get; set; }
            public string Created_By
            {
                get
                {
                    return CreatedBy;
                }
                set
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    CreatedBy = !string.IsNullOrEmpty(value) ? textInfo.ToTitleCase(value.ToLower()) : "";
                }
            }


            [NotMapped]
            public string ModifiedBy { get; set; }
            public string Modified_By
            {
                get
                {
                    return ModifiedBy;
                }
                set
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    ModifiedBy = !string.IsNullOrEmpty(value) ? textInfo.ToTitleCase(value.ToLower()) : "";
                }
            }
            public DateTime? Created_Date { get; set; }
            public DateTime? Modified_Date { get; set; }
            public bool? DELETED { get; set; }
            [NotMapped]
            public string Title { get; set; }
            [NotMapped]
            public string Best_Time_of_Call_Home { get; set; }
            [NotMapped]
            public string Best_Time_of_Call_Work { get; set; }
            [NotMapped]
            public string Best_Time_of_Call_Cell { get; set; }
            [NotMapped]
            public string Fax_Number { get; set; }
            //public string Email_Password { get; set; }
            [NotMapped]
            public long? PCP { get; set; }
            [NotMapped]
            public string Employment_Status { get; set; }
            [NotMapped]
            public string Patient_Status { get; set; }
            [NotMapped]
            public string Student_Status { get; set; }
            [NotMapped]
            public int? FINANCIAL_CLASS_ID { get; set; }
            public string Marital_Status { get; set; }
            [NotMapped]
            public bool? Expired { get; set; }

            public long? EMPLOYER_CODE { get; set; }
            public bool? chk_Hospice { get; set; }
            [NotMapped]
            public bool? CHK_ABN { get; set; }
            [NotMapped]
            public bool? CHK_HOME_HEALTH_EPISODE { get; set; }

            [NotMapped]
            public string Date_Of_Birth_In_String { get; set; }
            [NotMapped]
            public bool IsRegister { get; set; }
            [NotMapped]
            public double TOTAL_RECORD_PAGES { get; set; }
            [NotMapped]
            public int TOTAL_RECORDS { get; set; }
            [NotMapped]
            public List<PatientAddress> Patient_Address { get; set; }
            [NotMapped]
            public List<PatientInsurance> PatientInsurance { get; set; }
            [NotMapped]
            public string PCP_Name { get; set; }
            [NotMapped]
            public string PCP_Notes { get; set; }
            [NotMapped]
            public List<PatientPOSLocation> Patient_POS_Location_List { get; set; }
            [NotMapped]
            public List<PatientContact> Patient_Contacts_List { get; set; }
            [NotMapped]
            public string PrimaryInsuranceName { get; set; }
            [NotMapped]
            public long? PrimaryInsuranceID { get; set; }
            [NotMapped]
            public string FINANCIAL_CLASS { get; set; }
            [NotMapped]
            public List<PatientInsurance> Current_Patient_Insurances { get; set; }
            [NotMapped]
            public List<FinancialClass> FinancialClassList { get; set; }
            [NotMapped]
            public string POA_EMERGENCY_CONTACT { get; set; }
            [NotMapped]
            public long? PRACTICE_ORGANIZATION_ID { get; set; }
            public List<string> PlaceOfServicesToDeleteIds { get; set; }
            [NotMapped]
            public bool IsHomePhoneFromSLC { get; set; }
            public DateTime? Expiry_Date { get; set; }
            [NotMapped]
            public string Expiry_Date_In_Str { get; set; }
            [NotMapped]
            public bool is_Change { get; set; }
            [NotMapped]
            public FoxPHD.PhdPatientVerification PhdpatientverificationObj { get; set; }
            [NotMapped]
            public string USER_NAME { get; set; }
            [NotMapped]
            public long USER_ID { get; set; }
            public long? Referring_Physician { get; set; }
            [NotMapped]
            public bool IS_PATIENT_INTERFACE_SYNCED { get; set; }
            [NotMapped]
            public bool IS_WORK_ORDER_INTERFACE_SYNCED { get; set; }
            [NotMapped]
            public bool IS_PATIENT_OLD_OR_SYNCED { get; set; }
            [NotMapped]
            public bool FROM_INDEXINFO { get; set; }
            // For Allias Patient
            [NotMapped]
            public List<PatientAlias> Patient_Alias_List { get; set; }
            [NotMapped]
            public long? PATIENT_ALIAS_ID { get; set; }
            [NotMapped]
            public string ALIAS_TRACKING_NUMBER { get; set; }
            [NotMapped]
            public string RT_ALIAS_TRACKING { get; set; }
            [NotMapped]
            public string FIRST_NAME_ALIAS { get; set; }
            [NotMapped]
            public string MIDDLE_INITIALS_ALIAS { get; set; }
            [NotMapped]
            public string LAST_NAME_ALIAS { get; set; }
            [NotMapped]
            public string PATIENT_FINANCIAL_CLASS { get; set; }
            public bool? Address_To_Guarantor { get; set; }
            public string Address_Type { get; set; }

        }
    }
}
