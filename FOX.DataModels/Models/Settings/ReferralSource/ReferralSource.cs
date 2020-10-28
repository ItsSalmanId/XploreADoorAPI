using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Settings.ReferralSource
{
    [Table("FOX_TBL_ORDERING_REF_SOURCE")]
    public class ReferralSource : BaseModel
    {
        [Key]
        public long SOURCE_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string FIRST_NAME_Title
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(FIRST_NAME?.ToLower() ?? "");
            }
        }
        public string LAST_NAME { get; set; }
        public string LAST_NAME_Title
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(LAST_NAME?.ToLower() ?? "");
            }
        }
        public string MI { get; set; }
        public string Email { get; set; }
        public string ADDRESS { get; set; }
        public string ADDRESS_2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string Speciality { get; set; }
        public string TITLE { get; set; }
       
       
        public string REFERRAL_REGION { get; set; }
        public string REFERRAL_REGION_TITLE
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(REFERRAL_REGION?.ToLower() ?? "");
            }
        }
        public string NPI { get; set; }
     
        public string ACO { get; set; }
        public string ACO_TITLE
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(ACO_NAME?.ToLower() ?? "");
            }
        }
        [NotMapped]
        public string ACO_NAME { get; set; }
        [NotMapped]
        public string PRACTICE_ORGANIZATION_NAME { get; set; }
        public string ORGANIZATION { get; set; }
        public string ORGANIZATION_TITLE
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(PRACTICE_ORGANIZATION_NAME?.ToLower() ?? "");
            }
        }
        public string PHONEFormat
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(PHONE))
                {
                    if (PHONE.Contains("+1"))
                    {
                        PHONE = PHONE.Replace("+1", "");
                        PHONE = String.Format("{0:(###) ###-####}", Int64.Parse(PHONE));
                    }

                }
                return PHONE;
            }
        }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string FaxFormat
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FAX))
                {
                    if (FAX.Contains("+1"))
                    {
                        FAX = FAX.Replace("+1", "");
                        FAX = String.Format("{0:(###) ###-####}", Int64.Parse(FAX));
                    }

                }
                return FAX;
            }
        }
        public string CELL { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public long? INACTIVE_REASON_ID { get; set; }

        public DateTime? INACTIVE_DATE { get; set; }
        [NotMapped]
        public string INACTIVE_DATE_STR { get; set; }
        [NotMapped]
        public string SOURCE_DELIVERY_METHOD_NAME { get; set; }
        public long? SOURCE_DELIVERY_METHOD_ID { get; set; }
        public string NOTES { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_BY_Title
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(CREATED_BY?.ToLower() ?? "");
            }
        }
        public DateTime? CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public string MODIFIED_BY_Title
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(MODIFIED_BY?.ToLower() ?? "");
            }
        }
        public DateTime? MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public string Template { get { return $@"<p>[{CODE}] <b>{LAST_NAME}, {FIRST_NAME}</b><br>{ADDRESS}, {CITY}, {STATE}. {PHONE}</p>"; } }

        
        public long? ACO_ID { get; set; }
        [NotMapped]
        public string Practice_Name { get; set; }
        //MTBC Referral Code Forien Key
        public long? REFERRAL_CODE { get; set; }
        [NotMapped]
        public string REFERRAL_REGION_NAME { get; set; }

        [NotMapped]
        public string INACTIVE_REASON { get; set; }
        [NotMapped]
        public string inactive { get; set; }
    }

    public class ReferralSourceSearch : BaseModel
    {
        public long SOURCE_ID { get; set; }
        public string CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string TITLE { get; set; }
        public string ADDRESS { get; set; }
        public string ADDRESS_2 { get; set; }
        public string Email { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string REFERRAL_REGION { get; set; }
        public string NPI { get; set; }
        public string ORGANIZATION { get; set; }
        public string ACO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string searchString { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
    }

    [Table("FOX_TBL_REFERRAL_SOURCE_INACTIVE_REASON")]
    public class ReferralSourceInactiveReason
    {
        [Key]
        public long INACTIVE_REASON_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string REASON { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_REFERRAL_SOURCE_DELIVERY_METHOD")]
    public class ReferralSourceDeliveryMethod
    {
        [Key]
        public long SOURCE_DELIVERY_METHOD_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class InactiveReasonAndDeliveryMethod
    {
        public List<ReferralSourceInactiveReason> ReferralSourceInactiveReason { get; set; }
        public List<ReferralSourceDeliveryMethod> ReferralSourceDeliveryMethod { get; set; }
    }
    [Table("Referral_Physicians")]

    public class Referral_Physicians
    {
        [Key]
        public long REFERRAL_CODE { get; set; }
        public string REFERRAL_LNAME { get; set; }
        public string REFERRAL_FNAME { get; set; }
        public string REFERRAL_MI { get; set; }
        public string REFERRAL_ADDRESS { get; set; }
        public string REFERRAL_CITY { get; set; }
        public string REFERRAL_STATE { get; set; }
        public string REFERRAL_ZIP { get; set; }
        public string REFERRAL_PHONE { get; set; }
        public string REFERRAL_CONTACT_PERSON { get; set; }
        public string REFERRAL_TAX_ID { get; set; }
        public string REFERRAL_LICENSE { get; set; }
        public string REFERRAL_UPIN { get; set; }
        public string REFERRAL_SSN { get; set; }
        public string REFERRAL_TAXONOMY_CODE { get; set; }
        public string MTBCREFERRAL_CODE { get; set; }
        public bool? EXPORTED { get; set; }
        public bool? RECENT_USE { get; set; }
        public  System.DateTime? MTBC_MODIFIED_DATE { get; set; }
        public System.DateTime? EXPORT_DATE { get; set; }
        public bool IS_DEMO { get; set; }
        public string PIN { get; set; }
        public System.DateTime Sync_Date { get; set; }
        public string NPI { get; set; }
        public string REFERRAL_FAX { get; set; }
        public string title { get; set; }
        public string REFERRAL_EMAIL { get; set; }
        public string REFERRAL_DIRECT_EMAIL { get; set; }
        public bool? IN_ACTIVE { get; set; }
        public string CREATED_FROM { get; set; }
        public int? SALES_GROUP_ID { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }

    }
}