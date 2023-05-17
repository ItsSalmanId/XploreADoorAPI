using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.EligibilityService
{
     public class EligibilityModel
    {
        public string InquiryDate { get; set; }
        public string EligibilityPeriod { get; set; }
        public string PlanBegin { get; set; }
        public string EligibilityStatus { get; set; }
        public CapLimit CapLimit { get; set; }
        public PayerDetails PayerDetails { get; set; }
        public ProvierDetails ProvierDetails { get; set; }
        public object SubscriberDetails { get; set; }
        public LogInformation LogInformation { get; set; }
        public PatientInfo PatientInfo { get; set; }
        public object LstMedicareAdvantage { get; set; }
        public object PartD { get; set; }
        public object Msp { get; set; }
        public object Hospice { get; set; }
        public object HomeHealthCertification { get; set; }
        public List<object> Qmb { get; set; }
        public BenefitInformation BenefitInformation { get; set; }
        public RehabilitationSessions RehabilitationSessions { get; set; }
        public List<PartAb> PartAb { get; set; }
        public List<Dictionary<string, string>> BehavioralServices { get; set; }
        public List<Dictionary<string, string>> PreventiveServices { get; set; }
        public List<Dictionary<string, string>> OtherPreventiveServices { get; set; }
        public List<Dictionary<string, string>> WellnessServices { get; set; }
        public List<object> ElectroCardioGraphic { get; set; }
        public List<EligibilityBenefit> EligibilityBenefit { get; set; }
        public string Name { get; set; }
        public long? Founded { get; set; }
        public List<string> Members { get; set; }
    }

    public partial class BenefitInformation
    {
        public object Qmb { get; set; }
        public string PartAStatus { get; set; }
        public string PartBStatus { get; set; }
        public string PartAPlanDateFrom { get; set; }
        public string PartAPlanDateTo { get; set; }
        public string PartBPlanDateFrom { get; set; }
        public string PartBPlanDateTo { get; set; }
        public object DateOfDeath { get; set; }
        public long? LifetimePsychiatricDays { get; set; }
        public long? LifetimeReserveDays { get; set; }
        public long? SmokingCessationDays { get; set; }
        public object EsrdDialysisMethod { get; set; }
        public string EsrdDialysisStartDate { get; set; }
        public string EsrdTransplantDischargeDate { get; set; }
        public object BeneficiaryIdCrosswalk { get; set; }
        public object ImmunizationNetwork { get; set; }
        public string ImmunizationCoveragePlan { get; set; }
        public string ImmunizationStatus { get; set; }
        public object BloodPoints { get; set; }
        public object Beneficiary { get; set; }
        public object BeneficiaryDate { get; set; }
        public string DeductibleBase { get; set; }
        public string DeductibleRemaining { get; set; }
        public string CoInsurance { get; set; }
        public object RailroadRetirementMedicareBeneficiary { get; set; }
    }

    public partial class CapLimit
    {
        public Therapy OccupationalTherapy { get; set; }
        public Therapy PhysicalTherapy { get; set; }
        public Therapy SpeaechTherapy { get; set; }
    }

    public partial class Therapy
    {
        public long? Amount { get; set; }
        public string DateType { get; set; }
        public string DateValue { get; set; }
        public string InsuranceType { get; set; }
        public string Message { get; set; }
    }

    public partial class EligibilityBenefit
    {
        public string ServiceType { get; set; }
        public string InsuranceType { get; set; }
        public List<object> PlanInformation { get; set; }
        public List<ServiceInformation> ServiceInformation { get; set; }
    }

    public class ServiceInformation
    {
        public object Hcpcs { get; set; }
        public object Description { get; set; }
        public object LongDescription { get; set; }
        public object BenefitDate { get; set; }
        public object TechnicalDate { get; set; }
        public object ProffesionalDate { get; set; }
        public object DeductibleBase { get; set; }
        public object DeductibleRemaining { get; set; }
        public object DeductibleCalendarYear { get; set; }
        public object DeductiblePerVisit { get; set; }
        public object CoInsurance { get; set; }
        public object CoPayment { get; set; }
        public string InsuranceType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public object ReserveLifeTime { get; set; }
        public object ReserveLifeTimeRemaining { get; set; }
        public string BenefitDescription { get; set; }
        public object Limitation { get; set; }
        public object Exclusion { get; set; }
        public string EligibilityBenefit { get; set; }
        public object EligibilityStatus { get; set; }
        public object NetworkType { get; set; }
        public object CoverageLevel { get; set; }
        public object ServiceDelivery { get; set; }
        public object OutOfPocket { get; set; }
        public object OutOfPocketRemaining { get; set; }
    }

    public partial class LogInformation
    {
        public object EligibilityStatus { get; set; }
        public object RejectionReasonCode { get; set; }
        public object FollowUpCode { get; set; }
        public object RejectionLevel { get; set; }
        public string TransactionReferenceNo { get; set; }
        public string TransactionOrgCompanyId { get; set; }
        public object CoPayment { get; set; }
        public object CoInsunrance { get; set; }
        public object Deductible { get; set; }
        public object PcpLastName { get; set; }
        public object PcpFirstName { get; set; }
        public object PcpMi { get; set; }
        public object PrimaryPayer { get; set; }
        public object SecondaryPayer { get; set; }
    }

    public partial class PartAb
    {
        public string Type { get; set; }
        public string FirstBillDate { get; set; }
        public string LastBillDate { get; set; }
        public long? HospitalDaysFull { get; set; }
        public long? HospitalDaysCoins { get; set; }
        public string HospitalDaysBaseAmount { get; set; }
        public long? SnfDaysFull { get; set; }
        public long? SnfDaysCoins { get; set; }
        public string SnfDaysBaseAmount { get; set; }
        public string InPatientDeductible { get; set; }
        public string DeductibleRemaining { get; set; }
        public string PhysicalTherapy { get; set; }
        public string OccupationalTherapy { get; set; }
        public long? BloodPointsPartAb { get; set; }
    }

    public partial class PatientInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string PolicyNumber { get; set; }
        public string Address1 { get; set; }
        public object Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public object Relationship { get; set; }
        public object Ssn { get; set; }
        public object GroupNumber { get; set; }
        public object DateOfDeath { get; set; }
    }

    public partial class PayerDetails
    {
        public string Payer_Name { get; set; }
        public string Payer_ID { get; set; }
    }

    public partial class ProvierDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object MiddleName { get; set; }
        public long? Npi { get; set; }
        public object Ssn { get; set; }
        public string TaxId { get; set; }
        public object MedicareProvierNumber { get; set; }
        public object MedicaidProvierNumber { get; set; }
        public object LicenseNumber { get; set; }
        public object Pin { get; set; }
        public object FacilityNumber { get; set; }
        public object PriorIdentifierNumber { get; set; }
        public string Ptan { get; set; }
        public object Address1 { get; set; }
        public object Address2 { get; set; }
        public object City { get; set; }
        public object State { get; set; }
        public object ZipCode { get; set; }
    }

    public partial class RehabilitationSessions
    {
        public long? PulmonaryRemainingTech { get; set; }
        public long? PulmonaryRemainingProf { get; set; }
        public long? CardiacAppliedTech { get; set; }
        public long? CardiacAppliedProf { get; set; }
        public long? IntensiveCardiacAppliedTech { get; set; }
        public long? IntensiveCardiacAppliedProf { get; set; }
    }

    public partial class Album
    {
        public string Name { get; set; }
        public ArtistClass Artist { get; set; }
        public List<Track> Tracks { get; set; }
    }

    public partial class ArtistClass
    {
        public string Name { get; set; }
        public long? Founded { get; set; }
        public List<string> Members { get; set; }
    }

    public partial class Track
    {
        public string Name { get; set; }
        public long? Duration { get; set; }
    }

    public class EligibilityServiceResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string PolicyNumber { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string GroupNumber { get; set; }
        public string EligibilityPeriod { get; set; }
        public string EligibilityStatus { get; set; }
        public List<EligibilityServiceDetails> EligibilityServices { get; set; }
    }

    public class EligibilityServiceDetails
    {
        public string ServiceType { get; set; }
        public string CoInsurance { get; set; }
        public string DeductibleBase { get; set; }
        public string DeductibleRemaining { get; set; }
        public string OutOfPocket { get; set; }
        public string OutOfPocketRemaining { get; set; }
        public string NetworkType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
