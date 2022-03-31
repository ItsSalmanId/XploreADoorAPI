using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextPatient : DbContext
    {
        public DbContextPatient() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);            
            modelBuilder.Entity<PatientAddress>().Property(t => t.PATIENT_ADDRESS_HISTORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_VW_CASE>().Property(t => t.CASE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientInsurance>().Property(t => t.Patient_Insurance_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientUpdateHistory>().Property(t => t.patient_update_history_id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientPOSLocation>().Property(t => t.Patient_POS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FacilityLocation>().Property(t => t.LOC_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FacilityType>().Property(t => t.FACILITY_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSource>().Property(t => t.SOURCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientContact>().Property(t => t.Contact_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AF_TBL_PATIENT_NEXT_OF_KIN>().Property(t => t.PATIENT_NEXT_OF_KIN_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ContactType>().Property(t => t.Contact_Type_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Subscriber>().Property(t => t.GUARANTOR_CODE).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<MedicareLimitType>().Property(t => t.MEDICARE_LIMIT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<MedicareLimitType>().Property(t => t.MEDICARE_LIMIT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<MedicareLimit>().Property(t => t.MEDICARE_LIMIT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Employer>().Property(t => t.Employer_Code).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            //modelBuilder.Entity<FoxInsurancePayors>().Property(t => t.INSURANCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_PATIENT>().Property(t => t.FOX_TBL_PATIENT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            modelBuilder.Entity<FOX_TBL_AUTH>().Property(t => t.AUTH_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_APPT_TYPE>().Property(t => t.APPT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_AUTH_APPT_TYPE>().Property(t => t.AUTH_APPT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_AUTH_CHARGES>().Property(t => t.AUTH_CHARGES_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_AUTH_DOC>().Property(t => t.AUTH_DOC_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_AUTH_STATUS>().Property(t => t.AUTH_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_AUTH_VALUE_TYPE>().Property(t => t.AUTH_VALUE_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES>().Property(t => t.NOTES_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES_TYPE>().Property(t => t.NOTES_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<OriginalQueue>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_ELIG_HTML>().Property(t => t.ELIG_HTML_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FinancialClass>().Property(t => t.FINANCIAL_CLASS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReconcileDemographics>().Property(t => t.FOX_REC_DEM_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_GENERAL_NOTE>().Property(t => t.GENERAL_NOTE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK>().Property(t => t.TASK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AlertType>().Property(t => t.ALERT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxInsurancePayers>().Property(t => t.INSURANCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientPRDiscount>().Property(t => t.PR_DISCOUNT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientPRPeriod>().Property(t => t.PR_PERIOD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<NoteAlert>().Property(t => t.FOX_TBL_ALERT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<GeneralNotesInterfaceLog>().Property(t => t.FOX_INTERFACE_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PHR>().Property(t => t.USER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_GROUP_IDENTIFIER>().Property(t => t.GROUP_IDENTIFIER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_LOCATION_CORPORATION>().Property(t => t.LOCATION_CORPORATION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER>().Property(t => t.IDENTIFIER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_TYPE>().Property(t => t.TASK_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_ORDER_STATUS>().Property(t => t.ORDER_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_SOURCE_OF_REFERRAL>().Property(t => t.SOURCE_OF_REFERRAL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxDocumentType>().Property(t => t.DOCUMENT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<IdentifierType>().Property(t => t.IDENTIFIER_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            modelBuilder.Entity<Patient_Additional_Info_TalkEHR>().Property(t => t.PATIENT_ADDITIONAL_INFO_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Patient_Address_History_WebEHR>().Property(t => t.PATIENT_ADDRESS_HISTORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientAddressAdditionalInfo>().Property(t => t.PATIENT_ADDRESS_ADDITIONAL_INFO).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            modelBuilder.Entity<PatientAlias>().Property(t => t.PATIENT_ALIAS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<CountryResponse>().Property(t => t.FOX_TBL_COUNTRY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<DsFoxOcr>().Property(t => t.DS_FOX_RFO_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxOcrStatus>().Property(t => t.OCR_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PracticeAddressBook>().Property(t => t.ADDRESSBOOK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        
        public virtual DbSet<Patient> Patient { get; set; }        
        public virtual DbSet<PatientAddress> PatientAddress { get; set; }
        public virtual DbSet<PatientInsurance> PatientInsurance { get; set; }
        public virtual DbSet<PatientUpdateHistory> PatientUpdateHistory { get; set; }
        public virtual DbSet<PatientPOSLocation> PatientPOSLocation { get; set; }
        public virtual DbSet<FacilityLocation> FacilityLocation { get; set; }
        public virtual DbSet<FacilityType> FaclityTypes { get; set; }
        public virtual DbSet<ReferralSource> OrderingRefSource { get; set; }
        public virtual DbSet<PatientContact> PatientContact { get; set; }
        public virtual DbSet<AF_TBL_PATIENT_NEXT_OF_KIN> AF_TBL_PATIENT_NEXT_OF_KIN { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<Subscriber> Subscriber { get; set; }
        public virtual DbSet<MedicareLimitType> MedicareLimitType { get; set; }
        public virtual DbSet<MedicareLimit> MedicareLimit { get; set; }
        public virtual DbSet<Employer> Employer { get; set; }
        //public virtual DbSet<FoxInsurancePayors> FoxInsurancePayors { get; set; }
        public virtual DbSet<FOX_TBL_PATIENT> FOX_TBL_PATIENT { get; set; }

        public virtual DbSet<FOX_TBL_AUTH> FOX_TBL_AUTH { get; set; }
        public virtual DbSet<FOX_TBL_APPT_TYPE> FOX_TBL_APPT_TYPE { get; set; }
        public virtual DbSet<FOX_TBL_AUTH_APPT_TYPE> FOX_TBL_AUTH_APPT_TYPE { get; set; }
        public virtual DbSet<FOX_TBL_AUTH_CHARGES> FOX_TBL_AUTH_CHARGES { get; set; }
        public virtual DbSet<FOX_TBL_AUTH_DOC> FOX_TBL_AUTH_DOC { get; set; }
        public virtual DbSet<FOX_TBL_AUTH_STATUS> FOX_TBL_AUTH_STATUS { get; set; }
        public virtual DbSet<FOX_TBL_AUTH_VALUE_TYPE> FOX_TBL_AUTH_VALUE_TYPE { get; set; }
        public virtual DbSet<FOX_TBL_NOTES> Notes { get; set; }
        public virtual DbSet<FOX_TBL_NOTES_TYPE> NotesType { get; set; }
        public virtual DbSet<OriginalQueue> FOX_TBL_WORK_QUEUE { get; set; }
        public virtual DbSet<FOX_TBL_ELIG_HTML> FOX_TBL_ELIG_HTML { get; set; }
        public virtual DbSet<FinancialClass> FinancialClass { get; set; }
        public virtual DbSet<ReconcileDemographics> ReconcileDemo { get; set; }

        public virtual DbSet<FOX_TBL_GENERAL_NOTE> GeneralNotes { get; set; }
        public virtual DbSet<FOX_TBL_CASE> Case { get; set; }
        public virtual DbSet<FOX_VW_CASE> vwCase { get; set; }
        public virtual DbSet<AlertType> AlertTypes { get; set; }
        public virtual DbSet<FoxInsurancePayers> FoxInsurancePayers { get; set; }
        public virtual DbSet<PatientPRDiscount> PatientPRDiscount { get; set; }
        public virtual DbSet<PatientPRPeriod> PatientPRPeriod { get; set; }
        public virtual DbSet<NoteAlert> NoteAlerts { get; set; }
        public virtual DbSet<GeneralNotesInterfaceLog> GeneralNotesInterfaceLogs { get; set; }
        public virtual DbSet<FOX_TBL_GROUP_IDENTIFIER> FOX_TBL_GROUP_IDENTIFIER { get; set; }
        public virtual DbSet<FOX_TBL_LOCATION_CORPORATION> FOX_TBL_LOCATION_CORPORATION { get; set; }
        public virtual DbSet<Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER> FOX_TBL_IDENTIFIER { get; set; }
        public virtual DbSet<FOX_TBL_TASK_TYPE> FOX_TBL_TASK_TYPE { get; set; }
        public virtual DbSet<FOX_TBL_ORDER_STATUS> FOX_TBL_ORDER_STATUS { get; set; }
        public virtual DbSet<FOX_TBL_SOURCE_OF_REFERRAL> FOX_TBL_SOURCE_OF_REFERRAL { get; set; }
        public virtual DbSet<FoxDocumentType> FoxDocumentType { get; set; }
        public virtual DbSet<IdentifierType> IdentifierType { get; set; }
        public virtual DbSet<PHR> PHR { get; set; }

        public virtual DbSet<Patient_Additional_Info_TalkEHR> Patient_Additional_Info_TalkEHR { get; set; }
        public virtual DbSet<Patient_Address_History_WebEHR> Patient_Address_History_WebEHR { get; set; }
        public virtual DbSet<PatientAddressAdditionalInfo> PatientAddressAdditionalInfo { get; set; }
        public virtual DbSet<CountryResponse> CountryResponse { get; set; }
        public virtual DbSet<DsFoxOcr> DsFoxOcr { get; set; }
        public virtual DbSet<FoxOcrStatus> FoxOcrStatus { get; set; }
        public virtual DbSet<PracticeAddressBook> PracticeAddressBook { get; set; }
    }
}