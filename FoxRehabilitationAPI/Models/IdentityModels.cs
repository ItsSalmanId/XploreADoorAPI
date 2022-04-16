using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System.Collections.Generic;
using FOX.BusinessOperations.CommonService;

namespace FoxRehabilitationAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string USER_NAME { get; set; }
        public override string UserName
        {
            get
            {
                return USER_NAME;
            }
            set
            {
                base.UserName = USER_NAME;
            }
        }
        [NotMapped]
        public string EMAIL { get { return base.Email; } set { base.Email = value; } }
        public override string PhoneNumber { get { return PHONE_NO; } set { PHONE_NO = value; } }
        public long USER_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string PASSWORD { get; set; }
        public int FAILED_PASSWORD_ATTEMPT_COUNT { get; set; }
        public DateTime? PASSWORD_CHANGED_DATE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string DESIGNATION { get; set; }
        public bool IS_ADMIN { get; set; }
        [NotMapped]
        public int STATUS { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string USER_DISPLAY_NAME { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public int? RESET_PASS { get; set; }
        public string SECURITY_QUESTION { get; set; }
        public string SECURITY_QUESTION_ANSWER { get; set; }
        public string LOCKEDBY { get; set; }
        public bool IS_LOCKED_OUT { get; set; }
        public DateTime? LAST_LOGIN_DATE { get; set; }
        public string COMMENTS { get; set; }
        public string PASS_RESET_CODE { get; set; }
        public string ACTIVATION_CODE { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string PHONE_NO { get; set; }
        public long? ROLE_ID { get; set; }
        public string ADDRESS_1 { get; set; }
        public string ADDRESS_2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long? MANAGER_ID { get; set; }
        [NotMapped]
        public string ROLE_NAME { get; set; }

        public string USER_TYPE { get; set; }
        [NotMapped]
        public string PRACTICE_NAME { get; set; }
        public string NPI { get; set; }
        public string MOBILE_PHONE { get; set; }
        [NotMapped]
        public bool? Is_Electronic_POC { get; set; }
        public string FAX { get; set; }
        public string FAX_2 { get; set; }
        public string FAX_3 { get; set; }
        public string GENDER { get; set; }
        public string LANGUAGE { get; set; }
        public string TIME_ZONE { get; set; }
        [NotMapped]
        public string CREATED_DATE_STR { get; set; }
        public DateTime? TERMINATION_DATE { get; set; }
        public string SIGNATURE_PATH { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public string PASSWORD_RESET_TICKS { get; set; }
        public string WORK_PHONE { get; set; }
        public long? NOTE_ID { get; set; }
        public long? ACO { get; set; }
        [NotMapped]
        public string ACO_NAME { get; set; }
        public long? SPECIALITY { get; set; }
        [NotMapped]
        public string SPECIALITY_NAME { get; set; }
        public long? SNF { get; set; }
        [NotMapped]
        public string SNF_NAME { get; set; }
        public long? HOSPITAL { get; set; }
        [NotMapped]
        public string HOSPITAL_NAME { get; set; }
        public long? HHH { get; set; }
        [NotMapped]
        public string HHH_NAME { get; set; }
        public string THIRD_PARTY_REFERRAL_SOURCE { get; set; }
        [NotMapped]
        public string COMMENT { get; set; }
        public long? PRACTICE_ORGANIZATION_ID { get; set; }
        public bool? IS_APPROVED { get; set; }
        [NotMapped]
        public bool hasToSendEmail { get; set; }
        public string EXTENSION { get; set; }
        public string PRACTICE_ORGANIZATION_TEXT { get; set; }
        public string ACO_TEXT { get; set; }
        public string SPECIALITY_TEXT { get; set; }
        public string SNF_TEXT { get; set; }
        public string HOSPITAL_TEXT { get; set; }
        public string HHH_TEXT { get; set; }
        public bool? IS_AD_USER { get; set; }
        public long? REFERRAL_REGION_ID { get; set; }

        [NotMapped]
        public List<RoleAndRights> ApplicationUserRoles { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType, UserProfile userProfile = null)
        {
            if(userProfile?.isTalkRehab == false)
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
                try
                {
                    // Add custom user claims here
                    if (userProfile != null)
                    {
                        userIdentity.AddClaim(new Claim("UserProfile", Newtonsoft.Json.JsonConvert.SerializeObject(userProfile)));
                    }
                    return userIdentity;
                }
                catch (Exception ex)
                {
                    return userIdentity;
                }
            }
            else
            {
                ApplicationUserManager applicationUserManagerTalkRehab = new ApplicationUserManager(new UserStore<ApplicationUser>(new TalkRehabDBContext()));
                var userIdentity = await applicationUserManagerTalkRehab.CreateIdentityAsync(this, authenticationType);
                try
                {
                    // Add custom user claims here
                    if (userProfile != null)
                    {
                        userIdentity.AddClaim(new Claim("UserProfile", Newtonsoft.Json.JsonConvert.SerializeObject(userProfile)));
                    }
                    return userIdentity;
                }
                catch (Exception ex)
                {
                    return userIdentity;
                }
            }
        }
        public string RT_USER_ID { get; set; }
        public bool? FULL_ACCESS_OVER_APP { get; set; }
        [NotMapped]
        public string SENDER_TYPE_NAME { get; set; }
        [NotMapped]
        public bool? Is_Blocked { get; set; }
    }

    public class ClaimsModel
    {
        public static UserProfile GetUserProfile(ClaimsIdentity identity)
        {
            var profileString = identity.Claims.Where(c => c.Type == "UserProfile").Select(s => s.Value).FirstOrDefault();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfile>(profileString);
        }

    }
}