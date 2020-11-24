
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System.Data.Entity;
namespace FOX.DataModels.Context
{
    public class DBContextFoxPHD : DbContext
    {
        public DBContextFoxPHD() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientAddress>().Property(t => t.PATIENT_ADDRESS_HISTORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PHDCallDetail>().Property(t => t.FOX_PHD_CALL_DETAILS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PhdCallScenario>().Property(t => t.PHD_CALL_SCENARIO_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PhdCallReason>().Property(t => t.PHD_CALL_REASON_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PhdCallRequest>().Property(t => t.PHD_CALL_REQUEST_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PhdPatientVerification>().Property(t => t.FOX_PHD_CALL_PATIENT_VERIFICATION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>().Property(t => t.USER_NAME).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PHDUnmappedCalls>().Property(t => t.PHD_CALL_UNMAPPED_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Maintenance>().Property(t => t.Office_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<CSCustomerSupportInfo>().Property(t => t.CS_Case_No).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<CSCaseProgress>().Property(t => t.CS_Case_No).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<CSCaseHistory>().Property(t => t.CS_Track_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CS_Case_Categories>().Property(t => t.CS_Category_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PhdCallMapping>().Property(t => t.PHD_CALL_MAPPING_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PhdCallLogHistory>().Property(t => t.PHD_CALL_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PHDCallDetail> AddUpdatePHDDetails { get; set; }
        public virtual DbSet<PatientAddress> PatientAddresses { get; set; }
        public virtual DbSet<PhdCallScenario> PhdCallScenarios { get; set; }
        public virtual DbSet<PhdCallReason> PhdCallReasons { get; set; }
        public virtual DbSet<PhdCallRequest> PhdCallRequests { get; set; }
        public virtual DbSet<PhdPatientVerification> PhdPatientVerifications { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<PHDUnmappedCalls> PHDUnmappedCalls { get; set; }
        public virtual DbSet<Maintenance> Maintenance { get; set; }
        public virtual DbSet<CSCustomerSupportInfo> CSCustomerSupportInfos { get; set; }
        public virtual DbSet<CSCaseProgress> CSCaseProgresses { get; set; }
        public virtual DbSet<CSCaseHistory> CSCaseHistories { get; set; }
        public virtual DbSet<CS_Case_Categories> CSCaseCategories { get; set; }
        public virtual DbSet<PhdCallMapping> PhdCallMappings{ get; set; }
        public virtual DbSet<PhdCallLogHistory> PhdCallLogHistories { get; set; }
    }
}
