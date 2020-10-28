using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.WorkOrderHistoryModel
{
    [Table("FOX_TBL_WORK_ORDER_HISTORY")]
    public class WorkOrderHistory
    {
        [Key]
        public long LOG_ID { get; set; }
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string LOG_MESSAGE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_BY_In_Title_Case
        {
            get
            {
                System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                CREATED_BY = strConverter.ToTitleCase(CREATED_BY);
                return CREATED_BY;
            }
        }
        public DateTime CREATED_ON { get; set; }
    }
}