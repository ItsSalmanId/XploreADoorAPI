using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.Settings.ClinicianSetup
{
    //[Table("FOX_TBL_PROVIDER")]
    //public class FoxProviderClass
    //{
    //    [Key]
    //    public long FOX_PROVIDER_ID { get; set; }
    //    public long? PRACTICE_CODE { get; set; }
    //    [NotMapped]
    //    public long ROW { get; set; }
    //    public string FOX_PROVIDER_CODE { get; set; }
    //    public long? PROVIDER_CODE { get; set; }
    //    public string FIRST_NAME { get; set; }
    //    public string LAST_NAME { get; set; }
    //    public string NPI { get; set; }
    //    [NotMapped]
    //    public string REFERRAL_REGION_NAME { get; set; }
    //    public string STATE { get; set; }
    //    public string JOB_DESC { get; set; }
    //    [NotMapped]
    //    public string TREATMENT_LOCATION { get; set; }
    //    [NotMapped]
    //    public string DISPLINE_NAME { get; set; }
    //    public int? PRIMARY_POS_DISTANCE { get; set; }
    //    [NotMapped]
    //    public string VISIT_QOUTA_WEEK { get; set; }
    //    public int? ACTIVE_CASES { get; set; }
    //    public int? PTO_HRS { get; set; }
    //    [NotMapped]
    //    public string CREATED_BYNAME { get; set; }
    //    [NotMapped]
    //    public string CREATED_DATE_STRING { get; set; }
    //    public string ADDRESS { get; set; }
    //    public string SSN { get; set; }
    //    public System.DateTime? DATE_OF_BIRTH { get; set; }
    //    //public bool? IS_ACTIVE { get; set; }
    //    public bool? IS_INACTIVE { get; set; }
    //    public long? REFERRAL_REGION_ID { get; set; }
    //    public long? TREATMENT_LOC_ID { get; set; }
    //    public int? DISCIPLINE_ID { get; set; }
    //    public int? VISIT_QOUTA_WEEK_ID { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public System.DateTime? CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public System.DateTime? MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }
    //    [NotMapped]
    //    public string DOB { get; set; }
    //    [NotMapped]
    //    public double TOTAL_RECORD_PAGES { get; set; }
    //    [NotMapped]
    //    public int TOTAL_RECORDS { get; set; }
    //    public string FINANCE_REGION { get; set; }
    //    public System.DateTime? TERMINATION_DATE { get; set; }
    //    public string ACTIVE_INACTIVE { get; set; }
    //    public string SPECIALITY { get; set; }
    //    public string TAXONOMY_CODE { get; set; }
    //    [NotMapped]
    //    public long? PT_PROVIDER_ID { get; set; }
    //    [NotMapped]
    //    public long? OT_PROVIDER_ID { get; set; }
    //    [NotMapped]
    //    public long? ST_PROVIDER_ID { get; set; }
    //    [NotMapped]
    //    public long? EP_PROVIDER_ID { get; set; }
    //}


    [Table("FOX_TBL_PROVIDER")]
    public class FoxProviderClass
    {
        [Key]
        public long FOX_PROVIDER_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        [NotMapped]
        public long ROW { get; set; }
        public string FOX_PROVIDER_CODE { get; set; }
        public Nullable<long> PROVIDER_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public string INDIVIDUAL_NPI { get; set; }
        [NotMapped]
        public string REFERRAL_REGION_NAME { get; set; }
        public string STATE { get; set; }
        [NotMapped]
        public string TREATMENT_LOCATION { get; set; }
        [NotMapped]
        public string DISPLINE_NAME { get; set; }
        public string STATUS { get; set; }
        public int? PRIMARY_POS_DISTANCE { get; set; }
        [NotMapped]
        public string VISIT_QOUTA_WEEK { get; set; }
        public decimal? CLR { get; set; }
        public int? ACTIVE_CASES { get; set; }
        public int? PTO_HRS { get; set; }
        [NotMapped]
        public string CREATED_BYNAME { get; set; }
        [NotMapped]
        public string CREATED_DATE_STRING { get; set; }
        public string ADDRESS { get; set; }
        public string SSN { get; set; }
        public bool? IS_INACTIVE { get; set; }
        
        public long? REFERRAL_REGION_ID { get; set; }
        public long? TREATMENT_LOC_ID { get; set; }
        public int? DISCIPLINE_ID { get; set; }
        public int? VISIT_QOUTA_WEEK_ID { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        [NotMapped]
        public string MODIFIED_BYNAME { get; set; }
        [NotMapped]
        public string MODIFIED_DATE_STRING { get; set; }
        public System.DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string DOB { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public string TAXONOMY_CODE { get; set; }
        [NotMapped]
        public long? PT_PROVIDER_ID { get; set; }
        [NotMapped]
        public long? OT_PROVIDER_ID { get; set; }
        [NotMapped]
        public long? ST_PROVIDER_ID { get; set; }
        [NotMapped]
        public long? EP_PROVIDER_ID { get; set; }
        // new
        public string REGION_CODE { get; set; }
        public string REGION_NAME { get; set; }
        public string PROVIDER_NAME { get; set; }
        public string CONTRACTED_QUOTA { get; set; }
    
        public string EMPLOYEE_ID { get; set; }
        public string ASHA_ID { get; set; }
        public string UBER { get; set; }
        public string CONTRACT { get; set; }
        public string HOT_SYNC_ENABLED { get; set; }
        public string HOT_SYNC_AUTO_LOCK { get; set; }
        public string LAST_LOGIN { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN_DATE { get; set; }
        public string PROGRAM { get; set; }
        public string MI { get; set; }
        public string CITY { get; set; }
        public string ZIP { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
       public string GROUP_NPI { get; set; }
        public string LICENSE_NO { get; set; }
        public string TAX_ID { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        public string GROUP_TAXONOMY_CODE { get; set; }
        public Nullable<System.DateTime> HOLD_BILLING_UNTIL_DOS { get; set; }
     
    }





    [Table("FOX_TBL_VISIT_QOUTA_PER_WEEK")]
    public class VisitQoutaWeek
    {
        [Key]
        public int VISIT_QOUTA_WEEK_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string VISIT_QOUTA_WEEK { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
    }

    public class GetClinicanReq
    {
        public string SEARCH_STRING { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public long FOX_PROVIDER_ID { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string EMAIL { get; set; }
        public string RT_Code { get; set; }
        public string NPI { get; set; }
        public string Treating_Region { get; set; }
        public string Treating_Location { get; set; }
        public string Discipline { get; set; }
    }
    public class GetClinicanRes
    {
        public string Message { get; set; }
    }
    public class ProviderLocationRes
    {
        public long? LOC_ID { get; set; }
        public string CODE { get; set; }
        public string TREATMENT_LOCATION { get; set; }
    }
    public class ProviderLocationReq
    {
        public string ROVIDER_TYPE { get; set; }
        public long FOX_PROVIDER_ID { get; set; }
    }

    public class ProviderExcel
    {
        public string ClinicianCode{ get; set; }
        public string CL { get; set; }

    }

}
