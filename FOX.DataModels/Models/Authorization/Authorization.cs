using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Authorization
{
    [Table("FOX_TBL_APPT_TYPE")]
    public class FOX_TBL_APPT_TYPE
    {
        [Key]
        public int APPT_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string APPT_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_AUTH")]
    public class FOX_TBL_AUTH
    {
        [Key]
        public long AUTH_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string AUTH_UNIQUE_ID { get; set; }
        public Nullable<long> AUTH_PARENT_ID { get; set; }
        public Nullable<long> Parent_Patient_insurance_Id { get; set; }
        public Nullable<System.DateTime> RECORD_DATE { get; set; }
        [NotMapped]
        public string RECORD_DATE_IN_STR { get; set; }
        public Nullable<System.DateTime> RECORD_TIME { get; set; }
        [NotMapped]
        public string RECORD_TIME_IN_STR { get; set; }
        public Nullable<System.DateTime> REQUESTED_ON { get; set; }
        [NotMapped]
        public string REQUESTED_ON_IN_STR { get; set; }
        public Nullable<System.DateTime> RECEIVED_ON { get; set; }
        [NotMapped]
        public string RECEIVED_ON_IN_STR { get; set; }
        public Nullable<int> REQUESTED_VISITS { get; set; }
        public Nullable<System.DateTime> EFFECTIVE_FROM { get; set; }
        [NotMapped]
        public string EFFECTIVE_FROM_IN_STR { get; set; }
        public Nullable<System.DateTime> EFFECTIVE_TO { get; set; }
        [NotMapped]
        public string EFFECTIVE_TO_IN_STR { get; set; }
        public Nullable<int> AUTH_STATUS_ID { get; set; }
        public Nullable<long> CASE_ID { get; set; }
        public Nullable<int> DISCIPLINE_ID { get; set; }
        public Nullable<long> BILLING_PROVIDER_ID { get; set; }
        public string AUTH_NUMBER { get; set; }
        public Nullable<long> REFERRED_BY_ID { get; set; }
        public string VERIFIED_BY { get; set; }
        public Nullable<long> LOC_ID { get; set; }
        public Nullable<decimal> MULT_VALUE { get; set; }
        public Nullable<int> MULT_VALUE_TYPE_ID { get; set; }
        [NotMapped]
        public string MULT_VALUE_TYPE_NAME { get; set; }
        public Nullable<decimal> MULT_USED { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<decimal> MULT_REMAINING { get; set; }
        public Nullable<bool> IS_AS_ONE { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string RT_CASE_NO { get; set; }
        [NotMapped]
        public string CASE_NO { get; set; }
        [NotMapped]
        public string BILLING_PROVIDER_NAME { get; set; }
        [NotMapped]
        public string BILLING_PROVIDER_NOTES { get; set; }
        [NotMapped]
        public string REFERRED_BY_NAME { get; set; }
        [NotMapped]
        public string REFERRED_BY_NOTES { get; set; }
        [NotMapped]
        public string LOC_NAME { get; set; }
        [NotMapped]
        public bool IS_SPLIT_AUTH { get; set; }
        [NotMapped]
        public Nullable<decimal> AuthTotalUsed { get; set; }
        [NotMapped]
        public Nullable<decimal> AuthTotalRemaining { get; set; }
        //[NotMapped]
        //public List<FOX_TBL_AUTH_APPT_TYPE> AuthorizationAppointments { get; set; }
        [NotMapped]
        public List<int> AuthAppointmentIds { get; set; }
        [NotMapped]
        public List<FOX_TBL_AUTH_CHARGES> AuthorizationChargesList { get; set; }
        [NotMapped]
        public List<FOX_TBL_AUTH_DOC> AuthorizationDocuments { get; set; }
        [NotMapped]
        public string AuthComments { get; set; }
       
        public Nullable<bool> IsSimple { get; set; }
        
        public string BillCode { get; set; }
        public Nullable<int> SimpleAllowded { get; set; }
        public Nullable<int> SimpleUsed { get; set; }
        [NotMapped]
        public int SimpleRemaining { get; set; }

        [NotMapped]
        public bool ChargesMultiple { get; set; }

    }

    [Table("FOX_TBL_AUTH_APPT_TYPE")]
    public class FOX_TBL_AUTH_APPT_TYPE
    {
        [Key]
        public long AUTH_APPT_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> AUTH_ID { get; set; }
        public Nullable<int> APPT_TYPE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

        [NotMapped]
        public string APPT_CODE { get; set; }
        [NotMapped]
        public string NAME { get; set; }

    }

    [Table("FOX_TBL_AUTH_CHARGES")]
    public class FOX_TBL_AUTH_CHARGES
    {
        [Key]
        public long AUTH_CHARGES_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> AUTH_ID { get; set; }
        public string CHARGES { get; set; } 
        public string CPT_RANGE_FROM_CODE { get; set; }
        public string CPT_RANGE_TO_CODE { get; set; }
        public Nullable<bool> IS_EXEMPT { get; set; }
        public string DIAGNOSIS_CODE { get; set; }
        public decimal? VALUE { get; set; }
        public Nullable<int> VALUE_TYPE_ID { get; set; }
        [NotMapped]
        public string ValueTypeName { get; set; }
        public Nullable<decimal> USED { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<decimal> REMAINING { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped] 
        public bool Disable { get; set; } 

        /////////////

        /////////////////////////////+
        [NotMapped]
        public bool IsValidCPTFrom { get; set; } = true;
        [NotMapped]
        public bool IsValidCPTTo { get; set; } = true;
        [NotMapped]
        public bool ShowSmartProcFrom { get; set; } = false;
        [NotMapped]
        public bool ShowSmartProcTo { get; set; } = false;
        [NotMapped]
        public bool showSmartDiag { get; set; } = false;
        [NotMapped]
        public bool IsValidDiagnosis { get; set; } = true;

        [NotMapped]
        public List<GetSmartProceduresRes> ListToCheckValidProcFrom { get; set; } = new List<GetSmartProceduresRes>();
        [NotMapped]
        public List<GetSmartProceduresRes> ListToCheckValidProcTo { get; set; } = new List<GetSmartProceduresRes>();
        [NotMapped]
        public List<GetSmartProceduresRes> SmartProceduresListFrom { get; set; } = new List<GetSmartProceduresRes>();
        [NotMapped]
        public List<GetSmartProceduresRes> SmartProceduresListTo { get; set; } = new List<GetSmartProceduresRes>();
        [NotMapped]
        public List<GetSmartDiagnosisRes> ListToCheckValidDiag { get; set; } = new List<GetSmartDiagnosisRes>();

        [NotMapped] 
        public string formControlNameCharges { get; set; } = "CHARGES"; 
        [NotMapped]
        public string formControlNameCPTFrom { get; set; } = "CPT_RANGE_FROM_CODE";
        [NotMapped]
        public string formControlNameCPTTo { get; set; } = "CPT_RANGE_TO_CODE";
        [NotMapped]
        public string formControlNameIsExempt { get; set; } = "IS_EXEMPT";
        [NotMapped]
        public string formControlNameDiagnosisCode { get; set; } = "DIAGNOSIS_CODE";
        [NotMapped]
        public string formControlNameValue { get; set; } = "VALUE";
        [NotMapped]
        public string formControlNameValueTypeId { get; set; } = "VALUE_TYPE_ID";
        [NotMapped]
        public string formControlNameValueUsed { get; set; } = "USED";
        [NotMapped]
        public string formControlNameValueRemaining { get; set; } = "REMAINING";
    }

    [Table("FOX_TBL_AUTH_DOC")]
    public class FOX_TBL_AUTH_DOC
    {
        [Key]
        public long AUTH_DOC_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> AUTH_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string LOGO_FILE_PATH { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_AUTH_STATUS")]
    public class FOX_TBL_AUTH_STATUS
    {
        [Key]
        public int AUTH_STATUS_ID { get; set; }
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
        [NotMapped]
        public string Inactive { get; set; }
        public bool ?IS_ACTIVE { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }
    [Table("FOX_TBL_AUTH_VALUE_TYPE")]
    public class FOX_TBL_AUTH_VALUE_TYPE
    {
        [Key]
        public int AUTH_VALUE_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class PatientAuthAppointments
    {
        public long Appt_Id { get; set; }
    }

    public class PatientInsuranceAuthDetails: BaseModel
    {

        public string Patient_Account_Str { get; set; }
        public FOX_TBL_AUTH AuthToCreateUpdate { get; set; }
        public List<FOX_TBL_AUTH> PatientAuthorizationList { get; set; }
        public List<PatientInsurance> Current_Patient_Insurances { get; set; }
        public List<FOX_TBL_APPT_TYPE> AppointmentTypes { get; set; }
        public List<FOX_TBL_AUTH_STATUS> AuthorizationStatuses { get; set; }
        public List<PatientCasesForDD> PatientCasesList { get; set; }
        public List<FOX_TBL_AUTH_VALUE_TYPE> ValueTypeList { get; set; }
        public bool is_Change { get; set; }
    }

    public class splitauthorization {
        public List<FOX_TBL_AUTH> splitauthorizationList { get; set; }
        public Nullable<decimal> GRANDTOTAL { get; set; }
        public Nullable<decimal> AuthGrandTotalUsed { get; set; }
        public Nullable<decimal> AuthGrandTotalRemaining { get; set; }
        public System.DateTime? StartDate { get; set; }
        public System.DateTime? EndDate { get; set; }
    }

    public class WEB_PROC_ADI_VALIDATEUSERIPResult
    {
        public int VALIDIP { get; set; }
        public string countryLONG { get; set; }
    }
}