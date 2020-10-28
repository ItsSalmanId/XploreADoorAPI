using System.Data.Entity;
using FOX.DataModels.Models.Reconciliation;

namespace FOX.DataModels.Context
{
    public class DBContextHrAutoEmails : DbContext
    {
        public DBContextHrAutoEmails() : base(EntityHelper.getDB5ConnectionStringName())
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MTBC_Credentials_Fox_Automation>().Property(t => t.ASSOCIATION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<MTBC_Credentials_Fox_Automation>().HasKey(o => new { o.ASSOCIATION_ID, o.CREATED_DATE });
        }
        public virtual DbSet<MTBC_Credentials_Fox_Automation> HrAutoEmails { get; set; }
    }
}
