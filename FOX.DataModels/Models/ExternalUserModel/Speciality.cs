using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    [Table("FOX_TBL_SPECIALITY")]
    public class Speciality
    {
        [Key]
        public long SPECIALITY_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string NAME { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public bool DELETED { get; set; }
    }
}