using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.StatesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Security
{
    [Table("FOX_TBL_REFERRAL_REGION")]
    public class ReferralRegion : BaseModel
    {
        [Key]
        public long REFERRAL_REGION_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string REFERRAL_REGION_NAME_Title
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(REFERRAL_REGION_NAME?.ToLower() ?? "");
            }
        }
        [NotMapped]
        public string Seleced_Counties { get; set; }
        [NotMapped]
        public string REGIONAL_DIRECTOR { get; set; }
        [NotMapped]
        public string SENIOR_REGIONAL_DIRECTOR { get; set; }
        public string ACCOUNT_MANAGER { get; set; }
        public string ACCOUNT_MANAGER_EMAIL { get; set; }
        public string ACCOUNT_MANAGER_Title
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                return strConverter.ToTitleCase(ACCOUNT_MANAGER?.ToLower() ?? "");
            }
        }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        public bool IS_ACTIVE { get; set; }
        public DateTime? IN_ACTIVEDATE { get; set; }
        public long? ALTERNATE_REGION_ID { get; set; }
        [NotMapped]
        public string IN_ACTIVEDATE_Str { get; set; }

        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }

        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        //public string Template { get { return $"<p>[{REFERRAL_REGION_CODE}] {REFERRAL_REGION_NAME.ToTitleCase()} </p>"; } }
        public string Template { get { return $"[{REFERRAL_REGION_CODE}] {REFERRAL_REGION_NAME.ToTitleCase()}"; } }

        [NotMapped]
        public string Name { get { return REFERRAL_REGION_NAME; } }
        public long? REGIONAL_DIRECTOR_ID { get; set; }
        public string STATE_CODE { get; set; }
        [NotMapped]
        public List<FOX_TBL_ZIP_STATE_COUNTY> COUNTIES { get; set; }
        [NotMapped]
        public string ALT_REGIONAL_DIRECTOR_NAME { get; set; }
        public bool? IS_INACTIVE { get; set; }
        [NotMapped]
        public List<FOX_TBL_ZIP_STATE_COUNTY> ZipStateCountyList { get; set; }
        public long? ACCOUNT_MANAGER_ID { get; set; }
        [NotMapped]
        public string ACCOUNT_MANAGER_NAME { get; set; }
        public long? SENIOR_REGIONAL_DIRECTOR_ID  { get; set; }
        [NotMapped]
        public string SENIOR_REGIONAL_DIRECTOR_NAME { get; set; }    
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        [NotMapped]
        public List<FOX_TBL_DASHBOARD_ACCESS> Dashboard_Access { get; set; }
        [NotMapped]
        public List<FOX_TBL_DASHBOARD_ACCESS> Dashboard_AccessTemp { get; set; }
        [NotMapped]
        public string REGIONAL_DIRECTOR_NAME { get; set; }

    }

    public class ReferralRegionSearch : BaseModel
    {
        public long REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string ACCOUNT_MANAGER_EMAIL { get; set; }
        public string ACCOUNT_MANAGER { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public int signal { get; set; }
        public string searchString { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public long PracticeCode { get; set; }
    }

    public class AdvancedRegionSmartSearch
    {
        public long REFERRAL_REGION_ID { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string REGION_CODE_NAME { get; set; }
    }
    public class AdvancedRegionsWithZipCodes
    {
        public long ROW_NUM { get; set; }
        public string STATE_CODE { get; set; }
        public string REFERRAL_REGION_CODE { get; set; }
        public string REFERRAL_REGION_NAME { get; set; }
        public string COUNTY { get; set; }
        public string CITY { get; set; }
        public string ZIP_CODE { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }
    public class AdvanceRegionSearchRequest
    {
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string CheckedRegionsIDString { get; set; }
    }
    public class showHideAdvancedRegionCol
    {
        public bool STATE_CODE { get; set; }
        public bool REFERRAL_REGION_CODE { get; set; }
        public bool REFERRAL_REGION_NAME { get; set; }
        public bool COUNTY { get; set; }
        public bool CITY { get; set; }
        public bool ZIP_CODE { get; set; }
        public AdvanceRegionSearchRequest ObjAdvanceRegionSearchRequest { get; set; }
    }
[Table("FOX_TBL_DASHBOARD_ACCESS")]
public class FOX_TBL_DASHBOARD_ACCESS : BaseModel
{
    [Key]
    public long DASHBOARD_ACCESS_ID { get; set; }
    public string USER_NAME { get; set; }
    public long? SHOW_AS_ROLE { get; set; }
    public long? REFERRAL_REGION_ID { get; set; }
    public bool? IS_WRITE_ALLOWDED { get; set; }
    public bool? DELETED { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime? CREATED_ON { get; set; }
    public string MODIFIED_BY { get; set; }
    public DateTime? MODIFIED_DATE { get; set; }
    [NotMapped]
    public string LAST_NAME { get; set; }
    [NotMapped]
    public string ROLE_NAME { get; set; }
    [NotMapped]
    public string FIRST_NAME { get; set; }
}

public class DashBoardUserModal
{
    public List<FOX_TBL_DASHBOARD_ACCESS> RegionDashBoardUser { get; set; }
    public List<User> DashBoardUsers { get; set; }
}

}