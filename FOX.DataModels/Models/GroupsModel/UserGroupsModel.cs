using FOX.DataModels.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.GroupsModel
{
    [Table("FOX_TBL_GROUP")]
    public class GROUP : SmartSearch
    {
        [Key]
        public long GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_USER_GROUP")]
    public class USERS_GROUP
    {
        [Key]
        public long GROUP_USER_ID { get; set; }
        public long? GROUP_ID { get; set; }
        public string USER_NAME { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class UserWithRoles
    {
        public long GROUP_USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public long USER_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public long ROLE_ID { get; set; }
        public long? GROUP_ID { get; set; }
        public bool? DELETED { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        
    }
    public class GroupUsersCreateViewModel
    {
        public UserWithRoles[] USERS { get; set; }
    }
    public class UsersAndGroupUsersViewModel
    {
        public List<UserWithRoles> AllUsers { get; set; }
        public List<UserWithRoles> GroupUsers { get; set; }
    }
}
