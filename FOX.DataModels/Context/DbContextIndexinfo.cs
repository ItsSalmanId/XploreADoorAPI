using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Settings.ReferralSource;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Context
{
    public class DbContextIndexinfo : DbContext
    {
        public DbContextIndexinfo() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FOX_TBL_SENDER>().Property(t => t.SENDER_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_NOTES_HISTORY>().Property(t => t.NOTE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_PATIENT_DIAGNOSIS>().Property(t => t.PAT_DIAG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_PATIENT_PROCEDURE>().Property(t => t.PAT_PROC_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Models.OriginalQueueModel.OriginalQueue>().Property(t => t.WORK_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<IndexPatReq>().Property(t => t.Patient_Account).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FOX_TBL_PATIENT_DOCUMENTS>().Property(t => t.PAT_DOC_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReferralSource>().Property(t => t.SOURCE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxDocumentType>().Property(t => t.DOCUMENT_TYPE_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<FoxSpecialityProgram>().Property(t => t.SPECIALITY_PROGRAM_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        //public virtual DbSet<FOX_TBL_SENDER> Sender { get; set; }
        public virtual DbSet<FOX_TBL_NOTES_HISTORY> NotesHistory { get; set; }
        public virtual DbSet<FOX_TBL_PATIENT_DIAGNOSIS> InsertDiag { get; set; }
        public virtual DbSet<FOX_TBL_PATIENT_PROCEDURE> InsertProc { get; set; }
        public virtual DbSet<Models.OriginalQueueModel.OriginalQueue> InsertSourceAdd { get; set; }
        public virtual DbSet<IndexPatReq> updatePatient { get; set; }
        public virtual DbSet<FOX_TBL_PATIENT_DOCUMENTS> InsertUpdateDocuments { get; set; }
        public virtual DbSet<ReferralSource> insertupdateSource { get; set; }
        public virtual DbSet<FoxDocumentType> FoxDocumentType { get; set; }
        public virtual DbSet<FoxSpecialityProgram> FoxSpecialityProgram { get; set; }

    }
}