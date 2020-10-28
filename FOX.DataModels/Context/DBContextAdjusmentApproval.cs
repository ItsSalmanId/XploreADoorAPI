using FOX.DataModels.Models.AdjustmentApproval;
using System.Data.Entity;

namespace FOX.DataModels.Context
{
    public class DBContextAdjusmentApproval: DbContext
    {
        public DBContextAdjusmentApproval() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdjustmentAmount>().Property(t => t.ADJUSTMENT_AMOUNT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AdjustmentClaimStatus>().Property(t => t.STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientAdjustmentDetails>().Property(t => t.ADJUSTMENT_DETAIL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AdjustmentLog>().Property(t => t.ADJUSTMENT_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<AdjustmentAmount> AdjustmentAmount { get; set; }
        public virtual DbSet<AdjustmentClaimStatus> AdjustmentClaimStatus { get; set; }
        public virtual DbSet<PatientAdjustmentDetails> PatientAdjustmentDetails { get; set; }
        public virtual DbSet<AdjustmentLog> AdjustmentLog { get; set; }
    }
}
