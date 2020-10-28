using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FOX.DataModels.Models.LDAPUser
{
    public class LDAPUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string ExtensionAttribute { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }
        public string Division { get; set; }
        public string employeeNumber { get; set; }

    }
    public class GetADUserReqViewModel
    {
        public string Domain { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class GetADUserResViewModel
    {
        public string Message { get; set; }
        public bool IsSussess { get; set; }
        public Exception Exception { get; set; }
        public LDAPUser lDAPUser { get; set; }
    }

    [Table("FOX_TBL_AD_ROLE")]
    public class ActiveDirectoryRole
    {
        [Key]
        public int AD_ROLE_ID { get; set; }
        public Nullable<long> PRACTICE_CODE { get; set; }
        public string AD_ROLE_NAME { get; set; }
        public Nullable<long> ROLE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

    }
}
