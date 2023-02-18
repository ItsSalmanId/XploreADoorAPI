using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.StatesModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextSettings : DbContext
    {
        public DbContextSettings() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferralRegion>().Property(t => t.REFERRAL_REGION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PracticeOrganization>().Property(t => t.PRACTICE_ORGANIZATION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSourceInactiveReason>().Property(t => t.INACTIVE_REASON_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSourceDeliveryMethod>().Property(t => t.SOURCE_DELIVERY_METHOD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<GROUP>().Property(t => t.GROUP_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<USERS_GROUP>().Property(t => t.GROUP_USER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_IDENTIFIER>().Property(t => t.IDENTIFIER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxProviderClass>().Property(t => t.FOX_PROVIDER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<VisitQoutaWeek>().Property(t => t.VISIT_QOUTA_WEEK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Referral_Physicians>().Property(t => t.REFERRAL_CODE).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_ZIP_STATE_COUNTY>().Property(t => t.ZIP_STATE_COUNTY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_DASHBOARD_ACCESS>().Property(t => t.DASHBOARD_ACCESS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxRoles>().Property(t => t.ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Announcements>().Property(t => t.ANNOUNCEMENT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AnnouncementRoles>().Property(t => t.ANNOUNCEMENT_ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<OtpEnableDate>().Property(t => t.OtpEnableId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

        }
        public virtual DbSet<ReferralRegion> ReferralRegion { get; set; }
        public virtual DbSet<PracticeOrganization> PracticeOrganization { get; set; }
        public virtual DbSet<ReferralSourceInactiveReason> ReferralSourceInactiveReason { get; set; }
        public virtual DbSet<ReferralSourceDeliveryMethod> ReferralSourceDeliveryMethod { get; set; }
        public virtual DbSet<GROUP> Groups { get; set; }
        public virtual DbSet<USERS_GROUP> UsersGroups { get; set; }
        public virtual DbSet<FOX_TBL_IDENTIFIER> FOX_TBL_IDENTIFIER { get; set; }
        public virtual DbSet<FoxProviderClass> FoxProvider { get; set; }
        public virtual DbSet<VisitQoutaWeek> VisitQoutaWeek { get; set; }
        public virtual DbSet<Referral_Physicians> _Referral_Physicians { get; set; }
        public virtual DbSet<FOX_TBL_ZIP_STATE_COUNTY> ZipCityCounty { get; set; }
        public virtual DbSet<FOX_TBL_DASHBOARD_ACCESS> DashBoardAccess { get; set; }
        public virtual DbSet<FoxRoles> FoxRoles { get; set; }
        public virtual DbSet<Announcements> Announcements { get; set; }
        public virtual DbSet<AnnouncementRoles> AnnouncementRoles { get; set; }
        public virtual DbSet<OtpEnableDate> OtpEnableDate { get; set; }
    }
}