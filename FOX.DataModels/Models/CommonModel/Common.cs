using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.SenderName;
using FOX.DataModels.Models.SenderType;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.CommonModel
{

    public class FileRecieverResult
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string AccessType { get; set; }
    }

    public class SortedDocumentFiles
    {
        public string FilePath { get; set; }
        public int SortNo { get; set; }

    }

    public class CommonFilePath
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_PATH1 { get; set; }

    }

    public class RequestUploadFilesModel
    {
        public IList<string> AllowedFileExtensions { get; set; }
        public string UploadFilesPath { get; set; }
        public HttpFileCollection Files { get; set; }
        public int HR_CONFIGURE_ID { get; set; }
    }

    public class ResponseUploadFilesModel
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }

    public class ResponseLedgerUploadFilesModel
    {
        public string FilePath { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public string AbsolutePath { get; set; }
        public string AbsolutePathWithFileName { get; set; }
        public string OrignalFileName { get; set; }
    }

    public class ResponseModel : BaseModel
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public string ID { get; set; }
        public bool AU { get; set; }
        public string FilePath { get; set; }
    }
    public class SmartSearch
    {
        [NotMapped]
        public string Template { get; set; }
    }

    public class ResponseGetSenderTypesModel : ResponseModel
    {
        public List<FOX_TBL_SENDER_TYPE> SenderTypeList { get; set; }
    }

    public class ResponseGetSenderNamesModel : ResponseModel
    {
        public List<FOX_TBL_SENDER_NAME> SenderNameList { get; set; }
    }

    public class ReqGetSenderNamesModel : BaseModel
    {
        public long SenderTypeId { get; set; }
        public string SearchValue { get; set; }
        public long PracticeCode { get; set; }
        public string UserName { get; set; }
    }

    [Table("FOX_TBL_NOTES")]
    public class Notes
    {
        [Key]
        public long NOTES_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public Nullable<long> CASE_ID { get; set; }
        public Nullable<long> TASK_ID { get; set; }
        public Nullable<long> AUTH_ID { get; set; }
        public Nullable<long> NOTES_TYPE_ID { get; set; }
        public string NOTES { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string CREATED_BY_FULL_NAME { get; set; }
    }

    public class ZipCityState
    {
        public string ZIP_CODE { get; set; }
        public string CITY_NAME { get; set; }
        public string STATE_CODE { get; set; }
    }
    public class ZipCityStateAddress
    {
        public string ADDRESS { get; set; }
        public string ZIP_CODE { get; set; }
        public string CITY_NAME { get; set; }
        public string STATE_CODE { get; set; }
        public string TIME_ZONE { get; set; }
    }

    public class ZipRegionIDName
    {
        public long REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
    }

    public class CityStateModel
    {
        public string NAME { get; set; }
    }

    [Table("FOX_TBL_PROVIDER")]
    public class Provider
    {
        [Key]
        public long FOX_PROVIDER_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string REGION_CODE { get; set; }
        public string REGION_NAME { get; set; }
        public Nullable<long> PROVIDER_CODE { get; set; }
        public string FOX_PROVIDER_CODE { get; set; }
        public string PROVIDER_NAME { get; set; }
        public string CONTRACTED_QUOTA { get; set; }
        public string STATUS { get; set; }
        public string EMPLOYEE_ID { get; set; }
        public string ASHA_ID { get; set; }
        public string UBER { get; set; }
        public string CONTRACT { get; set; }
        public string HOT_SYNC_ENABLED { get; set; }
        public string HOT_SYNC_AUTO_LOCK { get; set; }
        public string LAST_LOGIN { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN_DATE { get; set; }
        public string PROGRAM { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MI { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public string INDIVIDUAL_NPI { get; set; }
        public string GROUP_NPI { get; set; }
        public string LICENSE_NO { get; set; }
        public string TAX_ID { get; set; }
        public string TAXONOMY_CODE { get; set; }
        public string GROUP_TAXONOMY_CODE { get; set; }
        public Nullable<System.DateTime> HOLD_BILLING_UNTIL_DOS { get; set; }
        public string SSN { get; set; }
        public Nullable<long> REFERRAL_REGION_ID { get; set; }
        public Nullable<bool> IS_INACTIVE { get; set; }
        public Nullable<int> DISCIPLINE_ID { get; set; }
        public Nullable<long> TREATMENT_LOC_ID { get; set; }
        public Nullable<int> PRIMARY_POS_DISTANCE { get; set; }
        public Nullable<int> VISIT_QOUTA_WEEK_ID { get; set; }
        public Nullable<int> ACTIVE_CASES { get; set; }
        public Nullable<int> PTO_HRS { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public Nullable<bool> DELETED { get; set; }
        [NotMapped]
        public string Template => $"{LAST_NAME?.ToTitleCase()}, {FIRST_NAME?.ToTitleCase()} | NPI: {INDIVIDUAL_NPI}";
        [NotMapped]
        public string REFERRAL_REGION_CODE { get; set; }
        [NotMapped]
        public string REFERRAL_REGION_NAME { get; set; }
    }

    [Table("FOX_TBL_EMAIL_FAX_LOG")]
    public class EmailFaxLog
    {
        [Key]
        public long FOX_EMAIL_FAX_LOG_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string TYPE { get; set; }
        public string FROM { get; set; }
        public string TO { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string ATTACHMENT_PATH { get; set; }
        public string STATUS { get; set; }
        public string EXCEPTION_SHORT_MSG { get; set; }
        public string EXCEPTION_TRACE { get; set; }
        public long? WORK_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string WORK_STATUS { get; set; }
    }


    public class WORK_ID_MODEL
    {
        public string WORK_STATUS { get; set; }
    }
    [Table("FOX_TBL_SPLASH")]
    public class Splash
    {
        [Key]
        public long FOX_SPLASH_ID { get; set; }
        public string SPLASH_TYPE { get; set; }
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public long SHOW_COUNT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class PHDRecordingConfiguration
    {
        public int PHD_CALLING_UPDATE_SERVICE_CONFIG_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string ENVIRONMENT_NAME { get; set; }
        public bool LOAD_XML { get; set; }
        public string RECORDING_FOLDER_PATH { get; set; }
        public string XML_FOLDER_PATH { get; set; }
        public bool DELETED { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
    }
}