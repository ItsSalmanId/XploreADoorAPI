using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Security
{
    [Table("FOX_TBL_PASSWORD_HISTORY")]
    public class PasswordHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PASSWORD_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public long USER_ID { get; set; }
        public string PASSWORD { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("WS_TBL_FOX_Login_LOGS")]
    public class WS_TBL_FOX_Login_LOGS
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LogId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceInfo { get; set; }
        public string AdResponse { get; set; }
        public string ServiceResponse { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string AdLoginResponse { get; set; }


    }
}