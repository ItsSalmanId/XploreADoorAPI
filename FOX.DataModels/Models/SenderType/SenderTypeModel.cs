using FOX.DataModels.Models.SenderName;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.SenderType
{
    [Table("FOX_TBL_SENDER_TYPE")]
    public class FOX_TBL_SENDER_TYPE
    {
        [Key]
        public long FOX_TBL_SENDER_TYPE_ID { get; set; }
        public string SENDER_TYPE_NAME { get; set; }
        public string SENDER_TYPE_DESCRIPTION { get; set; }
        public long PRACTICE_CODE { get; set; }
        public int? DISPLAY_ORDER { get; set; }
        public bool? IsInternal { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<DateTime> MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        //public ICollection<FOX_TBL_SENDER_NAME> FOX_TBL_SENDER_NAMEs { get; set; }
    }
}