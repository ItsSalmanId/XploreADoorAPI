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
        public string ORS_NPI { get; set; }
        public string ORS_FIRST_NAME { get; set; }
        public string ORS_LAST_NAME { get; set; }
        public string ORS_ADDRESS { get; set; }
        public string ORS_CITY { get; set; }
        public string ORS_STATE { get; set; }
        public string ORS_ZIP_CODE { get; set; }
        public string ORS_REGION { get; set; }
        public string ORS_REGION_CODE { get; set; }
        public string ORS_FAX { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public DateTime PATIENT_DOB { get; set; }
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
    public class OrderReferralSourceRequest
    {
        public string OrsNpi { get; set; }
        public string OrsFirstName { get; set; }
        public string OrsLastName { get; set; }
        public string OrsAddress { get; set; }
        public string OrsCity { get; set; }
        public string OrsState { get; set; }
        public string OrsZipCode { get; set; }
        public string OrsRegion { get; set; }
        public string OrsRegionCode { get; set; }
        public string OrsFax { get; set; }
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
    public class OrderingReferralSourceResponse : ResponseModel
    {
        public string OrsNpi { get; set; }
        public string OrsFirstName { get; set; }
        public string OrsLastName { get; set; }
        public string OrsAddress { get; set; }
        public string OrsCity { get; set; }
        public string OrsState { get; set; }
        public string OrsZipCode { get; set; }
        public string OrsRegion { get; set; }
        public string OrsRegionCode { get; set; }
        public string OrsFax { get; set; }
    }
    #endregion
}
