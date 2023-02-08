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
            modelBuilder.Entity<SurveyQuestions>().Property(t => t.SURVEY_QUESTIONS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientSurveyHistory>().Property(t => t.SURVEY_HISTORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientSurvey>().Property(t => t.SURVEY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
           // modelBuilder.Entity<SurveyAutomationLog>().Property(t => t.SURVEY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AutomatedSurveyUnSubscription>().Property(t => t.AUTOMATED_SURVEY_UNSUBSCRIPTION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<SurveyServiceLog>().Property(t => t.SURVEY_AUTOMATION_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<SurveyQuestions> SurveyQuestions { get; set; }
        public virtual DbSet<PatientSurveyHistory> PatientSurveyHistory { get; set; }
        public virtual DbSet<PatientSurvey> PatientSurvey { get; set; }
       // public virtual DbSet<SurveyAutomationLog> SurveyAutomationLog { get; set; }
        public virtual DbSet<AutomatedSurveyUnSubscription> AutomatedSurveyUnSubscription { get; set; }
        public virtual DbSet<SurveyServiceLog> SurveyServiceLog { get; set; }
    }
}
