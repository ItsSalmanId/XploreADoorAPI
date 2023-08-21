using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.SurveyAutomation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;

namespace FOX.DataModels.Context
{
    public class DBContextConsentToCare : DbContext
    {
        public DBContextConsentToCare() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoxTblConsentToCare>().Property(t => t.CONSENT_TO_CARE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ConsentToCareDocument>().Property(t => t.DOCUMENTS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ConsentToCareStatus>().Property(t => t.CONSENT_TO_CARE_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<FoxTblConsentToCare> foxTblConsentToCare { get; set; }
        public virtual DbSet<ConsentToCareDocument> ConsentToCareDocument { get; set; }
        public virtual DbSet<ConsentToCareStatus> ConsentToCareStatus{ get; set; }
    }
}
