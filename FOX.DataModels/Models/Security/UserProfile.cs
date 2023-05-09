using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Security
{
    public class UserProfile
    {
        public long userID { get; set; }
        public string UserName { get; set; }
        public long PracticeCode { get; set; }
        public string PracticeName { get; set; }
        public string PracticeAddress { get; set; }
        public string PracticeAddressLine2 { get; set; }
        public string PracCity { get; set; }
        public string PracState { get; set; }
        public string PracZip { get; set; }
        public long RoleId { get; set; }
        public string PracPhone { get; set; }
        public string PracEmailAddress { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
        public long? MANAGER_ID { get; set; }
        public string EXTENSION { get; set; }
        public bool? IS_ACTIVE_EXTENSION { get; set; }
        public string PracticeDocumentDirectory { get; set; }
        public string PracticeDocumentServerIP { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public string SIGNATURE_PATH { get; set; }
        public string ROLE_NAME { get; set; }
        public string SENDER_TYPE { get; set; }
        public string EMAIL { get; set; }
        public List<RoleAndRights> ApplicationUserRoles { get; set; }
        public bool isTalkRehab { set; get; }
        public bool MFA { set; get; }
        public bool IS_AD_USER { set; get; }
        public int showMfaEanbleScreen { set; get; }

    }

    public class ResetPasswordViewModel
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }
        public string Ticks { get; set; }
        public string Message { get; set; }
        public string PasswordHash { get; set; }
    }

    public class ReqVerifyWorkOrder
    {
        public string Value { get; set; }
    }

    public class SubmitSignatureImageWithData
    {
        public string base64textString { get; set; }
        public string workId { get; set; }
        public bool _isSignaturePresent { get; set; }
        public string comments { get; set; }
        public bool _approval { get; set; }
    }
    public class GetUserIP
    {
        public string userIP { get; set; }
        public string userName { get; set; }

    }
}