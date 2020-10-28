using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.DataModels.Models.CorrectedClaims;
using FOX.DataModels.Models.CasesModel;

namespace FOX.DataModels.Context
{
    public class DbContextCorrectedClaims : DbContext
    {

        public DbContextCorrectedClaims() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CORRECTED_CLAIM>().Property(t => t.CORRECTED_CLAIM_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<CORRECTED_CLAIM_TYPE>().Property(t => t.CORRECTED_CLAIM_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<AdjustmentClaimStatus>().Property(t => t.STATUS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_CASE>().Property(t => t.CASE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<CorrectedClaimLog>().Property(t => t.CORRECTED_CLAIM_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            
        }

        public virtual DbSet<CORRECTED_CLAIM> CorrectedClaim { get; set; }
        public virtual DbSet<CORRECTED_CLAIM_TYPE> CorrectedClaimType { get; set; }
        public virtual DbSet<AdjustmentClaimStatus> AdjCorrectedClaimType { get; set; }
        public virtual DbSet<FOX_TBL_CASE> caseNo { get; set; }
        public virtual DbSet<CorrectedClaimLog> Log { get; set; }


    }
}
