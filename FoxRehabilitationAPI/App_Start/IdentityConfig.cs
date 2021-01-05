using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using FoxRehabilitationAPI.Models;
using System.Linq;
using FOX.BusinessOperations.SettingsService.UserMangementService;
using System.Data.Entity;
using FOX.DataModels.Models.Security;
using System;
using FOX.BusinessOperations.Security;
using FoxRehabilitationAPI.App_Start;
using static FOX.BusinessOperations.CommonServices.AppConfiguration.ActiveDirectoryViewModel;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.LDAPUser;
using FOX.DataModels.Context;
using System.IO;
using System.Web;
using FOX.BusinessOperations.AccountService;
using FoxRehabilitationAPI.Controllers;
using System.Collections;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace FoxRehabilitationAPI
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
        }

        //LDAPUser GetUserFromAD(string userName, string password)
        //{
        //    LDAPClient _LDAPClient = new LDAPClient();
        //    LDAPUser aduser = _LDAPClient.GetADUser(DomainURL, userName.Split('@')[0], password);
        //    return aduser;
        //}


        public async Task<Tuple<ApplicationUser, UserProfile>> FindProfileAsync(string userName, string password)
        {
            ApplicationUser user = new ApplicationUser();
            UserProfile userProfile = new UserProfile();
            UserManagementService userService = new UserManagementService();
            AccountController accountController = new AccountController();
            AccountServices accountServices = new AccountServices();
            ApplicationUserManager applicationUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ADDetail _ADDetail = null;

            userName = userName.Trim();
            if (userName.Contains("@"))
            {
                _ADDetail = ADDetailList.FirstOrDefault(t => t.DomainForSearch.Equals(userName.Split('@')[1].ToLower()));
            }

            #region New Logic

            //using (ApplicationDbContext dbContext = new ApplicationDbContext())
            //{
            //    //Get User from Database

            //    //Validate User from Active Directory
            //    LDAPUser adUser = GetUserFromAD(userName, password);
            //    //User Doesnot exists in Active Directory
            //    if (adUser == null)
            //    {
            //        //check user from database 
            //        //user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower())).FirstOrDefaultAsync();
            //        userProfile = userService.GetUserProfileByName(userName);
            //        user = await base.FindAsync(userProfile.UserName, password);
            //        if (user != null)
            //        {
            //            //userProfile = userService.GetUserProfileByName(userName);
            //            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
            //            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
            //        }
            //    }
            //    else
            //    {
            //        //Check from database that user exists 
            //        user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower())).FirstOrDefaultAsync();
            //        //user doesnot exists in database
            //        if (user == null)
            //        {
            //            //Add User To database
            //            user = new ApplicationUser();
            //            user.FIRST_NAME = adUser.FirstName;
            //            user.LAST_NAME = adUser.LastName;
            //            user.USER_NAME = adUser.FullName;
            //            user.EMAIL = adUser.Email;
            //            user.USER_ID = Helper.getMaximumId("USER_ID");
            //            user.CREATED_BY = user.MODIFIED_BY = "FOX Team";
            //            user.CREATED_DATE = user.MODIFIED_DATE = Helper.GetCurrentDate();
            //            user.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
            //            user.ROLE_ID = RoleId;
            //            user.IS_ACTIVE = true;
            //            user.IS_APPROVED = true;
            //            user.USER_TYPE = "Internal";
            //            FOX.DataModels.Context.DbContextCommon _DbContextCommon = new FOX.DataModels.Context.DbContextCommon();
            //            var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => t.SENDER_TYPE_NAME.ToLower().Contains("Fox Account Manager".ToLower()));
            //            if (_FOX_TBL_SENDER_TYPE != null)
            //            {
            //                user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
            //            }
            //            user.PASSWORD = Encrypt.getEncryptedCode(password);
            //            IdentityResult result = await applicationUserManager.CreateAsync(user, password);
            //            if (result.Succeeded)
            //            {
            //                userService.SavePasswordHisotry(user, user.PRACTICE_CODE);
            //                userService.SaveSenderName(user, user.PRACTICE_CODE);
            //            }
            //            userProfile = userService.GetUserProfileByName(userName);
            //            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
            //            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
            //        }
            //        else //user exists in database than return that user and if required update it's password in database
            //        {
            //            userProfile = userService.GetUserProfileByName(userName);
            //            user.USER_NAME = userProfile.UserName;
            //            user.PasswordHash = base.PasswordHasher.HashPassword(password);
            //            userService.UpdateADUserPassword(Encrypt.getEncryptedCode(password), user.PasswordHash, userProfile);
            //            //await dbContext.SaveChangesAsync();
            //            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
            //            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);

            //        }

            //    }

            //}

            #endregion New Logic

            #region  Old Code

            using (var dbContext = new ApplicationDbContext())
            {
                if (_ADDetail != null)
                {
                    string serverName = System.Web.HttpContext.Current.Request.Url.Host;
                    try
                    {
                        LDAPClient _LDAPClient = new LDAPClient();
                        //sattar code 04/04/2019 start
                        DbContextSecurity _DbContextSecurity = new DbContextSecurity();
                        //sattar code 04/04/2019 end
                        //validate from Active Directory that user exists in Active directory 
                        LDAPUser aduser = _LDAPClient.GetADUser(_ADDetail.DomainURL, userName, userName.Split('@')[0], password);
                        if (aduser != null)  //User exists in active directory
                        {
                            //check from fox database that user exists 
                            user = await dbContext.Users.Where(u => u.Email.ToLower() == userName.ToLower() && !u.DELETED).FirstOrDefaultAsync();
                            //if user exists in active diretory but doesnot exists in database
                            //Add User in database                       

                            if (user == null)
                            {
                                user = new ApplicationUser();
                                user.FIRST_NAME = aduser.FirstName;
                                user.LAST_NAME = string.IsNullOrEmpty(aduser.LastName) ? "" : aduser.LastName;
                                //user.Email = aduser.Email;
                                user.Email = userName;
                                user.DESIGNATION = aduser.Division;
                                user.USER_ID = Helper.getMaximumId("USER_ID");
                                user.USER_NAME = $"{user.LAST_NAME.Replace(" ", string.Empty)}_{user.USER_ID}";
                                user.CREATED_BY = user.MODIFIED_BY = "FOX_AD";
                                user.CREATED_DATE = user.MODIFIED_DATE = Helper.GetCurrentDate();
                                user.PRACTICE_CODE = _ADDetail.PracticeCode;
                                user.ROLE_ID = userService.GetADRole(_ADDetail.PracticeCode, aduser.Title, _ADDetail.RoleId);
                                user.IS_ACTIVE = true;
                                user.IS_APPROVED = true;
                                user.USER_TYPE = "Internal_User";
                                user.PASSWORD = Encrypt.getEncryptedCode(password);
                                user.IS_AD_USER = true;
                                user.RT_USER_ID = aduser.employeeNumber;
                                user.FULL_ACCESS_OVER_APP = true;
                                DbContextCommon _DbContextCommon = new DbContextCommon();
                                var _FOX_TBL_ROLE = _DbContextCommon.GetUserRoles.FirstOrDefault(t => !t.DELETED && (t.PRACTICE_CODE == _ADDetail.PracticeCode || t.PRACTICE_CODE == null) && t.ROLE_ID == user.ROLE_ID);
                                //var _FOX_TBL_ROLE = _DbContextCommon.GetUserRoles.FirstOrDefault(t => !t.DELETED && t.PRACTICE_CODE == _ADDetail.PracticeCode && t.ROLE_ID == user.ROLE_ID);

                                if(_FOX_TBL_ROLE != null)
                                {
                                    user.ROLE_NAME = _FOX_TBL_ROLE.ROLE_NAME;
                                }
                                if(!string.IsNullOrEmpty(user.ROLE_NAME) && (user.ROLE_NAME.ToLower().Contains("administrator") || user.ROLE_NAME.ToLower().Contains("supervisor") || user.ROLE_NAME.ToLower().Contains("director revenue") || user.ROLE_NAME.ToLower().Contains("adjustment poster") || user.ROLE_NAME.ToLower().Contains("lead fox survey") || user.ROLE_NAME.ToLower().Contains("auditor") || user.ROLE_NAME.ToLower().Contains("coordinator") || user.ROLE_NAME.ToLower().Contains("view indexed queue") || user.ROLE_NAME.ToLower().Contains("feedback caller")))
                                {

                                    var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => !t.DELETED && t.PRACTICE_CODE == _ADDetail.PracticeCode && t.SENDER_TYPE_NAME.ToLower().Contains("fox admin".ToLower()));

                                    if (_FOX_TBL_SENDER_TYPE != null)
                                    {
                                        user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(user.ROLE_NAME) && (user.ROLE_NAME.ToLower().Contains("ceo founder") || user.ROLE_NAME.ToLower().Contains("executive") || user.ROLE_NAME.ToLower().Contains("senior regional director") || (user.ROLE_NAME.ToLower().Contains("regional qa liaison")) || user.ROLE_NAME.ToLower().Contains("vice president") || user.ROLE_NAME.ToLower().Contains("fox optimal living director") || user.ROLE_NAME.ToLower().Contains("clinician")) )
                                {
                                    var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => !t.DELETED && t.PRACTICE_CODE == _ADDetail.PracticeCode && t.SENDER_TYPE_NAME.ToLower().Contains("fox clinician".ToLower()));

                                    if (_FOX_TBL_SENDER_TYPE != null)
                                    {
                                        user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(user.ROLE_NAME) && (user.ROLE_NAME.ToLower().Contains("area sales director") || user.ROLE_NAME.ToLower().Contains("indexer") || user.ROLE_NAME.ToLower().Contains("agent") || user.ROLE_NAME.ToLower().Contains("account manager")))
                                {
                                    var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => !t.DELETED && t.PRACTICE_CODE == _ADDetail.PracticeCode && t.SENDER_TYPE_NAME.ToLower().Contains("fox account manager".ToLower()));

                                    if (_FOX_TBL_SENDER_TYPE != null)
                                    {
                                        user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(user.ROLE_NAME) && user.ROLE_NAME.ToLower().Contains("regional director"))
                                {

                                    var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => !t.DELETED && t.PRACTICE_CODE == _ADDetail.PracticeCode && t.SENDER_TYPE_NAME.ToLower().Contains("fox regional director".ToLower()));

                                    if (_FOX_TBL_SENDER_TYPE != null)
                                    {
                                        user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
                                    }
                                }
                                else
                                {
                                    var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => !t.DELETED && t.PRACTICE_CODE == _ADDetail.PracticeCode && t.SENDER_TYPE_NAME.ToLower().Contains("fox account manager".ToLower()));

                                    if (_FOX_TBL_SENDER_TYPE != null)
                                    {
                                        user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
                                    }
                                }
                                //sattar code 04/04/2019 start
                                ReferralRegion singleregiondata = new ReferralRegion();
                                if (!string.IsNullOrEmpty(aduser.ExtensionAttribute))
                                {
                                    singleregiondata = _DbContextSecurity.ReferralRegions.FirstOrDefault(u => u.REFERRAL_REGION_NAME.ToLower() == aduser.ExtensionAttribute.ToLower() && u.PRACTICE_CODE == _ADDetail.PracticeCode && !u.DELETED);
                                    user.REFERRAL_REGION_ID = singleregiondata?.REFERRAL_REGION_ID;
                                }
                                //sattar code 04/04/2019 end
                                var dbUser = _DbContextCommon.User.FirstOrDefault(u => u.EMAIL.ToLower() == userName.ToLower() && !u.DELETED);

                                if (dbUser == null)
                                {
                                    IdentityResult result = await applicationUserManager.CreateAsync(user, password);

                                    if (result.Succeeded)
                                    {
                                        userService.SavePasswordHisotry(user, user.PRACTICE_CODE);
                                        userService.SaveSenderName(user, user.PRACTICE_CODE);
                                    }
                                }
                            }
                            //sattar code 04/04/2019 start
                            ReferralRegion SingleRegionData = new ReferralRegion();
                                if (!string.IsNullOrEmpty(aduser.ExtensionAttribute))
                                {
                                    SingleRegionData = _DbContextSecurity.ReferralRegions.FirstOrDefault(u => u.REFERRAL_REGION_NAME.ToLower() == aduser.ExtensionAttribute.ToLower() && u.PRACTICE_CODE == _ADDetail.PracticeCode && !u.DELETED);
                                if (user?.REFERRAL_REGION_ID == null)
                                {
                                    user.REFERRAL_REGION_ID = SingleRegionData?.REFERRAL_REGION_ID;
                                }
                                }
                                //sattar code 04/04/2019 end
                                //sattar code 04/04/2019 start
                                if (!string.IsNullOrEmpty(aduser.ExtensionAttribute))
                                {
                                    if (SingleRegionData?.REFERRAL_REGION_ID > 0)
                                    {
                                        if (user.ROLE_ID == userService.GetRoleId("regional director", user.PRACTICE_CODE))
                                        {
                                            userService.SaveRegionalDirectorID(user, SingleRegionData);
                                        }
                                        if (user.ROLE_ID == userService.GetRoleId("account manager", user.PRACTICE_CODE))
                                        {
                                            userService.SaveAccountManagerID(user, SingleRegionData);
                                        }
                                    }
                                }
                                //sattar code 04/04/2019 end
                            userProfile = userService.GetUserProfileByName(user.Email);
                            user.USER_NAME = userProfile.UserName;
                            user.RT_USER_ID = aduser.employeeNumber;
                            //user.IS_AD_USER = true;
                            //Set AD password into DB
                            //We will uncomment this peace of caode after client review
                            user.PASSWORD = Encrypt.getEncryptedCode(password);
                            user.PasswordHash = base.PasswordHasher.HashPassword(password);
                            userService.UpdateADUserPassword(Encrypt.getEncryptedCode(password), user.PasswordHash, userProfile);

                            await dbContext.SaveChangesAsync();
                            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
                            string encryptedPassword = Encrypt.EncryptPassword(password);
                            string detectedBrowser = AccountController.BrowserDetection();
                            accountServices.InsertLogs(user, encryptedPassword, detectedBrowser, "Valid Internal Login");
                            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
                        }
                        else
                        {
                            //userProfile = userService.GetUserProfileByName(user.Email);
                            DbContextCommon _DbContextCommon = new DbContextCommon();
                            var dbUser = _DbContextCommon.User.FirstOrDefault(u => u.EMAIL.ToLower() == userName.ToLower() && !u.DELETED);
                            if(dbUser != null)
                            {
                                string encryptedPassword = Encrypt.EncryptPassword(password);
                                string detectedBrowser = AccountController.BrowserDetection();
                                accountServices.InsertLogs(dbUser, encryptedPassword, detectedBrowser, "Invalid Internal Login");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<ApplicationUser, UserProfile>(null, null);
                    }
                    //else // user not validated from active directory
                    //{
                    //    //TODO : Implement mechanism that user is coming from Active directory but has not valid user name and password
                    //    return new Tuple<ApplicationUser, UserProfile>(null, null);
                    //}
                }
                else
                {
                    userProfile = userService.GetUserProfileByName(userName);
                    if (userProfile != null)
                    {
                        //getting user by username and password hash
                        user = await base.FindAsync(userProfile.UserName, password);
                        if (user == null)
                        {
                            string encryptedPass = Encrypt.getEncryptedCode(password);
                            //getting user by username and old password
                            user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower()) && (u.PASSWORD == encryptedPass)).FirstOrDefaultAsync();
                            if (user != null)
                            {
                                //user.PASSWORD = string.Empty;
                                user.USER_NAME = userProfile.UserName;
                                user.PasswordHash = base.PasswordHasher.HashPassword(password);
                                await dbContext.SaveChangesAsync();
                                user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
                                //Creating Logs for Successfull Login
                                string encryptedPassword = Encrypt.EncryptPassword(password);
                                string detectedBrowser = AccountController.BrowserDetection();
                                accountServices.InsertLogs(userProfile, encryptedPassword, detectedBrowser, "Login");

                                return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
                            }
                            else
                            {
                                string encryptedPassword = Encrypt.EncryptPassword(password);
                                string detectedBrowser = AccountController.BrowserDetection();
                                accountServices.InsertLogs(userProfile, encryptedPassword, detectedBrowser, "Login");
                            }
                        }
                        else
                        {
                            //if (!string.IsNullOrEmpty(user.PASSWORD))
                            //{
                            //    user.PASSWORD = string.Empty;
                            //    user.USER_NAME = userProfile.UserName;
                            //    await base.UpdateAsync(user);
                            //}
                            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
                            string encryptedPassword = Encrypt.EncryptPassword(password);
                            string detectedBrowser = AccountController.BrowserDetection();
                            accountServices.InsertLogs(userProfile, encryptedPassword, detectedBrowser, "Login");
                            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
                        }
                    }
                }
            }

            #endregion Old Code
            return new Tuple<ApplicationUser, UserProfile>(null, null);
        }
        
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 8,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public bool CheckUserExistingLoginAttempts(string userName)
        {
            UserManagementService user = new UserManagementService();
            try
            {
                return user.CheckValidUserLoginAttempt(userName: userName);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return user.CheckValidUserLoginAttempt(userName: userName);
            }
        }

        public bool IsCheckedUserBlocked(string userName)
        {
            UserManagementService user = new UserManagementService();
            try
            {
                return user.IsUserBlocked(userName: userName);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return user.IsUserBlocked(userName: userName);
            }
        }

        public bool AddInvalidLoginAttempt(string userName)
        {
            UserManagementService user = new UserManagementService();
            try
            {
                return user.AddUserInvalidLoginAttempt(userName: userName);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return user.AddUserInvalidLoginAttempt(userName: userName);
            }
        }

        public bool AddValidLoginAttempt(string userName)
        {
            UserManagementService user = new UserManagementService();
            try
            {
                return user.AddUserValidLoginAttempt(userName: userName);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return user.AddUserValidLoginAttempt(userName: userName);
            }
        }
    }
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {

        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }
}
