using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.StatesModel
{
    [Table("States")]
    public class States
    {
        [Key]
        public string State_Code { get; set; }
        public string State_Name { get; set; }
        public bool Deleted { get; set; }
        public Guid rowguid { get; set; }
        //public long? ZoneId { get; set; }
    }
    [Table("FOX_TBL_REGION_ZIPCODE_DATA")]
    public class REGION_ZIPCODE_DATA
    {
        [Key]
        public long REGION_ZIPCODE_DATA_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string STATE { get; set; }
        public string REGION { get; set; }
        public string INSURANCE_CODE { get; set; }
        public string RR_MCR { get; set; }
        public string ZIP_CODE { get; set; }
        public string TOWN_CITY { get; set; }
        public string COUNTY_CITY { get; set; }
        public string REGIONAL_DIRECTOR { get; set; }
        public string SENIOR_REGIONAL_DIRECTOR { get; set; }
        public string ACCOUNT_MANAGER { get; set; }
        public string SPECIAL_NOTES_FOR_ASSIGNMENTS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

    }
    [Table("FOX_TBL_REFERRAL_REGION_COUNTY")]
    public class REFERRAL_REGION_COUNTY
    {
        [Key]
        public long REFERRAL_REGION_COUNTY_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long REFERRAL_REGION_ID { get; set; }
        public long? REGION_ZIPCODE_DATA_ID { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long? ZIP_STATE_COUNTY_ID { get; set; }        
    }
    [Table("FOX_TBL_ZIP_STATE_COUNTY")]
    public class FOX_TBL_ZIP_STATE_COUNTY
    {
        [Key]
        public long ZIP_STATE_COUNTY_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string ZIP_CODE { get; set; }
        public string PLACE_NAME { get; set; }
        //public string STATE_ABBRIVATION { get; set; }
        public string STATE { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        public bool? IS_MAP { get; set; }
        public string COUNTY { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public List<FOX_TBL_ZIP_STATE_COUNTY> Sublist { get; set; }
        [NotMapped]
        public bool Is_New_County { get; set; }
        [NotMapped]
        public string MAPED_ZIP_COUNT { get; set; }
        [NotMapped]
        public string TOTAL_COUNTIES_COUNT { get; set; }
        [NotMapped]
        public string REFERRAL_REGION_CODE { get; set; }
        [NotMapped]
        public string REFERRAL_REGION_NAME { get; set; }
    }
    public class RegionZipCodeDataReq
    {
        public string COUNTY{ get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        public string State { get; set; }
    }
    public class SaveMappCountyReq
    {
        public string COUNTY { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        public List<FOX_TBL_ZIP_STATE_COUNTY> SublistMapped { get; set; }
    }
    public class ZipCityStateCountyRegion
    {
        public string ZIP_CODE { get; set; }
        public string CITY_NAME { get; set; }
        public string STATE_CODE { get; set; }
        public string COUNTY { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }
        public string REGION { get; set; }
    }
}
