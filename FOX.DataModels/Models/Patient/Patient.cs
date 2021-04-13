using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Settings.FacilityLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FOX.DataModels.Models.Patient
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

    }

    public class PatientExportToExcelModel
    {
        public int ROW { get; set; }
        public string Patient_Account { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string MI { get; set; }

        public string FIRST_NAME_ALIAS { get; set; }
        public string LAST_NAME_ALIAS { get; set; }
        public string MIDDLE_INITIALS_ALIAS { get; set; }

        public string MRN { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date_Time { get; set; }
        public string Updated_By { get; set; }
        public DateTime? Update_Date_Time { get; set; }

    }

    [Table("FOX_TBL_PATIENT")]
    public partial class FOX_TBL_PATIENT
    {
        [Key]
        public long FOX_TBL_PATIENT_ID { get; set; }
        public long? Patient_Account { get; set; }
        public string Title { get; set; }
        public string Best_Time_of_Call_Home { get; set; }
        public string Best_Time_of_Call_Work { get; set; }
        public string Best_Time_of_Call_Cell { get; set; }
        public string Fax_Number { get; set; }
        public long? PCP { get; set; }
        public string Employment_Status { get; set; }
        public string Patient_Status { get; set; }
        public string Student_Status { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        public bool? Expired { get; set; }
        public bool? CHK_ABN { get; set; }
        public bool? CHK_HOME_HEALTH_EPISODE { get; set; }
        public string POA_EMERGENCY_CONTACT { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool? DELETED { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public string Is_Opened_By { get; set; }
    }


    [Table("FOX_TBL_PATIENT_ADDRESS")]
    public class PatientAddress
    {
        [Key]
        public long PATIENT_ADDRESS_HISTORY_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string ADDRESS { get; set; }
        public string Home_Phone { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string ADDRESS_TYPE { get; set; }
        public bool? DELETED { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string COUNTRY { get; set; }
        public bool? Same_As_POS { get; set; }

        [NotMapped]
        public bool To_Be_Deleted { get; set; }

        [NotMapped]
        public List<string> CitiesList { get; set; }
        public long? PATIENT_POS_ID { get; set; }
        public string POS_Phone { get; set; }
        public string POS_Work_Phone { get; set; }
        public string POS_Cell_Phone { get; set; }
        public string POS_Fax { get; set; }
        public string POS_Email_Address { get; set; }
        public string POS_REGION { get; set; }
        public string POS_County { get; set; }
        public Single? Longitude { get; set; }
        public Single? Latitude { get; set; }
        public long? WEBEHR_PATIENT_ADDRESS_ID { get; set; }

    }

    [Table("FOX_TBL_PATIENT_PHONE")]
    public class PatientPhone
    {
        [Key]
        public long PATIENT_PHONE_ID { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public string PHONE_TYPE { get; set; }
        public string PHONE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_PATIENT_UPDATE_HISTORY")]
    public class PatientUpdateHistory
    {
        [Key]
        public long patient_update_history_id { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public string field_name { get; set; }
        public string previous_value { get; set; }
        public string new_value { get; set; }
        public string ip_address { get; set; }
        public string updated_by { get; set; }
        [NotMapped]
        public string USER_NAME { get; set; }
        public DateTime? updated_time { get; set; }
        public bool? isActive { get; set; }
        public bool? isDeleted { get; set; }
    }

    public class PatientSearchRequest : BaseModel
    {
        public string Patient_Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MRN { get; set; }
        public string SSN { get; set; }
        public DateTime? DOB { get; set; }
        public string DOBInString { get; set; }
        public string Gender { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInString { get; set; }
        public string ModifiedBy { get; set; }
        public long PracticeCode { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public bool INCLUDE_ALIAS { get; set; }
    }

    //public class ZipCityState
    //{
    //    public string ZIP_CODE { get; set; }
    //    public string CITY_NAME { get; set; }
    //    public string STATE_CODE { get; set; }
    //}

    public class ZipCity
    {
        public string CITY_NAME { get; set; }
    }

    /// <summary>
    /// PAtient Insurance models
    /// </summary>
    [Table("Patient_Insurance")]
    public class MTBCPatientInsurance
    {
        [Key]
        public long Patient_Insurance_Id { get; set; }
        public long? Patient_Account { get; set; }
        public long Insurance_Id { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
        public decimal? Co_Payment { get; set; }
        public decimal? Deductions { get; set; }
        public string Policy_Number { get; set; }
        public string Group_Number { get; set; }
        public DateTime? Effective_Date { get; set; }
        public DateTime? Termination_Date { get; set; }
        public long? Subscriber { get; set; }
        public string Relationship { get; set; }
        public string Eligibility_Status { get; set; }
        //public Nullable<long> Eligibility_S_No { get; set; }
        //public Nullable<System.DateTime> Eligibility_Enquiry_Date { get; set; }
        //public string Access_Carolina_Number { get; set; }
        //public Nullable<bool> Is_Capitated_Patient { get; set; }
        //public Nullable<int> Allowed_Visits { get; set; }
        //public Nullable<int> Remaining_Visits { get; set; }
        //public Nullable<System.DateTime> Visits_Start_Date { get; set; }
        //public Nullable<System.DateTime> Visits_End_Date { get; set; }
        //public Nullable<long> HL7PatInsurance_S_No { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool? Deleted { get; set; }
        //public DateTime? Eligibility_DOS { get; set; }
        //public string CCN { get; set; }
        //public DateTime Sync_Date { get; set; }
        //public Guid rowguid { get; set; }
        //public string Group_Name { get; set; }
        //public string CREATED_FROM { get; set; }
        //public string MCR_SEC_Payer { get; set; }
        //public Nullable<long> MCR_SEC_Payer_Code { get; set; }
        //public string Eligibility_Difference { get; set; }
        //public string Filing_Indicator_Code { get; set; }
        //public string Filing_Indicator { get; set; }
        //public string Plan_type { get; set; }
        //public string coverage_description { get; set; }
        //public int? CO_PAYMENT_PER { get; set; }
        public string Plan_Name { get; set; }
        //public string PLAN_NAME_TYPE { get; set; }
        //public Nullable<bool> INACTIVE { get; set; }
        //public Nullable<System.DateTime> SUPRESS_BILLING_UNTIL { get; set; }
        //public Nullable<bool> IS_COPAY_PER_VISIT { get; set; }
        //public Nullable<System.DateTime> DED_AMT_VERIFIED_ON { get; set; }
        //public string DED_POLICY_LIMIT_RESET_ON { get; set; }
        //public Nullable<decimal> YEARLY_DED_AMT { get; set; }
        //public Nullable<decimal> DED_MET { get; set; }
        //public Nullable<System.DateTime> DED_MET_AS_OF { get; set; }
        //public Nullable<decimal> DED_REMAINING { get; set; }
        //public Nullable<bool> IS_PT_ST_THRESHOLD_REACHED { get; set; }
        //public Nullable<bool> IS_OT_THRESHOLD_REACHED { get; set; }
        //public Nullable<decimal> PT_ST_TOT_AMT_USED { get; set; }
        //public Nullable<decimal> PT_ST_RT_AMT { get; set; }
        //public Nullable<decimal> PT_ST_OUTSIDE_AMT_USED { get; set; }
        //public Nullable<decimal> OT_TOT_AMT_USED { get; set; }
        //public Nullable<decimal> OT_RT_AMT { get; set; }
        //public Nullable<decimal> OT_OUTSIDE_AMT_USED { get; set; }
        //public Nullable<decimal> PT_ST_YTD_AMT { get; set; }
        //public Nullable<decimal> OT_YTD_AMT { get; set; }
        //public Nullable<System.DateTime> BENEFIT_AMT_VERIFIED_ON { get; set; }
        //public string BENEFIT_POLICY_LIMIT_RESET_ON { get; set; }
        //public Nullable<decimal> MYB_LIMIT_DOLLARS { get; set; }
        //public Nullable<int> MYB_LIMIT_VISIT { get; set; }
        //public Nullable<decimal> MYB_USED_OUTSIDE_DOLLARS { get; set; }
        //public Nullable<int> MYB_USED_OUTSIDE_VISIT { get; set; }
        //public Nullable<decimal> MYB_USED_DOLLARS { get; set; }
        //public Nullable<int> MYB_USED_VISIT { get; set; }
        //public Nullable<decimal> MYB_REMAINING_DOLLARS { get; set; }
        //public Nullable<int> MYB_REMAINING_VISIT { get; set; }
        //public Nullable<decimal> MOP_AMT { get; set; }
        //public Nullable<decimal> MOP_USED_OUTSIDE_RT { get; set; }
        //public Nullable<decimal> MOP_USED { get; set; }
        //public Nullable<decimal> MOP_REMAINING { get; set; }
        //public string SPOKE_TO { get; set; }
        //public Nullable<long> CASE_ID { get; set; }
        //public string BENEFIT_COMMENTS { get; set; }
        //public string GENERAL_COMMENTS { get; set; }
        //public string FOX_INSURANCE_STATUS { get; set; }
        //public string VERIFIED_BY { get; set; }
        //public Nullable<bool> IS_COPAY_PER { get; set; }
        //public Nullable<bool> IS_VERIFIED { get; set; }
        //public Nullable<long> Plan_id { get; set; }
    }

    //Eligibility
    //[Table("FOX_TBL_PATIENT_ELIGIBILITY")]
    [Table("FOX_TBL_PATIENT_INSURANCE")]
    public class PatientInsurance
    {
        [Key]
        public long Patient_Insurance_Id { get; set; }
        public long? MTBC_Patient_Insurance_Id { get; set; }
        public long Patient_Account { get; set; }
        public long? Parent_Patient_insurance_Id { get; set; }
        public long Insurance_Id { get; set; }
        public long FOX_TBL_INSURANCE_ID { get; set; }
        public string Policy_Number { get; set; }
        public string Group_Number { get; set; }
        public decimal? Co_Payment { get; set; }
        public decimal? Deductions { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
        public DateTime? Effective_Date { get; set; }
        [NotMapped]
        public string Effective_Date_In_String { get; set; }
        public DateTime? Termination_Date { get; set; }
        [NotMapped]
        public string Termination_Date_In_String { get; set; }
        public string Relationship { get; set; }
        [NotMapped]
        public string InsPayer_Id { get; set; }
        [NotMapped]
        public string InsPayer_Description { get; set; }
        [NotMapped]
        public string Ins_Type { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool? Deleted { get; set; }
        public string Plan_Name { get; set; }
        public long? Subscriber { get; set; }
        public bool? INACTIVE { get; set; }
        public DateTime? SUPRESS_BILLING_UNTIL { get; set; }
        [NotMapped]
        public string SUPRESS_BILLING_UNTIL_DATE_IN_STRING { get; set; }
        public bool? IS_COPAY_PER_VISIT { get; set; }
        public DateTime? DED_AMT_VERIFIED_ON { get; set; }
        [NotMapped]
        public string DED_AMT_VERIFIED_ON_DATE_IN_STRING { get; set; }
        public string DED_POLICY_LIMIT_RESET_ON { get; set; }
        public decimal? YEARLY_DED_AMT { get; set; }
        public decimal? DED_MET { get; set; }
        public DateTime? DED_MET_AS_OF { get; set; }
        [NotMapped]
        public string DED_MET_AS_OF_IN_STRING { get; set; }
        public decimal? DED_REMAINING { get; set; }
        public bool? IS_PT_ST_THRESHOLD_REACHED { get; set; }
        public bool? IS_OT_THRESHOLD_REACHED { get; set; }
        public decimal? PT_ST_TOT_AMT_USED { get; set; }
        public decimal? PT_ST_RT_AMT { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PT_ST_OUTSIDE_AMT_USED { get; set; }//Ask Ali, because this is computed field
        public decimal? OT_TOT_AMT_USED { get; set; }
        public decimal? OT_RT_AMT { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? OT_OUTSIDE_AMT_USED { get; set; }//Ask Ali, because this is computed field
        public decimal? PT_ST_YTD_AMT { get; set; }
        public decimal? OT_YTD_AMT { get; set; }
        public DateTime? BENEFIT_AMT_VERIFIED_ON { get; set; }
        [NotMapped]
        public string BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING { get; set; }
        public string BENEFIT_POLICY_LIMIT_RESET_ON { get; set; }
        public decimal? MYB_LIMIT_DOLLARS { get; set; }
        public int? MYB_LIMIT_VISIT { get; set; }
        public decimal? MYB_USED_OUTSIDE_DOLLARS { get; set; }
        public int? MYB_USED_OUTSIDE_VISIT { get; set; }
        public decimal? MYB_USED_DOLLARS { get; set; }
        public int? MYB_USED_VISIT { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? MYB_REMAINING_DOLLARS { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? MYB_REMAINING_VISIT { get; set; }
        public decimal? MOP_AMT { get; set; }
        public decimal? MOP_USED_OUTSIDE_RT { get; set; }
        public decimal? MOP_USED { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? MOP_REMAINING { get; set; }
        public string SPOKE_TO { get; set; }
        public long? CASE_ID { get; set; }
        public string BENEFIT_COMMENTS { get; set; }
        public string GENERAL_COMMENTS { get; set; }
        public string FOX_INSURANCE_STATUS { get; set; }
        public string VERIFIED_BY { get; set; }
        public bool? IS_COPAY_PER { get; set; }
        public bool? IS_VERIFIED { get; set; }
        public DateTime? VERIFIED_DATE { get; set; }

        [NotMapped]
        public Subscriber SUBSCRIBER_DETAILS { get; set; }

        [NotMapped]
        public int DisplayOrder { get; set; }
        [NotMapped]
        public string CASE_NO { get; set; }
        [NotMapped]
        public string RT_CASE_NO { get; set; }
        [NotMapped]
        public decimal? AuthGrandTotal { get; set; }
        [NotMapped]
        public decimal? AuthGrandTotalUsed { get; set; }
        [NotMapped]
        public decimal? AuthGrandTotalRemaining { get; set; }
        //------------------------------------------------------
        public bool? CHK_ABN { get; set; }
        public bool? CHK_HOSPICE { get; set; }
        public bool? CHK_HOME_HEALTH_EPISODE { get; set; }
        public long? ABN_LIMIT_ID { get; set; }
        public long? HOSPICE_LIMIT_ID { get; set; }
        public long? HOME_HEALTH_LIMIT_ID { get; set; }

        [NotMapped]
        public List<MedicareLimit> CurrentMedicareLimitList { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }

        public decimal? PERIODIC_PAYMENT { get; set; }

        public int? PR_PERIOD_ID { get; set; }

        public int? PR_DISCOUNT_ID { get; set; }

        public bool? IsWellness { get; set; }
        public bool? IsSkilled { get; set; }
        public bool? IS_PRIVATE_PAY { get; set; }

        public string FOX_GUID { get; set; }
        public DateTime? Deceased_Date { get; set; }
        public string FOX_ELIGIBILITY_GUID { get; set; }
        public DateTime? ELIG_LOADED_ON { get; set; }
        [NotMapped]
        public string Deceased_Date_In_String { get; set; }
        [NotMapped]
        public List<ClaimInsuranceViewModel> ClaimsToMapToNewInsurance { get; set; }
        public bool? Is_Authorization_Required { get; set; }
        [NotMapped]
        public List<long> Patient_Insurancs_Ids_With_Overlapping_Dates { get; set; }
        [NotMapped]
        public string FINANCIAL_CLASS_CODE { get; set; }
        [NotMapped]
        public string Eligibility_Status { get; set; }
        [NotMapped]
        public string CURRENT_DATE_STR { get; set; }
        [NotMapped]
        public string Eligibility_MSP_Data { get; set; }
        [NotMapped]
        public bool IS_MVP_VIEW { get; set; }
        [NotMapped]
        public long Work_ID { get; set; }
    }

    public class PatientInsuranceDetail
    {
        public long? Patient_Insurance_Id { get; set; }
        public long? Patient_Account { get; set; }
        public string Patient_Account_Str
        {
            get
            {
                return Patient_Account.ToString();
            }
            set
            {
                long _lg;
                Patient_Account = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        public long? Insurance_Id { get; set; }
        public long? FOX_TBL_INSURANCE_ID { get; set; }
        public string InsPayer_Id { get; set; }
        public string InsPayer_Description { get; set; }
        public string INSURANCE_NAME { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
        public decimal? Co_Payment { get; set; }
        public decimal? Deductions { get; set; }
        public string Policy_Number { get; set; }
        public string Group_Number { get; set; }
        public DateTime? Effective_Date { get; set; }
        public DateTime? Termination_Date { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string Created_By { get; set; }
        public string Modified_By { get; set; }
        public string Relationship { get; set; }
        public string Insurance_Phone_Number1 { get; set; }
        public string FAX { get; set; }
        public string Insurance_Address { get; set; }
        public string Insurance_Zip { get; set; }
        public string Insurance_City { get; set; }
        public string Insurance_State { get; set; }
        public bool? Deleted { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        public bool? INACTIVE { get; set; }
        public string Plan_Name { get; set; }
        public DateTime? SUPRESS_BILLING_UNTIL { get; set; }
        public Subscriber SUBSCRIBER_DETAILS { get; set; }
        public long? Subscriber { get; set; }
        public string Name
        {
            get
            {
                return InsPayer_Description.ToTitleCase();
            }
        }
        public string Template
        {
            get
            {
                string zipcode = string.IsNullOrEmpty(Insurance_Zip) ? "" : Insurance_Zip.Length > 5 ? Insurance_Zip.Substring(0, 5) + '-' + Insurance_Zip.Substring(5) : Insurance_Zip;
                return $"<p>[{ InsPayer_Description.ToTitleCase() }] <br />Phone:  { (!string.IsNullOrWhiteSpace(Insurance_Phone_Number1) && Insurance_Phone_Number1.Length == 10 ? (String.Format("{0:(###) ###-####}", double.Parse(Insurance_Phone_Number1))) : Insurance_Phone_Number1)} <br /> Address:  {Insurance_Address.ToTitleCase()}, {Insurance_City.ToTitleCase()}, {Insurance_State.ToTitleCase()} {zipcode} </p>";
            }
        }

        public string FOX_INSURANCE_STATUS { get; set; }
        public bool IS_PRIVATE_PAY { get; set; }
        public string FINANCIAL_CLASS_CODE { get; set; }
        public long MTBC_Patient_Insurance_Id { get; set; }

    }

    public class PatientInsuranceInformation
    {
        public int? INS_TYPE { get; set; }
        public long PATIENT_INSURANCE_ID { get; set; }
        public long Parent_Patient_insurance_Id { get; set; }
        public string RELATIONSHIP { get; set; }
        public string GROUP_NUMBER { get; set; }
        public string POLICY_NUMBER { get; set; }
        public string ZIP { get; set; }
        public string STATE { get; set; }
        public string ADDRESS { get; set; }
        public string LAST_NAME { get; set; }
        public string CITY { get; set; }
        public string FIRST_NAME { get; set; }
        public string SSN { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public string GENDER { get; set; }
        public string GURANTOR_LNAME { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public string INSPAYER_DESCRIPTION { get; set; }
        public string GURANTOR_FNAME { get; set; }
        public DateTime GUARANTOR_DOB { get; set; }
        public string GUARANTOR_SSN { get; set; }
        public string PRI_SEC_OTH_TYPE { get; set; }
        public string GUARANTOR_GENDER { get; set; }
        public string PRACTICE_NAME { get; set; }
        public string PRACTICE_NPI { get; set; }
        public long INSURANCE_ID { get; set; }
        public long FOX_TBL_INSURANCE_ID { get; set; }
        public long INSPAYER_ID { get; set; }
        public string PRACTICE_TAX_ID { get; set; }
        public string PROVIDER_LNAME { get; set; }
        public string GUARANT_ADDRESS { get; set; }
        public string GUARANT_CITY { get; set; }
        public string GUARANT_STATE { get; set; }
        public string GUARANT_ZIP { get; set; }
        public string PROVIDER_FNAME { get; set; }
        public string PROVID_STATE_LICENSE { get; set; }
        public string PROVIDER_SSN { get; set; }
        public char REALTIMEELIG { get; set; }
        public DateTime? PROVIDER_DOB { get; set; }
        public string PROVIDER_NPI { get; set; }
        public string PROVIDER_NUMBER { get; set; }
        public string INSPAYER_ELIGIBILITY_ID { get; set; }
        public string PAYER_NAME { get; set; }
        public string PAYER_SOURCE { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string PRAC_TYPE { get; set; }
        public string Fox_Insurance_Name { get; set; }
        public long? MTBC_Patient_Insurance_Id { get; set; }
    }

    public class PatientBasicInfoModel
    {
        public long Practice_Code { get; set; }
        public string Patient_Account { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Cell_Phone { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string InsPayer_Description { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
    }

    [Table("Fox_Tbl_Patient_POS")]
    public class PatientPOSLocation : BaseModel
    {
        [Key]
        public long Patient_POS_ID { get; set; }
        public long Patient_Account { get; set; }
        public long Loc_ID { get; set; }
        public DateTime? Effective_From { get; set; }
        public DateTime? Effective_To { get; set; }
        public bool? Is_Void { get; set; }
        public bool? Is_Default { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool? Deleted { get; set; }

        [NotMapped]
        public string Patient_Account_Str
        {
            get
            {
                return Patient_Account.ToString();
            }
            set
            {
                long _lg;
                Patient_Account = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        [NotMapped]
        public FacilityLocation Patient_POS_Details { get; set; }
        [NotMapped]
        public string Effective_From_In_String { get; set; }
        [NotMapped]
        public string Effective_To_In_String { get; set; }
        [NotMapped]
        public bool IsNew { get; set; }
        [NotMapped]
        public bool IsUpdation { get; set; }
        [NotMapped]
        public bool IsAddition { get; set; }
        [NotMapped]
        public string HomePhoneFromPatienSLC { get; set; }
        [NotMapped]
        public string Patient_Home_Phone { get; set; }
    }

    [Table("Fox_Tbl_Patient_Contacts")]
    public class PatientContact : BaseModel
    {
        [Key]
        public long Contact_ID { get; set; }
        public long Patient_Account { get; set; }
        public long? Contact_Type_Id { get; set; }
        public string First_Name { get; set; }
        public string MI { get; set; }
        public string Last_Name { get; set; }
        public string Room { get; set; }
        public string Address { get; set; }
        public bool Is_Same_Patient_Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email_Address { get; set; }
        public string Fax_Number { get; set; }
        public string Home_Phone { get; set; }
        public bool? Call_Home { get; set; }
        public string Best_Time_Call_Home { get; set; }

        public int? Preferred_Contact { get; set; }
        public string Work_Phone { get; set; }
        public bool? Call_Work { get; set; }
        public string Best_Time_Call_Work { get; set; }

        public string Cell_Phone { get; set; }
        public bool? Call_Cell { get; set; }
        public string Best_Time_Call_Cell { get; set; }
        public string Preferred_Delivery_Method { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public bool? Marketing_Referral_Source { get; set; }
        public bool? Flag_Financially_Responsible_Party { get; set; }
        public bool? Flag_Preferred_Contact { get; set; }
        public bool? Flag_Emergency_Contact { get; set; }
        public bool? Flag_Power_Of_Attorney { get; set; }
        public bool? Flag_Power_Of_Attorney_Financial { get; set; }
        public bool? Flag_Power_Of_Attorney_Medical { get; set; }
        public bool? Flag_Lives_In_Household_SLC { get; set; }
        public bool? Flag_Service_Location { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_On { get; set; }
        public bool? Deleted { get; set; }
        public int? EXT_WORK_PHONE { get; set; }
        public bool STATEMENT_ADDRESS_MARKED { get; set; }
        [NotMapped]
        public string Patient_Account_Str
        {
            get
            {
                return Patient_Account.ToString();
            }
            set
            {
                long _lg;
                Patient_Account = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        //[NotMapped]
        //public string Patient_Account_Str { get { return Patient_Account.ToString(); } }
        [NotMapped]
        public string Contact_Type_Name { get; set; }
        [NotMapped]
        public string Start_Date_In_String { get; set; }
        [NotMapped]
        public string End_Date_In_String { get; set; }
        [NotMapped]
        public string Flags { get; set; }
        [NotMapped]
        public bool PopulateStatementAddress { get; set; }
        [NotMapped]
        public List<string> CitiesList { get; set; }
        //public long? FOX_TBL_COUNTRY_ID { get; set; }
        [NotMapped]
        public string CODE { get; set; }
        [NotMapped]
        public string NAME { get; set; }
        [NotMapped]
        public string DESCRIPTION { get; set; }
    }

    public class ContactTypesForDropdown
    {
        public long Contact_Type_ID { get; set; }
        public string Type_Name { get; set; }
    }
    public class BestTimeToCallForDropdown
    {
        public string RT_CODE { get; set; }
        public string DESCRIPTION { get; set; }
    }

    [Table("Fox_Tbl_Patient_Contact_Types")]
    public class ContactType
    {
        [Key]
        public long Contact_Type_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string CODE { get; set; }
        public string Type_Name { get; set; }
        public long Practice_Code { get; set; }
        public string Display_Order { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_On { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_On { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool? Deleted { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    public class SSNExist
    {
        public string SSN { get; set; }
        public string Patient_Account { get; set; }
    }

    public class PatientExist
    {
        public string Patient_Account { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string Date_of_Birth { get; set; }
    }

    [Table("Guarantors")]
    public class Subscriber
    {
        [Key]
        public long GUARANTOR_CODE { get; set; }
        public string GUARANT_FNAME { get; set; }
        public string GUARANT_LNAME { get; set; }
        public string GUARANT_MI { get; set; }
        public string GUARANT_ADDRESS { get; set; }
        public string GUARANT_CITY { get; set; }
        public string GUARANT_STATE { get; set; }
        public string GUARANT_ZIP { get; set; }
        public string GUARANT_HOME_PHONE { get; set; }
        public DateTime? GUARANT_DOB { get; set; }
        [NotMapped]
        public string GUARANT_DOB_IN_STRING { get; set; }
        public string GUARANT_GENDER { get; set; }
        public bool? Deleted { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }

        [NotMapped]
        public bool IS_NEW_SUBSCRIBER { get; set; }
        [NotMapped]
        public bool Flag_PERSONAL_INFORMATION { get; set; }
        public string Guarant_Type { get; set; }
        public long? guarant_practice_code { get; set; }
        public string GUARANT_WORK_PHONE { get; set; }
    }

    [Table("FOX_TBL_MEDICARE_LIMIT_TYPE")]
    public class MedicareLimitType
    {
        [Key]
        public int MEDICARE_LIMIT_TYPE_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public int DISPLAY_ORDER { get; set; }
    }

    [Table("FOX_TBL_MEDICARE_LIMIT")]
    public class MedicareLimit
    {
        [Key]
        public long MEDICARE_LIMIT_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? Patient_Account { get; set; }
        public long? CASE_ID { get; set; }
        public DateTime? EFFECTIVE_DATE { get; set; }

        [NotMapped]
        public string EFFECTIVE_DATE_IN_STRING { get; set; }
        public DateTime? END_DATE { get; set; }

        [NotMapped]
        public string END_DATE_IN_STRING { get; set; }
        public decimal? ABN_EST_WK_COST { get; set; }
        public string ABN_COMMENTS { get; set; }
        public string NPI { get; set; }
        public int? MEDICARE_LIMIT_TYPE_ID { get; set; }
        public string MEDICARE_LIMIT_STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

        [NotMapped]
        public string MEDICARE_LIMIT_TYPE_NAME { get; set; }

        [NotMapped]
        public int DISPLAY_ORDER { get; set; }
    }

    [Table("EMPLOYERS")]
    public class Employer
    {
        [Key]
        public long Employer_Code { get; set; }
        public string Employer_Name { get; set; }
        public string Employer_Address { get; set; }
        public string Employer_City { get; set; }
        public string Employer_State { get; set; }
        public string Employer_ZIP { get; set; }
        public string Employer_Contact_Phone { get; set; }
        public bool? Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string FAX_NUMBER { get; set; }
        public string PLAN_NAME { get; set; }
        public bool? IS_REFERRAL_REQUIRED { get; set; }
        public long? REFERRAL_PROVIDER_ID { get; set; }
        [NotMapped]
        public string REFERRAL_PROVIDER_NAME { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                return $@" {Employer_Name} {Environment.NewLine}
                           {Employer_Address}, {Employer_City}, {Employer_State}   {Environment.NewLine}
                           Phone: {Employer_Contact_Phone}  ";
            }
            //set { Name = value; }
        }

    }

    //[Table("FOX_TBL_INSURANCE_OLD")]
    //public class FoxInsurancePayors
    //{
    //    [Key]
    //    public long FOX_TBL_INSURANCE_ID { get; set; }
    //    public string INSURANCE_PAYERS_ID { get; set; }
    //    public string INSURANCE_ID { get; set; }
    //    public string INSURANCE_NAME { get; set; }
    //    public string ADDRESS { get; set; }
    //    public string STATE { get; set; }
    //    public string ZIP { get; set; }
    //    public string PHONE { get; set; }
    //    [NotMapped]
    //    public double TOTAL_RECORD_PAGES { get; set; }
    //    [NotMapped]
    //    public int TOTAL_RECORDS { get; set; }
    //}

    public class SubscriberSearchReq : BaseModel
    {
        public string SEARCHVALUE { get; set; }
    }

    public class EmployerSearchReq : BaseModel
    {
        public string SEARCHVALUE { get; set; }
        public long PracticeCode { get; set; }
    }

    public class PatientCasesForDD
    {
        public long CASE_ID { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public string CASE_STATUS_NAME { get; set; }
        public string DISCIPLINE_NAME { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CASE_SUFFIX_NAME { get; set; }
    }

    public class PR_VIEW_CASES
    {
        public long CASE_ID { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public string CASE_STATUS_NAME { get; set; }
        public DateTime? ADMISSION_DATE { get; set; }
        public string PosLocation { get; set; }
        public long? POS_ID { get; set; }
        public string TreatingProviderName { get; set; }
        public bool? IsWellness { get; set; }
        public bool? IsSkilled { get; set; }

    }

    public class PatientInsuranceEligibilityDetail : BaseModel
    {
        public string Patient_Account_Str { get; set; }
        //public bool? chk_Hospice { get; set; }
        //public bool? CHK_ABN { get; set; }
        //public bool? CHK_HOME_HEALTH_EPISODE { get; set; }
        public bool hasPrimaryIns { get; set; }
        public bool hasSecondaryIns { get; set; }
        public bool hasTertiaryIns { get; set; }
        public bool hasQuaternaryIns { get; set; }
        public bool hasInvalidPrimaryIns { get; set; }
        public bool hasInvalidSecondaryIns { get; set; }
        public bool hasPatientResponsibilityIns { get; set; }
        public string Suggested_MC_Payer { get; set; }
        public PatientInsurance InsuranceToCreateUpdate { get; set; }
        public List<PatientInsurance> Current_Patient_Insurances { get; set; }
        public List<PatientInsurance> Old_Patient_Insurances { get; set; }
        //public List<MedicareLimit> CurrentMedicareLimitList { get; set; }
        public Employer Employer_Details { get; set; }
        public List<PatientCasesForDD> PatientCasesList { get; set; }
        public bool is_Change { get; set; }
        public List<FinancialClass> FinancialClassList { get; set; }
        public List<PatientPRPeriod> PatientPRPeriodList { get; set; }
        public List<PatientPRDiscount> PatientPRDiscountList { get; set; }

        public List<PR_VIEW_CASES> PR_CASES_LIST { get; set; }

    }

    public class ExtractEligibilityDataViewModel
    {
        public List<Copay> CopayList { get; set; } = new List<Copay>();
        public string DED_POLICY_LIMIT_RESET_ON { get; set; }
        public decimal? YEARLY_DED_AMT { get; set; }
        public decimal? DED_MET
        {
            get
            {
                return YEARLY_DED_AMT - DED_REMAINING;
            }
        }
        public DateTime? DED_MET_AS_OF { get; set; } = DateTime.Now;
        public decimal? DED_REMAINING { get; set; }
        public decimal? PT_ST_TOT_AMT_USED { get; set; }
        public decimal? OT_TOT_AMT_USED { get; set; }
        public string BENEFIT_POLICY_LIMIT_RESET_ON { get; set; } = @"01/01";
        public decimal? MYB_LIMIT_DOLLARS { get; set; }
        public int? MYB_LIMIT_VISIT { get; set; }
        public decimal? MYB_LIMIT_Remaining_DOLLARS { get; set; }
        public int? MYB_LIMIT_Remaining_VISIT { get; set; }
        public decimal? MYB_USED_OUTSIDE_DOLLARS
        {
            get
            {
                return MYB_LIMIT_DOLLARS - MYB_LIMIT_Remaining_DOLLARS;
            }
        }
        public int? MYB_USED_OUTSIDE_VISIT
        {
            get
            {
                return MYB_LIMIT_VISIT - MYB_LIMIT_Remaining_VISIT;
            }
        }
        public decimal? MOP_AMT { get; set; }
        public decimal? MOP_AMT_Remaining { get; set; }
        public decimal? MOP_USED_OUTSIDE_RT
        {
            get
            {
                return MOP_AMT - MOP_AMT_Remaining;
            }
        }
        public string Network { get; set; }
        public string CoveragePlan { get; set; }
        public DateTime? Active_Coverage_From { get; set; }
        public DateTime? Active_Coverage_To { get; set; }
        public DateTime? HHE_Effective_Date { get; set; }
        public DateTime? HHE_End_Date { get; set; }
        public string HHE_NPI { get; set; }
        public DateTime? HOS_Effective_Date { get; set; }
        public DateTime? HOS_End_Date { get; set; }
        public string HOS_NPI { get; set; }
        public DateTime? Deceased_Date { get; set; }
        public string Eligibility_Status { get; set; }
        public string Eligibility_MSP_Data { get; set; }
        public bool Allow_Save { get; set; }
        public string Not_Allow_Save_Reason { get; set; }
        public bool Is_Railroad_Medicare { get; set; }
        public bool Is_Plan_Expired { get; set; }
        public bool Is_PPO { get; set; }
    }

    public class Copay
    {
        public string Type { get; set; }
        public decimal? Co_Payment { get; set; }
        public bool? IS_COPAY_PER { get; set; }
        public bool? IS_COPAY_PER_VISIT { get; set; }
    }

    //public class EligibilitySearchReq : BaseModel
    //{
    //    public string Patient_Account_Str { get; set; }
    //    public int INS_TYPE { get; set; }
    //}

    public class MedicareLimitHistory
    {
        public string Patient_Account { get; set; }
        public string Owner { get; set; }
        public string CaseNo { get; set; }
        public DateTime? Effective_From { get; set; }
        public DateTime? Effective_To { get; set; }
    }
    public class MedicareLimitHistorySearchReq : BaseModel
    {
        public string Patient_Account { get; set; }
        public int? MedicareLimitTypeId { get; set; }
    }

    public class PatientResponse : ResponseModel
    {
        public PatientPOSLocation PatientPosObj { get; set; }
    }
    public class SmartSearchInsuranceReq
    {
        public string Insurance_Name { get; set; }
        public string Patient_Account { get; set; }
        public string PatientState { get; set; } = "";
        public int? FINANCIAL_CLASS_ID { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
    }

    [Table("FOX_TBL_ELIG_HTML")]
    public class FOX_TBL_ELIG_HTML
    {
        [Key]
        public long ELIG_HTML_ID { get; set; }
        public string ELIG_HTML { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> PATIENT_ACCOUNT { get; set; }
        public Nullable<long> PATIENT_INSURANCE_ID { get; set; }
    }

    [Table("FOX_TBL_FINANCIAL_CLASS")]
    public class FinancialClass
    {
        [Key]
        public int FINANCIAL_CLASS_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool SHOW_FOR_INSURANCE { get; set; }
    }

    public class PayorDataModel
    {
        public string PayorFirstName { get; set; }
        public string PayorLastName { get; set; }

        public string PayorDOB { get; set; }

        public string PayorGender { get; set; }

        public string PayorAdress { get; set; }

        public string PayorZip { get; set; }

        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string Exeption { get; set; }
        public long Patient_Account { get; set; }

    }
    [Table("FOX_TBL_RECONCILE_DEMOGRAPHICS")]
    public class ReconcileDemographics
    {
        [Key]
        public long FOX_REC_DEM_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string ZIP { get; set; }
        public string STATE { get; set; }
        public System.DateTime? DOB { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string Error { get; set; }
    }

    public class AdvanceInsuranceSearch
    {
        public string SearchString { get; set; }
        public long? Insurance_Id { get; set; }
        public long FOX_TBL_INSURANCE_ID { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        public string Insurance_Phone_Number1 { get; set; }
        public string FAX { get; set; }
        public string InsPayer_Description { get; set; }
        public string FINANCIAL_CLASS_NAME { get; set; }
        public string Insurance_Address { get; set; }
        public string Insurance_Zip { get; set; }
        public string Insurance_City { get; set; }
        public string Insurance_State { get; set; }
        public long Practice_Code { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }
    public class AdvancePatientSearch
    {
        public string SearchString { get; set; }
        public long Patient_Account { get; set; }
        public string Patient_Account_Str
        {
            get
            {
                return Patient_Account.ToString();
            }
        }
        public string PATIENT_NAME
        {
            get
            {
                return string.IsNullOrWhiteSpace(First_Name) ? "" : $@"{Last_Name}, {First_Name}";
            }
        }
        public string First_Name { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string Last_Name { get; set; }
        public string Chart_Id { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public string Date_Of_Birth_In_String { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Created_Date_Str { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }
    public class SmartPatientRes
    {
        public long PATIENT_ACCOUNT { get; set; }
        public string Patient_Account_Str
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
        }
        public string PATIENT_NAME
        {
            get
            {
                return $@"{LAST_NAME}, {FIRST_NAME}";
            }
        }
        public string NAME
        {
            get
            {
                return string.IsNullOrWhiteSpace(FIRST_NAME) ? "" : $@"{LAST_NAME}, {FIRST_NAME}";
            }
        }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string Chart_Id { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Template
        {
            get
            {
                return string.IsNullOrWhiteSpace(FIRST_NAME) ? "" : $@"{LAST_NAME}, {FIRST_NAME}";

            }
        }
    }
    public class SmartPatientResForTask
    {
        public long PATIENT_ACCOUNT { get; set; }
        public string Patient_Account_Str
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
        }
        //public string PATIENT_NAME
        //{
        //    get
        //    {
        //        return $@"{FIRST_NAME} {LAST_NAME}";
        //    }
        //}
        public string NAME
        {
            get
            {
                return string.IsNullOrWhiteSpace(FIRST_NAME) ? "" : $@"{LAST_NAME}, {FIRST_NAME}";
            }
        }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string Chart_Id { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Template
        {
            get
            {
                DateTime Temp_Date_Of_Birth = Convert.ToDateTime(Date_Of_Birth);
                int Age = new DateTime(DateTime.Now.Subtract(Temp_Date_Of_Birth).Ticks).Year - 1;

                if (!string.IsNullOrWhiteSpace(LAST_NAME))
                {
                    return $@"MRN: {Chart_Id} <br> Patient Account No: {PATIENT_ACCOUNT} <br> Patient Name: {NAME} <br> Gender: {Gender} <br> Age: {Age} Years ";
                    
                }
                else
                {
                    return $@"MRN: {Chart_Id} <br> Patient Account No: {PATIENT_ACCOUNT} <br> Patient Name: {FIRST_NAME} <br> Gender: {Gender} <br> Age: {Age} Years ";
                }
            }
            set
            {

            }
        }
        public int ROW { get; set; }
    }
    
    public class DefaultInsuranceParameters
    {
        public long? patientAccount { get; set; }
        public string ZIP { get; set; }
        public string State { get; set; }
    }
    public class SubscriberInfoRequest
    {
        public long? patientAccount { get; set; }
        public long Practice_Code { get; set; }
    }
    public class SubscriberInformation
    {
        public Patient patientinfo { get; set; }
        public List<PatientContact> PatientContactsList { get; set; }
        public List<PatientAddress> PatientAddress { get; set; }
        public List<ContactType> ContactTypeList { get; set; }
    }
    [Table("FOX_TBL_DISCOUNT")]
    public class PatientPRDiscount
    {
        [Key]
        public int PR_DISCOUNT_ID { get; set; }
        public string TYPE { get; set; }
        public string CODE { get; set; }
        public decimal PERCENTAGE { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_PR_PERIOD")]
    public class PatientPRPeriod
    {
        [Key]
        public int PR_PERIOD_ID { get; set; }
        public string PERIOD_DESCRIPTION { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_INSURANCE")]
    public class FoxInsurancePayers
    {
        [Key]
        public long FOX_TBL_INSURANCE_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string INSURANCE_PAYERS_ID { get; set; }
        public string INSURANCE_NAME { get; set; }
        public string PAYING_AGENCY { get; set; }
        public string PAYER_TYPE { get; set; }
        public string PLAN_CODE { get; set; }
        public string ADDRESS { get; set; }
        public long? INSURANCE_ID { get; set; }
        public string ADDRESS_1 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string COUNTY { get; set; }
        public string PHONE { get; set; }
        [NotMapped]
        public string FC_NAME { get; set; }
        public string FAX { get; set; }
        public string FIELD_SET { get; set; }
        public string ALLOW_CONCURRENT_THERAPY { get; set; }
        public string ALLOW_CO_TREATMENT { get; set; }
        public string CCI_VALIDATION { get; set; }
        public string MODIFIER_KX { get; set; }
        public string MODIFIER_52_APPLIES { get; set; }
        public string CAP_TRACKING { get; set; }
        public string MPPR_ADJUSTED { get; set; }
        public string PQRS { get; set; }
        public string FUNCTIONAL_LIMITATIONS_REPORTING { get; set; }
        public string EIGHT_MIN_RULE_CALCULATION_METHOD { get; set; }
        public string BILL_FORM { get; set; }
        public string ELECTRONIC_SUBMISSION { get; set; }
        public string CLAIM_FILING_INDICATOR_CODE { get; set; }
        public string SUBSEQUENT_ELECTRONIC_CLAIM { get; set; }
        public bool? SEPARATE_CLAIM_FOR_EACH_DISCIPLINE { get; set; }
        public bool? REBILL_REQUIRES_DCN { get; set; }
        public bool? INCLUDE_ZEROPOINT_ONE_CHARGES_ON_PROFESSIONAL_CLAIMS_SUBMITTED_AFTER_PRIMARY_INSTITUTIONAL_CLAIMS { get; set; }
        public bool? APPLY_SEQUESTRATION_ADJUSTMENT { get; set; }
        public bool? SEPARATE_PROFESSIONAL_CLAIM_BY_RENDERING_THERAPIST_NPI { get; set; }
        public bool? RIM_HOLDS_WILL_PREVENT_CLAIMS_FROM_RELEASING { get; set; }
        public bool? PHYSICIAN_REQUIREMENT_NPI { get; set; }
        public bool? RENDERING_PROVIDER_REQUIRES_NPI { get; set; }
        public bool? RENDERING_PROVIDER_REQUIRES_STATE_LICENSE { get; set; }
        public bool? RENDERING_PROVIDER_REQUIRES_TAXONOMY { get; set; }
        public bool? REQUIRE_ADMITTING_DIAGNOSIS { get; set; }
        public string PHONE_NUMBER_FOR_AUTHORIZATIONS { get; set; }
        public int? TIMELY_FILING_LIMIT { get; set; }
        public bool? OUTPATIENT_PRIOR_AUTHORIZATION_REQUIRED { get; set; }
        public bool? OUTPATIENT_INCLUDE_KX_MODIFIER_ON_CLAIMS { get; set; }
        public bool? OUTPATIENT_APPLY_KX_TO_ENTIRE_CLAIM_PERIOD { get; set; }
        public bool? OUTPATIENT_INCLUDE_59_MODIFIER_ON_CLAIMS { get; set; }
        public string RENDERING_PROVIDER_CREDENTIALING_REQUIREMENTS { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        public string CARRIER { get; set; }
        public string CARRIER_STATE { get; set; }
        public string CARRIER_LOCALITY { get; set; }
        [NotMapped]
        public string FEE_PLAN_REDIRECT { get; set; }
        public string FEE_REDIRECT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool? DELETED { get; set; }
        public bool IS_CCI_VALIDATION { get; set; }
        public bool IS_MODIFIER_KX { get; set; }
        public bool IS_CAP_TRACKING { get; set; }
        public bool IS_PQRS { get; set; }
        public bool IS_FUNCTIONAL_LIMITATIONS_REPORTING { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }

        public bool Is_Authorization_Required { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    [Table("WS_FOX_TBL_PHR_USERS")]
    public class PHR
    {
        [Key]
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string USER_PHONE { get; set; }
        public string PASSWORD { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        public long PRACTICE_CODE { get; set; }
        public Nullable<bool> IS_ACTIVATED { get; set; }
        public Nullable<bool> IS_BLOCK { get; set; }
        public Nullable<bool> DELETED { get; set; }
        public string INVITE_STATUS { get; set; }
        public Nullable<DateTime> ACCEPT_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<DateTime> MODIFIED_DATE { get; set; }
        public Nullable<bool> FORGOT_PASSWORD { get; set; }
        public Nullable<bool> AGREEMENT { get; set; }
        public Nullable<DateTime> AGREEMENT_DATE { get; set; }
        public string AGREEMENT_IP { get; set; }
        public string AGREEMENT_VERSION { get; set; }
        public string TEMP_PASSWORD { get; set; }
        public Nullable<DateTime> TEMP_DATE { get; set; }
        public Nullable<DateTime> PASSWORD_CHANGED_DATE { get; set; }
        public Nullable<DateTime> LAST_LOGIN_DATE { get; set; }
        [NotMapped]
        public string PatientAccountInString
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
        [NotMapped]
        public bool ShouldUpdatePatient { get; set; }
    }



    public class WorkOrderDocs
    {
        public long PATIENT_ACCOUNT { get; set; }
        [NotMapped]
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
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? FILE_ID { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string WORK_STATUS { get; set; }
        public string SENDER_NAME { get; set; }

        public string DEPARTMENT_ID { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string href { get; set; }
        public string src { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string Template { get; set; }


    }

    public class AutoPopulateModel
    {
        public long CASE_ID { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
        public string patientAccount { get; set; }
        public bool IS_WELLNESS { get; set; }
    }


    public class PatientEligibilitySearchModel
    {
        public string Patient_Account_Str { get; set; }
        public long? Patient_Insurance_id { get; set; }
        public bool IS_MVP_VIEW { get; set; }
        public long? WORK_ID { get; set; }
    }

    public class POSCoordinates
    {
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    [Table("AF_TBL_PATIENT_ADDITIONAL_INFO")]
    public class Patient_Additional_Info_TalkEHR
    {
        [Key]
        public long PATIENT_ADDITIONAL_INFO_ID { get; set; }
        public long? Patient_Account { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
        public string Title { get; set; }
        public string Employment_Status { get; set; }
        public string Patient_Status { get; set; }
        public string Student_Status { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
        public string Best_Time_of_Call_Home { get; set; }
        public string Best_Time_of_Call_Work { get; set; }
        public string Best_Time_of_Call_Cell { get; set; }
        public string Fax_Number { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public bool? Expired { get; set; }
    }

    [Table("WEBEHR_TBL_PATIENT_ADDRESS_HISTORY")]
    public class Patient_Address_History_WebEHR
    {
        [Key]
        public long? PATIENT_ADDRESS_HISTORY_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool? DELETED { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string Home_Phone { get; set; }
        public string COUNTRY { get; set; }
        public string ADDRESS_TYPE { get; set; }
        public bool? Same_As_POS { get; set; }
        public long? PATIENT_POS_ID { get; set; }
        public string Phone { get; set; }
        public string Work_Phone { get; set; }
        public string Cell_Phone { get; set; }
        public string Fax { get; set; }
        public string Email_Address { get; set; }
        public string REGION { get; set; }
        public string County { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }

    public class PatientDeceasedInfo
    {
        public long Patient_Account { get; set; }
        [NotMapped]
        public string Patient_Account_Str
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
        public DateTime? Deceased_Date { get; set; }
    }

    [Table("CLAIMS_REF_EXTFIELDS")]
    public class CLAIMS_REF_EXTFIELDS
    {
        public long Claim_No { get; set; }
        public Nullable<long> Fox_Case_ID { get; set; }
        public Nullable<long> Fox_Claim_Type_Id { get; set; }
        public Nullable<long> Fox_Special_Account_Services_Id { get; set; }
        public Nullable<long> Fox_Driving_Program_Services_Id { get; set; }
        public Nullable<long> Fox_Better_Living_Id { get; set; }
        public Nullable<long> Fox_Private_Pay_Services_Id { get; set; }
        public Nullable<bool> Fox_Long_Term_Care { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<decimal> Expected_Payments { get; set; }
        public Nullable<bool> Patient_Expired { get; set; }
    }

    public class SuggestedMCPayer
    {
        public string Zip { get; set; }
        public string State { get; set; }
    }

    [Table("FOX_TBL_PATIENT_ADDRESS_ADDITIONAL_INFO")]
    public class PatientAddressAdditionalInfo
    {
        [Key]
        public long PATIENT_ADDRESS_ADDITIONAL_INFO { get; set; }
        public long PATIENT_ADDRESS_HISTORY_ID { get; set; }
        public long? WORK_ID { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public bool? DELETED { get; set; }
    }


    [Table("Fox_Tbl_Patient_Alias")]
    public partial class PatientAlias
    {
        [Key]
        public long PATIENT_ALIAS_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                PATIENT_ACCOUNT = Convert.ToInt64(value);
            }
        }
        public string ALIAS_TRACKING_NUMBER { get; set; }
        public string RT_ALIAS_TRACKING { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MIDDLE_INITIALS { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public bool Is_Update { get; set; }
    }

    public class SmartSearchCountriesReq
    {
        public string SEARCHVALUE { get; set; }
        //public long Practice_Code { get; set; }
    }

    [Table("FOX_TBL_COUNTRY")]
    public class CountryResponse
    {
        [Key]
        public long FOX_TBL_COUNTRY_ID { get; set; }
        public string CODE { get; set; }
        //public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        //public long PRACTICE_CODE { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string NAME
        {
            get
            {
                return $"[{CODE}] - {DESCRIPTION}";
            }
        }
    }

    public class DocumentSaveEligibility
    {
        public string document_html { get; set; }
        public string Patient_Account_Str { get; set; }
        public long Patient_Insurance_Id { get; set; }
        public bool IS_MVP_VIEW { get; set; }
        public long WORK_ID { get; set; }
    }

    public class CheckDuplicatePatientsReq
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string Date_Of_Birth_In_String { get; set; }
        public string Gender { get; set; }
        public string PATIENT_ACCOUNT { get; set; }
    }

    public class CheckDuplicatePatientsRes
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public long Patient_Account { get; set; }
        public string Chart_ID { get; set; }
        public string gender { get; set; }
        public string MI { get; set; }
        public string SSN { get; set; }
        public DateTime DOB { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? referral_Completed_On { get; set; }
        public string referral_Completed_By { get; set; }
        public long? Work_ID { get; set; }
        public string Document_Type { get; set; }
        public string Sender_Source { get; set; }
        public string Discipline { get; set; }

        public bool IS_PATIENT_INTERFACE_SYNCED { get; set; }
        public string Primary_INS { get; set; }
        public string POS { get; set; }
        public string FINANCIAL_CLASS { get; set; }
        public int? FINANCIAL_CLASS_ID { get; set; }
    }

    public class SmartModifiedBy
    {
        
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string NAME
        {
            get
            {
                return string.IsNullOrWhiteSpace(FIRST_NAME) ? "" : $@"{LAST_NAME}, {FIRST_NAME}";
            }
        }
        
        public string USER_NAME { get; set; }
        public string ROLE_NAME { get; set; }
        public string Template
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ROLE_NAME))
                {
                    return $@"{ NAME } - { ROLE_NAME }";
                }
                else
                {
                    return $@"{ NAME }";
                }
                
            }
            set
            {

            }
        }
        public int ROW { get; set; }
    }

}
