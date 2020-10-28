using FOX.DataModels.Models.Patient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DBContextMTBCPatientInsurance : DbContext
    {
        public DBContextMTBCPatientInsurance() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MTBCPatientInsurance>().Property(t => t.Patient_Insurance_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<MTBCPatientInsurance> MTBCPatientInsurance { get; set; }
    }
}
