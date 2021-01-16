using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.HrAutoEmail
{
    [Table("FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS")]
    public class HrAutoEmailConfigure
    {
        [Key]
        public long HR_CONFIGURE_ID { get; set; }
        public string NAME { get; set; }
        public long PRACTICE_CODE { get; set; }
        public bool IS_ENABLED { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public string FILE_PATH { get; set; }
    }
    [Table("FOX_TBL_HR_EMAIL_DOCUMENT_FILE_ALL")]
    public class HrEmailDocumentFileAll
    {
        [Key]
        public long HR_MTBC_EMAIL_DOCUMENT_FILE_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long HR_CONFIGURE_ID { get; set; }
        public string DOCUMENT_PATH { get; set; }
        public string ORIGINAL_FILE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class GetDocumentFileDetails
    {
        public long HR_CONFIGURE_ID { get; set; }
        public string NAME { get; set; }
        public long PRACTICE_CODE { get; set; }
        public bool IS_ENABLED { get; set; }
        public bool DELETED { get; set; }
        public string DOCUMENT_PATH { get; set; }
        public string ORIGINAL_FILE_NAME { get; set; }
        public string FILE_EXTENSION { get; set; }
    }
}
