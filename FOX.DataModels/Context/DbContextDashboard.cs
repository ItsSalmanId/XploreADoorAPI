using FOX.DataModels.Models.DashboardModel;
using System.Data.Entity;
//again sent
namespace FOX.DataModels.Context
{
    public class DbContextDashboard : DbContext
    {
        public DbContextDashboard() : base(EntityHelper.getConnectionStringName())
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Dashboard>().Property(t => t.actual).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<Dashboard> Patient { get; set; }
    }
}