using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.Patient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DbContextFrictionless : DbContext
    {
        #region FUNCTIONS
        public DbContextFrictionless() : base(EntityHelper.getConnectionStringName()){}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoxInsurancePayers>().Property(t => t.FOX_TBL_INSURANCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FrictionLessReferral>().Property(t => t.FRICTIONLESS_REFERRAL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<FoxInsurancePayers> FoxInsurancePayers { get; set; }
        public virtual DbSet<FrictionLessReferral> FrictionLessReferrals { get; set; }
        #endregion
    }
}
