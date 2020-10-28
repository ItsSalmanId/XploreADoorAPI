using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextCases : DbContext
    {
        public DbContextCases() : base(EntityHelper.getConnectionStringName())
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FOX_TBL_CASE>().Property(t => t.CASE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_VW_CASE>().Property(t => t.CASE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CASE_TYPE>().Property(t => t.CASE_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_DISCIPLINE>().Property(t => t.DISCIPLINE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CASE_STATUS>().Property(t => t.CASE_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CASE_SUFFIX>().Property(t => t.CASE_SUFFIX_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_GROUP_IDENTIFIER>().Property(t => t.GROUP_IDENTIFIER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_SOURCE_OF_REFERRAL>().Property(t => t.SOURCE_OF_REFERRAL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES>().Property(t => t.NOTES_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES_TYPE>().Property(t => t.NOTES_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_ORDER_STATUS>().Property(t => t.ORDER_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_ORDER_INFORMATION>().Property(t => t.ORDER_INFO_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_IDENTIFIER>().Property(t => t.IDENTIFIER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_COMMUNICATION_CALL_TYPE>().Property(t => t.FOX_CALL_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<InterfaceSynchModel>().Property(t => t.FOX_INTERFACE_SYNCH_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            //Task
            modelBuilder.Entity<FOX_TBL_TASK>().Property(t => t.TASK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_TYPE>().Property(t => t.TASK_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_SUB_TYPE>().Property(t => t.TASK_SUB_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_TASK_SUB_TYPE>().Property(t => t.TASK_TASK_SUB_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientInsurance>().Property(t => t.Patient_Insurance_Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Patient>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Models.OriginalQueueModel.OriginalQueue>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<COMMUNICATION_CALL_STATUS>().Property(t => t.FOX_CALL_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<COMMUNICATION_STATUS_OF_CARE>().Property(t => t.FOX_CARE_STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSource>().Property(t => t.SOURCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_VW_CALLS_LOG>().Property(t => t.FOX_CALLS_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CALLS_LOG>().Property(t => t.FOX_CALLS_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_COMMUNICATION_CALL_RESULT>().Property(t => t.FOX_CALL_RESULT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Speciality>().Property(t => t.SPECIALITY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD>().Property(t => t.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientPOSLocation>().Property(t => t.Patient_POS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CASE_TREATMENT_TEAM>().Property(t => t.TREATMENT_TEAM_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_HOLD_NON_REASONS>().Property(t => t.HOLD_NON_REASONS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<FOX_TBL_CASE> Case { get; set; }
        public virtual DbSet<FOX_VW_CASE> vwCase { get; set; }
        public virtual DbSet<FOX_TBL_CASE_TYPE> CaseType { get; set; }
        public virtual DbSet<FOX_TBL_DISCIPLINE> Descipline { get; set; }
        public virtual DbSet<FOX_TBL_CASE_STATUS> Status { get; set; }
        public virtual DbSet<FOX_TBL_CASE_SUFFIX> Suffix { get; set; }
        public virtual DbSet<FOX_TBL_GROUP_IDENTIFIER> GrpIdentifier { get; set; }
        public virtual DbSet<FOX_TBL_SOURCE_OF_REFERRAL> SourceofRef { get; set; }
        public virtual DbSet<FOX_TBL_NOTES> Notes { get; set; }
        public virtual DbSet<FOX_TBL_NOTES_TYPE> NotesType { get; set; }
        public virtual DbSet<FOX_TBL_ORDER_STATUS> OrderStatus { get; set; }
        public virtual DbSet<FOX_TBL_ORDER_INFORMATION> OrderInformation { get; set; }
        public virtual DbSet<FOX_TBL_IDENTIFIER> Identfier { get; set; }
        //task
        public virtual DbSet<FOX_TBL_TASK> Task { get; set; }
        public virtual DbSet<FOX_TBL_TASK_TYPE> TaskType { get; set; }
        public virtual DbSet<FOX_TBL_TASK_SUB_TYPE> TaskSubType { get; set; }
        public virtual DbSet<FOX_TBL_TASK_TASK_SUB_TYPE> TaskTaskSubType { get; set; }
        public virtual DbSet<PatientInsurance> PatInsurance { get; set; }
         public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Models.OriginalQueueModel.OriginalQueue> WorkOrderQueue { get; set; }
        public virtual DbSet<COMMUNICATION_CALL_STATUS> callstatus { get; set; }
        public virtual DbSet<COMMUNICATION_STATUS_OF_CARE> carestatus { get; set; }
        public virtual DbSet<FOX_TBL_CALLS_LOG> CallsLog { get; set; }
        public virtual DbSet<FOX_VW_CALLS_LOG> VWCallsLog { get; set; }
        public virtual DbSet<ReferralSource> Source { get; set; }
        public virtual DbSet<FOX_TBL_COMMUNICATION_CALL_RESULT> CallResult { get; set; }
        public virtual DbSet<FOX_TBL_COMMUNICATION_CALL_TYPE> CallType { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD> TaskSubTypeIntelTaskField { get; set; }
        public virtual DbSet<PatientPOSLocation> _PatientPOSLocation { get; set; }
        public virtual DbSet<InterfaceSynchModel> InterfaceSynch { get; set; }
        public virtual DbSet<FOX_TBL_CASE_TREATMENT_TEAM> FOX_TBL_CASE_TREATMENT_TEAM { get; set; }
        public virtual DbSet<FOX_TBL_HOLD_NON_REASONS> FOX_TBL_HOLD_NON_REASONS { get; set; }

    }
}