using FOX.DataModels.Models.Patient;
using System.Data.Entity;

namespace FOX.DataModels.Context
{
    public class DbContextClaim: DbContext
    {
        public DbContextClaim() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Claims>().Property(t => t.Claim_No).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ClaimInsurance>().Property(t => t.Claim_Insurance_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ClaimNotes>().Property(t => t.Claim_Notes_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        //public virtual DbSet<Claims> Claims { get; set; }
        public virtual DbSet<ClaimInsurance> ClaimInsurance { get; set; }
        public virtual DbSet<ClaimNotes> ClaimNotes { get; set; }
    }
}
