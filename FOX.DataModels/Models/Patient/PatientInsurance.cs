using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.Patient
{
    [Table("INSURANCES")]
    public class Insurances
    {
        [Key]
        public long Insurance_Id { get; set; }
        public long? InsPayer_Id { get; set; }
        public string Insurance_Address { get; set; }
        public string Insurance_City { get; set; }
        public string Insurance_State { get; set; }
        public string Insurance_Zip { get; set; }
        public string Insurance_CardCategory { get; set; }
        public string Insurance_Phone_Type1 { get; set; }
        public string Insurance_Phone_Type2 { get; set; }
        public string Insurance_Phone_Type3 { get; set; }
        public string Insurance_Phone_Number1 { get; set; }
        public string Insurance_Phone_Number2 { get; set; }
        public string Insurance_Phone_Number3 { get; set; }
        public string Insurance_Department { get; set; }
        public bool Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public DateTime? Sync_Date { get; set; }
        //public string rowguid { get; set; }
        //public string Is_Sec_Attach_Need { get; set; }
        //public string IS_SEC_Paper { get; set; }
        //public string InActive { get; set; }
        //public string ClaimFilingLimit { get; set; }
        //public string AppealFilingLimit { get; set; }
        //public string EPSDT_WOKERINFO { get; set; }
        //public string Sub_Method { get; set; }
        //public string Time_From { get; set; }
        //public string Time_To { get; set; }
        //public string STOP_SUBMISSION { get; set; }
        //public string IS_DEMO { get; set; }
    }

    [Table("INSURANCE_PAYERS")]
    public class InsurancesPayer
    {
        [Key]
        public long InsPayer_Id { get; set; }
        public long InsName_Id { get; set; }
        public string InsPayer_Description { get; set; }
        public string InsPayer_Plan { get; set; }
        public string InsPayer_State { get; set; }
        public string InsPayer_837_Id { get; set; }
        public string InsPayer_835_Id { get; set; }
        public string InsPayer_Eligibility_Id { get; set; }
        public string InsPayer_Claim_Status_Id { get; set; }
        public string InsPayer_Referral_Id { get; set; }
        public bool Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public DateTime? Sync_Date { get; set; }
        //public string submission_type { get; set; }
        //public string rowguid { get; set; }
        //public string IS_SEC_PAPER { get; set; }
        //public string Electronic_Corrected_Claims { get; set; }
        //public string Electronic_Late_Filing { get; set; }
        //public string Timely_Filing_Days { get; set; }
        //public string IS_RTA_PAYER { get; set; }
        //public string Restricted_Calls { get; set; }
        //public string SERVER_ID { get; set; }
        //public string Is_Part_A { get; set; }
        //public string IVR_SERVER_ID { get; set; }
        //public string EDISETUPREQUIRED { get; set; }
        //public string ERASETUPREQUIRED { get; set; }
        //public string EFTSETUPREQUIRED { get; set; }
        //public string NPI_TYPE { get; set; }
        //public string MU_Category { get; set; }
        //public string InsPayer_Description_old { get; set; }
        //public string IS_NONPAR_ERA { get; set; }
        //public string IS_NONPAR_CS { get; set; }
        //public string ACKNOWLEDGEMENT_TYPE { get; set; }
    }

    public class UnmappedInsuranceRequest
    {
        public bool includeMpped { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public string FinancialClassID { get; set; }
        public string State { get; set; }
        public string PayerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZIP { get; set; }
        public string Phone { get; set; }
        public string Carrier { get; set; }
        public string Carrier_Locality { get; set; }
        public string Carrier_State { get; set; }
        public string Fee_Redirect { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
    }

    public class MTBCInsurances
    {
        public long INSGROUP_ID { get; set; }
        public string INSGROUP_NAME { get; set; }
        public long INSPAYER_ID { get; set; }
        public string INSPAYER_DESCRIPTION { get; set; }
        public string INSPAYER_PLAN { get; set; }
        public string InsPayer_837_Id { get; set; }
        public string INSPAYER_STATE { get; set; }
        public long INSURANCE_ID { get; set; }
        public string INSNAME_DESCRIPTION { get; set; }
        public long INSNAME_ID { get; set; }
        public string INSURANCE_ZIP { get; set; }
        public string INSURANCE_CITY { get; set; }
        public string INSURANCE_STATE { get; set; }
        public string INSURANCE_ADDRESS { get; set; }
        public string INSURANCE_CARDCATEGORY { get; set; }
        public string INSURANCE_PHONE_TYPE1 { get; set; }
        public string INSURANCE_PHONE_NUMBER1 { get; set; }
        public string INSURANCE_PHONE_TYPE2 { get; set; }
        public string INSURANCE_PHONE_NUMBER2 { get; set; }
        public string INSURANCE_PHONE_TYPE3 { get; set; }
        public string INSURANCE_PHONE_NUMBER3 { get; set; }
        public bool? INACTIVE { get; set; }
        public string INSURANCE_DEPARTMENT { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
    }

    public class MTBCInsurancesSearchData
    {
        public List<string> States { get; set; }
        public List<string> Departments { get; set; }
        public List<string> PhoneTypes { get; set; }
    }

    public class MTBCInsurancesRequest
    {
        public long? payerId { get; set; }
        public string payerDescription { get; set; }
        public long? insNameId { get; set; }
        public string insuranceName { get; set; }
        public string groupName { get; set; }
        public long? insuranceId { get; set; }
        public string insuranceAddress { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string insuranceState { get; set; }
        public string payerState { get; set; }
        public string department { get; set; }
        public string EMC { get; set; }
        public string phone { get; set; }
        public string phoneType { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
    }
}
