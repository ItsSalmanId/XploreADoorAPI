using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextTasks : DbContext
    {
        public DbContextTasks() : base(EntityHelper.getConnectionStringName())
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FOX_TBL_TASK>().Property(t => t.TASK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_TYPE>().Property(t => t.TASK_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_SUB_TYPE>().Property(t => t.TASK_SUB_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_INTEL_TASK_CATEGORY>().Property(t => t.INTEL_TASK_CATEGORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_INTEL_TASK_FIELD>().Property(t => t.INTEL_TASK_FIELD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_SEND_CONTEXT>().Property(t => t.SEND_CONTEXT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_DELIVERY_METHOD>().Property(t => t.DELIVERY_METHOD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_APPLICATION_USER>().Property(t => t.TASK_APPLICATION_USER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CATEGORY>().Property(t => t.CATEGORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD>().Property(t => t.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING>().Property(t => t.TASKSUBTYPE_REFSOURCE_MAPPING_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<TaskLog>().Property(t => t.TASK_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_TASK_TASK_SUB_TYPE>().Property(t => t.TASK_TASK_SUB_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<TaskWorkInterfaceMapping>().Property(t => t.TwmID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<FOX_TBL_TASK> Task { get; set; }
        public virtual DbSet<FOX_TBL_TASK_TYPE> TaskType { get; set; }
        public virtual DbSet<FOX_TBL_TASK_SUB_TYPE> TaskSubType { get; set; }
        public virtual DbSet<FOX_TBL_INTEL_TASK_CATEGORY> categorycheck { get; set; }
        public virtual DbSet<FOX_TBL_INTEL_TASK_FIELD> fieldcheck { get; set; }
        public virtual DbSet<FOX_TBL_SEND_CONTEXT> SendContext { get; set; }
        public virtual DbSet<FOX_TBL_DELIVERY_METHOD> DeliveryMethod { get; set; }
        public virtual DbSet<FOX_TBL_CATEGORY> Category { get; set; }
        public virtual DbSet<FOX_TBL_TASK_APPLICATION_USER> TaskApplicationUser { get; set; }
        public virtual DbSet<ReferralSource> ReferralSource { get; set; }
        public virtual DbSet<ActiveLocation> ActiveLocation { get; set; }
        public virtual DbSet<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD> TaskSubTypeIntelTaskField { get; set; }
        public virtual DbSet<FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING> TaskRefMap { get; set; }
        public virtual DbSet<TaskLog> TaskLog { get; set; }
        public virtual DbSet<FOX_TBL_TASK_TASK_SUB_TYPE> FOX_TBL_TASK_TASK_SUB_TYPE { get; set; }
        public virtual DbSet<TaskWorkInterfaceMapping> TaskWorkInterfaceMapping { get; set; }
    }
}
