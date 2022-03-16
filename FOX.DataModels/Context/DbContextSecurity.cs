using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.EmailConfig;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.StatesModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static FOX.DataModels.Models.Security.ProfileToken;

namespace FOX.DataModels.Context
{
    public class DbContextSecurity : DbContext
    {
        public DbContextSecurity() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<REGION_ZIPCODE_DATA>().Property(t => t.REGION_ZIPCODE_DATA_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<REFERRAL_REGION_COUNTY>().Property(t => t.REFERRAL_REGION_COUNTY_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>().Property(t => t.USER_NAME).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_PRACTICE_ROLE_RIGHTS>().Property(t => t.RIGHTS_OF_ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<RoleToAdd>().Property(t => t.ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralRegion>().Property(t => t.REFERRAL_REGION_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<RegionCoverLetter>().Property(t => t.REGION_COVER_SHEET_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSource>().Property(t => t.SOURCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_RIGHTS_OF_ROLE>().Property(t => t.ROLE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<InterFaxDetail>().Property(t => t.PRACTICE_CODE).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<EmailConfig>().Property(t => t.ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<NotesHistory>().Property(t => t.NOTE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FacilityLocation>().Property(t => t.LOC_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PasswordHistory>().Property(t => t.PASSWORD_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FacilityType>().Property(t => t.FACILITY_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_USER_RIGHTS>().Property(t => t.RIGHT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_USER_RIGHTS_TYPE>().Property(t => t.RIGHT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_APP_USER_ADDITIONAL_INFO>().Property(t => t.FOX_USER_ADDITIONAL_INFO_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ActiveIndexer>().Property(t => t.ACTIVE_INDEXER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ActiveIndexerLogs>().Property(t => t.ACTIVE_INDEXER_ID_LOGS).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ActiveIndexerHistory>().Property(t => t.ACTIVE_INDEXER_ID_HISTORY).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            //modelBuilder.Entity<WS_TBL_FOX_Login_LOGS>().Property(t => t.LogId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ProfileTokensSecurity>().Property(t => t.TokenSecurityID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);       
        }


        #region Tables
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ProfileToken> Token { get; set; }
        public virtual DbSet<FOX_TBL_PRACTICE_ROLE_RIGHTS> FOX_TBL_PRACTICE_ROLE_RIGHTS { get; set; }
        public virtual DbSet<ReferralRegion> ReferralRegions { get; set; }
        public virtual DbSet<RegionCoverLetter> RegionCoverLetters { get; set; }
        public virtual DbSet<ReferralSource> ReferralSource { get; set; }
        public virtual DbSet<FOX_TBL_RIGHTS_OF_ROLE> FOX_TBL_RIGHTS_OF_ROLE { get; set; }
        public virtual DbSet<EmailConfig> EmailConfig { get; set; }
        public virtual DbSet<NotesHistory> NotesHistory { get; set; }
        public virtual DbSet<FacilityLocation> FacilityLocation { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
        public virtual DbSet<PracticeOrganization> PracticeOrganization { get; set; }
        public virtual DbSet<REGION_ZIPCODE_DATA> RegionsZipCodesData { get; set; }
        public virtual DbSet<REFERRAL_REGION_COUNTY> ReferralRegionCounties { get; set; }
        public virtual DbSet<FacilityType> FacilityTypes { get; set; }
        public virtual DbSet<FOX_TBL_USER_RIGHTS> Rights { get; set; }
        public virtual DbSet<FOX_TBL_USER_RIGHTS_TYPE> RightsType { get; set; }
        public virtual DbSet<FOX_TBL_APP_USER_ADDITIONAL_INFO> USER_ADDITIONAL_INFO { get; set; }
        public virtual DbSet<WS_TBL_FOX_Login_LOGS> FoxLoginLogs { get; set; }
        public virtual DbSet<ProfileTokensSecurity> ProfileTokensSecurities { get; set; }

        #endregion
    }

}