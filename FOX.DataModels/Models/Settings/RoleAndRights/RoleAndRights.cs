using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.Security;

namespace FOX.DataModels.Models.Settings.RoleAndRights
{
    public class RoleAndRights
    {
        public long ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public string RIGHT_TYPE_NAME { get; set; }
        public long RIGHT_ID { get; set; }
        public long RIGHTS_OF_ROLE_ID { get; set; }
        public string RIGHT_NAME { get; set; }
        public bool IS_CHECKED { get; set; }
        public int? OrderId { get; set; }
    }

    public class AuthViewModel
    {
        public bool IsAuthorized { get; set; }
        public UserProfile Profile { get; set; }
        public string Token { get; set; }
        public string TokenExpiry { get; set; }
        public List<RoleAndRights> UserRights { get; set; }
    }

    public class right
    {
        public long RIGHT_ID { get; set; }
        public string RIGHT_NAME { get; set; }
        public string RIGHT_TYPE_NAME { get; set; }
        public List<Role> Roles { get; set; }
        public int? OrderId { get; set; }

    }

    public class Role : BaseModel
    {
        public long ROLE_ID { get; set; }
        public long RIGHT_ID { get; set; }
        public long RIGHTS_OF_ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public bool IS_CHECKED { get; set; }
    }

    public class FOX_TBL_PRACTICE_ROLE_RIGHTS
    {
        [Key, Column(Order = 0)]
        public long RIGHTS_OF_ROLE_ID { get; set; }
        [Key, Column(Order = 1)]
        public long PRACTICE_CODE { get; set; }
        public bool CHECKED { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class FOX_TBL_RIGHTS_OF_ROLE
    {
        [Key]
        public long RIGHTS_OF_ROLE_ID { get; set; }
        public long RIGHT_ID { get; set; }
        public long ROLE_ID { get; set; }
        //public string ROLE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class PasswordChangeRequest : BaseModel
    {
        public long User_id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }
    }
    [Table("FOX_TBL_ROLE")]
    public class RoleToAdd : BaseModel
    {
        [Key]
        public long ROLE_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string ROLE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }

    }
    public class EmailExist
    {
        public string EMAIL { get; set; }
    }
    [Table("FOX_TBL_USER_RIGHTS")]
    public class FOX_TBL_USER_RIGHTS
    {
        [Key]
        public long RIGHT_ID { get; set; }
        public string RIGHT_NAME { get; set; }
        public long? RIGHT_TYPE_ID { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public int? DISPLAY_ORDER { get; set; }

    }

    [Table("FOX_TBL_USER_RIGHTS_TYPE")]
    public class FOX_TBL_USER_RIGHTS_TYPE
    {
        [Key]
        public long RIGHT_TYPE_ID { get; set; }
        public string RIGHT_TYPE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class TalkRehabDisabledModules
    {
        public long TalkrehabModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool IsShow { get; set; }

    }
}