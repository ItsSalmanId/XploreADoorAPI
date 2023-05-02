using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.ExternalUserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.FrictionlessReferral.SupportStaff
{
    #region PROPERTIES
    [Table("FOX_TBL_FRICTIONLESS_REFERRAL")]
    public class FrictionLessReferral
    {
        [Key]
        public long FRICTIONLESS_REFERRAL_ID { get; set; }
        public string USER_TYPE { get; set; }
        public bool IS_SIGNED_REFERRAL { get; set; }
        public string SUBMITER_FIRST_NAME { get; set; }
        public string SUBMITTER_LAST_NAME { get; set; }
        public string SUBMITTER_PHONE { get; set; }
        public string SUBMITTER_EMAIL { get; set; }
        public string PROVIDER_NPI { get; set; }
        public string PROVIDER_FIRST_NAME { get; set; }
        public string PROVIDER_LAST_NAME { get; set; }
        public string PROVIDER_ADDRESS { get; set; }
        public string PROVIDER_CITY { get; set; }
        public string PROVIDER_STATE { get; set; }
        public string PROVIDER_TAXONOMY_DESC { get; set; }
        public string PROVIDER_ZIP_CODE { get; set; }
        public string PROVIDER_REGION { get; set; }
        public string PROVIDER_REGION_CODE { get; set; }
        public string PROVIDER_FAX { get; set; }
        public string PROVIDER_PHONE_NO { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public DateTime? PATIENT_DOB { get; set; }
        [NotMapped]
        public string PATIENT_DOB_STRING { get; set; }
        [NotMapped]
        public bool IS_SUBMIT_CHECK { get; set; }
        [NotMapped]
        public List<string> FILE_NAME_LIST { get; set; }
        public string PATIENT_MOBILE_NO { get; set; }
        public string PATIENT_EMAIL { get; set; }
        public string PATIENT_SUBSCRIBER_ID { get; set; }
        public string PATIENT_INSURANCE_PAYER_ID { get; set; }
        public bool IS_CHECK_ELIGIBILITY { get; set; }
        public string PATIENT_DISCIPLINE_ID { get; set; }
        public string PATIENT_REFERRAL_NOTES { get; set; }
        public long WORK_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }   
    public class InsurancePayer
    {
        public long FoxTblInsurance_Id { get; set; }
        public string InsurancePayersId { get; set; }
        public string InsuranceName { get; set; }
    }
    public class PatientDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }
    }
    public class ProviderReferralSourceRequest
    {
        public string ProviderNpi { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderState { get; set; }
    }
    public class NPPESRegistryRequest
    {
        public int result_count { get; set; }
        public IList<Results> results { get; set; }
    }
    public class Results
    {
        public string created_epoch { get; set; }
        public string enumeration_type { get; set; }
        public string last_updated_epoch { get; set; }
        public string number { get; set; }
        public IList<Taxonomy> taxonomies { get; set; }
        public IList<Address> addresses { get; set; }
        public IList<Identifier> identifiers { get; set; }
        public IList<OtherName> other_names { get; set; }
        public Basic basic { get; set; }
    }
    public class ProviderReferralSourceResponse : ResponseModel
    {
        public string ProviderNpi { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderCity { get; set; }
        public string ProviderState { get; set; }
        public string ProviderTaxonomyDesc { get; set; }
        public string ProviderZipCode { get; set; }
        public string ProviderRegion { get; set; }
        public string ProviderRegionCode { get; set; }
        public string ProviderFax { get; set; }
        public string ProviderPhoneNo { get; set; }
        public bool IsNPPES { get; set; }
    }
    public class FrictionLessReferralResponse 
    {
        public FrictionLessReferral FrictionLessReferralObj { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    [Table("FOX_TBL_FRICTIONLESS_WORK_QUEUE_FILE_ALL")]
    public class FrictionlessReferralForm
    {
        [Key]
        public long FRICTIONLESS_REFERRAL_FILE_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_PATH1 { get; set; }
        public long? WORK_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
        public long PRACTICE_CODE { get; set; }
    }
    public class SubmitReferralModel : BaseModel
    {
        public string AttachmentHTML { get; set; }
        public string FileName { get; set; }
        public long WorkId { get; set; }
        public bool IsFromIndexInfo { get; set; }
        public string PatientLastName { get; set; }
    }

    public class ServiceAvailability : BaseModel
    {
        public string CITY_NAME { get; set; }
        public string ZIP_CODE { get; set; }
    }

    public class ExternalUserInfo : BaseModel
    {
        public string SUBMITER_FIRST_NAME { get; set; }
        public string SUBMITTER_LAST_NAME { get; set; }
        public string SUBMITTER_PHONE { get; set; }
        public string SUBMITTER_EMAIL { get; set; }
        public string ZIP_CODE { get; set; }
        public string CITY_NAME { get; set; }
    }

    public class ServiceAvailable : BaseModel
    {
        public bool IsAvailable { get; set; }
    }
    #endregion
}
