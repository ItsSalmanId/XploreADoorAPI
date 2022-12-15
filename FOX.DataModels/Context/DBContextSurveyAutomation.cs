using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.SurveyAutomation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DBContextSurveyAutomation : DbContext
    {
        public DBContextSurveyAutomation() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<SurveyQuestions>().Property(t => t.PATIENT_SURVEY_QUESTIONS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<Patient> patient { get; set; }
        public virtual DbSet<SurveyQuestions> surveyQuestions { get; set; }
    }
}
