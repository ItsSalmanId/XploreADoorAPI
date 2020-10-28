using FOX.DataModels.Models.Patient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DbContextPatientNew : DbContext
    {
        //For minimizing timeout exception
        public DbContextPatientNew() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientUpdateHistory>().Property(t => t.patient_update_history_id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientUpdateHistory> PatientUpdateHistory { get; set; }
    }
}
