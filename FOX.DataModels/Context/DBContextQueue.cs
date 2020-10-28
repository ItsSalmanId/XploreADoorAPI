using FOX.DataModels.Models.OriginalQueueModel;
using System.Data.Entity;
using FOX.DataModels.Models.IndexedQueueModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.GroupsModel;

namespace FOX.DataModels.Context
{
    public class DBContextQueue : DbContext
    {
        public DBContextQueue() : base(EntityHelper.getConnectionStringName())
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.OriginalQueueModel.OriginalQueue>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<OriginalQueueFiles>().Property(t => t.FILE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<WorkTransfer>().Property(t => t.WORK_TRANFER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_PATIENT_DOCUMENTS>().Property(t => t.PAT_DOC_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Referral_Assignment_details>().Property(t => t.FOX_REFRRAL_ASSIGNMENT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<RoleToAdd>().Property(t => t.ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>().Property(t => t.USER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<BlacklistedWhitelistedSource>().Property(t => t.BLACKLISTED_WHITELISTED_SOURCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<WorkOrderAddtionalInfo>().Property(t => t.WORK_ORDER_ADDTIONAL_INFO_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<TherapyTreatmentRequestForm>().Property(t => t.THERAPY_TREATMENT_REFERRAL_REQUEST_FORM_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_REFERRAL_SOURCE>().Property(t => t.FOX_SOURCE_CATEGORY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<GROUP>().Property(t => t.GROUP_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

        }

        public virtual DbSet<Models.OriginalQueueModel.OriginalQueue> WorkQueue { get; set; }
        public virtual DbSet<OriginalQueueFiles> OriginalQueueFiles { get; set; }
        public virtual DbSet<WorkTransfer> WorkTransfer { get; set; }
        public virtual DbSet<FOX_TBL_PATIENT_DOCUMENTS> DOC { get; set; }
        public virtual DbSet<Referral_Assignment_details> RefDetails { get; set; }
        public virtual DbSet<RoleToAdd> Role { get; set; }
        public virtual DbSet<User> Usr { get; set; }
        public virtual DbSet<BlacklistedWhitelistedSource> BlacklistedWhitelistedSource { get; set; }
        public virtual DbSet<WorkOrderAddtionalInfo> WorkOrderAddtionalInfo { get; set; }
        public virtual DbSet<TherapyTreatmentRequestForm> TherapyTreatmentRequestForm { get; set; }
        public virtual DbSet<FOX_TBL_REFERRAL_SOURCE> FOX_TBL_REFERRAL_SOURCE { get; set; }
        public virtual DbSet<GROUP> UserGroups { get; set; }


    }
}