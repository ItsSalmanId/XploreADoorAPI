using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Exceptions
{
    //[Table("FOX_TBL_EXCEPTION_LOG")]
    public class FOX_TBL_EXCEPTION_LOG
    {
        //[Key]
        public long EXCEPTION_LOG_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string SHORT_MSG { get; set; }
        public string LONG_MSG { get; set; }
        public System.DateTime LOG_DATE { get; set; }
        public string LOG_BY { get; set; }
    }
}