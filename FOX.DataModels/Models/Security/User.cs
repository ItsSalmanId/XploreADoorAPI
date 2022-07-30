using FOX.DataModels.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Security
{

    // [Serializable]
    [Table("fox_tbl_application_user")]
    //[Table("fox_tbl_application_user_temp2")]
    public class User : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public long USER_ID { get; set; }

        public string USER_NAME { get; set; }

        public long PRACTICE_CODE { get; set; }
        public string PASSWORD { get; set; }
        public int FAILED_PASSWORD_ATTEMPT_COUNT { get; set; }
        public DateTime? PASSWORD_CHANGED_DATE { get; set; }
        [NotMapped]
        public int? ROW { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        [NotMapped]
        public string Employee_name { get; set; }
        public string DESIGNATION { get; set; }
        public bool IS_ADMIN { get; set; }
        [NotMapped]
        public int STATUS { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string USER_DISPLAY_NAME { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public string EMAIL { get; set; }
        public string RT_USER_ID { get; set; }

        public int? RESET_PASS { get; set; }
        public string SECURITY_QUESTION { get; set; }
        public string SECURITY_QUESTION_ANSWER { get; set; }
        public string LOCKEDBY { get; set; }
        public bool IS_LOCKED_OUT { get; set; }
        public DateTime? LAST_LOGIN_DATE { get; set; }
        public string COMMENTS { get; set; }
        public string PASS_RESET_CODE { get; set; }
        public string ACTIVATION_CODE { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string PHONE_NO { get; set; }
        public long? ROLE_ID { get; set; }
        public string ADDRESS_1 { get; set; }
        public string ADDRESS_2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string SENDER_TYPE_NAME { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long? MANAGER_ID { get; set; }
        [NotMapped]
        public string ROLE_NAME { get; set; }
        public string USER_TYPE { get; set; }
        [NotMapped]
        public string PRACTICE_NAME { get; set; }
        public string SENDER_TYPE { get; set; }
        public string NPI { get; set; }
        public string MOBILE_PHONE { get; set; }
        public string FAX { get; set; }
        public string FAX_2 { get; set; }
        public string FAX_3 { get; set; }
        public string GENDER { get; set; }
        public string LANGUAGE { get; set; }
        public string TIME_ZONE { get; set; }
        [NotMapped]
        public string CREATED_DATE_STR { get; set; }
        public DateTime? TERMINATION_DATE { get; set; }
        public string SIGNATURE_PATH { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public string PASSWORD_RESET_TICKS { get; set; }
        public string WORK_PHONE { get; set; }
        public long? NOTE_ID { get; set; }
        public long? ACO { get; set; }
        [NotMapped]
        public string ACO_NAME { get; set; }
        public long? SPECIALITY { get; set; }
        [NotMapped]
        public string SPECIALITY_NAME { get; set; }
        public long? SNF { get; set; }
        [NotMapped]
        public string SNF_NAME { get; set; }
        public long? HOSPITAL { get; set; }
        [NotMapped]
        public string HOSPITAL_NAME { get; set; }
        public long? HHH { get; set; }
        [NotMapped]
        public string HHH_NAME { get; set; }
        public string THIRD_PARTY_REFERRAL_SOURCE { get; set; }
        [NotMapped]
        public string COMMENT { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public bool? IS_APPROVED { get; set; }
        [NotMapped]
        public bool hasToSendEmail { get; set; }
        public string EXTENSION { get; set; }
        public bool? IS_ACTIVE_EXTENSION { get; set; }
        //public long? REFERRAL_SOURCE_ID { get; set; }
        public string PRACTICE_ORGANIZATION_TEXT { get; set; }
        public string ACO_TEXT { get; set; }
        public string SPECIALITY_TEXT { get; set; }
        public string SNF_TEXT { get; set; }
        public string HOSPITAL_TEXT { get; set; }
        public string HHH_TEXT { get; set; }
        [NotMapped]
        public string Template { get { return $"<p>{LAST_NAME.ToTitleCase()}, {FIRST_NAME.ToTitleCase()}</p>"; } }
        public long? USR_REFERRAL_SOURCE_ID { get; set; }
        public string Discriminator { get; set; }
        public string PasswordHash { get; set; }

        [NotMapped]
        //public int TOTAL_RECORD_PAGES { get; set; }
        public Double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public bool? IS_AD_USER { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        [NotMapped]
        public string REGION_NAME { get; set; }

        public bool? FULL_ACCESS_OVER_APP { get; set; }

        [NotMapped]
        public bool? Is_Electronic_POC { get; set; }
        [NotMapped]
        public bool? Is_Blocked { get; set; }
        [NotMapped]
        public string DB_PASSWORD { get; set; }
        [NotMapped]
        public string SELECTED_PASSWORD { get; set; }
        [NotMapped]
        public string SHOW_TO_USER_PASSWORD { get; set; }
        [NotMapped]
        public bool HIDE_EYE_ICON { get; set; }
        [NotMapped]
        public string ADMIN_PASSWORD { get; set; }
        [NotMapped]
        public bool PROFILE { get; set; }
        public int? AUTO_LOCK_TIME_SPAN { get; set; }


    }
    public class UserRequest : BaseModel
    {
        public string SearchText { get; set; }
        public bool FilterIs_Approved { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }

    }

    [Table("FOX_TBL_APP_USER_ADDITIONAL_INFO")]
    public class FOX_TBL_APP_USER_ADDITIONAL_INFO : BaseModel
    {
        [Key]
        public long FOX_USER_ADDITIONAL_INFO_ID { get; set; }
        public long FOX_TBL_APPLICATION_USER_ID { get; set; }
        public bool? IS_ELECTRONIC_POC { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_INVALID_USER_ATTEMPTS")]
    public class Valid_Login_Attempts
    {
        [Key]
        public long INVALID_USER_COUNT_ID { get; set; }
        public string USER_NAME { get; set; }
        public long FAIL_ATTEMPT_COUNT { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
    }

    public class Clinician_And_Referral_Region_Data
    {
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string DASHBOARD_USER_ID { get; set; }
        public long? SHOW_AS_ROLE { get; set; }
        public long ROLE_ID { get; set; }
        public string ROLE_NAME_IN_DASHBOARD { get; set; }
        public string USER_EMAIL { get; set; }
        public string CLINICIAN_EMAIL { get; set; }
        public string RT_USER_ID { get; set; }
        public string FOX_PROVIDER_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public long? REGIONAL_DIRECTOR_ID { get; set; }
        public long? ACCOUNT_MANAGER_ID { get; set; }
        public long? SENIOR_REGIONAL_DIRECTOR_ID { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        public long? DB_REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }

        public string ROLE_NAME { get; set; }


    }
    public class Login_log_Data
    {
        public string Password { get; set; }
        public string DeviceInfo { get; set; }
        public string AdLoginResponse { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class Decripted_Password_Info
    {
        public string Decrypted_Passwords { get; set; }
        public string Device_Info { get; set; }
        public string AdLoginResponse { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_date { get; set; }

    }
    public class User_And_Regions_Data
    {
        public List<Login_log_Data> LoginLog_Info { get; set; }
        public List<Clinician_And_Referral_Region_Data> Reagions_info { get; set; }
        public List<Decripted_Password_Info> Password_Info { get; set; }
        public string Admin_Password { get; set; }
        //public List <string> Decrypted_Password { get; set; }
    }
    public class DeleteUserModel
    {
        public User user { get; set; }
        public string reason { get; set; }
        public bool _isADuser { get; set; }
    }
    [Table("FOX_TBL_ACTIVE_INDEXER")]
    public class ActiveIndexer : BaseModel
    {
        [Key]
        public long ACTIVE_INDEXER_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string INDEXER { get; set; }
        public string DEFAULT_VALUE { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string FIRST_NAME { get; set; }
        [NotMapped]
        public string LAST_NAME { get; set; }
        [NotMapped]
        public int CurrentPage { get; set; }
        [NotMapped]
        public int RecordPerPage { get; set; }
        [NotMapped]
        public string SearchText { get; set; }
        [NotMapped]
        public Double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }
    [Table("FOX_TBL_ACTIVE_INDEXER_LOGS")]
    public class ActiveIndexerLogs : BaseModel
    {
        [Key]
        public long ACTIVE_INDEXER_ID_LOGS { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string INDEXER { get; set; }
        public string LOG_MESSAGE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string FULL_NAME { get; set; }
        [NotMapped]
        public string FIRST_NAME { get; set; }
        [NotMapped]
        public string LAST_NAME { get; set; }
        [NotMapped]
        public int CurrentPage { get; set; }
        [NotMapped]
        public int RecordPerPage { get; set; }
        [NotMapped]
        public string SearchText { get; set; }
        [NotMapped]
        public Double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    [Table("FOX_TBL_ACTIVE_INDEXER_HISTORY")]
    public class ActiveIndexerHistory : BaseModel
    {
        [Key]
        public long ACTIVE_INDEXER_ID_HISTORY { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string INDEXER { get; set; }
        [NotMapped]
        public string FULL_NAME { get; set; }
        public long WORK_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_DATE_STR { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string FIRST_NAME { get; set; }
        [NotMapped]
        public string LAST_NAME { get; set; }
        [NotMapped]
        public int CurrentPage { get; set; }
        [NotMapped]
        public int RecordPerPage { get; set; }
        [NotMapped]
        public string SearchText { get; set; }
        [NotMapped]
        public Double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }
    public class GetTeamList
    {
        public long PHD_CALL_SCENARIO_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }

    }
    public class UserTeamModel
    {
        [Key]
        public long USER_TEAM_ID { get; set; }
        public long USER_ID { get; set; }
        public long PHD_CALL_SCENARIO_ID { get; set; }
        public string PRACTICE_CODE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
    }
    public class TeamAddUpdateModel
    {
        public bool DELETED { get; set; }
        public long PHD_CALL_SCENARIO_ID { get; set; }
        public long ROLE_ID { get; set; }
        public long USER_ID { get; set; }
    }
}
