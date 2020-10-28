using FOX.DataModels.Models.Reporting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextReporting : DbContext
    {
        public DbContextReporting() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferralReport>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<ReferralReport> ReferralReport { get; set; }
    }
}