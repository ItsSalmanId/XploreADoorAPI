using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    [Table("Zip_City_State")]
    public class Zip_City_State
    {
        [Key]
        public string ZIP_Code { get; set; }
        public string City_Name { get; set; }
        public string State_Code { get; set; }
        public bool Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        //public Guid rowguid { get; set; }
        public string Time_Zone { get; set; }
    }
}