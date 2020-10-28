using FOX.DataModels.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DBContextExceptionLog : DbContext
    {
        public DBContextExceptionLog() : base(EntityHelper.getConnectionStringName())
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FOX_TBL_EXCEPTION_LOG>().Property(t => t.EXCEPTION_LOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<FOX_TBL_EXCEPTION_LOG> FOX_TBL_EXCEPTION_LOG { get; set; }
    }
}