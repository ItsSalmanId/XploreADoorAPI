using FOX.DataModels.HelperClasses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Settings.FacilityLocation
{
    [Table("FOX_TBL_ACTIVE_LOCATIONS")]
    public class FacilityLocation : BaseModel
    {
        [Key]
        public long LOC_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string Address { get; set; }
        [NotMapped]
        public string COMPLETE_ADDRESS { get; set; }
        public string Country { get; set; }
        public string REGION { get; set; }
        [NotMapped]
        public string REGION_NAME { get; set; }
        public string Phone { get; set; }
        [NotMapped]
        public string PhoneFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(Phone))
                {
                    try
                    {
                        var temp = String.Format("{0:(###) ###-####}", Int64.Parse(Phone));
                        if (temp.Length == 13)
                        {
                            temp = temp.Replace("(", "(0");
                            return temp;
                        }
                        return temp;
                    }
                    catch (Exception) { return Phone; }
                }
                return null;
            }
        }
        public string Fax { get; set; }
        [NotMapped]
        public string FaxFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(Fax))
                {
                    try
                    {
                        var temp = String.Format("{0:(###) ###-####}", Int64.Parse(Fax));
                        if (temp.Length == 13)
                        {
                            temp = temp.Replace("(", "(0");
                            return temp;
                        }
                        return temp;
                    }
                    catch (Exception) { return Fax; }
                }
                return null;
            }
        }
        public string POS_Code { get; set; }
        public string Capacity { get; set; }
        public string Census { get; set; }
        public string PT { get; set; }
        public string OT { get; set; }
        public string ST { get; set; }
        public string EP { get; set; }
        public string Lead { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public long? FACILITY_TYPE_ID { get; set; }
        public DateTime? Last_Update { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        [NotMapped]
        public string CREATED_DATE_STRING { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        [NotMapped]
        public string MODIFIED_DATE_STRING { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public long? PRACTICE_CODE { get; set; }

        public string Work_Phone { get; set; }
        public string Cell_Phone { get; set; }
        public string Email_Address { get; set; }
        [NotMapped]
        public string AddressType { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        public bool IS_ACTIVE { get; set; }
        [NotMapped]
        public string FACILITY_TYPE_NAME { get; set; }
        //public bool? IS_MAP { get; set; }
        public long? PT_PROVIDER_ID { get; set; }
        public long? OT_PROVIDER_ID { get; set; }
        public long? ST_PROVIDER_ID { get; set; }
        public long? EP_PROVIDER_ID { get; set; }
        public long? LEAD_PROVIDER_ID { get; set; }
        [NotMapped]
        public long PATIENT_POS_ID { get; set; }
        [NotMapped]
        public bool? Is_Void { get; set; }
        [NotMapped]
        public bool? Is_Default { get; set; }
        [NotMapped]
        public DateTime? Effective_From { get; set; }
        [NotMapped]
        public DateTime? Effective_To { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

    }

    public class FacilityLocationViewModel
    {
 
        public long LOC_ID { get; set; }
        public int ROW { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string Address { get; set; }
        public string COMPLETE_ADDRESS { get; set; }
        public string Country { get; set; }
        public string REGION { get; set; }
        public string REGION_NAME { get; set; }
        public string Phone { get; set; }
        public string PhoneFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(Phone))
                {
                    try
                    {
                        var temp = String.Format("{0:(###) ###-####}", Int64.Parse(Phone));
                        if (temp.Length == 13)
                        {
                            temp = temp.Replace("(", "(0");
                            return temp;
                        }
                        return temp;
                    }
                    catch (Exception) { return Phone; }
                }
                return null;
            }
        }
        public string Fax { get; set; }
        public string FaxFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(Fax))
                {
                    try
                    {
                        var temp = String.Format("{0:(###) ###-####}", Int64.Parse(Fax));
                        if (temp.Length == 13)
                        {
                            temp = temp.Replace("(", "(0");
                            return temp;
                        }
                        return temp;
                    }
                    catch (Exception) { return Fax; }
                }
                return null;
            }
        }
        public string POS_Code { get; set; }
        public string Capacity { get; set; }
        public string Census { get; set; }
        public string PT { get; set; }
        public string OT { get; set; }
        public string ST { get; set; }
        public string EP { get; set; }
        public string Lead { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public long? FACILITY_TYPE_ID { get; set; }
        public DateTime? Last_Update { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string Created_Date_Str { get; set; }
        public string CREATED_DATE_STRING { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string Modified_Date_Str { get; set; }
        public string MODIFIED_DATE_STRING { get; set; }
        public bool DELETED { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string Work_Phone { get; set; }
        public string Cell_Phone { get; set; }
        public string Email_Address { get; set; }
        public string AddressType { get; set; }
        public string Inactive { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string FACILITY_TYPE_NAME { get; set; }
        //public bool? IS_MAP { get; set; }
        public long? PT_PROVIDER_ID { get; set; }
        public long? OT_PROVIDER_ID { get; set; }
        public long? ST_PROVIDER_ID { get; set; }
        public long? EP_PROVIDER_ID { get; set; }
        public long? LEAD_PROVIDER_ID { get; set; }
        public long PATIENT_POS_ID { get; set; }
        public bool? Is_Void { get; set; }
        public bool? Is_Default { get; set; }
        public DateTime? Effective_From { get; set; }
        public DateTime? Effective_To { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }

    public class FacilityLocationSearch : BaseModel
    {
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string searchString { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string CalledFrom { get; set; }
        public string Code { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string Complete_Address { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public int? FacilityType { get; set; }
    }

    public class ProvidersName
    {
        public long Provider_Code { get; set; }
        public string Provid_FName { get; set; }
        public string Provid_MName { get; set; }
        public string Provid_LName { get; set; }
        public string Template { get { return $"<p>{Provid_FName.ToTitleCase()} {Provid_LName.ToTitleCase()}  </p>"; } set { Template = value; } }
    }
    [Table("FOX_TBL_FACILITY_TYPE")]
    public class FacilityType
    {
        [Key]
        public long FACILITY_TYPE_ID { get; set; }
        public string NAME { get; set; }
        public string DISPLAY_NAME { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class GroupIdentifierSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }        
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
    }
    [Table("FOX_TBL_LOCATION_CORPORATION")]
    public class FOX_TBL_LOCATION_CORPORATION
    {
        [Key]
        public long LOCATION_CORPORATION_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        [NotMapped]
        public string Created_Date_Str { get; set; }
        public string MODIFIED_BY { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        [NotMapped]
        public string Modified_Date_Str { get; set; }
        public System.DateTime? END_DATE { get; set; }
        public System.DateTime? START_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public string Inactive { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }

    }
    public class LocationCorporationSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    [Table("FOX_TBL_IDENTIFIER")]
    public class FOX_TBL_IDENTIFIER
    {
        [Key]
        public long IDENTIFIER_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public Nullable<int> IDENTIFIER_TYPE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string IDENTIFIER_TYPE_NAME { get; set; }
        public string CODE_NAME { get; set; }
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
    public class IdentifierSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public long identifier_type { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class AuthStatusSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class TaskTpyeSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class OrderStatusSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class SourceOfreferralSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class AlertTypeSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class DocumentTypeSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class PatientContactTypeSearch : BaseModel
    {
        public string searchString { get; set; }
        public int recordPerpage { get; set; }
        public int currentPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    [Table("FOX_TBL_IDENTIFIER_TYPE")]
    public class IdentifierType
    {
        [Key]
        public int IDENTIFIER_TYPE_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class LocationPatientAccount 
    {
        public string PATIENT_ACCOUNT { get; set; }
        public long Location_id { get; set; }
        public long? WORK_ID { get; set; }
    }
}