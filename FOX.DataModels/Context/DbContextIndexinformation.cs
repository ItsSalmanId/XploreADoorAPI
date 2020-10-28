using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextIndexinformation : DbContext
    {
        public DbContextIndexinformation() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        
    }
}