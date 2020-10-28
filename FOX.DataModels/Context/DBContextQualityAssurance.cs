using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DBContextQualityAssurance : DbContext
    {
        public DBContextQualityAssurance() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EvaluationCriteria>().Property(t => t.EVALUATION_CRITERIA_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<EvaluationCriteriaCategories>().Property(t => t.EVALUATION_CRITERIA_CATEGORIES_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<WowFactor>().Property(t => t.WOW_FACTOR_NEGATIVE_FEEDBACK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<GradingSetup>().Property(t => t.GRADING_SETUP_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<SurveyAuditScores>().Property(t => t.SURVEY_AUDIT_SCORES_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>().Property(t => t.USER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<EvaluationCriteria> EvaluationCriteria { get; set; }
        public virtual DbSet<EvaluationCriteriaCategories> EvaluationCriteriaCategories { get; set; }
        public virtual DbSet<WowFactor> WowFactor { get; set; }
        public virtual DbSet<GradingSetup> GradingSetup { get; set; }
        public virtual DbSet<SurveyAuditScores> SurveyAuditScores { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
