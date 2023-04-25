using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.CasesModel
{
    [Table("FOX_TBL_CASE")]
    public class FOX_TBL_CASE : BaseModel
    {
        [Key]
        public long CASE_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR { get; set; }
        public Nullable<int> CASE_TYPE_ID { get; set; }
        [NotMapped]
        public string CASE_TYPE_NAME { get; set; }
        public Nullable<int> DISCIPLINE_ID { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public Nullable<int> CASE_STATUS_ID { get; set; }
        public Nullable<int> CASE_SUFFIX_ID { get; set; }
        public Nullable<int> GROUP_IDENTIFIER_ID { get; set; }
        public Nullable<long> TREATING_PROVIDER_ID { get; set; }
        public Nullable<long> POS_ID { get; set; }
        public Nullable<long> TREATING_REGION_ID { get; set; }
        public Nullable<bool> IS_MANUAL_CHANGE_REGION { get; set; }
        public Nullable<System.DateTime> ADMISSION_DATE { get; set; }
        public Nullable<System.DateTime> START_CARE_DATE { get; set; }
        public Nullable<System.DateTime> END_CARE_DATE { get; set; }
        [NotMapped]
        public string ADMISSION_DATE_String { get; set; }
        [NotMapped]
        public string START_CARE_DATE_String { get; set; }
        [NotMapped]
        public string END_CARE_DATE_String { get; set; }
        public Nullable<int> VISIT_PER_WEEK { get; set; }
        public Nullable<int> NO_OF_WEEK { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<int> TOTAL_VISITS { get; set; }
        public Nullable<long> WORK_ID { get; set; }
        public Nullable<long> ORDERING_REF_SOURCE_ID { get; set; }
        public Nullable<long> ACU_IDENTIFIER_ID { get; set; }
        public Nullable<long> HOSPITAL_IDENTIFIER_ID { get; set; }
        public Nullable<long> SNF_IDENTIFIER_ID { get; set; }
        public Nullable<long> HHH_IDENTIFIER_ID { get; set; }
        public Nullable<long> REF_REGION_ID { get; set; }
        public Nullable<int> SOURCE_OF_REFERRAL_ID { get; set; }
        public Nullable<long> PRIMARY_PHY_ID { get; set; }
        public Nullable<long> CERTIFYING_REF_SOURCE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public Nullable<long> HEAR_ABOUT_US_ID { get; set; }

        [NotMapped]
        public string Comments { get; set; }

        [NotMapped]
        public string ImportantNotes { get; set; }

        [NotMapped]
        public string VoidReason { get; set; }

        [NotMapped]
        public List<FOX_TBL_ORDER_INFORMATION> OrderInformationList { get; set; }
        [NotMapped]
        public List<FOX_TBL_ORDER_INFORMATION> OrderInformationList_deleted { get; set; }

        [NotMapped]
        public List<OpenIssueList> openIssueList { get; set; }
        [NotMapped]
        public List<FOX_TBL_CALLS_LOG> CallsLogList { get; set; }
        [NotMapped]
        public List<FOX_TBL_CALLS_LOG> CallsLogList_deleted { get; set; }
        [NotMapped]
        public Nullable<long> POSRegionID { get; set; }
        [NotMapped]
        public string POSRegionCode { get; set; }
        [NotMapped]
        public string POSRegionName { get; set; }
        [NotMapped]
        public string HEAR_ABOUT_US_DISPLAY_TEXT { get; set; }
        [NotMapped]
        public bool Is_Chnage { get; set; }

        public long? PATIENT_RESP_INS_ID { get; set; }

        public bool? IsWellness { get; set; }
        public bool? IsSkilled { get; set; }

        public System.DateTime? DISCHARGE_DATE { get; set; }
        public DateTime? HOLD_DATE { get; set; }
        public DateTime? HOLD_TILL_DATE { get; set; }
        public string HOLD_DURATION { get; set; }
        public DateTime? HOLD_FOLLOW_UP_DATE { get; set; }
        [NotMapped]
        public string HOLD_FOLLOW_UP_DATE_String { get; set; }
        public string NON_ADMIT_REASON { get; set; }


    }

    [Table("FOX_VW_CASE")]
    public class FOX_VW_CASE
    {
        [Key]
        public long CASE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR { get; set; }
        public Nullable<int> CASE_TYPE_ID { get; set; }
        public string CASE_TYPE_NAME { get; set; }
        public Nullable<int> DISCIPLINE_ID { get; set; }
        public string DISCIPLINE_NAME { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public Nullable<int> CASE_STATUS_ID { get; set; }
        public string CASE_STATUS_NAME { get; set; }
        public string CASE_STATUS_COLOR { get; set; }
        public Nullable<int> CASE_SUFFIX_ID { get; set; }
        public int? GROUP_IDENTIFIER_ID { get; set; }
        public Nullable<long> TREATING_PROVIDER_ID { get; set; }
        public Nullable<long> POS_ID { get; set; }
        public Nullable<long> TREATING_REGION_ID { get; set; }
        public Nullable<bool> IS_MANUAL_CHANGE_REGION { get; set; }
        public Nullable<System.DateTime> ADMISSION_DATE { get; set; }
        public Nullable<System.DateTime> START_CARE_DATE { get; set; }
        public Nullable<System.DateTime> END_CARE_DATE { get; set; }
        public Nullable<int> VISIT_PER_WEEK { get; set; }
        public Nullable<int> NO_OF_WEEK { get; set; }
        public Nullable<int> TOTAL_VISITS { get; set; }
        public Nullable<long> WORK_ID { get; set; }
        public Nullable<long> ORDERING_REF_SOURCE_ID { get; set; }
        public Nullable<long> ACU_IDENTIFIER_ID { get; set; }
        public Nullable<long> HOSPITAL_IDENTIFIER_ID { get; set; }
        public Nullable<long> SNF_IDENTIFIER_ID { get; set; }
        public Nullable<long> HHH_IDENTIFIER_ID { get; set; }
        public Nullable<long> REF_REGION_ID { get; set; }
        public Nullable<int> SOURCE_OF_REFERRAL_ID { get; set; }
        public Nullable<long> PRIMARY_PHY_ID { get; set; }
        public Nullable<long> CERTIFYING_REF_SOURCE_ID { get; set; }
        public string TreatingProviderName { get; set; }
        //public string TreatingProviderNotes { get; set; }
        public string PosLocation { get; set; }
        public string TREATING_REFERRAL_REGION { get; set; }
        public string OrderingRefName { get; set; }
        public string OrderingRefNotes { get; set; }
        public string ACOIdentifier { get; set; }
        public string ACOCode { get; set; }
        public string ACODescription { get; set; }
        public string HospitalIdentifier { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalDescription { get; set; }
        public string SNFIdentifier { get; set; }
        public string SNFCode { get; set; }
        public string SNFDescription { get; set; }
        public string HHHIdentifier { get; set; }
        public string HHHCode { get; set; }
        public string HHHDescription { get; set; }
        public string REFERRAL_REGION { get; set; }
        public string PriamryPhy { get; set; }
        public string CertifyRefSource { get; set; }
        public string CertifyFullName { get; set; }
        public string CertifySourceNotes { get; set; }
        public string CertifyPhone { get; set; }
        public string CaertifyFax { get; set; }
        public string CertifyAddress { get; set; }

        public string CertifyZip { get; set; }
        public string CertifyCity { get; set; }
        public string CertifyState { get; set; }

        public string UNIQUE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long? HEAR_ABOUT_US_ID { get; set; }
        [NotMapped]
        public string HEAR_ABOUT_US_DISPLAY_TEXT { get; set; }
        [NotMapped]
        public string Template
        {
            get
            {
                return $"<p>[{CASE_NO.ToTitleCase()}] {CASE_STATUS_NAME.ToTitleCase()}, {string.Format("{0:MM/dd/yyyy}", CREATED_DATE)} </p>";
            }
        }

        public long? POSRegionID { get; set; }
        public string POSRegionCode { get; set; }
        public string POSRegionName { get; set; }
        [NotMapped]
        public ReferralSource ObjReferralSource { get; set; }
        public string CASE_SUFFIX_NAME { get; set; }

        public long? PATIENT_RESP_INS_ID { get; set; }

        public bool? IsWellness { get; set; }
        public bool? IsSkilled { get; set; }
        public System.DateTime? DISCHARGE_DATE { get; set; }

        public DateTime? HOLD_DATE { get; set; }
        public DateTime? HOLD_TILL_DATE { get; set; }

        public string HOLD_DURATION { get; set; }
        public DateTime? HOLD_FOLLOW_UP_DATE { get; set; }
        public string HOLD_RT_CODE { get; set; }
        public string NON_ADMIT_REASON { get; set; }

    }

    [Table("FOX_TBL_CASE_SUFFIX")]
    public class FOX_TBL_CASE_SUFFIX
    {
        [Key]
        public int CASE_SUFFIX_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_CASE_STATUS")]
    public class FOX_TBL_CASE_STATUS
    {
        [Key]
        public int CASE_STATUS_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string COLLAPSE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_CASE_TYPE")]
    public class FOX_TBL_CASE_TYPE
    {
        [Key]
        public int CASE_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_CATEGORY")]
    public class FOX_TBL_CATEGORY
    {
        [Key]
        public int CATEGORY_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_DELIVERY_METHOD")]
    public class FOX_TBL_DELIVERY_METHOD
    {
        [Key]
        public int DELIVERY_METHOD_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_DISCIPLINE")]
    public class FOX_TBL_DISCIPLINE
    {
        [Key]
        public int DISCIPLINE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_GROUP_IDENTIFIER")]
    public class FOX_TBL_GROUP_IDENTIFIER
    {
        [Key]
        public int GROUP_IDENTIFIER_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }

    }
    public class InactiveListOfGroupIDNAndSourceOfReferral
    {
        public List<FOX_TBL_GROUP_IDENTIFIER> Group_Identifier { get; set; }
        public List<FOX_TBL_SOURCE_OF_REFERRAL> Spourc_Of_Refferal { get; set; }
    }
    [Table("FOX_TBL_IDENTIFIER")]
    public class FOX_TBL_IDENTIFIER
    {
        [Key]
        public long IDENTIFIER_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<int> IDENTIFIER_TYPE_ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string CODE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool? IS_ACTIVE { get; set; }
    }

    [Table("FOX_TBL_IDENTIFIER_TYPE")]
    public class FOX_TBL_IDENTIFIER_TYPE
    {
        [Key]
        public int IDENTIFIER_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_NOTES")]
    public class FOX_TBL_NOTES
    {
        [Key]
        public long NOTES_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> CASE_ID { get; set; }
        public Nullable<long> TASK_ID { get; set; }
        public Nullable<long> AUTH_ID { get; set; }
        public Nullable<long> NOTES_TYPE_ID { get; set; }
        public string NOTES { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_NOTES_TYPE")]
    public class FOX_TBL_NOTES_TYPE
    {
        [Key]
        public long NOTES_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_ORDER_INFORMATION")]
    public class FOX_TBL_ORDER_INFORMATION
    {
        [Key]
        public long ORDER_INFO_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> CASE_ID { get; set; }
        public Nullable<int> ORDER_STATUS_ID { get; set; }
        public Nullable<int> CASE_TYPE_ID { get; set; }
        public Nullable<long> PRESC_PROVIDER_ID { get; set; }
        public Nullable<System.DateTime> EFFECTIVE_DATE { get; set; }
        public Nullable<System.DateTime> END_DATE { get; set; }
        [NotMapped]
        public string EFFECTIVE_DATE_Stirng { get; set; }
        [NotMapped]
        public string END_DATE_String { get; set; }

        public Nullable<int> VISIT_PER_WEEK { get; set; }
        public Nullable<int> NO_OF_WEEK { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<int> TOTAL_VISITS { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string CODE { get; set; }
        [NotMapped]
        public string DESCRIPTION { get; set; }
        [NotMapped]
        public string SOURCE_NAME { get; set; }
        [NotMapped]
        public string PRESC_PROVIDER_NAME { get; set; }
    }

    [Table("FOX_TBL_ORDER_STATUS")]
    public class FOX_TBL_ORDER_STATUS
    {
        [Key]
        public int ORDER_STATUS_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<int> DAYS { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool DELETED { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    [Table("FOX_TBL_SEND_CONTEXT")]
    public class FOX_TBL_SEND_CONTEXT
    {
        [Key]
        public int SEND_CONTEXT_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    //[Table("FOX_TBL_SENDER_NAME")]
    //public class FOX_TBL_SENDER_NAME
    //{
    //    [Key]
    //    public long FOX_TBL_SENDER_NAME_ID { get; set; }
    //    public Nullable<long> FOX_TBL_SENDER_TYPE_ID { get; set; }
    //    public string SENDER_NAME_CODE { get; set; }
    //    public string SENDER_NAME_DESCRIPTION { get; set; }
    //    public Nullable<long> PRACTICE_CODE { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public System.DateTime CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public System.DateTime MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }
    //}

    //[Table("FOX_TBL_SENDER_TYPE")]
    //public class FOX_TBL_SENDER_TYPE
    //{
    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    //    public FOX_TBL_SENDER_TYPE()
    //    {
    //        this.FOX_TBL_SENDER_NAME = new HashSet<FOX_TBL_SENDER_NAME>();
    //    }

    //    public long FOX_TBL_SENDER_TYPE_ID { get; set; }
    //    public string SENDER_TYPE_NAME { get; set; }
    //    public string SENDER_TYPE_DESCRIPTION { get; set; }
    //    public Nullable<long> PRACTICE_CODE { get; set; }
    //    public Nullable<int> DISPLAY_ORDER { get; set; }
    //    public Nullable<bool> IsInternal { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public System.DateTime CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public System.DateTime MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    //    public virtual ICollection<FOX_TBL_SENDER_NAME> FOX_TBL_SENDER_NAME { get; set; }
    //}

    [Table("FOX_TBL_SOURCE_OF_REFERRAL")]
    public class FOX_TBL_SOURCE_OF_REFERRAL
    {
        [Key]
        public int SOURCE_OF_REFERRAL_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool DELETED { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    public class ResponseGetCasesDDL : ResponseModel
    {
        public List<FOX_TBL_CASE_TYPE> FOX_TBL_CASE_TYPEList { get; set; }
        public List<FOX_TBL_DISCIPLINE> FOX_TBL_DISCIPLINEList { get; set; }
        public List<FOX_TBL_CASE_STATUS> FOX_TBL_CASE_STATUSList { get; set; }
        public List<FOX_TBL_CASE_SUFFIX> FOX_TBL_CASE_SUFFIXList { get; set; }
        public List<FOX_TBL_GROUP_IDENTIFIER> FOX_TBL_GROUP_IDENTIFIERList { get; set; }
        public List<FOX_TBL_SOURCE_OF_REFERRAL> FOX_TBL_SOURCE_OF_REFList { get; set; }
        public List<FOX_VW_CASE> FOX_VW_CASEList { get; set; }
        public List<FOX_TBL_ORDER_STATUS> FOX_TBL_ORDER_STATUSList { get; set; }
        public List<GetTotalDisciplineRes> GET_TOTAL_DISCIPLINEList { get; set; }
        public List<PatientInsurance> InsuranceEligibilityHistoryList { get; set; }
        public List<OriginalQueue> WorkOrderQueueList { get; set; }
        public List<COMMUNICATION_CALL_STATUS> CallStatusList { get; set; }
        public List<COMMUNICATION_STATUS_OF_CARE> StatusofCareList { get; set; }
        public Patient.Patient PatientObj { get; set; }
        public List<Patient.Patient> PatientTalkrehab { get; set; }
        public List<FOX_TBL_COMMUNICATION_CALL_RESULT> ResultCallList { get; set; }
        public List<FOX_TBL_COMMUNICATION_CALL_TYPE> CallTypeList { get; set; }
        public List<PatientInsurance> InsuranceList { get; set; }
        public List<FOX_TBL_CASE_TREATMENT_TEAM> CaseTreatmentTeamList { get; set; }

    }

    [Table("FOX_TBL_COMMUNICATION_CALL_STATUS")]
    public class COMMUNICATION_CALL_STATUS
    {
        [Key]
        public long FOX_CALL_STATUS_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CALL_STATUS_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_COMMUNICATION_STATUS_OF_CARE")]
    public class COMMUNICATION_STATUS_OF_CARE
    {
        [Key]
        public long FOX_CARE_STATUS_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CARE_STATUS_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_COMMUNICATION_CALL_RESULT")]
    public class FOX_TBL_COMMUNICATION_CALL_RESULT
    {
        [Key]
        public long FOX_CALL_RESULT_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CALL_RESULT_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class ResponseAddEditCase : ResponseModel
    {
        public long CASE_ID { get; set; }
    }

    public class SmartIdentifierReq : BaseModel
    {
        public long PRACTICE_CODE { get; set; }
        public int TYPE_ID { get; set; }
        public string SEARCHVALUE { get; set; }
        public string TYPE { get; set; }
        public long PracticeCode { get; set; }
    }
    public class SmartIdentifierRes

    {
        public long IDENTIFIER_ID { get; set; }
        public Nullable<int> IDENTIFIER_TYPE_ID { get; set; }
        public string CODE { get; set; }
        public string Name { get; set; }
        public string CODE_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string NAME
        {
            get
            {
                return $"[{CODE}] {DESCRIPTION}";
            }
        }
    }

    public class OpenIssueViewModel
    {
        public Nullable<long> TASK_ID { get; set; }
        public int TASK_TYPE_ID { get; set; }
        public string TASK_TYPE { get; set; }
        public int TASK_SUB_TYPE_ID { get; set; }
        public string TASK_SUB_TYPE { get; set; }
        public bool IS_CHECKED { get; set; }
        public string RT_CODE { get; set; }
    }

    public class OpenIssueList
    {
        public int TASK_TYPE_ID { get; set; }
        public string TaskTypeName { get; set; }
        public List<OpenIssueViewModel> TaskSubTypeList = new List<OpenIssueViewModel>();
        public bool IsRadioBtn { get; set; }
        public bool Is_Green { get; set; }
        public bool Is_Yellow { get; set; }
        public List<FOX_TBL_TASK> Tasks { get; set; }
        public string Notes { get; set; }
    }

    public class NONandHOLDIssueList
    {
        public int HOLD_NON_REASONS_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string TYPE { get; set; }
        public bool IS_CHECKED { get; set; }
        public string RT_CODE { get; set; }

    }

    public class OpenIssueNotes
    {
        public int TASK_TYPE_ID { get; set; }
        public string TaskTypeName { get; set; }
        public string NOTES { get; set; }
    }

    public class OpenIssueListToDelete
    {
        public long CASE_ID { get; set; }
        public List<OpenIssueViewModel> TaskSubTypeList = new List<OpenIssueViewModel>();
    }

    public class GetOpenIssueAllListRes : ResponseModel
    {
        public long CASE_ID { get; set; }
        public List<OpenIssueList> openIssueList = new List<OpenIssueList>();
    }

    public class GetNONandHOLDAllListRes : ResponseModel
    {
        public long CASE_ID { get; set; }
        public List<NONandHOLDIssueList> NONandHOLDIssueList = new List<NONandHOLDIssueList>();
        public string Notes { get; set; }

    }

    public class GetSmartPoslocRes
    {
        public long LOC_ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        //public bool IS_DEFAULT { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string Template { get { return $"[{CODE.ToTitleCase()}] {NAME.ToTitleCase()}"; } }
    }
    public class GetSmartPoslocReq
    {
        public string SEARCHVALUE { get; set; }
        public long PracticeCode { get; set; }
    }
    public class getOrderInfoReq : BaseModel
    {
        public long CASE_ID { get; set; }
    }

    public class GetTreatingProviderReq : BaseModel
    {
        public long POS_ID { get; set; }
        public string CASE_DISCIPLINE_NAME { get; set; }
    }

    public class GetTreatingProviderRes : BaseModel
    {
        public long Treating_Provider_Id { get; set; }
        public string TreatingProviderName { get; set; }
    }

    public class GetTotalDisciplineRes
    {
        public int DISCIPLINE_ID { get; set; }
        public string NAME { get; set; }
        public int TOTAL { get; set; }
    }

    public class NotesViewModel
    {
        public string NotesType { get; set; }
        public string Notes { get; set; }
        public long CASE_ID { get; set; }
        public long NOTES_ID { get; set; }
        public long NOTES_TYPE_ID { get; set; }
    }
    public class OrderInformationAndNotes : ResponseModel
    {
        public List<FOX_TBL_ORDER_INFORMATION> OrderInformationList { get; set; }
        public List<NotesViewModel> CasesNotesList { get; set; }

    }
    public class GetOpenIssueListReq : BaseModel
    {
        public long CASE_ID { get; set; }
        public int CASE_STATUS_ID { get; set; }
        public string PATIENT_ACCOUNT_STR { get; set; }
        public string TYPE { get; set; }
        public long PATIENT_ACCOUNT
        {
            get
            {
                long _dec;
                return long.TryParse(PATIENT_ACCOUNT_STR, out _dec) ? _dec : 0;
            }
        }
    }

    public class CallReq : BaseModel
    {
        public long CASE_ID { get; set; }

    }

    [Table("FOX_TBL_CALLS_LOG")]
    public class FOX_TBL_CALLS_LOG
    {
        [Key]
        public long FOX_CALLS_LOG_ID { get; set; }
        public long CASE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public long FOX_CALL_TYPE_ID { get; set; }
        public System.DateTime? DISCHARGE_DATE { get; set; }
        public string PATIENT_STATUS { get; set; }
        public Nullable<int> GROUP_IDENTIFIER_ID { get; set; }
        public string RESULT_OF_CALL { get; set; }
        public System.DateTime? CALL_DATE { get; set; }
        [NotMapped]
        public String CALL_DATE_String { get; set; }
        public string CASE_NO { get; set; }
        public System.DateTime? ADMISSION_DATE { get; set; }
        [NotMapped]
        public string ADMISSION_DATE_String { get; set; }
        public long? CASE_STATUS { get; set; }
        public long? PROVIDER_ID { get; set; }
        public long? REGION_ID { get; set; }
        public long? LOCATION_ID { get; set; }
        public long? FOX_CALL_STATUS_ID { get; set; }
        public long? FOX_CARE_STATUS_ID { get; set; }
        public long? FOX_CALL_RESULT_ID { get; set; }
        public bool? IS_WORK_CALL { get; set; }
        public bool? IS_CELL_CALL { get; set; }
        public bool? IS_HOME_CALL { get; set; }
        public string COMMENTS { get; set; }
        public string REMARKABLE_REPORT_COMMENTS { get; set; }
        public System.DateTime? COMPLETED_DATE { get; set; }
        [NotMapped]
        public String COMPLETED_DATE_String { get; set; }
        [NotMapped]
        public String DISCHARGE_DATE_String { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_VW_CALLS_LOG")]
    public class FOX_VW_CALLS_LOG
    {
        [Key]
        public long FOX_CALLS_LOG_ID { get; set; }
        public long CASE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public long FOX_CALL_TYPE_ID { get; set; }
        public string CALL_TYPE_NAME { get; set; }
        public System.DateTime? DISCHARGE_DATE { get; set; }
        public string PATIENT_STATUS { get; set; }
        public string RESULT_OF_CALL { get; set; }
        public string PROVIDER_NAME { get; set; }
        public string STATUS_OF_CALL { get; set; }
        public string STATUS_OF_CARE { get; set; }
        public System.DateTime? CALL_DATE { get; set; }
        public string CASE_NO { get; set; }
        public System.DateTime? ADMISSION_DATE { get; set; }
        public string CASE_STATUS { get; set; }
        public long? PROVIDER_ID { get; set; }
        public long? REGION_ID { get; set; }
        public long? LOCATION_ID { get; set; }
        public long? FOX_CALL_STATUS_ID { get; set; }
        public long? FOX_CARE_STATUS_ID { get; set; }
        public Nullable<int> GROUP_IDENTIFIER_ID { get; set; }
        public bool? IS_WORK_CALL { get; set; }
        public bool? IS_CELL_CALL { get; set; }
        public bool? IS_HOME_CALL { get; set; }
        public string COMMENTS { get; set; }
        public long? FOX_CALL_RESULT_ID { get; set; }
        public string CALL_RESULT_NAME { get; set; }
        public string REMARKABLE_REPORT_COMMENTS { get; set; }
        public System.DateTime? COMPLETED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class WORK_ORDER_INFO_REQ : BaseModel
    {
        public string PATIENT_ACCOUNT { get; set; }
    }

    public class WORK_ORDER_INFO_RES
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string SENDER_NAME { get; set; }
        public long SENDER_ID { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string RECEIVE_DATE { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public bool? IS_EVALUATE_TREAT { get; set; }
        public string HEALTH_NAME { get; set; }
        public string HEALTH_NUMBER { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public string POS_LOCATION { get; set; }
        public long POS_ID { get; set; }
    }

    public class FOX_TBL_COMMUNICATION_CALL_TYPE
    {
        [Key]
        public long FOX_CALL_TYPE_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CALL_TYPE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class GetOrderingRefSourceinfoRes
    {
        public long? SOURCE_ID { get; set; }
        //public string OrderingRefName { get; set; }
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
    }
    public class GetOrderingRefSourceinfoReq : BaseModel
    {
        public long WORK_ID { get; set; }
        public long CASE_ID { get; set; }
    }

    [Table("FOX_TBL_HEAR_ABOUT_US_OPTIONS")]
    public class FOX_TBL_HEAR_ABOUT_US_OPTIONS
    {
        [Key]
        public long FOX_TBL_HEAR_ABOUT_US_OPTION_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public bool DELETED { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string Template
        {
            get
            {
                return $"[{CODE}] {NAME}";
            }
        }
    }

    public class SmartSearchReq
    {
        public string Keyword { get; set; }
        public string TYPE { get; set; }

    }

    [Table("FOX_TBL_INTERFACE_SYNCH")]
    public class InterfaceSynchModel
    {
        [Key]
        public long FOX_INTERFACE_SYNCH_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? CASE_ID { get; set; }
        public long? TASK_ID { get; set; }
        public bool? IS_SYNCED { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long? Work_ID { get; set; }
        public string APPLICATION { get; set; }
        public long? APPOINTMENT_ID { get; set; }
        public long? GENERAL_NOTE_ID { get; set; }
    }

    public class CaseAndOpenIssues
    {
        public FOX_VW_CASE CaseDetail { get; set; }
        public GetOpenIssueAllListRes OpenIssues { get; set; }
    }


    public class Referral_Region_View
    {
        public long? REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string REFERRAL_REGION { get; set; }
    }
    public class CasesSearchRequest
    {
        public long ProviderCode { get; set; }
        public long LocationCode { get; set; }
        public string ProviderName { get; set; }
        public string LocationName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long PracticeCode { get; set; }    
    }


    [Table("FOX_TBL_CASE_TREATMENT_TEAM")]
    public class FOX_TBL_CASE_TREATMENT_TEAM
    {
        [Key]
        public long TREATMENT_TEAM_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? CASE_ID { get; set; }
        public long? TREATING_PROVIDER_ID { get; set; }
        [NotMapped]
        public String TREATING_PROVIDER { get; set; }
        public DateTime? ADMIT_DATE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public DateTime? LAST_VISIT_DATE { get; set; }
        public DateTime? CERTIFICATION_END_DATE { get; set; }
        public long? CASE_PROVIDER_ID { get; set; }
        [NotMapped]
        public String CASE_PROVIDER { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_HOLD_NON_REASONS")]
    public partial class FOX_TBL_HOLD_NON_REASONS : BaseModel
    {
        [Key]
        public long HOLD_NON_REASONS_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string TYPE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string RT_CODE { get; set; }
    }


}