using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Security
{
    [Table("FOX_TBL_OTP_ENABLE_DATE")]
    public class FOX_TBL_OTP_ENABLE_DATE
    {
        [Key]
        public long FOX_OTP_ENABLE_ID { get; set; }
        public long USER_ID { get; set; }
        public DateTime OTP_ENABLE_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
}
