using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.PatientDocuments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Context
{
    public class DBContextPatientDocuments : DbContext
    {
        public DBContextPatientDocuments() : base(EntityHelper.getConnectionStringName())
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoxDocumentType>().Property(t => t.DOCUMENT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<PatientPATDocument>().Property(t => t.PAT_DOCUMENT_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity <PatientDocumentFiles>().Property(t => t.PATIENT_DOCUMENT_FILE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public virtual DbSet<FoxDocumentType> FoxDocumentType { get; set; }
        public virtual DbSet<PatientPATDocument> FoxPATDocument { get; set; }
        public virtual DbSet<PatientDocumentFiles> FoxDocumentFiles { get; set; }
    }
}
