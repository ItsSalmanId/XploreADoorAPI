using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.GroupsModel;

namespace FOX.DataModels.Models.TasksModel
{
    [Table("FOX_TBL_TASK")]
    public partial class FOX_TBL_TASK : BaseModel
    {
        [Key]
        public long TASK_ID { get; set; }
        public Nullable<long> PATIENT_ACCOUNT { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> CASE_ID { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR { get; set; }
        public Nullable<int> TASK_TYPE_ID { get; set; }
        public Nullable<int> CATEGORY_ID { get; set; }
        public Nullable<long> PROVIDER_ID { get; set; }
        public Nullable<long> LOC_ID { get; set; }
        public Nullable<long> SEND_TO_ID { get; set; }
        public Nullable<long> FINAL_ROUTE_ID { get; set; }
        public string PRIORITY { get; set; }
        public Nullable<System.DateTime> DUE_DATE_TIME { get; set; }
        [NotMapped]
        public string DUE_DATE_TIME_str { get; set; }
        public Nullable<bool> IS_REQ_SIGNOFF { get; set; }
        public Nullable<bool> IS_SENDING_ROUTE_DETAILS { get; set; }
        public Nullable<long> SEND_CONTEXT_ID { get; set; }
        public string CONTEXT_INFO { get; set; }
        public Nullable<long> DEVELIVERY_ID { get; set; }
        public string DESTINATIONS { get; set; }
        public bool IS_TEMPLATE { get; set; }
        public Nullable<bool> IS_SEND_EMAIL_AUTO { get; set; }
        [Obsolete("This Column is no more being used, need to delete in DB as well.")]
        public Nullable<bool> IS_COMPLETED { get; set; }
        public Nullable<bool> IS_TEMPORARY_DELETED { get; set; }
        public string TEMPORARY_DELETED_BY { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string TASK_TYPE { get; set; }
        [NotMapped]
        public string CATEGORY_CODE { get; set; }
        [NotMapped]
        public string SEND_TO_NAME { get; set; }
        [NotMapped]
        public string FINAL_ROUTE_NAME { get; set; }
        [NotMapped]
        public string CREATED_BY_FULL_NAME { get; set; }
        [NotMapped]
        public string MODIFIED_BY_FULL_NAME { get; set; }
        [NotMapped]
        public string COMMENTS { get; set; }
        [NotMapped]
        public string LOCATION_NAME { get; set; }
        [NotMapped]
        public string LOCATION_CODE { get; set; }
        [NotMapped]
        public string PROVIDER_FULL_NAME { get; set; }
        [NotMapped]
        public string TEMP_DELETED_BY_FULL_NAME { get; set; }
        [NotMapped]
        public List<FOX_TBL_TASK_APPLICATION_USER> TASK_APPLICATION_USER { get; set; }
        [NotMapped]
        public List<FOX_TBL_TASK_SUB_TYPE> TASK_SUB_TYPE { get; set; }
        [NotMapped]
        //public List<FOX_TBL_TASK_SUB_TYPE> TASK_ALL_SUB_TYPES { get; set; }
        public List<OpenIssueViewModel> TASK_ALL_SUB_TYPES_LIST { get; set; }
        [NotMapped]
        public ReferralSource PROVIDER_DETAIL { get; set; }
        public long? GENERAL_NOTE_ID { get; set; }
        public int IS_COMPLETED_INT { get; set; }
        [NotMapped]
        public List<USERS_GROUP> FinalRouteGroupUsers { get; set; }
        [NotMapped]
        public List<USERS_GROUP> SendToGroupUsers { get; set; }
        public string ATTACHMENT_PATH { get; set; }
        public string ATTACHMENT_TITLE { get; set; }
        public bool IS_SEND_TO_USER { get; set; }
        public bool IS_FINAL_ROUTE_USER { get; set; }
        [NotMapped]
        public bool Is_Change { get; set; }
        public bool IS_SENDTO_MARK_COMPLETE { get; set; }
        public bool IS_FINALROUTE_MARK_COMPLETE { get; set; }
        [NotMapped]
        public string dbChangeMsg { get; set; }
    }

    [Table("FOX_TBL_TASK_TYPE")]
    public class FOX_TBL_TASK_TYPE
    {
        [Key]
        public int TASK_TYPE_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public int? CASE_STATUS_ID { get; set; }
        public string RT_CODE { get; set; }
        public string CATEGORY_CODE { get; set; }
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
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public bool? SHOW_IN_TASKS { get; set; }
    }

    [Table("FOX_TBL_TASK_SUB_TYPE")]
    public partial class FOX_TBL_TASK_SUB_TYPE : BaseModel
    {
        [Key]
        public int TASK_SUB_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public int TASK_TYPE_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool IS_EDITABLE { get; set; }
    }
    [Table("FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD")]
    public partial class FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD
    {
        [Key]
        public long TASK_SUB_TYPE_INTEL_TASK_FIELD_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public int TASK_SUB_TYPE_ID { get; set; }
        //public Nullable<long> USER_ID { get; set; }
        public int INTEL_TASK_FIELD_ID { get; set; }
        public bool? IS_REQUIRED { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_INTEL_TASK_CATEGORY")]
    public partial class FOX_TBL_INTEL_TASK_CATEGORY
    {
        [Key]
        public int INTEL_TASK_CATEGORY_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        //[NotMapped]
        //public List<FOX_TBL_INTEL_TASK_FIELD> FieldList { get; set; } = new List<FOX_TBL_INTEL_TASK_FIELD>();
        [NotMapped]
        public List<CatFieldRes> FieldList { get; set; } = new List<CatFieldRes>();
    }
    [Table("FOX_TBL_INTEL_TASK_FIELD")]
    public partial class FOX_TBL_INTEL_TASK_FIELD
    {
        [Key]
        public int INTEL_TASK_FIELD_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public int INTEL_TASK_CATEGORY_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }

        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

        [NotMapped]
        public bool? IS_REQUIRED { get; set; }
    }

    [Table("FOX_TBL_TASK_TASK_SUB_TYPE")]
    public partial class FOX_TBL_TASK_TASK_SUB_TYPE
    {
        [Key]
        public long TASK_TASK_SUB_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> TASK_ID { get; set; }
        public Nullable<int> TASK_SUB_TYPE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_TASK_APPLICATION_USER")]
    public class FOX_TBL_TASK_APPLICATION_USER
    {
        [Key]
        public long TASK_APPLICATION_USER_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? TASK_ID { get; set; }
        public long? USER_ID { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string USER_FNAME { get; set; }
        [NotMapped]
        public string USER_LNAME { get; set; }
        [NotMapped]
        public string USER_EMAIL { get; set; }
    }

    public class DropDownData
    {
        public List<FOX_TBL_SEND_CONTEXT> SEND_CONTEXT { get; set; }
        public List<FOX_TBL_DELIVERY_METHOD> DELIVERY_METHOD { get; set; }
        public List<FOX_TBL_CATEGORY> CATEGORY { get; set; }
        public List<FOX_TBL_ORDER_STATUS> ORDER_STATUS_RESULT { get; set; }
    }

    public class TaskDetail
    {
        [NotMapped]
        public int ROW { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_DATE_STR { get; set; }
        public string CREATED_TIME_STR { get; set; }
        public string CREATED_BY_FULL_NAME { get; set; }
        public string SENT_TO { get; set; }
        public string FINAL_ROUTE { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string TASK_TYPE_NAME { get; set; }
        public string TASK_SUBTYPES { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string PATIENT_ACCOUNT_STR
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
        }
        public string MRN { get; set; }
        public string PATIENT_FULL_NAME { get; set; }
        public string CASE_NO { get; set; }
        public string RT_CASE_NO { get; set; }
        public string PROVIDER_FULL_NAME { get; set; }
        public string LOCATION_NAME { get; set; }
        public string PRIORITY { get; set; }
        public DateTime? DUE_DATE_TIME { get; set; }
        public string DUE_DATE_TIME_STR { get; set; }
        public string REGION { get; set; }
        public string STATE { get; set; }
        public int NO_OF_TIMES_MODIFIED { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public string GENDER { get; set; }
        public long? CASE_ID { get; set; }
        public long TASK_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public long? SEND_TO_ID { get; set; }
        public string TASK_TYPE_DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public string MODIFIED_BY_FULL_NAME { get; set; }
        public bool IS_SENDTO_MARK_COMPLETE { get; set; }
    }

    public class TaskSearchRequest : BaseModel
    {
        public long? CASE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string statusOption { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public long? USER_ID { get; set; }
        public long? INSURANCE_ID { get; set; }
        public long? TASK_TYPE_ID { get; set; }

        public long? TASK_SUB_TYPE_ID { get; set; }
        public long? PROVIDER_ID { get; set; }
        public long? LOC_ID { get; set; }
        public long? CERTIFYING_REF_SOURCE_ID { get; set; }

        public long? CERTIFYING_REF_SOURCE_FAX { get; set; }
        public long? PATIENT_ZIP_CODE { get; set; }
        public int? TIME_FRAME { get; set; }
        public DateTime? DUE_DATE_TIME { get; set; }
        public DateTime? DATE_TO { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string REGION { get; set; }
        public string DUE_DATE_TIME_str { get; set; }
        public string DATE_TO_STR { get; set; }
        public string DATE_FROM_STR { get; set; }
        public long? OWNER_ID { get; set; }
        public string Modified_By { get; set; }
        public bool isUserLevel { get; set; }

    }

    public class categoriesData
    {
        public List<FOX_TBL_INTEL_TASK_CATEGORY> categories { get; set; }
        public List<FOX_TBL_INTEL_TASK_FIELD> Fields { get; set; }
    }
    public class subItems
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_required { get; set; }

    }
    public class GetCategoryFieldResp : ResponseModel
    {
        public List<FOX_TBL_INTEL_TASK_CATEGORY> CategoryList { get; set; }

        //public List<TaskSubtype_Ref_Mapping> SubTypeRef_MappingList { get; set; }
    }
    public class CatFieldRes
    {
        public int TASK_SUB_TYPE_ID { get; set; }
        public string TASK_SUB_TYPE_NAME { get; set; }
        public int INTEL_TASK_CATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public int INTEL_TASK_FIELD_ID { get; set; }
        public string FIELD_NAME { get; set; }
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public bool? IS_REQUIRED { get; set; }
        public long? TASK_SUB_TYPE_INTEL_TASK_FIELD_ID { get; set; }
        public string Condition { get; set; }
        public List<long?> SourceIdList { get; set; }
        public List<int?> OrderStatusIdList { get; set; }
        public List<SmartOrderSource> SourceListDataToDisplay { get; set; }
    }

    //public class SubType_Mapping
    //{
    //    public long TASKSUBTYPE_REFSOURCE_MAPPING_ID { get; set; }
    //    public long? SourceIdList { get; set; }
    //    public int? OrderStatusIdList { get; set; }
    //}

    public class getCatFieldReq : BaseModel
    {
        public int TASK_SUB_TYPE_ID { get; set; }
        public long? TASK_SUB_TYPE_INTEL_TASK_FIELD_ID { get; set; }
        public long TASKSUBTYPE_REFSOURCE_MAPPING_ID { get; set; }
    }

    [Table("FOX_TBL_ACTIVE_LOCATIONS")]
    public class ActiveLocation
    {
        [Key]
        public long LOC_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string Template { get { return $"[{CODE.ToTitleCase()}] {NAME.ToTitleCase()}"; } }
    }

    public class CaseTaskTypeList
    {
        public List<FOX_VW_CASE> CASE { get; set; }
        public List<FOX_TBL_TASK_TYPE> TASK_TYPE { get; set; }
        public Patient.Patient PatientDetail { get; set; }

    }
    [Table("FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING")]
    public class TaskSubtype_Ref_Mapping
    {
        [Key]
        public long TASKSUBTYPE_REFSOURCE_MAPPING_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long TASK_SUB_TYPE_INTEL_TASK_FIELD_ID { get; set; }
        public string CONDITION { get; set; }
        public int ORDER_STATUS_ID { get; set; }
        public long SOURCE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING")]
    public class FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING
    {
        [Key]
        public long TASKSUBTYPE_REFSOURCE_MAPPING_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string CONDITION { get; set; }
        public Nullable<long> SOURCE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public Nullable<int> ORDER_STATUS_ID { get; set; }
        public Nullable<long> TASK_SUB_TYPE_INTEL_TASK_FIELD_ID { get; set; }
    }



    public class TaskWithHistory
    {
        public FOX_TBL_TASK Task { get; set; }
        public List<TaskHistory> taskHistory { get; set; }
    }

    [Table("FOX_TBL_TASK_LOG")]
    public class TaskLog
    {
        [Key]
        public long TASK_LOG_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> TASK_ID { get; set; }
        public string ACTION { get; set; }
        public string ACTION_DETAIL { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

        public static implicit operator List<object>(TaskLog v)
        {
            throw new NotImplementedException();
        }
    }

    public class GetTaskTemplateInitialData
    {
        public List<FOX_TBL_TASK_TYPE> Task_Types { get; set; }
        public DropDownData dropDownData { get; set; }
        public GetTaskTemplateResponse getTaskTemplateResponse { get; set; }
    }

    public class GetTaskTemplateResponse
    {
        public FOX_TBL_TASK Task { get; set; }
        public List<FOX_TBL_TASK_SUB_TYPE> Task_Sub_Types { get; set; }
       
        public List<OpenIssueViewModel> TASK_ALL_SUB_TYPES_LIST { get; set; }
    }

    public class TaskHistory
    {
        public long? taskId { get; set; }
        public string type { get; set; }
        public string detail { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
    }


    public class UserAndGroup
    {
        public long USER_OR_GROUP_ID { get; set; }
        public string USER_OR_GROUP_NAME { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string Template { get; set; }
        public bool IS_USER { get; set; }
        public string RT_USER_ID { get; set; }
    }

    public class TaskStatus
    {
        public string GROUP_NAME { get; set; }
        public int CLOSE_TASK { get; set; }
        public int OPEN_TASK { get; set; }
    }
    public class TaskDashboardSearchRequest
    {
        public long PRACTICE_CODE { get; set; }
        public string GROUP_IDs { get; set; }
        public string TASK_TYPE_IDs { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string DATE_FROM_STR { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public string TIME_FRAME { get; set; }
    }
    public class TaskDueDateStatus
    {
        public string GROUP_NAME { get; set; }
        public int DUE_DATE_TIME { get; set; }
    }
    public class TaskOverAllStatus
    {
        public int? NEW_CREATED { get; set; }
        public int? TOTAL_CLOSE { get; set; }
        public int? TOTAL_OPEN { get; set; }
        public string AVERAGE_CLOSURE { get; set; }
        public int? TOTAL_PAST_DUE { get; set; }
    }
    public class TaskaAverageClosureTime
    {
        public string GROUP_NAME { get; set; }
        public int? AVERAGE_CLOSURE { get; set; }
    }
    public class NewTaskStatus
    {
        public int day { get; set; }
        public int month { get; set; }
        public string monthName { get; set; }
        public int year { get; set; }
        public int totalRecord { get; set; }
        public int? average { get; set; }
    }
    public class TaskTypeDashboardData
    {
        public DateTime dates { get; set; }
        public int? BLOCK { get; set; }
        public int? Call_patient { get; set; }
        public int? PORTA { get; set; }
        public int? Verbal_order { get; set; }
        public int? Demographic_corrections { get; set; }
        public int? Durable_Medical_Equipment_Order { get; set; }
        public int? Audit_Follow_up { get; set; }
        public int? Client_POA { get; set; }
        public int? Contract_Billing_Follow_Up { get; set; }
        public int? Hold_Pending_Assignment { get; set; }
        public int? High_Balance_Review { get; set; }
        public int? Home_Health_Episode { get; set; }
        public int? Home_Health_Follow_up { get; set; }
        public int? Insurance_Follow_up { get; set; }
        public int? Insurance_Verification { get; set; }
        public int? MSP { get; set; }
        public int? Non_Admission_Reasons { get; set; }
        public int? Physician_Follow_up { get; set; }
        public int? Request_Authorization { get; set; }
        public int? Strategic_Accounts { get; set; }
        public int? Verbal_Order { get; set; }
        public int? Verify_Insurance { get; set; }
    }
    public class TaskDashboardResponse : BaseModel
    {
        public List<TaskStatus> TaskStatus { get; set; }
        public List<TaskDueDateStatus> TaskDueDateStatus { get; set; }
        public TaskOverAllStatus TaskOverAllStatus { get; set; }
        public List<TaskaAverageClosureTime> TaskaAverageClosureTime { get; set; }
        public List<NewTaskStatus> NewTaskStatus { get; set; }
        public List<TaskTypeDashboardData> taskTypeDashboardData { get; set; }
        public string TaskTypeDashboardDataString { get; set; }
    }
}
