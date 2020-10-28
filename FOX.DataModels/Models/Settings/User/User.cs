using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Settings.User
{

    public class UsersForDropdown
    {
        public UsersForDropdown()
        {


        }
        public UsersForDropdown(string _FIRST_NAME, string _LAST_NAME, long _UserID, string _ROLE_NAME, long _ROLE_ID)
        {
            UserID = _UserID;
            FIRST_NAME = _FIRST_NAME;
            LAST_NAME = _LAST_NAME;
            ROLE_NAME = _ROLE_NAME;
            ROLE_ID = _ROLE_ID;
        }
        public long UserID { get; set; }
        public string USER_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string ROLE_NAME { get; set; }
        public long ROLE_ID { get; set; }

    }
    public class PreviousPasswordCheck: BaseModel
    {
        public string password { get; set; }
        public long user_id { get; set; }
        public string PasswordHash { get; set; }
    }

    public class InterfcaeFailedPatient : BaseModel
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public long Patient_Account { get; set; }
        public string Chart_Id { get; set; }

    }

}