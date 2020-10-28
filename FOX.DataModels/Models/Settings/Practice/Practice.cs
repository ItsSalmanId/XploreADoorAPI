using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.SenderType;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Settings.Practice
{
    [Table("PRACTICES_PROFILE_OTHER_INFO")]
    public class InterFaxDetail
    {
        [Key]
        public long PRACTICE_CODE { get; set; }
        public string INTERFAX_NO { get; set; }
        public string INTERFAX_USERNAME { get; set; }
        public string INTERFAX_PASSWORD { get; set; }
        public string ACC_COMPANY { get; set; }
    }

    [Table("FOX_TBL_PRACTICE_ORGANIZATION")]
    public class PracticeOrganization : BaseModel
    {
        [Key]
        public long PRACTICE_ORGANIZATION_ID { get; set; }
        [NotMapped]
        public long ACTIVEROW { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string ZIP { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ADDRESS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public string Template
        {

            get {
                string zipcode = string.IsNullOrEmpty(ZIP) ? "" : ZIP.Length > 5 ? ZIP.Substring(0, 5) + '-' + ZIP.Substring(5) : ZIP;
                return $"<p>[{CODE}] {NAME.ToTitleCase()} <br> { CITY.ToTitleCase()} {zipcode}  {STATE.ToTitleCase()}</p>";
            }
        }
    }

    public class PracticeOrganizationRequest : BaseModel
    {
        public long? PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public string SEARCH_STRING { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public string CalledFrom { get; set; }
    }

    public class RespUserProfileDDL
    {
        public List<PracticeOrganization> PRACTICE_ORGANIZATION { get; set; }
        public List<Role> ROLE { get; set; }
        public List<FOX_TBL_SENDER_TYPE> SENDER_TYPE { get; set; }
    } //Initial/dropdown data of user profile
}