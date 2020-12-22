using FOX.DataModels.Models.Reconciliation;
using System.Data.Entity;

namespace FOX.DataModels.Context
{
    public class DBContextReconciliations : DbContext
    {
        public DBContextReconciliations() : base(EntityHelper.getConnectionStringName()) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReconciliationStatus>().Property(t => t.RECONCILIATION_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReconciliationDepositType>().Property(t => t.DEPOSIT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReconciliationCategory>().Property(t => t.CATEGORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReconciliationFiles>().Property(t => t.RECONCILIATION_FILE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReconciliationCP>().Property(t => t.RECONCILIATION_CP_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReconciliationCPLogs>().Property(t => t.RECONCILIATION_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<MTBC_Credentials_Fox_Automation>().Property(t => t.MTBC_CREDENTIALS_AUTOMATION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<ReconciliationStatus> ReconciliationStatus { get; set; }
        public virtual DbSet<ReconciliationDepositType> ReconciliationDepositType { get; set; }
        public virtual DbSet<ReconciliationCategory> ReconciliationCategory { get; set; }
        public virtual DbSet<ReconciliationFiles> ReconciliationFiles { get; set; }
        public virtual DbSet<ReconciliationCP> ReconciliationCP { get; set; }
        public virtual DbSet<ReconciliationCPLogs> ReconciliationCPLogs { get; set; }
        public virtual DbSet<MTBC_Credentials_Fox_Automation> HrAutoEmails { get; set; }
    }
}
