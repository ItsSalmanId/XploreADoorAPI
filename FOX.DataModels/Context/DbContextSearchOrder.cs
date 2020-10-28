using FOX.DataModels.Models.SearchOrderModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextSearchOrder : DbContext
    {
        public DbContextSearchOrder() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchOrder>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<SearchOrder> Patient { get; set; }
    }
}