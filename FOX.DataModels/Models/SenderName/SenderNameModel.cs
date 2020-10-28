using FOX.DataModels.Models.SenderType;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.SenderName
{
    [Table("FOX_TBL_SENDER_NAME")]
    public class FOX_TBL_SENDER_NAME
    {
        [Key]
        public long FOX_TBL_SENDER_NAME_ID { get; set; }
        [ForeignKey("FOX_TBL_SENDER_TYPE")]
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public FOX_TBL_SENDER_TYPE FOX_TBL_SENDER_TYPE { get; set; }
        public string SENDER_NAME_CODE { get; set; }
        public string SENDER_NAME_DESCRIPTION { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<DateTime> MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
}