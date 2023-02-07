using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.Security
{
    [Table("FOX_TBL_OTPENABLEDATE")]
    public class OtpEnableDate
    {
        [Key]
        public long OtpEnableId { get; set; }
        public long UserId { get; set; }
        public DateTime EnableOtpDate { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
}
