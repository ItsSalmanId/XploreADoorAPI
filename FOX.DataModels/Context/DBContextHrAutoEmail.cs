using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using FOX.DataModels.Models.HrAutoEmail;

namespace FOX.DataModels.Context
{
   public class DBContextHrAutoEmail : DbContext
    {
        public DBContextHrAutoEmail() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HrAutoEmailConfigure>().Property(t => t.HR_CONFIGURE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<HrEmailDocumentFileAll>().Property(t => t.HR_MTBC_EMAIL_DOCUMENT_FILE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        public virtual DbSet<HrAutoEmailConfigure> AutoEmailConfigures { get; set; }
        public virtual DbSet<HrEmailDocumentFileAll> HrEmailDocumentFileAlls { get; set; }
    }
}
