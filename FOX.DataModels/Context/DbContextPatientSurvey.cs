using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System.Data.Entity;

namespace FOX.DataModels.Context
{
    public class DbContextPatientSurvey: DbContext
    {
        public DbContextPatientSurvey() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientSurvey>().Property(t => t.SURVEY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientSurveyHistory>().Property(t => t.SURVEY_HISTORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientSurveyCallLog>().Property(t => t.SURVEY_CALL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientSurveyFormatType>().Property(t => t.SURVEY_FORMAT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<RoleToAdd>().Property(t => t.ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<SurveyServiceLog>().Property(t => t.SURVEY_AUTOMATION_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientSurveyNotAnswered>().Property(t => t.NOT_ANSWERD_REASON_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<PatientSurvey> PatientSurvey { get; set; }
        public virtual DbSet<PatientSurveyHistory> PatientSurveyHistory { get; set; }
        public virtual DbSet<PatientSurveyCallLog> PatientSurveyCallLog { get; set; }
        public virtual DbSet<PatientSurveyFormatType> PatientSurveyFormatType { get; set; }
        public virtual DbSet<RoleToAdd> Role { get; set; }
        public virtual DbSet<SurveyServiceLog> SurveyServiceLog { get; set; }
        public virtual DbSet<PatientSurveyNotAnswered> PatientSurveyNotAnswered { get; set; }
    }
}