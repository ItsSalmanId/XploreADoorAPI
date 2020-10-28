using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;




namespace FOX.DataModels.Context
{
    public class DbContextSP : DbContext
    {
        public DbContextSP() : base(EntityHelper.getConnectionStringName())
        {
            Database.SetInitializer<DbContextSP>(null);
        }

        public DbContextSP(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<DbContextSP>(null);
        }
        
    }
    
}

