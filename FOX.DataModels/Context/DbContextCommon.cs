using System.Data.Entity;
using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.WorkOrderHistoryModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.SenderType;
using FOX.DataModels.Models.SenderName;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.LDAPUser;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.StatesModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Settings;
using FOX.DataModels.Models.Settings.RoleAndRights;

namespace FOX.DataModels.Context
{
    public class DbContextCommon : DbContext
    {
        public DbContextCommon() : base(EntityHelper.getConnectionStringName())
        {
            // Database.SetInitializer<DbContextCommon>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Maintenance_Counter>().Property(t => t.Col_Name).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<WorkOrderHistory>().Property(t => t.LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>().Property(t => t.USER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Patient>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            //modelBuilder.Entity<FOX_TBL_SENDER>().Property(t => t.SENDER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_SENDER_TYPE>().Property(t => t.FOX_TBL_SENDER_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_SENDER_NAME>().Property(t => t.FOX_TBL_SENDER_NAME_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PracticeOrganization>().Property(t => t.PRACTICE_ORGANIZATION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Zip_City_State>().Property(t => t.ZIP_Code).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ActiveDirectoryRole>().Property(t => t.AD_ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES>().Property(t => t.NOTES_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES_TYPE>().Property(t => t.NOTES_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<States>().Property(t => t.State_Code).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Provider>().Property(t => t.FOX_PROVIDER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<EmailFaxLog>().Property(t => t.FOX_EMAIL_FAX_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<OriginalQueue>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Valid_Login_Attempts>().Property(t => t.INVALID_USER_COUNT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<RoleToAdd>().Property(t => t.ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSender>().Property(t => t.ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<Maintenance_Counter> Maintenance_Counter { get; set; }
        public virtual DbSet<WorkOrderHistory> WorkOrderHistory { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        //public virtual DbSet<FOX_TBL_SENDER> Sender { get; set; }
        public virtual DbSet<FOX_TBL_SENDER_TYPE> insertUpdateSenderType { get; set; }
        public virtual DbSet<FOX_TBL_SENDER_NAME> insertUpdateSenderName { get; set; }
        public virtual DbSet<PracticeOrganization> PracticeOrganizations { get; set; }
        public virtual DbSet<Zip_City_State> ZipCitiesStates { get; set; }
        public virtual DbSet<ActiveDirectoryRole> ActiveDirectoryRole { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<EmailFaxLog> EmailFaxLog { get; set; }
        public virtual DbSet<OriginalQueue> OriginalQueue { get; set; }
        public virtual DbSet<Valid_Login_Attempts> ValidLoginAttempts { get; set; }
        public virtual DbSet<RoleToAdd> GetUserRoles { get; set; }
        public virtual DbSet<ReferralSender> ReferralSender { get; set; }

    }
}