using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DbContextPatientInsurance : DbContext
    {
        public DbContextPatientInsurance() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Patient.Insurances>().Property(t => t.Insurance_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Models.Patient.InsurancesPayer>().Property(t => t.InsPayer_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }


        public virtual DbSet<Models.Patient.Insurances> Insurances { get; set; }
        public virtual DbSet<Models.Patient.InsurancesPayer> InsurancesPayer { get; set; }

    }

}
